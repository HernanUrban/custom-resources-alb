# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code and publish
COPY . ./
RUN dotnet publish -c Release -o out

# Use a smaller runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out .

ENV DOTNET_ENVIRONMENT=Docker

# Set the environment variable to specify the port
ENV ASPNETCORE_URLS=http://+:5273

# Expose the port the app is listening on
EXPOSE 5273

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "UniqueUuidApi.dll", "--launch-profile Docker"]
