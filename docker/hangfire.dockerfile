FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "src/OnlineShop.Web/OnlineShop.Web.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "OnlineShop.Web.dll", "--hangfire"]