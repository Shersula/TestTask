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
        private void UpdateWeight(int weight)
        {
            this.Weight += weight;
            if(this.parrent != null) this.parrent.UpdateWeight(weight);
        }
        public Files(string name, bool File, int weight=0)
        {
            this.Name = name;
            this.Weight = weight;
            this.children = new List<Files>();
            this.Hash = this.GetHashCode();
            this.isAFile = File;
        }
        public Files(string name, bool File, int weight, Files Parent)
        {
            this.Name = name;
            this.Weight = weight;
            this.parrent = Parent;
            this.parrent.UpdateWeight(this.Weight);
            this.parrent.children.Add(this);
            this.children = new List<Files>();
            this.Hash = this.GetHashCode();
            this.isAFile = File;
        }
        public bool RemoveChild(Files child)
        {
            return children.Remove(child);
        }
        public List<Files> getChild()
        {
            return children;
        }
        public List<Files> FindChildByHashCode(int HashCode)
        {
            if (this.Hash == HashCode) return this.getChild();
            foreach (Files child in getChild())
            {
                if (child.Hash == HashCode)
                {
                    if (child.getChild().Count <= 0) return null;
                    return child.getChild();
                }
                else return child.FindChildByHashCode(HashCode);
            }
            return null;
        }

        public Files FindByHashCode(int HashCode)
        {
            if (this.Hash == HashCode) return this;
            foreach (Files child in getChild())
            {
                if (child.Hash == HashCode) return child;
                else return child.FindByHashCode(HashCode);
            }
            return null;
        }
    }
}
