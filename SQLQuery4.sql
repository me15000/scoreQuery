USE [dbltfuli]
GO
/****** Object:  Table [dbo].[school.special]    Script Date: 05/18/2018 11:36:36 ******/
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
/****** Object:  Table [dbo].[school.score]    Script Date: 05/18/2018 11:36:36 ******/
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
/****** Object:  Table [dbo].[school.data]    Script Date: 05/18/2018 11:36:36 ******/
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
	[f985] [varchar](5) NULL,
	[f211] [varchar](5) NULL,
	[level] [nvarchar](50) NULL,
	[autonomyrs] [varchar](5) NULL,
	[library] [varchar](20) NULL,
	[membership] [nvarchar](50) NULL,
	[schoolnature] [nvarchar](50) NULL,
	[shoufei] [nvarchar](500) NULL,
	[jianjie] [nvarchar](500) NULL,
	[schoolcode] [varchar](50) NULL,
	[ranking] [varchar](20) NULL,
	[rankingCollegetype] [varchar](20) NULL,
	[guanwang] [varchar](200) NULL,
	[oldname] [nvarchar](50) NULL,
	[master] [varchar](20) NULL,
	[num] [varchar](20) NULL,
	[firstrate] [nvarchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
