using AWAPI_BusinessLibrary.library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AWAPI_Data.Data;
using System.Collections.Generic;

namespace Awapi_Test
{
    
    
    /// <summary>
    ///This is a test class for ContentLibraryTest and is intended
    ///to contain all ContentLibraryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContentLibraryTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetList
        ///</summary>
        [TestMethod()]
        public void GetListTest1()
        {
            ContentLibrary target = new ContentLibrary(); // TODO: Initialize to an appropriate value
            long siteId = 7512913955264234783; // TODO: Initialize to an appropriate value
            string @where = string.Empty; // TODO: Initialize to an appropriate value
            string sortField = string.Empty; // TODO: Initialize to an appropriate value
            IList<awContent> expected = null; // TODO: Initialize to an appropriate value
            IList<awContent> actual;
            actual = target.GetList(siteId, @where, sortField);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetList
        ///</summary>
        [TestMethod()]
        public void GetListTest()
        {
            ContentLibrary target = new ContentLibrary(); // TODO: Initialize to an appropriate value
            bool onlyAvailables = false; // TODO: Initialize to an appropriate value
            long siteId = 7512913955264234783; // TODO: Initialize to an appropriate value
            string @where = string.Empty; // TODO: Initialize to an appropriate value
            string sortField = string.Empty; // TODO: Initialize to an appropriate value
            string cultureCode = string.Empty; // TODO: Initialize to an appropriate value
            IList<awContent> expected = null; // TODO: Initialize to an appropriate value
            IList<awContent> actual;
            actual = target.GetList(onlyAvailables, siteId, @where, sortField, cultureCode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }


    }
}
