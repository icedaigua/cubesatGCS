using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satClient.Model
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
