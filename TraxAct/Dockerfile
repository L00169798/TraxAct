# Use the .NET MAUI base image
FROM mcr.microsoft.com/dotnet/maui/sdk:latest AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files into the container
COPY . .

# Restore dependencies and build the application
RUN dotnet restore
RUN dotnet build -c Release

# Expose the port used by your application
EXPOSE 8080

# Run the application
CMD ["dotnet", "run", "--urls", "http://0.0.0.0:8080"]
