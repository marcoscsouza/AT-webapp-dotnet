# Site da Aplicação



# Migrations

### Add
dotnet ef migrations add AddInitial --startup-project MusicoWebMVC --project Data --context MusicoContext --output-dir Migrations

### Update
dotnet ef database update --startup-project MusicoWebMVC --project Data --context MusicoContext