IF NOT EXISTS (SELECT 0
               FROM INFORMATION_SCHEMA.SCHEMATA 
               WHERE schema_name='User')
BEGIN
  EXEC sp_executesql N'CREATE SCHEMA [User]';
END