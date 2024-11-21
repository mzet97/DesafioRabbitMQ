# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Desafio.ProtocoloAPI/Desafio.ProtocoloAPI.API/Desafio.ProtocoloAPI.API.csproj", "Desafio.ProtocoloAPI.API/"]
COPY ["Desafio.ProtocoloAPI/Desafio.ProtocoloAPI.Application/Desafio.ProtocoloAPI.Application.csproj", "Desafio.ProtocoloAPI.Application/"]
COPY ["Desafio.ProtocoloAPI/Desafio.ProtocoloAPI.Core/Desafio.ProtocoloAPI.Core.csproj", "Desafio.ProtocoloAPI.Core/"]
COPY ["Desafio.ProtocoloAPI/Desafio.ProtocoloAPI.Infrastructure/Desafio.ProtocoloAPI.Infrastructure.csproj", "Desafio.ProtocoloAPI.Infrastructure/"]
RUN dotnet restore "./Desafio.ProtocoloAPI.API/Desafio.ProtocoloAPI.API.csproj"
COPY . .
WORKDIR "/src/Desafio.ProtocoloAPI/Desafio.ProtocoloAPI.API"
RUN dotnet build "./Desafio.ProtocoloAPI.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Desafio.ProtocoloAPI.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Definindo as variáveis de ambiente
ENV ConnectionStrings__DefaultConnection="Host=postgres;Port=5432;Pooling=true;Database=dbProtocolos;User Id=admin;Password=admin;Pooling=true;Minimum Pool Size=5;Maximum Pool Size=100;Timeout=15;Connection Idle Lifetime=300;"
ENV AppSettings__Secret="MXFhejJ3c3gzZWRjZHdkd3dxZnFlZ3JoanlrdWlsbw=="
ENV AppSettings__ExpirationHours=24
ENV AppSettings__Issuer="Desafio"
ENV AppSettings__ValidOn="https://localhost:8080"
ENV RabbitMQ__HostName="rabbitmq"
ENV RabbitMQ__Port=5672
ENV RabbitMQ__UserName="admin"
ENV RabbitMQ__Password="admin"

ENTRYPOINT ["dotnet", "Desafio.ProtocoloAPI.API.dll"]
