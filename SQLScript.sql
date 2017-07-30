USE [Inventory]
ALTER TABLE [dbo].[tblItem]
ALTER COLUMN [inMalayalam] [nvarchar](max) NULL

ALTER TABLE [dbo].[tblItem]
ADD 
[CGSTPER] [decimal](18, 2) NOT NULL DEFAULT 0,
[SGSTPER] [decimal](18, 2) NOT NULL DEFAULT 0,
[IGSTPER] [decimal](18, 2) NOT NULL DEFAULT 0,
[ISCGSTPER] [decimal](18, 2) NOT NULL DEFAULT 0,
[ISSGSTPER] [decimal](18, 2) NOT NULL DEFAULT 0,
[ISIGSTPER] [decimal](18, 2) NOT NULL DEFAULT 0,
[HSN] [varchar](50) NULL

ALTER TABLE tblCustomer
ADD GSTIN VARCHAR(50),
Address VARCHAR (MAX),
State	VARCHAR(50),
State_code VARCHAR(4)