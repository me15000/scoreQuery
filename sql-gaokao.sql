USE [app.gaokao]
GO
/****** Object:  Table [dbo].[user.data]    Script Date: 05/29/2018 17:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[user.data](
	[id] [int] IDENTITY(10000,1) NOT NULL,
	[dk] [varchar](96) NULL,
	[date] [datetime] NULL,
 CONSTRAINT [PK_user.data] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[special.data]    Script Date: 05/29/2018 17:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[special.data](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NULL,
	[code] [varchar](50) NULL,
	[zycengci] [nvarchar](50) NULL,
	[zytype] [nvarchar](50) NULL,
	[bnum] [varchar](20) NULL,
	[znum] [varchar](20) NULL,
	[zyid] [varchar](20) NULL,
	[ranking] [varchar](20) NULL,
	[rankingType] [varchar](20) NULL,
	[des] [ntext] NULL,
	[data] [ntext] NULL,
 CONSTRAINT [PK_special.data] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[school.special.data]    Script Date: 05/29/2018 17:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[school.special.data](
	[schoolid] [int] NOT NULL,
	[specialid] [int] NOT NULL,
 CONSTRAINT [PK_school.special.data] PRIMARY KEY CLUSTERED 
(
	[schoolid] ASC,
	[specialid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[school.special]    Script Date: 05/29/2018 17:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[school.special](
	[schoolid] [int] NOT NULL,
	[provinceid] [varchar](20) NOT NULL,
	[examieeid] [varchar](20) NOT NULL,
	[specialname] [nvarchar](96) NOT NULL,
	[year] [int] NOT NULL,
	[maxfs] [varchar](20) NULL,
	[varfs] [varchar](20) NULL,
	[minfs] [varchar](20) NULL,
	[pc] [nvarchar](50) NULL,
	[stype] [nvarchar](50) NULL,
	[n_maxfs] [int] NULL,
	[n_varfs] [int] NULL,
	[n_minfs] [int] NULL,
 CONSTRAINT [PK_school.special] PRIMARY KEY CLUSTERED 
(
	[schoolid] ASC,
	[provinceid] ASC,
	[examieeid] ASC,
	[specialname] ASC,
	[year] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[school.score.last]    Script Date: 05/29/2018 17:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[school.score.last](
	[schoolid] [int] NOT NULL,
	[provinceid] [varchar](20) NOT NULL,
	[examieeid] [varchar](20) NOT NULL,
	[batchid] [varchar](20) NOT NULL,
	[year] [int] NULL,
	[n_avgScore] [int] NULL,
 CONSTRAINT [PK_school.score.last] PRIMARY KEY CLUSTERED 
(
	[schoolid] ASC,
	[provinceid] ASC,
	[examieeid] ASC,
	[batchid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[school.score]    Script Date: 05/29/2018 17:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[school.score](
	[schoolid] [int] NOT NULL,
	[provinceid] [varchar](20) NOT NULL,
	[examieeid] [varchar](20) NOT NULL,
	[batchid] [varchar](20) NOT NULL,
	[year] [int] NOT NULL,
	[maxScore] [varchar](20) NULL,
	[minScore] [varchar](20) NULL,
	[avgScore] [varchar](20) NULL,
	[ps] [varchar](20) NULL,
	[fc] [varchar](20) NULL,
	[rb] [varchar](20) NULL,
	[rs] [varchar](20) NULL,
	[ph] [varchar](20) NULL,
	[n_maxScore] [int] NULL,
	[n_minScore] [int] NULL,
	[n_avgScore] [int] NULL,
 CONSTRAINT [PK_school.score] PRIMARY KEY CLUSTERED 
(
	[schoolid] ASC,
	[provinceid] ASC,
	[examieeid] ASC,
	[batchid] ASC,
	[year] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[school.data]    Script Date: 05/29/2018 17:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[school.data](
	[schoolid] [int] NOT NULL,
	[schoolname] [nvarchar](50) NULL,
	[province] [nvarchar](50) NULL,
	[schooltype] [nvarchar](50) NULL,
	[schoolproperty] [nvarchar](50) NULL,
	[edudirectly] [varchar](20) NULL,
	[f985] [bit] NULL,
	[f211] [bit] NULL,
	[level] [nvarchar](50) NULL,
	[autonomyrs] [varchar](5) NULL,
	[library] [varchar](20) NULL,
	[membership] [nvarchar](50) NULL,
	[schoolnature] [nvarchar](50) NULL,
	[shoufei] [nvarchar](500) NULL,
	[jianjie] [nvarchar](500) NULL,
	[schoolcode] [varchar](50) NULL,
	[ranking] [int] NULL,
	[rankingCollegetype] [int] NULL,
	[guanwang] [varchar](200) NULL,
	[oldname] [nvarchar](500) NULL,
	[master] [int] NULL,
	[num] [varchar](20) NULL,
	[firstrate] [nvarchar](50) NULL,
	[des] [ntext] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[school.article]    Script Date: 05/29/2018 17:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[school.article](
	[schoolid] [int] NULL,
	[year] [int] NULL,
	[type] [nvarchar](50) NULL,
	[data] [ntext] NULL,
	[title] [nvarchar](300) NULL,
	[key] [varchar](50) NOT NULL,
 CONSTRAINT [PK_school.article] PRIMARY KEY CLUSTERED 
(
	[key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[province.score]    Script Date: 05/29/2018 17:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[province.score](
	[provinceid] [varchar](20) NOT NULL,
	[year] [int] NOT NULL,
	[batch] [nvarchar](50) NOT NULL,
	[type] [nvarchar](50) NOT NULL,
	[score] [int] NULL,
 CONSTRAINT [PK_province.score] PRIMARY KEY CLUSTERED 
(
	[provinceid] ASC,
	[year] ASC,
	[batch] ASC,
	[type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
