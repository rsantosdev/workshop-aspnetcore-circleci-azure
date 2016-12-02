FROM microsoft/aspnetcore

WORKDIR /app

COPY src/WebAPIApplication/bin/Release/netcoreapp1.1/publish .

ENTRYPOINT ["dotnet", "WebAPIApplication.dll"]