USE [Awapi]
GO
/****** Object:  Table [dbo].[awCulture]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awCulture](
	[cultureCode] [nvarchar](2) NOT NULL,
	[title] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_awLanguage] PRIMARY KEY CLUSTERED 
(
	[cultureCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_WebEvent_Events]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aspnet_WebEvent_Events](
	[EventId] [char](32) NOT NULL,
	[EventTimeUtc] [datetime] NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [nvarchar](256) NOT NULL,
	[EventSequence] [decimal](19, 0) NOT NULL,
	[EventOccurrence] [decimal](19, 0) NOT NULL,
	[EventCode] [int] NOT NULL,
	[EventDetailCode] [int] NOT NULL,
	[Message] [nvarchar](1024) NULL,
	[ApplicationPath] [nvarchar](256) NULL,
	[ApplicationVirtualPath] [nvarchar](256) NULL,
	[MachineName] [nvarchar](256) NOT NULL,
	[RequestUrl] [nvarchar](1024) NULL,
	[ExceptionType] [nvarchar](256) NULL,
	[Details] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[aspnet_SchemaVersions]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_SchemaVersions](
	[Feature] [nvarchar](128) NOT NULL,
	[CompatibleSchemaVersion] [nvarchar](128) NOT NULL,
	[IsCurrentVersion] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Feature] ASC,
	[CompatibleSchemaVersion] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_Applications]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Applications](
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awConfiguration]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[awConfiguration](
	[configurationId] [bigint] NOT NULL,
	[alias] [varchar](50) NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[description] [nvarchar](512) NULL,
	[fileServiceUrl] [varchar](512) NOT NULL,
	[contentServiceUrl] [varchar](512) NOT NULL,
	[blogServiceUrl] [varchar](512) NOT NULL,
	[pollServiceUrl] [varchar](512) NOT NULL,
	[automatedTaskServiceUrl] [varchar](512) NOT NULL,
	[twitterServiceUrl] [varchar](512) NOT NULL,
	[weatherServiceUrl] [varchar](512) NOT NULL,
	[adminBaseUrl] [varchar](512) NOT NULL,
	[fileDirectory] [varchar](255) NOT NULL,
	[fileMimeXMLPath] [varchar](255) NOT NULL,
	[fileSaveOnAmazonS3] [bit] NOT NULL,
	[fileAmazonS3BucketName] [varchar](50) NOT NULL,
	[fileAmazonS3AccessKey] [varchar](50) NOT NULL,
	[fileAmazonS3SecreyKey] [varchar](100) NOT NULL,
	[smtpServer] [varchar](20) NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awConfiguration] PRIMARY KEY CLUSTERED 
(
	[configurationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[awUser]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[awUser](
	[userId] [bigint] NOT NULL,
	[username] [nvarchar](50) NULL,
	[email] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[firstName] [nvarchar](50) NOT NULL,
	[lastName] [nvarchar](50) NOT NULL,
	[isSuperAdmin] [bit] NOT NULL,
	[isEnabled] [bit] NOT NULL,
	[isDeleted] [bit] NOT NULL,
	[imageurl] [nvarchar](255) NULL,
	[link] [nvarchar](255) NULL,
	[description] [nvarchar](max) NULL,
	[gender] [varchar](2) NULL,
	[birthday] [datetime] NULL,
	[tel] [varchar](50) NULL,
	[tel2] [varchar](50) NULL,
	[fax] [varchar](50) NULL,
	[address] [nvarchar](255) NULL,
	[city] [nvarchar](50) NULL,
	[state] [nvarchar](50) NULL,
	[postalcode] [nvarchar](20) NULL,
	[country] [nvarchar](50) NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awUser] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[awRole]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awRole](
	[roleId] [bigint] NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[description] [nvarchar](255) NULL,
	[module] [nvarchar](20) NOT NULL,
	[canRead] [bit] NOT NULL,
	[canUpdateStatus] [bit] NOT NULL,
	[canUpdate] [bit] NOT NULL,
	[canAdd] [bit] NOT NULL,
	[canDelete] [bit] NOT NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awRoleGroup] PRIMARY KEY CLUSTERED 
(
	[roleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awCultureValue]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awCultureValue](
	[cultureValueId] [bigint] NOT NULL,
	[cultureCode] [nvarchar](2) NOT NULL,
	[resourceRowId] [bigint] NOT NULL,
	[resourceTable] [nvarchar](50) NOT NULL,
	[resourceField] [nvarchar](50) NOT NULL,
	[resourceValue] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_awCultureValue] PRIMARY KEY CLUSTERED 
(
	[cultureValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awSite]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[awSite](
	[siteId] [bigint] NOT NULL,
	[alias] [varchar](100) NOT NULL,
	[title] [nvarchar](255) NULL,
	[description] [nvarchar](512) NULL,
	[isEnabled] [bit] NOT NULL,
	[cultureCode] [varchar](50) NULL,
	[link] [varchar](255) NULL,
	[imageurl] [varchar](255) NULL,
	[pubDate] [datetime] NULL,
	[maxBlogs] [int] NOT NULL,
	[maxUsers] [int] NOT NULL,
	[maxContents] [int] NOT NULL,
	[accessKey] [varchar](50) NULL,
	[grantedDomains] [varchar](512) NULL,
	[bannedDomains] [varchar](512) NULL,
	[twitterUsername] [varchar](50) NULL,
	[twitterPassword] [varchar](50) NULL,
	[userConfirmationEmailTemplateId] [bigint] NULL,
	[userResetPasswordEmailTemplateId] [bigint] NULL,
	[userId] [bigint] NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NULL,
 CONSTRAINT [PK_awSite] PRIMARY KEY CLUSTERED 
(
	[siteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[awAutomatedTask]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[awAutomatedTask](
	[automatedTaskId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[automatedTaskGroupId] [bigint] NOT NULL,
	[guidToRun] [uniqueidentifier] NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[description] [nvarchar](512) NULL,
	[isCompleted] [bit] NOT NULL,
	[deleteAfterComplete] [bit] NOT NULL,
	[completedMessage] [nvarchar](512) NULL,
	[errorMessage] [nvarchar](512) NULL,
	[namespaceAndClass] [varchar](100) NOT NULL,
	[methodName] [nvarchar](50) NOT NULL,
	[methodParameters] [nvarchar](1024) NULL,
	[resultRedirectUrl] [varchar](512) NULL,
	[lastBuildDate] [datetime] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awAutomatedTask] PRIMARY KEY CLUSTERED 
(
	[automatedTaskId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[awContentFormGroup]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContentFormGroup](
	[contentFormGroupId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[description] [nvarchar](512) NULL,
 CONSTRAINT [PK_contentFormGroup] PRIMARY KEY CLUSTERED 
(
	[contentFormGroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContent]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[awContent](
	[contentId] [bigint] NOT NULL,
	[alias] [varchar](255) NOT NULL,
	[title] [nvarchar](512) NOT NULL,
	[description] [nvarchar](max) NULL,
	[siteId] [bigint] NOT NULL,
	[contentType] [nvarchar](20) NOT NULL,
	[isEnabled] [bit] NOT NULL,
	[isCommentable] [bit] NOT NULL,
	[parentContentId] [bigint] NULL,
	[lineage] [nvarchar](512) NULL,
	[sortOrder] [int] NULL,
	[link] [nvarchar](512) NULL,
	[imageurl] [nvarchar](512) NULL,
	[userId] [bigint] NOT NULL,
	[pubDate] [datetime] NULL,
	[pubEndDate] [datetime] NULL,
	[eventStartDate] [datetime] NULL,
	[eventEndDate] [datetime] NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awContent] PRIMARY KEY CLUSTERED 
(
	[contentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[awContentComment]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContentComment](
	[contentCommentId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[contentId] [bigint] NOT NULL,
	[title] [nvarchar](255) NULL,
	[description] [nvarchar](1024) NULL,
	[isEnabled] [bit] NULL,
	[userId] [bigint] NULL,
	[email] [nvarchar](50) NULL,
	[createDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awPage]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awPage](
	[pageId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [nvarchar](255) NULL,
	[keywords] [nvarchar](255) NULL,
	[header] [nvarchar](1024) NULL,
	[pageContent] [nvarchar](max) NULL,
	[theme] [nvarchar](50) NULL,
	[isEnabled] [bit] NOT NULL,
	[isLoginRequired] [bit] NOT NULL,
	[outputCaching] [int] NULL,
	[pubDate] [datetime] NULL,
	[pubEndDate] [datetime] NULL,
	[userId] [bigint] NOT NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awPage] PRIMARY KEY CLUSTERED 
(
	[pageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awSiteMap]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awSiteMap](
	[siteMapId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[userId] [bigint] NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[description] [nvarchar](50) NOT NULL,
	[pageId] [bigint] NOT NULL,
	[url] [nvarchar](255) NULL,
	[isEnabled] [bit] NOT NULL,
	[isLoginRequired] [bit] NOT NULL,
	[lastBuildDate] [datetime] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awSiteMap] PRIMARY KEY CLUSTERED 
(
	[siteMapId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awEnvironment]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awEnvironment](
	[environmentId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[description] [nvarchar](512) NULL,
	[publicKey] [nvarchar](512) NULL,
	[serviceUrl] [nvarchar](512) NULL,
	[userId] [bigint] NULL,
	[lastBuildDate] [datetime] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awEnvironment] PRIMARY KEY CLUSTERED 
(
	[environmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContentForm]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContentForm](
	[contentFormId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[contentId] [bigint] NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[description] [nvarchar](512) NULL,
	[isEnabled] [bit] NOT NULL,
	[applyToSub] [bit] NOT NULL,
	[canCreateNew] [bit] NOT NULL,
	[canUpdate] [bit] NOT NULL,
	[canDelete] [bit] NOT NULL,
	[userId] [bigint] NULL,
	[lastBuildDate] [datetime] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_contentForm] PRIMARY KEY CLUSTERED 
(
	[contentFormId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awPoll]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awPoll](
	[pollId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[userId] [bigint] NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[description] [nvarchar](512) NULL,
	[answeredQuestion] [nvarchar](512) NULL,
	[isEnabled] [bit] NOT NULL,
	[isPublic] [bit] NOT NULL,
	[isMultipleChoice] [bit] NOT NULL,
	[pubDate] [datetime] NULL,
	[pubEndDate] [datetime] NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awPoll] PRIMARY KEY CLUSTERED 
(
	[pollId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awEmailTemplate]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awEmailTemplate](
	[emailTemplateId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[title] [nvarchar](255) NOT NULL,
	[description] [nvarchar](512) NULL,
	[emailFrom] [nvarchar](100) NULL,
	[emailSubject] [nvarchar](512) NULL,
	[emailBody] [nvarchar](max) NULL,
	[userId] [bigint] NULL,
	[lastBuildDate] [datetime] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awEmailTemplate] PRIMARY KEY CLUSTERED 
(
	[emailTemplateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awSiteDomainSecurity]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awSiteDomainSecurity](
	[siteDomainFilterId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[domainAddress] [nchar](50) NOT NULL,
	[allowRead] [bit] NOT NULL,
	[allowUpdate] [bit] NOT NULL,
	[allowCreate] [bit] NOT NULL,
	[allowDelete] [bit] NOT NULL,
	[userId] [bigint] NULL,
	[lastBuildDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awSiteIpSecurity] PRIMARY KEY CLUSTERED 
(
	[siteDomainFilterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awSiteCulture]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awSiteCulture](
	[siteCultureId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[cultureCode] [nvarchar](2) NOT NULL,
 CONSTRAINT [PK_siteLanguage] PRIMARY KEY CLUSTERED 
(
	[siteCultureId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awShareIt]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awShareIt](
	[shareItId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[title] [nvarchar](255) NOT NULL,
	[description] [nvarchar](512) NULL,
	[link] [nvarchar](512) NULL,
	[shareWithEveryone] [bit] NOT NULL,
	[userId] [bigint] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awShareIt] PRIMARY KEY CLUSTERED 
(
	[shareItId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awRoleMember]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awRoleMember](
	[roleMemberId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[roleId] [bigint] NOT NULL,
	[userId] [bigint] NOT NULL,
 CONSTRAINT [PK_awRoleGroupMember] PRIMARY KEY CLUSTERED 
(
	[roleMemberId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awFileGroup]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awFileGroup](
	[fileGroupId] [bigint] NOT NULL,
	[title] [nvarchar](50) NULL,
	[description] [nvarchar](255) NULL,
	[pubDate] [datetime] NULL,
	[lastBuildDate] [datetime] NULL,
	[userId] [bigint] NULL,
	[siteId] [bigint] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awFileGroup] PRIMARY KEY CLUSTERED 
(
	[fileGroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awSiteUser]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awSiteUser](
	[siteUserId] [bigint] NOT NULL,
	[siteId] [bigint] NULL,
	[userId] [bigint] NULL,
	[isEnabled] [bit] NOT NULL,
	[status] [int] NULL,
	[joinDate] [datetime] NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awSiteUser] PRIMARY KEY CLUSTERED 
(
	[siteUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awBlog]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[awBlog](
	[blogId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[userId] [bigint] NOT NULL,
	[alias] [varchar](100) NOT NULL,
	[title] [nvarchar](255) NOT NULL,
	[description] [nvarchar](512) NULL,
	[imageurl] [nvarchar](255) NULL,
	[blogPostPage] [varchar](100) NULL,
	[isEnabled] [bit] NOT NULL,
	[enableCommentEmailNotifier] [bit] NOT NULL,
	[commentEmailTo] [varchar](512) NULL,
	[commentEmailTemplateId] [bigint] NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awBlog] PRIMARY KEY CLUSTERED 
(
	[blogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[awSiteTag]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awSiteTag](
	[siteTagId] [bigint] NOT NULL,
	[title] [nvarchar](20) NOT NULL,
	[siteId] [bigint] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awTag] PRIMARY KEY CLUSTERED 
(
	[siteTagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContest]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[awContest](
	[contestId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[title] [nvarchar](255) NOT NULL,
	[description] [nvarchar](512) NULL,
	[isEnabled] [bit] NOT NULL,
	[maxEntry] [int] NOT NULL,
	[entryPerUser] [int] NOT NULL,
	[entryPerUserPeriodValue] [int] NOT NULL,
	[entryPerUserPeriodType] [char](2) NULL,
	[numberOfWinners] [int] NOT NULL,
	[pubDate] [datetime] NULL,
	[pubEndDate] [datetime] NULL,
	[userId] [bigint] NULL,
	[lastBuildDate] [datetime] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awContest] PRIMARY KEY CLUSTERED 
(
	[contestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1, 2, 3, .... This value will be combined with maxEntryPerUserPeriodType...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'awContest', @level2type=N'COLUMN',@level2name=N'entryPerUserPeriodValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'd, w, y' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'awContest', @level2type=N'COLUMN',@level2name=N'entryPerUserPeriodType'
GO
/****** Object:  Table [dbo].[awContestGroup]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContestGroup](
	[contestGroupId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[title] [nvarchar](255) NOT NULL,
	[description] [nvarchar](512) NULL,
	[isEnabled] [bit] NOT NULL,
	[numberOfWinners] [int] NOT NULL,
	[pubDate] [datetime] NULL,
	[pubEndDate] [datetime] NULL,
	[userId] [bigint] NULL,
	[lastBuildDate] [datetime] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_contestGroup] PRIMARY KEY CLUSTERED 
(
	[contestGroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_Paths]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Paths](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NOT NULL,
	[Path] [nvarchar](256) NOT NULL,
	[LoweredPath] [nvarchar](256) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_Users]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Users](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[LoweredUserName] [nvarchar](256) NOT NULL,
	[MobileAlias] [nvarchar](16) NULL,
	[IsAnonymous] [bit] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_Membership]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Membership](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[LoweredEmail] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_PersonalizationPerUser]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_PersonalizationPerUser](
	[Id] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_PersonalizationAllUsers]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_PersonalizationAllUsers](
	[PathId] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_Profile]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [ntext] NOT NULL,
	[PropertyValuesString] [ntext] NOT NULL,
	[PropertyValuesBinary] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_UsersInRoles]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awPollChoice]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awPollChoice](
	[pollChoiceId] [bigint] NOT NULL,
	[pollId] [bigint] NOT NULL,
	[title] [nvarchar](255) NOT NULL,
	[description] [nvarchar](255) NULL,
	[numberOfVotes] [int] NOT NULL,
	[sortOrder] [int] NOT NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_pollChoice] PRIMARY KEY CLUSTERED 
(
	[pollChoiceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awShareItWith]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awShareItWith](
	[shareItWithId] [bigint] NOT NULL,
	[shareItId] [bigint] NOT NULL,
	[userId] [bigint] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awShareItWith] PRIMARY KEY CLUSTERED 
(
	[shareItWithId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awFile]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awFile](
	[fileId] [bigint] NOT NULL,
	[siteId] [bigint] NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[description] [nvarchar](512) NULL,
	[path] [nvarchar](255) NULL,
	[thumbnail] [nvarchar](512) NULL,
	[contentType] [nvarchar](100) NULL,
	[contentSize] [int] NOT NULL,
	[link] [nvarchar](255) NULL,
	[pubDate] [datetime] NULL,
	[lastBuildDate] [datetime] NULL,
	[isEnabled] [bit] NOT NULL,
	[isOnLocal] [bit] NOT NULL,
	[userId] [bigint] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awFile] PRIMARY KEY CLUSTERED 
(
	[fileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awFileInGroup]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awFileInGroup](
	[fileInGroupId] [bigint] NOT NULL,
	[fileGroupId] [bigint] NOT NULL,
	[fileId] [bigint] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awFileInGroup] PRIMARY KEY CLUSTERED 
(
	[fileInGroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awBlogCategory]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awBlogCategory](
	[blogCategoryId] [bigint] NOT NULL,
	[blogId] [bigint] NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[description] [nvarchar](512) NULL,
	[isEnabled] [bit] NOT NULL,
	[parentBlogCategoryId] [bigint] NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awBlogCategory] PRIMARY KEY CLUSTERED 
(
	[blogCategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awBlogTag]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awBlogTag](
	[blogTagId] [bigint] NOT NULL,
	[blogId] [bigint] NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awBlogTag] PRIMARY KEY CLUSTERED 
(
	[blogTagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awBlogPost]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awBlogPost](
	[blogPostId] [bigint] NOT NULL,
	[blogId] [bigint] NOT NULL,
	[authorUserId] [bigint] NULL,
	[title] [nvarchar](512) NULL,
	[description] [nvarchar](max) NULL,
	[summary] [nvarchar](512) NULL,
	[isPublished] [bit] NOT NULL,
	[isCommentable] [bit] NOT NULL,
	[recommended] [int] NOT NULL,
	[pubDate] [datetime] NULL,
	[pubEndDate] [datetime] NULL,
	[geoTag] [nvarchar](255) NULL,
	[imageurl] [nvarchar](512) NULL,
	[userId] [bigint] NOT NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awBlogPost] PRIMARY KEY CLUSTERED 
(
	[blogPostId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awSiteTaggedContent]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awSiteTaggedContent](
	[siteTaggedContentId] [bigint] NOT NULL,
	[siteTagId] [bigint] NOT NULL,
	[contentId] [bigint] NOT NULL,
 CONSTRAINT [PK_awTaggedContent] PRIMARY KEY CLUSTERED 
(
	[siteTaggedContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContestWinner]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContestWinner](
	[contestWinnerId] [bigint] NOT NULL,
	[contestGroupId] [bigint] NULL,
	[contestId] [bigint] NULL,
	[winnerUserId] [bigint] NOT NULL,
	[title] [nvarchar](255) NULL,
	[description] [nvarchar](max) NULL,
	[imageUrl] [nvarchar](512) NULL,
	[userId] [bigint] NULL,
	[lastBuildDate] [datetime] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awContestWinner] PRIMARY KEY CLUSTERED 
(
	[contestWinnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If it is a group winner, contestId will be empty, and contestGroupId will be filled.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'awContestWinner', @level2type=N'COLUMN',@level2name=N'contestGroupId'
GO
/****** Object:  Table [dbo].[awContestGroupMember]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContestGroupMember](
	[contestGroupMemberId] [bigint] NOT NULL,
	[contestGroupId] [bigint] NOT NULL,
	[contestId] [bigint] NOT NULL,
 CONSTRAINT [PK_awContestGroupMember] PRIMARY KEY CLUSTERED 
(
	[contestGroupMemberId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContestEntry]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[awContestEntry](
	[contestEntryId] [bigint] NOT NULL,
	[contestId] [bigint] NOT NULL,
	[userId] [bigint] NULL,
	[cultureCode] [varchar](2) NULL,
	[email] [varchar](100) NULL,
	[firstName] [nvarchar](50) NULL,
	[lastName] [nvarchar](50) NULL,
	[title] [nvarchar](255) NULL,
	[description] [nvarchar](512) NULL,
	[fileId] [bigint] NULL,
	[fileUrl] [varchar](512) NULL,
	[imageUrl] [varchar](512) NULL,
	[tel] [varchar](20) NULL,
	[telType] [varchar](20) NULL,
	[address] [nvarchar](255) NULL,
	[city] [nvarchar](50) NULL,
	[province] [nvarchar](20) NULL,
	[postalCode] [varchar](50) NULL,
	[country] [varchar](50) NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awContestEntry] PRIMARY KEY CLUSTERED 
(
	[contestEntryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[awBlogPostComment]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awBlogPostComment](
	[blogCommentID] [bigint] NOT NULL,
	[blogPostId] [bigint] NOT NULL,
	[title] [nvarchar](50) NULL,
	[description] [nvarchar](1024) NOT NULL,
	[userId] [bigint] NULL,
	[email] [nvarchar](50) NULL,
	[userName] [nvarchar](50) NULL,
	[firstName] [nvarchar](50) NULL,
	[lastName] [nvarchar](50) NULL,
	[status] [int] NOT NULL,
	[pubDate] [datetime] NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awBlogPostComment] PRIMARY KEY CLUSTERED 
(
	[blogCommentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-> Pending, 1->Approved, 2->Rejected' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'awBlogPostComment', @level2type=N'COLUMN',@level2name=N'status'
GO
/****** Object:  Table [dbo].[awBlogTagPost]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awBlogTagPost](
	[blogTagPostId] [bigint] NOT NULL,
	[blogTagId] [bigint] NOT NULL,
	[blogPostId] [bigint] NOT NULL,
 CONSTRAINT [PK_awBlogTagPost] PRIMARY KEY CLUSTERED 
(
	[blogTagPostId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awBlogCategoryPost]    Script Date: 05/07/2010 15:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awBlogCategoryPost](
	[blogCategoryPostId] [bigint] NOT NULL,
	[blogCategoryId] [bigint] NOT NULL,
	[blogPostId] [bigint] NOT NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awBlogCategoryPost] PRIMARY KEY CLUSTERED 
(
	[blogCategoryPostId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContentCustomField]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContentCustomField](
	[customFieldId] [bigint] NOT NULL,
	[contentId] [bigint] NOT NULL,
	[title] [nvarchar](50) NULL,
	[description] [nvarchar](255) NULL,
	[applyToSubContents] [bit] NOT NULL,
	[fieldType] [nvarchar](50) NOT NULL,
	[maximumLength] [int] NULL,
	[minimumValue] [nvarchar](50) NULL,
	[maximumValue] [nchar](10) NULL,
	[defaultValue] [nvarchar](255) NULL,
	[regularExpression] [nvarchar](255) NULL,
	[sortOrder] [int] NOT NULL,
	[isEnabled] [bit] NOT NULL,
	[userId] [bigint] NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awContentCuistomField] PRIMARY KEY CLUSTERED 
(
	[customFieldId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContentCulture]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContentCulture](
	[contentCultureId] [bigint] NOT NULL,
	[contentId] [bigint] NOT NULL,
	[cultureCode] [nvarchar](2) NOT NULL,
	[title] [nvarchar](512) NOT NULL,
	[description] [nvarchar](max) NULL,
	[link] [nvarchar](512) NULL,
	[imageurl] [nvarchar](512) NULL,
	[userId] [bigint] NOT NULL,
 CONSTRAINT [PK_awContentLanguage] PRIMARY KEY CLUSTERED 
(
	[contentCultureId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContentFormFieldSetting]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContentFormFieldSetting](
	[contentFormFieldSettingId] [bigint] NOT NULL,
	[contentFormId] [bigint] NOT NULL,
	[staticFieldName] [nvarchar](100) NULL,
	[isContentCustomField] [bit] NOT NULL,
	[contentCustomFieldId] [bigint] NULL,
	[sortOrder] [int] NOT NULL,
	[isEnabled] [bit] NOT NULL,
	[isVisible] [bit] NOT NULL,
 CONSTRAINT [PK_contentFormField] PRIMARY KEY CLUSTERED 
(
	[contentFormFieldSettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContentFormGroupMember]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContentFormGroupMember](
	[contentFormGroupMemberId] [bigint] NOT NULL,
	[contentFormGroupId] [bigint] NOT NULL,
	[contentFormId] [bigint] NOT NULL,
	[sortOrder] [int] NOT NULL,
 CONSTRAINT [PK_awContentFormGroupMember] PRIMARY KEY CLUSTERED 
(
	[contentFormGroupMemberId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[awContentCustomFieldValue]    Script Date: 05/07/2010 15:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awContentCustomFieldValue](
	[customFieldValueId] [bigint] NOT NULL,
	[customFieldId] [bigint] NOT NULL,
	[contentId] [bigint] NOT NULL,
	[cultureCode] [nvarchar](2) NULL,
	[fieldValue] [nvarchar](2048) NULL,
	[userId] [bigint] NOT NULL,
	[lastBuildDate] [datetime] NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_awContentCustomFieldValue] PRIMARY KEY CLUSTERED 
(
	[customFieldValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF__aspnet_Ap__Appli__5812160E]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Applications] ADD  DEFAULT (newid()) FOR [ApplicationId]
GO
/****** Object:  Default [DF__aspnet_Me__Passw__59063A47]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Membership] ADD  DEFAULT ((0)) FOR [PasswordFormat]
GO
/****** Object:  Default [DF__aspnet_Pa__PathI__59FA5E80]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Paths] ADD  DEFAULT (newid()) FOR [PathId]
GO
/****** Object:  Default [DF__aspnet_Perso__Id__5AEE82B9]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] ADD  DEFAULT (newid()) FOR [Id]
GO
/****** Object:  Default [DF__aspnet_Ro__RoleI__5BE2A6F2]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Roles] ADD  DEFAULT (newid()) FOR [RoleId]
GO
/****** Object:  Default [DF__aspnet_Us__UserI__5CD6CB2B]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT (newid()) FOR [UserId]
GO
/****** Object:  Default [DF__aspnet_Us__Mobil__5DCAEF64]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT (NULL) FOR [MobileAlias]
GO
/****** Object:  Default [DF__aspnet_Us__IsAno__5EBF139D]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT ((0)) FOR [IsAnonymous]
GO
/****** Object:  Default [DF_awAutomatedTask_isCompleted]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awAutomatedTask] ADD  CONSTRAINT [DF_awAutomatedTask_isCompleted]  DEFAULT ((0)) FOR [isCompleted]
GO
/****** Object:  Default [DF_awAutomatedTask_deleteAfterComplete]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awAutomatedTask] ADD  CONSTRAINT [DF_awAutomatedTask_deleteAfterComplete]  DEFAULT ((0)) FOR [deleteAfterComplete]
GO
/****** Object:  Default [DF_awAutomatedTask_lastBuildDate]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awAutomatedTask] ADD  CONSTRAINT [DF_awAutomatedTask_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awAutomatedTask_createDate]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awAutomatedTask] ADD  CONSTRAINT [DF_awAutomatedTask_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awBlog_enabled]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlog] ADD  CONSTRAINT [DF_awBlog_enabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awBlog_enableCommentEmailNotifier]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlog] ADD  CONSTRAINT [DF_awBlog_enableCommentEmailNotifier]  DEFAULT ((0)) FOR [enableCommentEmailNotifier]
GO
/****** Object:  Default [DF_awBlog_lastBuildDate]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlog] ADD  CONSTRAINT [DF_awBlog_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awBlog_createDate]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlog] ADD  CONSTRAINT [DF_awBlog_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awBlogCategory_isEnabled]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogCategory] ADD  CONSTRAINT [DF_awBlogCategory_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awBlogPost_isPublished]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPost] ADD  CONSTRAINT [DF_awBlogPost_isPublished]  DEFAULT ((0)) FOR [isPublished]
GO
/****** Object:  Default [DF_awBlogPost_isCommentable]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPost] ADD  CONSTRAINT [DF_awBlogPost_isCommentable]  DEFAULT ((0)) FOR [isCommentable]
GO
/****** Object:  Default [DF_awBlogPost_recommended]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPost] ADD  CONSTRAINT [DF_awBlogPost_recommended]  DEFAULT ((0)) FOR [recommended]
GO
/****** Object:  Default [DF_awBlogPost_createDate]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPost] ADD  CONSTRAINT [DF_awBlogPost_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awBlogPostComment_isApproved]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPostComment] ADD  CONSTRAINT [DF_awBlogPostComment_isApproved]  DEFAULT ((0)) FOR [status]
GO
/****** Object:  Default [DF_awBlogPostComment_createDate]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPostComment] ADD  CONSTRAINT [DF_awBlogPostComment_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awConfiguration_description]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_description]  DEFAULT ('') FOR [description]
GO
/****** Object:  Default [DF_awConfiguration_fileServiceUrl]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_fileServiceUrl]  DEFAULT ('') FOR [fileServiceUrl]
GO
/****** Object:  Default [DF_awConfiguration_contentServiceUrl]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_contentServiceUrl]  DEFAULT ('') FOR [contentServiceUrl]
GO
/****** Object:  Default [DF_awConfiguration_blogServiceUrl]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_blogServiceUrl]  DEFAULT ('') FOR [blogServiceUrl]
GO
/****** Object:  Default [DF_awConfiguration_pollServiceUrl]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_pollServiceUrl]  DEFAULT ('') FOR [pollServiceUrl]
GO
/****** Object:  Default [DF_awConfiguration_automatedTaskServiceUrl]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_automatedTaskServiceUrl]  DEFAULT ('') FOR [automatedTaskServiceUrl]
GO
/****** Object:  Default [DF_awConfiguration_twitterServiceUrl]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_twitterServiceUrl]  DEFAULT ('') FOR [twitterServiceUrl]
GO
/****** Object:  Default [DF_awConfiguration_weatherServiceUrl]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_weatherServiceUrl]  DEFAULT ('') FOR [weatherServiceUrl]
GO
/****** Object:  Default [DF_awConfiguration_adminBaseUrl]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_adminBaseUrl]  DEFAULT ('') FOR [adminBaseUrl]
GO
/****** Object:  Default [DF_awConfiguration_fileDirectory]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_fileDirectory]  DEFAULT ('') FOR [fileDirectory]
GO
/****** Object:  Default [DF_awConfiguration_fileMimeXMLPath]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_fileMimeXMLPath]  DEFAULT ('') FOR [fileMimeXMLPath]
GO
/****** Object:  Default [DF_awConfiguration_fileSaveOnAmazonS3]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_fileSaveOnAmazonS3]  DEFAULT ((0)) FOR [fileSaveOnAmazonS3]
GO
/****** Object:  Default [DF_awConfiguration_fileAmazonS3BucketName]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_fileAmazonS3BucketName]  DEFAULT ('') FOR [fileAmazonS3BucketName]
GO
/****** Object:  Default [DF_awConfiguration_fileAmazonS3AccessKey]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_fileAmazonS3AccessKey]  DEFAULT ('') FOR [fileAmazonS3AccessKey]
GO
/****** Object:  Default [DF_awConfiguration_fileAmazonS3SecreyKey]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_fileAmazonS3SecreyKey]  DEFAULT ('') FOR [fileAmazonS3SecreyKey]
GO
/****** Object:  Default [DF_awConfiguration_smtpServer]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_smtpServer]  DEFAULT ('') FOR [smtpServer]
GO
/****** Object:  Default [DF_awConfiguration_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awConfiguration] ADD  CONSTRAINT [DF_awConfiguration_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awContent_title]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_title]  DEFAULT ((0)) FOR [title]
GO
/****** Object:  Default [DF_awContent_siteId]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_siteId]  DEFAULT ((0)) FOR [siteId]
GO
/****** Object:  Default [DF_awContent_contentType]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_contentType]  DEFAULT (N'content') FOR [contentType]
GO
/****** Object:  Default [DF_awContent_isEnabled_1]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_isEnabled_1]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awContent_isCommentable]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_isCommentable]  DEFAULT ((0)) FOR [isCommentable]
GO
/****** Object:  Default [DF_awContent_parentContentId]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_parentContentId]  DEFAULT ((0)) FOR [parentContentId]
GO
/****** Object:  Default [DF_awContent_orderNo]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_orderNo]  DEFAULT ((0)) FOR [sortOrder]
GO
/****** Object:  Default [DF_awContent_userId]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_userId]  DEFAULT ((0)) FOR [userId]
GO
/****** Object:  Default [DF_awContent_pubDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_pubDate]  DEFAULT (getdate()) FOR [pubDate]
GO
/****** Object:  Default [DF_awContent_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent] ADD  CONSTRAINT [DF_awContent_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awContentComment_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentComment] ADD  CONSTRAINT [DF_awContentComment_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awContentComment_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentComment] ADD  CONSTRAINT [DF_awContentComment_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awContentCustomField_applyToSubContents]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomField] ADD  CONSTRAINT [DF_awContentCustomField_applyToSubContents]  DEFAULT ((0)) FOR [applyToSubContents]
GO
/****** Object:  Default [DF_awContentCuistomField_fieldType]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomField] ADD  CONSTRAINT [DF_awContentCuistomField_fieldType]  DEFAULT (N'string') FOR [fieldType]
GO
/****** Object:  Default [DF_awContentCuistomField_maximumLength]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomField] ADD  CONSTRAINT [DF_awContentCuistomField_maximumLength]  DEFAULT ((0)) FOR [maximumLength]
GO
/****** Object:  Default [DF_awContentCuistomField_sortOrder]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomField] ADD  CONSTRAINT [DF_awContentCuistomField_sortOrder]  DEFAULT ((0)) FOR [sortOrder]
GO
/****** Object:  Default [DF_awContentCustomField_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomField] ADD  CONSTRAINT [DF_awContentCustomField_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awContentCustomField_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomField] ADD  CONSTRAINT [DF_awContentCustomField_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awContentCustomFieldValue_userId]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomFieldValue] ADD  CONSTRAINT [DF_awContentCustomFieldValue_userId]  DEFAULT ((0)) FOR [userId]
GO
/****** Object:  Default [DF_awContentCustomFieldValue_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomFieldValue] ADD  CONSTRAINT [DF_awContentCustomFieldValue_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awContentForm_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentForm] ADD  CONSTRAINT [DF_awContentForm_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_contentForm_applyToSub]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentForm] ADD  CONSTRAINT [DF_contentForm_applyToSub]  DEFAULT ((0)) FOR [applyToSub]
GO
/****** Object:  Default [DF_contentForm_canCreateNew]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentForm] ADD  CONSTRAINT [DF_contentForm_canCreateNew]  DEFAULT ((0)) FOR [canCreateNew]
GO
/****** Object:  Default [DF_contentForm_canUpdate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentForm] ADD  CONSTRAINT [DF_contentForm_canUpdate]  DEFAULT ((0)) FOR [canUpdate]
GO
/****** Object:  Default [DF_awContentForm_canDelete]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentForm] ADD  CONSTRAINT [DF_awContentForm_canDelete]  DEFAULT ((0)) FOR [canDelete]
GO
/****** Object:  Default [DF_contentForm_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentForm] ADD  CONSTRAINT [DF_contentForm_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_contentForm_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentForm] ADD  CONSTRAINT [DF_contentForm_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_Table_1_isFixedField]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormFieldSetting] ADD  CONSTRAINT [DF_Table_1_isFixedField]  DEFAULT ((0)) FOR [isContentCustomField]
GO
/****** Object:  Default [DF_contentFormField_sortOrder]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormFieldSetting] ADD  CONSTRAINT [DF_contentFormField_sortOrder]  DEFAULT ((0)) FOR [sortOrder]
GO
/****** Object:  Default [DF_contentFormField_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormFieldSetting] ADD  CONSTRAINT [DF_contentFormField_isEnabled]  DEFAULT ((1)) FOR [isEnabled]
GO
/****** Object:  Default [DF_contentFormField_isVisible]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormFieldSetting] ADD  CONSTRAINT [DF_contentFormField_isVisible]  DEFAULT ((1)) FOR [isVisible]
GO
/****** Object:  Default [DF_awContentFormGroupMember_sortOrder]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormGroupMember] ADD  CONSTRAINT [DF_awContentFormGroupMember_sortOrder]  DEFAULT ((0)) FOR [sortOrder]
GO
/****** Object:  Default [DF_awContest_maxEntry]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContest] ADD  CONSTRAINT [DF_awContest_maxEntry]  DEFAULT ((0)) FOR [maxEntry]
GO
/****** Object:  Default [DF_awContest_maxEntryPerUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContest] ADD  CONSTRAINT [DF_awContest_maxEntryPerUser]  DEFAULT ((0)) FOR [entryPerUser]
GO
/****** Object:  Default [DF_awContest_maxEntryPerUserPeriodValue]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContest] ADD  CONSTRAINT [DF_awContest_maxEntryPerUserPeriodValue]  DEFAULT ((0)) FOR [entryPerUserPeriodValue]
GO
/****** Object:  Default [DF_awContest_numberOfWinners]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContest] ADD  CONSTRAINT [DF_awContest_numberOfWinners]  DEFAULT ((1)) FOR [numberOfWinners]
GO
/****** Object:  Default [DF_awContest_pubDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContest] ADD  CONSTRAINT [DF_awContest_pubDate]  DEFAULT (getdate()) FOR [pubDate]
GO
/****** Object:  Default [DF_awContest_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContest] ADD  CONSTRAINT [DF_awContest_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awContest_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContest] ADD  CONSTRAINT [DF_awContest_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awContestEntry_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestEntry] ADD  CONSTRAINT [DF_awContestEntry_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_contestGroup_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestGroup] ADD  CONSTRAINT [DF_contestGroup_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awContestGroup_numberOfWinners]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestGroup] ADD  CONSTRAINT [DF_awContestGroup_numberOfWinners]  DEFAULT ((0)) FOR [numberOfWinners]
GO
/****** Object:  Default [DF_contestGroup_pubDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestGroup] ADD  CONSTRAINT [DF_contestGroup_pubDate]  DEFAULT (getdate()) FOR [pubDate]
GO
/****** Object:  Default [DF_contestGroup_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestGroup] ADD  CONSTRAINT [DF_contestGroup_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_contestGroup_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestGroup] ADD  CONSTRAINT [DF_contestGroup_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awContestWinner_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestWinner] ADD  CONSTRAINT [DF_awContestWinner_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awContestWinner_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestWinner] ADD  CONSTRAINT [DF_awContestWinner_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awEmailTemplate_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awEmailTemplate] ADD  CONSTRAINT [DF_awEmailTemplate_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awEmailTemplate_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awEmailTemplate] ADD  CONSTRAINT [DF_awEmailTemplate_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awEnvironment_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awEnvironment] ADD  CONSTRAINT [DF_awEnvironment_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awEnvironment_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awEnvironment] ADD  CONSTRAINT [DF_awEnvironment_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awFile_contentSize]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFile] ADD  CONSTRAINT [DF_awFile_contentSize]  DEFAULT ((0)) FOR [contentSize]
GO
/****** Object:  Default [DF_awFile_pubDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFile] ADD  CONSTRAINT [DF_awFile_pubDate]  DEFAULT (getdate()) FOR [pubDate]
GO
/****** Object:  Default [DF_awFile_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFile] ADD  CONSTRAINT [DF_awFile_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awFile_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFile] ADD  CONSTRAINT [DF_awFile_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awFile_isOnLocal]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFile] ADD  CONSTRAINT [DF_awFile_isOnLocal]  DEFAULT ((1)) FOR [isOnLocal]
GO
/****** Object:  Default [DF_awFile_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFile] ADD  CONSTRAINT [DF_awFile_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awFileGroup_siteId]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFileGroup] ADD  CONSTRAINT [DF_awFileGroup_siteId]  DEFAULT ((0)) FOR [siteId]
GO
/****** Object:  Default [DF_awFileGroup_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFileGroup] ADD  CONSTRAINT [DF_awFileGroup_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awFileInGroup_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFileInGroup] ADD  CONSTRAINT [DF_awFileInGroup_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awPage_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPage] ADD  CONSTRAINT [DF_awPage_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awPage_isLoginRequired]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPage] ADD  CONSTRAINT [DF_awPage_isLoginRequired]  DEFAULT ((0)) FOR [isLoginRequired]
GO
/****** Object:  Default [DF_awPage_outputCaching]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPage] ADD  CONSTRAINT [DF_awPage_outputCaching]  DEFAULT ((0)) FOR [outputCaching]
GO
/****** Object:  Default [DF_awPage_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPage] ADD  CONSTRAINT [DF_awPage_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awPage_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPage] ADD  CONSTRAINT [DF_awPage_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awPoll_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPoll] ADD  CONSTRAINT [DF_awPoll_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awPoll_isLoginRequired]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPoll] ADD  CONSTRAINT [DF_awPoll_isLoginRequired]  DEFAULT ((1)) FOR [isPublic]
GO
/****** Object:  Default [DF_awPoll_isMultipleChoice]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPoll] ADD  CONSTRAINT [DF_awPoll_isMultipleChoice]  DEFAULT ((0)) FOR [isMultipleChoice]
GO
/****** Object:  Default [DF_awPoll_pubDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPoll] ADD  CONSTRAINT [DF_awPoll_pubDate]  DEFAULT (getdate()) FOR [pubDate]
GO
/****** Object:  Default [DF_awPoll_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPoll] ADD  CONSTRAINT [DF_awPoll_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awPollChoice_numberOfVotes]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPollChoice] ADD  CONSTRAINT [DF_awPollChoice_numberOfVotes]  DEFAULT ((0)) FOR [numberOfVotes]
GO
/****** Object:  Default [DF_pollChoice_sortOrder]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPollChoice] ADD  CONSTRAINT [DF_pollChoice_sortOrder]  DEFAULT ((0)) FOR [sortOrder]
GO
/****** Object:  Default [DF_pollChoice_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPollChoice] ADD  CONSTRAINT [DF_pollChoice_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awRole_canRead]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRole] ADD  CONSTRAINT [DF_awRole_canRead]  DEFAULT ((0)) FOR [canRead]
GO
/****** Object:  Default [DF_awRole_canUpdateStatus]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRole] ADD  CONSTRAINT [DF_awRole_canUpdateStatus]  DEFAULT ((0)) FOR [canUpdateStatus]
GO
/****** Object:  Default [DF_awRole_canUpdate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRole] ADD  CONSTRAINT [DF_awRole_canUpdate]  DEFAULT ((0)) FOR [canUpdate]
GO
/****** Object:  Default [DF_awRole_canAdd]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRole] ADD  CONSTRAINT [DF_awRole_canAdd]  DEFAULT ((0)) FOR [canAdd]
GO
/****** Object:  Default [DF_awRole_canDelete]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRole] ADD  CONSTRAINT [DF_awRole_canDelete]  DEFAULT ((0)) FOR [canDelete]
GO
/****** Object:  Default [DF_awRole_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRole] ADD  CONSTRAINT [DF_awRole_lastBuildDate]  DEFAULT ((0)) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awRole_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRole] ADD  CONSTRAINT [DF_awRole_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awShareIt_shareWithEveryone]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awShareIt] ADD  CONSTRAINT [DF_awShareIt_shareWithEveryone]  DEFAULT ((0)) FOR [shareWithEveryone]
GO
/****** Object:  Default [DF_awShareIt_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awShareIt] ADD  CONSTRAINT [DF_awShareIt_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awShareItWith_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awShareItWith] ADD  CONSTRAINT [DF_awShareItWith_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awSite_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSite] ADD  CONSTRAINT [DF_awSite_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awSite_maxBlogs]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSite] ADD  CONSTRAINT [DF_awSite_maxBlogs]  DEFAULT ((10)) FOR [maxBlogs]
GO
/****** Object:  Default [DF_awSite_maxUsers]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSite] ADD  CONSTRAINT [DF_awSite_maxUsers]  DEFAULT ((0)) FOR [maxUsers]
GO
/****** Object:  Default [DF_awSite_maxContent]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSite] ADD  CONSTRAINT [DF_awSite_maxContent]  DEFAULT ((0)) FOR [maxContents]
GO
/****** Object:  Default [DF_awSite_userConfirmationEmailTemplateId]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSite] ADD  CONSTRAINT [DF_awSite_userConfirmationEmailTemplateId]  DEFAULT ((0)) FOR [userConfirmationEmailTemplateId]
GO
/****** Object:  Default [DF_awSite_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSite] ADD  CONSTRAINT [DF_awSite_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awSiteIpSecurity_allowRead]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteDomainSecurity] ADD  CONSTRAINT [DF_awSiteIpSecurity_allowRead]  DEFAULT ((0)) FOR [allowRead]
GO
/****** Object:  Default [DF_awSiteIpSecurity_allowUpdate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteDomainSecurity] ADD  CONSTRAINT [DF_awSiteIpSecurity_allowUpdate]  DEFAULT ((0)) FOR [allowUpdate]
GO
/****** Object:  Default [DF_awSiteIpSecurity_allowCreate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteDomainSecurity] ADD  CONSTRAINT [DF_awSiteIpSecurity_allowCreate]  DEFAULT ((0)) FOR [allowCreate]
GO
/****** Object:  Default [DF_awSiteIpSecurity_allowDelete]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteDomainSecurity] ADD  CONSTRAINT [DF_awSiteIpSecurity_allowDelete]  DEFAULT ((0)) FOR [allowDelete]
GO
/****** Object:  Default [DF_awSiteIpSecurity_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteDomainSecurity] ADD  CONSTRAINT [DF_awSiteIpSecurity_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awSiteMap_pageId]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteMap] ADD  CONSTRAINT [DF_awSiteMap_pageId]  DEFAULT ((0)) FOR [pageId]
GO
/****** Object:  Default [DF_awSiteMap_isLoginRequired]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteMap] ADD  CONSTRAINT [DF_awSiteMap_isLoginRequired]  DEFAULT ((0)) FOR [isLoginRequired]
GO
/****** Object:  Default [DF_awSiteMap_lastBuildDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteMap] ADD  CONSTRAINT [DF_awSiteMap_lastBuildDate]  DEFAULT (getdate()) FOR [lastBuildDate]
GO
/****** Object:  Default [DF_awSiteMap_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteMap] ADD  CONSTRAINT [DF_awSiteMap_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awTag_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteTag] ADD  CONSTRAINT [DF_awTag_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awSiteUser_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteUser] ADD  CONSTRAINT [DF_awSiteUser_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awSiteUser_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteUser] ADD  CONSTRAINT [DF_awSiteUser_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  Default [DF_awUser_isSuperAdmin]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awUser] ADD  CONSTRAINT [DF_awUser_isSuperAdmin]  DEFAULT ((0)) FOR [isSuperAdmin]
GO
/****** Object:  Default [DF_awUser_isEnabled]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awUser] ADD  CONSTRAINT [DF_awUser_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
/****** Object:  Default [DF_awUser_isDeleted]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awUser] ADD  CONSTRAINT [DF_awUser_isDeleted]  DEFAULT ((0)) FOR [isDeleted]
GO
/****** Object:  Default [DF_awUser_createDate]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awUser] ADD  CONSTRAINT [DF_awUser_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
/****** Object:  ForeignKey [FK__aspnet_Me__Appli__3D2915A8]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
/****** Object:  ForeignKey [FK__aspnet_Me__Appli__3E1D39E1]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
/****** Object:  ForeignKey [FK__aspnet_Me__UserI__3F115E1A]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
/****** Object:  ForeignKey [FK__aspnet_Me__UserI__40058253]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pa__Appli__40F9A68C]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Paths]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pa__Appli__41EDCAC5]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Paths]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pe__PathI__42E1EEFE]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pe__PathI__43D61337]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pe__PathI__44CA3770]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pe__PathI__45BE5BA9]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pe__UserI__46B27FE2]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pe__UserI__47A6A41B]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pr__UserI__489AC854]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
/****** Object:  ForeignKey [FK__aspnet_Pr__UserI__498EEC8D]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
/****** Object:  ForeignKey [FK__aspnet_Ro__Appli__4A8310C6]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Roles]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
/****** Object:  ForeignKey [FK__aspnet_Ro__Appli__4B7734FF]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Roles]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
/****** Object:  ForeignKey [FK__aspnet_Us__Appli__4C6B5938]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Users]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
/****** Object:  ForeignKey [FK__aspnet_Us__Appli__4D5F7D71]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_Users]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
/****** Object:  ForeignKey [FK__aspnet_Us__RoleI__4E53A1AA]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
GO
/****** Object:  ForeignKey [FK__aspnet_Us__RoleI__4F47C5E3]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
GO
/****** Object:  ForeignKey [FK__aspnet_Us__UserI__503BEA1C]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
/****** Object:  ForeignKey [FK__aspnet_Us__UserI__51300E55]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
/****** Object:  ForeignKey [FK_awAutomatedTask_awSite]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awAutomatedTask]  WITH CHECK ADD  CONSTRAINT [FK_awAutomatedTask_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awAutomatedTask] CHECK CONSTRAINT [FK_awAutomatedTask_awSite]
GO
/****** Object:  ForeignKey [FK_awBlog_awEmailTemplate]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlog]  WITH CHECK ADD  CONSTRAINT [FK_awBlog_awEmailTemplate] FOREIGN KEY([commentEmailTemplateId])
REFERENCES [dbo].[awEmailTemplate] ([emailTemplateId])
GO
ALTER TABLE [dbo].[awBlog] CHECK CONSTRAINT [FK_awBlog_awEmailTemplate]
GO
/****** Object:  ForeignKey [FK_awBlog_awSite]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlog]  WITH CHECK ADD  CONSTRAINT [FK_awBlog_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awBlog] CHECK CONSTRAINT [FK_awBlog_awSite]
GO
/****** Object:  ForeignKey [FK_awBlog_awUser]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlog]  WITH CHECK ADD  CONSTRAINT [FK_awBlog_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awBlog] CHECK CONSTRAINT [FK_awBlog_awUser]
GO
/****** Object:  ForeignKey [FK_awBlogCategory_awBlog]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogCategory]  WITH CHECK ADD  CONSTRAINT [FK_awBlogCategory_awBlog] FOREIGN KEY([blogId])
REFERENCES [dbo].[awBlog] ([blogId])
GO
ALTER TABLE [dbo].[awBlogCategory] CHECK CONSTRAINT [FK_awBlogCategory_awBlog]
GO
/****** Object:  ForeignKey [FK_awBlogCategoryPost_awBlogCategory1]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogCategoryPost]  WITH CHECK ADD  CONSTRAINT [FK_awBlogCategoryPost_awBlogCategory1] FOREIGN KEY([blogCategoryId])
REFERENCES [dbo].[awBlogCategory] ([blogCategoryId])
GO
ALTER TABLE [dbo].[awBlogCategoryPost] CHECK CONSTRAINT [FK_awBlogCategoryPost_awBlogCategory1]
GO
/****** Object:  ForeignKey [FK_awBlogCategoryPost_awBlogPost]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogCategoryPost]  WITH CHECK ADD  CONSTRAINT [FK_awBlogCategoryPost_awBlogPost] FOREIGN KEY([blogPostId])
REFERENCES [dbo].[awBlogPost] ([blogPostId])
GO
ALTER TABLE [dbo].[awBlogCategoryPost] CHECK CONSTRAINT [FK_awBlogCategoryPost_awBlogPost]
GO
/****** Object:  ForeignKey [FK_awBlogPost_awBlog]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPost]  WITH CHECK ADD  CONSTRAINT [FK_awBlogPost_awBlog] FOREIGN KEY([blogId])
REFERENCES [dbo].[awBlog] ([blogId])
GO
ALTER TABLE [dbo].[awBlogPost] CHECK CONSTRAINT [FK_awBlogPost_awBlog]
GO
/****** Object:  ForeignKey [FK_awBlogPost_awUser1]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPost]  WITH CHECK ADD  CONSTRAINT [FK_awBlogPost_awUser1] FOREIGN KEY([authorUserId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awBlogPost] CHECK CONSTRAINT [FK_awBlogPost_awUser1]
GO
/****** Object:  ForeignKey [FK_awBlogPostComment_awBlogPost]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPostComment]  WITH CHECK ADD  CONSTRAINT [FK_awBlogPostComment_awBlogPost] FOREIGN KEY([blogPostId])
REFERENCES [dbo].[awBlogPost] ([blogPostId])
GO
ALTER TABLE [dbo].[awBlogPostComment] CHECK CONSTRAINT [FK_awBlogPostComment_awBlogPost]
GO
/****** Object:  ForeignKey [FK_awBlogPostComment_awUser]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogPostComment]  WITH CHECK ADD  CONSTRAINT [FK_awBlogPostComment_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awBlogPostComment] CHECK CONSTRAINT [FK_awBlogPostComment_awUser]
GO
/****** Object:  ForeignKey [FK_awBlogTag_awBlog]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogTag]  WITH CHECK ADD  CONSTRAINT [FK_awBlogTag_awBlog] FOREIGN KEY([blogId])
REFERENCES [dbo].[awBlog] ([blogId])
GO
ALTER TABLE [dbo].[awBlogTag] CHECK CONSTRAINT [FK_awBlogTag_awBlog]
GO
/****** Object:  ForeignKey [FK_awBlogTagPost_awBlogPost]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogTagPost]  WITH CHECK ADD  CONSTRAINT [FK_awBlogTagPost_awBlogPost] FOREIGN KEY([blogPostId])
REFERENCES [dbo].[awBlogPost] ([blogPostId])
GO
ALTER TABLE [dbo].[awBlogTagPost] CHECK CONSTRAINT [FK_awBlogTagPost_awBlogPost]
GO
/****** Object:  ForeignKey [FK_awBlogTagPost_awBlogTag]    Script Date: 05/07/2010 15:33:39 ******/
ALTER TABLE [dbo].[awBlogTagPost]  WITH CHECK ADD  CONSTRAINT [FK_awBlogTagPost_awBlogTag] FOREIGN KEY([blogTagId])
REFERENCES [dbo].[awBlogTag] ([blogTagId])
GO
ALTER TABLE [dbo].[awBlogTagPost] CHECK CONSTRAINT [FK_awBlogTagPost_awBlogTag]
GO
/****** Object:  ForeignKey [FK_awContent_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent]  WITH CHECK ADD  CONSTRAINT [FK_awContent_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awContent] CHECK CONSTRAINT [FK_awContent_awSite]
GO
/****** Object:  ForeignKey [FK_awContent_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContent]  WITH CHECK ADD  CONSTRAINT [FK_awContent_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awContent] CHECK CONSTRAINT [FK_awContent_awUser]
GO
/****** Object:  ForeignKey [FK_awContentComment_awContent]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentComment]  WITH CHECK ADD  CONSTRAINT [FK_awContentComment_awContent] FOREIGN KEY([contentId])
REFERENCES [dbo].[awContent] ([contentId])
GO
ALTER TABLE [dbo].[awContentComment] CHECK CONSTRAINT [FK_awContentComment_awContent]
GO
/****** Object:  ForeignKey [FK_awContentComment_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentComment]  WITH CHECK ADD  CONSTRAINT [FK_awContentComment_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awContentComment] CHECK CONSTRAINT [FK_awContentComment_awSite]
GO
/****** Object:  ForeignKey [FK_awContentLanguage_awContent]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCulture]  WITH CHECK ADD  CONSTRAINT [FK_awContentLanguage_awContent] FOREIGN KEY([contentId])
REFERENCES [dbo].[awContent] ([contentId])
GO
ALTER TABLE [dbo].[awContentCulture] CHECK CONSTRAINT [FK_awContentLanguage_awContent]
GO
/****** Object:  ForeignKey [FK_awContentLanguage_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCulture]  WITH CHECK ADD  CONSTRAINT [FK_awContentLanguage_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awContentCulture] CHECK CONSTRAINT [FK_awContentLanguage_awUser]
GO
/****** Object:  ForeignKey [FK_awContentCustomField_awContent]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomField]  WITH CHECK ADD  CONSTRAINT [FK_awContentCustomField_awContent] FOREIGN KEY([contentId])
REFERENCES [dbo].[awContent] ([contentId])
GO
ALTER TABLE [dbo].[awContentCustomField] CHECK CONSTRAINT [FK_awContentCustomField_awContent]
GO
/****** Object:  ForeignKey [FK_awContentCustomFieldValue_awContentCustomField]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomFieldValue]  WITH CHECK ADD  CONSTRAINT [FK_awContentCustomFieldValue_awContentCustomField] FOREIGN KEY([customFieldId])
REFERENCES [dbo].[awContentCustomField] ([customFieldId])
GO
ALTER TABLE [dbo].[awContentCustomFieldValue] CHECK CONSTRAINT [FK_awContentCustomFieldValue_awContentCustomField]
GO
/****** Object:  ForeignKey [FK_awContentCustomFieldValue_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentCustomFieldValue]  WITH CHECK ADD  CONSTRAINT [FK_awContentCustomFieldValue_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awContentCustomFieldValue] CHECK CONSTRAINT [FK_awContentCustomFieldValue_awUser]
GO
/****** Object:  ForeignKey [FK_awContentForm_awContent]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentForm]  WITH CHECK ADD  CONSTRAINT [FK_awContentForm_awContent] FOREIGN KEY([contentId])
REFERENCES [dbo].[awContent] ([contentId])
GO
ALTER TABLE [dbo].[awContentForm] CHECK CONSTRAINT [FK_awContentForm_awContent]
GO
/****** Object:  ForeignKey [FK_awContentForm_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentForm]  WITH CHECK ADD  CONSTRAINT [FK_awContentForm_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awContentForm] CHECK CONSTRAINT [FK_awContentForm_awSite]
GO
/****** Object:  ForeignKey [FK_awContentFormFieldSetting_awContentCustomField]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormFieldSetting]  WITH CHECK ADD  CONSTRAINT [FK_awContentFormFieldSetting_awContentCustomField] FOREIGN KEY([contentCustomFieldId])
REFERENCES [dbo].[awContentCustomField] ([customFieldId])
GO
ALTER TABLE [dbo].[awContentFormFieldSetting] CHECK CONSTRAINT [FK_awContentFormFieldSetting_awContentCustomField]
GO
/****** Object:  ForeignKey [FK_awContentFormFieldSetting_awContentForm]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormFieldSetting]  WITH CHECK ADD  CONSTRAINT [FK_awContentFormFieldSetting_awContentForm] FOREIGN KEY([contentFormId])
REFERENCES [dbo].[awContentForm] ([contentFormId])
GO
ALTER TABLE [dbo].[awContentFormFieldSetting] CHECK CONSTRAINT [FK_awContentFormFieldSetting_awContentForm]
GO
/****** Object:  ForeignKey [FK_awContentFormGroup_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormGroup]  WITH CHECK ADD  CONSTRAINT [FK_awContentFormGroup_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awContentFormGroup] CHECK CONSTRAINT [FK_awContentFormGroup_awSite]
GO
/****** Object:  ForeignKey [FK_awContentFormGroupMember_awContentForm]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormGroupMember]  WITH CHECK ADD  CONSTRAINT [FK_awContentFormGroupMember_awContentForm] FOREIGN KEY([contentFormId])
REFERENCES [dbo].[awContentForm] ([contentFormId])
GO
ALTER TABLE [dbo].[awContentFormGroupMember] CHECK CONSTRAINT [FK_awContentFormGroupMember_awContentForm]
GO
/****** Object:  ForeignKey [FK_awContentFormGroupMember_awContentFormGroup]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContentFormGroupMember]  WITH CHECK ADD  CONSTRAINT [FK_awContentFormGroupMember_awContentFormGroup] FOREIGN KEY([contentFormGroupId])
REFERENCES [dbo].[awContentFormGroup] ([contentFormGroupId])
GO
ALTER TABLE [dbo].[awContentFormGroupMember] CHECK CONSTRAINT [FK_awContentFormGroupMember_awContentFormGroup]
GO
/****** Object:  ForeignKey [FK_awContest_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContest]  WITH CHECK ADD  CONSTRAINT [FK_awContest_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awContest] CHECK CONSTRAINT [FK_awContest_awSite]
GO
/****** Object:  ForeignKey [FK_awContest_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContest]  WITH CHECK ADD  CONSTRAINT [FK_awContest_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awContest] CHECK CONSTRAINT [FK_awContest_awUser]
GO
/****** Object:  ForeignKey [FK_awContestEntry_awContest]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestEntry]  WITH CHECK ADD  CONSTRAINT [FK_awContestEntry_awContest] FOREIGN KEY([contestId])
REFERENCES [dbo].[awContest] ([contestId])
GO
ALTER TABLE [dbo].[awContestEntry] CHECK CONSTRAINT [FK_awContestEntry_awContest]
GO
/****** Object:  ForeignKey [FK_awContestEntry_awFile]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestEntry]  WITH CHECK ADD  CONSTRAINT [FK_awContestEntry_awFile] FOREIGN KEY([fileId])
REFERENCES [dbo].[awFile] ([fileId])
GO
ALTER TABLE [dbo].[awContestEntry] CHECK CONSTRAINT [FK_awContestEntry_awFile]
GO
/****** Object:  ForeignKey [FK_awContestEntry_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestEntry]  WITH CHECK ADD  CONSTRAINT [FK_awContestEntry_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awContestEntry] CHECK CONSTRAINT [FK_awContestEntry_awUser]
GO
/****** Object:  ForeignKey [FK_awContestGroup_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestGroup]  WITH CHECK ADD  CONSTRAINT [FK_awContestGroup_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awContestGroup] CHECK CONSTRAINT [FK_awContestGroup_awSite]
GO
/****** Object:  ForeignKey [FK_awContestGroup_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestGroup]  WITH CHECK ADD  CONSTRAINT [FK_awContestGroup_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awContestGroup] CHECK CONSTRAINT [FK_awContestGroup_awUser]
GO
/****** Object:  ForeignKey [FK_awContestGroupMember_awContest1]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestGroupMember]  WITH CHECK ADD  CONSTRAINT [FK_awContestGroupMember_awContest1] FOREIGN KEY([contestId])
REFERENCES [dbo].[awContest] ([contestId])
GO
ALTER TABLE [dbo].[awContestGroupMember] CHECK CONSTRAINT [FK_awContestGroupMember_awContest1]
GO
/****** Object:  ForeignKey [FK_awContestGroupMember_awContestGroup1]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestGroupMember]  WITH CHECK ADD  CONSTRAINT [FK_awContestGroupMember_awContestGroup1] FOREIGN KEY([contestGroupId])
REFERENCES [dbo].[awContestGroup] ([contestGroupId])
GO
ALTER TABLE [dbo].[awContestGroupMember] CHECK CONSTRAINT [FK_awContestGroupMember_awContestGroup1]
GO
/****** Object:  ForeignKey [FK_awContestWinner_awContest]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestWinner]  WITH CHECK ADD  CONSTRAINT [FK_awContestWinner_awContest] FOREIGN KEY([contestId])
REFERENCES [dbo].[awContest] ([contestId])
GO
ALTER TABLE [dbo].[awContestWinner] CHECK CONSTRAINT [FK_awContestWinner_awContest]
GO
/****** Object:  ForeignKey [FK_awContestWinner_awContestGroup]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestWinner]  WITH CHECK ADD  CONSTRAINT [FK_awContestWinner_awContestGroup] FOREIGN KEY([contestGroupId])
REFERENCES [dbo].[awContestGroup] ([contestGroupId])
GO
ALTER TABLE [dbo].[awContestWinner] CHECK CONSTRAINT [FK_awContestWinner_awContestGroup]
GO
/****** Object:  ForeignKey [FK_awContestWinner_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestWinner]  WITH CHECK ADD  CONSTRAINT [FK_awContestWinner_awUser] FOREIGN KEY([winnerUserId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awContestWinner] CHECK CONSTRAINT [FK_awContestWinner_awUser]
GO
/****** Object:  ForeignKey [FK_awContestWinner_awUser1]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awContestWinner]  WITH CHECK ADD  CONSTRAINT [FK_awContestWinner_awUser1] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awContestWinner] CHECK CONSTRAINT [FK_awContestWinner_awUser1]
GO
/****** Object:  ForeignKey [FK_awEmailTemplate_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awEmailTemplate]  WITH CHECK ADD  CONSTRAINT [FK_awEmailTemplate_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awEmailTemplate] CHECK CONSTRAINT [FK_awEmailTemplate_awSite]
GO
/****** Object:  ForeignKey [FK_awEnvironment_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awEnvironment]  WITH CHECK ADD  CONSTRAINT [FK_awEnvironment_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awEnvironment] CHECK CONSTRAINT [FK_awEnvironment_awSite]
GO
/****** Object:  ForeignKey [FK_awFile_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFile]  WITH CHECK ADD  CONSTRAINT [FK_awFile_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awFile] CHECK CONSTRAINT [FK_awFile_awUser]
GO
/****** Object:  ForeignKey [FK_awFileGroup_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFileGroup]  WITH CHECK ADD  CONSTRAINT [FK_awFileGroup_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awFileGroup] CHECK CONSTRAINT [FK_awFileGroup_awSite]
GO
/****** Object:  ForeignKey [FK_awFileGroup_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFileGroup]  WITH CHECK ADD  CONSTRAINT [FK_awFileGroup_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awFileGroup] CHECK CONSTRAINT [FK_awFileGroup_awUser]
GO
/****** Object:  ForeignKey [FK_awFileInGroup_awFile]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFileInGroup]  WITH CHECK ADD  CONSTRAINT [FK_awFileInGroup_awFile] FOREIGN KEY([fileId])
REFERENCES [dbo].[awFile] ([fileId])
GO
ALTER TABLE [dbo].[awFileInGroup] CHECK CONSTRAINT [FK_awFileInGroup_awFile]
GO
/****** Object:  ForeignKey [FK_awFileInGroup_awFileGroup]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awFileInGroup]  WITH CHECK ADD  CONSTRAINT [FK_awFileInGroup_awFileGroup] FOREIGN KEY([fileGroupId])
REFERENCES [dbo].[awFileGroup] ([fileGroupId])
GO
ALTER TABLE [dbo].[awFileInGroup] CHECK CONSTRAINT [FK_awFileInGroup_awFileGroup]
GO
/****** Object:  ForeignKey [FK_awPage_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPage]  WITH CHECK ADD  CONSTRAINT [FK_awPage_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awPage] CHECK CONSTRAINT [FK_awPage_awSite]
GO
/****** Object:  ForeignKey [FK_awPage_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPage]  WITH CHECK ADD  CONSTRAINT [FK_awPage_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awPage] CHECK CONSTRAINT [FK_awPage_awUser]
GO
/****** Object:  ForeignKey [FK_awPoll_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPoll]  WITH CHECK ADD  CONSTRAINT [FK_awPoll_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awPoll] CHECK CONSTRAINT [FK_awPoll_awSite]
GO
/****** Object:  ForeignKey [FK_awPoll_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPoll]  WITH CHECK ADD  CONSTRAINT [FK_awPoll_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awPoll] CHECK CONSTRAINT [FK_awPoll_awUser]
GO
/****** Object:  ForeignKey [FK_awPollChoice_awPoll]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awPollChoice]  WITH CHECK ADD  CONSTRAINT [FK_awPollChoice_awPoll] FOREIGN KEY([pollId])
REFERENCES [dbo].[awPoll] ([pollId])
GO
ALTER TABLE [dbo].[awPollChoice] CHECK CONSTRAINT [FK_awPollChoice_awPoll]
GO
/****** Object:  ForeignKey [FK_awRoleGroupMember_awRoleGroup]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRoleMember]  WITH CHECK ADD  CONSTRAINT [FK_awRoleGroupMember_awRoleGroup] FOREIGN KEY([roleId])
REFERENCES [dbo].[awRole] ([roleId])
GO
ALTER TABLE [dbo].[awRoleMember] CHECK CONSTRAINT [FK_awRoleGroupMember_awRoleGroup]
GO
/****** Object:  ForeignKey [FK_awRoleGroupMember_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRoleMember]  WITH CHECK ADD  CONSTRAINT [FK_awRoleGroupMember_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awRoleMember] CHECK CONSTRAINT [FK_awRoleGroupMember_awSite]
GO
/****** Object:  ForeignKey [FK_awRoleGroupMember_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awRoleMember]  WITH CHECK ADD  CONSTRAINT [FK_awRoleGroupMember_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awRoleMember] CHECK CONSTRAINT [FK_awRoleGroupMember_awUser]
GO
/****** Object:  ForeignKey [FK_awShareIt_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awShareIt]  WITH CHECK ADD  CONSTRAINT [FK_awShareIt_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awShareIt] CHECK CONSTRAINT [FK_awShareIt_awSite]
GO
/****** Object:  ForeignKey [FK_awShareIt_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awShareIt]  WITH CHECK ADD  CONSTRAINT [FK_awShareIt_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awShareIt] CHECK CONSTRAINT [FK_awShareIt_awUser]
GO
/****** Object:  ForeignKey [FK_awShareItWith_awShareIt]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awShareItWith]  WITH CHECK ADD  CONSTRAINT [FK_awShareItWith_awShareIt] FOREIGN KEY([shareItId])
REFERENCES [dbo].[awShareIt] ([shareItId])
GO
ALTER TABLE [dbo].[awShareItWith] CHECK CONSTRAINT [FK_awShareItWith_awShareIt]
GO
/****** Object:  ForeignKey [FK_awShareItWith_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awShareItWith]  WITH CHECK ADD  CONSTRAINT [FK_awShareItWith_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awShareItWith] CHECK CONSTRAINT [FK_awShareItWith_awUser]
GO
/****** Object:  ForeignKey [FK_awSite_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSite]  WITH CHECK ADD  CONSTRAINT [FK_awSite_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awSite] CHECK CONSTRAINT [FK_awSite_awUser]
GO
/****** Object:  ForeignKey [FK_awSiteLanguage_awLanguage]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteCulture]  WITH CHECK ADD  CONSTRAINT [FK_awSiteLanguage_awLanguage] FOREIGN KEY([cultureCode])
REFERENCES [dbo].[awCulture] ([cultureCode])
GO
ALTER TABLE [dbo].[awSiteCulture] CHECK CONSTRAINT [FK_awSiteLanguage_awLanguage]
GO
/****** Object:  ForeignKey [FK_awSiteLanguage_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteCulture]  WITH CHECK ADD  CONSTRAINT [FK_awSiteLanguage_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awSiteCulture] CHECK CONSTRAINT [FK_awSiteLanguage_awSite]
GO
/****** Object:  ForeignKey [FK_awSiteIpSecurity_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteDomainSecurity]  WITH CHECK ADD  CONSTRAINT [FK_awSiteIpSecurity_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awSiteDomainSecurity] CHECK CONSTRAINT [FK_awSiteIpSecurity_awSite]
GO
/****** Object:  ForeignKey [FK_awSiteIpSecurity_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteDomainSecurity]  WITH CHECK ADD  CONSTRAINT [FK_awSiteIpSecurity_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awSiteDomainSecurity] CHECK CONSTRAINT [FK_awSiteIpSecurity_awUser]
GO
/****** Object:  ForeignKey [FK_awSiteMap_awPage]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteMap]  WITH CHECK ADD  CONSTRAINT [FK_awSiteMap_awPage] FOREIGN KEY([pageId])
REFERENCES [dbo].[awPage] ([pageId])
GO
ALTER TABLE [dbo].[awSiteMap] CHECK CONSTRAINT [FK_awSiteMap_awPage]
GO
/****** Object:  ForeignKey [FK_awSiteMap_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteMap]  WITH CHECK ADD  CONSTRAINT [FK_awSiteMap_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awSiteMap] CHECK CONSTRAINT [FK_awSiteMap_awSite]
GO
/****** Object:  ForeignKey [FK_awSiteMap_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteMap]  WITH CHECK ADD  CONSTRAINT [FK_awSiteMap_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awSiteMap] CHECK CONSTRAINT [FK_awSiteMap_awUser]
GO
/****** Object:  ForeignKey [FK_awTag_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteTag]  WITH CHECK ADD  CONSTRAINT [FK_awTag_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awSiteTag] CHECK CONSTRAINT [FK_awTag_awSite]
GO
/****** Object:  ForeignKey [FK_awTaggedContent_awContent]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteTaggedContent]  WITH CHECK ADD  CONSTRAINT [FK_awTaggedContent_awContent] FOREIGN KEY([contentId])
REFERENCES [dbo].[awContent] ([contentId])
GO
ALTER TABLE [dbo].[awSiteTaggedContent] CHECK CONSTRAINT [FK_awTaggedContent_awContent]
GO
/****** Object:  ForeignKey [FK_awTaggedContent_awTag]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteTaggedContent]  WITH CHECK ADD  CONSTRAINT [FK_awTaggedContent_awTag] FOREIGN KEY([siteTagId])
REFERENCES [dbo].[awSiteTag] ([siteTagId])
GO
ALTER TABLE [dbo].[awSiteTaggedContent] CHECK CONSTRAINT [FK_awTaggedContent_awTag]
GO
/****** Object:  ForeignKey [FK_awSiteUser_awSite]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteUser]  WITH CHECK ADD  CONSTRAINT [FK_awSiteUser_awSite] FOREIGN KEY([siteId])
REFERENCES [dbo].[awSite] ([siteId])
GO
ALTER TABLE [dbo].[awSiteUser] CHECK CONSTRAINT [FK_awSiteUser_awSite]
GO
/****** Object:  ForeignKey [FK_awSiteUser_awUser]    Script Date: 05/07/2010 15:33:40 ******/
ALTER TABLE [dbo].[awSiteUser]  WITH CHECK ADD  CONSTRAINT [FK_awSiteUser_awUser] FOREIGN KEY([userId])
REFERENCES [dbo].[awUser] ([userId])
GO
ALTER TABLE [dbo].[awSiteUser] CHECK CONSTRAINT [FK_awSiteUser_awUser]
GO
