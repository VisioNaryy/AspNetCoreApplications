FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ./HttpsTestDocker.sln ./
COPY ./HttpsTestDocker/HttpsTestDocker.csproj ./HttpsTestDocker/
RUN dotnet restore

COPY . .
RUN dotnet publish ./HttpsTestDocker/HttpsTestDocker.csproj -c Release -o /out/

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as final
WORKDIR /app
COPY --from=build /out .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "HttpsTestDocker.dll"]