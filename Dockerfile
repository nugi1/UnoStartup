# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the project file and restore the dependencies
COPY ["UnoStartup/UnoStartup.csproj", "UnoStartup/"]
RUN dotnet restore "UnoStartup/UnoStartup.csproj"

# Copy the rest of the code
COPY . .

# Publish the app to the /out directory
RUN dotnet publish "UnoStartup/UnoStartup.csproj" -c Release -o /out

COPY wait-for-it.sh /app/wait-for-it.sh

RUN chmod +x /app/wait-for-it.sh

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Copy the published app from the build image
COPY --from=build /out .

# Set entry point
ENTRYPOINT ["dotnet", "UnoStartup.dll"]
