namespace WeatherArchive.Database.Domain;

public sealed class WeatherReport
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Дата
    /// </summary>
    public required DateOnly Date { get; set; }
    
    /// <summary>
    /// Московское время
    /// </summary>
    public required TimeOnly Time { get; set; }
    
    /// <summary>
    /// Температура
    /// </summary>
    public required double Temperature { get; set; }
    
    /// <summary>
    /// Влажность
    /// </summary>
    public required double Humidity { get; set; }
    
    /// <summary>
    /// Точка росы
    /// </summary>
    public required double DewPoint { get; set; }
    
    /// <summary>
    /// Атмосферное давление
    /// </summary>
    public required int Pressure { get; set; }
    
    /// <summary>
    /// Направления ветра
    /// </summary>
    public IEnumerable<ReportWindDirection> WindDirections { get; set; }
    
    /// <summary>
    /// Скорость ветра
    /// </summary>
    public int? WindSpeed { get; set; }
    
    /// <summary>
    /// Облачность
    /// </summary>
    public int? Cloudiness { get; set; }
    
    /// <summary>
    /// Нижняя граница облачности
    /// </summary>
    public required int LowerCloudCover { get; set; }
    
    /// <summary>
    /// Горизонтальная видимость
    /// </summary>
    public int? HorizontalVisibility { get; set; }
    
    /// <summary>
    /// Погодные явления
    /// </summary>
    public string Phenomena { get; set; }
}