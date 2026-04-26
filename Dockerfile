# syntax=docker/dockerfile:1.7

# -----------------------------------------------------------------------------
# Build stage
# -----------------------------------------------------------------------------
# Use the full .NET 10 SDK to restore + publish the API.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the solution and csproj files first, restore — this layer is cached
# unless those files change, which makes incremental rebuilds fast.
COPY BabyMonitor.sln global.json ./
COPY BabyMonitor.API/BabyMonitor.API.csproj BabyMonitor.API/
COPY BabyMonitor.Core/BabyMonitor.Core.csproj BabyMonitor.Core/
COPY BabyMonitor.Infrastructure/BabyMonitor.Infrastructure.csproj BabyMonitor.Infrastructure/
RUN dotnet restore BabyMonitor.sln

# Copy the rest of the source and publish in Release mode.
COPY . .
RUN dotnet publish BabyMonitor.API/BabyMonitor.API.csproj \
        --configuration Release \
        --no-restore \
        --output /app/publish

# -----------------------------------------------------------------------------
# Runtime stage
# -----------------------------------------------------------------------------
# Slim ASP.NET runtime image. Microsoft's ASP.NET images ship a non-root
# 'app' user out of the box; APP_UID is exposed as an env var.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Kestrel listens on 8080 inside the container; docker-compose maps it to the host.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Run as the unprivileged 'app' user.
USER $APP_UID

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "BabyMonitor.API.dll"]
