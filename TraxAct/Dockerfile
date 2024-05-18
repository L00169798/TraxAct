# Use the .NET MAUI base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project files into the container
COPY TraxAct.sln ./
COPY src/ ./src/
COPY TraxActUnitTests/ ./TraxActUnitTests/

# Ensure permissions before installing workloads
RUN icacls "C:\Program Files\dotnet\sdk-manifests\7.0.100\microsoft.net.sdk.android\WorkloadManifest.json" /grant Everyone:F

# Install the required .NET MAUI workloads
RUN dotnet workload install maui
RUN dotnet workload restore

# Restore, build, and publish commands
RUN dotnet restore
RUN dotnet build --no-restore -c Release
RUN dotnet publish --no-restore -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "TraxAct.dll"]
