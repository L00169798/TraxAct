# Use the .NET MAUI base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files into the container
COPY . .

# Restore dependencies and build the application
RUN dotnet restore
RUN dotnet build -c Release

# Expose the port used by your application
EXPOSE 80

# Command to run the application
ENTRYPOINT ["dotnet", "TraxAct\TraxAct\src\bin\Debug\net7.0\TraxAct.dll"]
