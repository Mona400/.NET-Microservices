FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore
EXPOSE 8080

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CommandsService.dll"]
