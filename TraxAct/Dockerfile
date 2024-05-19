# Use the .NET SDK base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Create a non-root user and group
RUN groupadd -r appgroup && useradd -r -g appgroup appuser

# Install MAUI Workloads (if required)
RUN dotnet workload install maui

# Copy the project files into the container
COPY TraxAct.sln ./
COPY src/ ./src/
COPY TraxActUnitTests/ ./TraxActUnitTests/

# Ensure the files have the correct permissions
RUN chown -R appuser:appgroup /app

# Switch to the non-root user
USER appuser

# Restore, build, and publish commands
RUN dotnet restore
RUN dotnet build --no-restore -c Release
RUN dotnet publish --no-restore -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy the non-root user and group from the build stage
COPY --from=build /etc/passwd /etc/passwd
COPY --from=build /etc/group /etc/group

# Ensure the output directory has the correct permissions
COPY --from=build /app/out .

# Set the ownership of the app directory to the non-root user
RUN chown -R appuser:appgroup /app

# Switch to the non-root user
USER appuser

EXPOSE 80
ENTRYPOINT ["dotnet", "TraxAct.dll"]