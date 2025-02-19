CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "COMPANY" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "StockTicker" text NOT NULL,
    "Exchange" text NOT NULL,
    "Isin" character varying(12) NOT NULL,
    "Website" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_COMPANY" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_Company_Isin" CHECK ("Isin" ~ '^[A-Z]{2}[A-Z0-9]{9}[0-9]$')
);

CREATE TABLE "USER" (
    "Id" uuid NOT NULL,
    "Email" text NOT NULL,
    "PasswordHash" bytea NOT NULL,
    "PasswordSalt" bytea NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (NOW()),
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_USER" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_COMPANY_Isin" ON "COMPANY" ("Isin");

CREATE UNIQUE INDEX "IX_USER_Email" ON "USER" ("Email");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250219112821_Init', '9.0.2');

COMMIT;

