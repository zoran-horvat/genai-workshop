-- migration-005.sql
-- Add AddressKind column to Addresses table as NOT NULL with default value for existing and new rows

ALTER TABLE business.Addresses ADD AddressKind INT NOT NULL DEFAULT 11;