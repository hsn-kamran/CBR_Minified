-- Table: public.CurrencyCourses

-- DROP TABLE IF EXISTS public."CurrencyCourses";

CREATE TABLE IF NOT EXISTS public."CurrencyCourses"
(
    "CurrencyId" text COLLATE pg_catalog."default" NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "NumCode" text COLLATE pg_catalog."default" NOT NULL,
    "CharCode" text COLLATE pg_catalog."default" NOT NULL,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    "Nominal" integer NOT NULL,
    "Value" double precision NOT NULL,
    "VunitRate" double precision NOT NULL,
    CONSTRAINT "PK_CurrencyCourses" PRIMARY KEY ("CurrencyId", "Date")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."CurrencyCourses"
    OWNER to root;
-- Index: IX_CurrencyCourses_CurrencyId_Date

-- DROP INDEX IF EXISTS public."IX_CurrencyCourses_CurrencyId_Date";

CREATE INDEX IF NOT EXISTS "IX_CurrencyCourses_CurrencyId_Date"
    ON public."CurrencyCourses" USING btree
    ("CurrencyId" COLLATE pg_catalog."default" ASC NULLS LAST, "Date" ASC NULLS LAST)
    TABLESPACE pg_default;