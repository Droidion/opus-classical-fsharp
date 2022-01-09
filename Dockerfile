# Build client assets (JavaScript and CSS)
FROM node:14-alpine as build-node
WORKDIR /usr/src/app
COPY ./Site/rollup.config.js ./
COPY ./Site/package*.json ./
RUN npm install
RUN npm install --unsafe-perm -g sass
COPY ./Site/Scripts ./Scripts
COPY ./Site/Sass ./Sass
WORKDIR /usr/src/app/Site
RUN npm run sass
RUN npm run build

# Build .NET app
FROM mcr.microsoft.com/dotnet/sdk:6.0.101-alpine3.14-amd64 AS build-env
WORKDIR /DockerSource
COPY *.sln .
COPY Site/*.fsproj ./Site/
COPY Site.Tests/*.fsproj ./Site.Tests/
RUN dotnet restore
COPY Site/. ./Site
COPY Site.Tests/. ./Site.Tests
RUN dotnet publish --no-restore -c release -o /DockerOutput/Site
COPY --from=build-node /usr/src/app/static/bundle.css /DockerOutput/Site/static/bundle.css
COPY --from=build-node /usr/src/app/static/bundle.js /DockerOutput/Site/static/bundle.js

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0.1-alpine3.14-amd64
WORKDIR /DockerOutput/Site
COPY --from=build-env /DockerOutput/Site ./
ENTRYPOINT ["dotnet", "Site.dll"]