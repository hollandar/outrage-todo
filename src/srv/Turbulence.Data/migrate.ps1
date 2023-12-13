param ([string]$migration_name)

dotnet ef migrations add ${migration_name} -s ..\Turbulence\Turbulence.csproj -p .\Turbulence.Data.csproj -c Turbulence.Data.TurbulenceDbContext
