FROM mcr.microsoft.com/dotnet/sdk:5.0.202-alpine3.13-amd64 AS build-env
WORKDIR /DockerSource

# Copy csproj/fsproj and restore as distinct layers
COPY *.sln .
COPY SiteSaturn/*.fsproj ./SiteSaturn/
COPY SiteSaturn.Tests/*.fsproj ./SiteSaturn.Tests/
RUN dotnet restore

# Copy everything else and build
COPY SiteSaturn/. ./SiteSaturn
COPY SiteSaturn.Tests/. ./SiteSaturn.Tests
RUN dotnet publish -c release -o /DockerOutput/SiteSaturn

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0.5-alpine3.13-amd64
WORKDIR /DockerOutput/SiteSaturn
COPY --from=build-env /DockerOutput/SiteSaturn ./
ENTRYPOINT ["./SiteSaturn", "SiteSaturn.dll"]