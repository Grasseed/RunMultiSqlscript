@echo off
rem @echo off
rem 資料庫IP\資料庫執行個體名稱
set dbIp=*
rem 資料庫名稱
set dbName=*
rem 登入帳號
set dbUsrAcc=*
rem 使用者密碼
set dbUsrPwd=*
rem 整理完畢的SQL指令集資料夾位置
set batchFilePath=""
rem 清單.SQL檔放置路徑
set dbSqlFilePath=""

rem log檔名
set x=%date:~0,4%%date:~5,2%%date:~8,2%
if %time:~0,2% leq 9 (set hour=0%time:~1,1%) else (set hour=%time:~0,2%) 
set min=%time:~3,2%%time:~6,2%
set y=%hour%%min%
set FileLog=view_%x%_%y%.log

rem 程式開始執行
cd %batchFilePath%
echo %dbSqlFilePath%
sqlcmd -S %dbIp% -d %dbName% -U %dbUsrAcc% -P %dbUsrPwd% -i %dbSqlFilePath% > .\log\%FileLog%

@echo on
rem @echo on
