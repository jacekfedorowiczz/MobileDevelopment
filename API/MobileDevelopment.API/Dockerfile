FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files first to leverage Docker cache
COPY ["MobileDevelopment.API/MobileDevelopment.API.csproj", "MobileDevelopment.API/"]
COPY ["MobileDevelopment.API.Domain/MobileDevelopment.API.Domain.csproj", "MobileDevelopment.API.Domain/"]
COPY ["MobileDevelopment.API.Models/MobileDevelopment.API.Models.csproj", "MobileDevelopment.API.Models/"]
COPY ["MobileDevelopment.API.Persistence/MobileDevelopment.API.Persistence.csproj", "MobileDevelopment.API.Persistence/"]
COPY ["MobileDevelopment.API.Services/MobileDevelopment.API.Services.csproj", "MobileDevelopment.API.Services/"]

# Restore dependencies
RUN dotnet restore "MobileDevelopment.API/MobileDevelopment.API.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/MobileDevelopment.API"

# Build the project
RUN dotnet build "MobileDevelopment.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MobileDevelopment.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MobileDevelopment.API.dll"]