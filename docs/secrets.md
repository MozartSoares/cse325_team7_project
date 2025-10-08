# 🔐 Setting up User Secrets for Local Development

Follow these steps to securely configure local environment secrets using **.NET User Secrets**.  
This keeps sensitive credentials (like MongoDB connection strings and JWT keys) **out of the repository**.

---

## 1️⃣ Ensure the project has a `UserSecretsId`

Open your `.csproj` file and make sure it contains a unique `UserSecretsId`:

```xml
<PropertyGroup>
  <TargetFramework>net9.0</TargetFramework>
  <UserSecretsId>cse325-team7-project</UserSecretsId>
</PropertyGroup>
```

If it’s already there, you can skip the init step later — only the first person to create the ID needs to run it.

---

## 2️⃣ Initialize User Secrets (only once per project)

Run this command from the project root (where your `.csproj` is located):

```bash
dotnet user-secrets init
```

This creates a secure local storage location for secrets on your machine.

---

## 3️⃣ Add your local secrets

Fill in your own credentials and run:

```bash
dotnet user-secrets set "Mongo:ConnectionString" "<your_mongodb_connection_string>"
dotnet user-secrets set "Mongo:Database" "<your_database_name>"

dotnet user-secrets set "Jwt:Issuer" "<your_issuer_url>"
dotnet user-secrets set "Jwt:Audience" "<your_audience_url>"
dotnet user-secrets set "Jwt:Key" "<your_32+_char_random_secret>"
```

**Notes**

- Use a strong, random string for `Jwt:Key` (minimum 32 characters).
- These values override `appsettings.json` when running locally.

---

## 4️⃣ Verify your secrets

```bash
dotnet user-secrets list
```

Expected output:

```text
Mongo:ConnectionString = mongodb+srv://...
Mongo:Database = MovieHubDb
Jwt:Issuer = https://localhost:5128
Jwt:Audience = https://localhost:5128
Jwt:Key = <your_32+_char_random_secret>
```

---

## 5️⃣ Run the app in Development mode

```bash
export ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

---

## 6️⃣ Production configuration

Use environment variables instead of User Secrets:

```bash
export Mongo__ConnectionString="<prod_connection_string>"
export Mongo__Database="<prod_db_name>"
export Jwt__Issuer="https://api.yourdomain.com"
export Jwt__Audience="https://app.yourdomain.com"
export Jwt__Key="<prod_32+_char_random_secret>"
```

These automatically map to your configuration keys in `Program.cs`.

---

## ✅ Summary

1. Run `dotnet user-secrets init` once (if not already).
2. Add secrets with `dotnet user-secrets set`.
3. Confirm with `dotnet user-secrets list`.
4. Run the app in **Development** — secrets load automatically.
