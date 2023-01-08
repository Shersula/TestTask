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
                new Files("Файл 1", true, 10, Root);
                new Files("Файл 2", true, 10, Root);
                new Files("Папка", false, 10, Root);
                new Files("Файл 4", true, 10, Root);
            }
            return View(Root);
        }

        [HttpGet]
        public JsonResult GetDirectory(int ParentCode)
        {
            List<Files> ChildList = Root.FindByHashCode(ParentCode).getChild();
            if (ChildList.Count <= 0) return Json(null);
            else return Json(ChildList);
        }

        [HttpPost]
        public void AddElement([FromBody] PostJSONResult data)
        {
            new Files(data.Name, data.isAFile, data.Weight, Root.FindByHashCode(data.ParentCode));
        }
        [HttpDelete]
        public void RemoveElement(int ElementCode)
        {
            Root.FindByHashCode(ElementCode).Remove();
        }
    }
}
