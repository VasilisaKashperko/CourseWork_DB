-------------- Процедура экспорта данных в XML --------------

CREATE OR REPLACE PROCEDURE ExportHousesToXML
IS
   v_xml XMLType;
   v_clob CLOB;
   v_file UTL_FILE.FILE_TYPE;
BEGIN
   -- Выборка данных из таблицы Houses в XMLType
   SELECT XMLElement(
             "Houses",
             XMLAgg(
                XMLElement(
                   "House",
                   XMLForest(
                      HouseId AS "HouseId",
                      HouseName AS "HouseName",
                      NumberOfFlats AS "NumberOfFlats",
                      NumberOfPorches AS "NumberOfPorches",
                      AddressId AS "AddressId",
                      UserId AS "UserId"
                   )
                )
             )
          )
   INTO v_xml
   FROM Houses;

   -- Преобразование XMLType в CLOB
   v_clob := v_xml.getClobVal();

   -- Создание и запись XML в файл
   v_file := UTL_FILE.FOPEN('EXPORT_DIR', 'houses.xml', 'W');
   UTL_FILE.PUT_RAW(v_file, UTL_RAW.CAST_TO_RAW(v_clob));
   UTL_FILE.FCLOSE(v_file);
   
   DBMS_OUTPUT.PUT_LINE('Export completed successfully.');
EXCEPTION
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error occurred while exporting houses to XML.');
      DBMS_OUTPUT.PUT_LINE(SQLERRM);
END;

-------------- Процедура импорта данных из XML --------------

CREATE OR REPLACE PROCEDURE ImportHousesFromXML
IS
   v_xml XMLType;
   v_house_name NVARCHAR2(20);
   v_number_of_flats NUMBER;
   v_number_of_porches NUMBER;
   v_address_id NUMBER;
   v_user_id NUMBER;
BEGIN
   -- Чтение XML из файла
   SELECT XMLType(
             UTL_RAW.CAST_TO_VARCHAR2(UTL_FILE.READ_RAW('IMPORT_DIR', 'houses.xml', UTL_FILE.GET_FILE_SIZE('IMPORT_DIR', 'houses.xml')))
          )
   INTO v_xml
   FROM dual;

   -- Импорт данных из XML в таблицу
   FOR r IN (
      SELECT ExtractValue(value(h), '/House/HouseName') AS house_name,
             ExtractValue(value(h), '/House/NumberOfFlats') AS number_of_flats,
             ExtractValue(value(h), '/House/NumberOfPorches') AS number_of_porches,
             ExtractValue(value(h), '/House/AddressId') AS address_id,
             ExtractValue(value(h), '/House/UserId') AS user_id
      FROM TABLE(XMLSequence(v_xml.extract('/Houses/House'))) h
   )
   LOOP
      v_house_name := r.house_name;
      v_number_of_flats := TO_NUMBER(r.number_of_flats);
      v_number_of_porches := TO_NUMBER(r.number_of_porches);
      v_address_id := TO_NUMBER(r.address_id);
      v_user_id := TO_NUMBER(r.user_id);
      
      -- Вставка данных в таблицу Houses
      INSERT INTO Houses (HouseName, NumberOfFlats, NumberOfPorches, AddressId, UserId)
      VALUES (v_house_name, v_number_of_flats, v_number_of_porches, v_address_id, v_user_id);
   END LOOP;
   
   DBMS_OUTPUT.PUT_LINE('Import completed successfully.');
EXCEPTION
   WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Error occurred while importing houses from XML.');
      DBMS_OUTPUT.PUT_LINE(SQLERRM);
END;