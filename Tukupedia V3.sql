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
    ID   NUMBER       not null
        constraint KATEGORI_PK
            primary key,
    KODE VARCHAR2(32) not null,
    NAMA VARCHAR2(30) not null
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
    STATUS      CHAR   default 1,
    PASSWORD    VARCHAR2(100) not null
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
    ID_SELLER   NUMBER
        constraint FK_ID_SELLER
            references SELLER
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
    ID                NUMBER       not null
        constraint H_TRANS_ITEM_CUSTOMER_ID_FK
            references CUSTOMER
        constraint H_TRANS_ITEM_KURIR_ID_FK
            references KURIR,
    KODE              VARCHAR2(32) not null,
    TANGGAL_TRANSAKSI DATE         not null,
    ID_CUSTOMER       NUMBER,
    ID_KURIR          NUMBER
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

create table H_TRANS_OS
(
    ID                NUMBER       not null,
    KODE              VARCHAR2(32) not null,
    TANGGAL_TRANSAKSI DATE         not null
)
/

create unique index H_TRANS_OS_ID_UINDEX
    on H_TRANS_OS (ID)
/

create unique index H_TRANS_OS_KODE_UINDEX
    on H_TRANS_OS (KODE)
/

alter table H_TRANS_OS
    add constraint H_TRANS_OS_PK
        primary key (ID)
/

create or replace trigger AUTO_ID_H_TRANS_OS
    before insert
    on H_TRANS_OS
    for each row
DECLARE
    new_id number;
BEGIN
    select nvl(max(id), 0) + 1 into new_id from H_TRANS_OS;
    :new.ID := new_id;
END;
/


