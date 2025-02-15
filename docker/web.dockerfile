FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем ВСЕ .csproj файлы сохраняя структуру папок
COPY ["src/OnlineShop.Web/OnlineShop.Web.csproj", "src/OnlineShop.Web/"]
COPY ["src/OnlineShop.Application/OnlineShop.Application.csproj", "src/OnlineShop.Application/"]
COPY ["src/OnlineShop.Domain/OnlineShop.Domain.csproj", "src/OnlineShop.Domain/"]
COPY ["src/OnlineShop.Infrastructure/OnlineShop.Infrastructure.csproj", "src/OnlineShop.Infrastructure/"]

# Восстанавливаем зависимости для основного проекта
RUN dotnet restore "src/OnlineShop.Web/OnlineShop.Web.csproj"

# Копируем весь исходный код
COPY . .

# Сборка
WORKDIR "/src/src/OnlineShop.Web"
RUN dotnet build "OnlineShop.Web.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "OnlineShop.Web.csproj" -c Release -o /app/publish

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.Web.dll"]