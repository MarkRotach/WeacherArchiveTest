namespace WeatherArchive.Database.Domain;

public class ReportWindDirection
{
    public long Id { get; set; }
    public required WeatherReport Report { get; set; }
    public required WindDirection WindDirection { get; set; }
}