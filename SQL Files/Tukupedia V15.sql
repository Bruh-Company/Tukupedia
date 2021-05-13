--------------------------------------------------------
--  File created - Thursday-May-13-2021   
--------------------------------------------------------
DROP TABLE "CATEGORY" cascade constraints;
DROP TABLE "CUSTOMER" cascade constraints;
DROP TABLE "D_DISKUSI" cascade constraints;
DROP TABLE "D_TRANS_ITEM" cascade constraints;
DROP TABLE "H_DISKUSI" cascade constraints;
DROP TABLE "H_TRANS_ITEM" cascade constraints;
DROP TABLE "ITEM" cascade constraints;
DROP TABLE "JENIS_PROMO" cascade constraints;
DROP TABLE "KONTRAK_OS" cascade constraints;
DROP TABLE "KURIR" cascade constraints;
DROP TABLE "METODE_PEMBAYARAN" cascade constraints;
DROP TABLE "PROMO" cascade constraints;
DROP TABLE "SELLER" cascade constraints;
DROP TABLE "TRANS_OS" cascade constraints;
DROP TABLE "ULASAN" cascade constraints;
--------------------------------------------------------
--  DDL for Table CATEGORY
--------------------------------------------------------

  CREATE TABLE "CATEGORY" ("ID" NUMBER, "KODE" VARCHAR2(32), "NAMA" VARCHAR2(30), "STATUS" CHAR(1) DEFAULT 1, "CREATED_AT" DATE DEFAULT sysdate)
--------------------------------------------------------
--  DDL for Table CUSTOMER
--------------------------------------------------------

  CREATE TABLE "CUSTOMER" ("ID" NUMBER, "EMAIL" VARCHAR2(100), "NAMA" VARCHAR2(100), "TANGGAL_LAHIR" DATE, "ALAMAT" VARCHAR2(100), "NO_TELP" VARCHAR2(20), "SALDO" NUMBER DEFAULT 0, "KODE" VARCHAR2(32), "STATUS" CHAR(1) DEFAULT 1, "PASSWORD" VARCHAR2(64), "IMAGE" VARCHAR2(255), "CREATED_AT" DATE DEFAULT sysdate)
--------------------------------------------------------
--  DDL for Table D_DISKUSI
--------------------------------------------------------

  CREATE TABLE "D_DISKUSI" ("ID" NUMBER, "ID_SELLER" NUMBER, "ID_CUSTOMER" NUMBER, "MESSAGE" VARCHAR2(255), "STATUS" CHAR(1) DEFAULT 1, "CREATED_AT" DATE DEFAULT sysdate, "SENDER" CHAR(1)) 
 

   COMMENT ON COLUMN "D_DISKUSI"."SENDER" IS 'Jika sender adalah customer maka ''C''
Jika senfer adalah seller maka ''S'''
--------------------------------------------------------
--  DDL for Table D_TRANS_ITEM
--------------------------------------------------------

  CREATE TABLE "D_TRANS_ITEM" ("ID" NUMBER, "ID_HTRANS" NUMBER, "ID_ITEM" NUMBER, "JUMLAH" NUMBER, "STATUS" CHAR(1) DEFAULT 'W')
--------------------------------------------------------
--  DDL for Table H_DISKUSI
--------------------------------------------------------

  CREATE TABLE "H_DISKUSI" ("ID" NUMBER, "ID_CUSTOMER" NUMBER, "MESSAGE" VARCHAR2(255), "ID_ITEM" NUMBER, "STATUS" CHAR(1) DEFAULT 1, "CREATED_AT" DATE DEFAULT sysdate)
--------------------------------------------------------
--  DDL for Table H_TRANS_ITEM
--------------------------------------------------------

  CREATE TABLE "H_TRANS_ITEM" ("ID" NUMBER, "KODE" VARCHAR2(32), "TANGGAL_TRANSAKSI" DATE DEFAULT sysdate, "ID_CUSTOMER" NUMBER, "ID_KURIR" NUMBER, "STATUS" CHAR(1) DEFAULT 'W')
--------------------------------------------------------
--  DDL for Table ITEM
--------------------------------------------------------

  CREATE TABLE "ITEM" ("ID" NUMBER, "KODE" VARCHAR2(32), "ID_CATEGORY" NUMBER, "HARGA" NUMBER, "STATUS" CHAR(1) DEFAULT 1, "NAMA" VARCHAR2(100), "DESKRIPSI" VARCHAR2(1000), "ID_SELLER" NUMBER, "BERAT" NUMBER, "STOK" NUMBER DEFAULT 0, "RATING" NUMBER DEFAULT 0, "IMAGE" VARCHAR2(255), "CREATED_AT" DATE DEFAULT sysdate)
--------------------------------------------------------
--  DDL for Table JENIS_PROMO
--------------------------------------------------------

  CREATE TABLE "JENIS_PROMO" ("ID" NUMBER, "NAMA" VARCHAR2(255), "ID_CATEGORY" NUMBER, "ID_KURIR" NUMBER, "ID_SELLER" NUMBER, "ID_METODE_PEMBAYARAN" NUMBER, "STATUS" CHAR(1) DEFAULT 1, "CREATED_AT" DATE DEFAULT sysdate)
--------------------------------------------------------
--  DDL for Table KONTRAK_OS
--------------------------------------------------------

  CREATE TABLE "KONTRAK_OS" ("ID" NUMBER, "ID_SELLER" NUMBER, "CREATED_AT" DATE DEFAULT sysdate, "JANGKA_WAKTU" NUMBER, "STATUS" CHAR(1) DEFAULT 1)
--------------------------------------------------------
--  DDL for Table KURIR
--------------------------------------------------------

  CREATE TABLE "KURIR" ("ID" NUMBER, "KODE" VARCHAR2(32), "NAMA" VARCHAR2(30), "HARGA" NUMBER, "STATUS" CHAR(1) DEFAULT 1, "CREATED_AT" DATE DEFAULT sysdate) 
 

   COMMENT ON COLUMN "KURIR"."HARGA" IS 'Harga/KM'
--------------------------------------------------------
--  DDL for Table METODE_PEMBAYARAN
--------------------------------------------------------

  CREATE TABLE "METODE_PEMBAYARAN" ("ID" NUMBER, "NAMA" VARCHAR2(255), "STATUS" CHAR(1) DEFAULT 1, "CREATED_AT" DATE DEFAULT sysdate)
--------------------------------------------------------
--  DDL for Table PROMO
--------------------------------------------------------

  CREATE TABLE "PROMO" ("ID" NUMBER, "KODE" VARCHAR2(32), "POTONGAN" NUMBER, "POTONGAN_MAKS" NUMBER, "HARGA_MIN" NUMBER, "JENIS_POTONGAN" CHAR(1), "ID_JENIS_PROMO" NUMBER, "TANGGAL_AWAL" DATE, "TANGGAL_AKHIR" DATE, "STATUS" CHAR(1) DEFAULT 1, "CREATED_AT" DATE DEFAULT sysdate)
--------------------------------------------------------
--  DDL for Table SELLER
--------------------------------------------------------

  CREATE TABLE "SELLER" ("ID" NUMBER, "KODE" VARCHAR2(32), "EMAIL" VARCHAR2(30), "NAMA_TOKO" VARCHAR2(100), "ALAMAT" VARCHAR2(30), "NO_TELP" VARCHAR2(30), "SALDO" NUMBER DEFAULT 0, "IS_OFFICIAL" CHAR(1), "STATUS" CHAR(1) DEFAULT 1, "PASSWORD" VARCHAR2(64), "CREATED_AT" DATE DEFAULT sysdate, "NAMA_SELLER" VARCHAR2(100), "NIK" NUMBER)
--------------------------------------------------------
--  DDL for Table TRANS_OS
--------------------------------------------------------

  CREATE TABLE "TRANS_OS" ("ID" NUMBER, "KODE" VARCHAR2(32), "TANGGAL_TRANSAKSI" DATE DEFAULT sysdate, "ID_KONTRAK" NUMBER, "STATUS" CHAR(1) DEFAULT 1)
--------------------------------------------------------
--  DDL for Table ULASAN
--------------------------------------------------------

  CREATE TABLE "ULASAN" ("ID" NUMBER, "ID_CUSTOMER" NUMBER, "REPLY" NUMBER, "MESSAGE" VARCHAR2(255) DEFAULT '', "RATING" NUMBER, "STATUS" CHAR(1) DEFAULT 1, "CREATED_AT" DATE DEFAULT sysdate, "ID_H_TRANS" NUMBER)
REM INSERTING into CATEGORY
SET DEFINE OFF;
REM INSERTING into CUSTOMER
SET DEFINE OFF;
REM INSERTING into D_DISKUSI
SET DEFINE OFF;
REM INSERTING into D_TRANS_ITEM
SET DEFINE OFF;
REM INSERTING into H_DISKUSI
SET DEFINE OFF;
REM INSERTING into H_TRANS_ITEM
SET DEFINE OFF;
REM INSERTING into ITEM
SET DEFINE OFF;
REM INSERTING into JENIS_PROMO
SET DEFINE OFF;
REM INSERTING into KONTRAK_OS
SET DEFINE OFF;
REM INSERTING into KURIR
SET DEFINE OFF;
REM INSERTING into METODE_PEMBAYARAN
SET DEFINE OFF;
REM INSERTING into PROMO
SET DEFINE OFF;
REM INSERTING into SELLER
SET DEFINE OFF;
REM INSERTING into TRANS_OS
SET DEFINE OFF;
REM INSERTING into ULASAN
SET DEFINE OFF;
--------------------------------------------------------
--  DDL for Index KURIR_KODE_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "KURIR_KODE_UINDEX" ON "KURIR" ("KODE")
--------------------------------------------------------
--  DDL for Index TRANS_OS_KODE_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "TRANS_OS_KODE_UINDEX" ON "TRANS_OS" ("KODE")
--------------------------------------------------------
--  DDL for Index METODE_PEMBAYARAN_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "METODE_PEMBAYARAN_ID_UINDEX" ON "METODE_PEMBAYARAN" ("ID")
--------------------------------------------------------
--  DDL for Index KURIR_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "KURIR_ID_UINDEX" ON "KURIR" ("ID")
--------------------------------------------------------
--  DDL for Index D_TRANS_ITEM_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "D_TRANS_ITEM_ID_UINDEX" ON "D_TRANS_ITEM" ("ID")
--------------------------------------------------------
--  DDL for Index SELLER_EMAIL_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "SELLER_EMAIL_UINDEX" ON "SELLER" ("EMAIL")
--------------------------------------------------------
--  DDL for Index JENIS_PROMO_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "JENIS_PROMO_ID_UINDEX" ON "JENIS_PROMO" ("ID")
--------------------------------------------------------
--  DDL for Index SELLER_KODE_SELLER_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "SELLER_KODE_SELLER_UINDEX" ON "SELLER" ("KODE")
--------------------------------------------------------
--  DDL for Index ULASAN_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "ULASAN_ID_UINDEX" ON "ULASAN" ("ID")
--------------------------------------------------------
--  DDL for Index DISKUSI_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "DISKUSI_ID_UINDEX" ON "H_DISKUSI" ("ID")
--------------------------------------------------------
--  DDL for Index PROMO_KODE_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "PROMO_KODE_UINDEX" ON "PROMO" ("KODE")
--------------------------------------------------------
--  DDL for Index CUSTOMER_EMAIL_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "CUSTOMER_EMAIL_UINDEX" ON "CUSTOMER" ("EMAIL")
--------------------------------------------------------
--  DDL for Index H_TRANS_ITEM_KODE_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "H_TRANS_ITEM_KODE_UINDEX" ON "H_TRANS_ITEM" ("KODE")
--------------------------------------------------------
--  DDL for Index KATEGORI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KATEGORI_PK" ON "CATEGORY" ("ID")
--------------------------------------------------------
--  DDL for Index H_TRANS_ITEM_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "H_TRANS_ITEM_ID_UINDEX" ON "H_TRANS_ITEM" ("ID")
--------------------------------------------------------
--  DDL for Index CATEGORY_KODE_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "CATEGORY_KODE_UINDEX" ON "CATEGORY" ("KODE")
--------------------------------------------------------
--  DDL for Index D_DISKUSI_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "D_DISKUSI_ID_UINDEX" ON "D_DISKUSI" ("ID")
--------------------------------------------------------
--  DDL for Index SELLER_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "SELLER_PK" ON "SELLER" ("ID")
--------------------------------------------------------
--  DDL for Index CUSTOMER_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "CUSTOMER_PK" ON "CUSTOMER" ("ID")
--------------------------------------------------------
--  DDL for Index CUSTOMER_KODE_CUSTOMER_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "CUSTOMER_KODE_CUSTOMER_UINDEX" ON "CUSTOMER" ("KODE")
--------------------------------------------------------
--  DDL for Index ITEM_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ITEM_PK" ON "ITEM" ("ID")
--------------------------------------------------------
--  DDL for Index PROMO_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "PROMO_ID_UINDEX" ON "PROMO" ("ID")
--------------------------------------------------------
--  DDL for Index ITEM_KODE_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "ITEM_KODE_UINDEX" ON "ITEM" ("KODE")
--------------------------------------------------------
--  DDL for Index KONTRAK_OS_ID_UINDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "KONTRAK_OS_ID_UINDEX" ON "KONTRAK_OS" ("ID")
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_CATEGORY
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_CATEGORY" 
    FOR insert or update
    ON CATEGORY
    COMPOUND TRIGGER
    new_id number;
    new_code varchar2(30);
    jumlah number;
    old_id number := -1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select nvl(max(id), 0) + 1 into new_id from CATEGORY;
    end if;
END BEFORE STATEMENT;

    BEFORE EACH ROW IS
    BEGIN
        if :new.NAMA like '%' || ' ' || '%' then
            new_code := substr(:new.NAMA, 1, 1) || substr(:new.NAMA, instr(:new.NAMA, ' ') + 1, 1);
        else
            new_code := substr(:new.NAMA, 1, 2);
        end if;
        new_code := upper(new_code);
        if inserting then
            -- Ganti Substr berdasarkan kode
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from CATEGORY where KODE like '%' || new_code || '%';
            new_code := 'CAT' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.NAMA <> :new.NAMA then
                old_id := :old.ID;
            end if;
        end if;
    END BEFORE EACH ROW;

    AFTER EACH ROW IS
    BEGIN
        null;
    END AFTER EACH ROW;

    AFTER STATEMENT IS
    BEGIN
        if updating then
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from CATEGORY where KODE like '%' || new_code || '%';
            new_code := 'CAT' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update SELLER set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_CATEGORY;
ALTER TRIGGER "AUTO_ID_CATEGORY" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_CUSTOMER
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_CUSTOMER" 
    FOR insert or update
    ON CUSTOMER
    COMPOUND TRIGGER
    new_id number;
    new_code varchar2(30);
    jumlah number;
    old_id number := -1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select nvl(max(id), 0) + 1 into new_id from CUSTOMER;
    end if;
END BEFORE STATEMENT;

    BEFORE EACH ROW IS
    BEGIN
        if :new.nama like '%' || ' ' || '%' then
            new_code := substr(:new.nama, 1, 1) || substr(:new.nama, instr(:new.nama, ' ') + 1, 1);
        else
            new_code := substr(:new.nama, 1, 2);
        end if;
        new_code := upper(new_code);
        if inserting then
            -- Ganti Substr berdasarkan kode
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from CUSTOMER where KODE like '%' || new_code || '%';
            new_code := 'CU' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.nama <> :new.nama then
                old_id := :old.ID;
            end if;
        end if;
    END BEFORE EACH ROW;

    AFTER EACH ROW IS
    BEGIN
        null;
    END AFTER EACH ROW;

    AFTER STATEMENT IS
    BEGIN
        if updating then
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from CUSTOMER where KODE like '%' || new_code || '%';
            new_code := 'CU' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update CUSTOMER set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_CUSTOMER;
ALTER TRIGGER "AUTO_ID_CUSTOMER" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_D_DISKUSI
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_D_DISKUSI" 
    before insert
    on D_DISKUSI
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from D_DISKUSI;
    :new.ID := new_id;
END;
ALTER TRIGGER "AUTO_ID_D_DISKUSI" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_D_TRANS_ITEM
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_D_TRANS_ITEM" 
    before insert
    on D_TRANS_ITEM
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from D_TRANS_ITEM;
    :new.ID := new_id;
END;
ALTER TRIGGER "AUTO_ID_D_TRANS_ITEM" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_H_DISKUSI
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_H_DISKUSI" 
    before insert
    on H_DISKUSI
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from H_DISKUSI;
    :new.ID := new_id;
END;
ALTER TRIGGER "AUTO_ID_H_DISKUSI" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_H_TRANS_ITEM
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_H_TRANS_ITEM" 
    before insert
    on H_TRANS_ITEM
    for each row
DECLARE
    new_id number;
    new_code varchar2(30);
    jumlah number;
BEGIN
    new_code := 'HTI' || to_char(sysdate,'ddmmyyyy') || lpad(jumlah,5,'0');
    select NVL(MAX(ltrim(substr(KODE,12),'0')),0) + 1 into jumlah from H_TRANS_ITEM where KODE like '%' || new_code || '%';
    select nvl(max(id), 0) + 1 into new_id from H_TRANS_ITEM;
    :new.ID := new_id;
    :new.kode := new_code;
END;
ALTER TRIGGER "AUTO_ID_H_TRANS_ITEM" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_ITEM
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_ITEM" 
    FOR insert or update
    ON ITEM
    COMPOUND TRIGGER
    new_id number;
    new_code varchar2(30);
    jumlah number;
    old_id number := -1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select nvl(max(id), 0) + 1 into new_id from SELLER;
    end if;
END BEFORE STATEMENT;

    BEFORE EACH ROW IS
    BEGIN
        if :new.NAMA like '%' || ' ' || '%' then
            new_code := substr(:new.NAMA, 1, 1) || substr(:new.NAMA, instr(:new.NAMA, ' ') + 1, 1);
        else
            new_code := substr(:new.NAMA, 1, 2);
        end if;
        new_code := upper(new_code);
        if inserting then
            -- Ganti Substr berdasarkan kode
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from ITEM where KODE like '%' || new_code || '%';
            new_code := 'IT' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.NAMA <> :new.NAMA then
                old_id := :old.ID;
            end if;
        end if;
    END BEFORE EACH ROW;

    AFTER EACH ROW IS
    BEGIN
        null;
    END AFTER EACH ROW;

    AFTER STATEMENT IS
    BEGIN
        if updating then
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from ITEM where KODE like '%' || new_code || '%';
            new_code := 'IT' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update ITEM set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_ITEM;
ALTER TRIGGER "AUTO_ID_ITEM" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_JENIS_PROMO
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_JENIS_PROMO" 
    before insert
    on JENIS_PROMO
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from JENIS_PROMO;
    :new.ID := new_id;
END;
ALTER TRIGGER "AUTO_ID_JENIS_PROMO" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_KONTRAK_OS
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_KONTRAK_OS" 
    before insert
    on KONTRAK_OS
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from KONTRAK_OS;
    :new.ID := new_id;
END;
ALTER TRIGGER "AUTO_ID_KONTRAK_OS" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_KURIR
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_KURIR" 
    FOR insert or update
    ON KURIR
    COMPOUND TRIGGER
    new_id number;
    new_code varchar2(30);
    jumlah number;
    old_id number := -1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select nvl(max(id), 0) + 1 into new_id from KURIR;
    end if;
END BEFORE STATEMENT;

    BEFORE EACH ROW IS
    BEGIN
        if :new.NAMA like '%' || ' ' || '%' then
            new_code := substr(:new.NAMA, 1, 1) || substr(:new.NAMA, instr(:new.NAMA, ' ') + 1, 1);
        else
            new_code := substr(:new.NAMA, 1, 2);
        end if;
        new_code := upper(new_code);
        if inserting then
            -- Ganti Substr berdasarkan kode
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from KURIR where KODE like '%' || new_code || '%';
            new_code := 'KR' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.NAMA <> :new.NAMA then
                old_id := :old.ID;
            end if;
        end if;
    END BEFORE EACH ROW;

    AFTER EACH ROW IS
    BEGIN
        null;
    END AFTER EACH ROW;

    AFTER STATEMENT IS
    BEGIN
        if updating then
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from KURIR where KODE like '%' || new_code || '%';
            new_code := 'KR' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update SELLER set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_KURIR;
ALTER TRIGGER "AUTO_ID_KURIR" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_METODE_PEMBAYARAN
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_METODE_PEMBAYARAN" 
    before insert
    on METODE_PEMBAYARAN
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from METODE_PEMBAYARAN;
    :new.ID := new_id;
END;
ALTER TRIGGER "AUTO_ID_METODE_PEMBAYARAN" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_PROMO
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_PROMO" 
    before insert
    on PROMO
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from PROMO;
    :new.ID := new_id;
END;
ALTER TRIGGER "AUTO_ID_PROMO" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_SELLER
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_SELLER" 
    FOR insert or update
    ON SELLER
    COMPOUND TRIGGER
    new_id number;
    new_code varchar2(30);
    jumlah number;
    old_id number := -1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select nvl(max(id), 0) + 1 into new_id from SELLER;
    end if;
END BEFORE STATEMENT;

    BEFORE EACH ROW IS
    BEGIN
        if :new.NAMA_TOKO like '%' || ' ' || '%' then
            new_code := substr(:new.NAMA_TOKO, 1, 1) || substr(:new.NAMA_TOKO, instr(:new.NAMA_TOKO, ' ') + 1, 1);
        else
            new_code := substr(:new.NAMA_TOKO, 1, 2);
        end if;
        new_code := upper(new_code);
        if inserting then
            -- Ganti Substr berdasarkan kode
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from SELLER where KODE like '%' || new_code || '%';
            new_code := 'SE' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.NAMA_TOKO <> :new.NAMA_TOKO then
                old_id := :old.ID;
            end if;
        end if;
    END BEFORE EACH ROW;

    AFTER EACH ROW IS
    BEGIN
        null;
    END AFTER EACH ROW;

    AFTER STATEMENT IS
    BEGIN
        if updating then
            select NVL(MAX(ltrim(substr(KODE,5),'0')),0) + 1 into jumlah from SELLER where KODE like '%' || new_code || '%';
            new_code := 'SE' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update SELLER set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_SELLER;
ALTER TRIGGER "AUTO_ID_SELLER" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_TRANS_OS
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_TRANS_OS" 
    before insert
    on TRANS_OS
    for each row
DECLARE
    new_id number;
    new_code varchar2(30);
    jumlah number;
BEGIN
    new_code := 'TOS' || to_char(sysdate,'ddmmyyyy') || lpad(jumlah,5,'0');
    select NVL(MAX(ltrim(substr(KODE,12),'0')),0) + 1 into jumlah from TRANS_OS where KODE like '%' || new_code || '%';
    select nvl(max(id), 0) + 1 into new_id from H_TRANS_ITEM;
    :new.ID := new_id;
    :new.kode := new_code;
END;
ALTER TRIGGER "AUTO_ID_TRANS_OS" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_ULASAN
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "AUTO_ID_ULASAN" 
    before insert
    on ULASAN
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from ULASAN;
    :new.ID := new_id;
END;
ALTER TRIGGER "AUTO_ID_ULASAN" ENABLE
--------------------------------------------------------
--  Constraints for Table METODE_PEMBAYARAN
--------------------------------------------------------

  ALTER TABLE "METODE_PEMBAYARAN" ADD CONSTRAINT "METODE_PEMBAYARAN_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "METODE_PEMBAYARAN" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "METODE_PEMBAYARAN" MODIFY ("NAMA" NOT NULL ENABLE)
 
  ALTER TABLE "METODE_PEMBAYARAN" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table TRANS_OS
--------------------------------------------------------

  ALTER TABLE "TRANS_OS" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "TRANS_OS" MODIFY ("TANGGAL_TRANSAKSI" NOT NULL ENABLE)
 
  ALTER TABLE "TRANS_OS" MODIFY ("ID_KONTRAK" NOT NULL ENABLE)
 
  ALTER TABLE "TRANS_OS" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table SELLER
--------------------------------------------------------

  ALTER TABLE "SELLER" ADD CONSTRAINT "SELLER_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "SELLER" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "SELLER" MODIFY ("NAMA_TOKO" NOT NULL ENABLE)
 
  ALTER TABLE "SELLER" MODIFY ("ALAMAT" NOT NULL ENABLE)
 
  ALTER TABLE "SELLER" MODIFY ("PASSWORD" NOT NULL ENABLE)
 
  ALTER TABLE "SELLER" MODIFY ("CREATED_AT" NOT NULL ENABLE)
 
  ALTER TABLE "SELLER" MODIFY ("NAMA_SELLER" NOT NULL ENABLE)
 
  ALTER TABLE "SELLER" MODIFY ("NIK" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table CATEGORY
--------------------------------------------------------

  ALTER TABLE "CATEGORY" ADD CONSTRAINT "KATEGORI_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "CATEGORY" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "CATEGORY" MODIFY ("NAMA" NOT NULL ENABLE)
 
  ALTER TABLE "CATEGORY" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table PROMO
--------------------------------------------------------

  ALTER TABLE "PROMO" ADD CONSTRAINT "PROMO_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "PROMO" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "PROMO" MODIFY ("KODE" NOT NULL ENABLE)
 
  ALTER TABLE "PROMO" MODIFY ("POTONGAN" NOT NULL ENABLE)
 
  ALTER TABLE "PROMO" MODIFY ("HARGA_MIN" NOT NULL ENABLE)
 
  ALTER TABLE "PROMO" MODIFY ("JENIS_POTONGAN" NOT NULL ENABLE)
 
  ALTER TABLE "PROMO" MODIFY ("ID_JENIS_PROMO" NOT NULL ENABLE)
 
  ALTER TABLE "PROMO" MODIFY ("TANGGAL_AWAL" NOT NULL ENABLE)
 
  ALTER TABLE "PROMO" MODIFY ("TANGGAL_AKHIR" NOT NULL ENABLE)
 
  ALTER TABLE "PROMO" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table H_TRANS_ITEM
--------------------------------------------------------

  ALTER TABLE "H_TRANS_ITEM" ADD CONSTRAINT "H_TRANS_ITEM_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "H_TRANS_ITEM" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "H_TRANS_ITEM" MODIFY ("TANGGAL_TRANSAKSI" NOT NULL ENABLE)
 
  ALTER TABLE "H_TRANS_ITEM" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table ULASAN
--------------------------------------------------------

  ALTER TABLE "ULASAN" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "ULASAN" MODIFY ("RATING" NOT NULL ENABLE)
 
  ALTER TABLE "ULASAN" MODIFY ("STATUS" NOT NULL ENABLE)
 
  ALTER TABLE "ULASAN" ADD CONSTRAINT "ULASAN_PK" PRIMARY KEY ("ID") ENABLE
--------------------------------------------------------
--  Constraints for Table CUSTOMER
--------------------------------------------------------

  ALTER TABLE "CUSTOMER" ADD CONSTRAINT "CUSTOMER_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "CUSTOMER" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "CUSTOMER" MODIFY ("EMAIL" NOT NULL ENABLE)
 
  ALTER TABLE "CUSTOMER" MODIFY ("NAMA" NOT NULL ENABLE)
 
  ALTER TABLE "CUSTOMER" MODIFY ("TANGGAL_LAHIR" NOT NULL ENABLE)
 
  ALTER TABLE "CUSTOMER" MODIFY ("NO_TELP" NOT NULL ENABLE)
 
  ALTER TABLE "CUSTOMER" MODIFY ("PASSWORD" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table D_TRANS_ITEM
--------------------------------------------------------

  ALTER TABLE "D_TRANS_ITEM" ADD CONSTRAINT "D_TRANS_ITEM_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "D_TRANS_ITEM" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "D_TRANS_ITEM" MODIFY ("ID_HTRANS" NOT NULL ENABLE)
 
  ALTER TABLE "D_TRANS_ITEM" MODIFY ("ID_ITEM" NOT NULL ENABLE)
 
  ALTER TABLE "D_TRANS_ITEM" MODIFY ("JUMLAH" NOT NULL ENABLE)
 
  ALTER TABLE "D_TRANS_ITEM" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table KURIR
--------------------------------------------------------

  ALTER TABLE "KURIR" ADD CONSTRAINT "KURIR_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "KURIR" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "KURIR" MODIFY ("NAMA" NOT NULL ENABLE)
 
  ALTER TABLE "KURIR" MODIFY ("HARGA" NOT NULL ENABLE)
 
  ALTER TABLE "KURIR" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table D_DISKUSI
--------------------------------------------------------

  ALTER TABLE "D_DISKUSI" ADD CONSTRAINT "D_DISKUSI_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "D_DISKUSI" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "D_DISKUSI" MODIFY ("MESSAGE" NOT NULL ENABLE)
 
  ALTER TABLE "D_DISKUSI" MODIFY ("SENDER" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table JENIS_PROMO
--------------------------------------------------------

  ALTER TABLE "JENIS_PROMO" ADD CONSTRAINT "JENIS_PROMO_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "JENIS_PROMO" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "JENIS_PROMO" MODIFY ("NAMA" NOT NULL ENABLE)
 
  ALTER TABLE "JENIS_PROMO" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table ITEM
--------------------------------------------------------

  ALTER TABLE "ITEM" ADD CONSTRAINT "ITEM_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "ITEM" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "ITEM" MODIFY ("HARGA" NOT NULL ENABLE)
 
  ALTER TABLE "ITEM" MODIFY ("STATUS" NOT NULL ENABLE)
 
  ALTER TABLE "ITEM" MODIFY ("NAMA" NOT NULL ENABLE)
 
  ALTER TABLE "ITEM" MODIFY ("BERAT" NOT NULL ENABLE)
 
  ALTER TABLE "ITEM" MODIFY ("STOK" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table KONTRAK_OS
--------------------------------------------------------

  ALTER TABLE "KONTRAK_OS" ADD CONSTRAINT "KONTRAK_OS_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "KONTRAK_OS" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "KONTRAK_OS" MODIFY ("JANGKA_WAKTU" NOT NULL ENABLE)
 
  ALTER TABLE "KONTRAK_OS" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Constraints for Table H_DISKUSI
--------------------------------------------------------

  ALTER TABLE "H_DISKUSI" ADD CONSTRAINT "DISKUSI_PK" PRIMARY KEY ("ID") ENABLE
 
  ALTER TABLE "H_DISKUSI" MODIFY ("ID" NOT NULL ENABLE)
 
  ALTER TABLE "H_DISKUSI" MODIFY ("MESSAGE" NOT NULL ENABLE)
 
  ALTER TABLE "H_DISKUSI" MODIFY ("ID_ITEM" NOT NULL ENABLE)
 
  ALTER TABLE "H_DISKUSI" MODIFY ("STATUS" NOT NULL ENABLE)
--------------------------------------------------------
--  Ref Constraints for Table D_DISKUSI
--------------------------------------------------------

  ALTER TABLE "D_DISKUSI" ADD CONSTRAINT "D_DISKUSI_CUSTOMER_ID_FK" FOREIGN KEY ("ID_CUSTOMER") REFERENCES "CUSTOMER" ("ID") ENABLE
 
  ALTER TABLE "D_DISKUSI" ADD CONSTRAINT "D_DISKUSI_SELLER_ID_FK" FOREIGN KEY ("ID_SELLER") REFERENCES "SELLER" ("ID") ENABLE
--------------------------------------------------------
--  Ref Constraints for Table D_TRANS_ITEM
--------------------------------------------------------

  ALTER TABLE "D_TRANS_ITEM" ADD CONSTRAINT "DTRANS_ITEM_H_TRANS_ITEM" FOREIGN KEY ("ID_HTRANS") REFERENCES "H_TRANS_ITEM" ("ID") ENABLE
 
  ALTER TABLE "D_TRANS_ITEM" ADD CONSTRAINT "D_TRANS_ITEM_ITEM_ID_FK" FOREIGN KEY ("ID_ITEM") REFERENCES "ITEM" ("ID") ENABLE
--------------------------------------------------------
--  Ref Constraints for Table H_DISKUSI
--------------------------------------------------------

  ALTER TABLE "H_DISKUSI" ADD CONSTRAINT "H_DISKUSI_CUSTOMER_ID_FK" FOREIGN KEY ("ID_CUSTOMER") REFERENCES "CUSTOMER" ("ID") ENABLE
 
  ALTER TABLE "H_DISKUSI" ADD CONSTRAINT "H_DISKUSI_ITEM_ID_FK" FOREIGN KEY ("ID_ITEM") REFERENCES "ITEM" ("ID") ENABLE
--------------------------------------------------------
--  Ref Constraints for Table H_TRANS_ITEM
--------------------------------------------------------

  ALTER TABLE "H_TRANS_ITEM" ADD CONSTRAINT "H_TRANS_ITEM_CUSTOMER_ID_FK" FOREIGN KEY ("ID_CUSTOMER") REFERENCES "CUSTOMER" ("ID") ENABLE
 
  ALTER TABLE "H_TRANS_ITEM" ADD CONSTRAINT "H_TRANS_ITEM_KURIR_ID_FK" FOREIGN KEY ("ID_KURIR") REFERENCES "KURIR" ("ID") ENABLE
--------------------------------------------------------
--  Ref Constraints for Table ITEM
--------------------------------------------------------

  ALTER TABLE "ITEM" ADD CONSTRAINT "FK_ID_SELLER" FOREIGN KEY ("ID_SELLER") REFERENCES "SELLER" ("ID") ENABLE
 
  ALTER TABLE "ITEM" ADD CONSTRAINT "FK_KATEGORY_ITEM" FOREIGN KEY ("ID_CATEGORY") REFERENCES "CATEGORY" ("ID") ENABLE
--------------------------------------------------------
--  Ref Constraints for Table JENIS_PROMO
--------------------------------------------------------

  ALTER TABLE "JENIS_PROMO" ADD CONSTRAINT "JENIS_PROMO_CATEGORY_ID_FK" FOREIGN KEY ("ID_CATEGORY") REFERENCES "CATEGORY" ("ID") ENABLE
 
  ALTER TABLE "JENIS_PROMO" ADD CONSTRAINT "JENIS_PROMO_KURIR_ID_FK" FOREIGN KEY ("ID_KURIR") REFERENCES "KURIR" ("ID") ENABLE
 
  ALTER TABLE "JENIS_PROMO" ADD CONSTRAINT "JENIS_PROMO_MP__FK" FOREIGN KEY ("ID_METODE_PEMBAYARAN") REFERENCES "METODE_PEMBAYARAN" ("ID") ENABLE
 
  ALTER TABLE "JENIS_PROMO" ADD CONSTRAINT "JENIS_PROMO_SELLER_ID_FK" FOREIGN KEY ("ID_SELLER") REFERENCES "SELLER" ("ID") ENABLE
--------------------------------------------------------
--  Ref Constraints for Table KONTRAK_OS
--------------------------------------------------------

  ALTER TABLE "KONTRAK_OS" ADD CONSTRAINT "KONTRAK_OS_SELLER_ID_FK" FOREIGN KEY ("ID_SELLER") REFERENCES "SELLER" ("ID") ENABLE
--------------------------------------------------------
--  Ref Constraints for Table PROMO
--------------------------------------------------------

  ALTER TABLE "PROMO" ADD CONSTRAINT "PROMO_JENIS_PROMO_ID_FK" FOREIGN KEY ("ID_JENIS_PROMO") REFERENCES "JENIS_PROMO" ("ID") ENABLE
--------------------------------------------------------
--  Ref Constraints for Table TRANS_OS
--------------------------------------------------------

  ALTER TABLE "TRANS_OS" ADD CONSTRAINT "TRANS_OS_KONTRAK_OS_ID_FK" FOREIGN KEY ("ID_KONTRAK") REFERENCES "KONTRAK_OS" ("ID") ENABLE
--------------------------------------------------------
--  Ref Constraints for Table ULASAN
--------------------------------------------------------

  ALTER TABLE "ULASAN" ADD CONSTRAINT "H_ULASAN_CUSTOMER_ID_FK" FOREIGN KEY ("ID_CUSTOMER") REFERENCES "CUSTOMER" ("ID") ENABLE
 
  ALTER TABLE "ULASAN" ADD CONSTRAINT "H_ULASAN_H_TRANS_ITEM_ID_FK" FOREIGN KEY ("ID_H_TRANS") REFERENCES "H_TRANS_ITEM" ("ID") ENABLE
 
  ALTER TABLE "ULASAN" ADD CONSTRAINT "H_ULASAN_ITEM_ID_FK" FOREIGN KEY ("REPLY") REFERENCES "ITEM" ("ID") ENABLE
