FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 4020

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OpusClassicalWeb/OpusClassicalWeb.fsproj", "OpusClassicalWeb/"]
RUN dotnet restore "OpusClassicalWeb/OpusClassicalWeb.fsproj"
COPY . .
WORKDIR "/src/OpusClassicalWeb"
RUN dotnet build "OpusClassicalWeb.fsproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OpusClassicalWeb.fsproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpusClassicalWeb.dll"]
