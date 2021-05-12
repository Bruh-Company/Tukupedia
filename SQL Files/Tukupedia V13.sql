DROP TABLE CATEGORY CASCADE CONSTRAINT PURGE;
DROP TABLE CUSTOMER CASCADE CONSTRAINT PURGE;
DROP TABLE D_DISKUSI CASCADE CONSTRAINT PURGE;
DROP TABLE D_TRANS_ITEM CASCADE CONSTRAINT PURGE;
DROP TABLE H_DISKUSI CASCADE CONSTRAINT PURGE;
DROP TABLE H_TRANS_ITEM CASCADE CONSTRAINT PURGE;
DROP TABLE ITEM CASCADE CONSTRAINT PURGE;
DROP TABLE JENIS_PROMO CASCADE CONSTRAINT PURGE;
DROP TABLE KONTRAK_OS CASCADE CONSTRAINT PURGE;
DROP TABLE KURIR CASCADE CONSTRAINT PURGE;
DROP TABLE METODE_PEMBAYARAN CASCADE CONSTRAINT PURGE;
DROP TABLE PROMO CASCADE CONSTRAINT PURGE;
DROP TABLE SELLER CASCADE CONSTRAINT PURGE;
DROP TABLE TRANS_OS CASCADE CONSTRAINT PURGE;
DROP TABLE ULASAN CASCADE CONSTRAINT PURGE;

create table CUSTOMER
(
    ID            NUMBER        not null
        constraint CUSTOMER_PK
            primary key,
    EMAIL         VARCHAR2(100) not null,
    NAMA          VARCHAR2(100) not null,
    TANGGAL_LAHIR DATE          not null,
    ALAMAT        VARCHAR2(100),
    NO_TELP       VARCHAR2(20)  not null,
    SALDO         NUMBER default 0,
    KODE          VARCHAR2(32)  not null,
    STATUS        CHAR   default 1,
    PASSWORD      VARCHAR2(64)  not null,
    IMAGE         VARCHAR2(255),
    CREATED_AT    DATE   default sysdate
)
/

create unique index CUSTOMER_EMAIL_UINDEX
    on CUSTOMER (EMAIL)
/

create unique index CUSTOMER_KODE_CUSTOMER_UINDEX
    on CUSTOMER (KODE)
/

create or replace trigger AUTO_ID_CUSTOMER
    instead of insert or update
    on CUSTOMER
    for each row
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from CUSTOMER
            where KODE like '%' || new_code || '%';
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from CUSTOMER
            where KODE like '%' || new_code || '%';
            new_code := 'CU' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update CUSTOMER set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_CUSTOMER;
/

create table CATEGORY
(
    ID         NUMBER         not null
        constraint KATEGORI_PK
            primary key,
    KODE       VARCHAR2(32)   not null,
    NAMA       VARCHAR2(30)   not null,
    STATUS     CHAR default 1 not null,
    CREATED_AT DATE default sysdate
)
/

create table SELLER
(
    ID          NUMBER                 not null
        constraint SELLER_PK
            primary key,
    KODE        VARCHAR2(32)           not null,
    EMAIL       VARCHAR2(30),
    NAMA_TOKO   VARCHAR2(100)          not null,
    ALAMAT      VARCHAR2(30)           not null,
    NO_TELP     VARCHAR2(30),
    SALDO       NUMBER default 0,
    IS_OFFICIAL CHAR,
    STATUS      CHAR   default 1,
    PASSWORD    VARCHAR2(64)           not null,
    CREATED_AT  DATE   default sysdate not null,
    NAMA_SELLER VARCHAR2(100)          not null,
    NIK         NUMBER                 not null
)
/

create or replace trigger AUTO_ID_CATEGORY
    instead of insert or update
    on CATEGORY
    for each row
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from CATEGORY
            where KODE like '%' || new_code || '%';
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from CATEGORY
            where KODE like '%' || new_code || '%';
            new_code := 'CAT' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update SELLER set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_CATEGORY;
/

create unique index SELLER_EMAIL_UINDEX
    on SELLER (EMAIL)
/

create unique index SELLER_KODE_SELLER_UINDEX
    on SELLER (KODE)
/

create or replace trigger AUTO_ID_SELLER
    instead of insert or update
    on SELLER
    for each row
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from SELLER
            where KODE like '%' || new_code || '%';
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from SELLER
            where KODE like '%' || new_code || '%';
            new_code := 'SE' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update SELLER set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_SELLER;
/

create table ITEM
(
    ID          NUMBER           not null
        constraint ITEM_PK
            primary key,
    KODE        VARCHAR2(32)     not null,
    ID_CATEGORY NUMBER
        constraint FK_KATEGORY_ITEM
            references CATEGORY,
    HARGA       NUMBER           not null,
    STATUS      CHAR   default 1 not null,
    NAMA        VARCHAR2(100)    not null,
    DESKRIPSI   VARCHAR2(1000),
    ID_SELLER   NUMBER
        constraint FK_ID_SELLER
            references SELLER,
    BERAT       NUMBER           not null,
    STOK        NUMBER default 0 not null,
    RATING      NUMBER default 0,
    IMAGE       VARCHAR2(255),
    CREATED_AT  DATE   default sysdate
)
/

create table H_DISKUSI
(
    ID          NUMBER         not null,
    ID_CUSTOMER NUMBER
        constraint H_DISKUSI_CUSTOMER_ID_FK
            references CUSTOMER,
    MESSAGE     VARCHAR2(255)  not null,
    ID_ITEM     NUMBER         not null
        constraint H_DISKUSI_ITEM_ID_FK
            references ITEM,
    STATUS      CHAR default 1 not null,
    CREATED_AT  DATE default sysdate
)
/

create unique index DISKUSI_ID_UINDEX
    on H_DISKUSI (ID)
/

alter table H_DISKUSI
    add constraint DISKUSI_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_H_DISKUSI
    before insert
    on H_DISKUSI
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from H_DISKUSI;
    :new.ID := new_id;
END;
/

create or replace trigger AUTO_ID_ITEM
    instead of insert or update
    on ITEM
    for each row
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from ITEM
            where KODE like '%' || new_code || '%';
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from ITEM
            where KODE like '%' || new_code || '%';
            new_code := 'IT' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update ITEM set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_ITEM;
/

create table KURIR
(
    ID         NUMBER         not null,
    KODE       VARCHAR2(32)   not null,
    NAMA       VARCHAR2(30)   not null,
    HARGA      NUMBER         not null,
    STATUS     CHAR default 1 not null,
    CREATED_AT DATE default sysdate
)
/

comment on column KURIR.HARGA is 'Harga/KM'
/

create unique index KURIR_ID_UINDEX
    on KURIR (ID)
/

create unique index KURIR_KODE_UINDEX
    on KURIR (KODE)
/

alter table KURIR
    add constraint KURIR_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_KURIR
    instead of insert or update
    on KURIR
    for each row
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from KURIR
            where KODE like '%' || new_code || '%';
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
            select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
            into jumlah
            from KURIR
            where KODE like '%' || new_code || '%';
            new_code := 'KR' || new_code || lpad(jumlah, 3, '0');
            if (old_id <> -1) and updating then
                update SELLER set KODE = new_code where id = old_id;
            end if;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_KURIR;
/

create table METODE_PEMBAYARAN
(
    ID         NUMBER         not null,
    NAMA       VARCHAR2(255)  not null,
    STATUS     CHAR default 1 not null,
    CREATED_AT DATE default sysdate
)
/

create unique index METODE_PEMBAYARAN_ID_UINDEX
    on METODE_PEMBAYARAN (ID)
/

alter table METODE_PEMBAYARAN
    add constraint METODE_PEMBAYARAN_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_METODE_PEMBAYARAN
    before insert
    on METODE_PEMBAYARAN
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from METODE_PEMBAYARAN;
    :new.ID := new_id;
END;
/

create table H_TRANS_ITEM
(
    ID                NUMBER               not null,
    KODE              VARCHAR2(32)         not null,
    TANGGAL_TRANSAKSI DATE default sysdate not null,
    ID_CUSTOMER       NUMBER
        constraint H_TRANS_ITEM_CUSTOMER_ID_FK
            references CUSTOMER,
    ID_KURIR          NUMBER
        constraint H_TRANS_ITEM_KURIR_ID_FK
            references KURIR,
    STATUS            CHAR default 'W'     not null
)
/

create unique index H_TRANS_ITEM_ID_UINDEX
    on H_TRANS_ITEM (ID)
/

create unique index H_TRANS_ITEM_KODE_UINDEX
    on H_TRANS_ITEM (KODE)
/

alter table H_TRANS_ITEM
    add constraint H_TRANS_ITEM_PK
        primary key (ID)
/

create table ULASAN
(
    ID          NUMBER                  not null,
    ID_CUSTOMER NUMBER
        constraint H_ULASAN_CUSTOMER_ID_FK
            references CUSTOMER,
    REPLY       NUMBER
        constraint H_ULASAN_ITEM_ID_FK
            references ITEM,
    MESSAGE     VARCHAR2(255) default '',
    RATING      NUMBER                  not null,
    STATUS      CHAR          default 1 not null,
    CREATED_AT  DATE          default sysdate,
    ID_H_TRANS  NUMBER
        constraint H_ULASAN_H_TRANS_ITEM_ID_FK
            references H_TRANS_ITEM
)
/

create unique index ULASAN_ID_UINDEX
    on ULASAN (ID)
/

alter table ULASAN
    add constraint ULASAN_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_ULASAN
    before insert
    on ULASAN
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from ULASAN;
    :new.ID := new_id;
END;
/

create or replace trigger AUTO_ID_H_TRANS_ITEM
    before insert
    on H_TRANS_ITEM
    for each row
DECLARE
    new_id   number;
    new_code varchar2(30);
    jumlah   number;
BEGIN
    new_code := 'HTI' || to_char(sysdate, 'ddmmyyyy') || lpad(jumlah, 5, '0');
    select NVL(MAX(ltrim(substr(KODE, 12), '0')), 0) + 1
    into jumlah
    from H_TRANS_ITEM
    where KODE like '%' || new_code || '%';
    select nvl(max(id), 0) + 1 into new_id from H_TRANS_ITEM;
    :new.ID := new_id;
    :new.kode := new_code;
END;
/

create table D_TRANS_ITEM
(
    ID        NUMBER           not null,
    ID_HTRANS NUMBER           not null
        constraint DTRANS_ITEM_H_TRANS_ITEM
            references H_TRANS_ITEM,
    ID_ITEM   NUMBER           not null
        constraint D_TRANS_ITEM_ITEM_ID_FK
            references ITEM,
    JUMLAH    NUMBER           not null,
    STATUS    CHAR default 'W' not null
)
/

create unique index D_TRANS_ITEM_ID_UINDEX
    on D_TRANS_ITEM (ID)
/

alter table D_TRANS_ITEM
    add constraint D_TRANS_ITEM_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_D_TRANS_ITEM
    before insert
    on D_TRANS_ITEM
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from D_TRANS_ITEM;
    :new.ID := new_id;
END;
/

create table JENIS_PROMO
(
    ID                   NUMBER         not null,
    NAMA                 VARCHAR2(255)  not null,
    ID_CATEGORY          NUMBER
        constraint JENIS_PROMO_CATEGORY_ID_FK
            references CATEGORY,
    ID_KURIR             NUMBER
        constraint JENIS_PROMO_KURIR_ID_FK
            references KURIR,
    ID_SELLER            NUMBER
        constraint JENIS_PROMO_SELLER_ID_FK
            references SELLER,
    ID_METODE_PEMBAYARAN NUMBER
        constraint JENIS_PROMO_MP__FK
            references METODE_PEMBAYARAN,
    STATUS               CHAR default 1 not null,
    CREATED_AT           DATE default sysdate
)
/

create unique index JENIS_PROMO_ID_UINDEX
    on JENIS_PROMO (ID)
/

alter table JENIS_PROMO
    add constraint JENIS_PROMO_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_JENIS_PROMO
    before insert
    on JENIS_PROMO
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from JENIS_PROMO;
    :new.ID := new_id;
END;
/

create table PROMO
(
    ID             NUMBER         not null,
    KODE           VARCHAR2(32)   not null,
    POTONGAN       NUMBER         not null,
    POTONGAN_MAKS  NUMBER,
    HARGA_MIN      NUMBER         not null,
    JENIS_POTONGAN CHAR           not null,
    ID_JENIS_PROMO NUMBER         not null
        constraint PROMO_JENIS_PROMO_ID_FK
            references JENIS_PROMO,
    TANGGAL_AWAL   DATE           not null,
    TANGGAL_AKHIR  DATE           not null,
    STATUS         CHAR default 1 not null,
    CREATED_AT     DATE default sysdate
)
/

create unique index PROMO_ID_UINDEX
    on PROMO (ID)
/

create unique index PROMO_KODE_UINDEX
    on PROMO (KODE)
/

alter table PROMO
    add constraint PROMO_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_PROMO
    before insert
    on PROMO
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from PROMO;
    :new.ID := new_id;
END;
/

create table KONTRAK_OS
(
    ID           NUMBER         not null,
    ID_SELLER    NUMBER
        constraint KONTRAK_OS_SELLER_ID_FK
            references SELLER,
    CREATED_AT   DATE default sysdate,
    JANGKA_WAKTU NUMBER         not null,
    STATUS       CHAR default 1 not null
)
/

create unique index KONTRAK_OS_ID_UINDEX
    on KONTRAK_OS (ID)
/

alter table KONTRAK_OS
    add constraint KONTRAK_OS_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_KONTRAK_OS
    before insert
    on KONTRAK_OS
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from KONTRAK_OS;
    :new.ID := new_id;
END;
/

create table TRANS_OS
(
    ID                NUMBER               not null,
    KODE              VARCHAR2(32)         not null,
    TANGGAL_TRANSAKSI DATE default sysdate not null,
    ID_KONTRAK        NUMBER               not null
        constraint TRANS_OS_KONTRAK_OS_ID_FK
            references KONTRAK_OS,
    STATUS            CHAR default 1       not null
)
/

create or replace trigger AUTO_ID_TRANS_OS
    before insert
    on TRANS_OS
    for each row
DECLARE
    new_id   number;
    new_code varchar2(30);
    jumlah   number;
BEGIN
    new_code := 'TOS' || to_char(sysdate, 'ddmmyyyy') || lpad(jumlah, 5, '0');
    select NVL(MAX(ltrim(substr(KODE, 12), '0')), 0) + 1
    into jumlah
    from TRANS_OS
    where KODE like '%' || new_code || '%';
    select nvl(max(id), 0) + 1 into new_id from H_TRANS_ITEM;
    :new.ID := new_id;
    :new.kode := new_code;
END;
/

create table D_DISKUSI
(
    ID          NUMBER        not null,
    ID_SELLER   NUMBER
        constraint D_DISKUSI_SELLER_ID_FK
            references SELLER,
    ID_CUSTOMER NUMBER
        constraint D_DISKUSI_CUSTOMER_ID_FK
            references CUSTOMER,
    MESSAGE     VARCHAR2(255) not null,
    STATUS      CHAR default 1,
    CREATED_AT  DATE default sysdate,
    SENDER      CHAR          not null
)
/

comment on column D_DISKUSI.SENDER is 'Jika sender adalah customer maka ''C''
Jika senfer adalah seller maka ''S'''
/

create unique index D_DISKUSI_ID_UINDEX
    on D_DISKUSI (ID)
/

alter table D_DISKUSI
    add constraint D_DISKUSI_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_D_DISKUSI
    before insert
    on D_DISKUSI
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from D_DISKUSI;
    :new.ID := new_id;
END;
/


