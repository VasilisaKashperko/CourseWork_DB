-------------- Добавление подъезда --------------

CREATE OR REPLACE PROCEDURE InsertPorche (
   p_PorchNumber IN Porches.PorchNumber%TYPE,
   p_HouseId IN Porches.HouseId%TYPE
) AS
BEGIN
   BEGIN
      INSERT INTO C##Vasilisa.Porches (PorchNumber, HouseId)
      VALUES (p_PorchNumber, p_HouseId);
   
      COMMIT;
   EXCEPTION
      WHEN INVALID_NUMBER THEN
         DBMS_OUTPUT.PUT_LINE('Invalid HouseId provided: ' || p_HouseId);
         ROLLBACK;
      WHEN OTHERS THEN
         DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
         ROLLBACK;
   END;
END;

BEGIN
   C##Vasilisa.InsertPorche(1, 1);
END;

-------------- Добавление подъездов и квартир по id дома --------------

CREATE OR REPLACE PROCEDURE InsertPorchesByHouseId (
   p_HouseId IN Porches.HouseId%TYPE
) AS
   v_Amount NUMBER;
   v_NumberOfPorches NUMBER;
   v_PorchId Porches.PorchId%TYPE;
BEGIN
   SELECT NumberOfFlats / NumberOfPorches INTO v_Amount
   FROM Houses
   WHERE HouseId = p_HouseId;

   SELECT NumberOfPorches INTO v_NumberOfPorches
   FROM Houses
   WHERE HouseId = p_HouseId;

   FOR i IN 1..v_NumberOfPorches LOOP
      BEGIN
         INSERT INTO Porches (PorchNumber, HouseId)
         VALUES (i, p_HouseId)
         RETURNING PorchId INTO v_PorchId;

         FOR j IN 1..v_Amount LOOP
            INSERT INTO Flats (FlatNumber, PorchId)
            VALUES (((i - 1) * v_Amount) + j, v_PorchId);
         END LOOP;

         COMMIT;
         DBMS_OUTPUT.PUT_LINE('Porch ' || i || ' created successfully.');
      EXCEPTION
         WHEN OTHERS THEN
            ROLLBACK;
            DBMS_OUTPUT.PUT_LINE('Error creating porch: ' || SQLERRM);
            RAISE;
      END;
   END LOOP;
EXCEPTION
   WHEN NO_DATA_FOUND THEN
      DBMS_OUTPUT.PUT_LINE('House with ID ' || p_HouseId || ' not found.');
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
END;

BEGIN
   InsertPorchesByHouseId(2);
END;

-------------- Изменение данных подъезда --------------

CREATE OR REPLACE PROCEDURE AlterPorch(
   p_PorchId IN INTEGER,
   p_PorchNumber IN INTEGER,
   p_HouseId IN INTEGER
)
IS
BEGIN
   UPDATE C##Vasilisa.Porches
   SET PorchNumber = p_PorchNumber,
       HouseId = p_HouseId
   WHERE PorchId = p_PorchId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Porche with ID ' || p_PorchId || ' not found.');
      ROLLBACK;
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('Porche altered successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering porche: ' || SQLERRM);
      RAISE;
END;

BEGIN
   C##Vasilisa.AlterPorch(1, 10, 1);
END;

-------------- Удаление подъезда --------------

CREATE OR REPLACE PROCEDURE DeletePorch(
   p_PorchId IN INTEGER
)
IS
BEGIN
   DELETE FROM C##Vasilisa.Porches
   WHERE PorchId = p_PorchId;

   IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Porch with ID ' || p_PorchId || ' not found.');
   ELSE
      COMMIT;
      DBMS_OUTPUT.PUT_LINE('Porch deleted successfully.');
   END IF;
EXCEPTION
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error deleting porch: ' || SQLERRM);
      RAISE;
END;

BEGIN
    C##Vasilisa.DeletePorch(1);
END;

-------------- Удаление всех подъездов --------------

CREATE OR REPLACE PROCEDURE DeleteAllPorches
IS
BEGIN
   BEGIN
      DELETE FROM C##Vasilisa.Porches;

      IF SQL%ROWCOUNT = 0 THEN
         DBMS_OUTPUT.PUT_LINE('No porches found.');
         ROLLBACK;
      ELSE
         COMMIT;
         DBMS_OUTPUT.PUT_LINE('All porches deleted successfully.');
      END IF;
   EXCEPTION
      WHEN OTHERS THEN
         ROLLBACK;
         DBMS_OUTPUT.PUT_LINE('Error deleting porches: ' || SQLERRM);
         RAISE;
   END;
EXCEPTION
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error deleting addresses: ' || SQLERRM);
      RAISE;
END;

BEGIN
    DeleteAllPorches();
END;

-------------- Вывод всех подъездов --------------

SELECT * FROM C##Vasilisa.Porches;