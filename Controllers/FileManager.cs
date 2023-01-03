using Microsoft.AspNetCore.Mvc;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class FileManager : Controller
    {
        public IActionResult Index()
        {
            Files Root = new Files("Диск C", 10);
            new Files("Файл 1", 10, Root);
            new Files("Файл 2", 10, Root);
            new Files("Файл 3", 10, Root);
            new Files("Файл 4", 10, Root);

            return View(Root);
        }
    }
}
