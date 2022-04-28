FROM mcr.microsoft.com/dotnet/aspnet:5.0.5-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0.202-buster-slim AS cache

WORKDIR /src
COPY "NuGet.config" "NuGet.config"
COPY *.ruleset ./DockerRestoreProjectFiles.tar ./
RUN tar -xvf DockerRestoreProjectFiles.tar > /dev/null
RUN dotnet restore "MobileApi/UltimatePlaylist.MobileApi.csproj"

FROM cache as build
WORKDIR /src
ENV SolutionDir "/src"

COPY ./MobileApi ./MobileApi
COPY ./Common ./Common
COPY ./Database ./Database
COPY ./Services ./Services

RUN dotnet build "MobileApi/UltimatePlaylist.MobileApi.csproj" --no-restore -c Release

FROM build AS publish
WORKDIR /src
RUN dotnet publish "MobileApi/UltimatePlaylist.MobileApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "UltimatePlaylist.MobileApi.dll"]
