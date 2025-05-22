-- migration-001.sql: Create Companies and Addresses tables in business schema

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'business')
BEGIN
    EXEC('CREATE SCHEMA business');
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Companies' AND xtype='U' AND uid = SCHEMA_ID('business'))
BEGIN
    CREATE TABLE business.Companies (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        TIN NVARCHAR(50) NOT NULL,
        UserId NVARCHAR(450) NOT NULL -- FK to Identity schema, but no cross-schema FK
    );
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Addresses' AND xtype='U' AND uid = SCHEMA_ID('business'))
BEGIN
    CREATE TABLE business.Addresses (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CompanyId INT NOT NULL,
        StreetAddress NVARCHAR(200) NOT NULL,
        City NVARCHAR(100) NOT NULL,
        State NVARCHAR(100) NOT NULL,
        PostalCode NVARCHAR(20) NOT NULL,
        Country NVARCHAR(100) NOT NULL,
        CONSTRAINT FK_Addresses_Companies FOREIGN KEY (CompanyId) REFERENCES business.Companies(Id)
    );
END
