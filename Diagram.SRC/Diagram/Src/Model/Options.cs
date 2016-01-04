﻿namespace Diagram
{

    /// <summary>
    /// Diagram options</summary>
    public class Options
    {
        public bool readOnly = false;                      // diagram is read only
        public bool grid = true;                           // show grid
        public bool borders = false;                       // show node borders
        public bool coordinates = false;                   // show coordinates for debuging purpose
        public Position shift = new Position();            // startup position in diagram
        public Position firstLayereShift = new Position(); // position in layer
        public int Left = 0;                               // diagram view position 
        public int Top = 0;                                // diagram view position 
        public int Width = 100;                            // diagram view position 
        public int Height = 100;                           // diagram view position 
        public int WindowState = 0;                        // 0 unset; 1 maximalized; 2 normal; 3 minimalized

        public Position homePosition = new Position();     // diagram start and home key position
        public int homeLayer = 0;                          // startup layer after diagram open
        public Position endPosition = new Position();      //  diagram end key position seet by end key
        public int endLayer = 0;                           // startup layer after diagram open
        public int keyArrowSlowSpeed = 1;                  // node moving speed
        public int keyArrowFastSpeed = 100;                // node moving speed

        public string colorDirectory = "#AF92FF";          // color for node linked with directory
        public string colorFile = "#D9CCFF";               // color for node linked with file
        public string colorLink = "#FFCCCC";               // color for node linked with url
        public string colorAttachment = "#C495DB";         // color for node linked with url
        public string colorNode = "#FFFFB8";               // new node color
    }
}
