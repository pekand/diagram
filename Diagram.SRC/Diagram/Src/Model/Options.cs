
/*
    class Options
*/

namespace Diagram
{

    /// <summary>
    /// opened diagram options</summary>
    public class Options
    {
        /*************************************************************************************************************************/
        // POSITION

        public Position shift = new Position();            // startup position in diagram
        public Position firstLayereShift = new Position(); // position in layer

        /*************************************************************************************************************************/
        // HOME AND END POSITIONS

        public Position homePosition = new Position();     // diagram start and home key position
        public int homeLayer = 0;                          // startup layer after diagram open
        public Position endPosition = new Position();      // diagram end key position seet by end key
        public int endLayer = 0;                           // startup layer after diagram open

        /*************************************************************************************************************************/
        // STATES

        public bool readOnly = false;                      // diagram is read only
        public bool grid = true;                           // show grid
        public bool borders = false;                       // show node borders
        public bool coordinates = false;                   // show coordinates for debuging purpose        
        public bool openLayerInNewView = false;            // show grid

        /*************************************************************************************************************************/
        // WINDOW STATES

        public bool restoreWindow = false;                 // restore last window position and state 
        public int Left = 0;                               // diagram view position 
        public int Top = 0;                                // diagram view position 
        public int Width = 100;                            // diagram view position 
        public int Height = 100;                           // diagram view position 
        public int WindowState = 0;                        // 0 unset; 1 maximalized; 2 normal; 3 minimalized

        /*************************************************************************************************************************/
        // MOVE SPEED

        public int keyArrowSlowMoveNodeSpeed = 1;          // node moving speed
        public int keyArrowFastMoveNodeSpeed = 100;        // node moving speed
        public int keyArrowSlowSpeed = 50;                 // node moving speed

        /*************************************************************************************************************************/
        // COLORS

        public string colorDirectory = "#AF92FF";          // color for node linked with directory
        public string colorFile = "#D9CCFF";               // color for node linked with file
        public string colorLink = "#FFCCCC";               // color for node linked with url
        public string colorAttachment = "#C495DB";         // color for node linked with url
        public string colorNode = "#FFFFB8";               // new node color

        /*************************************************************************************************************************/
        // ICON

        public string icon = "";                           // diagram custom icon
    }
}
