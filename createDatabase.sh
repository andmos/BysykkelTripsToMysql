#! /bin/bash

docker create --name data --volume /var/lib/mysql mysql:5.6 /bin/true
docker run --name bysykkeldb -p 3306:3306 --volumes-from data -e MYSQL_DATABASE=bysykkeldb -e MYSQL_ROOT_PASSWORD=passord1 -d mysql:5.6
sleep 10
docker run -it --link bysykkeldb:mysql --rm -v $(pwd):/opt/ mysql sh -c 'exec mysql -h"$MYSQL_PORT_3306_TCP_ADDR" -P"$MYSQL_PORT_3306_TCP_PORT" -uroot -p"$MYSQL_ENV_MYSQL_ROOT_PASSWORD" bysykkeldb < /opt/tripTable.sql'
