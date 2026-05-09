FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GameTracker/GameTracker.csproj GameTracker/
RUN dotnet restore GameTracker/GameTracker.csproj

COPY . .
RUN dotnet publish GameTracker/GameTracker.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "GameTracker.dll"]
