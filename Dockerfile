FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY DevOpsDemo.Api.csproj 
RUN dotnet restore DevOpsDemo.Api.csproj

COPY src/DevOpsDemo.Api/ src/DevOpsDemo.Api/
RUN dotnet publish DevOpsDemo.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DevOpsDemo.Api.dll"]
