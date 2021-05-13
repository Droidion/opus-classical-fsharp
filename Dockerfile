FROM mcr.microsoft.com/dotnet/sdk:5.0.203-alpine3.13-amd64 AS build-env
WORKDIR /DockerSource

# Copy csproj/fsproj and restore as distinct layers
COPY *.sln .
COPY Site/*.fsproj ./Site/
COPY Site.Tests/*.fsproj ./Site.Tests/
RUN dotnet restore

# Copy everything else and build
COPY Site/. ./Site
COPY Site.Tests/. ./Site.Tests
RUN dotnet publish -c release -o /DockerOutput/Site

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0.6-alpine3.13-amd64
WORKDIR /DockerOutput/Site
COPY --from=build-env /DockerOutput/Site ./
ENTRYPOINT ["dotnet", "Site.dll"]