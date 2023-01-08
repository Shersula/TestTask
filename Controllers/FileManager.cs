using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class FileManager : Controller
    {
        static Files Root = null;
        public IActionResult Index()
        {
            if (Root == null) Root = new Files("Проводник", false);
            return View(Root);
        }
        [HttpGet]
        public JsonResult GetInfo(int ElementCode)
        {
            return Json(Root.FindByHashCode(ElementCode));
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
