using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherArchive.Models;
using WeatherArchive.Services;

namespace WeatherArchive.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ArchiveUploader _archiveUploader;

    public HomeController(ILogger<HomeController> logger,
        ArchiveUploader archiveUploader)
    {
        _logger = logger;
        _archiveUploader = archiveUploader;
    }

    public IActionResult Index()
    {
        return View();
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
                ModelState.AddModelError("", $"Недопустимый формат файла \"{file.FileName}\"!");
        }
        
        if (!ModelState.IsValid)
            return View();

        foreach (var file in files)
        {
            var fileStream = file.OpenReadStream();
            await _archiveUploader.UploadAsync(fileStream);
        }
        
        return View();
    }
}