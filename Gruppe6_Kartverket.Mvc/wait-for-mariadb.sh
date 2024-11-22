#!/bin/sh

# Wait for the MariaDB service to be ready
until nc -z -v -w30 mariadb 3306
do
  echo "Waiting for MariaDB database connection..."
  sleep 5
done

echo "MariaDB is up - executing command"
exec "$@"

chmod +x wait-for-mariadb.sh