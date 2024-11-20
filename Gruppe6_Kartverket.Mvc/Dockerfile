# Base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj", "Gruppe6_Kartverket.Mvc/"]
RUN dotnet restore "./Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj"
COPY . .
WORKDIR "/src/Gruppe6_Kartverket.Mvc"
RUN dotnet build "./Gruppe6_Kartverket.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage to publish the project for deployment
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gruppe6_Kartverket.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image for running the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure the Keys directory exists
RUN mkdir -p /app/keys
RUN chmod -R 777 /app/keys

# Set environment variables
ENV AspnetCoreEnvironment=Docker

ENTRYPOINT ["dotnet", "Gruppe6_Kartverket.Mvc.dll"]

