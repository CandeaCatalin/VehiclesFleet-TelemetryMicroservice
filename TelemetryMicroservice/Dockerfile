FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TelemetryMicroservice/TelemetryMicroservice.csproj", "TelemetryMicroservice/"]
RUN dotnet restore "TelemetryMicroservice/TelemetryMicroservice.csproj"
COPY . .
WORKDIR "/src/TelemetryMicroservice"
RUN dotnet build "TelemetryMicroservice.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TelemetryMicroservice.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelemetryMicroservice.dll"]
