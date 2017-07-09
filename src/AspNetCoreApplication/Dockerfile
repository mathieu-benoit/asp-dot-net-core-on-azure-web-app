FROM microsoft/aspnetcore:1.1.2
ARG source
WORKDIR /app
EXPOSE 5000
COPY $source .
ENTRYPOINT ["dotnet", "AspNetCoreApplication.dll"]