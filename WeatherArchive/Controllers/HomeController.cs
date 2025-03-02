using Microsoft.AspNetCore.Mvc;
using WeatherArchive.Services;

namespace WeatherArchive.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    ArchiveUploader archiveUploader,
    ArchiveService archiveService)
    : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private const int PageSize = 20;

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ArchiveAsync(int page = 1, string month = "", int? year = null,
        CancellationToken cancellationToken = default)
    {
        var archive = await archiveService.GetWeatherArchiveAsync
        (
            page, PageSize, month, year, cancellationToken
        );
        
        return View(archive);
    }

    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IList<IFormFile> files)
    {
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName);
        
            if (extension is not (".xlsx" or ".xls"))
                ModelState.AddModelError("error", $"Недопустимый формат файла \"{file.FileName}\"!");
        }
        
        if (!ModelState.IsValid)
            return View();

        try
        {
            foreach (var file in files)
            {
                var fileStream = file.OpenReadStream();
                await archiveUploader.UploadAsync(fileStream);
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError("error", "Произошла ошибка обработки файлов!");
        }
        
        return View();
    }
}