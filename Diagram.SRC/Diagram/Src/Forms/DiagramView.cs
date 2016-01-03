using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Globalization;
using NCalc;

namespace Diagram
{

    public partial class DiagramView : Form
    {
        public Main main = null;

        public Popup PopupMenu;
        public System.Windows.Forms.SaveFileDialog DSave;
        public System.Windows.Forms.OpenFileDialog DOpen;
        public System.Windows.Forms.ColorDialog DColor;
        public System.Windows.Forms.ColorDialog DFontColor;
        public System.Windows.Forms.FontDialog DFont;
        public System.Windows.Forms.OpenFileDialog DImage;
        public System.Windows.Forms.Timer MoveTimer;
        public System.Windows.Forms.FontDialog defaultfontDialog;
        public System.Windows.Forms.SaveFileDialog exportFile;
        public System.Windows.Forms.SaveFileDialog saveTextFileDialog;
        public System.Windows.Forms.FolderBrowserDialog DSelectDirectoryAttachment;
        public System.Windows.Forms.OpenFileDialog DSelectFileAttachment;

        /*************************************************************************************************************************/

        // ATRIBUTES SCREEN
        public Position shift = new Position();                   // left corner position

        public Position startShift = new Position();              // temporary left corner position before change in diagram

        // ATTRIBUTES MOUSE
        public Position startMousePos = new Position();           // start movse position before change
        public Position startNodePos = new Position();            // start node position before change
        public Position vmouse = new Position();                  // vector position in selected node before change
        public Position actualMousePos = new Position();          // actual mouse position in form in drag process

        // ATTRIBUTES KEYBOARD
        public char key = ' ';                   // last key character - for new node add
        public bool keyshift = false;            // actual shift key state
        public bool keyctrl = false;             // actual ctrl key state
        public bool keyalt = false;              // actual alt key state

        // ATTRIBUTES STATES
        public bool drag = false;                // actual drag status
        public bool move = false;                // actual move node status
        public bool selecting = false;           // actual selecting node status or creating node by drag
        public bool addingNode = false;          // actual adding node by drag
        public bool dblclick = false;            // actual dblclick status
        public bool zooming = false;             // actual zooming by space status
        public bool searching = false;           // actual search edit form status

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
        public List<Node> selectedNodes = new List<Node>();  // all selected nodes by mouse

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
        Position currentPosition = new Position();
        public List<int> nodesSearchResult = new List<int>(); // all nodes found by search panel

        // COMPONENTS
        private IContainer components;

        // INIT COMPONENTS
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
            this.DImage.Filter = "All|*.bmp;*.jpg;*.jpeg;*.png;*.ico|Bmp|*.bmp|Jpg|*.jpg;*.jpeg|Png|*.png|Ico|*.ico";
            //
            // MoveTimer
            //
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
            this.Icon = global::Diagram.Properties.Resources.ico_diagram;
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
        public DiagramView(Main main, Diagram diagram)
        {
            this.main = main;
            this.diagram = diagram;

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
        }

        // FORM Load event -
        public void DiagramViewLoad(object sender, EventArgs e)
        {
            this.Left = 50;
            this.Top = 40;
            this.Width = Media.screenWidth(this) - 100;
            this.Height = Media.screenHeight(this) - 100;

            // Preddefinovana pozicia okna
            if (this.diagram != null)
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
            if (this.diagram.options.homeLayer != 0)
            {
                this.goToLayer(this.diagram.options.homeLayer);
                this.goToPosition(this.diagram.options.homePosition);
            }
            this.shift.set(diagram.options.homePosition);
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

        // FORM set home position
        public void setCurentPositionAsHomePosition()
        {
            diagram.options.homePosition.x = this.shift.x;
            diagram.options.homePosition.y = this.shift.y;
            diagram.options.homeLayer = this.currentLayer.id;
        }

        // FORM go to end position - center window to second remembered position
        public void GoToEnd()
        {
            this.shift.set(diagram.options.endPosition);
            this.goToLayer(diagram.options.endLayer);
            this.diagram.InvalidateDiagram();
        }

        // FORM set end position
        public void setCurentPositionAsEndPosition()
        {
            diagram.options.endPosition.x = this.shift.x;
            diagram.options.endPosition.y = this.shift.y;
            diagram.options.endLayer = this.currentLayer.id;
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

        /*************************************************************************************************************************/

        // SELECTION check if node is in current window selecton 
        public bool isSelected(Node a)
        {
            if (a == null) return false;

            bool found = false;

            if (this.selectedNodes.Count() > 0)
            {
                foreach (Node rec in this.selectedNodes) // Loop through List with foreach
                {

                    if (rec == a)
                    {
                        found = true;
                        break;
                    }
                }
            }

            return found;
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
        public void SelectNodes(List<Node> nodes)
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
            this.PaintDiagram(e.Graphics);
        }

        // EVENT Mouse DoubleClick
        public void DiagramApp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.dblclick = true;
        }

        // EVENT Mouse Down                                                                            // [MOUSE] [DOWN] [EVENT]
        public void DiagramApp_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.searching)
            {
                this.searching = false;
                this.searhPanel.HidePanel();
            }

			this.Focus();

            if (this.isEditing())
            {
                this.ActiveControl = null;
            }

            this.startMousePos.x = e.X;  // starting mouse position
            this.startMousePos.y = e.Y;

            this.startShift.x = this.shift.x;  // starting indent
            this.startShift.y = this.shift.y;

            if (e.Button == MouseButtons.Left)
            {
                this.sourceNode = this.findNodeInMousePosition(new Position(e.X, e.Y));

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
                if (this.editPanel.editing) // close edit panel after mouse click to form
                {
                    this.editPanel.saveNodeNamePanel();
                }
                else
                if (this.sourceNode == null)
                {
                    this.actualMousePos.x = e.X;
                    this.actualMousePos.y = e.Y;
                    if (!this.diagram.options.readOnly
                        && (this.keyctrl || this.keyalt)
                        && !this.keyshift) // add node by drag
                    {
                        this.addingNode = true;
                        MoveTimer.Enabled = true;
                        this.ClearSelection();
                    }
                    else // multiselect
                    {
                        this.selecting = true;
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
                    if (!this.diagram.options.readOnly)  //informations for draging
                    {
                        this.drag = true;
                        MoveTimer.Enabled = true;
                        this.startNodePos.x = this.sourceNode.position.x; // starting position of draging item
                        this.startNodePos.y = this.sourceNode.position.y;
                        this.vmouse.x = (int)(e.X * this.scale - (this.shift.x + this.sourceNode.position.x)); // mouse position in node
                        this.vmouse.y = (int)(e.Y * this.scale - (this.shift.y + this.sourceNode.position.y));

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
                this.move = true; // popupmenu or view move
            }
            else
            if (e.Button == MouseButtons.Middle)
            {
                if (!this.diagram.options.readOnly)
                {
                    this.actualMousePos.x = e.X;
                    this.actualMousePos.y = e.Y;
                    this.addingNode = true;// add node by drag
                    MoveTimer.Enabled = true;
                }
            }

            this.diagram.InvalidateDiagram();
        }

        // EVENT Mouse move                                                                            // [MOUSE] [MOVE] [EVENT]
        public void DiagramApp_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.selecting || this.addingNode)
            {
                this.actualMousePos.x = e.X;
                this.actualMousePos.y = e.Y;
                this.diagram.InvalidateDiagram();
            }
            else
            if (!this.diagram.options.readOnly && this.drag && !this.dblclick) // posunutie objektu
            {
                if (this.sourceNode != null)
                {
                    this.sourceNode.position.x = (int)(-this.shift.x + (e.X * this.scale - this.vmouse.x));
                    this.sourceNode.position.y = (int)(-this.shift.y + (e.Y * this.scale - this.vmouse.y));

                    this.diagram.InvalidateDiagram();
                }
            }
            else
            if (this.move) // posunutie obrazovky
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
            // States
            bool mousemove = ((this.actualMousePos.x != this.startMousePos.x) || (this.actualMousePos.y != this.startMousePos.y)); // mouse change position
            bool buttonleft = e.Button == MouseButtons.Left;
            bool buttonright = e.Button == MouseButtons.Right;
            bool buttonmiddle = e.Button == MouseButtons.Middle;
            bool isreadonly = this.diagram.options.readOnly;
            bool keyalt = this.keyalt;
            bool keyctrl = this.keyctrl;
            bool keyshift = this.keyshift;
            bool dblclick = this.dblclick;
            bool finishdraging = this.drag;
            bool finishadding = this.addingNode;
            bool finishselecting = mousemove && this.selecting;

            int vectorx = e.X - this.startMousePos.x;
            int vectory = e.Y - this.startMousePos.y;

            MoveTimer.Enabled = false;

            if(dblclick)
            {
                this.selecting = false;
            }
            else
            // KEY DRAG
            if (finishdraging) // drag node
            {
                if (!this.diagram.options.readOnly)
                {
                    if (this.sourceNode != null) // return node to starting position after connection is created
                    {
                        this.sourceNode.position.x = this.startNodePos.x;
                        this.sourceNode.position.y = this.startNodePos.y;
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
            // KEY DRAG+MRIGHT select nodes in selection rectangle
            if (finishselecting)
            {
                if (mousemove)
                {
                    int a = (int)(+this.shift.x - this.startShift.x + this.startMousePos.x * this.scale);
                    int b = (int)(+this.shift.y - this.startShift.y + this.startMousePos.y * this.scale);
                    int c = (int)(this.actualMousePos.x * this.scale);
                    int d = (int)(this.actualMousePos.y * this.scale);
                    int temp;
                    if (c < a) { temp = a; a = c; c = temp; }
                    if (d < b) { temp = d; d = b; b = temp; }
                    if (!this.keyshift) this.ClearSelection();
                    foreach (Node rec in this.currentLayer.nodes)
                    {
                        if (
                            (rec.layer == this.currentLayer.id || rec.id == this.currentLayer.id)
                            && -this.shift.x + a <= rec.position.x
                            && rec.position.x + rec.width <= -this.shift.x + c
                            && -this.shift.y + b <= rec.position.y
                            && rec.position.y + rec.height <= -this.shift.y + d) // get all nodes in selection rectangle
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
                }

                this.diagram.InvalidateDiagram();
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
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+ALT create node and conect with existing node
                if (!isreadonly
                    && keyalt
                    && TargetNode == null
                    && this.sourceNode != null)
                {
                    var s = this.sourceNode;
                    var r = this.CreateNode(new Position(e.X, e.Y));
                    r.shortcut = s.id;
                    this.diagram.Connect(s, r, this.currentLayer.id);
                    this.diagram.unsave();
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
                    && Math.Sqrt(vectorx*vectorx+vectory*vectory) > 5
                )
                {
                    this.sourceNode.position.x = (int)(-this.shift.x + (e.X * this.scale - this.vmouse.x));
                    this.sourceNode.position.y = (int)(-this.shift.y + (e.Y * this.scale - this.vmouse.y));
                    if (this.sourceNode.id != this.currentLayer.id
                        && this.sourceNode.haslayer)
                    {
                        this.sourceNode.layerShift.x -= (e.X - this.startMousePos.x);
                        this.sourceNode.layerShift.y -= (e.Y - this.startMousePos.y);
                    }

                    if (this.selectedNodes.Count() > 0)
                    {
                        var vx = this.sourceNode.position.x - this.startNodePos.x;
                        var vy = this.sourceNode.position.y - this.startNodePos.y;

                        foreach (Node rec in this.selectedNodes) // Loop through List with foreach
                        {
                            if (rec != this.sourceNode)
                            {
                                rec.position.x = rec.position.x + vx;
                                rec.position.y = rec.position.y + vy;

                                if (rec.id != this.currentLayer.id && rec.haslayer)
                                {
                                    rec.layerShift.x -= vx;
                                    rec.layerShift.y -= vy;
                                }
                            }
                        }
                    }

                    this.diagram.unsave();

                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+CTRL Vytvorenie noveho obdlznika a spojenie s existujucim
                if (!isreadonly
                    && keyctrl
                    && TargetNode != null
                    && this.sourceNode == null)
                {
                    this.diagram.Connect(
                        this.CreateNode(
                            new Position(
                                +this.shift.x - startShift.x + this.startMousePos.x,
                                +this.shift.y - startShift.y + this.startMousePos.y
                            )
                        ),
                        TargetNode,
                        this.currentLayer.id
                    );
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+ALT create
                if (!isreadonly
                    && keyalt
                    && !keyctrl
                    && !keyshift
                    && TargetNode != null
                    && this.sourceNode == null)
                {
                    Node newrec = this.CreateNode(
                        new Position(
                            +this.shift.x - startShift.x + this.startMousePos.x,
                            +this.shift.y - startShift.y + this.startMousePos.y
                        )
                    );

                    newrec.shortcut = TargetNode.id;
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DBLCLICK otvorenie editacie alebo linku [dblclick]
                if (dblclick
                    && this.sourceNode != null
                    && !keyctrl
                    && !keyalt
                    && !keyshift)
                {
                    this.OpenLinkAsync(this.sourceNode);
                }
                else
                // KEY DBLCLICK+SHIFT otvorenie editacie
                if (dblclick
                    && this.sourceNode != null
                    && !keyctrl
                    && !keyalt
                    && keyshift)
                {
                    this.diagram.EditNode(this.sourceNode);
                }
                else
                // KEY DBLCLICK+CTRL otvorenie adresára ak má noda link na súbor alebo je adresár
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
                // KEY DBLCLICK+SPACE presunutie sa v zoomingu na novú pozíciu
                if (dblclick
                    && this.zooming
                    && !keyctrl
                    && !keyalt
                    && !keyshift)
                {
                    this.shift.x = (int)(this.shift.x - (e.X * this.scale) + (this.ClientSize.Width * this.scale) / 2);
                    this.shift.y = (int)(this.shift.y - (e.Y * this.scale) + (this.ClientSize.Height * this.scale) / 2);
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
                    Node newrec = TargetNode;
                    if (newrec == null)
                    {
                        newrec = this.CreateNode(new Position(e.X - 10, e.Y - 10), false);
                    }

                    foreach (Node rec in this.selectedNodes)
                    {
                        this.diagram.Connect(rec, newrec, this.currentLayer.id);
                    }
                    this.SelectOnlyOneNode(newrec);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY CTRL+MLEFT
                // KEY DBLCLICK Vytvorenie noveho obdlznika
                if (!isreadonly
                    && (dblclick || keyctrl)
                    && TargetNode == null
                    && this.sourceNode == null
                    && e.X == this.startMousePos.x
                    && e.Y == this.startMousePos.y)
                {
                    this.CreateNode(new Position(e.X - 10, e.Y - 10), false);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+CTRL Skopirovanie farby a stylu z jednej nody na druhu
                if (!isreadonly
                    && !keyshift
                    && keyctrl
                    && TargetNode != null
                    && this.sourceNode != null
                    && this.sourceNode != TargetNode)
                {
                    if (this.selectedNodes.Count() > 1)
                    {
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
                        TargetNode.color = this.sourceNode.color;
                        TargetNode.font = this.sourceNode.font;
                        TargetNode.fontcolor = this.sourceNode.fontcolor;
                        TargetNode.transparent = this.sourceNode.transparent;
                        TargetNode.resize();

                        if (this.selectedNodes.Count() == 1 && this.selectedNodes[0] != this.sourceNode)
                        {
                            this.ClearSelection();
                            this.SelectNode(this.sourceNode);
                        }
                    }
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG Pridanie spojovaciej čiary
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
                                    this.diagram.Connect(TargetNode, rec, arrow, null, this.currentLayer.id);
                                }
                                else
                                {
                                    this.diagram.Connect(rec, TargetNode, arrow, null, this.currentLayer.id); 
                                }
                            }
                        }
                    }
                    else
                    {
                        this.diagram.Connect(sourceNode, TargetNode, arrow, null, this.currentLayer.id);
                    }

                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                // KEY CTRL+MLEFT pridanie prvku do vyberu
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
                // KEY CLICK+CTRL odstránenie prvku z vyberu
                else
                if (keyctrl
                    && TargetNode != null
                    && (this.sourceNode == TargetNode || this.isSelected(TargetNode)))
                {
                    this.RemoveNodeFromSelection(TargetNode);
                    this.diagram.InvalidateDiagram();
                }

            }
            else
            if (buttonright) // KEY MRIGHT
            {
                this.move = false; // show popup menu
                if (e.X == this.startMousePos.x
                    && e.Y == this.startMousePos.y
                    && this.startShift.x == this.shift.x
                    && this.startShift.y == this.shift.y)
                {
                    Node temp = this.findNodeInMousePosition(new Position(e.X, e.Y));
                    if (this.selectedNodes.Count() > 0 && !this.isSelected(temp))
                    {
                        this.ClearSelection();
                    }

                    if (this.selectedNodes.Count() == 0 && this.sourceNode != temp)
                    {
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
                        TargetNode,
                        this.currentLayer.id
                    );

                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
            }

            this.dblclick = false;
            this.drag = false;
            this.addingNode = false;
            this.selecting = false;
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
            if (this.isEditing() || this.searching)
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

            if (KeyMap.parseKey(KeyMap.newDiagram, keyData))  // [KEY] [CTRL+N] New Diagram
            {
                main.OpenDiagram();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.newDiagramView, keyData))  // [KEY] [CTRL+SHIFT+N] New Diagram view
            {
                this.diagram.openDiagramView();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.save, keyData))  // [KEY] [CTRL+S] Uloženie okna
            {
                this.save();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.open, keyData))  // [KEY] [CTRL+O] Otvorenie diagramu
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

            if (KeyMap.parseKey(KeyMap.pageUp, keyData)) // [KEY] [PAGEUP] page up
            {
                this.pageUp();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.pageDown, keyData)) // [KEY] [PAGEDOWN] Posun obrazovky
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
                return this.formHide();
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

            if (KeyMap.parseKey(KeyMap.alignLeft, keyData)) // [KEY] [TAB] zarovnanie vybranych prvkov dolava
            {
                this.alignLeft();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.alignRight, keyData))  // [KEY] [SHIFT+TAB] Zarovnanie vybranych prvkov doprava //KEY shift+tab
            {
                this.alignRight();
                return true;
            }

            if (KeyMap.parseKey(KeyMap.resetZoom, keyData))  // [KEY] [CTRL+0] Otvorenie diagramu
            {
                this.resetZoom();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // EVENT Key down
        public void DiagramApp_KeyDown(object sender, KeyEventArgs e)                                  // [KEYBOARD] [DOWN] [EVENT]
        {
            if (this.isEditing() || this.searching)
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

            if (e.KeyCode == Keys.Space && !this.zooming) // KEY SPACE
            {
                this.selecting = false;
                MoveTimer.Enabled = false;

                this.zooming = true;
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

            if (this.isEditing() || this.searching)
            {
                return;
            }

            if (this.zooming)
            {
                MoveTimer.Enabled = false;  // zrusenie prebiehajucich operácii
                this.move = false;
                this.addingNode = false;
                this.drag = false;
                this.selecting = false;

                this.zooming = false; // KEY SPACE cancel space zoom and restore prev zoom

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
            if (this.isEditing() || this.searching)
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

        // EVENT File Drop; DROP file
        public void DiagramApp_DragDrop(object sender, DragEventArgs e)                                // [DROP] [EVENT]
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    Node newrec = this.CreateNode(this.getMousePosition());
                    newrec.setName(Os.getFileName(file));

    				newrec.link = file;
    				if (Os.DirectoryExists(file)) // directory
                    {
    					newrec.link = Os.makeRelative(file, this.diagram.FileName);
                        newrec.color = Media.getColor(diagram.options.colorDirectory);
                    }
    				else
    				if (Os.Exists(file))
                    {
                        newrec.color = Media.getColor(diagram.options.colorFile);

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

                    this.diagram.unsave();
                }

                this.diagram.InvalidateDiagram();
            } catch (Exception ex) {
                Program.log.write("drop file goes wrong: error: " + ex.Message);
            }
        }

        // EVENT File Drop
        public void DiagramApp_DragEnter(object sender, DragEventArgs e)                               // [DRAG] [EVENT]
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        // EVENT Resize
        public void DiagramApp_Resize(object sender, EventArgs e)                                      // [RESIZE] [EVENT]
        {
            if (this.zooming)
            {
                this.zooming = false;
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
            if (this.drag || this.selecting || this.addingNode)
            {
                Point ptCursor = Cursor.Position;
                ptCursor = PointToClient(ptCursor);
                bool changed = false;
                if (this.ClientSize.Width - 20 < ptCursor.X)
                {
                    this.shift.x -= (int)(50 * this.scale);
                    changed = true;
                }

                if (ptCursor.X < 20)
                {
                    this.shift.x += (int)(50 * this.scale);
                    changed = true;
                }

                if (this.ClientSize.Height - 50 < ptCursor.Y)
                {
                    this.shift.y -= (int)(50 * this.scale);
                    changed = true;
                }

                if (ptCursor.Y < 10)
                {
                    this.shift.y += (int)(50 * this.scale);
                    changed = true;
                }
                //c.add("shiftx=" + this.shiftx.ToString() + " shifty=" + this.shifty.ToString());
                if (changed) this.diagram.InvalidateDiagram();
            }
        }                                      // [MOVE] [TIMER] [EVENT]

        // EVENT Focus - lost focus
        public void DiagramApp_Deactivate(object sender, EventArgs e)
        {
            this.keyctrl = false;
            this.keyalt = false;
            this.keyshift = false;
            if (this.zooming)
            {
                this.zooming = false;
                this.scale = this.zoomingDefaultScale;
            }
            this.drag = false;
            this.addingNode = false;
            this.selecting = false;
            this.move = false;

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
                this.diagram.InvalidateDiagram();
            }
        }

        // LAYER HISTORY Buld laier history from
        public void BuildLayerHistory(int id)
        {
            layersHistory.Clear();

            this.currentLayer = this.diagram.layers.getLayer(id);

            List<Node> nodes = this.diagram.getAllNodes();

            Layer temp = this.currentLayer;
            while (temp != null)
            {
                layersHistory.Add(temp);
                temp = temp.parentLayer;
            }

            layersHistory.Reverse(0, layersHistory.Count());
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

            List<Node> foundNodes = new List<Node>();

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

            middle.x = middle.x - this.Width / 2;
            middle.y = middle.y - this.Height / 2;

            foundNodes.Sort((first, second) =>
            {
                double d1 = first.position.convertToStandard().distance(middle);
                double d2 = second.position.convertToStandard().distance(middle);

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
                this.goToNode(node);
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
                this.goToNode(node);
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

            searhPanel.ShowPanel();
            this.searching = true;
        }

        // SEARCHPANEL CANCEL - restore beggining search position
        private void SearchCancel()
        {
            this.shift.x = currentPosition.x;
            this.shift.y = currentPosition.y;

            this.SearchClose();
        }

        // SEARCHPANEL Close - close search panel
        private void SearchClose()
        {
            this.Focus();
            this.searching = false;
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

        // SCROLLBAR MOVE LEFT-RIGHT                                                                       // SCROLLBAR
        /* move view in percent units 0-1*/
        public void moveScreenHorizontal(float per)
        {
            int minx = int.MaxValue;
            int maxx = int.MinValue;
            foreach (Node rec in this.diagram.getAllNodes())
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

        // SCROLLBAR GET POSITION LEFT-RIGHT nastaví aktuálnu pozíciu pre skrolbar
        /* move view in percent units 0-1*/
        public float getPositionHorizontal()
        {
            float per = 0;
            int minx = int.MaxValue;
            int maxx = int.MinValue;
            foreach (Node rec in this.diagram.getAllNodes())
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

        // SCROLLBAR MOVE UP-DOWN
        public void moveScreenVertical(float per)
        {
            int miny = int.MaxValue;
            int maxy = int.MinValue;
            foreach (Node rec in this.diagram.getAllNodes())
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

        // SCROLLBAR GET POSITION LEFT-RIGHT nastaví aktuálnu pozíciu pre skrolbar
        /* get view position in percent units 0-1*/
        public float getPositionVertical()
        {
            float per = 0;
            int miny = int.MaxValue;
            int maxy = int.MinValue;
            foreach (Node rec in this.diagram.getAllNodes())
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

        // SCROLLBAR Change position horizontal
        public void positionChangeBottom(object source, PositionEventArgs e)
        {
            moveScreenHorizontal(e.GetPosition());
        }

        // SCROLLBAR Change position vartical
        public void positionChangeRight(object source, PositionEventArgs e)
        {
            moveScreenVertical(e.GetPosition());
        }

        /*************************************************************************************************************************/

        // FILE Save - Ulozit súbor
        public void save()
        {
            if (!this.diagram.save())
            {
                this.saveas();
            }
        }

        // FILE SAVEAS - Uložiť súbor ako
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

        // FILE Open - Ulozit súbor
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

        // FILE Open diagram directory
        public void openDiagramDirectory()
        {
            if (!this.diagram.NewFile && Os.FileExists(this.diagram.FileName))
            {
                try
                {
                    System.Diagnostics.Process.Start(Os.getDirectoryName(this.diagram.FileName));
                }
                catch (Exception ex)
                {
                    Program.log.write("open diagram directory error: " + ex.Message);
                }
            }
        }

        /*************************************************************************************************************************/

        // EXPORT Export diagram to png
        public void exportDiagramToPng()
        {
            List<Node> nodes = this.diagram.getAllNodes();

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
                this.PaintDiagram(g, new Position(-this.shift.x - minx, -this.shift.y - miny), true);
                g.Dispose();
                bmp.Save(exportFile.FileName, System.Drawing.Imaging.ImageFormat.Png);
                bmp.Dispose();
            }
        }

        // EXPORT Export diagram to txt
        public void exportDiagramToTxt(string filePath)
        {
            if (this.diagram.getAllNodes().Count > 0)
            {
                string outtext = "";

                foreach (Node rec in this.diagram.getAllNodes())
                {
                    outtext += rec.name + "\n" + (rec.link != "" ? rec.link + "\n" : "") + "\n" + rec.note + "\n---\n";
                }
                Os.writeAllText(filePath, outtext);
            }
        }

        /*************************************************************************************************************************/

        // PAINT                                                                                      // [PAINT]
        void PaintDiagram(Graphics gfx, Position correction = null, bool export = false)
        {
            gfx.SmoothingMode = SmoothingMode.AntiAlias;

            if (this.diagram.options.grid && !export)
            {
                this.PaintGrid(gfx);
            }

            this.PaintLines(gfx, correction, export);

            // DRAW addingnode
            if (!export && this.addingNode && !this.zooming && (this.actualMousePos.x != this.startMousePos.x || this.actualMousePos.y != this.startMousePos.y))
            {
                this.PaintAddNode(gfx);
            }

            this.PaintNodes(gfx, correction, export);


            // DRAW select - zvyraznenie výberu viacerich elementov (multiselect)
            if (!export && this.selecting && (this.actualMousePos.x != this.startMousePos.x || this.actualMousePos.y != this.startMousePos.y))
            {
                this.PaintSelectedNodes(gfx);
            }

            // PREVIEW draw zoom mini screen
            if (!export && this.zooming)
            {
                this.PaintMiniScreen(gfx);
            }

            // DRAW coordinates
            if (this.diagram.options.coordinates)
            {
                this.PaintCoordinates(gfx);
            }

            // vykreslenie scrollbarov
            if (!export && bottomScrollBar != null && rightScrollBar != null)
            {
                bottomScrollBar.Paint(gfx);
                rightScrollBar.Paint(gfx);
            }
        }

        void PaintGrid(Graphics gfx)
        {
            float s = this.scale;
            System.Drawing.Pen myPen = new System.Drawing.Pen(Color.FromArgb(201, 201, 201), 1);

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

        void PaintMiniScreen(Graphics gfx)
        {
            float s = this.scale;
            System.Drawing.Pen myPen = new System.Drawing.Pen(Color.FromArgb(201, 201, 201), 1);

            myPen = new System.Drawing.Pen(System.Drawing.Color.Gray, 1);
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

        void PaintCoordinates(Graphics gfx)
        {
            float s = this.scale;
            System.Drawing.Pen myPen = new System.Drawing.Pen(Color.FromArgb(201, 201, 201), 1);

            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            gfx.DrawString(
                        (this.shift.x).ToString() + "," +
                        this.shift.y.ToString() +
                        " (" + this.ClientSize.Width.ToString() + "x" + this.ClientSize.Height.ToString() + ") " +
                        "scl:" + s.ToString() + "," + this.currentScale.ToString(),
                        drawFont, drawBrush, 10, 10);
        }

        void PaintSelectedNodes(Graphics gfx)
        {
            float s = this.scale;
            System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);

            int a = (int)(+this.shift.x - this.startShift.x + this.startMousePos.x * this.scale);
            int b = (int)(+this.shift.y - this.startShift.y + this.startMousePos.y * this.scale);
            int c = (int)(this.actualMousePos.x * this.scale);
            int d = (int)(this.actualMousePos.y * this.scale);
            int temp;
            if (c < a) { temp = a; a = c; c = temp; }
            if (d < b) { temp = d; d = b; b = temp; }

            gfx.FillRectangle(new SolidBrush(Color.FromArgb(100, 10, 200, 200)), new Rectangle((int)(a / this.scale), (int)(b / this.scale), (int)((c - a) / this.scale), (int)((d - b) / this.scale)));
        }

        void PaintAddNode(Graphics gfx)
        {
            float s = this.scale;
            System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);

            gfx.DrawLine(
                    myPen,
                    this.shift.x - this.startShift.x + this.startMousePos.x - 2 + 12,
                    this.shift.y - this.startShift.y + this.startMousePos.y - 2 + 12,
                    this.actualMousePos.x,
                    this.actualMousePos.y
                );
            gfx.FillEllipse(
                new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#FFFFB8")),
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

        void PaintNodes(Graphics gfx, Position correction = null, bool export = false)
        {
            bool isvisible = false; // drawonly visible elements
            float s = this.scale;

            System.Drawing.Pen myPen1 = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
            System.Drawing.Pen myPen2 = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

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
                                    gfx.FillEllipse(new SolidBrush(rec.color), rect1);
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
                                    gfx.FillRectangle(new SolidBrush(rec.color), rect1);
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
                                    rec.name,
                                    new System.Drawing.Font(
                                       rec.font.FontFamily,
                                       rec.font.Size / s,
                                       rec.font.Style,
                                       System.Drawing.GraphicsUnit.Point,
                                       ((byte)(0))
                                    ),
                                    new SolidBrush(rec.fontcolor),
                                    rect2
                                );
                            }
                        }
                    }
                }

            }
        }

        void PaintLines(Graphics gfx, Position correction = null, bool export = false)
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
                                new SolidBrush(lin.color),
                                curvePoints,
                                newFillMode
                            );
                        }
                        else
                        {
                            // draw line
                            gfx.DrawLine(
                                new System.Drawing.Pen(lin.color, 1),
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

        //DIAGRAM Set model
        public void setDiagram(Diagram diagram)
        {
            this.diagram = diagram;
        }

        //DIAGRAM Get model
        public Diagram getDiagram()
        {
            return this.diagram;
        }

        /*************************************************************************************************************************/

        // VIEW CLOSE
        private void DiagramView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.diagram.DiagramViews.Remove(this);
            main.DiagramViews.Remove(this);
            this.diagram.CloseDiagram();
        }

        // VIEW REFRESH
        private void DiagramView_Activated(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        // VIEW FOCUS
        public void setFocus()
        {
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
                            try
                            {
                                System.Diagnostics.Process.Start(node.link);
                            }
                            catch (Exception ex)
                            {
                                Program.log.write("open node link directory error: " + ex.Message);
                            }
                        }
                        else
                            if (Os.FileExists(node.link)) // open directory of selected files
                            {
                                try
                                {
                                    string parent_diectory = Os.getFileDirectory(this.selectedNodes[0].link);
                                    System.Diagnostics.Process.Start(parent_diectory);
                                }
                                catch (Exception ex)
                                {
                                    Program.log.write("open node link directory error: " + ex.Message);
                                }
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
                    bool canRefresh = false;
                    for (int i = DiagramView.selectedNodes.Count() - 1; i >= 0; i--)
                    {
                        if (this.diagram.canDeleteNode(DiagramView.selectedNodes[i]))
                        {
                            this.diagram.DeleteNode(DiagramView.selectedNodes[i]);
                            canRefresh = true;
                        }
                    }

                    if (canRefresh)
                    {
                        this.diagram.InvalidateDiagram();
                    }
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

        // NODE Najdenie nody podla pozicie myši
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
                    // set current directory to current diagrm file destination
                    if (Os.FileExists(this.diagram.FileName))
                    {
                        Os.setCurrentDirectory(Os.getFileDirectory(this.diagram.FileName));
                    }

                    Match matchFileOpenOnPosition = (new Regex("^([^#]+)#(.*)$")).Match(rec.link.Trim());


                    // node with link "script" is executed as script
                    if (matchFileOpenOnPosition.Success && Os.FileExists(Os.normalizedFullPath(matchFileOpenOnPosition.Groups[1].Value)))       // OPEN FILE ON POSITION
                    {
                        try
                        {

                            String fileName = matchFileOpenOnPosition.Groups[1].Value;
                            String searchString = matchFileOpenOnPosition.Groups[2].Value.Trim();

                            if(searchString.Trim() == "")
                            {
                                searchString = rec.name;
                            }

                            Match matchNumber = (new Regex("^(\\d+)$")).Match(searchString);

                            if (!matchNumber.Success)
                            {
                                searchString = Os.fndLineNumber(fileName, searchString).ToString();
                            }

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
                    else if (rec.link.Trim() == "script")  // OPEN SCRIPT
                    {
                        // run macro
                        Program.log.write("diagram: openlink: run macro");
                        Script script = new Script();
                        script.setDiagram(this.diagram);
                        script.setDiagramView(this);
                        script.setClipboard(clipboard);
                        script.runScript(rec.note);
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
                    else
                    {

                        /*
                        [DOCUMENTATION]
                        - po dvojkliku na nodu sa spusti link
                        - v linku sa nahradia klucove vyrazi
                            %TEXT%     - name of node
                            %NAME%     - name of node
                            %LINK%     - link in node
                            %NOTE%     - note in node
                            %ID%       - id of actual node
                            %FILENAME% - file name  of diagram
                            %DIRECTORY% - current diagram directory
                        */

                        string cmd = rec.link;                     // replace variables in link
                        cmd = cmd.Replace("%TEXT%", rec.name);
                        cmd = cmd.Replace("%NAME%", rec.name);
                        cmd = cmd.Replace("%LINK%", rec.link);
                        cmd = cmd.Replace("%NOTE%", rec.note);
                        cmd = cmd.Replace("%ID%", rec.id.ToString());
                        cmd = cmd.Replace("%FILENAME%", this.diagram.FileName);
                        cmd = cmd.Replace("%DIRECTORY%", Os.getFileDirectory(this.diagram.FileName));

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
        public void removeShortcuts(List<Node> Nodes)
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
                DColor.Color = this.selectedNodes[0].color;

                if (DColor.ShowDialog() == DialogResult.OK)
                {
                    if (!this.diagram.options.readOnly)
                    {
                        if (selectedNodes.Count() > 0)
                        {
                            foreach (Node rec in this.selectedNodes)
                            {
                                rec.color = DColor.Color;
                            }
                        }
                    }

                    this.diagram.InvalidateDiagram();
                }
            }
        }

        // NODE Select node font color
        public void selectFontColor()
        {
            if (selectedNodes.Count() > 0 && !this.diagram.options.readOnly)
            {
                DFontColor.Color = this.selectedNodes[0].color;

                if (DColor.ShowDialog() == DialogResult.OK)
                {
                    if (!this.diagram.options.readOnly)
                    {
                        if (selectedNodes.Count() > 0)
                        {
                            foreach (Node rec in this.selectedNodes)
                            {
                                rec.fontcolor = DColor.Color;
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

                if (Network.isURL(ClipText))  // [PASTE] [URL] [LINK] Spracovanie linku zo schranky
                {
                    newrec.link = ClipText;
                    newrec.setName(ClipText);

                    if (Network.isHttpsURL(ClipText))
                    {
                        Job.doJob(
                            new DoWorkEventHandler( // do work
                                delegate (object o, DoWorkEventArgs args)
                                {
                                    newrec.setName(Network.GetSecuredWebPageTitle(ClipText));

                                }
                            ),
                            new RunWorkerCompletedEventHandler( //do code after work
                                delegate (object o, RunWorkerCompletedEventArgs args)
                                {
                                    if (newrec.name == null) newrec.setName("url");
                                    newrec.color = System.Drawing.ColorTranslator.FromHtml("#F2FFCC");
                                    this.diagram.InvalidateDiagram();
                                }
                            )
                        );
                    }
                    else
                    {
                        Job.doJob(
                            new DoWorkEventHandler( // do work
                                delegate (object o, DoWorkEventArgs args)
                                {
                                    newrec.name = Network.GetWebPageTitle(ClipText);
                                }
                            ),
                            new RunWorkerCompletedEventHandler( //do code after work
                                delegate (object o, RunWorkerCompletedEventArgs args)
                                {
                                    if (newrec.name == null) newrec.setName("url");
                                    newrec.color = System.Drawing.ColorTranslator.FromHtml("#F2FFCC");
                                    this.diagram.InvalidateDiagram();
                                }
                            )
                        );
                    }

                    this.diagram.unsave();
                }
                else
                {                                                      // Spracovanie textu zo schranky
                    newrec.setName(ClipText);
                    

                    if (Os.FileExists(ClipText))
                    {
                        newrec.setName(Os.getFileName(ClipText));
                        newrec.link = Os.makeRelative(ClipText, this.diagram.FileName);
                        newrec.color = Media.getColor(diagram.options.colorFile);
                    }

                    if (Os.DirectoryExists(ClipText))
                    {
                        newrec.setName(Os.getFileName(ClipText));
                        newrec.link = Os.makeRelative(ClipText, this.diagram.FileName);
                        newrec.color = Media.getColor(diagram.options.colorDirectory);
                    }

                    this.diagram.unsave();
                }

                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
            else
            if (Clipboard.ContainsFileDropList()) // [FILES] [PASTE] insert files from clipboard
            {
                System.Collections.Specialized.StringCollection returnList = Clipboard.GetFileDropList();
                foreach (string file in returnList)
                {
                    Node newrec = this.CreateNode(position);
                    newrec.setName(Os.getFileNameWithoutExtension(file));

                    // odstranenie absolutnej cesty
                    string ext = Os.getExtension(file);

                    if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp") // [PASTE] [IMAGE] [FILE NAME] skratenie cesty k suboru
                    {
                        this.diagram.setImage(newrec, file);
                    }
                    else
                        if (this.diagram.FileName != "" && Os.FileExists(this.diagram.FileName) && file.IndexOf(new FileInfo(this.diagram.FileName).DirectoryName) == 0) // [PASTE] [FILE] - skratenie cesty k suboru
                    {
                        int start = new FileInfo(this.diagram.FileName).DirectoryName.Length;
                        int finish = file.Length - start;
                        newrec.link = "." + file.Substring(start, finish);
                    }
                    else
                            if (this.diagram.FileName != "" && Os.DirectoryExists(this.diagram.FileName)) // [PASTE] [DIRECTORY] - skatenie cesty k adresaru
                    {
                        int start = new FileInfo(this.diagram.FileName).DirectoryName.Length;
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
            else if (Clipboard.GetDataObject() != null)  // [PASTE] [IMAGE] [CLIPBOARD OBJECT] image
            {
                IDataObject data = Clipboard.GetDataObject();

                if (data.GetDataPresent(DataFormats.Bitmap))
                {
                    // paste image end embedded
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
                    Node newrec = this.CreateNode(this.getMousePosition());

                    newrec.note = ClipText;
                    this.diagram.unsave();
                }
                else
                {
                    foreach (Node rec in this.selectedNodes)
                    {
                        if (rec.note != "") // apend to node note
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
                    newrec.color = System.Drawing.ColorTranslator.FromHtml("#8AC5FF");

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
                newrec.color = System.Drawing.ColorTranslator.FromHtml("#8AC5FF");

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
            newrec.color = System.Drawing.ColorTranslator.FromHtml("#8AC5FF");

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
                Position position = new Position(
                    (int)((this.shift.x + rec.position.x) / this.scale),
                    (int)((this.shift.y + rec.position.y) / this.scale)
                );
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

        // NODE move nodes to left
        public void moveNodesToLeft(bool quick = false)
        {
            if (this.selectedNodes.Count() > 0)
            {
                int speed = (quick) ? this.diagram.options.keyArrowSlowSpeed : this.diagram.options.keyArrowFastSpeed;
                foreach (Node rec in this.selectedNodes)
                {
                    rec.position.x -= speed;

                    if (rec.haslayer)
                    {
                        rec.layerShift.x += speed;
                    }
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

                    if (rec.haslayer)
                    {
                        rec.layerShift.x -= speed;
                    }
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

                    if (rec.haslayer)
                    {
                        rec.layerShift.y += speed;
                    }
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

                    if (rec.haslayer)
                    {
                        rec.layerShift.y -= speed;
                    }
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
                    newrec.color = Media.getColor(diagram.options.colorAttachment);
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
                    newrec.color = Media.getColor(diagram.options.colorAttachment);
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
        public List<Line> getSelectedLines()
        {
            List<Line> SelectedLinesTemp = new List<Line>();
            int id = 0;

            foreach (Node srec in this.selectedNodes) {
                id = srec.id;
                foreach (Line lin in this.diagram.layers.getAllLines()) {
                    if (lin.start == id) {
                        SelectedLinesTemp.Add(lin);
                    }
                }
            }

            List<Line> SelectedLines = new List<Line>();

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

        // LINE selectcolor
        public void selectLineColor()
        {
            if (!this.diagram.options.readOnly)
            {
                if (this.selectedNodes.Count() > 0)
                {
                    //DColor.Color = this.SelectedNodes[0].color;

                    if (DColor.ShowDialog() == DialogResult.OK)
                    {

                        List<Line> SelectedLines = getSelectedLines();

                        foreach (Line lin in SelectedLines)
                        {
                            lin.color = DColor.Color;
                        }


                        this.diagram.InvalidateDiagram();
                    }
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
    }
}
