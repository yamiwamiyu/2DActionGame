:: ���������ļ�����Ŀ
call _CopyToCode.bat
:: ���±�����Ŀ
..\EntryBuilder.exe BuildDll ..\Code\Client\Client\ ..\Launch\Client\Client.dll 3.5 "..\Code\EntryEngine.dll" false
:: �������������ɵķ��������Ŀ¼
copy ..\Design\LANGUAGE.csv ..\Launch\Client\Content\Tables\LANGUAGE.csv
:: ������շ����
..\EntryBuilder.exe BuildOutputCSV ..\Launch\Client\Content\Tables\LANGUAGE.csv ""
:: �л�������Ŀ¼����Ӧ�ÿͻ��˳���
cd ..\Launch\Client
call Xna.exe