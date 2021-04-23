create table SELLER
(
    ID         NUMBER        not null
        constraint SELLER_PK
            primary key,
    KODE       VARCHAR2(32)  not null,
    EMAIL      VARCHAR2(30),
    NAMA       VARCHAR2(100) not null,
    ALAMAT     VARCHAR2(30)  not null,
    NO_TELP    VARCHAR2(30),
    SALDO      NUMBER default 0,
    ISOFFICIAL CHAR,
    STATUS     CHAR   default 1
)
/

create unique index SELLER_EMAIL_UINDEX
    on SELLER (EMAIL)
/

create unique index SELLER_KODE_SELLER_UINDEX
    on SELLER (KODE_SELLER)
/

