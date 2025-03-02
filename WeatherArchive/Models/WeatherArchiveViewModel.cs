namespace WeatherArchive.Models;

public sealed class WeatherArchiveViewModel
{
    public IEnumerable<WeatherReportDTO> WeatherReports { get; init; }
    
    public IEnumerable<int> Years { get; init; }
    public int? SelectedYear { get; init; }
    
    public IEnumerable<string> Months { get; init; }
    public string SelectedMonth { get; init; }
    
    public int PagesCount { get; init; }
    public int SelectedPage { get; init; }
}