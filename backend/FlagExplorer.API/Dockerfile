# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia o arquivo de projeto e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante do código e builda
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Expõe a porta do backend
EXPOSE 5144

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "FlagExplorer.API.dll"]
