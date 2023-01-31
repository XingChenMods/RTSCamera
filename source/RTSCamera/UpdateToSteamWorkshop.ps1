[xml]$gamePathProps = Get-Content "..\library\mission-library\source\BasicSharedLibrary\GamePath.props"
$gamePath = $gamePathProps.Project.PropertyGroup.GamePath
Start-Process -FilePath "${gamePath}bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.SteamWorkshop.exe" -ArgumentList "SteamWorkshopUpdate.xml"