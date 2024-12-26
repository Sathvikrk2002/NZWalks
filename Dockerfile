# Use the official ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NZWalks.API/NZWalks.API.csproj", "NZWalks.API/"]
RUN dotnet restore "NZWalks.API/NZWalks.API.csproj"
COPY . .
WORKDIR "/src/NZWalks.API"
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Use the base image and copy the published files
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NZWalks.API.dll"]
