#!/bin/sh

# Ожидание PostgreSQL
until PGPASSWORD=$POSTGRES_PASSWORD psql -h "db" -U "postgres" -d "online_shop" -c '\q'; do
  echo "Waiting for PostgreSQL..."
  sleep 1
done

# Применение миграций
dotnet ef database update --project ../OnlineShop.Infrastructure --startup-project .

# Запуск приложения
exec dotnet OnlineShop.Web.dll