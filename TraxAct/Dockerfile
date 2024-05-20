# Use the .NET SDK base image (Windows variant)
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project files into the container
COPY TraxAct.sln ./
COPY src/ ./src/
COPY TraxActUnitTests/ ./TraxActUnitTests/

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
