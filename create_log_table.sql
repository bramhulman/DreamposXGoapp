USE [POS_Restaurant];
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GoappApiLog]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[GoappApiLog](
        [Id] [bigint] IDENTITY(1,1) NOT NULL,
        [LogDate] [date] NOT NULL,
        [LogDateTime] [datetime2](7) NOT NULL,
        [HttpMethod] [varchar](10) NOT NULL,
        [EndpointUrl] [varchar](2048) NOT NULL,
        [RequestPayload] [nvarchar](max) NULL,
        [ResponsePayload] [nvarchar](max) NULL,
        [StatusCode] [int] NULL,
        [IsSuccess] [bit] NOT NULL,
        [ErrorMessage] [nvarchar](max) NULL,
        [DurationMs] [int] NOT NULL,
        [CreatedAt] [datetime] NOT NULL DEFAULT (getdate()),
    CONSTRAINT [PK_GoappApiLog] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
    
    CREATE NONCLUSTERED INDEX [IX_GoappApiLog_LogDate] ON [dbo].[GoappApiLog]
    (
        [LogDate] ASC
    );
END
