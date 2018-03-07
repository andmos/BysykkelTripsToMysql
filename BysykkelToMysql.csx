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
private string InsertSql = @"INSERT INTO trip(start_station_id, start_time, end_station_id, end_time)  VALUES(@StartStationId, @StartTime, @EndStationId, @EndTime)";

Stopwatch stopWatch = new Stopwatch();
Console.WriteLine($"Adding data to database..");
stopWatch.Start();
InsertAllTripDataToDatabase(DbConnectionString, ParseAllTripFiles()); 
stopWatch.Stop();
var ts = stopWatch.Elapsed;
var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

Console.WriteLine($"RunTime {elapsedTime}");


private IEnumerable<Trips> ParseAllTripFiles()
{
    var tripDataJsonFiles = Directory.GetFiles(".","trips-*.json", SearchOption.AllDirectories);
    var allTripRecords = new List<Trips>(); 
    foreach(var file in tripDataJsonFiles)
    {
        allTripRecords.Add(JsonConvert.DeserializeObject<Trips>(File.ReadAllText(file)));
    }
    return allTripRecords; 
}

private void InsertAllTripDataToDatabase(string connectionString, IEnumerable<Trips> trips)
{
    using (var dbConnection = new MySqlConnection(connectionString))
    {
        dbConnection.Execute(InsertSql, trips.SelectMany(t => t.TripCollection));
        dbConnection.Close();
    }
}
private class Trips
{
    [JsonProperty("trips")]
    public IEnumerable<Trip> TripCollection { get; set; }
}
private class Trip 
{
    [JsonProperty("start_station_id")]
    public int StartStationId { get; set; }
    
    [JsonProperty("start_time")]
    public DateTime StartTime { get; set; }
    
    [JsonProperty("end_station_id")] 
    public int EndStationId { get; set; }
    
    [JsonProperty("end_time")]
    public DateTime EndTime { get; set; }
}
