param
(
  [String]  
  $AzureServerName,

  [String]
  $AzureSubscriptionName,

  [String] 
  $AzureFirewallName = "AzureWebAppFirewall"
)

$ErrorActionPreference = 'Stop'

function New-AzureSQLServerFirewallRule {
  $agentIP = (New-Object net.webclient).downloadstring("http://checkip.dyndns.com") -replace "[^\d\.]"
  New-AzureSqlDatabaseServerFirewallRule -StartIPAddress $agentIp -EndIPAddress $agentIp -RuleName $AzureFirewallName -ServerName $AzureServerName
}
function Update-AzureSQLServerFirewallRule{
  $agentIP= (New-Object net.webclient).downloadstring("http://checkip.dyndns.com") -replace "[^\d\.]"
  Set-AzureSqlDatabaseServerFirewallRule -StartIPAddress $agentIp -EndIPAddress $agentIp -RuleName $AzureFirewallName -ServerName $AzureServerName
}

Get-AzureSubscription -SubscriptionName $AzureSubscriptionName

Select-AzureSubscription -SubscriptionName $AzureSubscriptionName

If ((Get-AzureSqlDatabaseServerFirewallRule -ServerName $AzureServerName -RuleName $AzureFirewallName -ErrorAction SilentlyContinue) -eq $null)
{
  New-AzureSQLServerFirewallRule
}
else
{
  Update-AzureSQLServerFirewallRule
}