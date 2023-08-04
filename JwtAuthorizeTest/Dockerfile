# Use the base ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app

# Use the .NET SDK for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy and restore the server, client, and shared projects
COPY Server/JwtAuthorizeTest.Server.csproj Server/
COPY Client/JwtAuthorizeTest.Client.csproj Client/
COPY Shared/JwtAuthorizeTest.Shared.csproj Shared/
RUN dotnet restore Server/JwtAuthorizeTest.Server.csproj

# Copy the whole solution and build the server
COPY . .
WORKDIR /src/Server
RUN dotnet build JwtAuthorizeTest.Server.csproj -c Release -o /app/build

# Publish the server
FROM build AS publish
RUN dotnet publish JwtAuthorizeTest.Server.csproj -c Release -o /app/publish /p:UseAppHost=false




# Use the base image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JwtAuthorizeTest.Server.dll"]
