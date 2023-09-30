-------------- Роль администратора --------------

--drop role C##Administrator;
--drop role C##Chairman;
--drop role C##Accountant;

create role C##Administrator;

GRANT ALL PRIVILEGES ON Addresses TO C##Administrator;
GRANT ALL PRIVILEGES ON Contacts TO C##Administrator;
GRANT ALL PRIVILEGES ON Flats TO C##Administrator;
GRANT ALL PRIVILEGES ON Houses TO C##Administrator;
GRANT ALL PRIVILEGES ON Owners TO C##Administrator;
GRANT ALL PRIVILEGES ON PhoneNumbers TO C##Administrator;
GRANT ALL PRIVILEGES ON Porches TO C##Administrator;
GRANT ALL PRIVILEGES ON Users TO C##Administrator;
GRANT CREATE SESSION TO C##Administrator;
GRANT DBA TO C##Administrator;

-------------- Роль председателя --------------

create role C##Chairman;

GRANT ALL PRIVILEGES ON C##Vasilisa.Houses TO C##Chairman;
GRANT ALL PRIVILEGES ON C##Vasilisa.Addresses TO C##Chairman;
GRANT ALL PRIVILEGES ON C##Vasilisa.Porches TO C##Chairman;
GRANT ALL PRIVILEGES ON C##Vasilisa.Flats TO C##Chairman;
GRANT ALL PRIVILEGES ON C##Vasilisa.Contacts TO C##Chairman;
GRANT ALL PRIVILEGES ON C##Vasilisa.Owners TO C##Chairman;
GRANT ALL PRIVILEGES ON C##Vasilisa.PhoneNumbers TO C##Chairman;
GRANT CREATE SESSION TO C##Chairman;

-------------- Роль бухгалетра --------------

create role C##Accountant;

GRANT SELECT ON C##Vasilisa.Houses TO C##Accountant;
GRANT SELECT ON C##Vasilisa.Addresses TO C##Accountant;
GRANT SELECT ON C##Vasilisa.Porches TO C##Accountant;
GRANT SELECT ON C##Vasilisa.Flats TO C##Accountant;
GRANT SELECT ON C##Vasilisa.PhoneNumbers TO C##Accountant;
GRANT ALL PRIVILEGES ON C##Vasilisa.Owners TO C##Accountant;
GRANT ALL PRIVILEGES ON C##Vasilisa.Contacts TO C##Accountant;
GRANT CREATE SESSION TO C##Accountant;

-------------- Пользователи --------------

--DROP USER C##Vasilisa CASCADE;
--DROP USER C##Youri;
--DROP USER C##Lisa;

CREATE USER C##Vasilisa IDENTIFIED BY Pa$$w0rd;
GRANT C##Administrator TO C##Vasilisa;
GRANT UNLIMITED TABLESPACE TO C##Vasilisa;
GRANT EXECUTE ANY PROCEDURE TO C##Vasilisa;
SET ROLE C##Administrator;

CREATE USER C##Youri IDENTIFIED BY Pa$$w0rd;
GRANT C##Chairman TO C##Youri;
GRANT UNLIMITED TABLESPACE TO C##Youri;
GRANT EXECUTE ANY PROCEDURE TO C##Youri;
SET ROLE C##Chairman;

CREATE USER C##Lisa IDENTIFIED BY Pa$$w0rd;
GRANT C##Accountant TO C##Lisa;
GRANT UNLIMITED TABLESPACE TO C##Lisa;
SET ROLE C##Accountant;

-------------- Просмотр пользователей --------------

SELECT username FROM all_users;
SELECT * FROM dba_role_privs;
SELECT DISTINCT TABLE_NAME FROM DBA_TAB_PRIVS WHERE GRANTEE = 'C##ACCOUNTANT';
SELECT username FROM dba_users WHERE account_status = 'OPEN';

-------------- Проверка таблиц --------------

SELECT * FROM C##Vasilisa.addresses;