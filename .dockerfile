FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Restore dependencies first to maximize layer caching
COPY ["cse325_team7_project.csproj", "./"]
RUN dotnet restore "cse325_team7_project.csproj"

# Copy the remainder of the source and publish the app
COPY . .
RUN dotnet publish "cse325_team7_project.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# Respect the PORT value provided by Render (default to 8080 when running locally)
CMD ["bash", "-c", "dotnet cse325_team7_project.dll --urls http://0.0.0.0:${PORT:-8080}"]
