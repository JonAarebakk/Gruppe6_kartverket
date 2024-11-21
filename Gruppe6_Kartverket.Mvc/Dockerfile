# Use the .NET SDK image for building and migrations
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj", "Gruppe6_Kartverket.Mvc/"]
RUN dotnet restore "./Gruppe6_Kartverket.Mvc/Gruppe6_Kartverket.Mvc.csproj"

# Copy the rest of the files
COPY . .

# Build and publish the app
WORKDIR "/src/Gruppe6_Kartverket.Mvc"
RUN dotnet build "./Gruppe6_Kartverket.Mvc.csproj" -c Release -o /app/build
RUN dotnet publish "./Gruppe6_Kartverket.Mvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage: run the app using the .NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install required packages, .NET SDK, and sudo in the final container image
RUN apt-get update && \
    apt-get install -y sudo apt-utils && \
    apt-get install -y dotnet-sdk-8.0 && \
    apt-get clean 

# Install dotnet-ef globally
RUN dotnet tool install --global dotnet-ef && \
    echo 'export PATH="$PATH:/root/.dotnet/tools"' >> /root/.bashrc && \
    export PATH="$PATH:/root/.dotnet/tools"


# Set root password
RUN echo 'root:123' | chpasswd

# Set root user to be able to use sudo without password
RUN echo 'root ALL=(ALL) NOPASSWD: ALL' > /etc/sudoers.d/90-root-nopassword

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Ensure the Keys directory exists and set permissions
RUN mkdir -p /app/keys
RUN chmod -R 777 /app/keys  # Optional: Adjust based on your actual security needs

# Set environment variables
ENV AspnetCoreEnvironment=Docker

# Set the entry point to run the app
ENTRYPOINT ["bash", "-c", "export PATH=\"$PATH:/root/.dotnet/tools\" && dotnet ef database update && dotnet Gruppe6_Kartverket.Mvc.dll"]
