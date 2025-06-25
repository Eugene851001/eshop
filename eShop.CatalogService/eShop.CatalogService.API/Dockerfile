#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eShop.CatalogService.API/eShop.CatalogService.API.csproj", "eShop.CatalogService.API/"]
COPY ["eShop.CatalogService.BLL/eShop.CatalogService.BLL.csproj", "eShop.CatalogService.BLL/"]
COPY ["eShop.CatalogService.ConfluentKafka/eShop.CatalogService.ConfluentKafka.csproj", "eShop.CatalogService.ConfluentKafka/"]
COPY ["eShop.CatalogService.DAL/eShop.CatalogService.DAL.csproj", "eShop.CatalogService.DAL/"]
RUN dotnet restore "./eShop.CatalogService.API/eShop.CatalogService.API.csproj"
COPY . .
WORKDIR "/src/eShop.CatalogService.API"
RUN dotnet build "./eShop.CatalogService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./eShop.CatalogService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.CatalogService.API.dll"]