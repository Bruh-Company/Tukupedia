DROP TABLE CATEGORY CASCADE CONSTRAINT PURGE;
DROP TABLE CUSTOMER CASCADE CONSTRAINT PURGE;
DROP TABLE D_TRANS_ITEM CASCADE CONSTRAINT PURGE;
DROP TABLE H_TRANS_ITEM CASCADE CONSTRAINT PURGE;
DROP TABLE ITEM CASCADE CONSTRAINT PURGE;
DROP TABLE JENIS_PROMO CASCADE CONSTRAINT PURGE;
DROP TABLE KONTRAK_OS CASCADE CONSTRAINT PURGE;
DROP TABLE KURIR CASCADE CONSTRAINT PURGE;
DROP TABLE METODE_PEMBAYARAN CASCADE CONSTRAINT PURGE;
DROP TABLE PROMO CASCADE CONSTRAINT PURGE;
DROP TABLE SELLER CASCADE CONSTRAINT PURGE;
DROP TABLE TRANS_OS CASCADE CONSTRAINT PURGE;

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
    STATUS        CHAR   default 1
)
/

create unique index CUSTOMER_EMAIL_UINDEX
    on CUSTOMER (EMAIL)
/

create unique index CUSTOMER_KODE_CUSTOMER_UINDEX
    on CUSTOMER (KODE)
/

create table CATEGORY
(
    ID   NUMBER       not null
        constraint KATEGORI_PK
            primary key,
    KODE VARCHAR2(32) not null,
    NAMA VARCHAR2(30) not null
)
/

create table ITEM
(
    ID          NUMBER        not null
        constraint ITEM_PK
            primary key,
    KODE        VARCHAR2(32)  not null,
    ID_CATEGORY NUMBER
        constraint FK_KATEGORY_ITEM
            references CATEGORY,
    HARGA       NUMBER        not null,
    STATUS      CHAR,
    NAMA        VARCHAR2(100) not null,
    DESKRIPSI   VARCHAR2(1000),
    CREATED_AT  DATE default sysdate
)
/

create table SELLER
(
    ID          NUMBER        not null
        constraint SELLER_PK
            primary key,
    KODE        VARCHAR2(32)  not null,
    EMAIL       VARCHAR2(30),
    NAMA        VARCHAR2(100) not null,
    ALAMAT      VARCHAR2(30)  not null,
    NO_TELP     VARCHAR2(30),
    SALDO       NUMBER default 0,
    IS_OFFICIAL CHAR,
    STATUS      CHAR   default 1
)
/

create unique index SELLER_EMAIL_UINDEX
    on SELLER (EMAIL)
/

create unique index SELLER_KODE_SELLER_UINDEX
    on SELLER (KODE)
/

create table KURIR
(
    ID      NUMBER       not null,
    KODE    VARCHAR2(32) not null,
    NAMA    VARCHAR2(30) not null,
    "Harga" NUMBER       not null
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

create table METODE_PEMBAYARAN
(
    ID   NUMBER        not null,
    NAMA VARCHAR2(255) not null
)
/

create unique index METODE_PEMBAYARAN_ID_UINDEX
    on METODE_PEMBAYARAN (ID)
/

alter table METODE_PEMBAYARAN
    add constraint METODE_PEMBAYARAN_PK
        primary key (ID)
/

create table JENIS_PROMO
(
    ID                   NUMBER        not null,
    NAMA                 VARCHAR2(255) not null,
    ID_CATEGORY          NUMBER,
    ID_KURIR             NUMBER,
    ID_SELLER            NUMBER,
    ID_METODE_PEMBAYARAN NUMBER
)
/

create unique index JENIS_PROMO_ID_UINDEX
    on JENIS_PROMO (ID)
/

alter table JENIS_PROMO
    add constraint JENIS_PROMO_PK
        primary key (ID)
/

create table PROMO
(
    ID             NUMBER       not null
        constraint PROMO_JENIS_PROMO_ID_FK
            references JENIS_PROMO,
    KODE           VARCHAR2(32) not null,
    POTONGAN       NUMBER       not null,
    POTONGAN_MAKS  NUMBER,
    HARGA_MIN      NUMBER       not null,
    JENIS_POTONGAN CHAR         not null,
    ID_JENIS_PROMO NUMBER       not null,
    TANGGAL_AWAL   DATE         not null,
    TANGGAL_AKHIR  DATE         not null
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

create table H_TRANS_ITEM
(
    ID                NUMBER           not null
        constraint H_TRANS_ITEM_CUSTOMER_ID_FK
            references CUSTOMER
        constraint H_TRANS_ITEM_KURIR_ID_FK
            references KURIR
        constraint H_TRANS_ITEM_PROMO_ID_FK
            references PROMO,
    KODE              VARCHAR2(32)     not null,
    TANGGAL_TRANSAKSI DATE             not null,
    ID_CUSTOMER       NUMBER           not null,
    ID_KURIR          NUMBER           not null,
    ID_PROMO          NUMBER,
    STATUS            CHAR default 'P' not null
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

create table D_TRANS_ITEM
(
    ID        NUMBER not null
        constraint DTRANS_ITEM_H_TRANS_ITEM
            references H_TRANS_ITEM
        constraint D_TRANS_ITEM_ITEM_ID_FK
            references ITEM,
    ID_HTRANS NUMBER not null,
    ID_ITEM   NUMBER not null
)
/

create unique index D_TRANS_ITEM_ID_UINDEX
    on D_TRANS_ITEM (ID)
/

alter table D_TRANS_ITEM
    add constraint D_TRANS_ITEM_PK
        primary key (ID)
/

create table KONTRAK_OS
(
    ID           NUMBER not null,
    ID_SELLER    NUMBER,
    CREATED_AT   DATE,
    JANGKA_WAKTU NUMBER not null,
    STATUS       CHAR   not null
)
/

create unique index KONTRAK_OS_ID_UINDEX
    on KONTRAK_OS (ID)
/

alter table KONTRAK_OS
    add constraint KONTRAK_OS_PK
        primary key (ID)
/

create table TRANS_OS
(
    ID                NUMBER       not null
        constraint TRANS_OS_KONTRAK_OS_ID_FK
            references KONTRAK_OS,
    KODE              VARCHAR2(32) not null,
    TANGGAL_TRANSAKSI DATE         not null,
    ID_KONTRAK        NUMBER       not null
)
/

create unique index H_TRANS_OS_ID_UINDEX
    on TRANS_OS (ID)
/

create unique index H_TRANS_OS_KODE_UINDEX
    on TRANS_OS (KODE)
/

alter table TRANS_OS
    add constraint H_TRANS_OS_PK
        primary key (ID)
/


