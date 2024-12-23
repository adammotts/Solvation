# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory to /app
WORKDIR /app

# Copy the current directory (i.e., ./backend) contents into the container
COPY . ./

# Restore the dependencies for the C# project
RUN dotnet restore ./backend.csproj

# Build the project
RUN dotnet build ./backend.csproj -c Release -o /app/build

# Publish the project to /app/publish
RUN dotnet publish ./backend.csproj -c Release -o /app/publish

# Use the .NET runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory to /app
WORKDIR /app

# Expose port 80
EXPOSE 80

# Set environment variables
ENV DATABASE_USER_USERNAME=${DATABASE_USER_USERNAME}
ENV DATABASE_USER_PASSWORD=${DATABASE_USER_PASSWORD}
ENV MONGO_CONNECTION_STRING=${MONGO_CONNECTION_STRING}

# Copy the published files from the build stage
COPY --from=build /app/publish .

# Set the entrypoint for the container to run the application
ENTRYPOINT ["dotnet", "backend.dll"]
