param ([string]$migration_name)

dotnet ef migrations add ${migration_name} -s ..\TodoApp\TodoApp\TodoApp.csproj -p .\TodoApp.Data.Auth.csproj -c TodoApp.Data.Auth.AuthDbContext
