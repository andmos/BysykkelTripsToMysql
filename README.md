BysykkelTripsToMysql
===

Simple script to add trip data from [Trondheim Bysykkel](https://trondheimbysykkel.no/en/open-data) to MySQL.

Download trip-data JSON files and run `dotnet script BysykkelToMysql.csx`.

Example query, sort longest legal trips by duration:

```sql
select start_station_name, start_station_id, started_at, end_station_name, end_station_id, ended_at, duration from trip WHERE duration < 2700 GROUP BY duration desc;

```