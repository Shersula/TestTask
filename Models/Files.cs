using System.Collections.Generic;
using System.Diagnostics;

namespace TestTask.Models
{
    public class Files
    {
        private Files parrent;
        private List<Files> children;
        public bool isAFile { private set; get; }
        public string Name { private set; get; }
        public int Weight { private set; get; }
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
        public Files(string name, bool File, int weight=0)
        {
            this.Name = name;
            this.children = new List<Files>();
            this.Hash = this.GetHashCode();
            this.isAFile = File;
            this.Weight = weight;
        }
        public Files(string name, bool File, int weight, Files Parent)
        {
            this.Name = name;
            this.parrent = Parent;
            this.parrent.children.Add(this);
            this.children = new List<Files>();
            this.Hash = this.GetHashCode();
            this.isAFile = File;
            this.Weight = weight;
            if (this.parrent != null) this.parrent.UpdateWeight();
        }
        public void Remove()
        {
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
    }
}
