del /q ..\..\Launch\Server\Tables\*.csv
del /q ..\..\Launch\Client\Content\Tables\*.csv
del /q ..\Tables_Build\*.xlsx
:: �������ݱ�
:: �������ݱ��ͻ���
copy *.xlsx ..\Tables_Build\
..\..\EntryBuilder.exe BuildCSVFromExcel ..\Tables_Build\ ..\..\Launch\Client\Content\Tables\ ..\LANGUAGE.csv 12.0 ..\..\Code\Protocol\Protocol\_TABLE.design.cs true
:: �������ݱ������
copy ..\..\Launch\Client\Content\Tables\*.csv ..\..\Launch\Server\Tables\