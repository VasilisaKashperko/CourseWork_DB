-------------- Добавление жильца собственника --------------

CREATE OR REPLACE PROCEDURE InsertOwner(
   p_Surname IN NVARCHAR2,
   p_Name IN NVARCHAR2,
   p_Patronymic IN NVARCHAR2,
   p_AdditionalInfo IN NVARCHAR2,
   p_CurrentDebt IN NUMBER,
   p_PhoneNumberId IN NUMBER,
   p_OwnerStatusId IN NUMBER,
   p_FlatId IN NUMBER
)
IS
BEGIN
   INSERT INTO C##Vasilisa.Owners (Surname, Name, Patronymic, AdditionalInfo, CurrentDebt, PhoneNumberId, OwnerStatusId, FlatId)
   VALUES (p_Surname, p_Name, p_Patronymic, p_AdditionalInfo, p_CurrentDebt, p_PhoneNumberId, p_OwnerStatusId, p_FlatId);

   COMMIT;
   DBMS_OUTPUT.PUT_LINE('Owner inserted successfully.');
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error inserting owner: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.InsertOwner('Smith', 'John', 'Robertovich', 'Additional info', 5000, 1, 1, 4);
END;

-------------- Изменение данных жильца собственника --------------

CREATE OR REPLACE PROCEDURE AlterOwner(
   p_OwnerId          IN NUMBER,
   p_Surname          IN NVARCHAR2,
   p_Name             IN NVARCHAR2,
   p_Patronymic       IN NVARCHAR2,
   p_AdditionalInfo   IN NVARCHAR2,
   p_CurrentDebt      IN NUMBER,
   p_PhoneNumberId    IN NUMBER,
   p_OwnerStatusId    IN NUMBER,
   p_FlatId           IN NUMBER
)
IS
BEGIN
   UPDATE C##Vasilisa.Owners
   SET Surname = p_Surname,
       Name = p_Name,
       Patronymic = p_Patronymic,
       AdditionalInfo = p_AdditionalInfo,
       CurrentDebt = p_CurrentDebt,
       PhoneNumberId = p_PhoneNumberId,
       OwnerStatusId = p_OwnerStatusId,
       FlatId = p_FlatId
   WHERE OwnerId = p_OwnerId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Owner with ID ' || p_OwnerId || ' not found.');
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('Owner altered successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering owner: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.AlterOwner(2,'Smith', 'John', 'Robertovich', 'Additional info', 5000, 1, 1, 1);
END;

-------------- Удаление жильца собственника --------------

CREATE OR REPLACE PROCEDURE DeleteOwner(
   p_OwnerId IN NUMBER
)
IS
BEGIN
   DELETE FROM C##Vasilisa.Owners
   WHERE OwnerId = p_OwnerId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Owner with ID ' || p_OwnerId || ' not found.');
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('Owner deleted successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error deleting owner: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.DeleteOwner(2);
END;

-------------- Удаление всех жильцов собственников --------------

CREATE OR REPLACE PROCEDURE DeleteAllOwners
IS
BEGIN
   BEGIN
      DELETE FROM C##Vasilisa.Owners;

      IF SQL%ROWCOUNT = 0 THEN
         DBMS_OUTPUT.PUT_LINE('No owners found.');
         ROLLBACK;
      ELSE
         COMMIT;
         DBMS_OUTPUT.PUT_LINE('All owners deleted successfully.');
      END IF;
   EXCEPTION
      WHEN OTHERS THEN
         ROLLBACK;
         DBMS_OUTPUT.PUT_LINE('Error deleting owners: ' || SQLERRM);
         RAISE;
   END;
EXCEPTION
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error deleting owners: ' || SQLERRM);
      RAISE;
END;

BEGIN
   DeleteAllOwners();
END;

-------------- Найти жильцов собственников по фамилии --------------

CREATE OR REPLACE PROCEDURE FindOwnerBySurname(
   p_Surname IN NVARCHAR2
)
IS
   v_OwnerId Owners.OwnerId%TYPE;
   v_Name Owners.Name%TYPE;
   v_Patronymic Owners.Patronymic%TYPE;
   v_AdditionalInfo Owners.AdditionalInfo%TYPE;
   v_CurrentDebt Owners.CurrentDebt%TYPE;
   v_PhoneNumberId Owners.PhoneNumberId%TYPE;
   v_OwnerStatusId Owners.OwnerStatusId%TYPE;
   v_FlatId Owners.FlatId%TYPE;
   
   CURSOR c_Owners IS
      SELECT OwnerId, Name, Patronymic, AdditionalInfo, CurrentDebt, PhoneNumberId, OwnerStatusId, FlatId
      FROM C##Vasilisa.Owners
      WHERE Surname = p_Surname;
BEGIN
   BEGIN
      OPEN c_Owners;
      LOOP
         FETCH c_Owners INTO v_OwnerId, v_Name, v_Patronymic, v_AdditionalInfo, v_CurrentDebt, v_PhoneNumberId, v_OwnerStatusId, v_FlatId;
         EXIT WHEN c_Owners%NOTFOUND;
         DBMS_OUTPUT.PUT_LINE('-------------------------');
         DBMS_OUTPUT.PUT_LINE('Owner ID: ' || v_OwnerId);
         DBMS_OUTPUT.PUT_LINE('Name: ' || v_Name);
         DBMS_OUTPUT.PUT_LINE('Patronymic: ' || v_Patronymic);
         DBMS_OUTPUT.PUT_LINE('Additional Info: ' || v_AdditionalInfo);
         DBMS_OUTPUT.PUT_LINE('Current Debt: ' || v_CurrentDebt);
         DBMS_OUTPUT.PUT_LINE('Phone Number ID: ' || v_PhoneNumberId);
         DBMS_OUTPUT.PUT_LINE('Owner Status ID: ' || v_OwnerStatusId);
         DBMS_OUTPUT.PUT_LINE('Flat ID: ' || v_FlatId);
         DBMS_OUTPUT.PUT_LINE('-------------------------');
      END LOOP;
      CLOSE c_Owners;
   EXCEPTION
      WHEN NO_DATA_FOUND THEN
         DBMS_OUTPUT.PUT_LINE('No owners found with surname ' || p_Surname);
      WHEN OTHERS THEN
         DBMS_OUTPUT.PUT_LINE('Error finding owners: ' || SQLERRM);
   END;
EXCEPTION
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error finding owners: ' || SQLERRM);
END;

BEGIN
   C##Vasilisa.FindOwnerBySurname('Smith');
END;

-------------- Найти жильцов собственников номеру квартиры и названию дома --------------

CREATE OR REPLACE PROCEDURE FindOwnerByFlatNumber(
   p_FlatNumber IN NUMBER,
   p_HouseName IN NVARCHAR2
)
IS
   v_OwnerName NVARCHAR2(50);
BEGIN
   SELECT O.Surname || ', ' || O.Name AS OwnerName
   INTO v_OwnerName
   FROM C##Vasilisa.Owners O
   JOIN C##Vasilisa.Flats F ON O.FlatId = F.FlatId
   JOIN C##Vasilisa.Porches P ON F.PorchId = P.PorchId
   JOIN C##Vasilisa.Houses H ON P.HouseId = H.HouseId
   WHERE F.FlatNumber = p_FlatNumber
   AND H.HouseName = p_HouseName;

   IF v_OwnerName IS NULL THEN
      DBMS_OUTPUT.PUT_LINE('Owner not found.');
   ELSE
      DBMS_OUTPUT.PUT_LINE('Owner: ' || v_OwnerName);
   END IF;
EXCEPTION
   WHEN NO_DATA_FOUND THEN
      DBMS_OUTPUT.PUT_LINE('Owner not found.');
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error finding owner: ' || SQLERRM);
END;

BEGIN
   C##Vasilisa.FindOwnerByFlatNumber(1, 'Sample House');
END;

-------------- Вывод всех жильцов собственников --------------

SELECT * FROM C##Vasilisa.Owners;