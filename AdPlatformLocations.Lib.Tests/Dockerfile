FROM mcr.microsoft.com/dotnet/sdk:9.0 AS test
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AdPlatformLocations.Lib.Tests/AdPlatformLocations.Lib.Tests.csproj", "AdPlatformLocations.Lib.Tests/"]
COPY ["AdPlatformLocations.Lib/AdPlatformLocations.Lib.csproj", "AdPlatformLocations.Lib/"]
RUN dotnet restore "AdPlatformLocations.Lib.Tests/AdPlatformLocations.Lib.Tests.csproj"
COPY . .
WORKDIR "/src/AdPlatformLocations.Lib.Tests"

ENTRYPOINT ["dotnet", "test"]
