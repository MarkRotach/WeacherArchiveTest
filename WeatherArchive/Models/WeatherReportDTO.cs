namespace WeatherArchive.Models;

public sealed class WeatherReportDTO
{
    public required DateOnly Date { get; set; }
    public required TimeOnly Time { get; set; }
    public required double Temperature { get; set; }
    public required double Humidity { get; set; }
    public required double DewPoint { get; set; }
    public required int Pressure { get; set; }
    public string WindDirections { get; set; }
    public int? WindSpeed { get; set; }
    public int? Cloudiness { get; set; }
    public string LowerCloudCover { get; set; }
    public string HorizontalVisibility { get; set; }
    public string Phenomena { get; set; }
}