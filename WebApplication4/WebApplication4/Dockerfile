FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY WebApplication4/WebApplication4.csproj WebApplication4/
RUN dotnet restore WebApplication4/WebApplication4.csproj
COPY . .
WORKDIR /src/WebApplication4
RUN dotnet build WebApplication4.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebApplication4.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApplication4.dll"]
