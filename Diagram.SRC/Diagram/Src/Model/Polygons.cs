using System.Collections.Generic;

namespace Diagram
{
    /// <summary>
    /// collection of polygons</summary>
    public class Polygons : List<Polygon> //UID7474399328
    {
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