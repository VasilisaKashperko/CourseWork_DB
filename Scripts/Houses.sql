-------------- Добавление дома --------------

CREATE OR REPLACE PROCEDURE InsertHouse(
   p_HouseName IN NVARCHAR2,
   p_NumberOfFlats IN NUMBER,
   p_NumberOfPorches IN NUMBER,
   p_AddressId IN NUMBER,
   p_UserId IN NUMBER
)
IS
BEGIN
   INSERT INTO C##Vasilisa.Houses (HouseName, NumberOfFlats, NumberOfPorches, AddressId, UserId)
   VALUES (p_HouseName, p_NumberOfFlats, p_NumberOfPorches, p_AddressId, p_UserId);
   
   COMMIT;
   
   DBMS_OUTPUT.PUT_LINE('House inserted successfully.');
EXCEPTION
   WHEN NO_DATA_FOUND THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error inserting house: No data found.');
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error inserting house: ' || SQLERRM);
END;

BEGIN
   C##Vasilisa.InsertHouse('Sample House', 10, 2, 1, 1);
END;

-------------- Изменение данных дома --------------

CREATE OR REPLACE PROCEDURE AlterHouse(
   p_HouseId IN NUMBER,
   p_HouseName IN NVARCHAR2,
   p_NumberOfFlats IN NUMBER,
   p_NumberOfPorches IN NUMBER,
   p_AddressId IN NUMBER,
   p_UserId IN NUMBER
)
IS
BEGIN
   UPDATE C##Vasilisa.Houses
   SET HouseName = p_HouseName,
       NumberOfFlats = p_NumberOfFlats,
       NumberOfPorches = p_NumberOfPorches,
       AddressId = p_AddressId,
       UserId = p_UserId
   WHERE HouseId = p_HouseId;
   
   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Error altering house: House with HouseId ' || p_HouseId || ' not found.');
      ROLLBACK;
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('House altered successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering house: ' || SQLERRM);
END;

BEGIN
   C##Vasilisa.AlterHouse(6,'Altered House', 10, 2, 2, 3);
END;

-------------- Удаление дома --------------

CREATE OR REPLACE PROCEDURE DeleteHouse(
   p_HouseId IN NUMBER
)
IS
BEGIN
   DELETE FROM C##Vasilisa.Houses
   WHERE HouseId = p_HouseId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('House with ID ' || p_HouseId || ' not found.');
      ROLLBACK;
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('House deleted successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error deleting house: ' || SQLERRM);
END;

BEGIN
   C##Vasilisa.DeleteHouse(1);
END;

-------------- Удаление всех домов --------------

CREATE OR REPLACE PROCEDURE DeleteAllHouses
IS
BEGIN
   DELETE FROM C##Vasilisa.Houses;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('No houses found.');
      ROLLBACK;
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('All houses deleted successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error deleting houses: ' || SQLERRM);
END;

BEGIN
   DeleteAllHouses;
END;

-------------- Вывод всех домов --------------

SELECT * FROM C##Vasilisa.Houses;