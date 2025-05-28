-- migration-006.sql

ALTER TABLE business.Companies ADD CompanyType VARCHAR(32) NOT NULL DEFAULT 'OwnedCompany';
