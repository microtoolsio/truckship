FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY *.sln ./
COPY Gateway/Gateway.csproj Gateway/
COPY Gateway.Domain/Gateway.Domain.csproj Gateway.Domain/
COPY Gateway.Core/Gateway.Core.csproj Gateway.Core/
RUN dotnet restore
COPY . .
WORKDIR /src/Gateway
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Gateway.dll"]
