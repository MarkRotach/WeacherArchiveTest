using System.Globalization;
using Microsoft.EntityFrameworkCore;
using WeatherArchive.Database;
using WeatherArchive.Database.Domain;
using WeatherArchive.Models;

namespace WeatherArchive.Services;

public class ArchiveService(WeatherArchiveDbContext context)
{
    public async Task<WeatherArchiveViewModel> GetWeatherArchiveAsync(
        int page, int pageSize, string month = null, int? year = 0,
        CancellationToken cancellationToken = default)
    {
        var reports = await GetWeatherReportsAsync(page, pageSize, month, year, cancellationToken);
        var availableYears = await GetAvailableYearsAsync(cancellationToken);
        var availableMonths = await GetAvailableMonthsAsync(cancellationToken);
        var pagesCount = await GetPagesCountAsync(pageSize, month, year, cancellationToken);

        return new WeatherArchiveViewModel
        {
            WeatherReports = reports,
            Years = availableYears,
            SelectedYear = year,
            Months = availableMonths.Select(m => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)),
            SelectedMonth = month,
            PagesCount = pagesCount,
            SelectedPage = page
        };
    }

    private async Task<IEnumerable<WeatherReportDTO>> GetWeatherReportsAsync(
        int page, int pageSize, string month, int? year, CancellationToken cancellationToken)
    {
        var reports = context.WeatherReports.AsNoTracking();
        reports = FilterReportsByMonthAndYear(reports, month, year);

        return await reports
            .Include(r => r.WindDirections)
            .ThenInclude(d => d.WindDirection)
            .OrderBy(r => r.Date)
            .ThenBy(r => r.Time)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => MapToDto(r))
            .ToListAsync(cancellationToken);
    }

    private static WeatherReportDTO MapToDto(WeatherReport weatherReport)
    {
        var windDirections = weatherReport.WindDirections.Any() 
            ? string.Join
            (
                ", ",
                weatherReport.WindDirections
                    .Select(d => d.WindDirection.Name)
                    .OrderBy(s => s)
            )
            : "Штиль";
        
        return new WeatherReportDTO
        {
            Date = weatherReport.Date,
            Time = weatherReport.Time,
            Temperature = weatherReport.Temperature,
            Humidity = weatherReport.Humidity,
            DewPoint = weatherReport.DewPoint,
            Pressure = weatherReport.Pressure,
            WindDirections = windDirections,
            WindSpeed = weatherReport.WindSpeed,
            Cloudiness = weatherReport.Cloudiness,
            LowerCloudCover = weatherReport.LowerCloudCover,
            HorizontalVisibility = weatherReport.HorizontalVisibility,
            Phenomena = weatherReport.Phenomena,
        };
    }

    private async Task<IEnumerable<int>> GetAvailableMonthsAsync(CancellationToken cancellationToken)
        => await context.WeatherReports
            .AsNoTracking()
            .Select(r => r.Date.Month)
            .Distinct()
            .OrderBy(month => month)
            .ToListAsync(cancellationToken);
    
    private async Task<IEnumerable<int>> GetAvailableYearsAsync(CancellationToken cancellationToken)
        => await context.WeatherReports
            .AsNoTracking()
            .Select(r => r.Date.Year)
            .Distinct()
            .OrderBy(year => year)
            .ToListAsync(cancellationToken);
    
    private async Task<int> GetPagesCountAsync(int pageSize, string month, int? year,
        CancellationToken cancellationToken)
    {
        var reports = context.WeatherReports.AsNoTracking();
        reports = FilterReportsByMonthAndYear(reports, month, year);
        var reportsCount = await reports.CountAsync(cancellationToken);
        return reportsCount / pageSize;
    }
    
    private static IQueryable<WeatherReport> FilterReportsByMonthAndYear(
        IQueryable<WeatherReport> reports, string month, int? year)
    {
        if (!string.IsNullOrWhiteSpace(month))
        {
            var monthAsInt = DateOnly.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month;
            
            reports = reports.Where
            (
                a => a.Date.Month == monthAsInt
            );
        }

        if (year.HasValue)
            reports = reports.Where(a => a.Date.Year == year.Value);
        
        return reports;
    }
}