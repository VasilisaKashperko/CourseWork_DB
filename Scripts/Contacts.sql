-------------- Добавление контакта --------------

CREATE OR REPLACE PROCEDURE InsertContact(
   p_Surname IN NVARCHAR2,
   p_Name IN NVARCHAR2,
   p_Patronymic IN NVARCHAR2,
   p_Position IN NVARCHAR2,
   p_PhoneNumberId IN NUMBER,
   p_UserId IN NUMBER
)
IS
BEGIN
   INSERT INTO C##Vasilisa.Contacts (Surname, Name, Patronymic, Position, PhoneNumberId, UserId)
   VALUES (p_Surname, p_Name, p_Patronymic, p_Position, p_PhoneNumberId, p_UserId);

   COMMIT;

   DBMS_OUTPUT.PUT_LINE('Contact inserted successfully.');
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error inserting contact: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.InsertContact('Smith', 'John', 'Doe', 'Manager', 1, 1);
END;

-------------- Изменение контакта --------------

CREATE OR REPLACE PROCEDURE AlterContact(
   p_ContactId IN NUMBER,
   p_Surname IN NVARCHAR2,
   p_Name IN NVARCHAR2,
   p_Patronymic IN NVARCHAR2,
   p_Position IN NVARCHAR2,
   p_PhoneNumberId IN NUMBER,
   p_UserId IN NUMBER
)
IS
BEGIN
   UPDATE C##Vasilisa.Contacts
   SET Surname = p_Surname,
       Name = p_Name,
       Patronymic = p_Patronymic,
       Position = p_Position,
       PhoneNumberId = p_PhoneNumberId,
       UserId = p_UserId
   WHERE ContactId = p_ContactId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Contact with ID ' || p_ContactId || ' not found.');
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('Contact altered successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering contact: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.AlterContact(1, 'Smith', 'John', 'Doe', 'Manager', 1, 1);
END;

-------------- Удаление контакта --------------

CREATE OR REPLACE PROCEDURE DeleteContact(
   p_ContactId IN NUMBER
)
IS
BEGIN
   DELETE FROM C##Vasilisa.Contacts
   WHERE ContactId = p_ContactId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Contact with ID ' || p_ContactId || ' not found.');
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('Contact deleted successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error deleting contact: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.DeleteContact(2);
END;

-------------- Удаление всех контактов --------------

CREATE OR REPLACE PROCEDURE DeleteAllPhoneNumbers
IS
BEGIN
   BEGIN
      DELETE FROM C##Vasilisa.PhoneNumbers;

      IF SQL%ROWCOUNT = 0 THEN
         DBMS_OUTPUT.PUT_LINE('No phone numbers found.');
         ROLLBACK;
      ELSE
         COMMIT;
         DBMS_OUTPUT.PUT_LINE('All phone numbers deleted successfully.');
      END IF;
   EXCEPTION
      WHEN OTHERS THEN
         ROLLBACK;
         DBMS_OUTPUT.PUT_LINE('Error deleting phone numbers: ' || SQLERRM);
         RAISE;
   END;
EXCEPTION
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error deleting phone numbers: ' || SQLERRM);
      RAISE;
END;

BEGIN
   DeleteAllPhoneNumbers();
END;

-------------- Вывод всех контактов --------------

SELECT * FROM C##Vasilisa.Contacts;