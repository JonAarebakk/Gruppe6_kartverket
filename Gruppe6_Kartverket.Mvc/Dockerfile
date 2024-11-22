# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081



# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj", "Gruppe6_Kartverket.Mvc/"]
RUN dotnet restore "./Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj"
COPY . .
WORKDIR "/src/Gruppe6_Kartverket.Mvc"
RUN dotnet build "./Gruppe6_Kartverket.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gruppe6_Kartverket.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV  AspnetCoreEnvironment=Docker


ENTRYPOINT ["dotnet", "Gruppe6_Kartverket.Mvc.dll"]






















# Use the official ASP.NET Core runtime as a parent image
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081

# Use the SDK image to build the app
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#COPY ["Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj", "Gruppe6_Kartverket.Mvc/"]
#RUN dotnet restore "./Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj"

# Copy the rest of the application code
#COPY . .
#WORKDIR "/src/Gruppe6_Kartverket.Mvc"
#RUN dotnet build "./Gruppe6_Kartverket.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Install the dotnet-ef tool
#RUN dotnet tool install --global dotnet-ef

# Ensure the dotnet tools are in the PATH
#ENV PATH="$PATH:/root/.dotnet/tools"
#RUN dotnet ef database update --project "./Gruppe6_Kartverket.Mvc.csproj"

# Install netcat-openbsd
#RUN apt-get update && apt-get install -y netcat-openbsd

#FROM build AS publish
#RUN dotnet publish "./Gruppe6_Kartverket.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# Use the runtime image to run the app
#FROM base AS final
#WORKDIR /app
#COPY --from=build /app/publish .
#COPY Gruppe6_Kartverket.Mvc/wait-for-mariadb.sh /app/wait-for-mariadb.sh
#RUN chmod +x /app/wait-for-mariadb.sh

# Run the wait script and then the application
#ENTRYPOINT ["/app/wait-for-mariadb.sh", "dotnet", "ef", "database", "update", "--project", "/src/Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj", "&&", "dotnet", "Gruppe6_Kartverket.Mvc.dll"]



# Use the runtime image to run the app
#FROM base AS final
#WORKDIR /app
#COPY --from=build /app/publish .

# Run Entity Framework migrations before starting the application
#ENTRYPOINT ["dotnet", "Gruppe6_Kartverket.Mvc.dll"]
#ENTRYPOINT ["bash", "-c", "dotnet ef database update && Gruppe6_Kartverket.Mvc.dll"]