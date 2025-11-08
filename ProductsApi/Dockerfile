# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 1. Copiar solo los csproj necesarios
COPY *.csproj ./
COPY Web/*.csproj ./Web/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/

# 2. Restore usando cache
RUN dotnet restore

# 3. Copiar solo las carpetas de código necesarias
COPY Web/ ./Web/
COPY Application/ ./Application/
COPY Domain/ ./Domain/
COPY Infrastructure/ ./Infrastructure/
COPY wwwroot/ ./wwwroot/
COPY Migrations/ ./Migrations/

# 4. Publish
RUN dotnet publish ProductsApi.csproj -c Release -o /app/out

# Etapa runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "ProductsApi.dll"]
