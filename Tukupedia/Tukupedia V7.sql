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
    PASSWORD      VARCHAR2(100)
)
/

create unique index CUSTOMER_EMAIL_UINDEX
    on CUSTOMER (EMAIL)
/

create unique index CUSTOMER_KODE_CUSTOMER_UINDEX
    on CUSTOMER (KODE)
/

create or replace trigger AUTO_ID_CUSTOMER
    before insert
    on CUSTOMER
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from CUSTOMER;
    :new.ID := new_id;
END;
/

create table CATEGORY
(
    ID     NUMBER         not null
        constraint KATEGORI_PK
            primary key,
    KODE   VARCHAR2(32)   not null,
    NAMA   VARCHAR2(30)   not null,
    STATUS CHAR default 1 not null
)
/

create or replace trigger AUTO_ID_CATEGORY
    before insert
    on CATEGORY
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from CATEGORY;
    :new.ID := new_id;
END;
/

create table SELLER
(
    ID            NUMBER        not null
        constraint SELLER_PK
            primary key,
    KODE          VARCHAR2(32)  not null,
    EMAIL         VARCHAR2(30),
    NAMA          VARCHAR2(100) not null,
    ALAMAT        VARCHAR2(30)  not null,
    NO_TELP       VARCHAR2(30),
    SALDO         NUMBER default 0,
    IS_OFFICIAL   CHAR,
    STATUS        CHAR   default 1,
    PASSWORD      VARCHAR2(100) not null,
    TANGGAL_LAHIR DATE          not null,
    NAMA_PENJUAL  VARCHAR2(100) not null,
    NIK           NUMBER        not null
)
/

create unique index SELLER_EMAIL_UINDEX
    on SELLER (EMAIL)
/

create unique index SELLER_KODE_SELLER_UINDEX
    on SELLER (KODE)
/

create or replace trigger AUTO_ID_SELLER
    before insert
    on SELLER
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from SELLER;
    :new.ID := new_id;
END;
/

create table ITEM
(
    ID          NUMBER         not null
        constraint ITEM_PK
            primary key,
    KODE        VARCHAR2(32)   not null,
    ID_CATEGORY NUMBER
        constraint FK_KATEGORY_ITEM
            references CATEGORY,
    HARGA       NUMBER         not null,
    STATUS      CHAR default 1 not null,
    NAMA        VARCHAR2(100)  not null,
    DESKRIPSI   VARCHAR2(1000),
    ID_SELLER   NUMBER
        constraint FK_ID_SELLER
            references SELLER,
    BERAT       NUMBER         not null
)
/

create or replace trigger AUTO_ID_ITEM
    before insert
    on ITEM
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from ITEM;
    :new.ID := new_id;
END;
/

create table KURIR
(
    ID      NUMBER         not null,
    KODE    VARCHAR2(32)   not null,
    NAMA    VARCHAR2(30)   not null,
    "Harga" NUMBER         not null,
    STATUS  CHAR default 1 not null
)
/

comment on column KURIR."Harga" is 'Harga/KM'
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
    before insert
    on KURIR
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from KURIR;
    :new.ID := new_id;
END;
/

create table METODE_PEMBAYARAN
(
    ID     NUMBER         not null,
    NAMA   VARCHAR2(255)  not null,
    STATUS CHAR default 1 not null
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
    ID                NUMBER         not null,
    KODE              VARCHAR2(32)   not null,
    TANGGAL_TRANSAKSI DATE           not null,
    ID_CUSTOMER       NUMBER
        constraint H_TRANS_ITEM_CUSTOMER_ID_FK
            references CUSTOMER,
    ID_KURIR          NUMBER
        constraint H_TRANS_ITEM_KURIR_ID_FK
            references KURIR,
    STATUS            CHAR default 1 not null
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

create or replace trigger AUTO_ID_H_TRANS_ITEM
    before insert
    on H_TRANS_ITEM
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from H_TRANS_ITEM;
    :new.ID := new_id;
END;
/

create table D_TRANS_ITEM
(
    ID        NUMBER not null,
    ID_HTRANS NUMBER not null
        constraint DTRANS_ITEM_H_TRANS_ITEM
            references H_TRANS_ITEM,
    ID_ITEM   NUMBER not null
        constraint D_TRANS_ITEM_ITEM_ID_FK
            references ITEM
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
    STATUS               CHAR default 1 not null
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
    STATUS         CHAR default 1 not null
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
    CREATED_AT   DATE,
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
    ID                NUMBER         not null,
    KODE              VARCHAR2(32)   not null,
    TANGGAL_TRANSAKSI DATE           not null,
    ID_KONTRAK        NUMBER         not null
        constraint TRANS_OS_KONTRAK_OS_ID_FK
            references KONTRAK_OS,
    STATUS            CHAR default 1 not null
)
/

create or replace trigger AUTO_ID_TRANS_OS
    before insert
    on TRANS_OS
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from TRANS_OS;
    :new.ID := new_id;
END;
/

create table DISKUSI
(
    ID          NUMBER         not null
        constraint DISKUSI_CUSTOMER_ID_FK
            references CUSTOMER,
    ID_CUSTOMER NUMBER,
    ID_SELLER   NUMBER
        constraint DISKUSI_SELLER_ID_FK
            references SELLER,
    MESSAGE     VARCHAR2(255)  not null,
    ID_ITEM     NUMBER         not null
        constraint DISKUSI_ITEM_ID_FK
            references ITEM,
    STATUS      CHAR default 1 not null
)
/

create unique index DISKUSI_ID_UINDEX
    on DISKUSI (ID)
/

alter table DISKUSI
    add constraint DISKUSI_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_DISKUSI
    before insert
    on DISKUSI
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from DISKUSI;
    :new.ID := new_id;
END;
/

create table ULASAN
(
    ID          NUMBER                  not null,
    ID_CUSTOMER NUMBER
        constraint ULASAN_CUSTOMER_ID_FK
            references CUSTOMER,
    ID_SELLER   NUMBER
        constraint ULASAN_SELLER_ID_FK
            references SELLER,
    ID_ITEM     NUMBER
        constraint ULASAN_ITEM_ID_FK
            references ITEM,
    MESSAGE     VARCHAR2(255) default '',
    RATING      NUMBER                  not null,
    STATUS      CHAR          default 1 not null
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


