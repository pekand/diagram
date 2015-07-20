namespace Diagram
{
    public class Options
    {
        /*
         * OPTIONS FOR DIAGRAM
         * evry diagram can have diferent options
         */

        public bool readOnly = false;      // nastavy diagram len na prezeranie
        public bool grid = true;                  // vykreslit grid
        public bool borders = false;              // vykreslovanie ohraničení prvkov 
        public bool coordinates = false;          // vykreslit aktualne súradnice
        public Position shift = new Position();
        public int layer = 0;
        public Position firstLayereShift = new Position();
        public int Left = 0;
        public int Top = 0;
        public int Width = 100;
        public int Height = 100;
        public int WindowState = 0; // 0 nenastavene; 1 maximzlizovane; 2 normalne; 3 minimzlizovane

        public Position startPosition = new Position();
        public int keyArrowSlowSpeed = 1;
        public int keyArrowFastSpeed = 100;

        public string colorDirectory = "#FFCCCC";
        public string colorFile = "#EB99EB";
        public string colorLink = "#FFCCCC";
    }
}
