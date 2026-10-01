FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/DevOpsDemo.Api/DevOpsDemo.Api.csproj src/DevOpsDemo.Api/
RUN dotnet restore src/DevOpsDemo.Api/DevOpsDemo.Api.csproj

COPY src/DevOpsDemo.Api/ src/DevOpsDemo.Api/
RUN dotnet publish src/DevOpsDemo.Api/DevOpsDemo.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DevOpsDemo.Api.dll"]
