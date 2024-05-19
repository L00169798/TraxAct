# Use the .NET SDK base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Ensure root user is used for subsequent commands
USER root

# Install MAUI Workloads (if required)
RUN dotnet workload install maui

# Copy the project files into the container
COPY TraxAct.sln ./
COPY src/ ./src/
COPY TraxActUnitTests/ ./TraxActUnitTests/

# Change permissions to ensure the current user has access
RUN chown -R root:root /app
RUN chmod -R 755 /app

# Restore, build, and publish commands
RUN dotnet restore
RUN dotnet build --no-restore -c Release
RUN dotnet publish --no-restore -c Release -o out

# Ensure the output directory has the correct permissions
RUN chmod -R 755 /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Ensure root user is used for subsequent commands
USER root

# Copy the published files from the build stage
COPY --from=build /app/out .

# Ensure the files have the correct permissions
RUN chmod -R 755 /app

EXPOSE 80
ENTRYPOINT ["dotnet", "TraxAct.dll"]
