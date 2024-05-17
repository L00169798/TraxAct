# Use the .NET MAUI base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project files into the container
COPY TraxAct/src/TraxAct.csproj ./

# Restore dependencies and build the application
dotnet workload restore  
        dotnet restore  
        dotnet workload install maui-android maui-windows  
        dotnet build TraxAct/TraxAct.sln --configuration Release 

# Publish the application
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "TraxAct.dll"]

