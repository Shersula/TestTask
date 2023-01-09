using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using TestTask.Models;
using static System.Net.WebRequestMethods;

namespace TestTask.Controllers
{
    public class FileManager : Controller
    {
        static List<Files> Root = null;
        public IActionResult Index()
        {
            if (Root == null)
            {
                Root = new List<Files>();
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady == true)
                    {
                        Files RootElement = new Files(drive.Name.Replace(":\\", ""), false, drive.TotalSize - drive.TotalFreeSpace);
                        Root.Add(RootElement);

                        AddDir(drive.RootDirectory.ToString(), RootElement);
                        AddFile(drive.RootDirectory.ToString(), RootElement);
                    }
                }
            }
            return View(Root);
        }

        public void AddDir(string directories, Files Root)
        {
            foreach (string file in Directory.GetDirectories(directories, "*.*"))
            {
                try
                {
                    Debug.WriteLine(file);
                    Files Element = new Files(file.Replace(directories, "").Replace("\\", ""), false, DirSize(new DirectoryInfo(file)), Root);
                    AddFile(file, Element);
                    AddDir(file, Element);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public void AddFile(string directories, Files Root)
        {
            foreach (string file in Directory.GetFiles(directories))
            {
                try
                {
                    new Files(file.Replace(directories, "").Replace("\\", ""), true, new FileInfo(file).Length, Root);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        [HttpGet]
        public JsonResult GetInfo(int ElementCode)
        {
            foreach(Files item in Root)
            {
                Files Element = item.FindByHashCode(ElementCode);
                if (Element != null) return Json(Element);
            }
            return Json(null);
        }
        [HttpGet]
        public JsonResult GetDirectory(int ParentCode)
        {
            foreach (Files item in Root)
            {
                Files Element = item.FindByHashCode(ParentCode);
                if(Element != null)
                {
                    Debug.WriteLine(Element.getDirectory());
                    List<Files> ChildList = Element.getChild();
                    if (ChildList != null && ChildList.Count > 0) return Json(ChildList);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public void AddElement([FromBody] PostJSONResult data)
        {
            foreach (Files item in Root)
            {
                Files Element = item.FindByHashCode(data.ParentCode);
                if (Element != null)
                {
                    new Files(data.Name, data.isAFile, 0, Element, data.Content);
                    break;
                }
            }
        }
        [HttpDelete]
        public void RemoveElement(int ElementCode)
        {
            foreach (Files item in Root)
            {
                Files Element = item.FindByHashCode(ElementCode);
                if (Element != null)
                {
                    Element.Remove();
                    break;
                }
            }
        }
    }
}
