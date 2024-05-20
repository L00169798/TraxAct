# Use the .NET SDK base image (Windows variant)
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Create a non-root user and group using PowerShell
SHELL ["powershell", "-Command"]
RUN New-LocalGroup -Name 'appgroup' -Description 'Application Group'; \
    New-LocalUser -Name 'appuser' -Group 'appgroup' -NoPassword

# Install MAUI Workloads (if required)
RUN dotnet workload install maui

# Copy the project files into the container
COPY TraxAct.sln ./
COPY src/ ./src/
COPY TraxActUnitTests/ ./TraxActUnitTests/

# Ensure the files have the correct permissions
# Note: Windows does not support chown, permissions are managed differently

# Restore, build, and publish commands
RUN dotnet restore
RUN dotnet build --no-restore -c Release
RUN dotnet publish --no-restore -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy the app files from the build stage
COPY --from=build /app/out .

# Switch to the non-root user
USER appuser

EXPOSE 80
ENTRYPOINT ["dotnet", "TraxAct.dll"]
