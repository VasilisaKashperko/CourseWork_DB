-------------- Добавление пользователя --------------

CREATE OR REPLACE PROCEDURE InsertUser(
   p_Surname IN NVARCHAR2,
   p_Name IN NVARCHAR2,
   p_Patronymic IN NVARCHAR2,
   p_Login IN NVARCHAR2,
   p_Password IN NVARCHAR2,
   p_Role IN NUMBER,
   p_AccountantId IN NUMBER
)
IS
BEGIN
   INSERT INTO C##Vasilisa.Users (Surname, Name, Patronymic, Login, Password, Role, AccountantId)
   VALUES (p_Surname, p_Name, p_Patronymic, p_Login, p_Password, p_Role, p_AccountantId);
   
   COMMIT;
   
   DBMS_OUTPUT.PUT_LINE('User inserted successfully.');
EXCEPTION
   WHEN DUP_VAL_ON_INDEX THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('User with the same login already exists.');
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error inserting user: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.InsertUser('Кашперко', 'Василиса', 'Сергеевна', 'vasilisa1', 'vasilisa1', 0, 0);
END;

BEGIN
   C##Vasilisa.InsertUser('Кашперко', 'Сергей', 'Олегович', 'sergey1', 'sergey1', 1, 1);
END;

-------------- Изменение данных пользователя --------------

CREATE OR REPLACE PROCEDURE AlterUser(
   p_UserId IN NUMBER,
   p_Surname IN NVARCHAR2,
   p_Name IN NVARCHAR2,
   p_Patronymic IN NVARCHAR2,
   p_Login IN NVARCHAR2,
   p_Password IN NVARCHAR2,
   p_Role IN NUMBER,
   p_AccountantId IN NUMBER
)
IS
BEGIN
   UPDATE C##Vasilisa.Users
   SET Surname = p_Surname,
       Name = p_Name,
       Patronymic = p_Patronymic,
       Login = p_Login,
       Password = p_Password,
       Role = p_Role,
       AccountantId = p_AccountantId
   WHERE UserId = p_UserId;
   
   IF SQL%ROWCOUNT = 0 THEN
      RAISE NO_DATA_FOUND;
   END IF;
   
   COMMIT;
   
   DBMS_OUTPUT.PUT_LINE('User altered successfully.');
EXCEPTION
   WHEN NO_DATA_FOUND THEN
      DBMS_OUTPUT.PUT_LINE('User with ID ' || p_UserId || ' not found.');
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering user: ' || SQLERRM);
END;

BEGIN
   C##Vasilisa.AlterUser(3, 'Кашперко', 'Сергей', 'Олегович', 'sergey1', 'sergey1', 1, 2);
END;

-------------- Удаление пользователя --------------

CREATE OR REPLACE PROCEDURE DeleteUser(
   p_UserId IN NUMBER
)
IS
BEGIN
   DELETE FROM C##Vasilisa.Users
   WHERE UserId = p_UserId;
   
   IF SQL%ROWCOUNT = 0 THEN
      RAISE NO_DATA_FOUND;
   END IF;

   COMMIT;

   DBMS_OUTPUT.PUT_LINE('User deleted successfully.');
EXCEPTION
   WHEN NO_DATA_FOUND THEN
      DBMS_OUTPUT.PUT_LINE('User with ID ' || p_UserId || ' not found.');
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error deleting user: ' || SQLERRM);
END;

BEGIN
   C##Vasilisa.DeleteUser(2);
END;

-------------- Удаление всех пользователей --------------

CREATE OR REPLACE PROCEDURE DeleteAllUsers
IS
BEGIN
   BEGIN
      DELETE FROM C##Vasilisa.Users;

      IF SQL%ROWCOUNT = 0 THEN
         DBMS_OUTPUT.PUT_LINE('No users found.');
         ROLLBACK;
      ELSE
         COMMIT;
         DBMS_OUTPUT.PUT_LINE('All users deleted successfully.');
      END IF;
   EXCEPTION
      WHEN OTHERS THEN
         ROLLBACK;
         DBMS_OUTPUT.PUT_LINE('Error deleting users: ' || SQLERRM);
   END;
EXCEPTION
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error deleting users: ' || SQLERRM);
END;

BEGIN
   DeleteAllUsers;
END;

-------------- Найти пользователя по фамилии --------------

CREATE OR REPLACE PROCEDURE FindUserBySurname(
   p_Surname IN NVARCHAR2
)
IS
   CURSOR c_Users IS
      SELECT *
      FROM C##Vasilisa.Users
      WHERE Surname = p_Surname;
   
   v_User Users%ROWTYPE;
   v_UserFound BOOLEAN := FALSE;
BEGIN
   DBMS_OUTPUT.PUT_LINE('-----------------------');
   DBMS_OUTPUT.PUT_LINE('Founded user(s):');
   DBMS_OUTPUT.PUT_LINE('-----------------------');
   OPEN c_Users;
   
   LOOP
      FETCH c_Users INTO v_User;
      EXIT WHEN c_Users%NOTFOUND;
      
      v_UserFound := TRUE;
      
      DBMS_OUTPUT.PUT_LINE('User ID: ' || v_User.UserId);
      DBMS_OUTPUT.PUT_LINE('Name: ' || v_User.Name);
      DBMS_OUTPUT.PUT_LINE('Surname: ' || v_User.Surname);
      
      DBMS_OUTPUT.PUT_LINE('-----------------------');
   END LOOP;
   
   CLOSE c_Users;
   
   IF NOT v_UserFound THEN
      DBMS_OUTPUT.PUT_LINE('No user found with the provided surname: ' || p_Surname);
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error finding user: ' || SQLERRM);
END;

BEGIN
   C##Vasilisa.FindUserBySurname('Doe');
END;

-------------- Найти пользователя по идентификатору --------------

CREATE OR REPLACE PROCEDURE FindUserById(
   p_UserId IN NUMBER
)
IS
   CURSOR c_Users (p_UserId NUMBER) IS
      SELECT *
      FROM C##Vasilisa.Users
      WHERE UserId = p_UserId;
   
   v_User Users%ROWTYPE;
BEGIN
   DBMS_OUTPUT.PUT_LINE('-----------------------');
   DBMS_OUTPUT.PUT_LINE('Founded user:');
   DBMS_OUTPUT.PUT_LINE('-----------------------');
   
   OPEN c_Users(p_UserId);
   
   FETCH c_Users INTO v_User;
   
   IF c_Users%FOUND THEN
      LOOP
         DBMS_OUTPUT.PUT_LINE('User ID: ' || v_User.UserId);
         DBMS_OUTPUT.PUT_LINE('Name: ' || v_User.Name);
         DBMS_OUTPUT.PUT_LINE('Surname: ' || v_User.Surname);
      
         DBMS_OUTPUT.PUT_LINE('-----------------------');
         
         FETCH c_Users INTO v_User;
         EXIT WHEN c_Users%NOTFOUND;
      END LOOP;
   ELSE
      DBMS_OUTPUT.PUT_LINE('No user found with the provided ID: ' || p_UserId);
   END IF;
   
   CLOSE c_Users;

EXCEPTION
   WHEN NO_DATA_FOUND THEN
      DBMS_OUTPUT.PUT_LINE('No user found with the provided ID: ' || p_UserId);
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error finding user: ' || SQLERRM);
END;

BEGIN
   C##Vasilisa.FindUserById(1);
END;

-------------- Вывод всех пользователей --------------

SELECT * FROM USERS order by UserId;