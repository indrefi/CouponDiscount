MERGE INTO  [User].[User] AS TARGET
USING 
(
    VALUES ('admin','d033e22ae348aeb5660fc2140aec35850c4da997', GETDATE() )
) 
AS SOURCE ([UserName], [UserPassword], [UpdatedAt])
ON TARGET.[UserName] = SOURCE.[UserName]

WHEN NOT MATCHED 
THEN
    INSERT ([UserName], [UserPassword], [UpdatedAt]) VALUES (SOURCE.[UserName], SOURCE.[UserPassword], SOURCE.[UpdatedAt])
WHEN NOT MATCHED BY SOURCE
THEN DELETE;
