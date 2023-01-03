namespace TestTask.Models
{
    public class Files
    {
        private Files parrent;
        private List<Files> children;
        private string Name;
        private int Weight;
        public Files(string name, int weight)
        {
            this.Name = name;
            this.Weight = weight;
            children = new List<Files>();
        }
        public Files(string name, int weight, Files Parent)
        {
            this.Name = name;
            this.Weight = weight;
            this.parrent = Parent;
            this.parrent.children.Add(this);
            this.children = new List<Files>();
        }
        public string getName()
        {
            return this.Name;
        }
        public int getWeight()
        {
            return this.Weight;
        }
        public bool RemoveChild(Files child)
        {
            return children.Remove(child);
        }
        public List<Files> getChild()
        {
            return children;
        }
    }
}
