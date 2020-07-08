@echo off

::create nuget_pub
if not exist nuget_pub (
    md nuget_pub
)

::clear nuget_pub
for /R "nuget_pub" %%s in (*) do (
    del %%s
)

::Utils
dotnet pack src/Bing.Utils -c Release -o nuget_pub
dotnet pack src/Bing.Utils.Drawing -c Release -o nuget_pub

::Core
dotnet pack src/Bing -c Release -o nuget_pub
dotnet pack src/Bing.AspNetCore -c Release -o nuget_pub

::Permissions
dotnet pack src/Bing.Security -c Release -o nuget_pub
dotnet pack src/Bing.Permissions -c Release -o nuget_pub

::Logs
dotnet pack src/Bing.Logs -c Release -o nuget_pub
dotnet pack src/Bing.Logs.Exceptionless -c Release -o nuget_pub
dotnet pack src/Bing.Logs.Log4Net -c Release -o nuget_pub
dotnet pack src/Bing.Logs.NLog -c Release -o nuget_pub
dotnet pack src/Bing.Logs.Serilog -c Release -o nuget_pub

::Events
dotnet pack src/Bing.Events -c Release -o nuget_pub

::Datas
dotnet pack src/Bing.Datas.Dapper -c Release -o nuget_pub
dotnet pack src/Bing.Datas.EntityFramework -c Release -o nuget_pub
dotnet pack src/Bing.Datas.EntityFramework.MySql -c Release -o nuget_pub
dotnet pack src/Bing.Datas.EntityFramework.PgSql -c Release -o nuget_pub
dotnet pack src/Bing.Datas.EntityFramework.SqlServer -c Release -o nuget_pub
dotnet pack src/Bing.Datas.EntityFramework.Oracle -c Release -o nuget_pub

::Applications
dotnet pack src/Bing.Applications -c Release -o nuget_pub

::Webs
dotnet pack src/Bing.Webs -c Release -o nuget_pub

::Caching
dotnet pack src/Bing.EasyCaching -c Release -o nuget_pub
dotnet pack src/Bing.Caching.CSRedis -c Release -o nuget_pub

::Biz
dotnet pack src/Bing.Biz -c Release -o nuget_pub

::Tools
dotnet pack src/Bing.MailKit -c Release -o nuget_pub
dotnet pack src/Bing.AutoMapper -c Release -o nuget_pub

for /R "nuget_pub" %%s in (*symbols.nupkg) do (
    del %%s
)

echo.
echo.

set /p key=input key:
set source=https://api.nuget.org/v3/index.json

for /R "nuget_pub" %%s in (*.nupkg) do (
    call dotnet nuget push %%s -k %key% -s %source%
    echo.
)

pause