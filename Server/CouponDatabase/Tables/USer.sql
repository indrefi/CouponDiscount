IF NOT EXISTS (SELECT 0 
           FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_SCHEMA = 'User' 
           AND TABLE_NAME = 'User')
BEGIN
    CREATE TABLE [User].[User]
    (
        [UserID] INT NOT NULL IDENTITY, 
        [UserName] NVARCHAR(50) NOT NULL,
        [UserPassword] NVARCHAR(50) NOT NULL,
        [UpdatedAt] DATETIME NOT NULL DEFAULT GETDATE(),            

        CONSTRAINT [PK_User_User] PRIMARY KEY ([UserID])

    )
END

IF NOT EXISTS(SELECT 0 FROM sys.indexes WHERE name='IX_NC_User_User_UserName' AND object_id = OBJECT_ID('User.User'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_NC_User_User_UserName]
    ON [User].[User]([UserName] ASC);
END