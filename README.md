# SmartStock - Setup Docker

Este projeto **SmartStock** utiliza Docker para facilitar o desenvolvimento e a execução da API .NET 8 junto com o SQL Server 2022. Este README explica como configurar e rodar o projeto usando Docker.

---

## 📦 Estrutura do Docker

### Dockerfile (API)

O Dockerfile usa **multi-stage build** para otimizar a imagem:

1. **Base**: `mcr.microsoft.com/dotnet/aspnet:8.0` – runtime da aplicação.
2. **Build**: `mcr.microsoft.com/dotnet/sdk:8.0` – SDK para compilar a aplicação.
3. **Publish**: gera os arquivos finais da aplicação.
4. **Final**: copia os arquivos publicados para a imagem final e define o ENTRYPOINT.

**Exemplo resumido:**

```dockerfile
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
```

---

### docker-compose.yml

O `docker-compose.yml` orquestra dois containers:

* **API** (`smartstock_api`) na porta **5000**
* **SQL Server** (`smartstock_db`) na porta **1433** com dados persistidos em um volume.

```yaml
services:
  api:
    build:
      context: .
      dockerfile: Project_SmartStock/SmartStock/SmartStock/Dockerfile
    container_name: smartstock_api
    ports:
      - "5000:8080"
    depends_on:
      - sqlserver
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__DefaultConnection: "Server=sqlserver;Database=SmartStockDb;User=sa;Password=Admin@2025;TrustServerCertificate=True"
    networks:
      - appnet

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: smartstock_db
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "Admin@2025"
    ports:
      - "1433:1433"
    volumes:
      - sqlserverdata:/var/opt/mssql
    networks:
      - appnet

volumes:
  sqlserverdata:

networks:
  appnet:
```

---

## 🚀 Como rodar o projeto

1. **Subir os containers em background**:

```bash
docker compose up -d --build
```

2. **Verificar containers ativos**:

```bash
docker ps
```

3. **Acessar a API via Swagger**:
   Abra o navegador em `http://localhost:5000/swagger/index.html`.

4. **Executar comandos SQL (opcional)**:

```bash
docker exec -it smartstock_db bash
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Admin@2025"
```

---

## ⚡ Observações

* A primeira execução pode exigir **migrations** para criar o banco `SmartStockDb`.
* A API e o SQL Server podem rodar em qualquer máquina com Docker.
* Qualquer alteração na **connection string** ou **senha do SA** deve ser feita no `docker-compose.yml`.
* Porta da API no host: `5000` → porta da API no container: `8080`

---

Isso garante que qualquer pessoa possa rodar o projeto sem instalar o .NET ou SQL Server localmente, apenas usando Docker.
