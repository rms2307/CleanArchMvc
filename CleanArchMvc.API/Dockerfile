#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CleanArchMvc.API/CleanArchMvc.API.csproj", "CleanArchMvc.API/"]
COPY ["CleanArchMvc.Infra.IoC/CleanArchMvc.Infra.IoC.csproj", "CleanArchMvc.Infra.IoC/"]
COPY ["CleanArchMvc.Application/CleanArchMvc.Application.csproj", "CleanArchMvc.Application/"]
COPY ["CleanArchMvc.Domain/CleanArchMvc.Domain.csproj", "CleanArchMvc.Domain/"]
COPY ["CleanArchMvc.Infra.Data/CleanArchMvc.Infrastructure.csproj", "CleanArchMvc.Infra.Data/"]
RUN dotnet restore "./CleanArchMvc.API/./CleanArchMvc.API.csproj"
COPY . .
WORKDIR "/src/CleanArchMvc.API"
RUN dotnet build "./CleanArchMvc.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CleanArchMvc.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchMvc.API.dll"]