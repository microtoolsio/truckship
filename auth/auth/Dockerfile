FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY *.sln ./
COPY Auth/Auth.csproj Auth/
COPY Auth.Domain/Auth.Domain.csproj Auth.Domain/
COPY Auth.Core/Auth.Core.csproj Auth.Core/
RUN dotnet restore
COPY . .
WORKDIR /src/Auth
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Auth.dll"]
