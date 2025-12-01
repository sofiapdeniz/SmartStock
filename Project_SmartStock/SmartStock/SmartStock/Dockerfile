FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Project_SmartStock/SmartStock/SmartStock/SmartStock.csproj SmartStock/
RUN dotnet restore "SmartStock/SmartStock.csproj"

COPY Project_SmartStock/SmartStock/SmartStock/ SmartStock/

WORKDIR /src/SmartStock
RUN dotnet build "SmartStock.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartStock.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartStock.dll"]
