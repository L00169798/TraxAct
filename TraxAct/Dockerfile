# Use the .NET MAUI base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project files into the container
COPY TraxAct.sln ./
COPY src/ ./src/
COPY TraxActUnitTests/ ./TraxActUnitTests/
COPY TraxAct.Android/ ./TraxAct.Android/
COPY TraxAct.Windows/ ./TraxAct.Windows/

# Restore, build, and publish commands for Android project
RUN dotnet restore TraxAct.Android/TraxAct.Android.csproj
RUN dotnet build --no-restore TraxAct.Android/TraxAct.Android.csproj -c Release
RUN dotnet publish --no-restore -c Release -o out/TraxAct.Android TraxAct.Android/TraxAct.Android.csproj

# Restore, build, and publish commands for Windows project
RUN dotnet restore TraxAct.Windows/TraxAct.Windows.csproj
RUN dotnet build --no-restore TraxAct.Windows/TraxAct.Windows.csproj -c Release
RUN dotnet publish --no-restore -c Release -o out/TraxAct.Windows TraxAct.Windows/TraxAct.Windows.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "TraxAct.dll"]

