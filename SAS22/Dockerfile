#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SAS22.csproj", "."]
RUN dotnet restore "./SAS22.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SAS22.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SAS22.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SAS22.dll"]