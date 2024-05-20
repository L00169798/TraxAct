FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY TraxAct/src/*.csproj ./TraxAct/src/

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out --verbosity detailed

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "TraxAct.dll"]
