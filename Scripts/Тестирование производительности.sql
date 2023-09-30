-- �������� ����� ������ � output
set serveroutput on;

declare
  v_t1 number; -- ����� ������
  v_t2 number; -- ����� ����������
  v_cpu1 number; -- ����� ��� �� 
  v_cpu2 number; -- ����� ��� ����� 
  v_t_res number; -- ����� ����� ����������
  v_cpu_res number; -- ����� ����� ���������� CPU

begin
  
  -- ��������� ������
  -- �������� ����� ����� � ����� ��� � ����������
  select t.hsecs
        ,dbms_utility.get_cpu_time
    into v_t1 
        ,v_cpu1
    from v$timer t;
    
  -- �������� 100000 ��� ����, ����������� ��������� ������� � ������� Addresses
  begin
  for i IN 1..100000
    loop
      C##Vasilisa.InsertAddress('TEST', 'TEST', 'TEST', 'TEST', 123, 'A');
    end loop;
  end;
  
  -- ��������� ����������
  -- ����� �������� ����� ����� � ����� ��� � ����������
  select t.hsecs
        ,dbms_utility.get_cpu_time
    into v_t2 
        ,v_cpu2
    from v$timer t;
    
  -- ������� ����� ����� ���������� � ����� ����� �������
  v_t_res := v_t2 - v_t1;
  
  -- ������� ����� ����� ��� � ����� ����� �������
  v_cpu_res := v_cpu2 - v_cpu1;
 
  -- ����� ���������� �� �����
  dbms_output.put_line('����� ����� ���������� � ��������: '||to_char(v_t_res/100,'0.00'));
  dbms_output.put_line('����� ����� ��� � ��������: '||to_char(v_cpu_res/100,'0.00'));

  -- ����� ���������
  rollback;
end;

CREATE INDEX idx_InsertAddress ON C##Vasilisa.Addresses(Country);
