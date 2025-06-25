#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8083
EXPOSE 8084

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eShop.CartService.API/eShop.CartService.API.csproj", "eShop.CartService.API/"]
COPY ["eShop.CartService/eShop.CartService.csproj", "eShop.CartService/"]
RUN dotnet restore "./eShop.CartService.API/eShop.CartService.API.csproj"
COPY . .
WORKDIR "/src/eShop.CartService.API"
RUN dotnet build "./eShop.CartService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./eShop.CartService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.CartService.API.dll"]