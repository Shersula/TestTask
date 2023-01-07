using System.Diagnostics;

namespace TestTask.Models
{
    public class Files
    {
        private Files parrent;
        private List<Files> children;
        public string Name { private set; get; }
        public int Weight { private set; get; }
        public int Hash { private set; get; }
        private void UpdateWeight(int weight)
        {
            this.Weight += weight;
            if(this.parrent != null) this.parrent.UpdateWeight(weight);
        }
        public Files(string name, int weight=0)
        {
            this.Name = name;
            this.Weight = weight;
            children = new List<Files>();
            Hash = this.GetHashCode();
        }
        public Files(string name, int weight, Files Parent)
        {
            this.Name = name;
            this.Weight = weight;
            this.parrent = Parent;
            this.parrent.UpdateWeight(this.Weight);
            this.parrent.children.Add(this);
            this.children = new List<Files>();
            Hash = this.GetHashCode();
        }
        public bool RemoveChild(Files child)
        {
            return children.Remove(child);
        }
        public List<Files> getChild()
        {
            return children;
        }
        public List<Files> FindByHashCode(int HashCode)
        {
            if (this.Hash == HashCode) return this.getChild();
            foreach (Files child in getChild())
            {
                if (child.Hash == HashCode)
                {
                    if (child.getChild().Count <= 0) return null;
                    return child.getChild();
                }
                else return child.FindByHashCode(HashCode);
            }
            return null;
        }
    }
}
