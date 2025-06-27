# Use the official .NET 8.0 ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory
WORKDIR /app

# Copy all files from the publish output (assumes you build/publish locally)
COPY ./bin/Release/net8.0/ ./

# Set the entrypoint
ENTRYPOINT ["dotnet", "VP.Totum.Url.dll"]