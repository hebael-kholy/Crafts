# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /App

# copy everything
COPY . ./

# restore all project dependencies
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build /App/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Crafts.Api.dll"]