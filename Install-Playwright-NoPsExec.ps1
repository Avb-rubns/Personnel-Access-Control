param(
    [string]$AppPoolName = "PersonnelPool"
)

Import-Module WebAdministration
$pool = Get-Item "IIS:\AppPools\$AppPoolName"

switch ($pool.processModel.identityType) {
    0 { $userName = "LocalSystem" }
    1 { $userName = "LocalService" }
    2 { $userName = "NetworkService" }
    3 {
        # ApplicationPoolIdentity -> usará systemprofile
        $userName = "NT AUTHORITY\SYSTEM"
        $isAppPoolIdentity = $true
    }
    default {
        $userName = $pool.processModel.userName
        $isAppPoolIdentity = $false
    }
}

Write-Host "El AppPool '$AppPoolName' corre bajo el usuario: $userName"

$binPath = "D:\Cluster-0\Documentos\proyects\Personnel-Access-Control\Personnel.Client\Server\bin\Debug\net9.0"

if ($isAppPoolIdentity) {
    Write-Host "⚡ Instalando navegadores en el perfil de systemprofile (ApplicationPoolIdentity)..."
    # Ejecuta como SYSTEM usando schtasks
    schtasks /Create /TN "PlaywrightInstall" /SC ONCE /TR "pwsh -File `"$binPath\playwright.ps1`" install --with-deps" /ST 00:00 /RU "SYSTEM"
    schtasks /Run /TN "PlaywrightInstall"
    Start-Sleep -Seconds 10
    schtasks /Delete /TN "PlaywrightInstall" /F
}
else {
    Write-Host "⚡ Ejecuta manualmente este comando para instalar como ${userName}:"
    Write-Host 'runas /user:'"$userName"' pwsh -File "'$binPath'\playwright.ps1" install --with-deps'
}


