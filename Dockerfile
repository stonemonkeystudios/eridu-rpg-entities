#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV DOTNET_ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY eridu-rpg-entities-host/. eridu-rpg-entities-host/
COPY eridu-rpg-entities-model/. eridu-rpg-entities-model/
COPY eridu-rpg-entities.sln eridu-rpg-entities.sln
RUN dotnet restore

RUN dotnet build  -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS deploy
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eridu-rpg-entities-host.dll"]
