-------------- Добавление адреса --------------

CREATE OR REPLACE PROCEDURE InsertAddress(
    p_Country IN NVARCHAR2,
    p_City IN NVARCHAR2,
    p_District IN NVARCHAR2,
    p_Street IN NVARCHAR2,
    p_HouseNumber IN NUMBER,
    p_HousingNumber IN NVARCHAR2
)
AS
BEGIN
    INSERT INTO C##Vasilisa.Addresses (Country, City, District, Street, HouseNumber, HousingNumber)
    VALUES (p_Country, p_City, p_District, p_Street, p_HouseNumber, p_HousingNumber);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Address inserted successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error occurred while inserting the address.');
        DBMS_OUTPUT.PUT_LINE(SQLERRM);
END;

BEGIN
    C##Vasilisa.InsertAddress('Country1', 'City1', 'District1', 'Street1', 123, 'A');
END;

-------------- Изменение данных адреса --------------

CREATE OR REPLACE PROCEDURE AlterAddress(
    p_AddressId IN NUMBER,
    p_Country IN NVARCHAR2,
    p_City IN NVARCHAR2,
    p_District IN NVARCHAR2,
    p_Street IN NVARCHAR2,
    p_HouseNumber IN NUMBER,
    p_HousingNumber IN NVARCHAR2
)
AS
    v_Count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_Count
    FROM C##Vasilisa.Addresses
    WHERE AddressId = p_AddressId;

    IF v_Count = 0 THEN
        DBMS_OUTPUT.PUT_LINE('Address with AddressId ' || p_AddressId || ' not found.');
        RETURN;
    END IF;

    UPDATE C##Vasilisa.Addresses
    SET Country = p_Country,
        City = p_City,
        District = p_District,
        Street = p_Street,
        HouseNumber = p_HouseNumber,
        HousingNumber = p_HousingNumber
    WHERE AddressId = p_AddressId;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Address updated successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error occurred while updating the address.');
        DBMS_OUTPUT.PUT_LINE(SQLERRM);
END;

BEGIN
    C##Vasilisa.AlterAddress(1, 'NewCountry', 'NewCity', 'NewDistrict', 'NewStreet', 456, 'B');
END;

-------------- Удаление адреса --------------

CREATE OR REPLACE PROCEDURE DeleteAddress(
   p_AddressId IN NUMBER
)
IS
BEGIN
   DELETE FROM C##Vasilisa.Addresses
   WHERE AddressId = p_AddressId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Address with ID ' || p_AddressId || ' not found.');
      ROLLBACK;
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('Address deleted successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error deleting address: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.DeleteAddress(1);
END;

-------------- Удаление всех адресов --------------

CREATE OR REPLACE PROCEDURE DeleteAllAddresses
IS
BEGIN
   BEGIN
      DELETE FROM C##Vasilisa.Addresses;

      IF SQL%ROWCOUNT = 0 THEN
         DBMS_OUTPUT.PUT_LINE('No addresses found.');
         ROLLBACK;
      ELSE
         COMMIT;
         DBMS_OUTPUT.PUT_LINE('All addresses deleted successfully.');
      END IF;
   EXCEPTION
      WHEN OTHERS THEN
         ROLLBACK;
         DBMS_OUTPUT.PUT_LINE('Error deleting addresses: ' || SQLERRM);
         RAISE;
   END;
EXCEPTION
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error deleting addresses: ' || SQLERRM);
      RAISE;
END;

BEGIN
   DeleteAllAddresses;
END;

-------------- Вывод всех адресов --------------

SELECT * FROM C##Vasilisa.Addresses;