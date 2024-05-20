FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /TraxAct

# Copy csproj and restore as distinct layers
COPY src/*.csproj ./src/

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out --verbosity detailed

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /TraxAct
COPY --from=build /TraxAct/out .
ENTRYPOINT ["dotnet", "TraxAct.dll"]
