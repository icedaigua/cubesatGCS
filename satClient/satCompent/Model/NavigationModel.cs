using System.Collections.Generic;

namespace satCompent.Model
{
    public class NavigationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Parent { get; set; }
        public int Define { get; set; }
        public List<NavigationModel> Children { get; set; }
    }
}
