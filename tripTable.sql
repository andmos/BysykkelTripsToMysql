CREATE TABLE trip (
    start_station_name VARCHAR(255),
    start_station_id INT,
    start_station_latitude DECIMAL(10, 8),
    start_station_longitude DECIMAL(11, 8),
    started_at DATETIME,
    end_station_name VARCHAR(255),
    end_station_id INT,
    end_station_latitude DECIMAL(10, 8),
    end_station_longitude DECIMAL(11, 8),
    ended_at DATETIME,
    duration INT,
    UNIQUE KEY record (start_station_id, started_at, end_station_id, ended_at)

)ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;