using System.Collections.Generic;

namespace Diagram
{
    /// <summary>
    /// collection of polygons</summary>
    public class Polygons : List<Polygon> //UID7474399328
    {
        public Polygons()
        {
        }

        public Polygons(int capacity) : base(capacity)
        {
        }

        public Polygons(Polygons collection) : base(collection)
        {
        }

        /*************************************************************************************************************************/
        // SETTERS AND GETTERS

        public void Copy(Polygons polygons)
        {
            this.Clear();

            foreach (Polygon polygon in polygons)
            {
                this.Add(polygon.clone());
            }

        }
    }
}