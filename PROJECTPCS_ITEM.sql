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
    DESKRIPSI   VARCHAR2(1000)
)
/

