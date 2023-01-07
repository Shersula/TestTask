using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class FileManager : Controller
    {
        static Files Root = null;
        public IActionResult Index()
        {
            if (Root == null)
            {
                Root = new Files("Диск C");
                Files RootTwo = new Files("Файл 1", 10, Root);
                new Files("Файл 2", 10, Root);
                new Files("Файл 3", 10, Root);
                new Files("Файл 4", 10, Root);

                new Files("Файл 10", 10, RootTwo);
                new Files("Файл 110", 10, RootTwo);
            }
            return View(Root);
        }
        [HttpGet]
        public JsonResult GetDirectory(int code)
        {
            return Json(Root.FindByHashCode(code));
        }
    }
}
