USE [POS_Restaurant];
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GoappRedeemLog]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[GoappRedeemLog](
        [Id] [bigint] IDENTITY(1,1) NOT NULL,
        [Waktu] [datetime] NOT NULL DEFAULT (getdate()),
        [Jenis] [varchar](20) NOT NULL,              -- 'RedeemVoucher' atau 'BurnPoint'
        [Kode] [varchar](100) NOT NULL,              -- Kode voucher atau payment_ref burn point
        [NamaVoucher] [varchar](150) NOT NULL,       -- Nama voucher atau promo / "GoappPoint"
        [NilaiPoint] [decimal](18, 2) NOT NULL DEFAULT (0), -- Nilai diskon / poin yang digunakan
        [Status] [smallint] NOT NULL DEFAULT (0),    -- 0 = Digunakan/Burn, 1 = Cancel
        [StatusPos] [smallint] NOT NULL DEFAULT (0), -- 0 = Belum masuk POS, 1 = Sudah masuk POS
        [TransactionRef] [varchar](50) NULL,         -- Nomor bill POS (SM_Number)
        [MemberUid] [varchar](50) NULL,              -- UID Member Goapp
        [MemberName] [varchar](100) NULL,            -- Nama Member Goapp
        [CreatedAt] [datetime] NOT NULL DEFAULT (getdate()),
        [UpdatedAt] [datetime] NOT NULL DEFAULT (getdate()),
        CONSTRAINT [PK_GoappRedeemLog] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE NONCLUSTERED INDEX [IX_GoappRedeemLog_StatusPos] ON [dbo].[GoappRedeemLog]([StatusPos] ASC, [Status] ASC);
    CREATE NONCLUSTERED INDEX [IX_GoappRedeemLog_TxRef] ON [dbo].[GoappRedeemLog]([TransactionRef] ASC);
    CREATE NONCLUSTERED INDEX [IX_GoappRedeemLog_Kode] ON [dbo].[GoappRedeemLog]([Kode] ASC, [Jenis] ASC);
END
GO
