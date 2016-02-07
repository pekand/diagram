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
using NCalc;

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

        // BREADCRUMBS
        public Breadcrumbs breadcrumbs = null;

        // MOVETIMER
        Timer moveTimer = new Timer(); // timer pre animaciu
        Position moveTimerSpeed = new Position();
        int moveTimerCounter = 0;

        // LINEWIDTHFORM
        LineWidthForm lineWidthForm = new LineWidthForm();

        // COLORPICKERFORM
        ColorPickerForm colorPickerForm = new ColorPickerForm();

        // COMPONENTS
        private IContainer components;
        private void InitializeComponent()
        {
            this.components = new Container();
            this.DSave = new SaveFileDialog();
            this.DOpen = new OpenFileDialog();
            this.DColor = new ColorDialog();
            this.DFontColor = new ColorDialog();
            this.DFont = new FontDialog();
            this.DImage = new OpenFileDialog();
            this.MoveTimer = new Timer(this.components);
            this.defaultfontDialog = new FontDialog();
            this.exportFile = new SaveFileDialog();
            this.saveTextFileDialog = new SaveFileDialog();
            this.DSelectDirectoryAttachment = new FolderBrowserDialog();
            this.DSelectFileAttachment = new OpenFileDialog();
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
            this.DFont.Color = SystemColors.ControlText;
            //
            // DImage
            //
            this.DImage.Filter = "All|*.bmp;*.jpg;*.jpeg;*.png;*.ico|Bmp|*.bmp|Jpg|*.jpg;*.jpeg|Png|*.png|Ico|*.ico";
            //
            // MoveTimer
            //
            this.MoveTimer.Interval = 5;
            this.MoveTimer.Tick += new EventHandler(this.MoveTimer_Tick);
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
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            this.ClientSize = new Size(383, 341);
            this.DoubleBuffered = true;
            this.Icon = global::Diagram.Properties.Resources.ico_diagram;
            this.KeyPreview = true;
            this.Name = "DiagramView";
            this.Text = "Diagram";
            this.Activated += new EventHandler(this.DiagramView_Activated);
            this.Deactivate += new EventHandler(this.DiagramApp_Deactivate);
            this.FormClosing += new FormClosingEventHandler(this.DiagramApp_FormClosing);
            this.FormClosed += new FormClosedEventHandler(this.DiagramView_FormClosed);
            this.Load += new EventHandler(this.DiagramViewLoad);
            this.Paint += new PaintEventHandler(this.DiagramApp_Paint);
            this.KeyDown += new KeyEventHandler(this.DiagramApp_KeyDown);
            this.KeyPress += new KeyPressEventHandler(this.DiagramApp_KeyPress);
            this.KeyUp += new KeyEventHandler(this.DiagramApp_KeyUp);
            this.MouseDoubleClick += new MouseEventHandler(this.DiagramApp_MouseDoubleClick);
            this.MouseDown += new MouseEventHandler(this.DiagramApp_MouseDown);
            this.MouseMove += new MouseEventHandler(this.DiagramApp_MouseMove);
            this.MouseUp += new MouseEventHandler(this.DiagramApp_MouseUp);
            this.Resize += new EventHandler(this.DiagramApp_Resize);
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
            this.currentLayer = this.diagram.layers.getLayer(0);
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
            this.moveTimer.Tick += new EventHandler(moveTimerTick);
            this.moveTimer.Interval = 10;
            this.moveTimer.Enabled = false;

            // lineWidthForm
            this.lineWidthForm.trackbarStateChanged += this.resizeLineWidth;

            //colorPickerForm
            this.colorPickerForm.changeColor += this.changeColor;
        }

        // FORM Load event -
        public void DiagramViewLoad(object sender, EventArgs e)
        {
            
            // Preddefinovana pozicia okna
            if (this.diagram.options.restoreWindow)
            {
                this.Left = this.diagram.options.Left;
                this.Top = this.diagram.options.Top;
                this.Width = this.diagram.options.Width;
                this.Height = this.diagram.options.Height;
                this.setWindowsStateCode(this.diagram.options.WindowState);
            }
            else
            {
                this.Left = 50;
                this.Top = 40;
                this.Width = Media.screenWidth(this) - 100;
                this.Height = Media.screenHeight(this) - 100;
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

            bottomScrollBar.OnChangePosition += new PositionChangeEventHandler(positionChangeBottom);
            rightScrollBar.OnChangePosition += new PositionChangeEventHandler(positionChangeRight);

            // set startup position
            if (this.parentView != null)
            {
                this.shift.set(this.parentView.shift);
                this.goToLayer(this.parentView.currentLayer.id);
                this.Width = this.parentView.Width;
                this.Height = this.parentView.Height;
                this.Top = this.parentView.Top;
                this.Left = this.parentView.Left;
                this.WindowState = this.parentView.WindowState;
            }
            else
            {
                this.goToLayer(this.diagram.options.homeLayer);
                this.shift.set(diagram.options.homePosition);
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
                    var res = MessageBox.Show(main.translations.saveBeforeExit, main.translations.confirmExit, MessageBoxButtons.YesNoCancel);
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
                    var res = MessageBox.Show(main.translations.saveBeforeExit, main.translations.confirmExit, MessageBoxButtons.YesNoCancel);
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

            e.Cancel = !close;

        }

        // FORM Title - set windows title
        public void SetTitle()
        {
            if (this.diagram.FileName.Trim() != "")
                this.Text = Os.getFileNameWithoutExtension(this.diagram.FileName);
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
            this.shift.set(diagram.options.homePosition);
            this.goToLayer(diagram.options.homeLayer);
            this.diagram.InvalidateDiagram();
        }

        // FORM open view and go to home position
        public void OpenViewAndGoToHome()
        {
            DiagramView child = this.diagram.openDiagramView(this);
            child.GoToHome();
            child.Invalidate();
        }

        // FORM set home position
        public void setCurentPositionAsHomePosition()
        {
            diagram.options.homePosition.x = this.shift.x;
            diagram.options.homePosition.y = this.shift.y;
            diagram.options.homeLayer = this.currentLayer.id;
            this.diagram.unsave();
        }

        // FORM go to end position - center window to second remembered position
        public void GoToEnd()
        {
            this.shift.set(diagram.options.endPosition);
            this.goToLayer(diagram.options.endLayer);
            this.diagram.InvalidateDiagram();
        }

        // FORM open view and go to home position
        public void OpenViewAndGoToEnd()
        {
            DiagramView child = this.diagram.openDiagramView(this);
            child.GoToEnd();
            child.Invalidate();
        }

        // FORM set end position
        public void setCurentPositionAsEndPosition()
        {
            diagram.options.endPosition.x = this.shift.x;
            diagram.options.endPosition.y = this.shift.y;
            diagram.options.endLayer = this.currentLayer.id;
            this.diagram.unsave();
        }

        // FORM cursor position
        public Position cursorPosition()
        {
            Point ptCursor = Cursor.Position;
            ptCursor = PointToClient(ptCursor);
            return new Position(ptCursor.X, ptCursor.Y);
        }

        // FORM hide
        public bool formHide()
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
        public void rememberPosition(bool state = true)
        {
            this.diagram.options.restoreWindow = state;

            this.diagram.options.Left = this.Left;
            this.diagram.options.Top = this.Top;
            this.diagram.options.Width = this.Width;
            this.diagram.options.Height = this.Height;

            this.diagram.options.WindowState = this.getWindowsStateCode();
        }

        // FORM get window state role
        public int getWindowsStateCode()
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
        public void setWindowsStateCode(int code = 0)
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

        /*************************************************************************************************************************/

        // SELECTION check if node is in current window selecton
        public bool isSelected(Node a)
        {
            return a.selected;
        }

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
        public void selectAll()
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
        public void DiagramApp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.stateDblclick = true;
        }

        // EVENT Mouse Down                                                                            // [MOUSE] [DOWN] [EVENT]
        public void DiagramApp_MouseDown(object sender, MouseEventArgs e)
        {
            this.actualMousePos.set(e.X, e.Y);

            if (this.stateSearching)
            {
                this.stateSearching = false;
                this.searhPanel.HidePanel();
            }

			this.Focus();

            if (this.isEditing())
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

            this.startMousePos.set(this.actualMousePos);  // starting mouse position
            this.startShift.set(this.shift);  // starting indent

            if (e.Button == MouseButtons.Left)
            {
                this.sourceNode = this.findNodeInMousePosition(new Position(e.X, e.Y));

                this.stateSourceNodeAlreadySelected = this.sourceNode != null && this.sourceNode.selected;

                if (bottomScrollBar != null && bottomScrollBar.MouseDown(e.X, e.Y))
                {
                    moveScreenHorizontal(bottomScrollBar.position);
                    this.diagram.InvalidateDiagram();
                    return;
                }
                else
                if (rightScrollBar != null && rightScrollBar.MouseDown(e.X, e.Y))
                {
                    moveScreenVertical(rightScrollBar.position);
                    this.diagram.InvalidateDiagram();
                    return;
                }
                else
                if (this.sourceNode == null)
                {
                    if (!this.diagram.options.readOnly
                        && (this.keyctrl || this.keyalt)
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
                    else
                    if (!this.diagram.options.readOnly && !this.stateDblclick)  //informations for draging
                    {
                        this.stateDragSelection = true;
                        MoveTimer.Enabled = true;
                        this.startNodePos.set(this.sourceNode.position); // starting position of draging item

                        this.vmouse
                            .set(this.actualMousePos)
                            .scale(this.scale)
                            .subtract(this.shift)
                            .subtract(this.sourceNode.position); // mouse position in node

                        if (!this.keyctrl && !this.isSelected(this.sourceNode))
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
                    this.stateAddingNode = true;// add node by drag
                    MoveTimer.Enabled = true;
                }
            }

            this.diagram.InvalidateDiagram();
        }

        // EVENT Mouse move                                                                            // [MOUSE] [MOVE] [EVENT]
        public void DiagramApp_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (this.stateSelectingNodes || this.stateAddingNode)
            {
                this.actualMousePos.set(e.X, e.Y);
                this.diagram.InvalidateDiagram();
            }
            else
            if (this.stateDragSelection || this.stateAddingNode || this.stateSelectingNodes) // posunutie objektu
            {
                this.actualMousePos.set(e.X, e.Y);
            }
            else
            if (this.stateMoveView) // posunutie obrazovky
            {
                this.shift.x = (int)(this.startShift.x + (e.X - this.startMousePos.x) * this.scale);
                this.shift.y = (int)(this.startShift.y + (e.Y - this.startMousePos.y) * this.scale);
                this.diagram.InvalidateDiagram();
            }
            else
            if (bottomScrollBar != null && rightScrollBar!= null && (bottomScrollBar.MouseMove(e.X, e.Y) || rightScrollBar.MouseMove(e.X, e.Y))) //SCROLLBARS
            {
                bottomScrollBar.setPosition(getPositionHorizontal());
                rightScrollBar.setPosition(getPositionVertical());
                this.diagram.InvalidateDiagram();
            }
        }

        // EVENT Mouse Up                                                                              // [MOUSE] [UP] [EVENT]
        public void DiagramApp_MouseUp(object sender, MouseEventArgs e)
        {
            this.actualMousePos.set(e.X, e.Y);
            Position mouseTranslation = new Position(this.actualMousePos).subtract(this.startMousePos);

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
                    if (this.sourceNode != null) // return node to starting position after connection is created
                    {
                        Position translation = new Position(this.startNodePos)
                            .subtract(sourceNode.position);

                        if (this.selectedNodes.Count > 0)
                        {
                            foreach (Node node in this.selectedNodes)
                            {
                                node.position.add(translation);
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
                        .scale(this.scale)
                        .add(this.shift)
                        .subtract(this.startShift);

                    Position b = new Position(this.actualMousePos)
                        .scale(this.scale);

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

            Node TargetNode = this.findNodeInMousePosition(new Position(e.X, e.Y));

            if (buttonleft) // MLEFT
            {

                if (bottomScrollBar != null
                    && rightScrollBar!=null
                    && (bottomScrollBar.MouseUp() || rightScrollBar.MouseUp()))
                {
                    this.diagram.InvalidateDiagram();
                }else
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
                // KEY DRAG+CTRL copy of node
                if (!isreadonly
                    && keyctrl
                    && TargetNode == null
                    && this.sourceNode != null)
                {
                    this.SelectNodes(
                        this.diagram.AddDiagramPart(
                            this.diagram.GetDiagramPart(this.selectedNodes),
                            this.getMousePosition().clone().scale(this.scale).subtract(this.shift),
                            this.currentLayer.id
                        )
                    );
                }
                else
                // KEY DRAG+ALT create node and conect with existing node
                if (!isreadonly
                    && keyalt
                    && TargetNode == null
                    && this.sourceNode != null)
                {
                    var s = this.sourceNode;
                    var node = this.CreateNode(new Position(e.X, e.Y));
                    node.shortcut = s.id;
                    this.diagram.Connect(s, node);
                    this.diagram.unsave("create", node);
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+ALT create shortcut beetwen objects
                if (!isreadonly
                    && keyalt
                    && TargetNode != null
                    && this.sourceNode != null
                    && TargetNode != this.sourceNode)
                {
                    this.sourceNode.shortcut = TargetNode.id;
                    this.diagram.unsave();
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
                            && this.isSelected(TargetNode)
                        )
                        || (TargetNode != null && this.sourceNode == TargetNode)
                    )
                    && Math.Sqrt(mouseTranslation.x* mouseTranslation.x + mouseTranslation.y * mouseTranslation.y) > 5
                )
                {
                    Position vector = new Position(this.actualMousePos)
                        .scale(this.scale)
                        .subtract(this.vmouse)
                        .subtract(this.shift)
                        .subtract(this.sourceNode.position);

                    if (this.selectedNodes.Count > 0)
                    {
                        this.diagram.undo.add("edit", this.selectedNodes);

                        foreach (Node node in this.selectedNodes)
                        {
                            node.position.add(vector);
                        }
                    }

                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+CTRL create node and conect with existing node
                if (!isreadonly
                    && keyctrl
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

                    this.diagram.unsave("create", node, line);
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+ALT create node and make shortcut to target node
                if (!isreadonly
                    && keyalt
                    && !keyctrl
                    && !keyshift
                    && TargetNode != null
                    && this.sourceNode == null)
                {
                    Node newrec = this.CreateNode(
                        new Position(this.shift).subtract(startShift).add(this.startMousePos)
                    );

                    newrec.shortcut = TargetNode.id;
                    this.diagram.unsave("create", newrec);
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DBLCLICK open link or edit window after double click on node [dblclick]
                if (dblclick
                    && this.sourceNode != null
                    && !keyctrl
                    && !keyalt
                    && !keyshift)
                {
                    this.OpenLinkAsync(this.sourceNode);
                }
                else
                // KEY DBLCLICK+SHIFT open node edit form
                if (dblclick
                    && this.sourceNode != null
                    && !keyctrl
                    && !keyalt
                    && keyshift)
                {
                    this.diagram.EditNode(this.sourceNode);
                }
                else
                // KEY DBLCLICK+CTRL open link in node
                if (dblclick
                    && this.sourceNode != null
                    && keyctrl
                    && !keyalt
                    && !keyshift)
                {
                    if (this.sourceNode.link!="")
                    {
                        Os.openPathInSystem(this.sourceNode.link);
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
                        .subtract(
                            this.actualMousePos
                            .clone()
                            .scale(this.scale)
                            )
                        .add(
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
                        newrec = this.CreateNode(this.actualMousePos.clone().subtract(10), false);
                        newNodes.Add(newrec);
                    }

                    Lines newLines = new Lines();
                    foreach (Node rec in this.selectedNodes)
                    {
                        Line line = this.diagram.Connect(rec, newrec);
                        newLines.Add(line);
                    }

                    this.SelectOnlyOneNode(newrec);
                    this.diagram.unsave("create", newNodes, newLines); 
                }
                else
                // KEY CTRL+MLEFT
                // KEY DBLCLICK create new node
                if (!isreadonly
                    && (dblclick || keyctrl)
                    && TargetNode == null
                    && this.sourceNode == null
                    && e.X == this.startMousePos.x
                    && e.Y == this.startMousePos.y)
                {
                    Node newNode = this.CreateNode(new Position(e.X - 10, e.Y - 10), false);
                    this.diagram.unsave("create", newNode);
                }
                else
                // KEY DRAG+CTRL copy style from node to other node
                if (!isreadonly
                    && !keyshift
                    && keyctrl
                    && TargetNode != null
                    && this.sourceNode != null
                    && this.sourceNode != TargetNode)
                {
                    if (this.selectedNodes.Count() > 1)
                    {
                        this.diagram.undo.add("edit", this.selectedNodes);
                        foreach (Node rec in this.selectedNodes)
                        {
                            rec.color = TargetNode.color;
                            rec.font = TargetNode.font;
                            rec.fontcolor = TargetNode.fontcolor;
                            rec.transparent = TargetNode.transparent;
                            rec.resize();
                        }
                    }

                    if (this.selectedNodes.Count() == 1
                        || (this.selectedNodes.Count() == 0 && this.sourceNode != null))
                    {
                        this.diagram.undo.add("edit", TargetNode);

                        TargetNode.color.set(this.sourceNode.color);
                        TargetNode.font = this.sourceNode.font;
                        TargetNode.fontcolor.set(this.sourceNode.fontcolor);
                        TargetNode.transparent = this.sourceNode.transparent;
                        TargetNode.resize();

                        if (this.selectedNodes.Count() == 1 && this.selectedNodes[0] != this.sourceNode)
                        {
                            this.ClearSelection();
                            this.SelectNode(this.sourceNode);
                        }
                    }
                    this.diagram.unsave();
                }
                else
                // KEY DRAG make link between two nodes
                if (!isreadonly
                    && TargetNode != null
                    && this.sourceNode != null
                    && this.sourceNode != TargetNode)
                {
                    bool arrow = false;
                    if (keyshift)
                    {
                        arrow = true;
                    }

                    if (this.selectedNodes.Count() > 0)
                    {
                        foreach (Node rec in this.selectedNodes)
                        {
                            if (rec != TargetNode)
                            {
                                if (keyctrl)
                                {
                                    this.diagram.Connect(TargetNode, rec, arrow, null);
                                }
                                else
                                {
                                    this.diagram.Connect(rec, TargetNode, arrow, null);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.diagram.Connect(sourceNode, TargetNode, arrow, null);
                    }

                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                // KEY CTRL+MLEFT add node to selected nodes
                else
                if (keyctrl
                    && !keyshift
                    && this.sourceNode == TargetNode
                    && TargetNode != null
                    && !this.isSelected(TargetNode))
                {
                    this.SelectNode(TargetNode);
                    this.diagram.InvalidateDiagram();
                }
                // KEY CLICK+CTRL remove node from selected nodes
                else
                if (keyctrl
                    && TargetNode != null
                    && (this.sourceNode == TargetNode || this.isSelected(TargetNode)))
                {
                    this.RemoveNodeFromSelection(TargetNode);
                    this.diagram.InvalidateDiagram();
                }
                else
                if (this.sourceNode == TargetNode
                    && this.stateSourceNodeAlreadySelected)
                {
                    this.rename();
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
                    Node temp = this.findNodeInMousePosition(new Position(e.X, e.Y));

                    if (this.sourceNode != temp && !this.isSelected(temp))
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
                // KEY DRAG+MMIDDLE create new node and conect with existing node
                if (!isreadonly && TargetNode != null)
                {
                    this.diagram.Connect(
                        this.CreateNode(
                            (new Position(this.shift)).subtract(this.startShift).add(this.startMousePos)
                        ),
                        TargetNode
                    );

                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
            }

            this.stateDblclick = false;
            this.stateDragSelection = false;
            this.stateAddingNode = false;
            this.stateSelectingNodes = false;
        }

        // EVENT Mouse Whell
        public void DiagramApp_MouseWheel(object sender, MouseEventArgs e)                             // [MOUSE] [WHELL] [EVENT]
        {
            //throw new NotImplementedException();
            if (e.Delta > 0) // MWHELL
            {
                if (this.keyctrl)
                {
                    if (this.scale < 7) //up
                    {
                        int a = (int)((this.shift.x - (this.ClientSize.Width / 2 * this.scale)));
                        int b = (int)((this.shift.y - (this.ClientSize.Height / 2 * this.scale)));
                        if (this.scale >= 1)
                            this.scale = this.scale + 1;
                        else
                            if (this.scale < 1)
                                this.scale = this.scale + 0.1f;

                        this.shift.x = (int)((a + (this.ClientSize.Width / 2 * this.scale)));
                        this.shift.y = (int)((b + (this.ClientSize.Height / 2 * this.scale)));
                    }

                    if (this.scale > 7) this.scale = 7;
                }
                else
                if (this.keyshift)
                {
                    this.shift.x += (int)(50 * this.scale);
                }
                else
                {
                    this.shift.y += (int)(50 * this.scale);
                }
                this.diagram.InvalidateDiagram();
            }
            else
            {
                if (this.keyctrl)
                {
                    if (this.scale > 0) // down
                    {
                        int a = (int)((this.shift.x - (this.ClientSize.Width / 2 * this.scale)));
                        int b = (int)((this.shift.y - (this.ClientSize.Height / 2 * this.scale)));
                        if (this.scale>1)
                            this.scale = this.scale - 1;
                        else
                        if (this.scale > 0.1f)
                            this.scale = this.scale - 0.1f;

                        this.shift.x = (int)((a + (this.ClientSize.Width / 2 * this.scale)));
                        this.shift.y = (int)((b + (this.ClientSize.Height / 2 * this.scale)));


                    }

                    if (this.scale < 0.1) this.scale = 0.1f;
                }
                else
                if (this.keyshift)
                {
                    this.shift.x -= (int)(50 * this.scale);
                }
                else
                {
                    this.shift.y -= (int)(50 * this.scale);
                }
                this.diagram.InvalidateDiagram();
            }
        }

        // EVENT Shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)                           // [KEYBOARD] [EVENT]
        {
            if (this.isEditing() || this.stateSearching)
            {
                return false;
            }

            /*
             * [doc] order : ProcessCmdKey, DiagramApp_KeyDown, DiagramApp_KeyPress, DiagramApp_KeyUp;
             */

            if (KeyMap.parseKey(KeyMap.selectAllElements, keyData) ) // [KEY] [CTRL+A] select all
            {
                this.selectAll();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignToLine, keyData)) // [KEY] [CTRL+L] align to line
            {
                this.alignToLine();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignToColumn, keyData)) // [KEY] [CTRL+H] align to column
            {
                this.alignToColumn();
                return true;
            }


            if (KeyMap.parseKey(KeyMap.alignToGroup, keyData)) // [KEY] [CTRL+K] align to group
            {
                this.alignToGroup();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignToLineGroup, keyData)) // [KEY] [CTRL+SHIFT+K] align to group
            {
                this.alignToLineGroup();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.copy, keyData))  // [KEY] [CTRL+C]
            {
                this.copy();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.copyLinks, keyData))  // [KEY] [CTRL+SHIFT+C] copy links from selected nodes
            {
                this.copyLink();
                return true;
            }

			if (KeyMap.parseKey(KeyMap.copyNotes, keyData))  // [KEY] [CTRL+ALT+SHIFT+C] copy notes from selected nodes
			{
                this.copyNote();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.cut, keyData))  // [KEY] [CTRL+X] Cut diagram
            {
                this.cut();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.paste, keyData))  // [KEY] [CTRL+V] [PASTE] Paste text from clipboard
            {
                Point ptCursor = Cursor.Position;
                ptCursor = PointToClient(ptCursor);
                this.paste(new Position(ptCursor.X, ptCursor.Y));
                return true;
            }

            if (KeyMap.parseKey(KeyMap.pasteToNote, keyData))  // [KEY] [CTRL+SHIFT+V] paste to note
            {
                this.pasteToNote();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.pasteToLink, keyData))  // [KEY] [CTRL+SHIFT+INS] paste to node link
            {
                this.pasteToLink();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.undo, keyData))  // [KEY] [CTRL+Z]
            {
                this.diagram.doUndo();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.redo, keyData))  // [KEY] [CTRL+SHIFT+Z]
            {
                this.diagram.doRedo();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.newDiagram, keyData))  // [KEY] [CTRL+N] New Diagram
            {
                main.OpenDiagram();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.newDiagramView, keyData))  // [KEY] [F7] New Diagram view
            {
                this.diagram.openDiagramView(this);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.save, keyData))  // [KEY] [CTRL+S] save diagram
            {
                this.save();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.open, keyData))  // [KEY] [CTRL+O] open diagram dialog window
            {
                this.open();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.search, keyData))  // [KEY] [CTRL+F] Search form
            {
                this.showSearchPanel();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.evaluateExpression, keyData))  // [KEY] [CTRL+G] Evaluate expresion
            {
                this.evaluateExpression();
            }

            if (KeyMap.parseKey(KeyMap.date, keyData))  // [KEY] [CTRL+D] date
            {
                this.evaluateDate();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.promote, keyData)) // [KEY] [CTRL+P] Promote node
            {
                this.promote();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.random, keyData)) // [KEY] [CTRL+R] Random generator
            {
                this.random();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.hideBackground, keyData)) // [KEY] [F3] Hide background
            {
                this.hideBackground();
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
                this.setCurentPositionAsHomePosition();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.end, keyData)) // KEY [END] go to end position
            {
                this.GoToEnd();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.setEnd, keyData))  // [KEY] [SHIFT+END] Move end point
            {
                this.setCurentPositionAsEndPosition();
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
                openLinkDirectory();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.console, keyData)) // [KEY] [F12] show Debug console
            {
                this.showConsole();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveNodeUp, keyData)) // KEY [CTRL+PAGEUP] move node up to foreground
            {
                this.moveNodesToForeground();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveNodeDown, keyData)) // [KEY] [CTRL+PAGEDOWN] move node down to background
            {
                this.moveNodesToBackground();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.pageUp, keyData)) // [KEY] [PAGEUP] change current position in diagram view
            {
                this.pageUp();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.pageDown, keyData)) // [KEY] [PAGEDOWN] change current position in diagram view
            {
                this.pageDown();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.editNodeName, keyData)) // [KEY] [F2] edit node name
            {
                this.rename();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.editNodeLink, keyData)) // [KEY] [F4] edit node name
            {
                this.editLink();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.evaluateNodes, keyData)) // [KEY] [F9] evaluate python script for selected nodes by stamp in link
            {
                this.evaluate();
                return true;
            }


            if (KeyMap.parseKey(KeyMap.openEditForm, keyData)) // [KEY] [CTRL+E] open edit form
            {
                this.edit();
                return true;
            }

            // prevent catch keys while node is creating or renaming
            if (this.isEditing())
            {
                return false;
            }

            if (KeyMap.parseKey(KeyMap.editOrLayerIn, keyData)) // [KEY] [ENTER] open edit form or layer in
            {
                this.layerInOrEdit();
                return true;
            }


            if (KeyMap.parseKey(KeyMap.layerIn, keyData)) // [KEY] [PLUS] Layer in
            {
                this.layerIn();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.layerOut, keyData) || KeyMap.parseKey(KeyMap.layerOut2, keyData)) // [KEY] [BACK] or [MINUS] Layer out
            {
                this.LayerOut();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.minimalize, keyData)) // [KEY] [ESC] minimalize diagram view
            {
                if (this.moveTimer.Enabled)
                {
                    this.moveTimer.Enabled = false; // stop move animation if exist
                }
                else
                {
                    return this.formHide();
                }
            }

            if (KeyMap.parseKey(KeyMap.delete, keyData)) // [KEY] [DELETE] delete
            {
                this.DeleteSelectedNodes(this);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveLeft, keyData) || KeyMap.parseKey(KeyMap.moveLeftFast, keyData))  // [KEY] [left] [SHIFT+LEFT] [ARROW] Move node
            {

                this.moveNodesToLeft(keyData == Keys.Left);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveRight, keyData) || KeyMap.parseKey(KeyMap.moveRightFast, keyData))  // [KEY] [right] [SHIFT+RIGHT] [ARROW] Move node
            {
                this.moveNodesToRight(keyData == Keys.Right);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveUp, keyData) || KeyMap.parseKey(KeyMap.moveUpFast, keyData))  // [KEY] [up] [SHIFT+UP] [ARROW] Move node
            {
                this.moveNodesUp(keyData == Keys.Up);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.moveDown, keyData) || KeyMap.parseKey(KeyMap.moveDownFast, keyData))  // [KEY] [down] [SHIFT+DOWN] [ARROW] Move node
            {
                this.moveNodesDown(keyData == Keys.Down);
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignLeft, keyData)) // [KEY] [TAB] align selected nodes to left
            {
                this.alignLeft();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignRight, keyData))  // [KEY] [SHIFT+TAB] align selected nodes to right
            {
                this.alignRight();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.resetZoom, keyData))  // [KEY] [CTRL+0] reset zoom level to default
            {
                this.resetZoom();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // EVENT Key down
        public void DiagramApp_KeyDown(object sender, KeyEventArgs e)                                  // [KEYBOARD] [DOWN] [EVENT]
        {
            if (this.isEditing() || this.stateSearching)
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

            if (this.isEditing())
            {
                return;
            }

            if (e.KeyCode == Keys.Space && !this.stateZooming) // KEY SPACE
            {
                this.stateSelectingNodes = false;
                MoveTimer.Enabled = false;

                this.stateZooming = true;
                Position tmp = new Position(this.shift);

                tmp.add(
                    (int)(-(this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / this.scale) * this.scale),
                    (int)(-(this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / this.scale) * this.scale)
                );

                this.currentScale = this.scale;
                this.scale = this.zoomingScale;

                tmp.add(
                    (int)(+(this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / this.scale) * this.scale),
                    (int)(+(this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / this.scale) * this.scale)
                );

                this.shift.set(tmp);

                this.diagram.InvalidateDiagram();
            }

        }

        // EVENT Key up
        public void DiagramApp_KeyUp(object sender, KeyEventArgs e)
        {
            this.keyshift = false;
            this.keyctrl = false;
            this.keyalt = false;

            if (this.isEditing() || this.stateSearching)
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

                shift.add(
                    (int)(-(this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / this.scale) * this.scale),
                    (int)(-(this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / this.scale) * this.scale)
                );

                this.scale = this.currentScale;

                shift.add(
                    (int)(+(this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / this.scale) * this.scale),
                    (int)(+(this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / this.scale) * this.scale)
                );

                this.diagram.InvalidateDiagram();
            }
        }                                 // [KEYBOARD] [UP] [EVENT]

        // EVENT Keypress
        public void DiagramApp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.isEditing() || this.stateSearching)
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
                    this.editPanel.showEditPanel(this.cursorPosition(), this.key);
                }
            }
        }                         // [KEYBOARD] [PRESS] [EVENT]

        // EVENT DROP file
        public void DiagramApp_DragDrop(object sender, DragEventArgs e)                                // [DROP] [EVENT]
        {
            try
            {
                Nodes newNodes = new Nodes();

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    Node newrec = this.CreateNode(this.getMousePosition());
                    newNodes.Add(newrec);
                    newrec.setName(Os.getFileName(file));

    				newrec.link = file;
    				if (Os.DirectoryExists(file)) // directory
                    {
    					newrec.link = Os.makeRelative(file, this.diagram.FileName);
                        newrec.color.set(Media.getColor(diagram.options.colorDirectory));
                    }
    				else
    				if (Os.Exists(file))
                    {
                        newrec.color.set(Media.getColor(diagram.options.colorFile));

                        if (this.diagram.FileName != "" && Os.FileExists(this.diagram.FileName)) // DROP FILE - skratenie cesty k suboru
                        {
                            newrec.link = Os.makeRelative(file, this.diagram.FileName);
                        }

                        string ext = "";
                        ext = Os.getExtension(file).ToLower();

                        if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp") // DROP IMAGE skratenie cesty k suboru
                        {
                            newrec.isimage = true;
                            newrec.imagepath = Os.makeRelative(file, this.diagram.FileName);
                            newrec.image = new Bitmap(file);
                            if (ext != ".ico") newrec.image.MakeTransparent(Color.White);
                            newrec.height = newrec.image.Height;
                            newrec.width = newrec.image.Width;
                        }

    					#if !MONO
                        if (ext == ".exe")// [EXECUTABLE] [DROP] [ICON] extract icon
                        {
                            try
                            {
                                Icon ico = Icon.ExtractAssociatedIcon(file);
                                newrec.isimage = true;
                                newrec.embeddedimage = true;
                                newrec.image = ico.ToBitmap();
                                newrec.image.MakeTransparent(Color.White);
                                newrec.height = newrec.image.Height;
                                newrec.width = newrec.image.Width;
                            }
                            catch (Exception ex)
                            {
                                Program.log.write("extract icon from exe error: " + ex.Message);
                            }
                        }
    					#endif

                        #if !MONO
                        if (ext == ".lnk") // [LINK] [DROP] extract target
                        {
                            try
                            {
                                newrec.link = Os.GetShortcutTargetFile(file);

                                // ak je odkaz a odkazuje na exe subor pokusit sa extrahovat ikonu
                                if (Os.FileExists(newrec.link) && Os.getExtension(newrec.link) == ".exe")// extract icon
                                {
                                    Icon ico = Icon.ExtractAssociatedIcon(newrec.link);
                                    newrec.isimage = true;
                                    newrec.embeddedimage = true;
                                    newrec.image = ico.ToBitmap();
                                    newrec.image.MakeTransparent(Color.White);
                                    newrec.height = newrec.image.Height;
                                    newrec.width = newrec.image.Width;
                                }

                            }
                            catch (Exception ex)
                            {
                                Program.log.write("extract icon from lnk error: " + ex.Message);
                            }
                        }
                        #endif

                    }
                }

                this.diagram.unsave("create", newNodes);
                
            } catch (Exception ex) {
                Program.log.write("drop file goes wrong: error: " + ex.Message);
            }
        }

        // EVENT DROP drag enter
        public void DiagramApp_DragEnter(object sender, DragEventArgs e)                               // [DRAG] [EVENT]
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        // EVENT Resize
        public void DiagramApp_Resize(object sender, EventArgs e)                                      // [RESIZE] [EVENT]
        {
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
                    if (this.sourceNode != null)
                    {
                        Position vector = new Position();

                        // calculate shift between start node position and current sourceNode position
                        vector
                            .set(this.actualMousePos)
                            .scale(this.scale)
                            .subtract(this.vmouse)
                            .subtract(this.shift)
                            .subtract(this.sourceNode.position);

                        if (this.selectedNodes.Count > 0)
                        {
                            foreach (Node node in this.selectedNodes)
                            {
                                node.position.add(vector);
                            }
                        }

                        changed = true;
                    }
                }

                if (changed) this.diagram.InvalidateDiagram();
            }
        }                                      // [MOVE] [TIMER] [EVENT]

        // EVENT Deactivate - lost focus
        public void DiagramApp_Deactivate(object sender, EventArgs e)                              // [FOCUS]
        {
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
                this.firstLayereShift.set(shift);
            }
            else
            {
                this.currentLayer.parentNode.layerShift.set(this.shift);
            }

            this.currentLayer = this.diagram.layers.createLayer(node);
            this.currentLayer.parentNode.haslayer = true;
            this.layersHistory.Add(this.currentLayer);
            this.shift.set(this.currentLayer.parentNode.layerShift);

            this.SetTitle();
            this.breadcrumbs.Update();
            this.diagram.InvalidateDiagram();
        }

        // LAYER OUT
        public void LayerOut()
        {
            if (this.currentLayer.parentLayer != null) { //this layer is not top layer

                this.currentLayer.parentNode.layerShift.set(this.shift);

                if (this.currentLayer.nodes.Count() == 0) {
                    this.currentLayer.parentNode.haslayer = false;
                }

                this.currentLayer = this.currentLayer.parentLayer;

                layersHistory.RemoveAt(layersHistory.Count() - 1);

                if (this.currentLayer.parentNode == null)
                {
                    this.shift.set(this.firstLayereShift);
                }
                else
                {
                    this.shift.set(this.currentLayer.parentNode.layerShift);
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

            this.currentLayer = this.diagram.layers.getLayer(id);

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
        public bool isNodeInLayerHistory(Node rec) {
            foreach (Layer layer in this.layersHistory)
            {
                if (layer.id == rec.id) {
                    return true;
                }
            }

            return false;
        }

        // LAYER layer in
        public void layerIn()
        {
            if (this.selectedNodes.Count() == 1)
            {
                this.LayerIn(this.selectedNodes[0]);
            }
        }

        // LAYER layer in or open edit form
        public void layerInOrEdit()
        {
            if (this.selectedNodes.Count() == 1)
            {
                if (this.selectedNodes[0].haslayer)
                {
                    this.LayerIn(this.selectedNodes[0]);
                }
                else
                {
                    this.diagram.EditNode(this.selectedNodes[0]);
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

            foreach (Node node in this.diagram.getAllNodes())
            {
                if (node.note.ToUpper().IndexOf(searchFor.ToUpper()) != -1
                    || node.name.ToUpper().IndexOf(searchFor.ToUpper()) != -1)
                {
                    foundNodes.Add(node);
                }
            }

            this.searhPanel.highlight(foundNodes.Count() == 0);

            Position middle = new Position();
            middle.copy(this.currentPosition);

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

                Node parent = this.diagram.layers.getLayer(first.layer).parentNode;
                Position m = (currentLayerId == first.layer) ? middle : (parent != null) ? parent.layerShift : firstLayereShift;
                double d1 = first.position.convertToStandard().distance(m);
                double d2 = second.position.convertToStandard().distance(m);

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
                this.goToNodeWithAnimation(node);
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
                this.goToNodeWithAnimation(node);
                this.SelectOnlyOneNode(node);
                this.diagram.InvalidateDiagram();
            }
        }

        // SEARCHPANEL SHOW
        private void showSearchPanel()
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
            this.goToLayer(currentPositionLayer);
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
        public void copyLinkToClipboard(Node node)
        {
            Clipboard.SetText(node.link);
        }

        /*************************************************************************************************************************/

        // SCROLLBAR MOVE LEFT-RIGHT set current position in view with relative (0-1) number          // SCROLLBAR              
        public void moveScreenHorizontal(float per)                                                
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
        public float getPositionHorizontal()
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
        public void moveScreenVertical(float per)
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
        public float getPositionVertical()
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
        public void positionChangeBottom(object source, PositionEventArgs e)
        {
            moveScreenHorizontal(e.GetPosition());
        }

        // SCROLLBAR EVENT position is changed by vertical scrollbar 
        public void positionChangeRight(object source, PositionEventArgs e)
        {
            moveScreenVertical(e.GetPosition());
        }

        /*************************************************************************************************************************/

        // FILE Save - Save diagram
        public void save()
        {
            if (!this.diagram.save())
            {
                this.saveas();
            }
        }

        // FILE SAVEAS - Save as diagram
        public bool saveas()
        {
            if (this.DSave.ShowDialog() == DialogResult.OK)
            {
                this.diagram.saveas(this.DSave.FileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        // FILE Open - Open diagram dialog
        public void open()
        {
            if (DOpen.ShowDialog() == DialogResult.OK)
            {
                if (Os.FileExists(DOpen.FileName))
                {
                    if (Os.getExtension(DOpen.FileName).ToLower() == ".diagram")
                    {
                        main.OpenDiagram(DOpen.FileName);

                        if (this.diagram.isNew())
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show(main.translations.wrongFileExtenson);
                    }
                }
            }
        }

        // FILE Open diagram directory if diagram is already saved
        public void openDiagramDirectory()
        {
            if (!this.diagram.NewFile && Os.FileExists(this.diagram.FileName))
            {
                Os.openDirectory(Os.getDirectoryName(this.diagram.FileName));
            }
        }

        /*************************************************************************************************************************/

        // EXPORT Export diagram to png
        public void exportDiagramToPng()
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
                this.DrawDiagram(g, new Position(this.shift).invert().subtract(minx, miny), true);
                g.Dispose();
                bmp.Save(exportFile.FileName, ImageFormat.Png);
                bmp.Dispose();
            }
        }

        // EXPORT Export diagram to txt
        public void exportDiagramToTxt(string filePath)
        {

            Nodes nodes = null;

            if (this.selectedNodes.Count == 0)
            {
                nodes = this.diagram.getAllNodes();
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
            Os.writeAllText(filePath, outtext);
        }

        /*************************************************************************************************************************/

        // DRAW                                                                                      // [DRAW]
        void DrawDiagram(Graphics gfx, Position correction = null, bool export = false)
        {
            gfx.SmoothingMode = SmoothingMode.AntiAlias;

            if (this.diagram.options.grid && !export)
            {
                this.DrawGrid(gfx);
            }

            this.DrawLines(gfx, correction, export);

            // DRAW addingnode
            if (!export && this.stateAddingNode && !this.stateZooming && (this.actualMousePos.x != this.startMousePos.x || this.actualMousePos.y != this.startMousePos.y))
            {
                this.DrawAddNode(gfx);
            }

            this.DrawNodes(gfx, correction, export);

            // DRAW select - select nodes by mouse drag (blue rectangle - multiselect)
            if (!export && this.stateSelectingNodes && (this.actualMousePos.x != this.startMousePos.x || this.actualMousePos.y != this.startMousePos.y))
            {
                this.DrawSelectNodes(gfx);
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
        void DrawGrid(Graphics gfx)
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
        void DrawMiniScreen(Graphics gfx)
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
        void DrawCoordinates(Graphics gfx)
        {
            float s = this.scale;

            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            gfx.DrawString(
                        (this.shift.x).ToString() + "," +
                        this.shift.y.ToString() +
                        " (" + this.ClientSize.Width.ToString() + "x" + this.ClientSize.Height.ToString() + ") " +
                        "scl:" + s.ToString() + "," + this.currentScale.ToString(),
                        drawFont, drawBrush, 10, 10);
        }

        // DRAW select node by mouse drag (blue rectangle)
        void DrawSelectNodes(Graphics gfx)
        {

            int a = (int)(+this.shift.x - this.startShift.x + this.startMousePos.x * this.scale);
            int b = (int)(+this.shift.y - this.startShift.y + this.startMousePos.y * this.scale);
            int c = (int)(this.actualMousePos.x * this.scale);
            int d = (int)(this.actualMousePos.y * this.scale);
            int temp;
            if (c < a) { temp = a; a = c; c = temp; }
            if (d < b) { temp = d; d = b; b = temp; }

            gfx.FillRectangle(new SolidBrush(Color.FromArgb(100, 10, 200, 200)), new Rectangle((int)(a / this.scale), (int)(b / this.scale), (int)((c - a) / this.scale), (int)((d - b) / this.scale)));
        }

        // DRAW add new node by drag
        void DrawAddNode(Graphics gfx)
        {
            Pen myPen = new Pen(Color.Black, 1);

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

        // DRAW nodes
        void DrawNodes(Graphics gfx, Position correction = null, bool export = false)
        {
            bool isvisible = false; // drawonly visible elements
            float s = this.scale;

            Pen myPen1 = new Pen(Color.Black, 1);
            Pen myPen2 = new Pen(Color.Black, 3);

            // fix position for image file export
            int cx = 0;
            int cy = 0;
            if (correction != null)
            {
                cx = correction.x;
                cy = correction.y;
            }

            // DRAW nodes
            foreach (Node rec in this.currentLayer.nodes) // Loop through List with foreach
            {
                if (rec.layer == this.currentLayer.id || rec.id == this.currentLayer.id)
                {
                    // vylucenie moznosti ktore netreba vykreslovat
                    isvisible = false;
                    if (export && this.currentLayer.id == rec.layer)
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
                                    myPen2,
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
                            if (this.diagram.options.coordinates)
                            {
                                Font drawFont = new Font("Arial", 10 / s);
                                SolidBrush drawBrush = new SolidBrush(Color.Black);
                                gfx.DrawString((rec.position.x).ToString() + "," + (rec.position.y).ToString(), drawFont, drawBrush, (this.shift.x + rec.position.x) / s, (this.shift.y + rec.position.y - 20) / s);
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
                                    if (this.diagram.options.borders) gfx.DrawEllipse(myPen1, rect1);
                                }

                                if (rec.haslayer && !export) // draw layer indicator
                                {
                                    gfx.DrawEllipse(myPen1, new Rectangle(
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
                                        myPen2,
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
                                    if (this.diagram.options.borders) gfx.DrawRectangle(myPen1, rect1);
                                }

                                // draw layer indicator
                                if (rec.haslayer && !export)
                                {
                                    gfx.DrawRectangle(
                                        myPen1,
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
                                        myPen2,
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
        }

        // DRAW lines
        void DrawLines(Graphics gfx, Position correction = null, bool export = false)
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
            foreach (Line lin in this.diagram.getAllLines()) // Loop through List with foreach
            {

                Node r1 = lin.startNode;
                Node r2 = lin.endNode;
                if (lin.layer == this.currentLayer.id)
                {
                    isvisible = false;
                    if (export && (this.currentLayer.id == lin.startNode.layer || this.currentLayer.id == lin.endNode.layer))
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
        }

        /*************************************************************************************************************************/

        // DIAGRAM Set model
        public void setDiagram(Diagram diagram)
        {
            this.diagram = diagram;
        }

        // DIAGRAM Get model
        public Diagram getDiagram()
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
            this.Invalidate();
        }

        // VIEW FOCUS
        public void setFocus()
        {
            //diagram bring to top hack in windows
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;

            this.Focus();
        }

        // VIEW page up
        public void pageUp()
        {
            this.shift.y = this.shift.y + this.ClientSize.Height;
            this.diagram.InvalidateDiagram();
        }

        // VIEW page down
        public void pageDown()
        {
            this.shift.y = this.shift.y - this.ClientSize.Height;
            this.diagram.InvalidateDiagram();
        }

        // VIEW reset zoom
        public void resetZoom()
        {
            this.currentScale = this.zoomingDefaultScale;
            this.scale = this.zoomingDefaultScale;
            this.diagram.InvalidateDiagram();
        }

        // VIEW get mouse position
        public Position getMousePosition()
        {
            Point ptCursor = Cursor.Position;
            ptCursor = this.PointToClient(ptCursor);
            return new Position(ptCursor.X, ptCursor.Y);
        }

        /*************************************************************************************************************************/

        // NODE create
        public Node CreateNode(Position position, bool SelectAfterCreate = true)
        {
            var rec = this.diagram.createNode(
                position.clone().scale(this.scale).subtract(this.shift),
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
        public void addNodeAfterNode()
        {
            if (this.selectedNodes.Count() == 1)
            {
                if (!this.editPanel.Visible)
                {
                    this.editPanel.prevSelectedNode = this.selectedNodes[0];
                    this.editPanel.showEditPanel(
                        this.selectedNodes[0]
                            .position
                            .clone()
                            .add(this.shift)
                            .add(this.selectedNodes[0].width + 10, 0),
                        ' ',
                        false
                    );
                }
            }
        }

        // NODE create below
        public void addNodeBelowNode()
        {
            if (this.selectedNodes.Count() == 1)
            {
                if (!this.editPanel.Visible)
                {
                    this.editPanel.prevSelectedNode = this.selectedNodes[0];
                    this.editPanel.showEditPanel(
                        this.selectedNodes[0]
                            .position
                            .clone()
                            .add(this.shift)
                            .add(0, this.selectedNodes[0].height + 10),
                        ' ',
                        false
                    );
                }
            }
        }

        // NODE open link directory
        public void openLinkDirectory()
        {
            if (this.selectedNodes.Count() > 0)
            {
                foreach (Node node in this.selectedNodes)
                {
                    if (node.link.Trim().Length > 0)
                    {
                        if (Os.DirectoryExists(node.link)) // open directory of selected nods
                        {
                            Os.openDirectory(node.link);
                        }
                        else
                        if (Os.FileExists(node.link)) // open directory of selected files
                        {
                            string parent_diectory = Os.getFileDirectory(this.selectedNodes[0].link);
                            Os.openDirectory(parent_diectory);
                        }
                    }
                }
            }
            else
            {
                this.openDiagramDirectory(); // open main directory of diagram
            }
        }

        // NODES DELETE SELECTION
        public void DeleteSelectedNodes(DiagramView DiagramView)
        {
            if (!this.diagram.options.readOnly)
            {
                if (DiagramView.selectedNodes.Count() > 0)
                {
                    this.diagram.DeleteNodes(DiagramView.selectedNodes);
                }
            }
        }

        // NODE Go to node position
        public void goToNode(Node rec)
        {
            if (rec != null)
            {
                this.goToPosition(rec.position);
                this.goToLayer(rec.layer);
            }
        }

        // NODE Go to position
        public void goToPosition(Position position)
        {
            if (position != null)
            {
                this.shift.x = (int)(-position.x + this.ClientSize.Width / 2 * this.scale);
                this.shift.y = (int)(-position.y + this.ClientSize.Height / 2 * this.scale);
            }
        }

        // NODE Go to node layer
        public void goToLayer(int layer = 0)
        {
            this.currentLayer = this.diagram.layers.getLayer(layer);
            this.BuildLayerHistory(layer);
        }

        // NODE find node in mouse cursor position
        public Node findNodeInMousePosition(Position position)
        {
            return this.diagram.findNodeInPosition(
                new Position(
                    (int)(position.x * this.scale - this.shift.x),
                    (int)(position.y * this.scale - this.shift.y)
                ),
                this.currentLayer.id
            );
        }

        // NODE Open Link
        public void OpenLinkAsync(Node rec)
        {
            Program.log.write("diagram: openlink: run worker");
            String clipboard = Os.getTextFormClipboard();


#if DEBUG
            var result = 0;
            result = this.OpenLink(rec, clipboard);
            if ((int)result == 1)
            {
                this.diagram.EditNode(rec);
            }
#else
            var worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += (sender, e) =>
            {
                e.Result = this.OpenLink(rec, clipboard);
            };
            worker.RunWorkerCompleted += (sender, e) =>
            {

                var result = e.Result;
                if ((int)result == 1)
                {
                    this.diagram.EditNode(rec);
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
                    this.SelectOnlyOneNode(rec);
                    this.attachmentDeploy();
                }
                else
                if (rec.shortcut > 0) // GO TO LINK
                {
                    Node target = this.diagram.GetNodeByID(rec.shortcut);
                    this.goToNode(target);
                    this.diagram.InvalidateDiagram();
                }
                else
                if (rec.link.Length > 0)
                {

                    string fileName = "";
                    string searchString = "";

                    // node with link "script" is executed as script
                    if (!Network.isURL(rec.link)
                        && Patterns.hasHastag(rec.link.Trim(), ref fileName, ref searchString)
                        && Os.FileExists(Os.normalizedFullPath(fileName)))       // OPEN FILE ON POSITION
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
                                searchString = Os.fndLineNumber(fileName, searchString).ToString();
                            }

                            // get external editor path from global configuration saved in user configuration directory
                            String editFileCmd = this.main.options.texteditor;
                            editFileCmd = editFileCmd.Replace("%FILENAME%", Os.normalizedFullPath(fileName));
                            editFileCmd = editFileCmd.Replace("%LINE%", searchString);

                            Program.log.write("diagram: openlink: open file on position " + editFileCmd);
                            Os.runCommand(editFileCmd);
                        }
                        catch (Exception ex)
                        {
                            Program.log.write("open link as file error: " + ex.Message);
                        }
                    }
                    else if (rec.link.Trim() == "script" || rec.link.Trim() == "macro" || rec.link.Trim() == "$")  // OPEN SCRIPT
                    {
                        this.evaluate(rec, clipboard);
                    }
                    else if (Os.DirectoryExists(rec.link))  // OPEN DIRECTORY
                    {
                        try
                        {
                            Program.log.write("diagram: openlink: open directory " + Os.normalizePath(rec.link));
                            Os.runProcess(rec.link);
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
                            if (Os.isDiagram(rec.link))
                            {
                                Program.log.write("diagram: openlink: open diagram " + Os.normalizePath(rec.link));
                                Os.openDiagram(rec.link);
                            }
                            else
                            {
                                Program.log.write("diagram: openlink: open file " + Os.normalizePath(rec.link));
                                Os.runProcess(rec.link);
                            }
                        }
                        catch (Exception ex)
                        {
                            Program.log.write("open link as file error: " + ex.Message);
                        }
                    }
                    else if (Network.isURL(rec.link)) // OPEN URL
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
                    else // run as command
                    {

                        // set current directory to current diagrm file destination
                        if (Os.FileExists(this.diagram.FileName))
                        {
                            Os.setCurrentDirectory(Os.getFileDirectory(this.diagram.FileName));
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
                        .Replace("%DIRECTORY%", Os.getFileDirectory(this.diagram.FileName));

                        Program.log.write("diagram: openlink: run command: " + cmd);
                        Os.runCommand(cmd, Os.getFileDirectory(this.diagram.FileName)); // RUN COMMAND
                    }
                }
                else if (rec.haslayer) {
                    this.LayerIn(rec);
                }
                else // EDIT NODE
                {
                    return 1;
                }
            }
            return 0;
        }

        // NODE Remove shortcuts
        public void removeShortcuts(Nodes Nodes)
        {
            foreach (Node rec in Nodes) // Loop through List with foreach
            {
                this.diagram.RemoveShortcut(rec);
            }

            this.diagram.InvalidateDiagram();
        }

        // NODE Select node color
        public void selectColor()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                colorPickerForm.ShowDialog();
            }
        }

        public void changeColor(ColorType color)
        {
            if (!this.diagram.options.readOnly)
            {
                if (selectedNodes.Count() > 0)
                {
                    this.diagram.undo.add("edit", this.selectedNodes);

                    foreach (Node rec in this.selectedNodes)
                    {
                        rec.color.set(color);
                    }

                    this.diagram.unsave();
                }
            }
        }

        // NODE Select node font color
        public void selectFontColor()
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
                                rec.fontcolor.set(DColor.Color);
                            }
                        }
                    }

                    this.diagram.InvalidateDiagram();
                }
            }
        }

        // NODE Select node font
        public void selectFont()
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
        public bool isSelectionTransparent()
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
        public void makeSelectionTransparent()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                bool isTransparent = this.isSelectionTransparent();

                foreach (Node rec in this.selectedNodes)
                {
                    rec.transparent = !isTransparent;
                }

                this.diagram.InvalidateDiagram();

            }
        }

        // NODE Select node default font
        public void selectDefaultFont()
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
        public void addImage()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                if (this.DImage.ShowDialog() == DialogResult.OK && Os.FileExists(this.DImage.FileName))
                {
                    foreach (Node rec in this.selectedNodes)
                    {
                        this.diagram.setImage(rec, this.DImage.FileName);
                    }

                    this.diagram.unsave();
                }

                this.diagram.InvalidateDiagram();
            }
            else
            {
                if (this.DImage.ShowDialog() == DialogResult.OK && Os.FileExists(this.DImage.FileName))
                {

                    Node newrec = this.CreateNode(this.startMousePos);
                    this.diagram.setImage(newrec, this.DImage.FileName);
                    this.diagram.unsave();
                }

                this.diagram.InvalidateDiagram();
            }
        }

        // NODE Select node image
        public void removeImagesFromSelection()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {

                foreach (Node rec in this.selectedNodes)
                {
                    if (rec.isimage)
                    {
                        this.diagram.removeImage(rec);
                    }
                }

                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE Check if selected nodes are transparent
        public bool hasSelectionImage()
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
        public bool hasSelectionNotEmbeddedImage()
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
        public void makeImagesEmbedded()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                bool hasImage = this.hasSelectionNotEmbeddedImage();

                if (hasImage) {
                    foreach (Node rec in this.selectedNodes)
                    {
                        this.diagram.setImageEmbedded(rec);
                    }

                    this.diagram.unsave();
                }


                this.diagram.InvalidateDiagram();
            }
        }

        // NODE copy
        public bool copy()
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
        public bool cut()
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
        public bool paste(Position position)
        {
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();

            if (retrievedData.GetDataPresent("DiagramXml"))  // [PASTE] [DIAGRAM] [CLIPBOARD OBJECT] insert diagram
            {
                this.SelectNodes(
                    this.diagram.AddDiagramPart(
                        retrievedData.GetData("DiagramXml") as string,
                        position.clone().scale(this.scale).subtract(this.shift),
                        this.currentLayer.id
                    )
                );
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
            else
            if (retrievedData.GetDataPresent(DataFormats.Text))  // [PASTE] [TEXT] insert text
            {
                Node newrec = this.CreateNode(position);

                string ClipText = retrievedData.GetData(DataFormats.Text) as string;

                if (Network.isURL(ClipText))  // [PASTE] [URL] [LINK] paste link from clipboard
                {
                    newrec.link = ClipText;
                    newrec.setName(ClipText);

                    // get page title async in thread
                    Job.doJob(
                        new DoWorkEventHandler(
                            delegate (object o, DoWorkEventArgs args)
                            {
                                newrec.setName(Network.GetWebPageTitle(ClipText));

                            }
                        ),
                        new RunWorkerCompletedEventHandler(
                            delegate (object o, RunWorkerCompletedEventArgs args)
                            {
                                if (newrec.name == null) newrec.setName("url");
                                newrec.color.set("#F2FFCC");
                                this.diagram.InvalidateDiagram();
                            }
                        )
                    );
                    
                    this.diagram.unsave();
                }
                else
                {                                                      
                    newrec.setName(ClipText);

                    // set link to node as path to file
                    if (Os.FileExists(ClipText))
                    {
                        newrec.setName(Os.getFileName(ClipText));
                        newrec.link = Os.makeRelative(ClipText, this.diagram.FileName);
                        newrec.color.set(Media.getColor(diagram.options.colorFile));
                    }

                    // set link to node as path to directory
                    if (Os.DirectoryExists(ClipText))
                    {
                        newrec.setName(Os.getFileName(ClipText));
                        newrec.link = Os.makeRelative(ClipText, this.diagram.FileName);
                        newrec.color.set(Media.getColor(diagram.options.colorDirectory));
                    }

                    this.diagram.unsave();
                }

                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
            else
            if (Clipboard.ContainsFileDropList()) // [FILES] [PASTE] insert files from clipboard
            {
                StringCollection returnList = Clipboard.GetFileDropList();
                foreach (string file in returnList)
                {
                    Node newrec = this.CreateNode(position);
                    newrec.setName(Os.getFileNameWithoutExtension(file));

                    string ext = Os.getExtension(file);

                    if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp") // paste image file direct to diagram as image instead of link
                    {
                        this.diagram.setImage(newrec, file);
                    }
                    else
                    if (
                        this.diagram.FileName != "" 
                        && Os.FileExists(this.diagram.FileName) 
                        && file.IndexOf(Os.getDirectoryName(this.diagram.FileName)) == 0
                    ) // [PASTE] [FILE]
                    {
                        // make path relative to saved diagram path
                        int start = Os.getDirectoryName(this.diagram.FileName).Length;
                        int finish = file.Length - start;
                        newrec.link = "." + file.Substring(start, finish);
                    }
                    else
                    if (this.diagram.FileName != "" && Os.DirectoryExists(this.diagram.FileName)) // [PASTE] [DIRECTORY]
                    {
                        // make path relative to saved diagram path
                        int start = Os.getDirectoryName(this.diagram.FileName).Length;
                        int finish = file.Length - start;
                        newrec.link = "." + file.Substring(start, finish);
                    }
                    else
                    {
                        newrec.link = file;
                    }
                }
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
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
                    }
                    catch (Exception e)
                    {
                        Program.log.write("paste immage error: " + e.Message);
                    }

                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }

            }

            return true;
        }

        // NODE paste to note
        public bool pasteToNote()
        {
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();

            if (retrievedData.GetDataPresent(DataFormats.Text))
            {
                string ClipText = retrievedData.GetData(DataFormats.Text) as string;

                if (this.selectedNodes.Count() == 0)
                {
                    // paste text as new node
                    Node newrec = this.CreateNode(this.getMousePosition());

                    newrec.note = ClipText;
                    this.diagram.unsave();
                }
                else
                {
                    // append text to all selected nodes
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
        public bool pasteToLink()
        {
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();

            if (retrievedData.GetDataPresent(DataFormats.Text))
            {
                string ClipText = retrievedData.GetData(DataFormats.Text) as string;

                if (this.selectedNodes.Count() == 0)
                {
                    Node newrec = this.CreateNode(this.getMousePosition());

                    newrec.link = ClipText;
                    this.diagram.unsave();
                }
                else
                {
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
        public void alignToLine()
        {
            if (this.selectedNodes.Count() > 0)
            {
                this.diagram.AlignToLine(this.selectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE align to column
        public void alignToColumn()
        {
            if (this.selectedNodes.Count() > 0)
            {
                this.diagram.AlignToColumn(this.selectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE align to group
        public void alignToGroup()
        {
            if (this.selectedNodes.Count() > 0)
            {
                this.diagram.AlignCompact(this.selectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE align to group
        public void alignToLineGroup()
        {
            if (this.selectedNodes.Count() > 0)
            {
                this.diagram.AlignCompactLine(this.selectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE align left
        public void alignLeft()
        {
            if (this.selectedNodes.Count() > 1)
            {
                this.diagram.AlignLeft(this.selectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
            else
            {
                this.addNodeAfterNode();
            }
        }

        // NODE align right
        public void alignRight()
        {
            if (this.selectedNodes.Count() > 1)
            {
                this.diagram.AlignRight(this.selectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
            else
            {
                this.addNodeBelowNode();
            }
        }

        // NODE copy node link
        public bool copyLink()
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
        public bool copyNote()
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
        public bool evaluateExpression()
        {
            if (this.selectedNodes.Count() == 1)
            {
                string expression = this.selectedNodes[0].name;
                string expressionResult = "";

                if (Regex.IsMatch(expression, @"^\d+$"))
                {
                    expression = expression + "+1";
                }

                try
                {
                    // NCALC
                    Expression e = new Expression(expression);
                    expressionResult = e.Evaluate().ToString();

                }
                catch (Exception ex)
                {
                    Program.log.write("evaluation error: " + ex.Message);
                }

                if (expressionResult != "")
                {
                    Node newrec = this.CreateNode(this.getMousePosition());
                    newrec.setName(expressionResult);
                    newrec.color.set("#8AC5FF");

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

                Node newrec = this.CreateNode(this.getMousePosition());
                newrec.setName(sum.ToString());
                newrec.color.set("#8AC5FF");

                this.diagram.InvalidateDiagram();
                return true;
            }

            return false;
        }

        // NODE evaluate date
        public bool evaluateDate()
        {
            bool insertdate = true;
            string insertdatestring = "";

            if (this.selectedNodes.Count() > 0)
            {
                DateTime d1;
                DateTime d2;

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
                if (
                    this.selectedNodes.Count() == 2 &&
                    DateTime.TryParse(this.selectedNodes[0].name, out d1) &&
                    DateTime.TryParse(this.selectedNodes[1].name, out d2)
                )
                {
                    try
                    {
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

            Node newrec = this.CreateNode(this.getMousePosition());
            newrec.setName(insertdatestring);
            newrec.color.set("#8AC5FF");

            this.diagram.InvalidateDiagram();
            return true;
        }

        // NODE promote
        public bool promote()
        {
            if (this.selectedNodes.Count() == 1)
            {
                Node selectedNode = this.selectedNodes[0];
                Node newrec = this.CreateNode(this.getMousePosition());
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
                    DateTime theDate;
                    DateTime.TryParseExact(expression, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out theDate);
                    theDate = theDate.AddDays(1);
                    string dateValue = matchesFloat[0].Groups[1].Value;
                    string newnDateValue = String.Format("{0:yyyy-MM-dd}", theDate);
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
        public void random()
        {
            Node node = this.CreateNode(this.getMousePosition(), true);
            node.setName(Encrypt.GetRandomString());

            this.diagram.unsave();
            this.diagram.InvalidateDiagram();
        }

        // NODE hide background
        public bool hideBackground()
        {
            bool changed = false;

            if (this.selectedNodes.Count > 0)
            {
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
                    changed = true;
                }
            }
            if (changed) this.diagram.unsave();
            this.diagram.InvalidateDiagram();

            return true;
        }

        // NODE open edit form
        public void edit()
        {
            if (this.selectedNodes.Count() == 1)
            {
                Node rec = this.selectedNodes[0];
                this.diagram.EditNode(rec);
            }
        }

        // NODE rename
        public void rename()
        {
            if (this.selectedNodes.Count() == 1)
            {
                Node rec = this.selectedNodes[0];
                Position position = new Position(this.shift).add(rec.position).split(this.scale);
                this.editPanel.editNode(position, this.selectedNodes[0]);
            }
        }

        // NODE edit link
        public void editLink()
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
        public void moveNodesToForeground()
        {
            if (this.selectedNodes.Count() > 0)
            {
                foreach (Node rec in this.selectedNodes)
                {
                    this.diagram.layers.moveToForeground(rec);
                }
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE move nodes to background
        public void moveNodesToBackground()
        {
            if (this.selectedNodes.Count() > 0)
            {
                foreach(Node rec in this.selectedNodes)
                {
                    this.diagram.layers.moveToBackground(rec);
                }
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE protect sesitive data in node name
        public void protectNodes()
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
        public void moveNodesToLeft(bool quick = false)
        {
            if (this.selectedNodes.Count() > 0)
            {
                int speed = (quick) ? this.diagram.options.keyArrowSlowSpeed : this.diagram.options.keyArrowFastSpeed;
                foreach (Node rec in this.selectedNodes)
                {
                    rec.position.x -= speed;
                }
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
            else // MOVE SCREEN
            {
                this.shift.x = this.shift.x + 50;
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE move nodes to right
        public void moveNodesToRight(bool quick = false)
        {
            if (this.selectedNodes.Count() > 0)
            {
                int speed = (quick) ? this.diagram.options.keyArrowSlowSpeed : this.diagram.options.keyArrowFastSpeed;
                foreach (Node rec in this.selectedNodes)
                {
                    rec.position.x += speed;
                }
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
            else // MOVE SCREEN
            {
                this.shift.x = this.shift.x - 50;
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE move nodes to up
        public void moveNodesUp(bool quick = false)
        {
            if (this.selectedNodes.Count() > 0)
            {
                int speed = (quick) ? this.diagram.options.keyArrowSlowSpeed : this.diagram.options.keyArrowFastSpeed;
                foreach (Node rec in this.selectedNodes)
                {
                    rec.position.y -= speed;
                }
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
            else // MOVE SCREEN
            {
                this.shift.y = this.shift.y + 50;
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE move nodes to up
        public void moveNodesDown(bool quick = false)
        {
            if (this.selectedNodes.Count() > 0)
            {
                int speed = (quick) ? this.diagram.options.keyArrowSlowSpeed : this.diagram.options.keyArrowFastSpeed;
                foreach (Node rec in this.selectedNodes)
                {
                    rec.position.y += speed;
                }
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
            else // MOVE SCREEN
            {
                this.shift.y = this.shift.y - 50;
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE node is editet with edit panel
        public bool isEditing()
        {
            if (this.editPanel.isEditing()
                || this.editLinkPanel.isEditing())
            {
                return true;
            }

            return false;
        }

        // NODE node is editet with edit panel
        public void cancelEditing()
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
        public bool hasSelectionAttachment()
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
        public void attachmentDeploy()
        {
            if (this.hasSelectionAttachment())
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
        public void attachmentAddFile(Position position)
        {
            if (this.DSelectFileAttachment.ShowDialog() == DialogResult.OK)
            {
                string data = Compress.compress(this.DSelectFileAttachment.FileName);

                if (this.selectedNodes.Count() > 0)
                {
                    foreach (Node node in this.selectedNodes)
                    {
                        node.attachment = data;
                    }
                }
                else
                {
                    Node newrec = this.CreateNode(position, true);
                    newrec.attachment = data;
                    newrec.color.set(diagram.options.colorAttachment);
                    newrec.setName(Os.getFileName(this.DSelectFileAttachment.FileName));
                }

                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE add directory to diagram as attachment
        public void attachmentAddDirectory(Position position)
        {
            if (this.DSelectDirectoryAttachment.ShowDialog() == DialogResult.OK)
            {
                string data = Compress.compress(this.DSelectDirectoryAttachment.SelectedPath);

                if (this.selectedNodes.Count() > 0)
                {
                    foreach (Node node in this.selectedNodes)
                    {
                        node.attachment = data;
                    }
                }
                else
                {
                    Node newrec = this.CreateNode(position, true);
                    newrec.attachment = data;
                    newrec.color.set(diagram.options.colorAttachment);
                    newrec.setName(Os.getFileName(this.DSelectDirectoryAttachment.SelectedPath));
                }

                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // NODE remove attachment from nodes
        public void attachmentRemove()
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
        public Lines getSelectedLines()
        {
            Lines SelectedLinesTemp = new Lines();
            int id = 0;

            foreach (Node srec in this.selectedNodes) {
                id = srec.id;
                foreach (Line lin in this.diagram.layers.getAllLines()) {
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

        // LINE change color of lines
        public void changeLineColor()
        {
            if (!this.diagram.options.readOnly)
            {
                if (this.selectedNodes.Count() > 0)
                {
                    Lines SelectedLines = getSelectedLines();

                    DColor.Color = SelectedLines[0].color.color;

                    if (DColor.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Line lin in SelectedLines)
                        {
                            lin.color.set(DColor.Color);
                        }


                        this.diagram.InvalidateDiagram();
                        this.diagram.unsave();
                    }
                }
            }
        }

        // LINE change line width
        public void changeLineWidth()
        {
            if (!this.diagram.options.readOnly)
            {
                if (this.selectedNodes.Count() > 0)
                {
                    Lines SelectedLines = getSelectedLines();
                    lineWidthForm.setValue(SelectedLines[0].width); // set trackbar to first selected line width
                    lineWidthForm.ShowDialog();

                }
            }
        }

        // LINE resize line with event called by line width form
        public void resizeLineWidth(int width = 1)
        {
            if (!this.diagram.options.readOnly)
            {
                if (this.selectedNodes.Count() > 0)
                {
                    Lines SelectedLines = getSelectedLines();

                    foreach (Line lin in SelectedLines)
                    {
                        lin.width = width;
                    }

                    this.diagram.InvalidateDiagram();
                    this.diagram.unsave();
                }
            }
        }

        /*************************************************************************************************************************/

        // DEBUG Show console
        public void showConsole()
        {
            if (main.console == null)
            {
                main.console = new Console(main);
            }
            main.console.Show();
        }

        // SCRIPT evaluate python script in nodes signed with stamp in node link [F9]
        void evaluate()
        {
            Nodes nodes = null;

            if (this.selectedNodes.Count() > 0) {
                nodes = new Nodes(this.selectedNodes);
            } else {
                nodes = new Nodes(this.diagram.getAllNodes());
            }
            // remove nodes whit link other then [ ! | eval | evaluate | !#num_order | eval#num_order |  evaluate#num_order]
            // higest number is executed first
            Regex regex = new Regex(@"^\s*(eval(uate)|!){1}(#\w+){0,1}\s*$");
            nodes.RemoveAll(n => !regex.Match(n.link).Success);

            nodes.OrderByLink();
            nodes.Reverse();

            String clipboard = Os.getTextFormClipboard();

#if !DEBUG
            var worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += (sender, e) =>
            {
#endif
            this.evaluate(nodes, clipboard);
#if !DEBUG
            };
            worker.RunWorkerAsync();
#endif

        }

        // SCRIPT evaluate python script in node name or in node note in nodes
        void evaluate(Nodes nodes, string clipboard = "")
        {
            Program.log.write("diagram: openlink: run macro");
            Script script = new Script();
            script.setDiagram(this.diagram);
            script.setDiagramView(this);
            script.setClipboard(clipboard);
            string body = "";

            foreach (Node node in nodes)
            {
                body = node.note.Trim() != "" ? node.note : node.name;
                script.runScript(body);
            }
        }

        // SCRIPT evaluate python script in node name or in node note in node
        void evaluate(Node node, string clipboard = "")
        {
            // run macro
            Program.log.write("diagram: openlink: run macro");
            Script script = new Script();
            script.setDiagram(this.diagram);
            script.setDiagramView(this);
            script.setClipboard(clipboard);
            string body = node.note.Trim() != "" ? node.note : node.name;
            script.runScript(body);
        }

        /*************************************************************************************************************************/

        // MOVE TIMER Go to node position
        public void goToNodeWithAnimation(Node node)
        {
            if (node != null)
            {
                if (node.layer != this.currentLayer.id) // if node is in different layer then move instantly
                {
                    this.goToNode(node);
                }
                else
                {
                    this.moveTimer.Enabled = true;
                    
                    this.moveTimerSpeed.set(this.shift.clone().invert())
                        .subtract(node.position)
                        .add(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);

                    double distance = this.moveTimerSpeed.size();
                    this.moveTimerCounter = (distance > 1000) ? 10 : 30;

                    this.moveTimerSpeed
                        .split(this.moveTimerCounter);

                }
            }
        }

        // MOVE TIMER Go to node position
        public void moveTimerTick(object sender, EventArgs e)
        {

            this.shift.add(this.moveTimerSpeed);

            if (--this.moveTimerCounter <= 0) {
                this.moveTimer.Enabled = false;
            }

            this.diagram.InvalidateDiagram();
        }
    }
}
