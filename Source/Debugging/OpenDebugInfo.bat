rem #Approve File 08/03/2022 11:35am.
cmd /C reg add "HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Applets\Regedit" /v "LastKey" /f /reg:64 /t REG_SZ /d "Computer\HKEY_CURRENT_USER\SOFTWARE\MysteryDebugInfo" && start "Registry Editor" "C:\Windows\Regedit.exe"