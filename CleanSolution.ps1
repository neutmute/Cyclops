(Get-WmiObject Win32_Service -ComputerName localhost -Filter "Name='iisadmin'").InvokeMethod("StopService", $null)

Get-ChildItem .\ -include bin,obj,Backup,_UpgradeReport_Files,buildOutput,TestResults,_ReSharper* -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse -Verbose}
