using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WeatherArchive.Database;
using WeatherArchive.Database.Domain;

namespace WeatherArchive.Services;

public sealed class ArchiveUploader(WeatherArchiveDbContext context)
{
    private readonly WeatherArchiveDbContext _context = context;
    private IEnumerable<WindDirection> _windDirections;
    private const int StartProcessingRowIndex = 4;
    private const int ValuedColumnsCount = 12;

    public async Task UploadAsync(Stream fileStream)
    {
        _windDirections = await _context.WindDirections.ToListAsync();
        
        using var workbook = new XSSFWorkbook(fileStream);

        for (var index = 0; index < workbook.NumberOfSheets; index++)
        {
            var sheet = workbook.GetSheetAt(index);
            await UploadArchiveToDatabaseAsync(sheet);
        }
        
        await _context.SaveChangesAsync();
    }

    private async Task UploadArchiveToDatabaseAsync(ISheet sheet)
    {
        for (var rowIndex = StartProcessingRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
        {
            var row = sheet.GetRow(rowIndex);
            if (row is null) 
                continue;

            var columnsCount = row.LastCellNum;
            if (columnsCount != ValuedColumnsCount)
                continue;
            
            var date = DateOnly.Parse(row.GetCell(0).StringCellValue);
            var time = TimeOnly.Parse(row.GetCell(1).StringCellValue);
            
            var isExistsReportByDate = await _context.WeatherReports
                .AnyAsync(r => r.Date == date && r.Time == time);
            
            if (isExistsReportByDate)
                continue;
            
            var weatherReport = CreateWeatherReport(row, date, time);
            var reportWindDirections = CreateReportWindDirections(row, weatherReport);

            await _context.WeatherReports.AddAsync(weatherReport);
            await _context.ReportWindDirections.AddRangeAsync(reportWindDirections);
        }
    }

    private static WeatherReport CreateWeatherReport(IRow row, DateOnly date, TimeOnly time)
    {
        return new WeatherReport
        {
            Date = date,
            Time = time,
            Temperature = Math.Round(row.GetCell(2).NumericCellValue, 1),
            Humidity = Math.Round(row.GetCell(3).NumericCellValue, 2),
            DewPoint = Math.Round(row.GetCell(4).NumericCellValue, 1),
            Pressure = (int)row.GetCell(5).NumericCellValue,
            WindSpeed = ParseNullableInt(row.GetCell(7), defaultValue: 0),
            Cloudiness = ParseNullableInt(row.GetCell(8)),
            LowerCloudCover = ParseIntCellAsString(row.GetCell(9)),
            HorizontalVisibility = ParseIntCellAsString(row.GetCell(10)),
            Phenomena = row.GetCell(11)?.StringCellValue ?? string.Empty
        };
    }
    
    private IEnumerable<ReportWindDirection> CreateReportWindDirections(IRow row, WeatherReport report)
    {
        var windDirectionsCellValue = row.GetCell(6).StringCellValue;
        
        if (string.IsNullOrWhiteSpace(windDirectionsCellValue) 
            || windDirectionsCellValue.Equals("штиль", StringComparison.OrdinalIgnoreCase))
            yield break;
        
        var windDirectionValues = windDirectionsCellValue.Split(',').Select(s => s.Trim());

        foreach (var windDirectionValue in windDirectionValues)
        {
            yield return new ReportWindDirection()
            {
                Report = report,
                WindDirection = _windDirections.FirstOrDefault
                (
                    wd => wd.Name.Equals(windDirectionValue, StringComparison.OrdinalIgnoreCase)
                )
            };
        }
    }

    private static int? ParseNullableInt(ICell cell, int? defaultValue = null)
        => cell.CellType == CellType.String && string.IsNullOrWhiteSpace(cell.StringCellValue)
            ? defaultValue
            : (int)cell.NumericCellValue;
    
    private static string ParseIntCellAsString(ICell cell)
        => cell.CellType == CellType.String
            ? cell.StringCellValue
            : ((int)cell.NumericCellValue).ToString();
}