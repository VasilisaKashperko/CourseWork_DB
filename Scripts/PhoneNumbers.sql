-------------- Добавление номера --------------

CREATE OR REPLACE PROCEDURE InsertPhoneNumber(
   p_MobilePhone IN NVARCHAR2,
   p_HomePhone IN NVARCHAR2,
   p_AdditionalPhone IN NVARCHAR2
)
IS
BEGIN
   INSERT INTO C##Vasilisa.PhoneNumbers (MobilePhone, HomePhone, AdditionalPhone)
   VALUES (p_MobilePhone, p_HomePhone, p_AdditionalPhone);
   
   COMMIT;
   
   DBMS_OUTPUT.PUT_LINE('Phone number inserted successfully.');
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error inserting phone number: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.InsertPhoneNumber('+375123456789', '123456', '+375987654321');
END;

-------------- Изменение номера --------------

CREATE OR REPLACE PROCEDURE AlterPhoneNumber(
   p_PhoneNumberId IN NUMBER,
   p_MobilePhone IN NVARCHAR2,
   p_HomePhone IN NVARCHAR2,
   p_AdditionalPhone IN NVARCHAR2
)
IS
BEGIN
   UPDATE C##Vasilisa.PhoneNumbers
   SET MobilePhone = p_MobilePhone,
       HomePhone = p_HomePhone,
       AdditionalPhone = p_AdditionalPhone
   WHERE PhoneNumberId = p_PhoneNumberId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Phone number with ID ' || p_PhoneNumberId || ' not found.');
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('Phone number altered successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering phone number: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.AlterPhoneNumber(2, '+375123456789', '123456', '+375987654321');
END;

-------------- Удаление номера --------------

CREATE OR REPLACE PROCEDURE DeletePhoneNumber(
   p_PhoneNumberId IN NUMBER
)
IS
BEGIN
   DELETE FROM C##Vasilisa.PhoneNumbers
   WHERE PhoneNumberId = p_PhoneNumberId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Phone number with ID ' || p_PhoneNumberId || ' not found.');
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('Phone number deleted successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error deleting phone number: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.DeletePhoneNumber(1);
END;

-------------- Удаление всех номеров --------------

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

-------------- Вывод всех номеров --------------

SELECT * FROM C##Vasilisa.PhoneNumbers;