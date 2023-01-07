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
                Root = new Files("Диск C", false);
                Files RootTwo = new Files("Файл 1", true, 10, Root);
                new Files("Файл 2", true, 10, Root);
                new Files("Файл 3", true, 10, Root);
                new Files("Файл 4", true, 10, Root);

                new Files("Файл 10", false, 10, RootTwo);
                new Files("Файл 110", true, 10, RootTwo);
            }
            return View(Root);
        }
        [HttpGet]
        public JsonResult GetDirectory(int code)
        {
            return Json(Root.FindChildByHashCode(code));
        }

        [HttpPost]
        public void AddElement([FromBody] PostJSONResult data)
        {
            new Files(data.Name, data.isAFile, 10, Root.FindByHashCode(data.ParentCode));
        }
    }
}
