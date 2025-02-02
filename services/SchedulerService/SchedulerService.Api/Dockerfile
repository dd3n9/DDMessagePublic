#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SchedulerService.Api/SchedulerService.Api.csproj", "SchedulerService.Api/"]
COPY ["SchedulerService.Infrastructure/SchedulerService.Infrastructure.csproj", "SchedulerService.Infrastructure/"]
COPY ["SchedulerService.Application/SchedulerService.Application.csproj", "SchedulerService.Application/"]
COPY ["SchedulerService.Contracts/SchedulerService.Contracts.csproj", "SchedulerService.Contracts/"]
COPY ["SchedulerService.Domain/SchedulerService.Domain.csproj", "SchedulerService.Domain/"]
RUN dotnet restore "./SchedulerService.Api/./SchedulerService.Api.csproj"
COPY . .
WORKDIR "/src/SchedulerService.Api"
RUN dotnet build "./SchedulerService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SchedulerService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchedulerService.Api.dll"]