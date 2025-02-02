#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MessageService.Api/MessageService.Api.csproj", "MessageService.Api/"]
COPY ["MessageService.Contracts/MessageService.Contracts.csproj", "MessageService.Contracts/"]
COPY ["MessageService.Infrastructure/MessageService.Infrastructure.csproj", "MessageService.Infrastructure/"]
COPY ["MessageService.Application/MessageService.Application.csproj", "MessageService.Application/"]
COPY ["MessageService.Domain/MessageService.Domain.csproj", "MessageService.Domain/"]
RUN dotnet restore "./MessageService.Api/./MessageService.Api.csproj"
COPY . .
WORKDIR "/src/MessageService.Api"
RUN dotnet build "./MessageService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MessageService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageService.Api.dll"]