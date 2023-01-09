using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace TestTask.Models
{
    public class Files
    {
        private Files parrent;
        private List<Files> children;
        public bool isAFile { private set; get; }
        public string Name { private set; get; }
        public long Weight { private set; get; }
        public int Hash { private set; get; }
        private void UpdateWeight()
        {
            this.Weight = 0;
            foreach (Files child in this.getChild())
            {
                this.Weight += child.Weight;
            }
            if (this.parrent != null) this.parrent.UpdateWeight();
        }
        public Files(string name, bool isFile, long weight=0)
        {
            this.Name = name;
            this.children = new List<Files>();
            this.Hash = this.GetHashCode();
            this.isAFile = isFile;
            this.Weight = weight;
        }
        public Files(string name, bool isFile, long weight, Files Parent, string content = "")
        {
            this.Name = name;
            this.parrent = Parent;
            this.parrent.children.Add(this);
            this.children = new List<Files>();
            this.Hash = this.GetHashCode();
            this.isAFile = isFile;
            this.Weight = weight;

            if(this.isAFile == true)
            {
                if (new FileInfo(this.getDirectory()).Exists == false)
                {
                    File.WriteAllTextAsync(this.getDirectory(), content);
                    this.Weight = new FileInfo(this.getDirectory()).Length;
                }
            }
            else
            {
                if(new DirectoryInfo(this.getDirectory()).Exists == false)
                {
                    Directory.CreateDirectory(this.getDirectory());
                    this.Weight = 0;
                }
            }
            if (this.parrent != null) this.parrent.UpdateWeight();
        }
        public void Remove()
        {
            try
            {
                if (this.isAFile == true) new FileInfo(this.getDirectory()).Delete();
                else new DirectoryInfo(this.getDirectory()).Delete();
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            Files Parent = this.parrent;
            this.parrent = null;

            if (Parent != null)
            {
                Parent.children.Remove(this);
                Parent.UpdateWeight();
            }
            this.children.Clear();
        }
        public List<Files> getChild()
        {
            return this.children;
        }

        public Files FindByHashCode(int HashCode)
        {
            if (this.Hash == HashCode) return this;

            foreach (Files child in this.getChild())
            {
                if (child.Hash == HashCode) return child;
            }

            foreach (Files child in this.getChild())
            {
                Files Finded = child.FindByHashCode(HashCode);
                if(Finded != null) return Finded;
            }
            return null;
        }

        public string getDirectory()
        {
            string path = "";
            if (this.parrent != null)
            {
                path += this.parrent.getDirectory() + "\\" + this.Name;
            }
            else path = this.Name + ":";
            return path;
        }
    }
}
