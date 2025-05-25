-- migration-004.sql
-- Make ExternalId columns NOT NULL and add unique constraints

UPDATE business.Companies SET ExternalId = NEWID() WHERE ExternalId IS NULL;
UPDATE business.Addresses SET ExternalId = NEWID() WHERE ExternalId IS NULL;

ALTER TABLE business.Companies ALTER COLUMN ExternalId UNIQUEIDENTIFIER NOT NULL;
ALTER TABLE business.Addresses ALTER COLUMN ExternalId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE business.Companies ADD CONSTRAINT UQ_Companies_ExternalId UNIQUE (ExternalId);
ALTER TABLE business.Addresses ADD CONSTRAINT UQ_Addresses_ExternalId UNIQUE (ExternalId);
