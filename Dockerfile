# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["KiranaStore/KiranaStore.csproj", "KiranaStore/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DAL/DAL.csproj", "DAL/"]

# Restore dependencies
RUN dotnet restore "KiranaStore/KiranaStore.csproj"

# Copy all source code
COPY . .

# Build
RUN dotnet build "KiranaStore/KiranaStore.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "KiranaStore/KiranaStore.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install PostgreSQL client tools (optional, for migrations)
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "KiranaStore.dll"]
