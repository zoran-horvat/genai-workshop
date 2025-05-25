-- migration-003.sql
-- Add nullable GUID columns to Companies and Addresses tables

ALTER TABLE business.Companies ADD ExternalId UNIQUEIDENTIFIER NULL;
ALTER TABLE business.Addresses ADD ExternalId UNIQUEIDENTIFIER NULL;
