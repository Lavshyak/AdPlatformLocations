FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AdPlatformLocations.WebApi/AdPlatformLocations.WebApi.csproj", "AdPlatformLocations.WebApi/"]
COPY ["AdPlatformLocations.Lib/AdPlatformLocations.Lib.csproj", "AdPlatformLocations.Lib/"]
RUN dotnet restore "AdPlatformLocations.WebApi/AdPlatformLocations.WebApi.csproj"
COPY . .
WORKDIR "/src/AdPlatformLocations.WebApi"
RUN dotnet build "./AdPlatformLocations.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AdPlatformLocations.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdPlatformLocations.WebApi.dll"]
