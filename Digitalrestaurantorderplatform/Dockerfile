# Base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Set the entry point
ENTRYPOINT ["dotnet", "Digitalrestaurantorderplatform.dll"]