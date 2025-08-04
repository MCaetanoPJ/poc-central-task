# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["src/CentralTask.Api/CentralTask.Api.csproj", "src/CentralTask.Api/"]
COPY ["src/CentralTask.Application/CentralTask.Application.csproj", "src/CentralTask.Application/"]
COPY ["src/CentralTask.Broker/CentralTask.Infra.Broker.csproj", "src/CentralTask.Broker/"]
COPY ["src/CentralTask.Core/CentralTask.Core.csproj", "src/CentralTask.Core/"]
COPY ["src/CentralTask.Domain/CentralTask.Domain.csproj", "src/CentralTask.Domain/"]
COPY ["src/CentralTask.DI/CentralTask.DI.csproj", "src/CentralTask.DI/"]
COPY ["src/CentralTask.Infra.Data/CentralTask.Infra.Data.csproj", "src/CentralTask.Infra.Data/"]
RUN dotnet restore "./src/CentralTask.Api/CentralTask.Api.csproj"
COPY . .
WORKDIR "/src/src/CentralTask.Api"
RUN dotnet build "./CentralTask.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./CentralTask.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CentralTask.Api.dll"]