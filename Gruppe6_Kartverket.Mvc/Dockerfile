# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj", "Gruppe6_Kartverket.Mvc/"]
RUN dotnet restore "Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj"

# Copy the rest of the application code
COPY . .
WORKDIR "/src/Gruppe6_Kartverket.Mvc"
RUN dotnet build "Gruppe6_Kartverket.Mvc.csproj" -c Release -o /app/build

# Install the dotnet-ef tool
RUN dotnet tool install --global dotnet-ef

# Ensure the dotnet tools are in the PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Install netcat-openbsd
RUN apt-get update && apt-get install -y netcat-openbsd

# Add a wait script
# COPY Gruppe6_Kartverket.Mvc/wait-for-it.sh /usr/local/bin/wait-for-it.sh
# RUN chmod +x /usr/local/bin/wait-for-it.sh

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Gruppe6_Kartverket.Mvc.dll"]