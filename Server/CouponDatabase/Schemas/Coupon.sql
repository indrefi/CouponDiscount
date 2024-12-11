IF NOT EXISTS (SELECT 0
               FROM INFORMATION_SCHEMA.SCHEMATA 
               WHERE schema_name='Coupon')
BEGIN
  EXEC sp_executesql N'CREATE SCHEMA Coupon';
END