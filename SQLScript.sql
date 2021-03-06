SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblSales]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblSales](
	[saleId] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [int] NULL,
	[BillDate] [datetime] NULL,
	[areaId] [int] NULL,
	[custId] [int] NULL,
	[CBalance] [float] NULL,
	[GrandTotal] [float] NULL,
	[Cash] [float] NULL,
	[Discount] [float] NULL,
	[Balance] [float] NULL,
	[updated] [bit] NULL,
	[VehicleNo] [varchar](50) NULL,
	[CGST] [decimal](18, 2) NULL,
	[SGST] [decimal](18, 2) NULL,
	[IGST] [decimal](18, 2) NULL,
	[BillType] [varchar](50) NULL,
	[InterStateBill] [bit] NULL,
 CONSTRAINT [PK_tblSales] PRIMARY KEY CLUSTERED 
(
	[saleId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblStaff]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblStaff](
	[staffid] [int] IDENTITY(1,1) NOT NULL,
	[StaffName] [varchar](150) NULL,
	[Address] [varchar](max) NULL,
	[phone] [varchar](50) NULL,
 CONSTRAINT [PK_tblStaff] PRIMARY KEY CLUSTERED 
(
	[staffid] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblTempStock]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblTempStock](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[itemId] [int] NULL,
	[ItemName] [varchar](150) NULL,
	[Qty] [float] NULL,
 CONSTRAINT [PK_tblTempStock] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblStock]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblStock](
	[stockId] [int] IDENTITY(1,1) NOT NULL,
	[staffId] [int] NULL,
	[SDate] [datetime] NULL,
	[itemid] [int] NULL,
	[Qty] [float] NULL,
 CONSTRAINT [PK_tblStock] PRIMARY KEY CLUSTERED 
(
	[stockId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblCustomer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblCustomer](
	[custId] [int] IDENTITY(1,1) NOT NULL,
	[Customer] [varchar](150) NOT NULL,
	[Tin] [varchar](50) NULL,
	[CST] [varchar](50) NULL,
	[Place] [varchar](150) NULL,
	[Phone] [varchar](50) NULL,
	[areaId] [int] NULL,
	[creditBal] [float] NULL,
	[Notes] [varchar](max) NULL,
	[OrderNo] [int] NULL,
	[Updated] [bit] NULL,
	[GSTIN] [varchar](50) NULL,
	[Address] [varchar](max) NULL,
	[State] [varchar](50) NULL,
	[State_code] [varchar](4) NULL,
 CONSTRAINT [PK_tblCustomer] PRIMARY KEY CLUSTERED 
(
	[custId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblTemp](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[itemCode] [varchar](50) NULL,
	[ItemId] [int] NULL,
	[ItemName] [varchar](150) NULL,
	[Qty] [decimal](18, 2) NULL,
	[Rate] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
	[cgstper] [float] NULL,
	[cgst] [float] NULL,
	[sgstper] [float] NULL,
	[sgst] [float] NULL,
	[igstper] [float] NULL,
	[igst] [float] NULL,
	[gst] [float] NULL,
	[disc] [float] NULL,
 CONSTRAINT [PK_tblTemp] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblItem](
	[itemId] [int] IDENTITY(1,1) NOT NULL,
	[Item_Code] [varchar](50) NULL,
	[Item_Name] [varchar](150) NULL,
	[inMalayalam] [nvarchar](max) NULL,
	[MRP] [float] NULL,
	[Rate] [float] NULL,
	[WRate] [float] NULL,
	[PRate] [float] NULL,
	[minRate] [float] NULL,
	[ReOrder] [float] NULL,
	[Opening_Stock] [float] NULL,
	[Current_Stock] [float] NULL,
	[Updated] [bit] NULL,
	[CGSTPER] [decimal](18, 2) NULL,
	[SGSTPER] [decimal](18, 2) NULL,
	[IGSTPER] [decimal](18, 2) NULL,
	[ISCGSTPER] [decimal](18, 2) NULL,
	[ISSGSTPER] [decimal](18, 2) NULL,
	[ISIGSTPER] [decimal](18, 2) NULL,
	[HSN] [varchar](50) NULL,
 CONSTRAINT [PK_tblItem] PRIMARY KEY CLUSTERED 
(
	[itemId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblArea]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblArea](
	[areaId] [int] IDENTITY(1,1) NOT NULL,
	[Area] [varchar](150) NULL,
	[Managed] [varchar](100) NULL,
	[Notes] [varchar](max) NULL,
	[Updated] [bit] NULL,
 CONSTRAINT [PK_tblArea] PRIMARY KEY CLUSTERED 
(
	[areaId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblParty]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblParty](
	[partyId] [int] IDENTITY(1,1) NOT NULL,
	[Party] [varchar](150) NOT NULL,
	[Tin] [varchar](50) NULL,
	[CST] [varchar](50) NULL,
	[Place] [varchar](150) NULL,
	[Phone] [varchar](50) NULL,
	[creditBal] [float] NULL,
	[Notes] [varchar](max) NULL,
	[Updated] [bit] NULL,
 CONSTRAINT [PK_tblParty] PRIMARY KEY CLUSTERED 
(
	[partyId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblSettings](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [int] NULL,
 CONSTRAINT [PK_tblSettings] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblSaleTrans]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblSaleTrans](
	[transId] [int] IDENTITY(1,1) NOT NULL,
	[saleId] [int] NULL,
	[itemid] [float] NULL,
	[qty] [float] NULL,
	[rate] [float] NULL,
	[total] [float] NULL,
	[Updated] [bit] NULL,
	[cgstper] [float] NULL,
	[cgst] [float] NULL,
	[sgstper] [float] NULL,
	[sgst] [float] NULL,
	[igstper] [float] NULL,
	[igst] [float] NULL,
	[gst] [float] NULL,
	[disc] [float] NULL,
 CONSTRAINT [PK_tblSaleTrans] PRIMARY KEY CLUSTERED 
(
	[transId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
