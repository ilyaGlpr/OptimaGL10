@echo off

:: Определяем текущую папку, где находится BAT-файл
set CURRENT_DIR=%~dp0

:: Определяем путь к файлу, для которого создаем ярлык (файл находится в той же папке)
set TARGET=%CURRENT_DIR%OptimaGL.exe

:: Определяем путь к рабочему столу
set DESKTOP=%USERPROFILE%\Desktop

:: Определяем имя ярлыка
set SHORTCUT=OptimaGL.lnk

:: Создаем ярлык с помощью PowerShell
powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%DESKTOP%\%SHORTCUT%'); $s.TargetPath='%TARGET%'; $s.WorkingDirectory='%CURRENT_DIR%'; $s.Save()"

echo Ярлык создан на рабочем столе.
pause
