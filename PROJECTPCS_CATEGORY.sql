create table CATEGORY
(
    ID   NUMBER       not null
        constraint KATEGORI_PK
            primary key,
    KODE VARCHAR2(32) not null,
    NAMA VARCHAR2(30) not null
)
/

