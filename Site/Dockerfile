FROM mcr.microsoft.com/dotnet/aspnet:5.0.5-alpine3.13-amd64 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0.202-alpine3.13-amd64 AS build
WORKDIR /src
COPY ["Site/Site.fsproj", "Site/"]
RUN dotnet restore "Site/Site.fsproj"
COPY . .
WORKDIR "/src/Site"
RUN dotnet build "Site.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Site.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Site.dll"]
