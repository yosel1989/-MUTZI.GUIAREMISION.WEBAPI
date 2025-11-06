# Imagen base de .NET 8 para ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5011

# Instalar herramientas de red en la imagen base
RUN apt-get update && apt-get install -y \
    curl \
    net-tools \
    iproute2 \
    && rm -rf /var/lib/apt/lists/*

# Imagen SDK de .NET 8 para construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Argumento para el token del agente desde el pipeline
ARG BUILD_TOKEN

# Establecer variables de entorno para autenticar con Azure Artifacts
ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED=true
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/carolinape/_packaging/carolinape/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${BUILD_TOKEN}\"}]}"

# Instalar el Azure Artifacts Credential Provider
RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | sh

# Copiar nuget.config desde la raíz del contexto
COPY ["nuget.config", "./"]

# Copiar todos los archivos .csproj de la solución (ajusta las rutas según tu estructura)
COPY ["MUTZI.GUIA.WEBAPI/MUTZI.GUIA.WEBAPI.csproj", "MUTZI.GUIA.WEBAPI/"]
COPY ["MUTZI.GUIA.APPLICATION/MUTZI.GUIA.APPLICATION.csproj", "MUTZI.GUIA.APPLICATION/"]
COPY ["MUTZI.GUIA.DOMAIN/MUTZI.GUIA.DOMAIN.csproj", "MUTZI.GUIA.DOMAIN/"]
COPY ["MUTZI.GUIA.INFRASTRUCTURE/MUTZI.GUIA.INFRASTRUCTURE.csproj", "MUTZI.GUIA.INFRASTRUCTURE/"]

# Restaurar dependencias
RUN dotnet restore "MUTZI.GUIA.WEBAPI/MUTZI.GUIA.WEBAPI.csproj" --verbosity detailed

# Copiar el resto de los archivos del proyecto API
COPY ["MUTZI.GUIA.WEBAPI/", "MUTZI.GUIA.WEBAPI/"]
COPY ["MUTZI.GUIA.APPLICATION/", "MUTZI.GUIA.APPLICATION/"]
COPY ["MUTZI.GUIA.DOMAIN/", "MUTZI.GUIA.DOMAIN/"]
COPY ["MUTZI.GUIA.INFRASTRUCTURE/", "MUTZI.GUIA.INFRASTRUCTURE/"]

# Construir el proyecto
RUN dotnet build "MUTZI.GUIA.WEBAPI/MUTZI.GUIA.WEBAPI.csproj" -c Release -o /app/build

# Publicación de la app
FROM build AS publish
RUN dotnet publish "MUTZI.GUIA.WEBAPI/MUTZI.GUIA.WEBAPI.csproj" -c Release -o /app/publish

# Imagen final de ejecución
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "MUTZI.GUIA.WEBAPI.dll"]