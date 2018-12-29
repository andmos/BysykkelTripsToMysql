#! "netcoreapp2.0"
#r "nuget: Newtonsoft.Json, 10.0.3"
#r "nuget: Dapper, 1.50.4 "
#r "nuget: MySql.Data, 6.10.6"
#r "nuget: System.Net.Http, 4.3.3"
using Newtonsoft.Json; 
using MySql.Data.MySqlClient;
using Dapper; 
using System.Net.Http; 

private readonly string DbConnectionString = "server=127.0.0.1;uid=root;pwd=passord1;database=bysykkeldb;";
private string InsertSql = @"INSERT INTO trip(start_station_name, start_station_id, start_station_latitude, start_station_longitude, started_at, end_station_name, end_station_id, end_station_latitude, end_station_longitude, ended_at, duration)  VALUES(@StartStationName, @StartStationId, @StartStationLatitude, @StartStationLongitude, @StartTime, @EndStationName, @EndStationId, @EndStationLatitude, @EndStationLongitude, @EndTime, @Duration) ON DUPLICATE KEY UPDATE start_station_id = @StartStationId, started_at = @StartTime, end_station_id = @EndStationId, ended_at = @EndTime";

Stopwatch stopWatch = new Stopwatch();
Console.WriteLine($"Adding data to database..");
stopWatch.Start();
InsertAllTripDataToDatabase(DbConnectionString, ParseAllMonthlyTripFiles()); 
stopWatch.Stop();
var ts = stopWatch.Elapsed;
var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

Console.WriteLine($"RunTime {elapsedTime}");


private IEnumerable<Trip> ParseAllMonthlyTripFiles()
{
    var tripDataJsonFiles = Directory.GetFiles(".","trips-*.json", SearchOption.AllDirectories);
    var allTripRecords = new List<Trip>(); 
    foreach(var file in tripDataJsonFiles)
    {
        allTripRecords.AddRange(JsonConvert.DeserializeObject<IEnumerable<Trip>>(File.ReadAllText(file)));
    }
    return allTripRecords; 
}

private void InsertAllTripDataToDatabase(string connectionString, IEnumerable<Trip> trips)
{
    using (var dbConnection = new MySqlConnection(connectionString))
    {
        dbConnection.Execute(InsertSql, trips);
        dbConnection.Close();
    }
}

private class Trip 
{
    [JsonProperty("start_station_name")]
    public string StartStationName {get; set;}

    [JsonProperty("start_station_id")]
    public int StartStationId { get; set; }
    
    [JsonProperty("started_at")]
    public DateTime StartTime { get; set; }

    [JsonProperty("start_station_latitude")]
    public double StartStationLatitude { get; set; }
    
    [JsonProperty("start_station_longitude")]
    public double StartStationLongitude { get; set; }

    [JsonProperty("end_station_name")]
    public string EndStationName {get; set;}

    [JsonProperty("end_station_id")] 
    public int EndStationId { get; set; }
    
    [JsonProperty("ended_at")]
    public DateTime EndTime { get; set; }

    [JsonProperty("end_station_latitude")]
    public double EndStationLatitude { get; set; }
    
    [JsonProperty("end_station_longitude")]
    public double EndStationLongitude { get; set; }

    [JsonProperty("duration")]
    public int Duration {get; set;}
}
