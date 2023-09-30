-- включаем вывод данных в output
set serveroutput on;

declare
  v_t1 number; -- время начала
  v_t2 number; -- время завершения
  v_cpu1 number; -- время ЦПУ до 
  v_cpu2 number; -- время ЦПУ после 
  v_t_res number; -- общее время выполнения
  v_cpu_res number; -- общее время выполнения CPU

begin
  
  -- фиксируем начало
  -- помещаем общее время и время ЦПУ в переменные
  select t.hsecs
        ,dbms_utility.get_cpu_time
    into v_t1 
        ,v_cpu1
    from v$timer t;
    
  -- запустим 100000 раз цикл, выполняющий процедуру вставки в таблицу Addresses
  begin
  for i IN 1..100000
    loop
      C##Vasilisa.InsertAddress('TEST', 'TEST', 'TEST', 'TEST', 123, 'A');
    end loop;
  end;
  
  -- фиксируем завершение
  -- снова помещаем общее время и время ЦПУ в переменные
  select t.hsecs
        ,dbms_utility.get_cpu_time
    into v_t2 
        ,v_cpu2
    from v$timer t;
    
  -- считаем общее время выполнения в сотых долях секунды
  v_t_res := v_t2 - v_t1;
  
  -- считаем общее время ЦПУ в сотых долях секунды
  v_cpu_res := v_cpu2 - v_cpu1;
 
  -- вывод результата на экран
  dbms_output.put_line('Общее время выполнения в секундах: '||to_char(v_t_res/100,'0.00'));
  dbms_output.put_line('Общее время ЦПУ в секундах: '||to_char(v_cpu_res/100,'0.00'));

  -- откат изменения
  rollback;
end;

CREATE INDEX idx_InsertAddress ON C##Vasilisa.Addresses(Country);
