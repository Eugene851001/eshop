#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eShop.CartService.Console/eShop.CartService.Console.csproj", "eShop.CartService.Console/"]
COPY ["eShop.CartService.ConfluentKafka/eShop.CartService.ConfluentKafka.csproj", "eShop.CartService.ConfluentKafka/"]
COPY ["eShop.CartService/eShop.CartService.csproj", "eShop.CartService/"]
RUN dotnet restore "./eShop.CartService.Console/eShop.CartService.Console.csproj"
COPY . .
WORKDIR "/src/eShop.CartService.Console"
RUN dotnet build "./eShop.CartService.Console.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./eShop.CartService.Console.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.CartService.Console.dll"]