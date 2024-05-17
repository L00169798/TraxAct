# Use the .NET MAUI base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project files into the container
COPY TraxAct/src/TraxAct.csproj ./

# Restore dependencies
RUN dotnet restore

# Install .NET MAUI workloads (if needed)
RUN dotnet workload install maui-android maui-windows

# Build the application
RUN dotnet build -c Release

# Publish the application
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "TraxAct.dll"]
