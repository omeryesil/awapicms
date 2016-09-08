using System;
using System.IO;
using System.Security.Cryptography;
using System.Reflection;

namespace AWAPI_Common.library
{
    /// <summary>
    ///   A System.IO.Stream that performs Base64 encoding and decoding.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Base64Stream is a decorator on another Stream.  It performs
    ///     base64 encoding or decoding as data is written to, or read
    ///     from, the wrapped stream.
    ///   </para>
    ///   <para>
    ///     The Base64Stream can be used in any combination of reading or writing,
    ///     and encoding or decoding.
    ///   </para>
    /// </remarks>
    public class Base64Stream : System.IO.Stream
    {
        /// <summary>
        ///   The mode - whether to encode or decode.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            ///   The Base64Stream will encode - produce Base64 on output.
            /// </summary>
            Encode,

            /// <summary>
            ///   The Base64Stream will decode - produce plaintext from a  Base64 input.
            /// </summary>
            Decode
        }

        private bool _isClosed;
        private Mode _mode;
        private int _totalWritten;
        private int _totalFlushed;
        private Stream _captiveStream;
        private MemoryStream _buffer;
        private CryptoStream _innerStream;
        private bool _disposed;
        private int _outputLineLength;
        private static readonly int _BUFFER_THRESHOLD = 1024;

        /// <summary>
        ///   Creates a Base64Stream, wrapped around an exising Stream
        /// </summary>
        /// <remarks>
        ///   <para>
        ///   </para>
        /// </remarks>
        /// <example>
        ///   <para>
        ///     This example uses a Base64Stream in concert with a DeflateStream
        ///     to first compress, then base64 encode, the contents of a file.
        ///     The result is stored in a MemoryStream.
        ///   </para>
        ///   <code>
        ///    byte[] working= new byte[1024];
        ///    int n;
        ///    using (Stream input = File.OpenRead(fileToCompress))
        ///    {
        ///        using (Stream output = new MemoryStream())
        ///        {
        ///            using (var b64 = new Base64Stream(output, Base64Stream.Mode.Encode))
        ///            {
        ///                b64.Rfc2045Compliant = true; // OutputLineLength = 76;
        ///
        ///                using (var compressor = new DeflateStream(b64, CompressionMode.Compress, true))
        ///                {
        ///                    while ((n = input.Read(working, 0, working.Length)) != 0)
        ///                    {
        ///                        compressor.Write(working, 0, n);
        ///                    }
        ///                }
        ///            }
        ///            output.Position = 0;
        ///            // use the MemoryStream here ...
        ///        }
        ///    }
        ///   </code>
        /// </example>
        public Base64Stream(System.IO.Stream stream, Mode mode)
        {
            _captiveStream = stream;
            _mode = mode;
        }


        /// <summary>
        ///   Get or set the line length used when writing through the Base64Stream.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     IETF RFC2045, covering MIME attachments, states that Base64
        ///     attachments should have lines of no more than 76 chars each.  On
        ///     the other hand, some other applications may not necessarily need
        ///     to confine their base64 streams to that line length.  This property
        ///     allows users of Base64Stream to set the line length, used in
        ///     output when encoding.
        ///   </para>
        ///   <para>
        ///     Setting this property to zero implies that the output will not be
        ///     segmented into lines.
        ///   </para>
        ///   <para>
        ///     This property has no effect on a Base64Stream used for decoding.
        ///   </para>
        /// </remarks>
        /// <seealso cref="Rfc2045Compliant"/>
        public int OutputLineLength
        {
            get
            {
                return _outputLineLength;
            }
            set
            {
                if (_innerStream != null) throw new ApplicationException("Base64Stream");
                _outputLineLength = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether RFC-2045 compliance is in effect.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     IETF RFC2045, covering MIME attachments, states that Base64
        ///     attachments should have lines of no more than 76 chars each.  On
        ///     the other hand, some other applications may not necessarily need
        ///     to confine their base64 streams to that line length.  This
        ///     property allows users of Base64Stream to get or set whether
        ///     RFC2045 compliance should be used.
        ///   </para>
        ///   <para>
        ///     Setting this property to false is equivalent to setting <see
        ///     cref="OutputLineLength"/> to zero.
        ///   </para>
        ///   <para>
        ///     This property has no effect on a Base64Stream used for decoding.
        ///   </para>
        /// </remarks>
        /// <seealso cref="OutputLineLength"/>
        public bool Rfc2045Compliant
        {
            get
            {
                return (_outputLineLength == 76);

            }
            set
            {
                if (_innerStream != null) throw new ApplicationException("Base64Stream");
                _outputLineLength = (value) ? 76 : 0;
            }
        }

        /// <summary>
        ///   The Dispose method on the type.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Users of the Base64Stream must call Dispose() when they are
        ///     finished using the instance. Typically this method is called
        ///     implicitly, by the end of a <c>using</c> scope (<c>Using</c> in
        ///     VB)
        ///   </para>
        /// </remarks>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!_disposed)
                {
                    if (disposing && (this._innerStream != null))
                    {
                        //this.Close();
                        _innerStream.Flush();
                        _innerStream.Close(); // final transform
                        _isClosed = true;
                        //this._innerStream= null;
                        HandleBufferedOutput(true);
                    }
                    _disposed = true;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }


        /// <summary>
        ///   Flushes any buffered output.
        /// </summary>
        public override void Flush()
        {
            if (_disposed) throw new ObjectDisposedException("Base64Stream");
            _innerStream.Flush();
            HandleBufferedOutput(true);
        }


        private void HandleBufferedOutput(bool flush)
        {
            // are we buffering?
            if (_outputLineLength > 0)
            {
                // is the buffer is full? or are we closed?
                if (_isClosed || (_buffer.Length > _BUFFER_THRESHOLD) || (flush && _buffer.Length > 0))
                {
                    // System.Text.Encoding e = System.Text.Encoding.Default;
                    byte[] b = _buffer.ToArray();
                    int i;
                    for (i = 0; i + _outputLineLength <= b.Length; i += _outputLineLength)
                    {
                        _captiveStream.Write(b, i, _outputLineLength);
                        _captiveStream.WriteByte(10); // 10==CR.
                        _captiveStream.WriteByte(13); // 13==LF.
                        _totalFlushed += _outputLineLength;
                    }

                    if (!_isClosed)
                        _buffer.SetLength(0);

                    if (flush)
                    {
                        _captiveStream.Write(b, i, b.Length - i);
                        _totalFlushed += b.Length - i;
                    }
                    else
                        _buffer.Write(b, i, b.Length - i);
                }
            }
        }


        /// <summary>
        ///   Read data through the stream.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_disposed) throw new ObjectDisposedException("Base64Stream");

            if (_innerStream == null)
            {
                ICryptoTransform transform = (_mode == Mode.Decode)
                    ? (ICryptoTransform)new FromBase64Transform()
                    : (ICryptoTransform)new ToBase64Transform();

                _innerStream = new CryptoStream(_captiveStream, transform, CryptoStreamMode.Read);
            }

            return _innerStream.Read(buffer, offset, count);
        }


        /// <summary>
        ///   Write data through the stream.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_disposed) throw new ObjectDisposedException("Base64Stream");

            if (_innerStream == null)
            {
                ICryptoTransform transform = (_mode == Mode.Decode)
                    ? (ICryptoTransform)new FromBase64Transform()
                    : (ICryptoTransform)new ToBase64Transform();

                Stream s = null;
                if (_outputLineLength > 0)
                {
                    _buffer = new MemoryStream();
                    s = _buffer;
                }
                else s = _captiveStream;

                _innerStream = new CryptoStream(s, transform, CryptoStreamMode.Write);
            }

            if (count == 0) return;

            _innerStream.Write(buffer, offset, count);
            _totalWritten += count;

            HandleBufferedOutput(false);
        }


        /// <summary>
        ///   Returns true if the Stream can be used to read.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("Base64Stream");
                if (_innerStream == null) throw new ApplicationException("Base64Stream");
                return _innerStream.CanRead;
            }
        }


        /// <summary>
        ///   Returns true if the Stream can be used to write.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("Base64Stream");
                return _innerStream.CanWrite;
            }
        }


        /// <summary>
        ///   Always returns false. The Base64Stream does not support seek operations.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("Base64Stream");
                return false;
            }
        }

        /// <summary>
        ///   Always throws a NotImplementedException.
        /// </summary>
        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Always throws a NotImplementedException.
        /// </summary>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Always throws a NotImplementedException.
        /// </summary>
        public override long Length
        {
            get { throw new NotImplementedException(); }
        }


        /// <summary>
        ///   Can be used to get the position on the stream.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///      Setting the Position always throws a NotImplementedException.
        ///   </para>
        /// </remarks>
        public override long Position
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("Base64Stream");
                if (_innerStream == null) throw new ApplicationException("Base64Stream");
                return _innerStream.Position;
            }
            set { throw new NotImplementedException(); }
        }
    }
}

