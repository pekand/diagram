using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Drawing.Imaging;
using System.Collections.Specialized;
using System.Text;

#if DEBUG
using System.Diagnostics;
#endif

/*

class DiagramView

    main;
    parentView;

    PopupMenu;
    DSave;
    DOpen;
    DColor;
    DFontColor;
    DFont;
    DImage;
    MoveTimer;
    defaultfontDialog;
    exportFile;
    saveTextFileDialog;
    DSelectDirectoryAttachment;
    DSelectFileAttachment;

    shift;
    startShift;

    startMousePos;
    startNodePos;
    vmouse;
    actualMousePos;

    key;
    keyshift;
    keyctrl;
    keyalt;

    stateDragSelection;
    stateMoveView;
    stateSelectingNodes;
    stateAddingNode;
    stateDblclick;
    stateZooming;
    stateSearching;
    stateSourceNodeAlreadySelected;
    stateCoping;

    zoomShift;
    zoomingDefaultScale;
    zoomingScale;
    currentScale;
    scale;

    diagram;

    sourceNode;
    selectedNodes;

    copySourceNode;
    copySelectedNodes;
    copySelectedLines;

    currentLayer;
    firstLayereShift;
    layersHistory;

    bottomScrollBar;
    rightScrollBar;

    editPanel;
    editLinkPanel;

    lastFound;
    searchFor;
    searhPanel;
    currentPosition;
    currentPositionLayer;
    nodesSearchResult;

    lastMarkNode;

    breadcrumbs;

    animationTimer;
    animationTimerSpeed;
    animationTimerCounter;

    zoomTimer;
    zoomTimerScale;
    zoomTimerStep;

    lineWidthForm;
    colorPickerForm;

    components;

    isFullScreen;

    // [FORM]
    InitializeComponent()
    DiagramView()
    DiagramViewLoad()
    DiagramApp_FormClosing()
    SetTitle()
    GoToHome()
    OpenViewAndGoToHome()
    setCurentPositionAsHomePosition()
    GoToEnd()
    OpenViewAndGoToEnd()
    setCurentPositionAsEndPosition()
    Position cursorPosition()
    formHide()
    rememberPosition()
    getWindowsStateCode()
    setWindowsStateCode()
    setIcon()

    // [SELECTION]
    ClearSelection()
    RemoveNodeFromSelection()
    SelectOnlyOneNode()
    SelectNode()
    SelectNodes()
    selectAll()

    // [EVENTS]
    DiagramApp_Paint()
    DiagramApp_MouseDoubleClick()
    DiagramApp_MouseDown()
    DiagramApp_MouseMove()
    DiagramApp_MouseUp()
    resetStates()
    DiagramApp_MouseWheel()
    ProcessCmdKey()
    DiagramApp_KeyDown()
    DiagramApp_KeyUp()
    DiagramApp_KeyPress()
    DiagramApp_DragDrop()
    DiagramApp_DragEnter()
    DiagramApp_Resize()
    MoveTimer_Tick()
    DiagramApp_Deactivate()

    // [LAYER]
    LayerIn()
    LayerOut()
    BuildLayerHistory()
    isNodeInLayerHistory()
    layerIn()
    layerInOrEdit()

    // [SEARCH]
    Search()
    SearchFirst()
    SearchNext()
    SearchPrev()
    showSearchPanel()
    SearchCancel()
    SearchClose()
    SearchPanelChanged()

    // [CLIPBOARD]
    copyLinkToClipboard()

    // [SCROLLBAR]
    moveScreenHorizontal()
    getPositionHorizontal()
    moveScreenVertical()
    getPositionVertical()
    positionChangeBottom()
    positionChangeRight()

    // [FILE]
    save()
    saveas()
    open()
    openDiagramDirectory()

    // [EXPORT]
    exportDiagramToPng()
    exportDiagramToTxt()

    // [DRAW]
    DrawDiagram()
    DrawGrid()
    DrawMiniScreen()
    DrawCoordinates()
    DrawAddNode()
    DrawNodes()
    DrawLines()

    // [DIAGRAM]
    setDiagram()
    getDiagram()

    // [VIEW]
    DiagramView_FormClosed()
    DiagramView_Activated()
    pageUp()
    pageDown()
    resetZoom()
    getMousePosition()
    fullScreenSwitch()

    // NODE
    CreateNode()
    addNodeAfterNode()
    addNodeBelowNode()
    openLinkDirectory()
    DeleteSelectedNodes()
    goToNode()
    goToPosition()
    isOnPosition()
    goToShift()
    goToLayer()
    findNodeInMousePosition()
    OpenLinkAsync()
    OpenLink()
    removeShortcuts()
    selectColor()
    changeColor()
    selectFontColor()
    selectFont()
    isSelectionTransparent()
    makeSelectionTransparent()
    selectDefaultFont()
    addImage()
    removeImagesFromSelection()
    hasSelectionImage()
    hasSelectionNotEmbeddedImage()
    makeImagesEmbedded()
    copy()
    cut()
    paste()
    pasteToNote()
    pasteToLink()
    alignToLine()
    alignToColumn()
    alignToGroup()
    sortNodes()
    splitNode()
    alignToLineGroup()
    alignLeft()
    alignRight()
    copyLink()
    copyNote()
    evaluateExpression()
    evaluateDate()
    promote()
    random()
    hideBackground()
    edit()
    rename()
    editLink()
    moveNodesToForeground()
    moveNodesToBackground()
    protectNodes()
    moveNodesToLeft()
    moveNodesToRight()
    moveNodesUp()
    moveNodesDown()
    isEditing()
    cancelEditing()
    hasSelectionAttachment()
    attachmentDeploy()
    attachmentAddFile()
    attachmentAddDirectory()
    attachmentRemove()
    getSelectedLines()
    switchMarkForSelectedNodes()
    markSelectedNodes()
    unMarkSelectedNodes()
    nextMarkedNode()
    prevMarkedNode()
    setNodeNameByLink()

    // [LINE]
    changeLineColor()
    changeLineWidth()
    resizeLineWidth()

    // [SCRIPT]
    evaluate()

    // [MOVE]
    goToNodeWithAnimation()
    animationTimer_Tick()

    // [ZOOM]
    zoomTimer_Tick()

    // [DEBUG]
    lastEvent; // remember last event log message for skiping similar messages
    logEvent() // log events only in debug mode to system console
 
*/
namespace Diagram
{

    public partial class DiagramView : Form
    {
        public Main main = null;
        public DiagramView parentView = null;

        public Popup PopupMenu;
        public SaveFileDialog DSave;
        public OpenFileDialog DOpen;
        public ColorDialog DColor;
        public ColorDialog DFontColor;
        public FontDialog DFont;
        public OpenFileDialog DImage;
        public Timer MoveTimer;
        public FontDialog defaultfontDialog;
        public SaveFileDialog exportFile;
        public SaveFileDialog saveTextFileDialog;
        public FolderBrowserDialog DSelectDirectoryAttachment;
        public OpenFileDialog DSelectFileAttachment;

        /*************************************************************************************************************************/

#if DEBUG
        // DEBUG TOOLS
        private string lastEvent = ""; // remember last event in console (for remove duplicate events)
#endif

        // ATRIBUTES SCREEN
        public Position shift = new Position();                   // left corner position

        public Position startShift = new Position();              // temporary left corner position before change in diagram

        // ATTRIBUTES MOUSE
        public Position startMousePos = new Position();           // start movse position before change
        public Position startNodePos = new Position();            // start node position before change
        public Position vmouse = new Position();                  // vector position in selected node before change
        public Position actualMousePos = new Position();          // actual mouse position in form in drag process

        // ATTRIBUTES KEYBOARDSTATES
        public char key = ' ';                   // last key character - for new node add
        public bool keyshift = false;            // actual shift key state
        public bool keyctrl = false;             // actual ctrl key state
        public bool keyalt = false;              // actual alt key state

        // ATTRIBUTES STATES
        public bool stateDragSelection = false;         // actual drag status
        public bool stateMoveView = false;              // actual move node status
        public bool stateSelectingNodes = false;        // actual selecting node status or creating node by drag
        public bool stateAddingNode = false;            // actual adding node by drag
        public bool stateDblclick = false;              // actual dblclick status
        public bool stateZooming = false;               // actual zooming by space status
        public bool stateSearching = false;             // actual search edit form status
        public bool stateSourceNodeAlreadySelected = false; // actual check if is clicket two time in same node for rename node
        public bool stateCoping = false;             // start copy part of diagram

        // ATTRIBUTES ZOOMING
        public Position zoomShift = new Position();// zoom view - left corner position before zoom space press
        public float zoomingDefaultScale = 1;      // zoom view - normal scale
        public float zoomingScale = 4;             // zoom view - scale in space preview
        public float currentScale = 1;             // zoom viev - scale before space zoom
        public float scale = 1;                    // zoom view - actual scale

        // ATTRIBUTES Diagram
        public Diagram diagram = null;             // diagram assigned to current view

        // ATTRIBUTES selected nodes
        public Node sourceNode = null;             // selected node by mouse
        public Nodes selectedNodes = new Nodes();  // all selected nodes by mouse

        // ATTRIBUTES copy nodes
        public Node copySourceNode = null;             // selected node by mouse for copy action
        public Nodes copySelectedNodes = new Nodes();  // all selected nodes by mouse for copy action
        public Lines copySelectedLines = new Lines();  // all selected nodes by mouse for copy action

        // ATTRIBUTES Layers
        public Layer currentLayer = null;
        public Position firstLayereShift = new Position();    // left corner position in zero top layer
        public List<Layer> layersHistory = new List<Layer>(); // layer history - last layer is current selected layer

        // COMPONENTS
        public ScrollBar bottomScrollBar = null;  // bottom scroll bar
        public ScrollBar rightScrollBar = null;   // right scroll bar

        // EDITPANEL
        public EditPanel editPanel = null;         // edit panel for add new node name
        public EditLinkPanel editLinkPanel = null; // edit panel for node link

        // SEARCHPANEL
        public int lastFound = -1;            // id of last node found by search panel
        public string searchFor = "";         // string selected by search panel
        public SearchPanel searhPanel = null; // search panel
        public Position currentPosition = new Position();
        public int currentPositionLayer = 0;
        public List<int> nodesSearchResult = new List<int>(); // all nodes found by search panel

        // MARKED NODES
        private int lastMarkNode = 0; // last marked node in navigation history

        // BREADCRUMBS
        public Breadcrumbs breadcrumbs = null;

        // MOVETIMER
        private Timer animationTimer = new Timer(); //timer for all animations (goto node animation)
        private Position animationTimerSpeed = new Position();
        private int animationTimerCounter = 0;

        // ZOOMTIMER
        private Timer zoomTimer = new Timer(); //zooming animation
        public float zoomTimerScale = 1;
        public float zoomTimerStep = 0;

        // LINEWIDTHFORM
        private LineWidthForm lineWidthForm = new LineWidthForm();

        // COLORPICKERFORM
        private ColorPickerForm colorPickerForm = new ColorPickerForm();

        // COMPONENTS
        private IContainer components;        
        
        public bool isFullScreen = false;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.DSave = new System.Windows.Forms.SaveFileDialog();
            this.DOpen = new System.Windows.Forms.OpenFileDialog();
            this.DColor = new System.Windows.Forms.ColorDialog();
            this.DFontColor = new System.Windows.Forms.ColorDialog();
            this.DFont = new System.Windows.Forms.FontDialog();
            this.DImage = new System.Windows.Forms.OpenFileDialog();
            this.MoveTimer = new System.Windows.Forms.Timer(this.components);
            this.defaultfontDialog = new System.Windows.Forms.FontDialog();
            this.exportFile = new System.Windows.Forms.SaveFileDialog();
            this.saveTextFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.DSelectDirectoryAttachment = new System.Windows.Forms.FolderBrowserDialog();
            this.DSelectFileAttachment = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            //
            // DSave
            //
            this.DSave.DefaultExt = "*.diagram";
            this.DSave.Filter = "Diagram (*.diagram)|*.diagram";
            //
            // DOpen
            //
            this.DOpen.DefaultExt = "*.diagram";
            this.DOpen.Filter = "Diagram (*.diagram)|*.diagram";
            //
            // DFont
            //
            this.DFont.Color = System.Drawing.SystemColors.ControlText;
            //
            // DImage
            //
            this.DImage.Filter = "All|*.bmp;*.jpg;*.jpeg;*.png;*.ico|Bmp|*.bmp|Jpg|*.jpg;*.jpeg|Png|*.png|Ico|*.ico" +
    "";
            //
            // MoveTimer
            //
            this.MoveTimer.Interval = 5;
            this.MoveTimer.Tick += new System.EventHandler(this.MoveTimer_Tick);
            //
            // exportFile
            //
            this.exportFile.DefaultExt = "*.png";
            this.exportFile.Filter = "Image (*.png) | *.png";
            //
            // saveTextFileDialog
            //
            this.saveTextFileDialog.DefaultExt = "*.txt";
            this.saveTextFileDialog.Filter = "Text file (*.txt)|*.txt";
            //
            // DSelectFileAttachment
            //
            this.DSelectFileAttachment.DefaultExt = "*.*";
            this.DSelectFileAttachment.Filter = "All files (*.*)|*.*";
            //
            // DiagramView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(383, 341);
            this.DoubleBuffered = true;
#if DEBUG
            this.Icon = global::Diagram.Properties.Resources.ico_diagram_debug;
#else
            this.Icon = global::Diagram.Properties.Resources.ico_diagram;
#endif
            this.KeyPreview = true;
            this.Name = "DiagramView";
            this.Text = "Diagram";
            this.Activated += new System.EventHandler(this.DiagramView_Activated);
            this.Deactivate += new System.EventHandler(this.DiagramApp_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiagramApp_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DiagramView_FormClosed);
            this.Load += new System.EventHandler(this.DiagramViewLoad);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DiagramApp_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DiagramApp_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DiagramApp_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DiagramApp_KeyUp);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DiagramApp_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DiagramApp_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DiagramApp_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DiagramApp_MouseUp);
            this.Resize += new System.EventHandler(this.DiagramApp_Resize);
            this.ResumeLayout(false);

        }

        /*************************************************************************************************************************/

        // FORM Constructor
        public DiagramView(Main main, Diagram diagram, DiagramView parentView = null)
        {
            this.main = main;
            this.diagram = diagram;
            this.parentView = parentView;

            // initialize layer history
            this.currentLayer = this.diagram.layers.GetLayer(0);
            this.layersHistory.Add(this.currentLayer);

            this.InitializeComponent();

            // initialize popup menu
			this.PopupMenu = new Popup(this.components, this);

            // initialize edit panel
            this.editPanel = new EditPanel(this);
            this.Controls.Add(this.editPanel);

            // initialize edit link panel
            this.editLinkPanel = new EditLinkPanel(this);
            this.Controls.Add(this.editLinkPanel);

            // initialize breadcrumbs
            this.breadcrumbs = new Breadcrumbs(this);

            // move timer
            this.animationTimer.Tick += new EventHandler(AnimationTimer_Tick);
            this.animationTimer.Interval = 10;
            this.animationTimer.Enabled = false;

            // move timer
            this.zoomTimer.Tick += new EventHandler(ZoomTimer_Tick);
            this.zoomTimer.Interval = 10;
            this.zoomTimer.Enabled = false;

            // lineWidthForm
            this.lineWidthForm.trackbarStateChanged += this.ResizeLineWidth;

            //colorPickerForm
            this.colorPickerForm.changeColor += this.ChangeColor;

            // custom diagram icon
            if (this.diagram.options.icon != "")
            {
                this.Icon = Media.StringToIcon(this.diagram.options.icon);
            }           
        }

        // FORM Load event -
        public void DiagramViewLoad(object sender, EventArgs e)
        {

            // predefined window position
            if (this.diagram.options.restoreWindow)
            {
                this.Left = this.diagram.options.Left;
                this.Top = this.diagram.options.Top;
                this.Width = this.diagram.options.Width;
                this.Height = this.diagram.options.Height;
                this.SetWindowsStateCode(this.diagram.options.WindowState);
            }
            else
            {
                this.Left = 50;
                this.Top = 40;
                this.Width = Media.ScreenWidth(this) - 100;
                this.Height = Media.ScreenHeight(this) - 100;
                this.WindowState = FormWindowState.Normal;
            }

            //Load Events
            this.MouseWheel += new MouseEventHandler(DiagramApp_MouseWheel);

            this.DragEnter += new DragEventHandler(DiagramApp_DragEnter);
            this.DragDrop += new DragEventHandler(DiagramApp_DragDrop);
            this.AllowDrop = true;

            // scrollbars
            bottomScrollBar = new ScrollBar(this, this.ClientRectangle.Width, this.ClientRectangle.Height, true);
            rightScrollBar = new ScrollBar(this, this.ClientRectangle.Width, this.ClientRectangle.Height, false);

            bottomScrollBar.OnChangePosition += new PositionChangeEventHandler(PositionChangeBottom);
            rightScrollBar.OnChangePosition += new PositionChangeEventHandler(PositionChangeRight);

            // set startup position
            if (this.parentView != null)
            {
                this.shift.Set(this.parentView.shift);
                this.GoToLayer(this.parentView.currentLayer.id);
                this.Width = this.parentView.Width;
                this.Height = this.parentView.Height;
                this.Top = this.parentView.Top;
                this.Left = this.parentView.Left;
                this.WindowState = this.parentView.WindowState;
            }
            else
            {
                this.GoToHome();
            }

            // custom diagram background image
            if (this.diagram.options.backgroundImage != null)
            {
                this.diagram.RefreshBackgroundImages();
            }
        }

        // FORM Quit Close
        public void DiagramApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool close = true;
            if (!this.diagram.SavedFile && (this.diagram.FileName == "" || !Os.FileExists(this.diagram.FileName))) // Ulozi ako novy subor
            {

                if (this.diagram.DiagramViews.Count() == 1) // can close if other views alredy opened
                {
                    var res = MessageBox.Show(Translations.saveBeforeExit, Translations.confirmExit, MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.Yes)
                    {
                        if (DSave.ShowDialog() == DialogResult.OK)
                        {
                            this.diagram.SaveXMLFile(this.DSave.FileName);
                            this.diagram.SetTitle();
                            close = true;
                        }
                        else
                        {
                            close = false;
                        }
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        close = false;
                    }
                    else
                    {
                        close = true;
                    }
                }
                else
                {
                    close = true;
                }
            }
            else if (!this.diagram.SavedFile && this.diagram.FileName != "" && Os.FileExists(this.diagram.FileName)) //ulozenie do aktualne otvoreneho suboru
            {
                if (this.diagram.DiagramViews.Count() == 1) // can close if other views alredy opened
                {
                    var res = MessageBox.Show(Translations.saveBeforeExit, Translations.confirmExit, MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.Yes)
                    {
                        this.diagram.SaveXMLFile(this.diagram.FileName);
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        close = false;
                    }
                    else
                    {
                        close = true;
                    }
                }
                else
                {
                    close = true;
                }
            }

            if (close) {
                this.main.ShowIfIsLastViews(this);
            }

            e.Cancel = !close;

        }

        // FORM Title - set windows title
        public void SetTitle()
        {
            if (this.diagram.FileName.Trim() != "")
                this.Text = Os.GetFileNameWithoutExtension(this.diagram.FileName);
            else
                this.Text = "Diagram";
            if (this.currentLayer.parentNode != null && this.currentLayer.parentNode.name.Trim() != "")
                this.Text += " - " + this.currentLayer.parentNode.name.Trim();
            if (!this.diagram.SavedFile)
                this.Text = "*" + this.Text;
        }

        // FORM go to home position - center window to home position
        public void GoToHome()
        {
            Nodes nodes = this.diagram.layers.SearchInAllNodes("@home");
            nodes.OrderByIdAsc();

            Node homenode = null;
            foreach (Node node in nodes)
            {
                if (node.link == "@home") {
                    homenode = node;
                }
            }

            if (homenode != null)
            {
                this.GoToNode(homenode);
            }
            else
            {
                this.shift.Set(diagram.options.homePosition);
                this.GoToLayer(diagram.options.homeLayer);
            }            
            this.diagram.InvalidateDiagram();
        }

        // FORM open view and go to home position
        public void OpenViewAndGoToHome()
        {
            DiagramView child = this.diagram.OpenDiagramView(this);
            child.GoToHome();
            child.Invalidate();
        }

        // FORM set home position
        public void SetCurentPositionAsHomePosition()
        {
            diagram.options.homePosition.x = this.shift.x;
            diagram.options.homePosition.y = this.shift.y;
            diagram.options.homeLayer = this.currentLayer.id;
            this.diagram.Unsave();
        }

        // FORM go to end position - center window to second remembered position
        public void GoToEnd()
        {
            Nodes nodes = this.diagram.layers.SearchInAllNodes("@end");
            nodes.OrderByIdAsc();

            Node homenode = null;
            foreach (Node node in nodes)
            {
                if (node.link == "@end")
                {
                    homenode = node;
                }
            }

            if (homenode != null)
            {
                this.GoToNode(homenode);
            }
            else
            {
                this.shift.Set(diagram.options.endPosition);
                this.GoToLayer(diagram.options.endLayer);
            }

            this.diagram.InvalidateDiagram();
        }

        // FORM open view and go to home position
        public void OpenViewAndGoToEnd()
        {
            DiagramView child = this.diagram.OpenDiagramView(this);
            child.GoToEnd();
            child.Invalidate();
        }

        // FORM set end position
        public void SetCurentPositionAsEndPosition()
        {
            diagram.options.endPosition.x = this.shift.x;
            diagram.options.endPosition.y = this.shift.y;
            diagram.options.endLayer = this.currentLayer.id;
            this.diagram.Unsave();
        }

        // FORM cursor position
        public Position CursorPosition()
        {
            Point ptCursor = Cursor.Position;
            ptCursor = PointToClient(ptCursor);
            return new Position(ptCursor.X, ptCursor.Y);
        }

        // FORM hide
        public bool FormHide()
        {
            if (!this.diagram.SavedFile && this.diagram.FileName != "")
            {
                this.diagram.SaveXMLFile(this.diagram.FileName);
                this.diagram.NewFile = false;
                this.diagram.SavedFile = true;
            }
            this.WindowState = FormWindowState.Minimized;
            return true;
        }

        // FORM remember current window position an restore when diagram is opened
        public void RememberPosition(bool state = true)
        {
            this.diagram.options.restoreWindow = state;

            this.diagram.options.Left = this.Left;
            this.diagram.options.Top = this.Top;
            this.diagram.options.Width = this.Width;
            this.diagram.options.Height = this.Height;

            this.diagram.options.WindowState = this.GetWindowsStateCode();
        }

        // FORM get window state role
        public int GetWindowsStateCode()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                return 1;
            }

            if (this.WindowState == FormWindowState.Normal)
            {
                return 2;
            }


            if (this.WindowState == FormWindowState.Minimized)
            {
                return 3;
            }

            return 0;
        }

        // FORM get window state role
        public void SetWindowsStateCode(int code = 0)
        {
            if (this.diagram.options.WindowState == 1)
            {
                this.WindowState = FormWindowState.Maximized;
            }

            if (this.diagram.options.WindowState == 2)
            {
                this.WindowState = FormWindowState.Normal;
            }


            if (this.diagram.options.WindowState == 3)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        // FORM set icon
        public void SetIcon()
        {
            // #icon
            OpenFileDialog openIconDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.bmp, *.ico, *.png, *.gif, *.tiff)| *.jpg; *.jpeg; *.bmp; *.ico; *.png; *.gif; *.tiff",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openIconDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Icon icon = null;

                    if (Os.GetExtension(openIconDialog.FileName).ToLower() == ".ico")
                    {
                        icon = Media.GetIcon(openIconDialog.FileName); 
                    }
                    else
                    {
                        Bitmap image = Media.GetImage(openIconDialog.FileName);
                        IntPtr Hicon = image.GetHicon();
                        icon = Icon.FromHandle(Hicon);
                    }

                    this.Icon = icon;
                    this.diagram.options.icon = Media.IconToString(icon);
                    this.diagram.Unsave();
                }
                catch (Exception e)
                {
                    Program.log.write("DiagramView: setIcon: error:" + e.Message);
                }
            }
            else
            {
                if (this.diagram.options.icon != "" &&  MessageBox.Show(Translations.confirmRemoveIconQuestion, Translations.confirmRemoveIcon, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
#if DEBUG
                    this.Icon = global::Diagram.Properties.Resources.ico_diagram_debug;
#else
                    this.Icon = global::Diagram.Properties.Resources.ico_diagram;
#endif
                    this.diagram.options.icon = "";
                    this.diagram.Unsave();
                }
            }
        }

        // FORM set icon
        public void SetBackgroundImage()
        {
            OpenFileDialog openIconDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.bmp, *.ico, *.png, *.gif, *.tiff)| *.jpg; *.jpeg; *.bmp; *.ico; *.png; *.gif; *.tiff",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openIconDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap background = Media.GetImage(openIconDialog.FileName);
                    this.diagram.options.backgroundImage = background;
                    this.diagram.RefreshBackgroundImages();
                    this.diagram.Unsave();
                }
                catch (Exception e)
                {
                    Program.log.write("DiagramView: setIcon: error:" + e.Message);
                }
            }
            else
            {
                //#background
                if (this.diagram.options.backgroundImage != null && MessageBox.Show(Translations.confirmRemoveBackgroundQuestion, Translations.confirmRemoveBackground, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.diagram.options.backgroundImage = null;
                    this.diagram.RefreshBackgroundImages();
                    this.diagram.Unsave();
                }
            }
        }

        public void RefreshBackgroundImage()
        {
            this.BackgroundImage = this.diagram.options.backgroundImage;
        }

        /*************************************************************************************************************************/

        // SELECTION Clear selection
        public void ClearSelection()
        {
            if (this.selectedNodes.Count() > 0) // odstranenie mulitvyberu
            {
                foreach (Node rec in this.selectedNodes)
                {
                    rec.selected = false;
                }

                this.selectedNodes.Clear();
            }
        }

        // SELECTION Remove Node from  selection
        public void RemoveNodeFromSelection(Node a)
        {
            if (this.selectedNodes.Count() > 0 && a!=null) // odstranenie mulitvyberu
            {
                for (int i = this.selectedNodes.Count() - 1; i >= 0; i--) // Loop through List with foreach
                {
                    if (this.selectedNodes[i] == a)
                    {
                        this.selectedNodes[i].selected = false;
                        this.selectedNodes.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        // SELECTION Clear Selection and Select node
        public void SelectOnlyOneNode(Node rec)
        {
            this.ClearSelection();
            this.SelectNode(rec);
        }

        // SELECTION Select node
        public void SelectNode(Node rec)
        {
            if (rec != null)
            {
                rec.selected = true;
                this.selectedNodes.Add(rec);
            }
        }

        // SELECTION Clear Selection and Select nodes
        public void SelectNodes(Nodes nodes)
        {
            this.ClearSelection();
            foreach (Node rec in nodes)
            {
                if (rec.layer == this.currentLayer.id)
                {
                    this.SelectNode(rec);
                }
            }
            this.diagram.InvalidateDiagram();
        }

        // SELECTION selsect all
        public void SelectAll()
        {
            this.SelectNodes(this.currentLayer.nodes);
        }

        /*************************************************************************************************************************/

        // EVENTS

        // EVENT Paint                                                                                 // [PAINT] [EVENT]
        public void DiagramApp_Paint(object sender, PaintEventArgs e)
        {
            this.DrawDiagram(e.Graphics);
        }

        // EVENT Mouse DoubleClick
        public void DiagramApp_MouseDoubleClick(object sender, MouseEventArgs e)                       // [MOUSE] [DBLCLICK] [EVENT]
        {

#if DEBUG
            this.LogEvent("MouseDouble");
#endif

            this.stateDblclick = true;
        }

        // EVENT Mouse Down                                                                            // [MOUSE] [DOWN] [EVENT]
        public void DiagramApp_MouseDown(object sender, MouseEventArgs e)
        {
#if DEBUG
            this.LogEvent("MouseDown");
#endif

            this.actualMousePos.Set(e.X, e.Y);

            if (this.stateSearching)
            {
                this.stateSearching = false;
                this.searhPanel.HidePanel();
            }

			this.Focus();

            if (this.IsEditing())
            {
                this.ActiveControl = null;
            }

            if (this.editPanel.editing) // close edit panel after mouse click to form
            {
                bool selectNode = false;
                this.editPanel.saveNodeNamePanel(selectNode);
            }
            else
            if (this.editLinkPanel.editing) // close link edit panel after mouse click to form
            {
                bool selectNode = false;
                this.editLinkPanel.saveNodeLinkPanel(selectNode);
            }

            this.startMousePos.Set(this.actualMousePos);  // starting mouse position
            this.startShift.Set(this.shift);  // starting indent

            if (e.Button == MouseButtons.Left)
            {
                this.sourceNode = this.FindNodeInMousePosition(new Position(e.X, e.Y));

                this.stateSourceNodeAlreadySelected = this.sourceNode != null && this.sourceNode.selected;

                if (bottomScrollBar != null && bottomScrollBar.MouseDown(e.X, e.Y))
                {
                    MoveScreenHorizontal(bottomScrollBar.position);
                    this.diagram.InvalidateDiagram();
                    return;
                }
                else
                if (rightScrollBar != null && rightScrollBar.MouseDown(e.X, e.Y))
                {
                    MoveScreenVertical(rightScrollBar.position);
                    this.diagram.InvalidateDiagram();
                    return;
                }
                else
                if (this.sourceNode == null)
                {
                    if (!this.diagram.options.readOnly
                        && ((!this.keyctrl && this.keyalt) || (this.keyctrl && this.keyalt))
                        && !this.keyshift) // add node by drag
                    {
                        this.stateAddingNode = true;
                        MoveTimer.Enabled = true;
                        this.ClearSelection();
                    }
                    else // multiselect
                    {
                        this.stateSelectingNodes = true;
                        MoveTimer.Enabled = true;
                    }
                }
                else if (this.sourceNode != null)
                {
                    if (this.keyshift && !this.keyctrl && !this.keyalt
                        && this.sourceNode.link.Trim() != ""
                        && (Os.FileExists(this.sourceNode.link) || Os.DirectoryExists(this.sourceNode.link))) // drag file from diagram
                    {
                        string[] array = { this.sourceNode.link };
                        var data = new DataObject(DataFormats.FileDrop, array);
                        this.DoDragDrop(data, DragDropEffects.Copy);
                    }
                    else if (!this.diagram.options.readOnly && !this.stateDblclick)  //informations for draging
                    {
                        if (!this.keyshift && this.keyctrl && !this.keyalt)  // start copy item
                        {
                            this.stateCoping = true;

                            this.copySelectedNodes.Copy(this.selectedNodes);
                            foreach (Node node in this.copySelectedNodes)
                            {
                                if (node.id == this.sourceNode.id)
                                {
                                    this.copySourceNode = node;
                                    break;
                                }
                            }
                            this.copySelectedLines.Copy(this.GetSelectedLines());

                            foreach (Line line in this.copySelectedLines)
                            {
                                foreach (Node node in this.copySelectedNodes)
                                {
                                    if (line.end == node.id)
                                    {
                                        line.endNode = node;
                                    }

                                    if (line.start == node.id)
                                    {
                                        line.startNode = node;
                                    }
                                }
                            }
                        }

                        this.stateDragSelection = true;
                        MoveTimer.Enabled = true;
                        this.startNodePos.Set(this.sourceNode.position); // starting position of draging item

                        this.vmouse
                            .Set(this.actualMousePos)
                            .Scale(this.scale)
                            .Subtract(this.shift)
                            .Subtract(this.sourceNode.position); // mouse position in node

                        if (!this.keyctrl && !this.keyshift && !this.sourceNode.selected)
                        {
                            this.SelectOnlyOneNode(this.sourceNode);
                            this.diagram.InvalidateDiagram();
                        }
                    }
                }
            }
            else
            if (e.Button == MouseButtons.Right)
            {
                this.stateMoveView = true; // popupmenu or view move
            }
            else
            if (e.Button == MouseButtons.Middle)
            {
                if (!this.diagram.options.readOnly)
                {
                    this.sourceNode = this.FindNodeInMousePosition(new Position(e.X, e.Y));
                    this.stateAddingNode = true;// add node by drag
                    MoveTimer.Enabled = true;
                }
            }

            this.diagram.InvalidateDiagram();
        }

        // EVENT Mouse move                                                                            // [MOUSE] [MOVE] [EVENT]
        public void DiagramApp_MouseMove(object sender, MouseEventArgs e)
        {

#if DEBUG
            this.LogEvent("MouseMove");
#endif

            if (this.stateSelectingNodes || this.stateAddingNode)
            {
                this.actualMousePos.Set(e.X, e.Y);
                this.diagram.InvalidateDiagram();
            }
            else
            if (this.stateDragSelection || this.stateAddingNode || this.stateSelectingNodes) // posunutie objektu
            {
                this.actualMousePos.Set(e.X, e.Y);
            }
            else
            if (this.stateMoveView) // screen moving
            {
                this.shift.x = (int)(this.startShift.x + (e.X - this.startMousePos.x) * this.scale);
                this.shift.y = (int)(this.startShift.y + (e.Y - this.startMousePos.y) * this.scale);
                this.diagram.InvalidateDiagram();
            }
            else
            if (bottomScrollBar != null && rightScrollBar!= null && (bottomScrollBar.MouseMove(e.X, e.Y) || rightScrollBar.MouseMove(e.X, e.Y))) //SCROLLBARS
            {
                bottomScrollBar.setPosition(GetPositionHorizontal());
                rightScrollBar.setPosition(GetPositionVertical());
                this.diagram.InvalidateDiagram();
            }
        }

        // EVENT Mouse Up                                                                              // [MOUSE] [UP] [EVENT]
        public void DiagramApp_MouseUp(object sender, MouseEventArgs e)
        {

#if DEBUG
            this.LogEvent("MouseUp");
#endif

            this.actualMousePos.Set(e.X, e.Y);
            Position mouseTranslation = new Position(this.actualMousePos).Subtract(this.startMousePos);

            // States
            bool mousemove = ((this.actualMousePos.x != this.startMousePos.x) || (this.actualMousePos.y != this.startMousePos.y)); // mouse change position
            bool buttonleft = e.Button == MouseButtons.Left;
            bool buttonright = e.Button == MouseButtons.Right;
            bool buttonmiddle = e.Button == MouseButtons.Middle;
            bool isreadonly = this.diagram.options.readOnly;
            bool keyalt = this.keyalt;
            bool keyctrl = this.keyctrl;
            bool keyshift = this.keyshift;
            bool dblclick = this.stateDblclick;
            bool finishdraging = this.stateDragSelection;
            bool finishadding = this.stateAddingNode;
            bool finishselecting = mousemove && this.stateSelectingNodes;

            MoveTimer.Enabled = false;

            if(dblclick)
            {
                this.stateSelectingNodes = false;
            }
            else
            // KEY DRAG
            if (finishdraging) // drag node
            {
                if (!this.diagram.options.readOnly)
                {
                    if (this.sourceNode != null && !keyctrl) // return node to starting position after connection is created
                    {
                        Position translation = new Position(this.startNodePos)
                            .Subtract(sourceNode.position);

                        if (this.selectedNodes.Count > 0)
                        {
                            foreach (Node node in this.selectedNodes)
                            {
                                node.position.Add(translation);
                            }
                        }

                        this.diagram.InvalidateDiagram();
                    }
                }
            }
            else
            // KEY DRAG-MMIDDLE
            if (finishadding)
            {
                this.diagram.InvalidateDiagram();
            }
            else
            // KEY DRAG+MLEFT select nodes with selection rectangle
            if (finishselecting)
            {
                if (mousemove)
                {
                    Position a = new Position(this.startMousePos)
                        .Scale(this.scale)
                        .Add(this.shift)
                        .Subtract(this.startShift);

                    Position b = new Position(this.actualMousePos)
                        .Scale(this.scale);

                    int temp;
                    if (b.x < a.x) { temp = a.x; a.x = b.x; b.x = temp; }
                    if (b.y < a.y) { temp = b.y; b.y = a.y; a.y = temp; }

                    if (!this.keyshift) this.ClearSelection();
                    foreach (Node rec in this.currentLayer.nodes)
                    {
                        if (
                            (rec.layer == this.currentLayer.id || rec.id == this.currentLayer.id)
                            && -this.shift.x + a.x <= rec.position.x
                            && rec.position.x + rec.width <= -this.shift.x + b.x
                            && -this.shift.y + a.y <= rec.position.y
                            && rec.position.y + rec.height <= -this.shift.y + b.y) // get all nodes in selection rectangle
                        {
                            if (keyshift && !keyctrl && !keyalt) // KEY SHIFT+MLEFT Invert selection
                            {
                                if (rec.selected)
                                {
                                    this.RemoveNodeFromSelection(rec);
                                }
                                else
                                {
                                    this.SelectNode(rec);
                                }
                            }

                            if (!keyshift && !keyctrl && !keyalt) // KEY MLEFT select nodes
                            {
                                this.SelectNode(rec);
                            }
                        }
                    }

                    this.diagram.InvalidateDiagram();
                }
            }

            Node TargetNode = this.FindNodeInMousePosition(new Position(e.X, e.Y));

            if (buttonleft) // MLEFT
            {

                if (!keyalt && keyctrl && !keyshift && TargetNode != null && TargetNode.selected) // CTRL+CLICK add node to selection
                {
                    this.RemoveNodeFromSelection(TargetNode);
                    this.diagram.InvalidateDiagram();
                }
                else
                if (!keyalt && keyctrl && !keyshift && TargetNode != null && !TargetNode.selected) // CTRL+CLICK remove node from selection
                {
                    this.SelectNode(TargetNode);
                    this.diagram.InvalidateDiagram();
                }
                else
                if (this.stateCoping && mousemove) // CTRL+DRAG copy part of diagram
                {
                    this.stateCoping = false;

                    DiagramBlock newBlock = this.diagram.DuplicatePartOfDiagram(this.selectedNodes, this.currentLayer.id);

                    Position vector = new Position(this.actualMousePos)
                        .Scale(this.scale)
                        .Subtract(this.vmouse)
                        .Subtract(this.shift)
                        .Subtract(this.sourceNode.position);

                    // filter only top nodes fromm all new created nodes. NewNodes containing sublayer nodes.
                    Nodes topNodes = new Nodes();

                    foreach (Node node in newBlock.nodes)
                    {
                        if (node.layer == this.currentLayer.id)
                        {
                            topNodes.Add(node);
                        }
                    }

                    foreach (Node node in topNodes)
                    {
                        node.position.Add(vector);
                    }

                    this.diagram.Unsave("create", newBlock.nodes, newBlock.lines);

                    this.SelectNodes(topNodes);

                    this.diagram.Unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                if (bottomScrollBar != null
                    && rightScrollBar != null
                    && (bottomScrollBar.MouseUp() || rightScrollBar.MouseUp()))
                {
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY MLEFT clear selection
                if (!mousemove
                    && TargetNode == null
                    && this.sourceNode == null
                    && this.selectedNodes.Count() > 0
                    && !keyalt
                    && !keyctrl
                    && !keyshift)
                {
                    this.ClearSelection();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY CTRL+ALT+DRAG create node and conect with existing node
                if (!isreadonly
                    && !keyshift
                    && keyctrl
                    && keyalt
                    && TargetNode == null
                    && this.sourceNode != null)
                {
                    var s = this.sourceNode;
                    var newNodePosition = new Position(e.X, e.Y);
                    var node = this.CreateNode(newNodePosition);
                    node.shortcut = s.id;
                    this.diagram.Connect(s, node);
                    this.diagram.Unsave("create", node, this.shift, this.currentLayer.id);
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY CTRL+ALT+DRAG create shortcut beetwen objects
                if (!isreadonly
                    && !keyshift
                    && keyctrl
                    && keyalt
                    && TargetNode != null
                    && this.sourceNode != null
                    && TargetNode != this.sourceNode)
                {
                    this.diagram.Unsave("edit", this.sourceNode, this.shift, this.currentLayer.id);
                    this.sourceNode.link = "#" + TargetNode.id.ToString();
                    this.diagram.Unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG move node
                if
                (
                    !isreadonly
                    && (
                        (TargetNode == null && this.sourceNode != null)
                        || (
                            TargetNode != null
                            && this.sourceNode != TargetNode
                            && TargetNode.selected
                        )
                        || (TargetNode != null && this.sourceNode == TargetNode)
                    )
                    && Math.Sqrt(mouseTranslation.x * mouseTranslation.x + mouseTranslation.y * mouseTranslation.y) > 5
                )
                {
                    Position vector = new Position(this.actualMousePos)
                        .Scale(this.scale)
                        .Subtract(this.vmouse)
                        .Subtract(this.shift)
                        .Subtract(this.sourceNode.position);

                    if (this.selectedNodes.Count > 0)
                    {
                        this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);

                        foreach (Node node in this.selectedNodes)
                        {
                            node.position.Add(vector);
                        }

                        this.diagram.Unsave();
                    }

                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+CTRL create node and conect with existing node
                if (!isreadonly
                    && !keyshift
                    && !keyctrl
                    && keyalt
                    && TargetNode != null
                    && this.sourceNode == null)
                {
                    Node node = this.CreateNode(
                            new Position(
                                +this.shift.x - startShift.x + this.startMousePos.x,
                                +this.shift.y - startShift.y + this.startMousePos.y
                            )
                        );

                    Line line = this.diagram.Connect(
                        node,
                        TargetNode
                    );

                    this.diagram.Unsave("create", node, line, this.shift, this.currentLayer.id);
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY CTRL+ALT+DRAG create node and make shortcut to target node
                if (!isreadonly
                    && keyalt
                    && keyctrl
                    && !keyshift
                    && TargetNode != null
                    && this.sourceNode == null)
                {
                    Position newNodePosition = new Position(this.shift)
                        .Subtract(startShift)
                        .Add(this.startMousePos);

                    Node newrec = this.CreateNode(
                        newNodePosition
                    );

                    newrec.link = "#" + TargetNode.id;
                    this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DBLCLICK open link or edit window after double click on node [dblclick] [open] [edit]
                if (dblclick
                    && this.sourceNode != null
                    && !keyctrl
                    && !keyalt
                    && !keyshift)
                {
                    this.ResetStates();
                    this.OpenLinkAsync(this.sourceNode);
                }
                else
                // KEY SHIFT+DBLCLICK open node edit form
                if (dblclick
                    && this.sourceNode != null
                    && !keyctrl
                    && !keyalt
                    && keyshift)
                {
                    this.diagram.EditNode(this.sourceNode);
                }
                else
                // KEY CTRL+DBLCLICK open link in node
                if (dblclick
                    && this.sourceNode != null
                    && keyctrl
                    && !keyalt
                    && !keyshift)
                {
                    if (this.sourceNode.link != "")
                    {
                        Os.OpenPathInSystem(this.sourceNode.link);
                    }
                }
                else
                // KEY DBLCLICK+SPACE change position in zoom view mode
                if (this.stateZooming
                    && dblclick
                    && !keyctrl
                    && !keyalt
                    && !keyshift)
                {
                    this.shift
                        .Subtract(
                            this.actualMousePos
                            .Clone()
                            .Scale(this.scale)
                            )
                        .Add(
                            (this.ClientSize.Width * this.scale) / 2,
                            (this.ClientSize.Height * this.scale) / 2
                        );
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY CTRL+SHIFT+MLEFT conect with selected nodes new node or selected node
                if (!isreadonly
                    && keyshift
                    && keyctrl
                    && this.selectedNodes.Count() > 0
                    && e.X == this.startMousePos.x
                    && e.Y == this.startMousePos.y)
                {
                    // TODO Still working this?
                    Node newrec = TargetNode;

                    Nodes newNodes = new Nodes();
                    if (newrec == null)
                    {
                        newrec = this.CreateNode(this.actualMousePos.Clone().Subtract(10), false);
                        newNodes.Add(newrec);
                    }

                    Lines newLines = new Lines();
                    foreach (Node rec in this.selectedNodes)
                    {
                        Line line = this.diagram.Connect(rec, newrec);
                        newLines.Add(line);
                    }

                    this.SelectOnlyOneNode(newrec);
                    this.diagram.Unsave("create", newNodes, newLines, this.shift, this.currentLayer.id);
                }
                else
                // KEY ALT+MLEFT
                // KEY DBLCLICK create new node
                if (!isreadonly
                    && (dblclick || keyalt)
                    && !keyshift
                    && !keyctrl
                    && TargetNode == null
                    && this.sourceNode == null
                    && e.X == this.startMousePos.x
                    && e.Y == this.startMousePos.y)
                {
                    Node newNode = this.CreateNode(this.actualMousePos.Clone().Subtract(10), false);
                    this.diagram.Unsave("create", newNode, this.shift, this.currentLayer.id);
                }
                else
                // KEY DRAG+ALT copy style from node to other node
                if (!isreadonly
                    && !keyshift
                    && !keyctrl
                    && keyalt
                    && TargetNode != null
                    && this.sourceNode != null
                    && this.sourceNode != TargetNode)
                {
                    if (this.selectedNodes.Count() > 1)
                    {
                        this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                        foreach (Node rec in this.selectedNodes)
                        {
                            rec.copyNodeStyle(TargetNode);
                        }
                        this.diagram.Unsave();
                    }

                    if (this.selectedNodes.Count() == 1
                        || (this.selectedNodes.Count() == 0 && this.sourceNode != null))
                    {
                        this.diagram.undoOperations.add("edit", TargetNode, this.shift, this.currentLayer.id);

                        TargetNode.copyNodeStyle(this.sourceNode);

                        if (this.selectedNodes.Count() == 1 && this.selectedNodes[0] != this.sourceNode)
                        {
                            this.ClearSelection();
                            this.SelectNode(this.sourceNode);
                        }
                        this.diagram.Unsave();
                    }
                }
                else
                // KEY DRAG make link between two nodes
                if (!isreadonly
                    && !keyctrl
                    && !keyalt
                    && TargetNode != null
                    && this.sourceNode != null
                    && this.sourceNode != TargetNode)
                {
                    bool arrow = false;
                    if (keyshift)
                    {
                        arrow = true;
                    }

                    Lines newLines = new Lines();
                    Lines removeLines = new Lines();
                    if (this.selectedNodes.Count() > 0)
                    {
                        foreach (Node rec in this.selectedNodes)
                        {
                            if (rec != TargetNode)
                            {
                                if (this.diagram.HasConnection(rec, TargetNode))
                                {
                                    Line removeLine = this.diagram.GetLine(rec, TargetNode);
                                    removeLines.Add(removeLine);
                                    this.diagram.Disconnect(rec, TargetNode);

                                }
                                else
                                {
                                    Line newLine = this.diagram.Connect(rec, TargetNode, arrow, null);
                                    newLines.Add(newLine);
                                }
                            }
                        }
                    }

                    if (newLines.Count() > 0 && removeLines.Count() > 0)
                    {
                        this.diagram.undoOperations.startGroup();
                        this.diagram.undoOperations.add("create", null, newLines, this.shift, this.currentLayer.id);
                        this.diagram.undoOperations.add("delete", null, removeLines, this.shift, this.currentLayer.id);
                        this.diagram.undoOperations.endGroup();
                        this.diagram.Unsave();
                    }
                    else if (newLines.Count() > 0)
                    {
                        this.diagram.undoOperations.add("create", null, newLines, this.shift, this.currentLayer.id);
                    }
                    else if (removeLines.Count() > 0)
                    {
                        this.diagram.undoOperations.add("delete", null, removeLines, this.shift, this.currentLayer.id);
                    }


                    this.diagram.InvalidateDiagram();
                }
                // KEY SHIFT+MLEFT add node to selected nodes
                else
                if (!keyctrl
                    && keyshift
                    && !keyalt
                    && this.sourceNode == TargetNode
                    && TargetNode != null
                    && !TargetNode.selected)
                {
                    this.SelectNode(TargetNode);
                    this.diagram.InvalidateDiagram();
                }
                // KEY SHIFT+MLEFT remove node from selected nodes
                else
                if (!keyctrl
                    && keyshift
                    && !keyalt
                    && TargetNode != null
                    && (this.sourceNode == TargetNode || TargetNode.selected))
                {
                    this.RemoveNodeFromSelection(TargetNode);
                    this.diagram.InvalidateDiagram();
                }
                else
                if (this.sourceNode == TargetNode
                    && this.stateSourceNodeAlreadySelected)
                {
                    this.Rename();
                }

            }
            else
            if (buttonright) // KEY MRIGHT
            {
                this.stateMoveView = false; // show popup menu
                if (e.X == this.startMousePos.x
                    && e.Y == this.startMousePos.y
                    && this.startShift.x == this.shift.x
                    && this.startShift.y == this.shift.y)
                {
                    Node temp = this.FindNodeInMousePosition(new Position(e.X, e.Y));

                    if (temp == null || (this.sourceNode != temp && !temp.selected))
                    {
                        this.ClearSelection();
                        this.SelectOnlyOneNode(temp);
                    }

                    this.diagram.InvalidateDiagram();
                    PopupMenu.Show(this.Left + e.X, this.Top + e.Y); // [POPUP] show popup
                }
                else { // KEY DRAG+MRIGHT move view
                    this.shift.x = (int)(this.startShift.x + (e.X - this.startMousePos.x) * this.scale);
                    this.shift.y = (int)(this.startShift.y + (e.Y - this.startMousePos.y) * this.scale);
                    this.diagram.InvalidateDiagram();
                }
            }
            else
            if (buttonmiddle) // MMIDDLE
            {
                // KEY DRAG+MMIDDLE conect two existing nodes
                if (this.sourceNode != null && TargetNode != null) {

                    Line newLine = this.diagram.Connect(
                        sourceNode,
                        TargetNode
                    );

                    if (newLine != null) {
                        this.diagram.Unsave("create", newLine, this.shift, this.currentLayer.id);
                        this.diagram.InvalidateDiagram();
                    }
                }
                else
                // KEY DRAG+MMIDDLE connect exixting node with new node
                if (this.sourceNode != null && TargetNode == null)
                {

                    Node newNode = this.CreateNode(this.actualMousePos.Clone().Subtract(10));

                    Line newLine = this.diagram.Connect(
                        sourceNode,
                        newNode
                    );

                    this.diagram.Unsave("create", newNode, newLine, this.shift, this.currentLayer.id);
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+MMIDDLE create new node and conect id with existing node
                if (!isreadonly && TargetNode != null)
                {
                    Node newNode = this.CreateNode(
                        (new Position(this.shift)).Subtract(this.startShift).Add(this.startMousePos)
                    );

                    Line newLine = this.diagram.Connect(
                        newNode,
                        TargetNode
                    );

                    this.diagram.Unsave("create", newNode, newLine, this.shift, this.currentLayer.id);
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+MMIDDLE create new node and conect with new node (create line)
                if (!isreadonly && TargetNode == null)
                {
                    Nodes nodes = new Nodes();
                    Lines lines = new Lines();

                    Node node1 = this.CreateNode(
                            (new Position(this.shift)).Subtract(this.startShift).Add(this.startMousePos),
                            false
                        );

                    nodes.Add(node1);

                    Node node2 = this.CreateNode(
                        this.actualMousePos.Clone().Subtract(10)
                    );

                    nodes.Add(node2);

                    lines.Add(this.diagram.Connect(
                        node1,
                        node2
                    ));

                    this.diagram.Unsave("create", nodes, lines, this.shift, this.currentLayer.id);
                    this.diagram.InvalidateDiagram();
                }
            }

            this.ResetStates();
        }

        // EVENT revert states to default
        private void ResetStates()
        {
            this.MoveTimer.Enabled = false;
            this.stateDragSelection = false;
            this.stateMoveView = false;
            this.stateSelectingNodes = false;
            this.stateAddingNode = false;
            this.stateDblclick = false;
            //this.stateZooming = false;
            this.stateSearching = false;
            this.stateSourceNodeAlreadySelected = false;
            this.stateCoping = false;
        }

        // EVENT Mouse Whell
        public void DiagramApp_MouseWheel(object sender, MouseEventArgs e)                             // [MOUSE] [WHELL] [EVENT]
        {
#if DEBUG
            this.LogEvent("MouseWheel");
#endif
            float newScale = 0;
            //throw new NotImplementedException();
            if (e.Delta > 0) // MWHELL
            {
                if (this.keyctrl) // zoom
                {
                    if (this.scale > 0) // down
                    {
                        int a = (int)((this.shift.x - (this.ClientSize.Width / 2 * this.scale)));
                        int b = (int)((this.shift.y - (this.ClientSize.Height / 2 * this.scale)));
                        if (this.scale > 1)
                            newScale = this.scale - 1;
                        else
                        if (this.scale > 0.1f)
                            newScale = this.scale - 0.1f;

                        this.shift.x = (int)((a + (this.ClientSize.Width / 2 * this.scale)));
                        this.shift.y = (int)((b + (this.ClientSize.Height / 2 * this.scale)));
                    }

                    if (newScale < 0.1f) newScale = 0.1f;
                }
                else
                if (this.keyshift) // move view
                {
                    this.shift.x += (int)(50 * this.scale);
                    this.diagram.InvalidateDiagram();
                }
                else // move view
                {
                    this.shift.y += (int)(50 * this.scale);
                    this.diagram.InvalidateDiagram();
                }
            }
            else
            {
                if (this.keyctrl) // zoom
                {
                    if (this.scale < 7) //up
                    {
                        int a = (int)((this.shift.x - (this.ClientSize.Width / 2 * this.scale)));
                        int b = (int)((this.shift.y - (this.ClientSize.Height / 2 * this.scale)));
                        if (this.scale >= 1)
                            newScale = this.scale + 1;
                        else
                            if (this.scale < 1)
                            newScale = this.scale + 0.1f;

                        this.shift.x = (int)((a + (this.ClientSize.Width / 2 * this.scale)));
                        this.shift.y = (int)((b + (this.ClientSize.Height / 2 * this.scale)));
                    }

                    if (newScale > 7) newScale = 7;
                }
                else
                if (this.keyshift) // move view
                {
                    this.shift.x -= (int)(50 * this.scale);
                    this.diagram.InvalidateDiagram();
                }
                else // move view
                {
                    this.shift.y -= (int)(50 * this.scale);
                    this.diagram.InvalidateDiagram();
                }
            }

            if (newScale != 0 && newScale != this.scale)
            {
                this.zoomTimerStep = Math.Abs((newScale - this.scale) / 30);
                if (this.zoomTimerStep <= 0) {
                    this.zoomTimerStep = 0.001f;
                }

                this.zoomTimerScale = newScale;
                this.zoomTimer.Enabled = true;
            }
        }

        // EVENT Shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)                           // [KEYBOARD] [EVENT]
        {

#if DEBUG
            this.LogEvent("ProcessCmdKey");
#endif

            if (this.IsEditing() || this.stateSearching)
            {
                return false;
            }

            /*
             * [doc] order : ProcessCmdKey, DiagramApp_KeyDown, DiagramApp_KeyPress, DiagramApp_KeyUp;
             */

            if (KeyMap.parseKey(KeyMap.selectAllElements, keyData) ) // [KEY] [CTRL+A] select all
            {
                this.SelectAll();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignToLine, keyData)) // [KEY] [CTRL+L] align to line
            {
                this.AlignToLine();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignToColumn, keyData)) // [KEY] [CTRL+H] align to column
            {
                this.AlignToColumn();
                return true;
            }


            if (KeyMap.parseKey(KeyMap.alignToGroup, keyData)) // [KEY] [CTRL+K] align to group
            {
                this.AlignToGroup();

                return true;
            }

            if (KeyMap.parseKey(KeyMap.markNodes, keyData)) // [KEY] [CTRL+M] mark node for navigation history
            {
                this.SwitchMarkForSelectedNodes();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.prevMarkNode, keyData)) // [KEY] [ALT+LEFT] find prev marked node
            {
                this.PrevMarkedNode();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.nextMarkNode, keyData)) // [KEY] [ALT+RIGHT] find next marked node
            {
                this.NextMarkedNode();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignToLineGroup, keyData)) // [KEY] [CTRL+SHIFT+K] align to group
            {
                this.AlignToLineGroup();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.copy, keyData))  // [KEY] [CTRL+C]
            {
                this.Copy();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.copyLinks, keyData))  // [KEY] [CTRL+SHIFT+C] copy links from selected nodes
            {
                this.CopyLink();
                return true;
            }

			if (KeyMap.parseKey(KeyMap.copyNotes, keyData))  // [KEY] [CTRL+ALT+SHIFT+C] copy notes from selected nodes
			{
                this.CopyNote();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.cut, keyData))  // [KEY] [CTRL+X] Cut diagram
            {
                this.Cut();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.paste, keyData))  // [KEY] [CTRL+V] [PASTE] Paste text from clipboard
            {
                Point ptCursor = Cursor.Position;
                ptCursor = PointToClient(ptCursor);
                this.Paste(new Position(ptCursor.X, ptCursor.Y));
                return true;
            }

            if (KeyMap.parseKey(KeyMap.pasteToNote, keyData))  // [KEY] [CTRL+SHIFT+V] paste to note
            {
                this.PasteToNote();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.pasteToLink, keyData))  // [KEY] [CTRL+SHIFT+INS] paste to node link
            {
                this.PasteToLink();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.undo, keyData))  // [KEY] [CTRL+Z]
            {
                this.diagram.DoUndo(this);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.redo, keyData))  // [KEY] [CTRL+SHIFT+Z]
            {
                this.diagram.DoRedo(this);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.newDiagram, keyData))  // [KEY] [CTRL+N] New Diagram
            {
                main.OpenDiagram();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.newDiagramView, keyData))  // [KEY] [F7] New Diagram view
            {
                this.diagram.OpenDiagramView(this);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.switchViews, keyData))  // [KEY] [F8] hide views
            {
                this.main.SwitchViews(this);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.save, keyData))  // [KEY] [CTRL+S] save diagram
            {
                this.Save();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.open, keyData))  // [KEY] [CTRL+O] open diagram dialog window
            {
                this.Open();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.search, keyData))  // [KEY] [CTRL+F] Search form
            {
                this.ShowSearchPanel();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.evaluateExpression, keyData))  // [KEY] [CTRL+G] Evaluate expresion or generate random value
            {
                if (this.selectedNodes.Count() == 0)
                {
                    this.Random();
                }
                else
                {
                    this.EvaluateExpression();
                }
            }

            if (KeyMap.parseKey(KeyMap.date, keyData))  // [KEY] [CTRL+D] date
            {
                this.EvaluateDate();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.promote, keyData)) // [KEY] [CTRL+P] Promote node
            {
                this.Promote();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.refresh, keyData)) // [KEY] [CTRL+R] Refresh
            {
                if (this.selectedNodes.Count > 0)
                {
                    this.diagram.RefreshNodes(this.selectedNodes);
                }
                else
                {
                    this.diagram.RefreshAll();
                }

                return true;
            }

            if (KeyMap.parseKey(KeyMap.hideBackground, keyData)) // [KEY] [F6] Hide background
            {
                this.HideBackground();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.searchNext, keyData)) // [KEY] [F3] reverse search
            {
                if (this.searhPanel != null) {
                    this.searhPanel.searchNext();
                }
                return true;
            }

            if (KeyMap.parseKey(KeyMap.reverseSearch, keyData)) // [KEY] [SHIFT+F3] reverse search
            {
                this.SearchPrev();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.home, keyData)) // KEY [HOME] go to home position
            {
                this.GoToHome();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.openViewHome, keyData)) // KEY [CTRL+HOME] open view and go to home position
            {
                this.OpenViewAndGoToHome();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.setHome, keyData))  // [KEY] [SHIFT+HOME] Move start point
            {
                this.SetCurentPositionAsHomePosition();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.end, keyData)) // KEY [END] go to end position
            {
                this.GoToEnd();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.setEnd, keyData))  // [KEY] [SHIFT+END] Move end point
            {
                this.SetCurentPositionAsEndPosition();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.openViewEnd, keyData)) // KEY [CTRL+END] open view and go to home position
            {
                this.OpenViewAndGoToEnd();
                return true;
            }

            /*
             [DOCUMENTATION]
             Shortcut F5
            -otvorenie adresara vybranej nody
            -prejdu sa vybrane nody a ak je to adresar alebo subor otvori sa adresar
            -ak nie su vybrane ziadne nody otvori sa adresar diagrammu
            */
            if (KeyMap.parseKey(KeyMap.openDrectory, keyData)) // KEY [F5] Open link directory or diagram directory
            {
                OpenLinkDirectory();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.console, keyData)) // [KEY] [F12] show Debug console
            {
                this.main.ShowConsole();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveNodeUp, keyData)) // KEY [CTRL+PAGEUP] move node up to foreground
            {
                this.MoveNodesToForeground();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveNodeDown, keyData)) // [KEY] [CTRL+PAGEDOWN] move node down to background
            {
                this.MoveNodesToBackground();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.pageUp, keyData)) // [KEY] [PAGEUP] change current position in diagram view
            {
                this.PageUp();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.pageDown, keyData)) // [KEY] [PAGEDOWN] change current position in diagram view
            {
                this.PageDown();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.editNodeName, keyData)) // [KEY] [F2] edit node name
            {
                this.Rename();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.editNodeLink, keyData)) // [KEY] [F4] edit node name
            {
                this.EditLink();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.evaluateNodes, keyData)) // [KEY] [F9] evaluate python script for selected nodes by stamp in link
            {
                this.Evaluate();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.fullScreean, keyData)) // [KEY] [F11] evaluate python script for selected nodes by stamp in link
            {
                this.FullScreenSwitch();
                return true;
            }


            if (KeyMap.parseKey(KeyMap.openEditForm, keyData)) // [KEY] [CTRL+E] open edit form
            {
                this.Edit();
                return true;
            }

            // prevent catch keys while node is creating or renaming
            if (this.IsEditing())
            {
                return false;
            }

            if (KeyMap.parseKey(KeyMap.editOrLayerIn, keyData)) // [KEY] [ENTER] open edit form or layer in
            {
                this.LayerInOrEdit();
                return true;
            }


            if (KeyMap.parseKey(KeyMap.layerIn, keyData)) // [KEY] [PLUS] Layer in
            {
                this.LayerIn();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.layerOut, keyData) || KeyMap.parseKey(KeyMap.layerOut2, keyData)) // [KEY] [BACK] or [MINUS] Layer out
            {
                this.LayerOut();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.minimalize, keyData)) // [KEY] [ESC] minimalize diagram view
            {
                if (this.animationTimer.Enabled)
                {
                    this.animationTimer.Enabled = false; // stop move animation if exist
                }
                else
                if (this.isFullScreen)
                {
                    this.FullScreenSwitch();
                }
                else
                {
                    return this.FormHide();
                }
            }

            if (KeyMap.parseKey(KeyMap.delete, keyData)) // [KEY] [DEL] [DELETE] delete
            {
                this.DeleteSelectedNodes(this);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveLeft, keyData) || KeyMap.parseKey(KeyMap.moveLeftFast, keyData))  // [KEY] [left] [SHIFT+LEFT] [ARROW] Move node
            {

                this.MoveNodesToLeft(KeyMap.parseKey(KeyMap.moveLeftFast, keyData));
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveRight, keyData) || KeyMap.parseKey(KeyMap.moveRightFast, keyData))  // [KEY] [right] [SHIFT+RIGHT] [ARROW] Move node
            {
                this.MoveNodesToRight(KeyMap.parseKey(KeyMap.moveRightFast, keyData));
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveUp, keyData) || KeyMap.parseKey(KeyMap.moveUpFast, keyData))  // [KEY] [up] [SHIFT+UP] [ARROW] Move node
            {
                this.MoveNodesUp(KeyMap.parseKey(KeyMap.moveUpFast, keyData));
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveDown, keyData) || KeyMap.parseKey(KeyMap.moveDownFast, keyData))  // [KEY] [down] [SHIFT+DOWN] [ARROW] Move node
            {
                this.MoveNodesDown(KeyMap.parseKey(KeyMap.moveDownFast, keyData));
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignLeft, keyData)) // [KEY] [TAB] align selected nodes to left
            {
                this.AlignLeft();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignRight, keyData))  // [KEY] [SHIFT+TAB] align selected nodes to right
            {
                this.AlignRight();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.resetZoom, keyData))  // [KEY] [CTRL+0] reset zoom level to default
            {
                this.ResetZoom();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.switchSecurityLock, keyData)) // [KEY] [CTRL+ALT+L] lock encrypted diagram
            {
                if (this.diagram.IsEncrypted())
                {
                    if (this.diagram.IsLocked())
                    {
                        this.diagram.UnlockDiagram();
                    }
                    else
                    {
#if !MONO
                        this.main.LockDiagrams();
#endif
                        this.diagram.UnlockDiagram();

                    }
                }
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // EVENT Key down
        public void DiagramApp_KeyDown(object sender, KeyEventArgs e)                                  // [KEYBOARD] [DOWN] [EVENT]
        {

#if DEBUG
            this.LogEvent("KeyDown");
#endif

            if (this.IsEditing() || this.stateSearching)
            {
                return;
            }

            if (e.Shift)
            {
                this.keyshift = true;
            }

            if (e.Control)
            {
                this.keyctrl = true;
            }

            if (e.Alt)
            {
                this.keyalt = true;
            }

            if (this.IsEditing())
            {
                return;
            }

            if (e.KeyCode == Keys.Space && !this.stateZooming) // KEY [SPACE] [SPACEBAR] [zoom] zoom preview
            {
                this.stateSelectingNodes = false;
                this.MoveTimer.Enabled = false;
                this.zoomTimer.Enabled = false;

                this.stateZooming = true;
                Position tmp = new Position(this.shift);

                tmp.Add(
                    (int)(-(this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / this.scale) * this.scale),
                    (int)(-(this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / this.scale) * this.scale)
                );

                this.currentScale = this.scale;
                this.scale = this.zoomingScale;

                tmp.Add(
                    (int)(+(this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / this.scale) * this.scale),
                    (int)(+(this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / this.scale) * this.scale)
                );

                this.shift.Set(tmp);

                this.diagram.InvalidateDiagram();
            }

        }

        // EVENT Key up
        public void DiagramApp_KeyUp(object sender, KeyEventArgs e)
        {

#if DEBUG
            this.LogEvent("KeyUp");
#endif

            this.keyshift = false;
            this.keyctrl = false;
            this.keyalt = false;

            if (this.IsEditing() || this.stateSearching)
            {
                return;
            }

            if (this.stateZooming)
            {
                MoveTimer.Enabled = false;  // zrusenie prebiehajucich operácii
                this.stateMoveView = false;
                this.stateAddingNode = false;
                this.stateDragSelection = false;
                this.stateSelectingNodes = false;

                this.stateZooming = false; // KEY SPACE cancel space zoom and restore prev zoom

                shift.Add(
                    (int)(-(this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / this.scale) * this.scale),
                    (int)(-(this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / this.scale) * this.scale)
                );

                this.scale = this.currentScale;

                shift.Add(
                    (int)(+(this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / this.scale) * this.scale),
                    (int)(+(this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / this.scale) * this.scale)
                );

                this.diagram.InvalidateDiagram();
            }
        }                                 // [KEYBOARD] [UP] [EVENT]

        // EVENT Keypress
        public void DiagramApp_KeyPress(object sender, KeyPressEventArgs e)
        {

#if DEBUG
            this.LogEvent("KeyPress");
#endif

            if (this.IsEditing() || this.stateSearching)
            {
                return;
            }

            this.key = e.KeyChar;

            if (!this.keyctrl && !this.keyalt)
            {

                if (this.key == '+') // KEY PLUS In to layer
                {
                    if (this.selectedNodes.Count() == 1 && this.selectedNodes[0].haslayer)
                    {
                        this.LayerIn(this.selectedNodes[0]);
                    }
                }
                else
                if (this.key == '-') // KEY MINUS Out to layer
                {
                    this.LayerOut();
                }
                else
                if (this.key != ' '
                    && this.key != '\t'
                    && this.key != '\r'
                    && this.key != '\n'
                    && this.key != '`'
                    && this.key != (char)27) // KEY OTHER Pisanie textu - Vytvorenie novej nody
                {
                    this.editPanel.showEditPanel(this.CursorPosition(), this.key);
                }
            }
        }                         // [KEYBOARD] [PRESS] [EVENT]

        // EVENT DROP file
        public void DiagramApp_DragDrop(object sender, DragEventArgs e)                                // [DROP] [DROP-FILE] [EVENT]
        {

#if DEBUG
            this.LogEvent("DragDrop");
#endif

            try
            {
                Nodes newNodes = new Nodes();

				string[] formats = e.Data.GetFormats();
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    Node newrec = this.CreateNode(this.GetMousePosition());
                    newNodes.Add(newrec);
                    newrec.setName(Os.GetFileName(file));

    				newrec.link = file;
    				if (Os.DirectoryExists(file)) // directory
                    {
    					newrec.link = Os.MakeRelative(file, this.diagram.FileName);
                        newrec.color.Set(Media.GetColor(diagram.options.colorDirectory));
                    }
    				else
    				if (Os.Exists(file))
                    {
                        newrec.color.Set(Media.GetColor(diagram.options.colorFile));

                        if (this.diagram.FileName != "" && Os.FileExists(this.diagram.FileName)) // DROP FILE - skratenie cesty k suboru
                        {
                            newrec.link = Os.MakeRelative(file, this.diagram.FileName);
                        }

                        string ext = "";
                        ext = Os.GetExtension(file).ToLower();

                        if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp") // DROP IMAGE skratenie cesty k suboru
                        {
                            newrec.isimage = true;
                            newrec.imagepath = Os.MakeRelative(file, this.diagram.FileName);
                            newrec.image = Media.GetImage(file);
                            if (ext != ".ico") newrec.image.MakeTransparent(Color.White);
                            newrec.height = newrec.image.Height;
                            newrec.width = newrec.image.Width;
                        }
#if !MONO
                        else
                        if (ext == ".lnk") // [LINK] [DROP] extract target
                        {
                            try
                            {
                                string[] shortcutInfo = Os.GetShortcutTargetFile(file);

                                Bitmap icoLnk = Media.ExtractLnkIcon(file);
                                if (icoLnk != null)// extract icon
                                {
                                    newrec.isimage = true;
                                    newrec.embeddedimage = true;
                                    newrec.image = icoLnk;
                                    newrec.image.MakeTransparent(Color.White);
                                    newrec.height = newrec.image.Height;
                                    newrec.width = newrec.image.Width;
                                }
                                else if (shortcutInfo[0] != "" && Os.FileExists(shortcutInfo[0]))
                                {
                                    Bitmap icoExe = Media.ExtractSystemIcon(shortcutInfo[0]);

                                    if (icoExe != null)// extract icon
                                    {
                                        newrec.isimage = true;
                                        newrec.embeddedimage = true;
                                        newrec.image = icoExe;
                                        newrec.image.MakeTransparent(Color.White);
                                        newrec.height = newrec.image.Height;
                                        newrec.width = newrec.image.Width;
                                    }
                                }

                                newrec.link = Os.MakeRelative(file, this.diagram.FileName);
                            }
                            catch (Exception ex)
                            {
                                Program.log.write("extract icon from lnk error: " + ex.Message);
                            }
                        }
                        else
                        {
                            Bitmap ico = Media.ExtractSystemIcon(file);
                            if (ico != null)// extract icon
                            {
                                newrec.isimage = true;
                                newrec.embeddedimage = true;
                                newrec.image = ico;
                                newrec.image.MakeTransparent(Color.White);
                                newrec.height = newrec.image.Height;
                                newrec.width = newrec.image.Width;
                            }
                        }
#endif
                    }
                }

                this.diagram.Unsave("create", newNodes, null, this.shift, this.currentLayer.id);

            } catch (Exception ex) {
                Program.log.write("drop file goes wrong: error: " + ex.Message);
            }
        }

        // EVENT DROP drag enter
        public void DiagramApp_DragEnter(object sender, DragEventArgs e)                               // [DRAG] [EVENT]
        {

#if DEBUG
            this.LogEvent("DragEnter");
#endif

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        // EVENT Resize
        public void DiagramApp_Resize(object sender, EventArgs e)                                      // [RESIZE] [EVENT]
        {

#if DEBUG
            this.LogEvent("Resize");
#endif

            if (this.stateZooming)
            {
                this.stateZooming = false;
                this.scale = this.zoomingDefaultScale;
                this.diagram.InvalidateDiagram();
            }

            // scrollbar - obnova po zmene šírky obrazovky
            if (bottomScrollBar != null && rightScrollBar != null)
            {
                bottomScrollBar.Resize(this.ClientRectangle.Width, this.ClientRectangle.Height);
                rightScrollBar.Resize(this.ClientRectangle.Width, this.ClientRectangle.Height);
            }

			if (this.diagram != null)
			{
				this.diagram.InvalidateDiagram ();
			}
        }

        // EVENT MOVE TIMER for move view when node is draged to window edge
        public void MoveTimer_Tick(object sender, EventArgs e)
        {

#if DEBUG
            this.LogEvent("MoveTimer_Tick");
#endif

            if (this.stateDragSelection || this.stateSelectingNodes || this.stateAddingNode)
            {
                bool changed = false;

                if (this.ClientSize.Width - 20 < this.actualMousePos.x)
                {
                    this.shift.x -= (int)(10 * this.scale);
                    changed = true;
                }

                if (this.actualMousePos.x < 20)
                {
                    this.shift.x += (int)(10 * this.scale);
                    changed = true;
                }

                if (this.ClientSize.Height - 50 < this.actualMousePos.y)
                {
                    this.shift.y -= (int)(10 * this.scale);
                    changed = true;
                }

                if (this.actualMousePos.y < 10)
                {
                    this.shift.y += (int)(10 * this.scale);
                    changed = true;
                }

                if (this.stateDragSelection) // drag selected  nodes
                {
                    if (this.sourceNode != null && this.sourceNode.selected)
                    {
                        Position vector = new Position();

                        if (this.stateCoping) {
                            // calculate shift between start node position and current sourceNode position
                            vector
                                .Set(this.actualMousePos)
                                .Scale(this.scale)
                                .Subtract(this.vmouse)
                                .Subtract(this.shift)
                                .Subtract(this.copySourceNode.position);

                            foreach (Node node in this.copySelectedNodes)
                            {
                                node.position.Add(vector);
                            }
                        }
                        else
                        if (this.selectedNodes.Count > 0)
                        {
                            // calculate shift between start node position and current sourceNode position
                            vector
                                .Set(this.actualMousePos)
                                .Scale(this.scale)
                                .Subtract(this.vmouse)
                                .Subtract(this.shift)
                                .Subtract(this.sourceNode.position);

                            foreach (Node node in this.selectedNodes)
                            {
                                node.position.Add(vector);
                            }
                        }

                        changed = true;
                    }
                }

                if (changed) this.diagram.InvalidateDiagram();
            }
        }                                      // [MOVE] [TIMER] [EVENT]

        // EVENT Deactivate - lost focus
        public void DiagramApp_Deactivate(object sender, EventArgs e)                                  // [FOCUS]
        {

#if DEBUG
            this.LogEvent("Deactivate");
#endif

            this.keyctrl = false;
            this.keyalt = false;
            this.keyshift = false;
            if (this.stateZooming)
            {
                this.stateZooming = false;
                this.scale = this.zoomingDefaultScale;
            }
            this.stateDragSelection = false;
            this.stateAddingNode = false;
            this.stateSelectingNodes = false;
            this.stateMoveView = false;

            this.diagram.InvalidateDiagram();
        }

        /*************************************************************************************************************************/

        // LAYER IN                                                                                    // [LAYER]
        public void LayerIn(Node node)
        {
            if (this.currentLayer.parentNode == null)
            {
                this.firstLayereShift.Set(shift);
            }
            else
            {
                this.currentLayer.parentNode.layerShift.Set(this.shift);
            }

            this.currentLayer = this.diagram.layers.CreateLayer(node);
            this.currentLayer.parentNode.haslayer = true;
            this.layersHistory.Add(this.currentLayer);
            this.shift.Set(this.currentLayer.parentNode.layerShift);

            this.SetTitle();
            this.breadcrumbs.Update();
            this.diagram.InvalidateDiagram();
        }

        // LAYER OUT
        public void LayerOut()
        {
            if (this.currentLayer.parentLayer != null) { //this layer is not top layer

                this.currentLayer.parentNode.layerShift.Set(this.shift);

                if (this.currentLayer.nodes.Count() == 0) {
                    this.currentLayer.parentNode.haslayer = false;
                }

                this.currentLayer = this.currentLayer.parentLayer;

                layersHistory.RemoveAt(layersHistory.Count() - 1);

                if (this.currentLayer.parentNode == null)
                {
                    this.shift.Set(this.firstLayereShift);
                }
                else
                {
                    this.shift.Set(this.currentLayer.parentNode.layerShift);
                }

                this.SetTitle();
                this.breadcrumbs.Update();
                this.diagram.InvalidateDiagram();
            }
        }

        // LAYER HISTORY Buld laier history from
        public void BuildLayerHistory(int id)
        {
            layersHistory.Clear();

            this.currentLayer = this.diagram.layers.GetLayer(id);

            Layer temp = this.currentLayer;
            while (temp != null)
            {
                layersHistory.Add(temp);
                temp = temp.parentLayer;
            }

            layersHistory.Reverse(0, layersHistory.Count());

            this.breadcrumbs.Update();
        }

        // LAYER check if node is parent trought layer history
        public bool IsNodeInLayerHistory(Node rec)
        {
            foreach (Layer layer in this.layersHistory)
            {
                if (layer.id == rec.id) {
                    return true;
                }
            }

            return false;
        }

        // LAYER layer in
        public void LayerIn()
        {
            if (this.selectedNodes.Count() == 1)
            {
                this.LayerIn(this.selectedNodes[0]);
            }
        }

        // LAYER layer in or open edit form
        public void LayerInOrEdit()
        {
            if (this.selectedNodes.Count() == 1)
            {
                if (this.selectedNodes[0].haslayer)
                {
                    this.LayerIn(this.selectedNodes[0]);
                }
                else
                {
                    TextForm textform = this.diagram.EditNode(this.selectedNodes[0]);
                }
            }
        }

        /*************************************************************************************************************************/

        // SEARCH                                                                                          // SEARCH
        public void Search(string find)
        {
            if (find.Trim() != "")
            {
                this.ClearSelection();
                searchFor = find.Trim();
            }
        }

        /// <summary>
        ///  SEARCH FIRST
        ///
        /// Build array of search results and then select first Node.
        ///
        /// </summary>
        /// <param name="find">Search string</param>
        public void SearchFirst(string find)
        {

            Nodes foundNodes = new Nodes();

            this.lastFound = -1;
            this.searchFor = find;

            foreach (Node node in this.diagram.GetAllNodes())
            {
                if (node.note.ToUpper().IndexOf(searchFor.ToUpper()) != -1
                    || node.name.ToUpper().IndexOf(searchFor.ToUpper()) != -1)
                {
                    foundNodes.Add(node);
                }
            }

            this.searhPanel.highlight(foundNodes.Count() == 0);

            Position middle = new Position();
            middle.Copy(this.currentPosition);

            middle.x = middle.x - this.ClientSize.Width / 2;
            middle.y = middle.y - this.ClientSize.Height / 2;

            int currentLayerId = this.currentLayer.id;

            foundNodes.Sort((first, second) =>
            {
                // sort by layers
                if (first.layer < second.layer)
                {
                    // current layer first
                    if (currentLayerId == second.layer) {
                        return 1;
                    }

                    return -1;
                }

                // sort by layers
                if (first.layer > second.layer)
                {
                    // current layer first
                    if (currentLayerId == first.layer)
                    {
                        return -1;
                    }

                    return 1;
                }

                Node parent = this.diagram.layers.GetLayer(first.layer).parentNode;
                Position m = (currentLayerId == first.layer) ? middle : (parent != null) ? parent.layerShift : firstLayereShift;
                double d1 = first.position.ConvertToStandard().Distance(m);
                double d2 = second.position.ConvertToStandard().Distance(m);

                // sort by distance if is same layer
                if (d1 < d2)
                {
                    return -1;
                } else {
                    if (d1 > d2) {
                        return 1;
                    } else {
                        return 0;
                    }
                }
            });

            nodesSearchResult.Clear();
            for (int i = 0; i < foundNodes.Count(); i++)
            {
                nodesSearchResult.Add(foundNodes[i].id);
            }

            this.SearchNext();

        }

        /// <summary>
        /// SEARCH NEXT
        ///
        /// Search node in cycle. Find first in array or start in begining of array
        /// </summary>
        public void SearchNext()
        {
            Node node = null;

            if (nodesSearchResult.Count() == 0)
                return;

            for (int i = lastFound+1; i < nodesSearchResult.Count(); i++)
            {
                node = this.diagram.GetNodeByID(nodesSearchResult[i]);

                if (node != null)
                {
                    lastFound = i;
                    break;
                }
            }

            if (node == null)
            {
                for (int i = 0; i < lastFound; i++)
                {
                    node = this.diagram.GetNodeByID(nodesSearchResult[i]);

                    if (node != null)
                    {
                        lastFound = i;
                        break;
                    }
                }
            }

            if (node != null)
            {
                this.GoToNodeWithAnimation(node);
                this.SelectOnlyOneNode(node);
                this.diagram.InvalidateDiagram();
            }

        }

        // SEARCH PREV
        public void SearchPrev()
        {
            Node node = null;

            if (nodesSearchResult.Count() == 0)
                return;

            if (lastFound == -1)
            {
                lastFound = 0;
            }

            for (int i = lastFound-1; i >= 0; i--)
            {
                node = this.diagram.GetNodeByID(nodesSearchResult[i]);

                if (node != null)
                {
                    lastFound = i;
                    break;
                }
            }

            if (node == null)
            {
                for (int i = nodesSearchResult.Count()-1; i >= lastFound; i--)
                {
                    node = this.diagram.GetNodeByID(nodesSearchResult[i]);

                    if (node != null)
                    {
                        lastFound = i;
                        break;
                    }
                }
            }

            if (node != null)
            {
                this.GoToNodeWithAnimation(node);
                this.SelectOnlyOneNode(node);
                this.diagram.InvalidateDiagram();
            }
        }

        // SEARCHPANEL SHOW
        private void ShowSearchPanel()
        {
            if (searhPanel == null)
            {
                searhPanel = new SearchPanel(this);
                this.searhPanel.SearchpanelStateChanged += this.SearchPanelChanged;
                this.Controls.Add(this.searhPanel);
            }

            currentPosition.x = this.shift.x;
            currentPosition.y = this.shift.y;
            currentPositionLayer = this.currentLayer.id;

            searhPanel.ShowPanel();
            this.stateSearching = true;
        }

        // SEARCHPANEL CANCEL - restore beggining search position
        private void SearchCancel()
        {
            this.GoToLayer(currentPositionLayer);
            this.shift.x = currentPosition.x;
            this.shift.y = currentPosition.y;


            this.SearchClose();
        }

        // SEARCHPANEL Close - close search panel
        private void SearchClose()
        {
            this.Focus();
            this.stateSearching = false;
            this.diagram.InvalidateDiagram();
        }

        // SEARCHPANEL action
        public void SearchPanelChanged(string action, string search)
        {
            if (action == "searchNext")
            {
                this.SearchNext();
            }

            if (action == "searchPrev")
            {
                this.SearchPrev();
            }

            if (action == "search")
            {
                this.SearchFirst(search);
            }

            if (action == "cancel")
            {
                this.SearchCancel();
            }

            if (action == "close")
            {
                this.SearchClose();
            }
        }

        /*************************************************************************************************************************/

        // CLIPBOARD Copy link to clipboard
        public void CopyLinkToClipboard(Node node)
        {
            Clipboard.SetText(node.link);
        }

        /*************************************************************************************************************************/

        // SCROLLBAR MOVE LEFT-RIGHT set current position in view with relative (0-1) number          // SCROLLBAR
        public void MoveScreenHorizontal(float per)
        {
            int minx = int.MaxValue;
            int maxx = int.MinValue;
            foreach (Node rec in this.currentLayer.nodes)
            {
                if (rec.layer == this.currentLayer.id || rec.id == this.currentLayer.id)
                {
                    if (rec.position.x < minx) minx = rec.position.x;
                    if (maxx < rec.position.x + rec.width) maxx = rec.position.x + rec.width;
                }
            }

            if (minx != int.MaxValue && maxx != int.MinValue)
            {
                minx = minx - 100;
                maxx = maxx + 100 - this.ClientSize.Width;
                this.shift.x = (int)(-(minx + (maxx - minx) * per));
            }
            else
            {
                this.shift.x = 0;
            }
        }

        // SCROLLBAR GET POSITION LEFT-RIGHT calculate current position in view as relative (0-1) number
        public float GetPositionHorizontal()
        {
            float per = 0;
            int minx = int.MaxValue;
            int maxx = int.MinValue;
            foreach (Node rec in this.currentLayer.nodes)
            {
                if (rec.layer == this.currentLayer.id || rec.id == this.currentLayer.id)
                {
                    if (rec.position.x < minx) minx = rec.position.x;
                    if (maxx < rec.position.x + rec.width) maxx = rec.position.x + rec.width;
                }
            }

            if (minx != int.MaxValue && maxx != int.MinValue)
            {
                minx = minx - 100;
                maxx = maxx + 100 - this.ClientSize.Width;
                per = (float)(-this.shift.x - minx) / (maxx - minx);
                if (per < 0) per = 0;
                if (per > 1) per = 1;
                return per;
            }
            else
            {
                return 0;
            }

        }

        // SCROLLBAR MOVE UP-DOWN set current position in view with relative (0-1) number
        public void MoveScreenVertical(float per)
        {
            int miny = int.MaxValue;
            int maxy = int.MinValue;
            foreach (Node rec in this.currentLayer.nodes)
            {
                if (rec.layer == this.currentLayer.id || rec.id == this.currentLayer.id)
                {
                    if (rec.position.y < miny) miny = rec.position.y;
                    if (maxy < rec.position.y + rec.height) maxy = rec.position.y + rec.height;
                }
            }

            if (miny != int.MaxValue && maxy != int.MinValue)
            {
                miny = miny - 100;
                maxy = maxy + 100 - this.ClientSize.Height;
                this.shift.y = -(int)(miny + (maxy - miny) * per);
            }
            else
            {
                this.shift.y = 0;
            }
        }

        // SCROLLBAR GET POSITION LEFT-RIGHT calculate current position in view as relative (0-1) number
        public float GetPositionVertical()
        {
            float per = 0;
            int miny = int.MaxValue;
            int maxy = int.MinValue;
            foreach (Node rec in this.currentLayer.nodes)
            {
                if (rec.layer == this.currentLayer.id || rec.id == this.currentLayer.id)
                {
                    if (rec.position.y < miny) miny = rec.position.y;
                    if (maxy < rec.position.y + rec.height) maxy = rec.position.y + rec.height;
                }
            }

            if (miny != int.MaxValue && maxy != int.MinValue)
            {
                miny = miny - 100;
                maxy = maxy + 100 - this.ClientSize.Height;
                per = (float)(-this.shift.y - miny) / (maxy - miny);
                if (per < 0) per = 0;
                if (per > 1) per = 1;
                return per;
            }
            else
            {
                return 0;
            }
        }

        // SCROLLBAR EVENT position is changed by horizontal scrollbar
        public void PositionChangeBottom(object source, PositionEventArgs e)
        {
            MoveScreenHorizontal(e.GetPosition());
        }

        // SCROLLBAR EVENT position is changed by vertical scrollbar
        public void PositionChangeRight(object source, PositionEventArgs e)
        {
            MoveScreenVertical(e.GetPosition());
        }

        /*************************************************************************************************************************/

        // FILE Save - Save diagram
        public void Save()
        {
            if (!this.diagram.Save())
            {
                this.Saveas();
            }
        }

        // FILE SAVEAS - Save as diagram
        public bool Saveas()
        {
            if (this.DSave.ShowDialog() == DialogResult.OK)
            {                
                this.diagram.Saveas(this.DSave.FileName);
                this.main.options.AddRecentFile(this.DSave.FileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        // FILE Open - Open diagram dialog
        public void Open()
        {
            if (DOpen.ShowDialog() == DialogResult.OK)
            {
                if (Os.FileExists(DOpen.FileName))
                {
                    if (Os.GetExtension(DOpen.FileName).ToLower() == ".diagram")
                    {                        
                        this.Open(DOpen.FileName);
                    }
                    else
                    {
                        MessageBox.Show(Translations.wrongFileExtenson);
                    }
                }
            }
        }

        // FILE Open - Open diagram dialog
        public void Open(String path)
        {
            if (Os.FileExists(path))
            {
                this.main.OpenDiagram(path);
                if (this.diagram.IsNew())
                {
                    this.Close();
                }
            }
        }

        // FILE Open diagram directory if diagram is already saved
        public void OpenDiagramDirectory()
        {
            if (!this.diagram.NewFile && Os.FileExists(this.diagram.FileName))
            {
                Os.ShowDirectoryInExternalApplication(Os.GetDirectoryName(this.diagram.FileName));
            }
        }

        /*************************************************************************************************************************/

        // EXPORT Export diagram to png
        public void ExportDiagramToPng()
        {
            Nodes nodes = null;

            if (this.selectedNodes.Count == 0)
            {
                nodes = this.currentLayer.nodes;
            }
            else
            {
                nodes = this.selectedNodes;
            }

            if (nodes.Count > 0)
            {

                int minx = nodes[0].position.x;
                int maxx = nodes[0].position.x + nodes[0].width;
                int miny = nodes[0].position.y;
                int maxy = nodes[0].position.y + nodes[0].height;

                foreach (Node rec in nodes) // Loop through List with foreach
                {
                    if (rec.position.x < minx)
                    {
                        minx = rec.position.x;
                    }

                    if (maxx < rec.position.x + rec.width)
                    {
                        maxx = rec.position.x + rec.width;
                    }

                    if (rec.position.y < miny)
                    {
                        miny = rec.position.y;
                    }

                    if (maxy < rec.position.y + rec.height)
                    {
                        maxy = rec.position.y + rec.height;
                    }
                }

                minx = minx - 100;
                maxx = maxx + 100;
                miny = miny - 100;
                maxy = maxy + 100;

                Bitmap bmp = new Bitmap(maxx - minx, maxy - miny);
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(this.BackColor);
                this.DrawDiagram(g, new Position(this.shift).Invert().Subtract(minx, miny), true);
                g.Dispose();
                bmp.Save(exportFile.FileName, ImageFormat.Png);
                bmp.Dispose();
            }
        }

        // EXPORT Export diagram to txt
        public void ExportDiagramToTxt(string filePath)
        {

            Nodes nodes = null;

            if (this.selectedNodes.Count == 0)
            {
                nodes = this.diagram.GetAllNodes();
            }
            else
            {
                nodes = this.selectedNodes;
            }

            string outtext = "";

            foreach (Node rec in nodes)
            {
                outtext += rec.name + "\n" + (rec.link != "" ? rec.link + "\n" : "") + "\n" + rec.note + "\n---\n";
            }
            Os.WriteAllText(filePath, outtext);
        }

        /*************************************************************************************************************************/

        // DRAW                                                                                      // [DRAW]
        private void DrawDiagram(Graphics gfx, Position correction = null, bool export = false)
        {
            gfx.SmoothingMode = SmoothingMode.AntiAlias;

            if (this.diagram.options.grid && !export)
            {
                this.DrawGrid(gfx);
            }

            if (this.diagram.IsLocked())
            {
                return;
            }

            this.DrawLines(gfx, this.currentLayer.lines, correction, export);

            if (!export && this.stateCoping)
            {
                this.DrawLines(gfx, this.copySelectedLines, correction, export);
            }

            // DRAW addingnode
            if (!export && this.stateAddingNode && !this.stateZooming && (this.actualMousePos.x != this.startMousePos.x || this.actualMousePos.y != this.startMousePos.y))
            {
                this.DrawAddNode(gfx);
            }

            this.DrawNodes(gfx, this.currentLayer.nodes, correction, export);

            if (!export && this.stateCoping) {
                this.DrawNodes(gfx, this.copySelectedNodes, correction, export);
            }

            // DRAW select - select nodes by mouse drag (blue rectangle - multiselect)
            if (!export && this.stateSelectingNodes && (this.actualMousePos.x != this.startMousePos.x || this.actualMousePos.y != this.startMousePos.y))
            {
                this.DrawNodesSelectArea(gfx);
            }

            // PREVIEW draw zoom mini screen
            if (!export && this.stateZooming)
            {
                this.DrawMiniScreen(gfx);
            }

            // DRAW coordinates
            if (this.diagram.options.coordinates)
            {
                this.DrawCoordinates(gfx);
            }

            // DRAW addingnode
            if (!export)
            {
                this.breadcrumbs.Draw(gfx);
            }
        }

        // DRAW grid
        private void DrawGrid(Graphics gfx)
        {
            float s = this.scale;
            Pen myPen = new Pen(Color.FromArgb(201, 201, 201), 1);

            float m = 100 / s;
            float sw = this.ClientSize.Width;
            float sh = this.ClientSize.Height;
            float lwc = sw / m + 1;
            float lhc = sh / m + 1;
            float six = this.shift.x / s % m;
            float siy = this.shift.y / s % m;

            for (int i = 0; i <= lwc; i++)
            {
                gfx.DrawLine(myPen, six + i * m, 0, six + i * m, sh);
            }

            for (int i = 0; i <= lhc; i++)
            {
                gfx.DrawLine(myPen, 0, siy + i * m, sw, siy + i * m);
            }
        }

        // DRAW diagram mini screen in zoom mode
        private void DrawMiniScreen(Graphics gfx)
        {
            float s = this.scale;
            Pen myPen = new Pen(Color.FromArgb(201, 201, 201), 1);

            myPen = new Pen(Color.Gray, 1);
            gfx.DrawRectangle(
                myPen,
                new Rectangle(
                    (int)((this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / s * this.currentScale)),
                    (int)((this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / s * this.currentScale)),
                    (int)(this.ClientSize.Width / s * this.currentScale),
                    (int)(this.ClientSize.Height / s * this.currentScale)
                )
            );
        }

        // DRAW coordinates for debuging
        private void DrawCoordinates(Graphics gfx)
        {
            float s = this.scale;

            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            gfx.DrawString(
                (this.shift.x).ToString() + "sx," +
                        this.shift.y.ToString() +
                        "sy (" + this.ClientSize.Width.ToString() + "w x " + this.ClientSize.Height.ToString() + "h) " +
                        "" + s.ToString() + "s," + this.currentScale.ToString() + "cs",
                drawFont,
                drawBrush,
                10,
                10
            );
        }

        // DRAW select node by mouse drag (blue rectangle)
        private void DrawNodesSelectArea(Graphics gfx)
        {
            SolidBrush brush = new SolidBrush(Color.FromArgb(100, 10, 200, 200));

            int a = (int)(+this.shift.x - this.startShift.x + this.startMousePos.x * this.scale);
            int b = (int)(+this.shift.y - this.startShift.y + this.startMousePos.y * this.scale);
            int c = (int)(this.actualMousePos.x * this.scale);
            int d = (int)(this.actualMousePos.y * this.scale);
            int temp;
            if (c < a) { temp = a; a = c; c = temp; }
            if (d < b) { temp = d; d = b; b = temp; }

            gfx.FillRectangle(
                brush,
                new Rectangle(
                    (int)(a / this.scale), (int)(b / this.scale),
                    (int)((c - a) / this.scale),
                    (int)((d - b) / this.scale)
                )
            );
        }

        // DRAW add new node by drag
        private void DrawAddNode(Graphics gfx)
        {
            Pen myPen = new Pen(Color.Black, 1);



            if (this.sourceNode == null)
            {
                gfx.DrawLine(
                    myPen,
                    this.shift.x - this.startShift.x + this.startMousePos.x - 2 + 12,
                    this.shift.y - this.startShift.y + this.startMousePos.y - 2 + 12,
                    this.actualMousePos.x,
                    this.actualMousePos.y
                );

                gfx.FillEllipse(
                    new SolidBrush(ColorTranslator.FromHtml("#FFFFB8")),
                    new Rectangle(
                        this.shift.x - this.startShift.x + this.startMousePos.x,
                        this.shift.y - this.startShift.y + this.startMousePos.y,
                        20,
                        20
                    )
                );
                gfx.DrawEllipse(
                    myPen,
                    new Rectangle(
                        this.shift.x - this.startShift.x + this.startMousePos.x,
                        this.shift.y - this.startShift.y + this.startMousePos.y,
                        20, 20
                    )
                );
            }
            else {
                gfx.DrawLine(
                    myPen,
                    this.shift.x + this.sourceNode.position.x + this.sourceNode.width / 2,
                    this.shift.y + this.sourceNode.position.y + this.sourceNode.height / 2,
                    this.actualMousePos.x,
                    this.actualMousePos.y
                );
            }
        }

        // DRAW nodes
        private void DrawNodes(Graphics gfx, Nodes nodes, Position correction = null, bool export = false)
        {
            bool isvisible = false; // drawonly visible elements
            float s = this.scale;

            Pen nodeBorder = new Pen(Color.Black, 1);
            Pen nodeSelectBorder = new Pen(Color.Black, 3);
            Pen nodeLinkBorder = new Pen(Color.DarkRed, 3);
            Pen nodeMarkBorder = new Pen(Color.Navy, 3);

            // fix position for image file export
            int cx = 0;
            int cy = 0;
            if (correction != null)
            {
                cx = correction.x;
                cy = correction.y;
            }

            // DRAW nodes
            foreach (Node rec in nodes) // Loop through List with foreach
            {
                // exclude not visible nodes
                isvisible = false;
                if (export)
                {
                    isvisible = true;
                }
                else
                    if (0 + this.ClientSize.Width <= (this.shift.x + rec.position.x) / s)
                {
                    isvisible = false;
                }
                else
                        if ((this.shift.x + rec.position.x + rec.width) / s <= 0)
                {
                    isvisible = false;
                }
                else
                            if (0 + this.ClientSize.Height <= (this.shift.y + rec.position.y) / s)
                {
                    isvisible = false;
                }
                else
                                if ((this.shift.y + rec.position.y + rec.height) / s <= 0)
                {
                    isvisible = false;
                }
                else
                {
                    isvisible = true;
                }

                if (isvisible && rec.visible)
                {
                    if (rec.isimage)
                    {
                        // DRAW Image
                        gfx.DrawImage(
                                rec.image, new Rectangle(
                                    (int)((this.shift.x + cx + rec.position.x) / s),
                                    (int)((this.shift.y + cy + rec.position.y) / s),
                                    (int)(rec.width / s),
                                    (int)(rec.height / s)
                                )
                        );

                        if (rec.selected && !export)
                        {
                            gfx.DrawRectangle(
                                nodeSelectBorder,
                                new Rectangle(
                                    (int)((this.shift.x + cx + rec.position.x - 3) / s),
                                    (int)((this.shift.y + cy + rec.position.y - 3) / s),
                                    (int)((rec.width + 5) / s),
                                    (int)((rec.height + 5) / s)
                                )
                            );
                        }

                    }
                    else
                    {
                        if (this.diagram.options.coordinates) // draw debug information
                        {
                            Font drawFont = new Font("Arial", 10 / s);
                            SolidBrush drawBrush = new SolidBrush(Color.Black);
                            gfx.DrawString(
                                rec.id.ToString() + "i:" + (rec.position.x).ToString() + "x," + (rec.position.y).ToString()+"y",
                                drawFont,
                                drawBrush,
                                (this.shift.x + rec.position.x) / s,
                                (this.shift.y + rec.position.y - 20) / s
                            );
                        }

                        // DRAW rectangle
                        Rectangle rect1 = new Rectangle(
                            (int)((this.shift.x + cx + rec.position.x) / s),
                            (int)((this.shift.y + cy + rec.position.y) / s),
                            (int)((rec.width) / s),
                            (int)((rec.height) / s)
                        );

                        // DRAW border

                        if (rec.name.Trim() == "") // draw empty point
                        {
                            if (!rec.transparent) // draw fill point
                            {
                                gfx.FillEllipse(new SolidBrush(rec.color.color), rect1);
                                if (this.diagram.options.borders) gfx.DrawEllipse(nodeBorder, rect1);
                            }

                            if (rec.haslayer && !export) // draw layer indicator
                            {
                                gfx.DrawEllipse(nodeBorder, new Rectangle(
                                        (int)((this.shift.x + cx + rec.position.x - 2) / s),
                                        (int)((this.shift.y + cy + rec.position.y - 2) / s),
                                        (int)((rec.width + 4) / s),
                                        (int)((rec.height + 4) / s)
                                    )
                                );
                            }

                            if (rec.selected && !export)
                            {
                                gfx.DrawEllipse(
                                    (rec.link != "") ? nodeLinkBorder : ((rec.mark) ? nodeMarkBorder : nodeSelectBorder),
                                    new Rectangle(
                                        (int)((this.shift.x + cx + rec.position.x - 2) / s),
                                        (int)((this.shift.y + cy + rec.position.y - 2) / s),
                                        (int)((rec.width + 4) / s),
                                        (int)((rec.height + 4) / s)
                                    )
                                );
                            }
                        }
                        else
                        {
                            // draw filled node rectangle
                            if (!rec.transparent)
                            {
                                gfx.FillRectangle(new SolidBrush(rec.color.color), rect1);
                                if (this.diagram.options.borders) gfx.DrawRectangle(nodeBorder, rect1);
                            }

                            // draw layer indicator
                            if (rec.haslayer && !export)
                            {
                                gfx.DrawRectangle(
                                    nodeBorder,
                                    new Rectangle(
                                        (int)((this.shift.x + cx + rec.position.x - 2) / s),
                                        (int)((this.shift.y + cy + rec.position.y - 2) / s),
                                        (int)((rec.width + 4) / s),
                                        (int)((rec.height + 4) / s)
                                    )
                                );
                            }

                            // draw selected node border
                            if (rec.selected && !export)
                            {
                                gfx.DrawRectangle(
                                    (rec.link != "") ? nodeLinkBorder: ((rec.mark)? nodeMarkBorder : nodeSelectBorder),
                                    new Rectangle(
                                        (int)((this.shift.x + cx + rec.position.x - 2) / s),
                                        (int)((this.shift.y + cy + rec.position.y - 2) / s),
                                        (int)((rec.width + 4) / s),
                                        (int)((rec.height + 4) / s)
                                    )
                                );
                            }


                            // DRAW text
                            RectangleF rect2 = new RectangleF(
                                (int)((this.shift.x + cx + rec.position.x + Node.NodePadding) / s),
                                (int)((this.shift.y + cy + rec.position.y + Node.NodePadding) / s),
                                (int)((rec.width - Node.NodePadding) / s),
                                (int)((rec.height - Node.NodePadding) / s)
                            );


                            gfx.DrawString(
                                (rec.protect) ? Node.protectedName : rec.name,
                                new Font(
                                    rec.font.FontFamily,
                                    rec.font.Size / s,
                                    rec.font.Style,
                                    GraphicsUnit.Point,
                                    ((byte)(0))
                                ),
                                new SolidBrush(rec.fontcolor.color),
                                rect2
                            );
                        }
                    }
                }
            }
        }

        // DRAW lines
        private void DrawLines(Graphics gfx, Lines lines, Position correction = null, bool export = false)
        {
            bool isvisible = false; // drawonly visible elements
            float s = this.scale;

            // fix position for image file export
            int cx = 0;
            int cy = 0;
            if (correction != null)
            {
                cx = correction.x;
                cy = correction.y;
            }

            // DRAW lines
            foreach (Line lin in lines) // Loop through List with foreach
            {

                Node r1 = lin.startNode;
                Node r2 = lin.endNode;

                isvisible = false;
                if (export)
                {
                    isvisible = true;
                }
                else
                    if (0 + this.ClientSize.Width <= (this.shift.x + r1.position.x) / s && 0 + this.ClientSize.Width <= (this.shift.x + r2.position.x) / s)
                {
                    isvisible = false;
                }
                else
                        if ((this.shift.x + r1.position.x) / s <= 0 && (this.shift.x + r2.position.x) / s <= 0)
                {
                    isvisible = false;
                }
                else
                            if (0 + this.ClientSize.Height <= (this.shift.y + r1.position.y) / s && 0 + this.ClientSize.Height <= (this.shift.y + r2.position.y) / s)
                {
                    isvisible = false;
                }
                else
                                if ((this.shift.y + r1.position.y) / s <= 0 && (this.shift.y + r2.position.y) / s <= 0)
                {
                    isvisible = false;
                }
                else
                {
                    isvisible = true;
                }


                if (isvisible)
                {

                    if (lin.arrow) // draw line as arrow
                    {
                        float x1 = (this.shift.x + cx + r1.position.x + r1.width / 2) / s;
                        float y1 = (this.shift.y + cy + r1.position.y + r1.height / 2) / s;
                        float x2 = (this.shift.x + cx + r2.position.x + r2.width / 2) / s;
                        float y2 = (this.shift.y + cy + r2.position.y + r2.height / 2) / s;
                        double nx1 = (Math.Cos(Math.PI / 2) * (x2 - x1) - Math.Sin(Math.PI / 2) * (y2 - y1) + x1);
                        double ny1 = (Math.Sin(Math.PI / 2) * (x2 - x1) + Math.Cos(Math.PI / 2) * (y2 - y1) + y1);
                        double nx2 = (Math.Cos(-Math.PI / 2) * (x2 - x1) - Math.Sin(-Math.PI / 2) * (y2 - y1) + x1);
                        double ny2 = (Math.Sin(-Math.PI / 2) * (x2 - x1) + Math.Cos(-Math.PI / 2) * (y2 - y1) + y1);
                        double size = Math.Sqrt((nx1 - x1) * (nx1 - x1) + (ny1 - y1) * (ny1 - y1));
                        nx1 = x1 + (((nx1 - x1) / size) * 7) / s;
                        ny1 = y1 + (((ny1 - y1) / size) * 7) / s;
                        nx2 = x1 + (((nx2 - x1) / size) * 7) / s;
                        ny2 = y1 + (((ny2 - y1) / size) * 7) / s;

                        // Create points that define polygon.
                        PointF point1 = new PointF((float)nx1, (float)ny1);
                        PointF point2 = new PointF((float)nx2, (float)ny2);
                        PointF point3 = new PointF(x2, y2);
                        PointF[] curvePoints = { point1, point2, point3 };

                        // Define fill mode.
                        FillMode newFillMode = FillMode.Winding;

                        // Fill polygon to screen.
                        gfx.FillPolygon(
                            new SolidBrush(lin.color.color),
                            curvePoints,
                            newFillMode
                        );
                    }
                    else
                    {
                        // draw line
                        gfx.DrawLine(
                            new Pen(lin.color.color, lin.width / s > 1 ? (int)lin.width / s : 1),
                            (this.shift.x + cx + r1.position.x + r1.width / 2) / s,
                            (this.shift.y + cy + r1.position.y + r1.height / 2) / s,
                            (this.shift.x + cx + r2.position.x + r2.width / 2) / s,
                            (this.shift.y + cy + r2.position.y + r2.height / 2) / s
                        );
                    }

                }

            }
        }

        /*************************************************************************************************************************/

        // DIAGRAM Set model
        public void SetDiagram(Diagram diagram)
        {
            this.diagram = diagram;
        }

        // DIAGRAM Get model
        public Diagram GetDiagram()
        {
            return this.diagram;
        }

        /*************************************************************************************************************************/

        // VIEW CLOSE
        private void DiagramView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.diagram.CloseView(this);
        }

        // VIEW REFRESH
        private void DiagramView_Activated(object sender, EventArgs e)
        {
            /*if (this.diagram.isLocked())
            {
                this.diagram.unlockDiagram();
            }*/

            this.Invalidate();
        }

        // VIEW page up
        public void PageUp()
        {
            this.shift.y = this.shift.y + this.ClientSize.Height;
            this.diagram.InvalidateDiagram();
        }

        // VIEW page down
        public void PageDown()
        {
            this.shift.y = this.shift.y - this.ClientSize.Height;
            this.diagram.InvalidateDiagram();
        }

        // VIEW reset zoom
        public void ResetZoom()
        {
            this.currentScale = this.zoomingDefaultScale;
            this.scale = this.zoomingDefaultScale;
            this.diagram.InvalidateDiagram();
        }

        // VIEW get mouse position
        public Position GetMousePosition()
        {
            Point ptCursor = Cursor.Position;
            ptCursor = this.PointToClient(ptCursor);
            return new Position(ptCursor.X, ptCursor.Y);
        }

        // VIEW full screen
        private void FullScreenSwitch()
        {
            if (this.isFullScreen)
            {
                this.isFullScreen = false;
                WindowState = FormWindowState.Normal;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

            }
            else
            {
                this.isFullScreen = true;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }


        }

        /*************************************************************************************************************************/

        // NODE create
        public Node CreateNode(Position position, bool SelectAfterCreate = true)
        {
            var rec = this.diagram.CreateNode(
                position.Clone().Scale(this.scale).Subtract(this.shift),
                "",
                this.currentLayer.id
            );

            if (rec != null)
            {
                if (SelectAfterCreate) this.SelectOnlyOneNode(rec);
            }

            return rec;
        }

        // NODE create after
        public void AddNodeAfterNode()
        {
            if (this.selectedNodes.Count() == 1)
            {
                if (!this.editPanel.Visible)
                {
                    this.editPanel.prevSelectedNode = this.selectedNodes[0];
                    this.editPanel.showEditPanel(
                        this.selectedNodes[0]
                            .position
                            .Clone()
                            .Add(this.shift)
                            .Add(this.selectedNodes[0].width + 10, 0),
                        ' ',
                        false
                    );
                }
            }
        }

        // NODE create below
        public void AddNodeBelowNode()
        {
            if (this.selectedNodes.Count() == 1)
            {
                if (!this.editPanel.Visible)
                {
                    this.editPanel.prevSelectedNode = this.selectedNodes[0];
                    this.editPanel.showEditPanel(
                        this.selectedNodes[0]
                            .position
                            .Clone()
                            .Add(this.shift)
                            .Add(0, this.selectedNodes[0].height + 10),
                        ' ',
                        false
                    );
                }
            }
        }

        // NODE open link directory
        public void OpenLinkDirectory()
        {
            if (this.selectedNodes.Count() > 0)
            {
                foreach (Node node in this.selectedNodes)
                {
                    if (node.link.Trim().Length > 0)
                    {
                        if (Os.DirectoryExists(node.link)) // open directory of selected nods
                        {
                            Os.ShowDirectoryInExternalApplication(node.link);
                        }
                        else
                        if (Os.FileExists(node.link)) // open directory of selected files
                        {
                            string parent_diectory = Os.GetFileDirectory(this.selectedNodes[0].link);
                            Os.ShowDirectoryInExternalApplication(parent_diectory);
                        }
                    }
                }
            }
            else
            {
                this.OpenDiagramDirectory(); // open main directory of diagram
            }
        }

        // NODES DELETE SELECTION
        public void DeleteSelectedNodes(DiagramView DiagramView)
        {
            if (!this.diagram.options.readOnly)
            {
                if (DiagramView.selectedNodes.Count() > 0)
                {
                    this.diagram.DeleteNodes(DiagramView.selectedNodes, this.shift, this.currentLayer.id);
                }
            }
        }

        // NODE Go to node position
        public void GoToNode(Node rec)
        {
            if (rec != null)
            {
                this.GoToPosition(rec.position);
                this.GoToLayer(rec.layer);
            }
        }

        // NODE Go to position
        public void GoToPosition(Position position)
        {
            if (position != null)
            {
                this.shift.x = (int)(-position.x + this.ClientSize.Width / 2 * this.scale);
                this.shift.y = (int)(-position.y + this.ClientSize.Height / 2 * this.scale);
            }
        }

        // NODE Check to shift
        public bool IsOnPosition(Position shift, int layer)
        {
            if (shift.x == this.shift.x && shift.y == this.shift.y && this.currentLayer.id == layer)
            {
                return true;
            }

            return false;
        }

        // NODE Go to shift
        public void GoToShift(Position shift)
        {
            if (shift != null)
            {
                this.shift.Set(shift);
            }
        }
        // NODE Go to node layer
        public void GoToLayer(int layer = 0)
        {
            this.currentLayer = this.diagram.layers.GetLayer(layer);
            this.BuildLayerHistory(layer);
        }

        // NODE find node in mouse cursor position
        public Node FindNodeInMousePosition(Position position, Node skipNode = null)
        {
            return this.diagram.FindNodeInPosition(
                new Position(
                    (int)(position.x * this.scale - this.shift.x),
                    (int)(position.y * this.scale - this.shift.y)
                ),
                this.currentLayer.id,
                skipNode
            );
        }

        // NODE Open Link
        public void OpenLinkAsync(Node rec)
        {
            Program.log.write("diagram: openlink");
            String clipboard = Os.GetTextFormClipboard();


#if DEBUG
            var result = 0;
            result = this.OpenLink(rec, clipboard);
            if ((int)result == 1)
            {
                this.diagram.EditNode(rec);
            }
#else
            var worker = new BackgroundWorker {
                WorkerSupportsCancellation = true
            };

            worker.DoWork += (sender, e) =>
            {
                Program.log.write("diagram: openlink: run worker");
                e.Result = this.OpenLink(rec, clipboard);
            };
            worker.RunWorkerCompleted += (sender, e) =>
            {

                var result = e.Result;
                if ((int)result == 1)
                {
                    this.diagram.EditNode(rec);
                }

                if ((int)result == 2)
                {
                   this.SelectOnlyOneNode(rec);
                   this.AttachmentDeploy();
                }
            };
            worker.RunWorkerAsync();
#endif
        }

        // NODE Open Link
        public int OpenLink(Node rec, String clipboard = "")
        {
            if (rec != null)
            {
                if (rec.attachment != "") { //deploy attachment                    
                    return 2; // deploy attachment
                }
                else
                if (rec.shortcut > 0) // GO TO LINK
                {
                    Node target = this.diagram.GetNodeByID(rec.shortcut);
                    this.GoToNode(target);
                    this.diagram.InvalidateDiagram();
                }
                else
                if (rec.link.Length > 0)
                {

                    string fileName = "";
                    string searchString = "";

                    if (!Patterns.isURL(rec.link)
                        && Patterns.hasHastag(rec.link.Trim(), ref fileName, ref searchString)
                        && Os.FileExists(Os.NormalizedFullPath(fileName)))       // OPEN FILE ON LINE POSITION
                    {
                        try
                        {
                            // if is not set search strin, only hastag after link then use node name as string for search
                            if(searchString.Trim() == "")
                            {
                                searchString = rec.name;
                            }

                            // if search string is not number then search for first line with search string
                            if (!Patterns.isNumber(searchString))
                            {
                                searchString = Os.FndLineNumber(fileName, searchString).ToString();
                            }

                            // get external editor path from global configuration saved in user configuration directory
                            String editFileCmd = this.main.options.texteditor;
                            editFileCmd = editFileCmd.Replace("%FILENAME%", Os.NormalizedFullPath(fileName));
                            editFileCmd = editFileCmd.Replace("%LINE%", searchString);

                            Program.log.write("diagram: openlink: open file on position " + editFileCmd);
                            Os.RunCommand(editFileCmd);
                        }
                        catch (Exception ex)
                        {
                            Program.log.write("open link as file error: " + ex.Message);
                        }
                    }
                    else if (rec.link.Trim() == "script" || rec.link.Trim() == "macro" || rec.link.Trim() == "$")  // OPEN SCRIPT node with link "script" is executed as script
                    {
                        this.Evaluate(rec, clipboard);
                    }
                    else if (Os.DirectoryExists(rec.link))  // OPEN DIRECTORY
                    {
                        try
                        {
                            Program.log.write("diagram: openlink: open directory " + Os.NormalizePath(rec.link));
                            Os.RunProcess(rec.link);
                        }
                        catch (Exception ex)
                        {
                            Program.log.write("open link as directory error: " + ex.Message);
                        }
                    }
                    else if (Os.FileExists(rec.link))       // OPEN FILE
                    {
                        try
                        {
                            if (Os.IsDiagram(rec.link))
                            {
                                Program.log.write("diagram: openlink: open diagram " + Os.NormalizePath(rec.link));
                                Os.OpenDiagram(rec.link);
                            }
                            else
                            {
                                Program.log.write("diagram: openlink: open file " + Os.NormalizePath(rec.link));
                                Os.RunProcess(rec.link);
                            }
                        }
                        catch (Exception ex)
                        {
                            Program.log.write("open link as file error: " + ex.Message);
                        }
                    }
                    else if (Patterns.isURL(rec.link)) // OPEN URL
                    {
                        try
                        {
                            Program.log.write("diagram: openlink: open url " + rec.link);
                            Network.openUrl(rec.link);
                        }
                        catch (Exception ex)
                        {
                            Program.log.write("open link as url error: " + ex.Message);
                        }
                    }
                    else if (Patterns.isGotoIdCommand(rec.link)) // GOTO position
                    {
                        Program.log.write("go to position in diagram " + rec.link);
                        Node node = this.diagram.GetNodeByID(Patterns.getGotoIdCommand(rec.link));
                        if (node != null) {
                            this.GoToNodeWithAnimation(node);
                        }
                    }
                    else if (Patterns.isGotoStringCommand(rec.link)) // GOTO position
                    {
                        Program.log.write("go to position in diagram " + rec.link);
                        String searchFor = Patterns.getGotoStringCommand(rec.link);
                        Nodes nodes = this.diagram.layers.SearchInAllNodes(searchFor);
                        if (nodes.Count() >= 2)
                        {
                            if (rec != nodes[0]) {
                                this.GoToNodeWithAnimation(nodes[0]);
                            } else {
                                this.GoToNodeWithAnimation(nodes[1]);
                            }
                        }
                    }
                    else // run as command
                    {

                        // set current directory to current diagrm file destination
                        if (Os.FileExists(this.diagram.FileName))
                        {
                            Os.SetCurrentDirectory(Os.GetFileDirectory(this.diagram.FileName));
                        }

                        /*
                        - stamps in command
                            %TEXT%     - name of node
                            %NAME%     - name of node
                            %LINK%     - link in node
                            %NOTE%     - note in node
                            %ID%       - id of actual node
                            %FILENAME% - file name  of diagram
                            %DIRECTORY% - current diagram directory
                        */

                        // replace stamps in link
                        string cmd = rec.link
                        .Replace("%TEXT%", rec.name)
                        .Replace("%NAME%", rec.name)
                        .Replace("%LINK%", rec.link)
                        .Replace("%NOTE%", rec.note)
                        .Replace("%ID%", rec.id.ToString())
                        .Replace("%FILENAME%", this.diagram.FileName)
                        .Replace("%DIRECTORY%", Os.GetFileDirectory(this.diagram.FileName));

                        Program.log.write("diagram: openlink: run command: " + cmd);
                        Os.RunCommand(cmd, Os.GetFileDirectory(this.diagram.FileName)); // RUN COMMAND
                    }
                }
                else if (rec.haslayer) {
                    if (this.diagram.options.openLayerInNewView)
                    {
                        this.diagram.OpenDiagramView(
                            this,
                            this.diagram.layers.GetLayer(
                                rec.id
                            )
                        );
                    }
                    else
                    {
                        this.LayerIn(rec);
                    }
                }
                else // EDIT NODE
                {
                    return 1;
                }
            }
            return 0;
        }

        // NODE Remove shortcuts
        public void RemoveShortcuts(Nodes Nodes)
        {
            foreach (Node rec in Nodes) // Loop through List with foreach
            {
                this.diagram.RemoveShortcut(rec);
            }

            this.diagram.InvalidateDiagram();
        }

        // NODE Select node color
        public void SelectColor()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                // show color picker and move it to vsible area of screen
                var screen = Screen.FromPoint(Cursor.Position);
                colorPickerForm.StartPosition = FormStartPosition.Manual;
                colorPickerForm.Left = Cursor.Position.X - 50;
                colorPickerForm.Top = Cursor.Position.Y - 50;

                if (Cursor.Position.X - 50 < 0) {
                    colorPickerForm.Left = 0;
                }

                if (Cursor.Position.Y - 50 < 0)
                {
                    colorPickerForm.Top = 0;
                }

                if (Cursor.Position.X - 50 + colorPickerForm.Width > screen.Bounds.Width)
                {
                    colorPickerForm.Left = screen.Bounds.Width - colorPickerForm.Width;
                }

                if (Cursor.Position.Y - 50 + colorPickerForm.Height > screen.Bounds.Height)
                {
                    colorPickerForm.Top = screen.Bounds.Height - colorPickerForm.Height;
                }

                colorPickerForm.ShowDialog();
            }
        }

        public void ChangeColor(ColorType color)
        {
            if (!this.diagram.options.readOnly)
            {
                if (selectedNodes.Count() > 0)
                {
                    if (!this.diagram.undoOperations.isSame("changeNodeColor", this.selectedNodes, null))
                    {
                        this.diagram.Unsave("changeNodeColor", this.selectedNodes, null, this.shift, this.currentLayer.id);
                    }

                    foreach (Node rec in this.selectedNodes)
                    {
                        rec.color.Set(color);
                    }

                    this.Invalidate();
                }
            }
        }

        // NODE Select node font color
        public void SelectFontColor()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                DFontColor.Color = this.selectedNodes[0].color.color;

                if (DColor.ShowDialog() == DialogResult.OK)
                {
                    if (!this.diagram.options.readOnly)
                    {
                        if (selectedNodes.Count() > 0)
                        {
                            foreach (Node rec in this.selectedNodes)
                            {
                                rec.fontcolor.Set(DColor.Color);
                            }
                        }
                    }

                    this.diagram.InvalidateDiagram();
                }
            }
        }

        // NODE Select node font
        public void SelectFont()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                DFont.Font = this.selectedNodes[0].font;

                if (DFont.ShowDialog() == DialogResult.OK)
                {
                    if (!this.diagram.options.readOnly)
                    {
                        if (selectedNodes.Count() > 0)
                        {
                            foreach (Node rec in this.selectedNodes)
                            {
                                rec.font = DFont.Font;
                                rec.resize();
                            }
                        }
                    }

                    this.diagram.InvalidateDiagram();
                }
            }
        }

        // NODE Check if selected nodes are transparent
        public bool IsSelectionTransparent()
        {
            bool isTransparent = false;

            if (selectedNodes.Count() > 0)
            {
                foreach (Node rec in this.selectedNodes)
                {
                    if (rec.transparent)
                    {
                        isTransparent = true;
                        break;
                    }
                }
            }

            return isTransparent;
        }

        // NODE Make selected node transparent
        public void MakeSelectionTransparent()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                bool isTransparent = this.IsSelectionTransparent();

                foreach (Node rec in this.selectedNodes)
                {
                    rec.transparent = !isTransparent;
                }

                this.diagram.InvalidateDiagram();

            }
        }

        // NODE Select node default font
        public void SelectDefaultFont()
        {
            this.defaultfontDialog.Font = this.diagram.FontDefault;
            if (this.defaultfontDialog.ShowDialog() == DialogResult.OK)
            {
                if (!this.diagram.options.readOnly)
                {
                    this.diagram.FontDefault = this.defaultfontDialog.Font;
                }
            }
        }

        // NODE Select node image
        public void AddImage()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                if (this.DImage.ShowDialog() == DialogResult.OK && Os.FileExists(this.DImage.FileName))
                {
                    this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);

                    foreach (Node rec in this.selectedNodes)
                    {
                        this.diagram.SetImage(rec, Os.GetFullPath(this.DImage.FileName));
                    }

                    this.diagram.Unsave();
                }

                this.diagram.InvalidateDiagram();
            }
            else
            {
                if (this.DImage.ShowDialog() == DialogResult.OK && Os.FileExists(this.DImage.FileName))
                {

                    Node newrec = this.CreateNode(this.startMousePos);
                    this.diagram.SetImage(newrec, this.DImage.FileName);
                    this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                }

                this.diagram.InvalidateDiagram();
            }
        }

        // NODE Select node image
        public void RemoveImagesFromSelection()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                foreach (Node rec in this.selectedNodes)
                {
                    if (rec.isimage)
                    {
                        this.diagram.RemoveImage(rec);
                    }
                }

                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE Check if selected nodes are transparent
        public bool HasSelectionImage()
        {
            bool hasImage = false;

            if (selectedNodes.Count() > 0)
            {
                foreach (Node rec in this.selectedNodes)
                {
                    if (rec.isimage)
                    {
                        hasImage = true;
                        break;
                    }
                }
            }

            return hasImage;
        }

        // NODE Check if selected nodes are transparent
        public bool HasSelectionNotEmbeddedImage()
        {
            bool hasImage = false;

            if (selectedNodes.Count() > 0)
            {
                foreach (Node rec in this.selectedNodes)
                {
                    if (rec.isimage && !rec.embeddedimage)
                    {
                        hasImage = true;
                        break;
                    }
                }
            }

            return hasImage;
        }

        // NODE Make selected node transparent
        public void MakeImagesEmbedded()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                bool hasImage = this.HasSelectionNotEmbeddedImage();

                if (hasImage) {
                    this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);

                    foreach (Node rec in this.selectedNodes)
                    {
                        this.diagram.SetImageEmbedded(rec);
                    }

                    this.diagram.Unsave();
                }

                this.diagram.InvalidateDiagram();
            }
        }

        // NODE copy
        public bool Copy()
        {
            if (this.selectedNodes.Count() > 0)
            {
                DataObject data = new DataObject();

                string copytext = "";
                foreach (Node rec in this.selectedNodes)
                {
                    copytext = copytext + rec.name;

                    if (this.selectedNodes.Count() > 1)
                    {
                        copytext = copytext + "\n";
                    }
                }

                data.SetData(copytext);

                data.SetData("DiagramXml", this.diagram.GetDiagramPart(this.selectedNodes));//create and copy xml

                Clipboard.SetDataObject(data);

                return true;
            }

            return false;
        }

        // NODE cut
        public bool Cut()
        {
            DataObject data = new DataObject();
            if (this.selectedNodes.Count() > 0)  // kopirovanie textu objektu
            {
                string copytext = "";
                foreach (Node rec in this.selectedNodes)
                {
                    copytext = copytext + rec.name + "\n";
                }

                data.SetData(copytext);

                data.SetData("DiagramXml", this.diagram.GetDiagramPart(this.selectedNodes)); //create and copy xml
                this.DeleteSelectedNodes(this);
                this.ClearSelection();
                this.diagram.InvalidateDiagram();
            }

            Clipboard.SetDataObject(data);

            return true;
        }

        // NODE paste
        public bool Paste(Position position)
        {
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();

            if (retrievedData.GetDataPresent("DiagramXml"))  // [PASTE] [DIAGRAM] [CLIPBOARD OBJECT] insert diagram
            {
                DiagramBlock newBlock = this.diagram.AddDiagramPart(
                    retrievedData.GetData("DiagramXml") as string,
                    position.Clone().Scale(this.scale).Subtract(this.shift),
                    this.currentLayer.id
                );

                this.diagram.Unsave("create", newBlock.nodes, newBlock.lines);

                // filter only top nodes fromm all new created nodes. NewNodes containing sublayer nodes.
                Nodes topNodes = new Nodes();
                foreach (Node node in newBlock.nodes)
                {
                    if (node.layer == this.currentLayer.id)
                    {
                        topNodes.Add(node);
                    }
                }

                this.SelectNodes(topNodes);
                this.diagram.InvalidateDiagram();
            }
            else
            if (retrievedData.GetDataPresent(DataFormats.StringFormat))  // [PASTE] [TEXT] insert text
            {
                Node newrec = this.CreateNode(position);

                string ClipText = retrievedData.GetData(DataFormats.StringFormat) as string;

                if (Patterns.isColor(ClipText)) {
                    newrec.setName(ClipText);
                    newrec.color.Set(Media.GetColor(ClipText));
                    this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                }
                else if (Patterns.isURL(ClipText))  // [PASTE] [URL] [LINK] paste link from clipboard
                {
                    newrec.link = ClipText;
                    newrec.setName(ClipText);

                    this.SetNodeNameByLink(newrec, ClipText);

                    this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                }
                else
                {
                    newrec.setName(ClipText);

                    // set link to node as path to file
                    if (Os.FileExists(ClipText))
                    {
                        newrec.setName(Os.GetFileName(ClipText));
                        newrec.link = Os.MakeRelative(ClipText, this.diagram.FileName);
                        newrec.color.Set(Media.GetColor(diagram.options.colorFile));
                    }

                    // set link to node as path to directory
                    if (Os.DirectoryExists(ClipText))
                    {
                        newrec.setName(Os.GetFileName(ClipText));
                        newrec.link = Os.MakeRelative(ClipText, this.diagram.FileName);
                        newrec.color.Set(Media.GetColor(diagram.options.colorDirectory));
                    }

                    this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                }

                this.diagram.InvalidateDiagram();
            }
            else
            if (Clipboard.ContainsFileDropList()) // [FILES] [PASTE] insert files from clipboard
            {
                StringCollection returnList = Clipboard.GetFileDropList();
                Nodes nodes = new Nodes();
                foreach (string file in returnList)
                {
                    Node newrec = this.CreateNode(position);
                    nodes.Add(newrec);
                    newrec.setName(Os.GetFileNameWithoutExtension(file));

                    string ext = Os.GetExtension(file);

                    if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp") // paste image file direct to diagram as image instead of link
                    {
                        this.diagram.SetImage(newrec, file);
                    }
                    else
                    if (
                        this.diagram.FileName != ""
                        && Os.FileExists(this.diagram.FileName)
                        && file.IndexOf(Os.GetDirectoryName(this.diagram.FileName)) == 0
                    ) // [PASTE] [FILE]
                    {
                        // make path relative to saved diagram path
                        int start = Os.GetDirectoryName(this.diagram.FileName).Length;
                        int finish = file.Length - start;
                        newrec.link = "." + file.Substring(start, finish);
                    }
                    else
                    if (this.diagram.FileName != "" && Os.DirectoryExists(this.diagram.FileName)) // [PASTE] [DIRECTORY]
                    {
                        // make path relative to saved diagram path
                        int start = Os.GetDirectoryName(this.diagram.FileName).Length;
                        int finish = file.Length - start;
                        newrec.link = "." + file.Substring(start, finish);
                    }
                    else
                    {
                        newrec.link = file;
                    }
                }

                if (nodes.Count() > 0)
                {
                    this.diagram.Unsave("create", nodes, null, this.shift, this.currentLayer.id);
                    this.diagram.InvalidateDiagram();
                }
            }
            else if (Clipboard.GetDataObject() != null)  // [PASTE] [IMAGE] [CLIPBOARD OBJECT] paste image
            {
                IDataObject data = Clipboard.GetDataObject();

                if (data.GetDataPresent(DataFormats.Bitmap))
                {
                    // paste image as embedded data direct inside diagram
                    try
                    {
                        Node newrec = this.CreateNode(position);

                        newrec.image = (Bitmap)data.GetData(DataFormats.Bitmap, true);
                        newrec.height = newrec.image.Height;
                        newrec.width = newrec.image.Width;
                        newrec.isimage = true;
                        newrec.embeddedimage = true;

                        this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                        this.diagram.InvalidateDiagram();

                    }
                    catch (Exception e)
                    {
                        Program.log.write("paste immage error: " + e.Message);
                    }
                }

            }

            return true;
        }

        // NODE paste to note
        public bool PasteToNote()
        {
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();

            if (retrievedData.GetDataPresent(DataFormats.Text))
            {
                string ClipText = retrievedData.GetData(DataFormats.Text) as string;

                if (this.selectedNodes.Count() == 0)
                {
                    // paste text as new node
                    Node newrec = this.CreateNode(this.GetMousePosition());

                    newrec.note = ClipText;
                    this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                }
                else
                {
                    // append text to all selected nodes
                    this.diagram.undoOperations.add("edit", this.selectedNodes);
                    foreach (Node rec in this.selectedNodes)
                    {
                        if (rec.note != "") // append to end of note
                        {
                            rec.note += "\n";
                        }

                        rec.note += ClipText;
                    }

                }
            }

            this.diagram.InvalidateDiagram();
            return true;
        }

        // NODE paste to link
        public bool PasteToLink()
        {
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();

            if (retrievedData.GetDataPresent(DataFormats.Text))
            {
                string ClipText = retrievedData.GetData(DataFormats.Text) as string;

                if (this.selectedNodes.Count() == 0)
                {
                    Node newrec = this.CreateNode(this.GetMousePosition());

                    newrec.link = ClipText;
                    this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                }
                else
                {
                    this.diagram.undoOperations.add("edit", this.selectedNodes);
                    foreach (Node rec in this.selectedNodes)
                    {

                        rec.link += ClipText;
                    }

                }
            }

            this.diagram.InvalidateDiagram();
            return true;
        }

        // NODE align to line
        public void AlignToLine()
        {
            if (this.selectedNodes.Count() > 0)
            {
                this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                this.diagram.AlignToLine(this.selectedNodes);
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE align to column
        public void AlignToColumn()
        {
            if (this.selectedNodes.Count() > 0)
            {
                this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                this.diagram.AlignToColumn(this.selectedNodes);
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE align to group
        public void AlignToGroup()
        {
            if (this.selectedNodes.Count() > 0)
            {
                this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                this.diagram.AlignCompact(this.selectedNodes);
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE align to group and sort
        public void SortNodes()
        {
            if (this.selectedNodes.Count() > 0)
            {
                this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                this.diagram.SortNodes(this.selectedNodes);
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE split node by line and grou
        public void SplitNode()
        {
            if (this.selectedNodes.Count() > 0)
            {
                Nodes newNodes = this.diagram.SplitNode(this.selectedNodes);
                this.diagram.Unsave("create", newNodes, null, this.shift, this.currentLayer.id);
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE align to group
        public void AlignToLineGroup()
        {
            if (this.selectedNodes.Count() > 0)
            {
                this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                this.diagram.AlignCompactLine(this.selectedNodes);
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE align left
        public void AlignLeft()
        {
            if (this.selectedNodes.Count() > 1)
            {
                this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                this.diagram.AlignLeft(this.selectedNodes);
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
            else
            {
                this.AddNodeAfterNode();
            }
        }

        // NODE align right
        public void AlignRight()
        {
            if (this.selectedNodes.Count() > 1)
            {
                this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                this.diagram.AlignRight(this.selectedNodes);
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
            else
            {
                this.AddNodeBelowNode();
            }
        }

        // NODE copy node link
        public bool CopyLink()
        {
            if (this.selectedNodes.Count() > 0)
            {
                string copytext = "";
                foreach (Node rec in this.selectedNodes)
                {
                    if (rec.link != null)
                    {
                        copytext = copytext + rec.link;

                        if (this.selectedNodes.Count() > 1)
                        { //separate nodes
                            copytext = copytext + "\n";
                        }
                    }
                }

                if (copytext != "")
                {
                    Clipboard.SetText(copytext);
                }
            }

            return true;
        }

        // NODE copy note
        public bool CopyNote()
        {
            if (this.selectedNodes.Count() > 0)
            {
                string copytext = "";
                foreach (Node rec in this.selectedNodes)
                {
                    if (rec.note != null)
                    {
                        copytext = copytext + rec.note;

                        if (this.selectedNodes.Count() > 1)
                        { //separate nodes
                            copytext = copytext + "\n";
                        }
                    }
                }

                if (copytext != "")
                {
                    Clipboard.SetText(copytext);
                }
            }

            return true;
        }

        // NODE evaluate masth expression
        public bool EvaluateExpression()
        {
            if (this.selectedNodes.Count() == 1)
            {
                string expression = this.selectedNodes[0].name;
                string expressionResult = "";

                if (Regex.IsMatch(expression, @"^\d+$"))
                {
                    expression = expression + "+1";
                }

                Evaluator ev = new Evaluator();
                expressionResult = ev.Evaluate(expression);

                if (expressionResult != "")
                {
                    Node newrec = this.CreateNode(this.GetMousePosition());
                    newrec.setName(expressionResult);
                    newrec.color.Set("#8AC5FF");

                    this.diagram.InvalidateDiagram();
                }


                return true;
            }
            else
            if (this.selectedNodes.Count() > 1)  // SUM sum nodes with numbers
            {
                float sum = 0;
                Match match = null;
                foreach (Node rec in this.selectedNodes)
                {
                    match = Regex.Match(rec.name, @"([-]{0,1}\d+[\.,]{0,1}\d*)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        sum = sum + float.Parse(match.Groups[1].Value.Replace(",", "."), CultureInfo.InvariantCulture);
                    }
                }

                Node newrec = this.CreateNode(this.GetMousePosition());
                newrec.setName(sum.ToString());
                newrec.color.Set("#8AC5FF");

                this.diagram.InvalidateDiagram();
                return true;
            }

            return false;
        }

        // NODE evaluate date
        public bool EvaluateDate()
        {
            bool insertdate = true;
            string insertdatestring = "";

            if (this.selectedNodes.Count() > 0)
            {
                bool aretimes = true;
                foreach (Node rec in this.selectedNodes) // Loop through List with foreach
                {
                    if (!Regex.Match(rec.name, @"^[0-9]{2}:[0-9]{2}:[0-9]{2}$", RegexOptions.IgnoreCase).Success)
                    {
                        aretimes = false;
                        break;
                    }
                }

                if (aretimes) // sum dates
                {
                    try
                    {
                        TimeSpan timesum = TimeSpan.Parse("00:00:00");
                        foreach (Node rec in this.selectedNodes)
                        {
                            timesum = timesum.Add(TimeSpan.Parse(rec.name));
                            insertdate = false;
                        }
                        insertdatestring = timesum.ToString();
                        insertdate = false;
                    }
                    catch (Exception ex)
                    {
                        Program.log.write("time span error: " + ex.Message);
                    }
                }
                else
                // count difference between two dates
				
                if (this.selectedNodes.Count() == 2)
                {
                    try
                    {
						DateTime d1 = Converter.ToDateAndTime(this.selectedNodes[0].name);
						DateTime d2 = Converter.ToDateAndTime(this.selectedNodes[1].name);
                        insertdatestring = ((d1 < d2) ? d2 - d1 : d1 - d2).ToString();
                        insertdate = false;
                    }
                    catch (Exception ex)
                    {
                        Program.log.write("time diff error: " + ex.Message);
                    }
                }
            }

            if (insertdate) // insert date
            {
                DateTime dt = DateTime.Now;
                insertdatestring =
                    dt.Year + "-" +
                    ((dt.Month < 10) ? "0" : "") + dt.Month + "-" +
                    ((dt.Day < 10) ? "0" : "") + dt.Day + " " +
                    ((dt.Hour < 10) ? "0" : "") + dt.Hour + ":" +
                    ((dt.Minute < 10) ? "0" : "") + dt.Minute + ":" +
                    ((dt.Second < 10) ? "0" : "") + dt.Second;
            }

            Node newrec = this.CreateNode(this.GetMousePosition());
            newrec.setName(insertdatestring);
            newrec.color.Set("#8AC5FF");

            this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
            this.diagram.InvalidateDiagram();
            return true;
        }

        // NODE promote
        public bool Promote()
        {
            if (this.selectedNodes.Count() == 1)
            {
                Node selectedNode = this.selectedNodes[0];
                Node newrec = this.CreateNode(this.GetMousePosition());
                newrec.copyNode(selectedNode, true, true);

                string expression = newrec.name;
                string[] days = { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
                int dayPosition = Array.IndexOf(days, newrec.name);

                var matchesFloat = Regex.Matches(expression, @"(\d+(?:\.\d+)?)");
                var matchesDate = Regex.Matches(expression, @"^(\d{4}-\d{2}-\d{2})$");

                if (dayPosition != -1)
                { //get next day
                    dayPosition += 1;
                    if (dayPosition == 7)
                    {
                        dayPosition = 0;
                    }

                    newrec.setName(days[dayPosition]);
                }
                else if (matchesDate.Count > 0) // add day to date
                {
					DateTime theDate = Converter.ToDate(expression);                    
                    theDate = theDate.AddDays(1);
                    string dateValue = matchesFloat[0].Groups[1].Value;
					string newnDateValue = Converter.DateToString(theDate);
                    newrec.setName(newnDateValue);
                }
                else if (matchesFloat.Count > 0) //add to number
                {
                    string number = matchesFloat[0].Groups[1].Value;
                    string newnumber = (float.Parse(number) + 1).ToString();
                    newrec.setName(expression.Replace(number, newnumber));
                }

                this.diagram.InvalidateDiagram();
                return true;

            }
            return true;
        }

        // NODE random
        public void Random()
        {
            Node node = this.CreateNode(this.GetMousePosition(), true);
            node.setName(Encrypt.GetRandomString());

            this.diagram.Unsave("create", node, this.shift, this.currentLayer.id);
            this.diagram.InvalidateDiagram();
        }

        // NODE hide background
        public bool HideBackground()
        {
            if (this.selectedNodes.Count > 0)
            {
                this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);

                //first all hide then show
                bool allHidden = true;
                foreach (Node rec in this.selectedNodes)
                {
                    if (!rec.transparent)
                    {
                        allHidden = false;
                        break;
                    }
                }

                bool someEmptyUnHidden = false;
                foreach (Node rec in this.selectedNodes)
                {
                    if (!rec.transparent && rec.name == "")
                    {
                        someEmptyUnHidden = true;
                        break;
                    }
                }

                if (someEmptyUnHidden)
                {
                    foreach (Node rec in this.selectedNodes)
                    {

                        if (!rec.transparent && rec.name == "")
                        {
                            rec.transparent = true;
                        }
                    }
                }
                else
                {
                    foreach (Node rec in this.selectedNodes)
                    {

                        if (allHidden)
                        {
                            rec.transparent = true;
                        }
                        else
                        {
                            rec.transparent = false;
                        }

                        rec.transparent = !rec.transparent;
                    }
                }

                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }

            return true;
        }

        // NODE open edit form
        public void Edit()
        {
            if (this.selectedNodes.Count() == 1)
            {
                Node rec = this.selectedNodes[0];
                this.diagram.EditNode(rec);
            }
        }

        // NODE rename
        public void Rename()
        {
            if (this.selectedNodes.Count() == 1)
            {
                Node rec = this.selectedNodes[0];
                Position position = new Position(this.shift).Add(rec.position).Split(this.scale);
                this.editPanel.editNode(position, this.selectedNodes[0]);
            }
        }

        // NODE edit link
        public void EditLink()
        {
            if (this.selectedNodes.Count() == 1)
            {
                Node rec = this.selectedNodes[0];
                Position position = new Position(
                    (int)((this.shift.x + rec.position.x) / this.scale),
                    (int)((this.shift.y + rec.position.y) / this.scale)
                );
                this.editLinkPanel.editNode(position, this.selectedNodes[0]);
            }
        }

        // NODE move nodes to foreground
        public void MoveNodesToForeground()
        {
            if (this.selectedNodes.Count() > 0)
            {
                foreach (Node rec in this.selectedNodes)
                {
                    this.diagram.layers.MoveToForeground(rec);
                }
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE move nodes to background
        public void MoveNodesToBackground()
        {
            if (this.selectedNodes.Count() > 0)
            {
                foreach(Node rec in this.selectedNodes)
                {
                    this.diagram.layers.MoveToBackground(rec);
                }
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE protect sesitive data in node name
        public void ProtectNodes()
        {
            if (this.selectedNodes.Count() > 0)
            {
                bool allProtected = true;
                foreach (Node rec in this.selectedNodes)
                {
                    if (rec.protect == false) {
                        allProtected = false;
                        break;
                    }
                }

                foreach (Node rec in this.selectedNodes)
                {
                    rec.setProtect(!allProtected);
                }

                this.diagram.InvalidateDiagram();
            }
        }

        // NODE move nodes to left
        public void MoveNodesToLeft(bool quick = false)
        {
            if (this.selectedNodes.Count() > 0)
            {
                if (!this.diagram.undoOperations.isSame("move", this.selectedNodes, null))
                {
                    this.diagram.undoOperations.add("move", this.selectedNodes, null, this.shift, this.currentLayer.id);
                }
                int speed = (quick) ? this.diagram.options.keyArrowFastMoveNodeSpeed : this.diagram.options.keyArrowSlowMoveNodeSpeed;
                foreach (Node rec in this.selectedNodes)
                {
                    rec.position.x -= speed;
                }
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
            else // MOVE SCREEN
            {
                int speed = (quick) ? this.ClientSize.Width : this.diagram.options.keyArrowSlowSpeed;
                this.shift.x = this.shift.x + speed;
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE move nodes to right
        public void MoveNodesToRight(bool quick = false)
        {
            if (this.selectedNodes.Count() > 0)
            {
                if (!this.diagram.undoOperations.isSame("move", this.selectedNodes, null))
                {
                    this.diagram.undoOperations.add("move", this.selectedNodes, null, this.shift, this.currentLayer.id);
                }
                int speed = (quick) ? this.diagram.options.keyArrowFastMoveNodeSpeed : this.diagram.options.keyArrowSlowMoveNodeSpeed;
                foreach (Node rec in this.selectedNodes)
                {
                    rec.position.x += speed;
                }
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
            else // MOVE SCREEN
            {
                int speed = (quick) ? this.ClientSize.Width : this.diagram.options.keyArrowSlowSpeed;
                this.shift.x = this.shift.x - speed;
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE move nodes to up
        public void MoveNodesUp(bool quick = false)
        {
            if (this.selectedNodes.Count() > 0)
            {
                if (!this.diagram.undoOperations.isSame("move", this.selectedNodes, null))
                {
                    this.diagram.undoOperations.add("move", this.selectedNodes, null, this.shift, this.currentLayer.id);
                }
                int speed = (quick) ? this.diagram.options.keyArrowFastMoveNodeSpeed : this.diagram.options.keyArrowSlowMoveNodeSpeed;
                foreach (Node rec in this.selectedNodes)
                {
                    rec.position.y -= speed;
                }
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
            else // MOVE SCREEN
            {
                int speed = (quick) ? this.ClientSize.Height : this.diagram.options.keyArrowSlowSpeed;
                this.shift.y = this.shift.y + speed;
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE move nodes to up
        public void MoveNodesDown(bool quick = false)
        {
            if (this.selectedNodes.Count() > 0)
            {
                if (!this.diagram.undoOperations.isSame("move", this.selectedNodes, null))
                {
                    this.diagram.undoOperations.add("move", this.selectedNodes, null, this.shift, this.currentLayer.id);
                }
                int speed = (quick) ? this.diagram.options.keyArrowFastMoveNodeSpeed : this.diagram.options.keyArrowSlowMoveNodeSpeed;
                foreach (Node rec in this.selectedNodes)
                {
                    rec.position.y += speed;
                }
                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
            else // MOVE SCREEN
            {
                int speed = (quick) ? this.ClientSize.Height : this.diagram.options.keyArrowSlowSpeed;
                this.shift.y = this.shift.y - speed;
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE node is editet with edit panel
        public bool IsEditing()
        {
            if (this.editPanel.isEditing()
                || this.editLinkPanel.isEditing())
            {
                return true;
            }

            return false;
        }

        // NODE node is editet with edit panel
        public void CancelEditing()
        {
            if (this.editPanel.isEditing())
            {
                this.editPanel.closePanel();
            }

            if (this.editLinkPanel.isEditing())
            {
                this.editPanel.closePanel();
            }
        }

        // NODE remove attachment from nodes
        public bool HasSelectionAttachment()
        {
            if (this.selectedNodes.Count() > 0)
            {
                foreach (Node node in this.selectedNodes)
                {
                    if (node.attachment != "") {
                        return true;
                    }
                }
            }

            return false;
        }

        // NODE save attachment from diagram to system
        public void AttachmentDeploy()
        {
            if (this.HasSelectionAttachment())
            {
                if (this.DSelectDirectoryAttachment.ShowDialog() == DialogResult.OK)
                {
                    foreach (Node node in this.selectedNodes)
                    {
                        if (node.attachment != "")
                        {
                            Compress.decompress(node.attachment, this.DSelectDirectoryAttachment.SelectedPath);
                        }
                    }
                }
            }
        }

        // NODE add file to diagram as attachment
        public void AttachmentAddFile(Position position)
        {
            if (this.DSelectFileAttachment.ShowDialog() == DialogResult.OK)
            {
                string data = Compress.compress(this.DSelectFileAttachment.FileName);

                if (this.selectedNodes.Count() > 0)
                {
                    this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                    foreach (Node node in this.selectedNodes)
                    {
                        node.attachment = data;
                    }
                    this.diagram.Unsave();
                }
                else
                {
                    Node newrec = this.CreateNode(position, true);
                    newrec.attachment = data;
                    newrec.color.Set(diagram.options.colorAttachment);
                    newrec.setName(Os.GetFileName(this.DSelectFileAttachment.FileName));
                    this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                }

                this.diagram.InvalidateDiagram();
            }
        }

        // NODE add directory to diagram as attachment
        public void AttachmentAddDirectory(Position position)
        {
            if (this.DSelectDirectoryAttachment.ShowDialog() == DialogResult.OK)
            {
                string data = Compress.compress(this.DSelectDirectoryAttachment.SelectedPath);

                if (this.selectedNodes.Count() > 0)
                {
                    this.diagram.undoOperations.add("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);
                    foreach (Node node in this.selectedNodes)
                    {
                        node.attachment = data;
                    }
                    this.diagram.Unsave();
                }
                else
                {
                    Node newrec = this.CreateNode(position, true);
                    newrec.attachment = data;
                    newrec.color.Set(diagram.options.colorAttachment);
                    newrec.setName(Os.GetFileName(this.DSelectDirectoryAttachment.SelectedPath));
                    this.diagram.Unsave("create", newrec, this.shift, this.currentLayer.id);
                }

                this.diagram.InvalidateDiagram();
            }
        }

        // NODE remove attachment from nodes
        public void AttachmentRemove()
        {
            if (this.selectedNodes.Count() > 0)
            {
                foreach (Node node in this.selectedNodes)
                {
                    node.attachment = "";
                }
            }
        }

        // NODE get lines which are connected with current selected nodes
        public Lines GetSelectedLines()
        {
            Lines SelectedLinesTemp = new Lines();
            int id = 0;

            foreach (Node srec in this.selectedNodes) {
                id = srec.id;
                foreach (Line lin in this.diagram.layers.GetAllLines()) {
                    if (lin.start == id) {
                        SelectedLinesTemp.Add(lin);
                    }
                }
            }

            Lines SelectedLines = new Lines();

            foreach (Line lin in SelectedLinesTemp)
            {
                id = lin.end;
                foreach (Node srec in this.selectedNodes)
                {
                    if (id == srec.id)
                    {
                        SelectedLines.Add(lin);
                    }
                }
            }

            return SelectedLines;
        }

        // NODE MARK nodes for navigation history
        public void SwitchMarkForSelectedNodes()
        {
            if (this.selectedNodes.Count > 0)
            {
                this.diagram.Unsave("edit", this.selectedNodes, null, this.shift, this.currentLayer.id);

                bool found = false;
                foreach (Node node in this.selectedNodes)
                {
                    if (node.mark)
                    {
                        found = true;
                    }
                }

                if (found)
                {
                    this.UnMarkSelectedNodes();
                }
                else
                {
                    this.MarkSelectedNodes();
                }

                this.diagram.Unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE MARK nodes for navigation history
        public void MarkSelectedNodes()
        {
            foreach (Node node in this.selectedNodes)
            {
                node.mark = true;
            }
        }

        // NODE MARK unmark nodes for navigation history
        public void UnMarkSelectedNodes()
        {
            foreach (Node node in this.selectedNodes)
            {
                node.mark = false;
            }
        }

        // NODE MARK find next marked node
        public void NextMarkedNode()
        {
            Nodes nodes = this.diagram.GetAllNodes();
            Nodes markedNodes = new Nodes();

            // get all marked nodes
            foreach (Node node in nodes)
            {
                if (node.mark) {
                    markedNodes.Add(node);
                }
            }

            // no marked node found
            if (markedNodes.Count == 0)
            {
                return;
            }

            // find last marked node
            if (lastMarkNode == 0)
            {
                lastMarkNode = markedNodes[0].id;
                this.GoToNode(markedNodes[0]);
                this.diagram.InvalidateDiagram();
                return;
            }

            //lastMarkNode position in markedNodes
            bool found = false;
            int i = 0;
            for (i = 0; i < markedNodes.Count; i++)
            {
                if (lastMarkNode == markedNodes[i].id)
                {
                    found = true;
                    break;
                }
            }

            //node is not longer marked then start from beginning
            if (!found)
            {
                lastMarkNode = markedNodes[0].id;
                this.GoToNode(markedNodes[0]);
                this.diagram.InvalidateDiagram();
                return;
            }

            // find next node
            if (i < (markedNodes.Count - 1))
            {
                lastMarkNode = markedNodes[i + 1].id;
                this.GoToNode(markedNodes[i + 1]);
                this.diagram.InvalidateDiagram();
                return;
            }

            // go to fisrt node
            lastMarkNode = markedNodes[0].id;
            this.GoToNode(markedNodes[0]);
            this.diagram.InvalidateDiagram();
        }

        // NODE MARK find prev marked node
        public void PrevMarkedNode()
        {
            Nodes nodes = this.diagram.GetAllNodes();
            Nodes markedNodes = new Nodes();

            // get all marked nodes
            foreach (Node node in nodes)
            {
                if (node.mark)
                {
                    markedNodes.Add(node);
                }
            }

            // no marked node found
            if (markedNodes.Count == 0)
            {
                return;
            }

            // find last marked node
            if (lastMarkNode == 0)
            {
                lastMarkNode = markedNodes[0].id;
                this.GoToNode(markedNodes[0]);
                this.diagram.InvalidateDiagram();
                return;
            }

            //lastMarkNode position in markedNodes
            bool found = false;
            int i = 0;
            for (i = 0; i < markedNodes.Count; i++)
            {
                if (lastMarkNode == markedNodes[i].id)
                {
                    found = true;
                    break;
                }
            }

            //node is not longer marked then start from beginning
            if (!found)
            {
                lastMarkNode = markedNodes[0].id;
                this.GoToNode(markedNodes[0]);
                this.diagram.InvalidateDiagram();
                return;
            }

            // find prev node
            if (0 < i)
            {
                lastMarkNode = markedNodes[i - 1].id;
                this.GoToNode(markedNodes[i - 1]);
                this.diagram.InvalidateDiagram();
                return;
            }

            // go to last node
            lastMarkNode = markedNodes[markedNodes.Count - 1].id;
            this.GoToNode(markedNodes[markedNodes.Count - 1]);
            this.diagram.InvalidateDiagram();
        }

        // NODE LINK NAME get link name from web
        public void SetNodeNameByLink(Node node, string url)
        {
            // get page title async in thread
            Job.doJob(
                new DoWorkEventHandler(
                    delegate (object o, DoWorkEventArgs args)
                    {
                        node.setName(
                            Network.GetWebPageTitle(
                                url,
                                this.main.options.proxy_uri,
                                this.main.options.proxy_password,
                                this.main.options.proxy_username
                            )
                        );

                    }
                ),
                new RunWorkerCompletedEventHandler(
                    delegate (object o, RunWorkerCompletedEventArgs args)
                    {
                        if (node.name == null) node.setName("url");
                        node.color.Set("#F2FFCC");
                        this.diagram.InvalidateDiagram();
                    }
                )
            );
        }

        // LINE change color of lines
        public void ChangeLineColor()
        {
            if (!this.diagram.options.readOnly)
            {
                if (this.selectedNodes.Count() > 0)
                {
                    Lines SelectedLines = GetSelectedLines();

                    DColor.Color = SelectedLines[0].color.color;

                    if (DColor.ShowDialog() == DialogResult.OK)
                    {
                        if (!this.diagram.undoOperations.isSame("changeLineColor", null, SelectedLines))
                        {
                            this.diagram.Unsave("changeLineColor", null, SelectedLines, this.shift, this.currentLayer.id);
                        }

                        foreach (Line lin in SelectedLines)
                        {
                            lin.color.Set(DColor.Color);
                        }

                        this.diagram.InvalidateDiagram();
                    }
                }
            }
        }

        // LINE change line width
        public void ChangeLineWidth()
        {
            if (!this.diagram.options.readOnly)
            {
                if (this.selectedNodes.Count() > 0)
                {
                    Lines SelectedLines = GetSelectedLines();
                    lineWidthForm.setValue(SelectedLines[0].width); // set trackbar to first selected line width
                    lineWidthForm.ShowDialog();

                }
            }
        }

        // LINE resize line with event called by line width form
        public void ResizeLineWidth(int width = 1)
        {
            if (!this.diagram.options.readOnly)
            {
                if (this.selectedNodes.Count() > 0)
                {
                    Lines SelectedLines = GetSelectedLines();

                    if (!this.diagram.undoOperations.isSame("changeLineWidth", null, SelectedLines))
                    {
                        this.diagram.Unsave("changeLineWidth", null, SelectedLines, this.shift, this.currentLayer.id);
                    }

                    foreach (Line lin in SelectedLines)
                    {
                        lin.width = width;
                    }

                    this.diagram.InvalidateDiagram();
                }
            }
        }

        /*************************************************************************************************************************/

        // SCRIPT evaluate python script in nodes signed with stamp in node link [F9]
        private void Evaluate()
        {
            Nodes nodes = null;

            if (this.selectedNodes.Count() > 0) {
                nodes = new Nodes(this.selectedNodes);
            } else {
                nodes = new Nodes(this.diagram.GetAllNodes());
            }
            // remove nodes whit link other then [ ! | eval | evaluate | !#num_order | eval#num_order |  evaluate#num_order]
            // higest number is executed first
            Regex regex = new Regex(@"^\s*(eval(uate)|!){1}(#\w+){0,1}\s*$");
            nodes.RemoveAll(n => !regex.Match(n.link).Success);

            nodes.OrderByLink();
            nodes.Reverse();

            String clipboard = Os.GetTextFormClipboard();

#if !DEBUG
            var worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };

            worker.DoWork += (sender, e) =>
            {
#endif
            this.Evaluate(nodes, clipboard);
#if !DEBUG
            };
            worker.RunWorkerAsync();
#endif

        }

        // SCRIPT evaluate python script in node name or in node note in nodes
        private void Evaluate(Nodes nodes, string clipboard = "")
        {
            Program.log.write("diagram: openlink: run macro");
            Script script = new Script();
            script.SetDiagram(this.diagram);
            script.SetDiagramView(this);
            script.SetClipboard(clipboard);
            string body = "";

            foreach (Node node in nodes)
            {
                body = node.note.Trim() != "" ? node.note : node.name;
                script.RunScript(body);
            }
        }

        // SCRIPT evaluate python script in node name or in node note in node
        private void Evaluate(Node node, string clipboard = "")
        {
            // run macro
            Program.log.write("diagram: openlink: run macro");
            Script script = new Script();
            script.SetDiagram(this.diagram);
            script.SetDiagramView(this);
            script.SetClipboard(clipboard);
            string body = node.note.Trim() != "" ? node.note : node.name;
            script.RunScript(body);
        }

        /*************************************************************************************************************************/

        // MOVE TIMER Go to node position
        public void GoToNodeWithAnimation(Node node)
        {
            if (node != null)
            {
                if (node.layer != this.currentLayer.id) // if node is in different layer then move instantly
                {
                    this.GoToNode(node);
                }
                else
                {
                    this.animationTimer.Enabled = true;

                    this.animationTimerSpeed.Set(this.shift.Clone().Invert())
                        .Subtract(node.position)
                        .Add(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);

                    double distance = this.animationTimerSpeed.Size();
                    this.animationTimerCounter = (distance > 1000) ? 10 : 30;

                    this.animationTimerSpeed
                        .Split(this.animationTimerCounter);

                }

                this.SelectOnlyOneNode(node);
            }
        }

        // MOVE TIMER Go to node position
        public void AnimationTimer_Tick(object sender, EventArgs e)
        {

#if DEBUG
            this.LogEvent("animationTimer");
#endif
            this.shift.Add(this.animationTimerSpeed);

            if (--this.animationTimerCounter <= 0) {
                this.animationTimer.Enabled = false;
            }

            this.diagram.InvalidateDiagram();
        }

        // ZOOM TIMER zoom animation
        public void ZoomTimer_Tick(object sender, EventArgs e)
        {

#if DEBUG
            this.LogEvent("zoomTimer");
#endif
            if (zoomTimerScale > this.scale) // zoom in
            {
                this.scale += zoomTimerStep;

                if (zoomTimerScale < this.scale) // prevent infinite animation
                {
                    this.scale = zoomTimerScale;
                    this.zoomTimer.Enabled = false;
                }

                this.diagram.InvalidateDiagram();
            }
            else
            if (zoomTimerScale < this.scale) // zoom out
            {
                this.scale -= zoomTimerStep;

                if (zoomTimerScale > this.scale) // prevent infinite animation
                {
                    this.scale = zoomTimerScale;
                    this.zoomTimer.Enabled = false;
                }

                this.diagram.InvalidateDiagram();
            }
            else
            {
                this.zoomTimer.Enabled = false; // prevent infinite animation
            }

            // border
            if (this.scale < 0.1f) {
                this.scale = 0.1f;
                this.zoomTimer.Enabled = false;
            }

            // border
            if (this.scale > 7)
            {
                this.scale = 7;
                this.zoomTimer.Enabled = false;
            }
        }

        /*************************************************************************************************************************/
#if DEBUG

        // DEBUG log event to output console and prevent duplicate events display
        public void LogEvent(string lastEvetMessage = "")
        {
            if (this.lastEvent != lastEvetMessage)
            {
                Debug.WriteLine(lastEvetMessage);
                this.lastEvent = lastEvetMessage;
            }
        }
#endif
    }
}
