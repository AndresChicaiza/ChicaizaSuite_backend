# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY ["ChicaizaSuite.Api.csproj", "./"]
RUN dotnet restore "ChicaizaSuite.Api.csproj"

# Copiar todo el código y compilar
COPY . .
WORKDIR "/src/."
RUN dotnet build "ChicaizaSuite.Api.csproj" -c Release -o /app/build

# Etapa 2: Publicación
FROM build AS publish
RUN dotnet publish "ChicaizaSuite.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Imagen final para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Exponer el puerto que Render y otros servicios de contenedores usan
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ChicaizaSuite.Api.dll"]
