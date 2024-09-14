# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine AS base
USER app
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Desafio.ProtocoloPublisher/Desafio.ProtocoloPublisher.csproj", "Desafio.ProtocoloPublisher/"]
COPY ["Desafio.ProtocoloPublisher.Core/Desafio.ProtocoloPublisher.Core.csproj", "Desafio.ProtocoloPublisher.Core/"]
COPY ["Desafio.ProtocoloPublisher.Infrastructure/Desafio.ProtocoloPublisher.Infrastructure.csproj", "Desafio.ProtocoloPublisher.Infrastructure/"]
RUN dotnet restore "./Desafio.ProtocoloPublisher/Desafio.ProtocoloPublisher.csproj"
COPY . .
WORKDIR "/src/Desafio.ProtocoloPublisher"
RUN dotnet build "./Desafio.ProtocoloPublisher.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Desafio.ProtocoloPublisher.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for the application
ENV RABBITMQ_HOSTNAME=rabbitmq
ENV RABBITMQ_PORT=5672
ENV RABBITMQ_USERNAME=admin
ENV RABBITMQ_PASSWORD=admin
ENV RABBITMQ_QUEUE_NAME=protocolos

ENTRYPOINT ["dotnet", "Desafio.ProtocoloPublisher.dll"]