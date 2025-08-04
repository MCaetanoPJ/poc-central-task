FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo csproj para pasta correta
COPY ["src/CentralTask.Api/CentralTask.Api.csproj", "src/CentralTask.Api/"]

# Restaura as dependências (pode levar algum tempo)
RUN dotnet restore "src/CentralTask.Api/CentralTask.Api.csproj"

# Copia todo o código para o container
COPY . .

WORKDIR "/src/src/CentralTask.Api"
RUN dotnet build "CentralTask.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CentralTask.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

COPY wait-for-it.sh .
RUN chmod +x wait-for-it.sh

ENTRYPOINT ["./wait-for-it.sh", "db:5432", "--", "./wait-for-it.sh", "rabbitmq:5672", "--", "dotnet", "CentralTask.Api.dll"]
