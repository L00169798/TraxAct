# Use the .NET MAUI base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project files into the container
COPY TraxAct/TraxAct.sln ./TraxAct
COPY TraxAct/src/TraxAct.csproj ./src/

# Restore dependencies
RUN dotnet restore
RUN dotnet workload install maui-android maui-windows
RUN dotnet build /TraxAct/TraxAct.sln --configuration Release 

# Publish the application
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "TraxAct.dll"]
