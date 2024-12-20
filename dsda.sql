USE [Tipografiya]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[Id_client] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[ContactDetails] [nvarchar](12) NOT NULL,
	[Preferences] [nvarchar](50) NULL,
	[Balance] [decimal](10, 0) NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id_client] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[Id_employee] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[ContactDetails] [nvarchar](12) NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id_employee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equipment]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipment](
	[Id_equipment] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Id_Type] [int] NULL,
	[LoadPercentage] [decimal](10, 0) NULL,
 CONSTRAINT [PK_Equipment] PRIMARY KEY CLUSTERED 
(
	[Id_equipment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Materials]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Materials](
	[Id_materials] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Min_kolichestvo] [int] NULL,
	[Count] [int] NULL,
	[Srok_godnosti] [date] NULL,
	[Cost] [decimal](10, 0) NULL,
 CONSTRAINT [PK_Materials] PRIMARY KEY CLUSTERED 
(
	[Id_materials] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id_notification] [int] IDENTITY(1,1) NOT NULL,
	[Id_orders] [int] NULL,
	[Id_material] [int] NULL,
	[Date] [date] NULL,
	[Message] [nvarchar](200) NULL,
	[IsRead] [bit] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[Id_notification] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order_details]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_details](
	[Id_details] [int] IDENTITY(1,1) NOT NULL,
	[Id_orders] [int] NULL,
	[Id_product] [int] NULL,
	[Quantity] [int] NULL,
	[Size] [int] NULL,
	[Color_scheme] [nvarchar](50) NULL,
	[Cost] [decimal](10, 0) NULL,
 CONSTRAINT [PK_Order_details] PRIMARY KEY CLUSTERED 
(
	[Id_details] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id_orders] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NULL,
	[Id_clients] [int] NULL,
	[Id_employee] [int] NULL,
	[Id_stages] [int] NULL,
	[Id_status] [int] NULL,
	[Total_cost] [decimal](10, 0) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id_orders] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id_product] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Cost] [decimal](10, 0) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id_product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductMaterialRequirement]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductMaterialRequirement](
	[Id_requirement] [int] IDENTITY(1,1) NOT NULL,
	[Id_product] [int] NULL,
	[Id_materials] [int] NULL,
	[Id_equipment] [int] NULL,
	[Quantity] [int] NULL,
 CONSTRAINT [PK_ProductMaterialRequirement] PRIMARY KEY CLUSTERED 
(
	[Id_requirement] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stages]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stages](
	[Id_stages] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Stages] PRIMARY KEY CLUSTERED 
(
	[Id_stages] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Id_status] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[Id_status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[Id_transactions] [int] IDENTITY(1,1) NOT NULL,
	[Id_client] [int] NULL,
	[TransactionAmount] [decimal](10, 0) NULL,
	[TransactionType] [nvarchar](50) NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id_transactions] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Type_equipment]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Type_equipment](
	[Id_type] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Type_equipment] PRIMARY KEY CLUSTERED 
(
	[Id_type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Work_report]    Script Date: 07.12.2024 13:28:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Work_report](
	[Id_work_report] [int] NOT NULL,
	[Count_finished] [int] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([Id_client], [Login], [Password], [FirstName], [LastName], [ContactDetails], [Preferences], [Balance]) VALUES (1, N'zxc', N'zxc', N'zxc', N'zxc', N'+79149894512', N'zxc', CAST(1492 AS Decimal(10, 0)))
INSERT [dbo].[Clients] ([Id_client], [Login], [Password], [FirstName], [LastName], [ContactDetails], [Preferences], [Balance]) VALUES (5, N'zxcv', N'asdf', N'', N'', N'+79149894543', N'', CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Clients] ([Id_client], [Login], [Password], [FirstName], [LastName], [ContactDetails], [Preferences], [Balance]) VALUES (6, N'help', N'zxvxc', N'', N'', N'+79149894509', N'', CAST(0 AS Decimal(10, 0)))
SET IDENTITY_INSERT [dbo].[Clients] OFF
GO
SET IDENTITY_INSERT [dbo].[Employee] ON 

INSERT [dbo].[Employee] ([Id_employee], [Login], [Password], [FirstName], [LastName], [ContactDetails]) VALUES (1, N'asd', N'asd', N'', N'', N'1421')
INSERT [dbo].[Employee] ([Id_employee], [Login], [Password], [FirstName], [LastName], [ContactDetails]) VALUES (2, N'qwer', N'qwer', N'', N'', N'15212')
INSERT [dbo].[Employee] ([Id_employee], [Login], [Password], [FirstName], [LastName], [ContactDetails]) VALUES (3, N'qwert', N'qwert', N'', N'', N'15212')
INSERT [dbo].[Employee] ([Id_employee], [Login], [Password], [FirstName], [LastName], [ContactDetails]) VALUES (4, N'qwerty', N'qwerty', N'', N'', N'15212')
SET IDENTITY_INSERT [dbo].[Employee] OFF
GO
SET IDENTITY_INSERT [dbo].[Equipment] ON 

INSERT [dbo].[Equipment] ([Id_equipment], [Name], [Id_Type], [LoadPercentage]) VALUES (1, N'УФ-лампа', 1, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Equipment] ([Id_equipment], [Name], [Id_Type], [LoadPercentage]) VALUES (2, N'Тиснильная машина', 2, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Equipment] ([Id_equipment], [Name], [Id_Type], [LoadPercentage]) VALUES (3, N'Сканер', 3, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Equipment] ([Id_equipment], [Name], [Id_Type], [LoadPercentage]) VALUES (4, N'Компьютерные рабочие станции', 4, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Equipment] ([Id_equipment], [Name], [Id_Type], [LoadPercentage]) VALUES (5, N'Клеевое оборудование', 5, CAST(10 AS Decimal(10, 0)))
INSERT [dbo].[Equipment] ([Id_equipment], [Name], [Id_Type], [LoadPercentage]) VALUES (6, N'Сушильные машины', 6, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Equipment] ([Id_equipment], [Name], [Id_Type], [LoadPercentage]) VALUES (1002, N'Цифровый цветной принтер', 1003, CAST(32 AS Decimal(10, 0)))
INSERT [dbo].[Equipment] ([Id_equipment], [Name], [Id_Type], [LoadPercentage]) VALUES (1003, N'Профессиональная ламинирующая машина', 1004, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Equipment] ([Id_equipment], [Name], [Id_Type], [LoadPercentage]) VALUES (1004, N'Профессиональный ручной резак для бумаги', 1005, CAST(20 AS Decimal(10, 0)))
SET IDENTITY_INSERT [dbo].[Equipment] OFF
GO
SET IDENTITY_INSERT [dbo].[Materials] ON 

INSERT [dbo].[Materials] ([Id_materials], [Name], [Min_kolichestvo], [Count], [Srok_godnosti], [Cost]) VALUES (1, N'Бумага', 20, 994, CAST(N'2024-12-30' AS Date), CAST(5 AS Decimal(10, 0)))
INSERT [dbo].[Materials] ([Id_materials], [Name], [Min_kolichestvo], [Count], [Srok_godnosti], [Cost]) VALUES (2, N'Чернила', 10, 880, CAST(N'2024-12-30' AS Date), CAST(10 AS Decimal(10, 0)))
INSERT [dbo].[Materials] ([Id_materials], [Name], [Min_kolichestvo], [Count], [Srok_godnosti], [Cost]) VALUES (3, N'Клей', 10, 1000, CAST(N'2024-12-30' AS Date), CAST(10 AS Decimal(10, 0)))
INSERT [dbo].[Materials] ([Id_materials], [Name], [Min_kolichestvo], [Count], [Srok_godnosti], [Cost]) VALUES (4, N'Шаблоны', 10, 994, CAST(N'2024-12-30' AS Date), CAST(10 AS Decimal(10, 0)))
INSERT [dbo].[Materials] ([Id_materials], [Name], [Min_kolichestvo], [Count], [Srok_godnosti], [Cost]) VALUES (5, N'Пленка', 10, 1000, CAST(N'2024-12-30' AS Date), CAST(10 AS Decimal(10, 0)))
INSERT [dbo].[Materials] ([Id_materials], [Name], [Min_kolichestvo], [Count], [Srok_godnosti], [Cost]) VALUES (6, N'Фольга', 10, 1000, CAST(N'2024-12-30' AS Date), CAST(10 AS Decimal(10, 0)))
SET IDENTITY_INSERT [dbo].[Materials] OFF
GO
SET IDENTITY_INSERT [dbo].[Notification] ON 

INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (1, 1, NULL, CAST(N'2024-12-04' AS Date), N'Ваш заказ успешно оформлен. Статус: ', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (2, 1, NULL, CAST(N'2024-12-04' AS Date), N'Вам назначен новый заказ от zxc zxc. Этап обработки: Новый заказ.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (3, 2, NULL, CAST(N'2024-12-04' AS Date), N'Ваш заказ успешно оформлен. Статус: ', 1)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (4, 2, NULL, CAST(N'2024-12-04' AS Date), N'Вам назначен новый заказ от zxc zxc. Этап обработки: Новый заказ.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (5, 3, NULL, CAST(N'2024-12-04' AS Date), N'Ваш заказ успешно оформлен. Статус: ', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (6, 3, NULL, CAST(N'2024-12-04' AS Date), N'Вам назначен новый заказ от zxc zxc. Этап обработки: Новый заказ.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (7, 4, NULL, CAST(N'2024-12-04' AS Date), N'Ваш заказ успешно оформлен. Статус: ', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (8, 4, NULL, CAST(N'2024-12-04' AS Date), N'Вам назначен новый заказ от zxc zxc. Этап обработки: Новый заказ.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (9, 5, NULL, CAST(N'2024-12-04' AS Date), N'Ваш заказ успешно оформлен. Статус: ', 1)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (10, 5, NULL, CAST(N'2024-12-04' AS Date), N'Вам назначен новый заказ от zxc zxc. Этап обработки: Новый заказ.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (11, 6, NULL, CAST(N'2024-12-04' AS Date), N'Ваш заказ успешно оформлен. Статус: ', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (12, 6, NULL, CAST(N'2024-12-04' AS Date), N'Вам назначен новый заказ от zxc zxc. Этап обработки: Новый заказ.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (13, NULL, 2, CAST(N'2024-12-04' AS Date), N'ВНИМАНИЕ: Минимальный запас материала ''Чернила'' достигнут.', 1)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (14, 7, NULL, CAST(N'2024-12-06' AS Date), N'Ваш заказ успешно оформлен. Статус: ', 1)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (15, 7, NULL, CAST(N'2024-12-06' AS Date), N'Вам назначен новый заказ от zxc zxc. Этап обработки: Новый заказ.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (16, NULL, 1, CAST(N'2024-12-06' AS Date), N'ВНИМАНИЕ: Минимальный запас материала ''Бумага'' достигнут.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (17, NULL, 3, CAST(N'2024-12-06' AS Date), N'ВНИМАНИЕ: Минимальный запас материала ''Клей'' достигнут.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (18, NULL, 2, CAST(N'2024-12-06' AS Date), N'ВНИМАНИЕ: Минимальный запас материала ''Чернила'' достигнут.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (19, 8, NULL, CAST(N'2024-12-06' AS Date), N'Ваш заказ успешно оформлен. Статус: ', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (20, 8, NULL, CAST(N'2024-12-06' AS Date), N'Вам назначен новый заказ от zxc zxc. Этап обработки: Новый заказ.', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (21, 9, NULL, CAST(N'2024-12-06' AS Date), N'Ваш заказ успешно оформлен. Статус: ', 0)
INSERT [dbo].[Notification] ([Id_notification], [Id_orders], [Id_material], [Date], [Message], [IsRead]) VALUES (22, 9, NULL, CAST(N'2024-12-06' AS Date), N'Вам назначен новый заказ от zxc zxc. Этап обработки: Новый заказ.', 0)
SET IDENTITY_INSERT [dbo].[Notification] OFF
GO
SET IDENTITY_INSERT [dbo].[Order_details] ON 

INSERT [dbo].[Order_details] ([Id_details], [Id_orders], [Id_product], [Quantity], [Size], [Color_scheme], [Cost]) VALUES (1, 5, 1, 2, 21, N'Кра', CAST(450 AS Decimal(10, 0)))
INSERT [dbo].[Order_details] ([Id_details], [Id_orders], [Id_product], [Quantity], [Size], [Color_scheme], [Cost]) VALUES (2, 5, 2, 5, 21, N'Кра', CAST(750 AS Decimal(10, 0)))
INSERT [dbo].[Order_details] ([Id_details], [Id_orders], [Id_product], [Quantity], [Size], [Color_scheme], [Cost]) VALUES (3, 6, 4, 1, 21, N'Синий', CAST(460 AS Decimal(10, 0)))
INSERT [dbo].[Order_details] ([Id_details], [Id_orders], [Id_product], [Quantity], [Size], [Color_scheme], [Cost]) VALUES (4, 7, 4, 1, 23, N'вы', CAST(460 AS Decimal(10, 0)))
INSERT [dbo].[Order_details] ([Id_details], [Id_orders], [Id_product], [Quantity], [Size], [Color_scheme], [Cost]) VALUES (5, 8, 1, 2, 21, N'ывф', CAST(450 AS Decimal(10, 0)))
INSERT [dbo].[Order_details] ([Id_details], [Id_orders], [Id_product], [Quantity], [Size], [Color_scheme], [Cost]) VALUES (6, 9, 1, 2, 21, N'фыв', CAST(450 AS Decimal(10, 0)))
INSERT [dbo].[Order_details] ([Id_details], [Id_orders], [Id_product], [Quantity], [Size], [Color_scheme], [Cost]) VALUES (7, 9, 1, 2, 21, N'фыв', CAST(450 AS Decimal(10, 0)))
SET IDENTITY_INSERT [dbo].[Order_details] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([Id_orders], [Date], [Id_clients], [Id_employee], [Id_stages], [Id_status], [Total_cost]) VALUES (1, CAST(N'2024-12-04' AS Date), 1, 1, 2, 1, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Orders] ([Id_orders], [Date], [Id_clients], [Id_employee], [Id_stages], [Id_status], [Total_cost]) VALUES (2, CAST(N'2024-12-04' AS Date), 1, 1, 6, 6, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Orders] ([Id_orders], [Date], [Id_clients], [Id_employee], [Id_stages], [Id_status], [Total_cost]) VALUES (3, CAST(N'2024-12-04' AS Date), 1, 1, 6, 3, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Orders] ([Id_orders], [Date], [Id_clients], [Id_employee], [Id_stages], [Id_status], [Total_cost]) VALUES (4, CAST(N'2024-12-04' AS Date), 1, 4, 1, 3, CAST(0 AS Decimal(10, 0)))
INSERT [dbo].[Orders] ([Id_orders], [Date], [Id_clients], [Id_employee], [Id_stages], [Id_status], [Total_cost]) VALUES (5, CAST(N'2024-12-04' AS Date), 1, 1, 1, 1, CAST(1200 AS Decimal(10, 0)))
INSERT [dbo].[Orders] ([Id_orders], [Date], [Id_clients], [Id_employee], [Id_stages], [Id_status], [Total_cost]) VALUES (6, CAST(N'2024-12-04' AS Date), 1, 2, 1, 2, CAST(460 AS Decimal(10, 0)))
INSERT [dbo].[Orders] ([Id_orders], [Date], [Id_clients], [Id_employee], [Id_stages], [Id_status], [Total_cost]) VALUES (7, CAST(N'2024-12-06' AS Date), 1, 3, 1, 2, CAST(460 AS Decimal(10, 0)))
INSERT [dbo].[Orders] ([Id_orders], [Date], [Id_clients], [Id_employee], [Id_stages], [Id_status], [Total_cost]) VALUES (8, CAST(N'2024-12-06' AS Date), 1, 3, 1, 2, CAST(450 AS Decimal(10, 0)))
INSERT [dbo].[Orders] ([Id_orders], [Date], [Id_clients], [Id_employee], [Id_stages], [Id_status], [Total_cost]) VALUES (9, CAST(N'2024-12-06' AS Date), 1, 4, 5, 6, CAST(900 AS Decimal(10, 0)))
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([Id_product], [Name], [Cost]) VALUES (1, N'Визитка', CAST(225 AS Decimal(10, 0)))
INSERT [dbo].[Product] ([Id_product], [Name], [Cost]) VALUES (2, N'Брошюры', CAST(150 AS Decimal(10, 0)))
INSERT [dbo].[Product] ([Id_product], [Name], [Cost]) VALUES (3, N'Плакаты', CAST(265 AS Decimal(10, 0)))
INSERT [dbo].[Product] ([Id_product], [Name], [Cost]) VALUES (4, N'Книги', CAST(460 AS Decimal(10, 0)))
INSERT [dbo].[Product] ([Id_product], [Name], [Cost]) VALUES (5, N'Флаеры', CAST(115 AS Decimal(10, 0)))
INSERT [dbo].[Product] ([Id_product], [Name], [Cost]) VALUES (6, N'Этикетки', CAST(65 AS Decimal(10, 0)))
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductMaterialRequirement] ON 

INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (1, 1, 2, 1002, 20)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (2, 1, 1, 1004, 1)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (3, 1, 4, 1003, 1)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (4, 2, 1, 1004, 4)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (5, 2, 2, 1002, 10)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (6, 3, 1, 1004, 1)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (7, 3, 2, 1002, 25)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (8, 4, 1, 1004, 40)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (9, 2, 3, 5, 2)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (10, 4, 3, 5, 20)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (11, 5, 1, 1004, 1)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (12, 5, 2, 1002, 10)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (13, 6, 1, 1004, 1)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (14, 6, 2, 1002, 5)
INSERT [dbo].[ProductMaterialRequirement] ([Id_requirement], [Id_product], [Id_materials], [Id_equipment], [Quantity]) VALUES (15, 4, 2, 1002, 5)
SET IDENTITY_INSERT [dbo].[ProductMaterialRequirement] OFF
GO
SET IDENTITY_INSERT [dbo].[Stages] ON 

INSERT [dbo].[Stages] ([Id_stages], [Name]) VALUES (1, N'Приём')
INSERT [dbo].[Stages] ([Id_stages], [Name]) VALUES (2, N'Дизайн')
INSERT [dbo].[Stages] ([Id_stages], [Name]) VALUES (3, N'Печать')
INSERT [dbo].[Stages] ([Id_stages], [Name]) VALUES (4, N'Пост-обработка')
INSERT [dbo].[Stages] ([Id_stages], [Name]) VALUES (5, N'Упаковка')
INSERT [dbo].[Stages] ([Id_stages], [Name]) VALUES (6, N'Выдача')
SET IDENTITY_INSERT [dbo].[Stages] OFF
GO
SET IDENTITY_INSERT [dbo].[Status] ON 

INSERT [dbo].[Status] ([Id_status], [Name]) VALUES (1, N'Отказ')
INSERT [dbo].[Status] ([Id_status], [Name]) VALUES (2, N'Принят')
INSERT [dbo].[Status] ([Id_status], [Name]) VALUES (3, N'Выполняется')
INSERT [dbo].[Status] ([Id_status], [Name]) VALUES (4, N'В пути')
INSERT [dbo].[Status] ([Id_status], [Name]) VALUES (5, N'Доставлен')
INSERT [dbo].[Status] ([Id_status], [Name]) VALUES (6, N'Выполнен')
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
SET IDENTITY_INSERT [dbo].[Transactions] ON 

INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (1, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (2, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (3, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (4, 1, CAST(123 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (5, 1, CAST(123 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (6, 1, CAST(123 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (7, 1, CAST(123 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (8, 1, CAST(123 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (9, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (10, 1, CAST(123 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (11, 1, CAST(-123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (12, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (13, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (14, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (15, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (16, 1, CAST(-123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (17, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (19, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (20, 1, CAST(123 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (21, 1, CAST(123 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (22, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (23, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (24, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (25, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (26, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (27, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (28, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (29, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (30, 1, CAST(12 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (31, 1, CAST(511 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (32, 1, CAST(421 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (33, 1, CAST(421 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (34, 1, CAST(-361 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (35, 1, CAST(-1 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (36, 1, CAST(-1 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (37, 1, CAST(-1 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (38, 1, CAST(4213 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (39, 1, CAST(352 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (40, 1, CAST(5444 AS Decimal(10, 0)), N'Пополнение')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (41, 1, CAST(23 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (42, 1, CAST(23 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (43, 1, CAST(23 AS Decimal(10, 0)), N'Вывод')
INSERT [dbo].[Transactions] ([Id_transactions], [Id_client], [TransactionAmount], [TransactionType]) VALUES (44, 1, CAST(23 AS Decimal(10, 0)), N'Вывод')
SET IDENTITY_INSERT [dbo].[Transactions] OFF
GO
SET IDENTITY_INSERT [dbo].[Type_equipment] ON 

INSERT [dbo].[Type_equipment] ([Id_type], [Name]) VALUES (1, N'УФ-отверждающая')
INSERT [dbo].[Type_equipment] ([Id_type], [Name]) VALUES (2, N'Тиснильная')
INSERT [dbo].[Type_equipment] ([Id_type], [Name]) VALUES (3, N'Сканирующее')
INSERT [dbo].[Type_equipment] ([Id_type], [Name]) VALUES (4, N'Компьютерное')
INSERT [dbo].[Type_equipment] ([Id_type], [Name]) VALUES (5, N'Клеевое')
INSERT [dbo].[Type_equipment] ([Id_type], [Name]) VALUES (6, N'Сушильная')
INSERT [dbo].[Type_equipment] ([Id_type], [Name]) VALUES (1003, N'Принтер')
INSERT [dbo].[Type_equipment] ([Id_type], [Name]) VALUES (1004, N'Ламинирующая машина')
INSERT [dbo].[Type_equipment] ([Id_type], [Name]) VALUES (1005, N'Резак')
SET IDENTITY_INSERT [dbo].[Type_equipment] OFF
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [FK_Equipment_Type_equipment] FOREIGN KEY([Id_Type])
REFERENCES [dbo].[Type_equipment] ([Id_type])
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [FK_Equipment_Type_equipment]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Orders] FOREIGN KEY([Id_orders])
REFERENCES [dbo].[Orders] ([Id_orders])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Orders]
GO
ALTER TABLE [dbo].[Order_details]  WITH CHECK ADD  CONSTRAINT [FK_Order_details_Orders] FOREIGN KEY([Id_orders])
REFERENCES [dbo].[Orders] ([Id_orders])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order_details] CHECK CONSTRAINT [FK_Order_details_Orders]
GO
ALTER TABLE [dbo].[Order_details]  WITH CHECK ADD  CONSTRAINT [FK_Order_details_Product] FOREIGN KEY([Id_product])
REFERENCES [dbo].[Product] ([Id_product])
GO
ALTER TABLE [dbo].[Order_details] CHECK CONSTRAINT [FK_Order_details_Product]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Clients] FOREIGN KEY([Id_clients])
REFERENCES [dbo].[Clients] ([Id_client])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Clients]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Employee] FOREIGN KEY([Id_employee])
REFERENCES [dbo].[Employee] ([Id_employee])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Employee]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Stages] FOREIGN KEY([Id_stages])
REFERENCES [dbo].[Stages] ([Id_stages])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Stages]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Status] FOREIGN KEY([Id_status])
REFERENCES [dbo].[Status] ([Id_status])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Status]
GO
ALTER TABLE [dbo].[ProductMaterialRequirement]  WITH CHECK ADD  CONSTRAINT [FK_ProductMaterialRequirement_Equipment] FOREIGN KEY([Id_equipment])
REFERENCES [dbo].[Equipment] ([Id_equipment])
GO
ALTER TABLE [dbo].[ProductMaterialRequirement] CHECK CONSTRAINT [FK_ProductMaterialRequirement_Equipment]
GO
ALTER TABLE [dbo].[ProductMaterialRequirement]  WITH CHECK ADD  CONSTRAINT [FK_ProductMaterialRequirement_Materials] FOREIGN KEY([Id_materials])
REFERENCES [dbo].[Materials] ([Id_materials])
GO
ALTER TABLE [dbo].[ProductMaterialRequirement] CHECK CONSTRAINT [FK_ProductMaterialRequirement_Materials]
GO
ALTER TABLE [dbo].[ProductMaterialRequirement]  WITH CHECK ADD  CONSTRAINT [FK_ProductMaterialRequirement_Product] FOREIGN KEY([Id_product])
REFERENCES [dbo].[Product] ([Id_product])
GO
ALTER TABLE [dbo].[ProductMaterialRequirement] CHECK CONSTRAINT [FK_ProductMaterialRequirement_Product]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Clients] FOREIGN KEY([Id_client])
REFERENCES [dbo].[Clients] ([Id_client])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Clients]
GO
