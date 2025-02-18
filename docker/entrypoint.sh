#!/bin/sh

# Ожидаем доступности PostgreSQL
echo "Waiting for PostgreSQL to become ready..."
while ! nc -z db 5432; do
  sleep 1
done

# Проверяем существование базы данных
if ! psql "postgresql://postgres:${POSTGRES_PASSWORD}@db:5432/postgres" -lqt | cut -d \| -f 1 | grep -qw ${POSTGRES_DB}; then
  echo "Creating database ${POSTGRES_DB}..."
  psql "postgresql://postgres:${POSTGRES_PASSWORD}@db:5432/postgres" -c "CREATE DATABASE ${POSTGRES_DB}"
fi

# Применяем миграции
echo "Applying database migrations..."
dotnet ef migrations add InitialCreate --project OnlineShop.Infrastructure --startup-project OnlineShop.Web --output-dir Migrations
dotnet ef database update --project OnlineShop.Infrastructure --startup-project OnlineShop.Web
exec "$@"

# Запускаем приложение
echo "Starting application..."
exec dotnet src/OnlineShop.Web/OnlineShop.Web.dll
