--------------------------------------------------------
--  File created - Thursday-May-13-2021
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_CATEGORY
--------------------------------------------------------
CREATE OR REPLACE FUNCTION GENERATE_KODE_ITEM(
    nama varchar2)
    return varchar2
    is
    new_code varchar2(30);
    jumlah   number;
begin
    if nama like '%' || ' ' || '%' then
        new_code := substr(nama, 1, 1) || substr(nama, instr(nama, ' ') + 1, 1);
    else
        new_code := substr(nama, 1, 2);
    end if;
    new_code := upper(new_code);
    select NVL(MAX(ltrim(substr(KODE, 5), '0')), 0) + 1
    into jumlah
    from ITEM
    where KODE like '%' || new_code || '%';
    new_code := 'IT' || new_code || lpad(jumlah, 3, '0');
    return new_code;
end GENERATE_KODE_ITEM;
/

CREATE OR REPLACE TRIGGER "AUTO_ID_CATEGORY"
    FOR insert or update
    ON CATEGORY
    COMPOUND TRIGGER
    new_id number;
    new_code varchar2(30);
    jumlah number;
    old_id number := -1;
    jml number := -1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select count(*) into jml from CATEGORY;
        if (jml = 0) then
            new_id := 1;
        else
            select nvl(max(id), 0) + 1 into new_id from CATEGORY;
        end if;
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
            select NVL(MAX(substr(KODE, 6)), 0) + 1
            into jumlah
            from CATEGORY
            where KODE like '%' || new_code || '%';
            new_code := 'CAT' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.NAMA <> :new.NAMA and substr(:old.KODE, 4, 2) <> new_code then
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
        if (old_id <> -1) then
            select NVL(MAX(substr(KODE, 6)), 0) + 1
            into jumlah
            from CATEGORY
            where KODE like '%' || new_code || '%';
            new_code := 'CAT' || new_code || lpad(jumlah, 3, '0');
            update CATEGORY set KODE = new_code where id = old_id;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_CATEGORY;

/
--ALTER TRIGGER "AUTO_ID_CATEGORY" ENABLE
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
    jml number := -1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select count(*) into jml from CUSTOMER;
        if (jml = 0) then
            new_id := 1;
        else
            select nvl(max(id), 0) + 1 into new_id from CUSTOMER;
        end if;
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
            select NVL(MAX(substr(KODE, 5)), 0) + 1
            into jumlah
            from CUSTOMER
            where KODE like '%' || new_code || '%';
            new_code := 'CU' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.nama <> :new.nama and substr(:old.KODE, 3, 2) <> new_code then
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
        if (old_id <> -1) then
            select NVL(MAX(substr(KODE, 5)), 0) + 1
            into jumlah
            from CUSTOMER
            where KODE like '%' || new_code || '%';
            new_code := 'CU' || new_code || lpad(jumlah, 3, '0');
            update CUSTOMER set KODE = new_code where id = old_id;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_CUSTOMER;
/
--ALTER TRIGGER "AUTO_ID_CUSTOMER" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_D_DISKUSI
--------------------------------------------------------

CREATE OR REPLACE TRIGGER AUTO_ID_D_DISKUSI
    before insert
    on D_DISKUSI
    for each row
DECLARE
    new_id number;
    jml    number := -1;
BEGIN
    select count(*) into jml from D_DISKUSI;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from D_DISKUSI;
    end if;
    :NEW.ID := new_id;
END AUTO_ID_D_DISKUSI;
/
--ALTER TRIGGER "AUTO_ID_D_DISKUSI" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_D_TRANS_ITEM
--------------------------------------------------------

CREATE OR REPLACE TRIGGER "AUTO_ID_D_TRANS_ITEM"
    before insert
    on D_TRANS_ITEM
    for each row
DECLARE
    new_id number;
    jml number:=-1;
BEGIN
    select count(*) into jml from D_TRANS_ITEM;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from D_TRANS_ITEM;
    end if;
    :NEW.ID := new_id;
END AUTO_ID_D_TRANS_ITEM;
/
--ALTER TRIGGER "AUTO_ID_D_TRANS_ITEM" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_H_DISKUSI
--------------------------------------------------------

CREATE OR REPLACE TRIGGER "AUTO_ID_H_DISKUSI"
    before insert
    on H_DISKUSI
    for each row
DECLARE
    new_id number;
    jml number:=-1;
BEGIN
    select count(*) into jml from H_DISKUSI;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from H_DISKUSI;
    end if;
    :NEW.ID := new_id;
END AUTO_ID_H_DISKUSI;
/
--ALTER TRIGGER "AUTO_ID_H_DISKUSI" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_H_TRANS_ITEM
--------------------------------------------------------

CREATE OR REPLACE TRIGGER "AUTO_ID_H_TRANS_ITEM"
    before insert
    on H_TRANS_ITEM
    for each row
DECLARE
    new_id   number;
    new_code varchar2(30);
    jumlah   number;
    jml number:=-1;
BEGIN
    new_code := 'HTI' || to_char(sysdate, 'ddmmyyyy');
    select NVL(MAX(substr(KODE, 12)), 0) + 1
    into jumlah
    from H_TRANS_ITEM
    where KODE like '%' || new_code || '%';
    select count(*) into jml from H_TRANS_ITEM;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from H_TRANS_ITEM;
    end if;
    :NEW.ID := new_id;
    :new.kode := new_code || lpad(jumlah, 5, '0');
END AUTO_ID_H_TRANS_ITEM;
/
--ALTER TRIGGER "AUTO_ID_H_TRANS_ITEM" ENABLE
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
    jml number:=-1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select count(*) into jml from ITEM;
        if (jml = 0) then
            new_id := 1;
        else
            select nvl(max(id), 0) + 1 into new_id from ITEM;
        end if;
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
            select NVL(MAX(substr(KODE, 5)), 0) + 1
            into jumlah
            from ITEM
            where KODE like '%' || new_code || '%';
            new_code := 'IT' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.NAMA <> :new.NAMA and substr(:old.KODE, 3, 2) <> new_code then
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
        if (old_id <> -1) then
            select NVL(MAX(substr(KODE, 5)), 0) + 1
            into jumlah
            from ITEM
            where KODE like '%' || new_code || '%';
            new_code := 'IT' || new_code || lpad(jumlah, 3, '0');
            update ITEM set KODE = new_code where id = old_id;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_ITEM;
/
--ALTER TRIGGER "AUTO_ID_ITEM" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_JENIS_PROMO
--------------------------------------------------------

CREATE OR REPLACE TRIGGER "AUTO_ID_JENIS_PROMO"
    before insert
    on JENIS_PROMO
    for each row
DECLARE
    new_id number;
    jml number:=-1;
BEGIN
    select count(*) into jml from JENIS_PROMO;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from JENIS_PROMO;
    end if;
    :NEW.ID := new_id;
END AUTO_ID_JENIS_PROMO;
/

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
    jml number:=-1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select count(*) into jml from KURIR;
        if (jml = 0) then
            new_id := 1;
        else
            select nvl(max(id), 0) + 1 into new_id from KURIR;
        end if;
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
            select NVL(MAX(substr(KODE, 5)), 0) + 1
            into jumlah
            from KURIR
            where KODE like '%' || new_code || '%';
            new_code := 'KR' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.NAMA <> :new.NAMA and substr(:old.KODE, 3, 2) <> new_code then
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
        if (old_id <> -1) then
            select NVL(MAX(substr(KODE, 5)), 0) + 1
            into jumlah
            from KURIR
            where KODE like '%' || new_code || '%';
            new_code := 'KR' || new_code || lpad(jumlah, 3, '0');
            update KURIR set KODE = new_code where id = old_id;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_KURIR;
/
--ALTER TRIGGER "AUTO_ID_KURIR" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_METODE_PEMBAYARAN
--------------------------------------------------------

CREATE OR REPLACE TRIGGER "AUTO_ID_METODE_PEMBAYARAN"
    before insert
    on METODE_PEMBAYARAN
    for each row
DECLARE
    new_id number;
    jml number:=-1;
BEGIN
    select count(*) into jml from METODE_PEMBAYARAN;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from METODE_PEMBAYARAN;
    end if;
    :NEW.ID := new_id;
END AUTO_ID_METODE_PEMBAYARAN;
/
--ALTER TRIGGER "AUTO_ID_METODE_PEMBAYARAN" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_PROMO
--------------------------------------------------------

CREATE OR REPLACE TRIGGER "AUTO_ID_PROMO"
    before insert
    on PROMO
    for each row
DECLARE
    new_id number;
    jml number:=-1;
BEGIN
    select count(*) into jml from PROMO;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from PROMO;
    end if;
    :NEW.ID := new_id;
END AUTO_ID_PROMO;
/
--ALTER TRIGGER "AUTO_ID_PROMO" ENABLE
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
    jml number:=-1;
BEFORE STATEMENT IS
BEGIN
    if inserting then
        select count(*) into jml from SELLER;
        if (jml = 0) then
            new_id := 1;
        else
            select nvl(max(id), 0) + 1 into new_id from SELLER;
        end if;
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
            select NVL(MAX(substr(KODE, 5)), 0) + 1
            into jumlah
            from SELLER
            where KODE like '%' || new_code || '%';
            new_code := 'SE' || new_code || lpad(jumlah, 3, '0');
            :new.ID := new_id;
            :new.KODE := new_code;
        end if;
        if updating then
            if :old.NAMA_TOKO <> :new.NAMA_TOKO and substr(:old.KODE, 3, 2) <> new_code then
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
        if (old_id <> -1) then
            select NVL(MAX(substr(KODE, 5)), 0) + 1
            into jumlah
            from SELLER
            where KODE like '%' || new_code || '%';
            new_code := 'SE' || new_code || lpad(jumlah, 3, '0');
            update SELLER set KODE = new_code where id = old_id;
        end if;
    END AFTER STATEMENT;
    END AUTO_ID_SELLER;
/
--ALTER TRIGGER "AUTO_ID_SELLER" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_TRANS_OS
--------------------------------------------------------

CREATE OR REPLACE TRIGGER "AUTO_ID_TRANS_OS"
    before insert
    on TRANS_OS
    for each row
DECLARE
    new_id   number;
    new_code varchar2(30);
    jumlah   number;
    jml number:=-1;
BEGIN
    new_code := 'TOS' || to_char(sysdate, 'ddmmyyyy');
    select NVL(MAX(substr(KODE, 12)), 0) + 1
    into jumlah
    from TRANS_OS
    where KODE like '%' || new_code || '%';
    select count(*) into jml from TRANS_OS;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from TRANS_OS;
    end if;
    :NEW.ID := new_id;
    :new.kode := new_code || lpad(jumlah, 5, '0');
END AUTO_ID_TRANS_OS;
/
--ALTER TRIGGER "AUTO_ID_TRANS_OS" ENABLE
--------------------------------------------------------
--  DDL for Trigger AUTO_ID_ULASAN
--------------------------------------------------------

CREATE OR REPLACE TRIGGER "AUTO_ID_ULASAN"
    before insert
    on ULASAN
    for each row
DECLARE
    new_id number;
    jml number:=-1;
BEGIN
    select count(*) into jml from ULASAN;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from ULASAN;
    end if;
    :NEW.ID := new_id;
END AUTO_ID_ULASAN;
/
--ALTER TRIGGER "AUTO_ID_ULASAN" ENABLE

CREATE OR REPLACE TRIGGER "AUTO_ID_KURIR_SELLER"
    before insert
    on KURIR_SELLER
    for each row
DECLARE
    new_id number;
    jml number:=-1;
BEGIN
    select count(*) into jml from KURIR_SELLER;
    if (jml = 0) then
        new_id := 1;
    else
        select nvl(max(id), 0) + 1 into new_id from KURIR_SELLER;
    end if;
    :NEW.ID := new_id;
END AUTO_ID_KURIR_SELLER;
/

--Trigger dari chen untuk rating
CREATE or REPLACE TRIGGER POST_ULASAN
    AFTER INSERT
    on ULASAN
DECLARE
BEGIN
    for i in (
        SELECT AVG(U.RATING) as ratingBaru, I.id as ID_ITEM
        FROM ULASAN U
                 join D_TRANS_ITEM DTI on DTI.ID = U.ID_D_TRANS_ITEM
                 join ITEM I on I.ID = DTI.ID_ITEM
        GROUP BY I.id
        )
        loop
            update ITEM
            set RATING=i.ratingBaru
            where ID = i.ID_ITEM;
        end loop;
END;
/




