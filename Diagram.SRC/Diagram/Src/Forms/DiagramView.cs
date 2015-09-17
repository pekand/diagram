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

        private System.Windows.Forms.ContextMenuStrip PopupMenu;
        public System.Windows.Forms.SaveFileDialog DSave;
        private System.Windows.Forms.OpenFileDialog DOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ColorDialog CDialog;
        private System.Windows.Forms.ToolStripMenuItem layerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outToolStripMenuItem;
        private System.Windows.Forms.Timer MoveTimer;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coordinatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readonlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openlinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copylinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bordersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultFontToolStripMenuItem;
        private System.Windows.Forms.FontDialog defaultfontDialog;
        private System.Windows.Forms.ToolStripMenuItem alignToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inColumnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeShortcutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem encryptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visitWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToPngToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog exportFile;

        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveTextFileDialog;

        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem newViewToolStripMenuItem;
        private ToolStripMenuItem resetFontToolStripMenuItem;
        private ToolStripMenuItem setStartPositionToolStripMenuItem;

        /*************************************************************************************************************************/

        // ATRIBUTES SCREEN
        public Position shift = new Position();                   //poun horneho rohu obrazovky;

        public Position startShift = new Position();              //docasne ulozenie pozicie obrazovky pred zmenou

        // ATTRIBUTES MOUSE
        public Position startMousePos = new Position();           //docasne ulozenie pozicie mysi
        public Position startNodePos = new Position();            //docasne ulozenie pozicie zaciatocnej nody
        public Position vmouse = new Position();                  //vektor posunutia mysi vo vybranom obdlzniku
        public Position actualMousePos = new Position();          //priebezna pozicia mysi vo forme pri tahani

        // ATTRIBUTES KEYBOARD
        public char key = ' ';                   // posledne zachytene pismenko
        public bool keyshift = false;            // detekovanie klavesovich modifikatorov pri mysi
        public bool keyctrl = false;
        public bool keyalt = false;

        // ATTRIBUTES STATES
        public bool drag = false;                // psuvanie nody
        public bool move = false;                // posunutie objektu
        public bool selecting = false;           // vytvorenie nody tahanim alebo vyber viacerich prvkov
        public bool addingNode = false;          // pidavanie nody tahanim
        public bool dblclick = false;            // dvojklik na plochu
        public bool zooming = false;             // zmensenie plochy
        public bool searching = false;           // search panel is show and focused

        // ATTRIBUTES ZOOMING
        public float zoomingDefaultScale = 1;      // zmensenie plochy - normalne zvetsenie
        public float zoomingScale = 4;             // zmensenie plochy - zvetsenie nahlad
        public float scale = 1;                    // zmensenie plochy - aktualne zvetsenie

        // ATTRIBUTES Diagram
        public Diagram diagram = null;       // diagram ktory je previazany z pohladom

        public Node SourceNode = null;                             // Vybrata noda - obdlznik s ktorou sa robi operacia pomocou mysy
        public List<Node> SelectedNodes = new List<Node>();  // Zoznam vybratych nod (Zatial sa nepouziva)

        // ATTRIBUTES Layers
        public int layer = 0;                      // vrstva v ktorej sa program nachádza (0 je najvrchnejšia)
        public Node LayerNode = null;        // Vybrata noda - obdlznik
        public Position firstLayereShift = new Position();          // poun horneho rohu obrazovky v najvrchnejsom layery
        public List<int> Layers = new List<int>(); // zoznam vnorenich layerov, na konci je posledny zobrazeny layer    ;

        // COMPONENTS
        public ScrollBar bottomScrollBar = null;
        public ScrollBar rightScrollBar = null;

        // EDITPANEL
        public Node prevSelectedNode = null;
        public bool editingNodeName = false; // panel je zobrazený
        public Panel nodeNamePanel = null; // panel margin pre edit form
        public TextBox nodeNameEdit = null; // edit pre nove meno nody

        // OTHER
        private ToolStripMenuItem groupToolStripMenuItem;
        private IContainer components;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeShortcutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openlinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copylinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.alignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToPngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setStartPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encryptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readonlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bordersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visitWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DSave = new System.Windows.Forms.SaveFileDialog();
            this.DOpen = new System.Windows.Forms.OpenFileDialog();
            this.CDialog = new System.Windows.Forms.ColorDialog();
            this.MoveTimer = new System.Windows.Forms.Timer(this.components);
            this.defaultfontDialog = new System.Windows.Forms.FontDialog();
            this.exportFile = new System.Windows.Forms.SaveFileDialog();
            this.saveTextFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PopupMenu.SuspendLayout();
            this.SuspendLayout();
            //
            // PopupMenu
            //
            this.PopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.colorToolStripMenuItem,
            this.removeShortcutToolStripMenuItem,
            this.linkToolStripMenuItem,
            this.alignToolStripMenuItem,
            this.toolStripMenuItem2,
            this.fileToolStripMenuItem,
            this.screenToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.layerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.optionToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.PopupMenu.Name = "contextMenuStrip1";
            this.PopupMenu.Size = new System.Drawing.Size(165, 280);
            this.PopupMenu.Opening += new System.ComponentModel.CancelEventHandler(this.PopupMenu_Opening);
            //
            // editToolStripMenuItem
            //
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            //
            // colorToolStripMenuItem
            //
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.colorToolStripMenuItem.Text = "Color";
            this.colorToolStripMenuItem.Click += new System.EventHandler(this.colorToolStripMenuItem_Click);
            //
            // removeShortcutToolStripMenuItem
            //
            this.removeShortcutToolStripMenuItem.Name = "removeShortcutToolStripMenuItem";
            this.removeShortcutToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.removeShortcutToolStripMenuItem.Text = "Remove shortcut";
            this.removeShortcutToolStripMenuItem.Click += new System.EventHandler(this.removeShortcutToolStripMenuItem_Click);
            //
            // linkToolStripMenuItem
            //
            this.linkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openlinkToolStripMenuItem,
            this.copylinkToolStripMenuItem,
            this.openDirectoryToolStripMenuItem1});
            this.linkToolStripMenuItem.Name = "linkToolStripMenuItem";
            this.linkToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.linkToolStripMenuItem.Text = "Link";
            //
            // openlinkToolStripMenuItem
            //
            this.openlinkToolStripMenuItem.Name = "openlinkToolStripMenuItem";
            this.openlinkToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.openlinkToolStripMenuItem.Text = "Open";
            this.openlinkToolStripMenuItem.Click += new System.EventHandler(this.openlinkToolStripMenuItem1_Click);
            //
            // copylinkToolStripMenuItem
            //
            this.copylinkToolStripMenuItem.Name = "copylinkToolStripMenuItem";
            this.copylinkToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.copylinkToolStripMenuItem.Text = "Copy";
            this.copylinkToolStripMenuItem.Click += new System.EventHandler(this.copylinkToolStripMenuItem_Click_1);
            //
            // openDirectoryToolStripMenuItem1
            //
            this.openDirectoryToolStripMenuItem1.Name = "openDirectoryToolStripMenuItem1";
            this.openDirectoryToolStripMenuItem1.Size = new System.Drawing.Size(153, 22);
            this.openDirectoryToolStripMenuItem1.Text = "Open directory";
            this.openDirectoryToolStripMenuItem1.Click += new System.EventHandler(this.openDirectoryToolStripMenuItem1_Click);
            //
            // alignToolStripMenuItem
            //
            this.alignToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftToolStripMenuItem,
            this.rightToolStripMenuItem,
            this.toLineToolStripMenuItem,
            this.inColumnToolStripMenuItem,
            this.groupToolStripMenuItem});
            this.alignToolStripMenuItem.Name = "alignToolStripMenuItem";
            this.alignToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.alignToolStripMenuItem.Text = "Align";
            //
            // leftToolStripMenuItem
            //
            this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
            this.leftToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.leftToolStripMenuItem.Text = "Left";
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            //
            // rightToolStripMenuItem
            //
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            this.rightToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rightToolStripMenuItem.Text = "Right";
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
            //
            // toLineToolStripMenuItem
            //
            this.toLineToolStripMenuItem.Name = "toLineToolStripMenuItem";
            this.toLineToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.toLineToolStripMenuItem.Text = "To line";
            this.toLineToolStripMenuItem.Click += new System.EventHandler(this.toLineToolStripMenuItem_Click);
            //
            // inColumnToolStripMenuItem
            //
            this.inColumnToolStripMenuItem.Name = "inColumnToolStripMenuItem";
            this.inColumnToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.inColumnToolStripMenuItem.Text = "In column";
            this.inColumnToolStripMenuItem.Click += new System.EventHandler(this.inColumnToolStripMenuItem_Click);
            //
            // toolStripMenuItem2
            //
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(161, 6);
            //
            // fileToolStripMenuItem
            //
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.openToolStripMenuItem,
            this.openDirectoryToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.fileToolStripMenuItem.Text = "File";
            //
            // newToolStripMenuItem
            //
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            //
            // saveToolStripMenuItem
            //
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            //
            // saveAsToolStripMenuItem
            //
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            //
            // exportToolStripMenuItem
            //
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textToolStripMenuItem,
            this.exportToPngToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.exportToolStripMenuItem.Text = "Export";
            //
            // textToolStripMenuItem
            //
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.textToolStripMenuItem.Text = "Text";
            this.textToolStripMenuItem.Click += new System.EventHandler(this.textToolStripMenuItem_Click);
            //
            // exportToPngToolStripMenuItem
            //
            this.exportToPngToolStripMenuItem.Name = "exportToPngToolStripMenuItem";
            this.exportToPngToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.exportToPngToolStripMenuItem.Text = "Export to png";
            this.exportToPngToolStripMenuItem.Click += new System.EventHandler(this.exportToPngToolStripMenuItem_Click);
            //
            // openToolStripMenuItem
            //
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            //
            // openDirectoryToolStripMenuItem
            //
            this.openDirectoryToolStripMenuItem.Name = "openDirectoryToolStripMenuItem";
            this.openDirectoryToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.openDirectoryToolStripMenuItem.Text = "Open Directory";
            this.openDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openDirectoryToolStripMenuItem_Click);
            //
            // exitToolStripMenuItem
            //
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            //
            // screenToolStripMenuItem
            //
            this.screenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.centerToolStripMenuItem,
            this.setStartPositionToolStripMenuItem});
            this.screenToolStripMenuItem.Name = "screenToolStripMenuItem";
            this.screenToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.screenToolStripMenuItem.Text = "Screen";
            //
            // centerToolStripMenuItem
            //
            this.centerToolStripMenuItem.Name = "centerToolStripMenuItem";
            this.centerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.centerToolStripMenuItem.Text = "Center";
            this.centerToolStripMenuItem.Click += new System.EventHandler(this.centerToolStripMenuItem_Click);
            //
            // setStartPositionToolStripMenuItem
            //
            this.setStartPositionToolStripMenuItem.Name = "setStartPositionToolStripMenuItem";
            this.setStartPositionToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.setStartPositionToolStripMenuItem.Text = "Set start position";
            this.setStartPositionToolStripMenuItem.Click += new System.EventHandler(this.setStartPositionToolStripMenuItem_Click);
            //
            // viewToolStripMenuItem
            //
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newViewToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.viewToolStripMenuItem.Text = "View";
            //
            // newViewToolStripMenuItem
            //
            this.newViewToolStripMenuItem.Name = "newViewToolStripMenuItem";
            this.newViewToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.newViewToolStripMenuItem.Text = "New View";
            this.newViewToolStripMenuItem.Click += new System.EventHandler(this.newViewToolStripMenuItem_Click);
            //
            // layerToolStripMenuItem
            //
            this.layerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inToolStripMenuItem,
            this.outToolStripMenuItem});
            this.layerToolStripMenuItem.Name = "layerToolStripMenuItem";
            this.layerToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.layerToolStripMenuItem.Text = "Layer";
            //
            // inToolStripMenuItem
            //
            this.inToolStripMenuItem.Name = "inToolStripMenuItem";
            this.inToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.inToolStripMenuItem.Text = "In";
            this.inToolStripMenuItem.Click += new System.EventHandler(this.inToolStripMenuItem_Click);
            //
            // outToolStripMenuItem
            //
            this.outToolStripMenuItem.Name = "outToolStripMenuItem";
            this.outToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.outToolStripMenuItem.Text = "Out";
            this.outToolStripMenuItem.Click += new System.EventHandler(this.outToolStripMenuItem_Click);
            //
            // toolStripMenuItem1
            //
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            //
            // optionToolStripMenuItem
            //
            this.optionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encryptToolStripMenuItem,
            this.changePasswordToolStripMenuItem,
            this.readonlyToolStripMenuItem,
            this.gridToolStripMenuItem,
            this.coordinatesToolStripMenuItem,
            this.bordersToolStripMenuItem,
            this.defaultFontToolStripMenuItem,
            this.resetFontToolStripMenuItem});
            this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            this.optionToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.optionToolStripMenuItem.Text = "Option";
            //
            // encryptToolStripMenuItem
            //
            this.encryptToolStripMenuItem.Name = "encryptToolStripMenuItem";
            this.encryptToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.encryptToolStripMenuItem.Text = "Encrypt";
            this.encryptToolStripMenuItem.Click += new System.EventHandler(this.encryptToolStripMenuItem_Click);
            //
            // changePasswordToolStripMenuItem
            //
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.changePasswordToolStripMenuItem.Text = "Change password";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            //
            // readonlyToolStripMenuItem
            //
            this.readonlyToolStripMenuItem.CheckOnClick = true;
            this.readonlyToolStripMenuItem.Name = "readonlyToolStripMenuItem";
            this.readonlyToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.readonlyToolStripMenuItem.Text = "Read only";
            this.readonlyToolStripMenuItem.Click += new System.EventHandler(this.readonlyToolStripMenuItem_Click);
            //
            // gridToolStripMenuItem
            //
            this.gridToolStripMenuItem.Checked = true;
            this.gridToolStripMenuItem.CheckOnClick = true;
            this.gridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
            this.gridToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.gridToolStripMenuItem.Text = "Grid";
            this.gridToolStripMenuItem.Click += new System.EventHandler(this.gridToolStripMenuItem_Click);
            //
            // coordinatesToolStripMenuItem
            //
            this.coordinatesToolStripMenuItem.CheckOnClick = true;
            this.coordinatesToolStripMenuItem.Name = "coordinatesToolStripMenuItem";
            this.coordinatesToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.coordinatesToolStripMenuItem.Text = "Coordinates";
            this.coordinatesToolStripMenuItem.Visible = false;
            this.coordinatesToolStripMenuItem.Click += new System.EventHandler(this.coordinatesToolStripMenuItem_Click);
            //
            // bordersToolStripMenuItem
            //
            this.bordersToolStripMenuItem.CheckOnClick = true;
            this.bordersToolStripMenuItem.Name = "bordersToolStripMenuItem";
            this.bordersToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.bordersToolStripMenuItem.Text = "Borders";
            this.bordersToolStripMenuItem.Click += new System.EventHandler(this.bordersToolStripMenuItem_Click);
            //
            // defaultFontToolStripMenuItem
            //
            this.defaultFontToolStripMenuItem.Name = "defaultFontToolStripMenuItem";
            this.defaultFontToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.defaultFontToolStripMenuItem.Text = "Default font";
            this.defaultFontToolStripMenuItem.Click += new System.EventHandler(this.defaultFontToolStripMenuItem_Click);
            //
            // resetFontToolStripMenuItem
            //
            this.resetFontToolStripMenuItem.Name = "resetFontToolStripMenuItem";
            this.resetFontToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.resetFontToolStripMenuItem.Text = "Reset font";
            this.resetFontToolStripMenuItem.Click += new System.EventHandler(this.resetFontToolStripMenuItem_Click);
            //
            // helpToolStripMenuItem
            //
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consoleToolStripMenuItem,
            this.visitWebsiteToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.helpToolStripMenuItem.Text = "Help";
            //
            // consoleToolStripMenuItem
            //
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.consoleToolStripMenuItem.Text = "Debug Console";
            this.consoleToolStripMenuItem.Visible = false;
            this.consoleToolStripMenuItem.Click += new System.EventHandler(this.consoleToolStripMenuItem_Click);
            //
            // visitWebsiteToolStripMenuItem
            //
            this.visitWebsiteToolStripMenuItem.Name = "visitWebsiteToolStripMenuItem";
            this.visitWebsiteToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.visitWebsiteToolStripMenuItem.Text = "Visit homesite";
            this.visitWebsiteToolStripMenuItem.Click += new System.EventHandler(this.visitWebsiteToolStripMenuItem_Click);
            //
            // aboutToolStripMenuItem
            //
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
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
            // groupToolStripMenuItem
            //
            this.groupToolStripMenuItem.Name = "groupToolStripMenuItem";
            this.groupToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.groupToolStripMenuItem.Text = "Group";
            this.groupToolStripMenuItem.Click += new System.EventHandler(this.groupToolStripMenuItem_Click);
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
            this.PopupMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        /*************************************************************************************************************************/

        // MENU Manage                                                                                // POPUP MENU
        public void PopupMenu_Opening(object sender, CancelEventArgs e)
        {
            if (this.SelectedNodes.Count() == 0)
            {
                editToolStripMenuItem.Visible = false;
                colorToolStripMenuItem.Visible = false;
                linkToolStripMenuItem.Visible = false;
                copylinkToolStripMenuItem.Enabled = false;
                openlinkToolStripMenuItem.Enabled = false;
                toolStripMenuItem2.Visible = false;//separator
                alignToolStripMenuItem.Visible = false;
                removeShortcutToolStripMenuItem.Visible = false;
                openDirectoryToolStripMenuItem1.Visible = false;
            }

            if (this.SelectedNodes.Count() == 1)
            {
                editToolStripMenuItem.Visible = true;
                colorToolStripMenuItem.Visible = true;
                linkToolStripMenuItem.Visible = this.SelectedNodes[0].link.Trim() != "";
                copylinkToolStripMenuItem.Enabled = this.SelectedNodes[0].link.Trim() != "";
                openlinkToolStripMenuItem.Enabled = this.SelectedNodes[0].link.Trim() != "";
                toolStripMenuItem2.Visible = true;//separator
                alignToolStripMenuItem.Visible = false;
                openDirectoryToolStripMenuItem1.Visible = false;
                if (this.SelectedNodes[0].link.Trim().Length > 0 && File.Exists(this.SelectedNodes[0].link))
                    openDirectoryToolStripMenuItem1.Visible = true;

            }

            if (this.SelectedNodes.Count() > 1)
            {
                editToolStripMenuItem.Visible = false;
                colorToolStripMenuItem.Visible = true;
                linkToolStripMenuItem.Visible = false;
                copylinkToolStripMenuItem.Enabled = false;
                openlinkToolStripMenuItem.Enabled = false;
                toolStripMenuItem2.Visible = true; //separator
                alignToolStripMenuItem.Visible = true;
                removeShortcutToolStripMenuItem.Visible = false;
                openDirectoryToolStripMenuItem1.Visible = false;
            }

            if (this.SelectedNodes.Count() > 0)
            {
                bool hasShortcut = false;
                foreach (Node node in this.SelectedNodes)
                {
                    if (node.shortcut > 0) {
                        hasShortcut = true;
                        break;
                    }
                }

                if (hasShortcut) {
                    removeShortcutToolStripMenuItem.Visible = true;
                }
            }

            if (this.diagram.password == "")
            {
                changePasswordToolStripMenuItem.Visible = false;
                encryptToolStripMenuItem.Visible = true;
            }
            else
            {
                changePasswordToolStripMenuItem.Visible = true;
                encryptToolStripMenuItem.Visible = false;
            }

        }

        // MENU Edit
        public void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() == 1)
            {
                this.diagram.EditNode(this.SelectedNodes[0]);
            }
        }

        // MENU Link Open
        public void openlinkToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() > 0)
            {
                this.OpenLinkAsync(this.SelectedNodes[0]);
            }
        }

        // MENU Link Copy
        public void copylinkToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() > 0)
            {
                this.copyLinkToClipboard(this.SelectedNodes[0]);
            }
        }

        // MENU New
        public void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            main.OpenDiagram();
        }

        // MENU Save
        public void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.save();
        }

        // MENU Save As
        public void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveas();
        }

        // MENU export to png
        private void exportToPngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (exportFile.ShowDialog() == DialogResult.OK)
            {
                this.exportDiagramToPng();
            }
        }

        // MENU export to txt
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveTextFileDialog.ShowDialog() == DialogResult.OK)
            {
                exportDiagramToTxt(saveTextFileDialog.FileName);
            }
        }

        // MENU Open
        public void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DOpen.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(DOpen.FileName))
                {
                    if (Path.GetExtension(DOpen.FileName).ToLower() == ".diagram")
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

        // MENU Open Directory  - otvory adresar v ktorom sa nachadza prave otvreny subor
        public void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDiagramDirectory();
        }

        // MENU Encription
        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (main.newPasswordForm == null)
            {
                main.newPasswordForm = new NewPasswordForm(main);
            }

            main.newPasswordForm.Clear();
            main.newPasswordForm.ShowDialog();
            if (!main.newPasswordForm.cancled)
            {
                this.diagram.password = main.newPasswordForm.GetPassword();
                this.diagram.unsave();
            }
        }

        // MENU Change password
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (main.changePasswordForm == null)
            {
                main.changePasswordForm = new ChangePasswordForm(main);
            }

            main.changePasswordForm.Clear();
            main.changePasswordForm.oldpassword = this.diagram.password;
            main.changePasswordForm.ShowDialog();
            if (!main.changePasswordForm.cancled)
            {
                this.diagram.password = main.changePasswordForm.GetPassword();
                this.diagram.unsave();
            }
        }

        // MENU Console
        public void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.showConsole();
        }

        // MENU Exit
        public void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // MENU Center
        public void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GoToHome();
        }

        // MENU set home position
        private void setStartPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setCurentPositionAsHomePosition();
        }

        // MENU Read only
        public void readonlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagram.options.readOnly = this.readonlyToolStripMenuItem.Checked;
        }

        // MENU Grid check
        public void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagram.options.grid = this.gridToolStripMenuItem.Checked;
            this.diagram.InvalidateDiagram();
        }

        // MENU Option Borders
        public void bordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagram.options.borders = this.bordersToolStripMenuItem.Checked;
            this.diagram.InvalidateDiagram();
        }

        // MENU Option Default font
        public void defaultFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            defaultfontDialog.Font = this.diagram.FontDefault;
            if (defaultfontDialog.ShowDialog() == DialogResult.OK)
            {
                if (!this.diagram.options.readOnly)
                {
                    this.diagram.FontDefault = defaultfontDialog.Font;
                }
            }
        }

        // MENU coordinates
        public void coordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagram.options.coordinates = this.coordinatesToolStripMenuItem.Checked;
            this.diagram.InvalidateDiagram();
        }

        // MENU Change color
        public void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedNodes.Count() > 0)
            {
                CDialog.Color = this.SelectedNodes[0].color;

                if (CDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!this.diagram.options.readOnly)
                    {
                        if (SelectedNodes.Count() > 0)
                        {
                            foreach (Node rec in this.SelectedNodes)
                            {
                                rec.color = CDialog.Color;
                            }
                        }
                    }

                    this.diagram.InvalidateDiagram();
                }
            }
        }

        // MENU VIEW NEW VIEW
        private void newViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // otvorenie novej insancie DiagramView
            this.diagram.openDiagramView();
        }

        // MENU Layer In
        public void inToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() == 1)
            {
                this.LayerIn(this.SelectedNodes[0]);
            }
        }

        // MENU Layer Out
        public void outToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayerOut();
        }

        // MENU align left
        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() > 0)
            {
                this.diagram.AlignLeft(this.SelectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // MENU align right
        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() > 0)
            {
                this.diagram.AlignRight(this.SelectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // MENU align to line
        private void toLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() > 0)
            {
                this.diagram.AlignToLine(this.SelectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // MENU align to column
        private void inColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() > 0)
            {
                this.diagram.AlignToColumn(this.SelectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // MENU align to group to column
        private void groupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() > 0)
            {
                this.diagram.AlignCompact(this.SelectedNodes);
                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }
        }

        // MENU remove shortcut
        private void removeShortcutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() > 0)
            {
                removeShortcuts(this.SelectedNodes);
            }
        }

        // MENU open directory for file in link
        private void openDirectoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
			this.openLinkDirectory();
        }

        // MENU reset font
        private void resetFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectedNodes.Count() > 0) {
                this.diagram.ResetFont(this.SelectedNodes);
            }
            else
            {
                this.diagram.ResetFont();
            }
        }

        // MENU Homepage navštíviť domovskú stránku
        private void visitWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Network.openUrl(main.parameters.home_page);
        }

        // MENU About navštíviť domovskú stránku
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (main.aboutForm == null)
            {
                 main.aboutForm = new AboutForm(main);
            }

            main.aboutForm.Show();
        }


        /*************************************************************************************************************************/

        // FORM Constructor
        public DiagramView(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        // FORM Load event -
        public void DiagramViewLoad(object sender, EventArgs e)
        {
            //Only in debug
#if DEBUG
            consoleToolStripMenuItem.Visible = true;
            coordinatesToolStripMenuItem.Visible = true;
#endif

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

            //LAYER open - zostavenie historie nody
            if (this.diagram.options.layer != 0)
            {
                this.layer = this.diagram.options.layer;
                this.BuildLayerHistory(this.diagram.options.layer);
            }

            this.gridToolStripMenuItem.Checked = this.diagram.options.grid;
            this.bordersToolStripMenuItem.Checked = this.diagram.options.borders;
            this.coordinatesToolStripMenuItem.Checked = this.diagram.options.coordinates;
            this.readonlyToolStripMenuItem.Checked = this.diagram.options.readOnly;

            this.inicializeNodeNamePanel();

            this.shift.x = diagram.options.homePosition.x;
            this.shift.y = diagram.options.homePosition.y;
        }

        // FORM Quit Close
        public void DiagramApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool close = true;
            if (!this.diagram.SavedFile && (this.diagram.FileName == "" || !File.Exists(this.diagram.FileName))) // Ulozi ako novy subor
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
            else if (!this.diagram.SavedFile && this.diagram.FileName != "" && File.Exists(this.diagram.FileName)) //ulozenie do aktualne otvoreneho suboru
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

            if (close)
            {
                this.CloseFile();
            }
            e.Cancel = !close;

        }

        // FORM Title - Nastavi hlavičku fomuláru
        public void SetTitle()
        {
            if (this.diagram.FileName.Trim() != "")
                this.Text = Path.GetFileNameWithoutExtension(this.diagram.FileName);
            else
                this.Text = "Diagram";
            if (this.LayerNode != null && this.LayerNode.text.Trim() != "")
                this.Text += " - " + this.LayerNode.text.Trim();
            if (!this.diagram.SavedFile)
                this.Text = "*" + this.Text;
        }

        // FORM go to home position - center window to home position
        public void GoToHome()
        {
            if (this.LayerNode != null)
            {
                this.shift.x = -this.LayerNode.position.x + this.ClientSize.Width / 2;
                this.shift.y = -this.LayerNode.position.y + this.ClientSize.Height / 2;
            }
            else
            {
                this.shift.x = diagram.options.homePosition.x;
                this.shift.y = diagram.options.homePosition.y;
            }
            this.diagram.InvalidateDiagram();
        }

        // FORM set home position - Centrovanie obrazovky
        public void setCurentPositionAsHomePosition()
        {
            diagram.options.homePosition.x = this.shift.x;
            diagram.options.homePosition.y = this.shift.y;
        }

        // FORM go to end position - senter window to second remembered position
        public void GoToEnd()
        {
            if (this.LayerNode != null)
            {
                this.shift.x = -this.LayerNode.position.x + this.ClientSize.Width / 2;
                this.shift.y = -this.LayerNode.position.y + this.ClientSize.Height / 2;
            }
            else
            {
                this.shift.x = diagram.options.endPosition.x;
                this.shift.y = diagram.options.endPosition.y;
            }
            this.diagram.InvalidateDiagram();
        }

        // FORM set end position - center window
        public void setCurentPositionAsEndPosition()
        {
            diagram.options.endPosition.x = this.shift.x;
            diagram.options.endPosition.y = this.shift.y;
        }

       /*************************************************************************************************************************/

        // SELECTION Zisti ci je noda vo vybere
        public bool isselected(Node a)
        {
            if (a == null) return false;

            bool found = false;

            if (this.SelectedNodes.Count() > 0)
            {
                foreach (Node rec in this.SelectedNodes) // Loop through List with foreach
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

        // SELECTION Clear selection - odstranenie prvkov z vyberu
        public void ClearSelection()
        {
            if (this.SelectedNodes.Count() > 0) // odstranenie mulitvyberu
            {
                foreach (Node rec in this.SelectedNodes)
                {
                    rec.selected = false;
                }

                this.SelectedNodes.Clear();
            }
        }

        // SELECTION Remove Node from  selection
        public void RemoveNodeFromSelection(Node a)
        {
            if (this.SelectedNodes.Count() > 0 && a!=null) // odstranenie mulitvyberu
            {
                for (int i = this.SelectedNodes.Count() - 1; i >= 0; i--) // Loop through List with foreach
                {
                    if (this.SelectedNodes[i] == a)
                    {
                        this.SelectedNodes[i].selected = false;
                        this.SelectedNodes.RemoveAt(i);
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
                //this.ClearSelection();
                rec.selected = true;
                this.SelectedNodes.Add(rec);
            }
        }

        /*************************************************************************************************************************/

        // EVENT Paint                                                                                 // [PAINT] [EVENT]
        public void DiagramApp_Paint(object sender, PaintEventArgs e)
        {
            PaintDiagram(e.Graphics);
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

            if (this.nodeNameEdit.Focused)
            {
                this.ActiveControl = null;
            }

            this.startMousePos.x = e.X;  // povodna pozicia mysi
            this.startMousePos.y = e.Y;
            this.startShift.x = this.shift.x;  //povodne odsadenie
            this.startShift.y = this.shift.y;

            if (e.Button == MouseButtons.Left)
            {
                this.SourceNode = this.findNodeInMousePosition(e.X, e.Y);

                if (bottomScrollBar!=null && bottomScrollBar.MouseDown(e.X, e.Y))
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
                if (editingNodeName) // zavretie editačného panelu ak sa klikne mimo
                {
                    this.saveNodeNamePanel();
                }
                else
                if (this.SourceNode == null)
                {
                    this.actualMousePos.x = e.X;
                    this.actualMousePos.y = e.Y;
                    if (!this.diagram.options.readOnly && this.keyctrl && !this.keyshift && !this.keyalt) // pridanie nody tahanim
                    {
                        this.addingNode = true;
                        MoveTimer.Enabled = true;
                        this.ClearSelection();
                    }
                    else // multivyber
                    {
                        this.selecting = true;
                        MoveTimer.Enabled = true;
                    }
                }
                else if (this.SourceNode != null)
                {
                    //informacie potrebne pre tahanie
                    if (!this.diagram.options.readOnly)
                    {
                        this.drag = true;
                        MoveTimer.Enabled = true;
                        this.startNodePos.x = this.SourceNode.position.x; // povodna pozicia tahaneho objektu
                        this.startNodePos.y = this.SourceNode.position.y;
                        this.vmouse.x = (int)(e.X * this.scale - (this.shift.x + this.SourceNode.position.x)); // pozicia mysi vrámci stvorceku
                        this.vmouse.y = (int)(e.Y * this.scale - (this.shift.y + this.SourceNode.position.y));

                        if (!this.keyctrl && !this.isselected(this.SourceNode))
                        {
                            this.SelectOnlyOneNode(this.SourceNode);
                            this.diagram.InvalidateDiagram();
                        }
                    }
                }
            }
            else
            if (e.Button == MouseButtons.Right)
            {
                this.move = true; // popupmenu alebo posun obrazovky
            }
            else
            if (e.Button == MouseButtons.Middle)
            {
                if (!this.diagram.options.readOnly)
                {
                    this.actualMousePos.x = e.X;
                    this.actualMousePos.y = e.Y;
                    this.addingNode = true;// pridanie nody tahanim
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
                if (this.SourceNode != null)
                {
                    this.SourceNode.position.x = (int)(-this.shift.x + (e.X * this.scale - this.vmouse.x));
                    this.SourceNode.position.y = (int)(-this.shift.y + (e.Y * this.scale - this.vmouse.y));

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

            //int s = this.scale;

            int vectorx = e.X - this.startMousePos.x;
            int vectory = e.Y - this.startMousePos.y;

            MoveTimer.Enabled = false;

            if(dblclick)
            {
                this.selecting = false;
            }
            else
            // KEY DRAG
            if (finishdraging) // posunutie objektu
            {
                if (!this.diagram.options.readOnly)
                {
                    if (this.SourceNode != null) // navrat bodu po tom ako sa vytvori spojenie do povodnej pozicie
                    {
                        this.SourceNode.position.x = this.startNodePos.x;
                        this.SourceNode.position.y = this.startNodePos.y;
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
            // KEY DRAG+MRIGHT výber prvkou ktoré sa nachádzajú vo výbere
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
                    foreach (Node rec in diagram.Nodes) // LOOP through List with foreach
                    {
                        if (
                            (rec.layer == this.layer || rec.id == this.layer)
                            && -this.shift.x + a <= rec.position.x
                            && rec.position.x + rec.width <= -this.shift.x + c
                            && -this.shift.y + b <= rec.position.y
                            && rec.position.y + rec.height <= -this.shift.y + d)
                        {
                            if (keyshift && !keyctrl && !keyalt)
                            {
                                this.RemoveNodeFromSelection(rec);
                            }
                            else
                            {
                                this.SelectNode(rec);
                            }
                        }
                    }
                }

                this.diagram.InvalidateDiagram();
            }


            Node TargetNode = this.findNodeInMousePosition(e.X, e.Y);
            if (buttonleft) // MLEFT
            {

                if (bottomScrollBar != null && rightScrollBar!=null && (bottomScrollBar.MouseUp() || rightScrollBar.MouseUp()))
                {
                    this.diagram.InvalidateDiagram();
                }else
                // KEY MLEFT clear selection
                if (!mousemove && TargetNode == null && this.SourceNode == null && this.SelectedNodes.Count() > 0 && !keyalt && !keyctrl && !keyshift)
                {
                    this.ClearSelection();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+CTRL Skopirovanie obdlznika
                if (!isreadonly && keyctrl && TargetNode == null && this.SourceNode != null)
                {
                    this.AddDiagramPart(GetDiagramPart());
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+ALT vytvorenie nody a prepojenie s existujucou nodou
                if (!isreadonly && keyalt && TargetNode == null && this.SourceNode != null)
                {
                    var s = this.SourceNode;
                    var r = this.CreateNode(e.X, e.Y);
                    r.shortcut = s.id;
                    this.diagram.Connect(s,r,false);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+ALT vytvorenie prepojenia(skratky) = odkazu medzi objektami
                if (!isreadonly && keyalt && TargetNode != null && this.SourceNode != null && TargetNode != this.SourceNode)
                {
                    TargetNode.shortcut = this.SourceNode.id;
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG Posunutie obdlznika
                if
                (
                    !isreadonly &&
                    ((TargetNode == null && this.SourceNode != null) ||
                     (TargetNode != null && this.SourceNode != TargetNode && this.isselected(TargetNode)) ||
                    (TargetNode != null && this.SourceNode == TargetNode)) &&
                    Math.Sqrt(vectorx*vectorx+vectory*vectory) > 5
                )
                {
                    this.SourceNode.position.x = (int)(-this.shift.x + (e.X * this.scale - this.vmouse.x));
                    this.SourceNode.position.y = (int)(-this.shift.y + (e.Y * this.scale - this.vmouse.y));
                    if (this.SourceNode.id != this.layer && this.SourceNode.haslayer)
                    {
                        this.MoveLayer(this.SourceNode, (e.X - this.startMousePos.x), (e.Y - this.startMousePos.y));
                        this.SourceNode.layershiftx -= (e.X - this.startMousePos.x);
                        this.SourceNode.layershifty -= (e.Y - this.startMousePos.y);
                    }

                    if (this.SelectedNodes.Count() > 0)
                    {
                        var vx = this.SourceNode.position.x - this.startNodePos.x;
                        var vy = this.SourceNode.position.y - this.startNodePos.y;

                        foreach (Node rec in this.SelectedNodes) // Loop through List with foreach
                        {
                            if (rec != this.SourceNode)
                            {
                                rec.position.x = rec.position.x + vx;
                                rec.position.y = rec.position.y + vy;

                                if (rec.id != this.layer && rec.haslayer)
                                {
                                    this.MoveLayer(rec, vx, vy);
                                    rec.layershiftx -= vx;
                                    rec.layershifty -= vy;
                                }
                            }
                        }
                    }

                    this.diagram.unsave();

                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+CTRL Vytvorenie noveho obdlznika a spojenie s existujucim
                if (!isreadonly && keyctrl && TargetNode != null && this.SourceNode == null)
                {
                    this.diagram.Connect(this.CreateNode( +this.shift.x - startShift.x + this.startMousePos.x, +this.shift.y - startShift.y + this.startMousePos.y), TargetNode, false);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DBLCLICK otvorenie editacie alebo linku [dblclick]
                if (dblclick && this.SourceNode != null && !keyctrl && !keyalt && !keyshift)
                {
                    this.OpenLinkAsync(this.SourceNode);
                }
                else
                // KEY DBLCLICK+SHIFT otvorenie editacie
                if (dblclick && this.SourceNode != null && !keyctrl && !keyalt && keyshift)
                {
                    this.diagram.EditNode(this.SourceNode);
                }
                else
                // KEY DBLCLICK+CTRL otvorenie adresára ak má noda link na súbor alebo je adresár
                if (dblclick && this.SourceNode != null && keyctrl && !keyalt && !keyshift)
                {
                    if (this.SourceNode.link!="")
                    {
                        Os.openPathInSystem(this.SourceNode.link);
                    }
                }
                else
                // KEY DBLCLICK+SPACE presunutie sa v zoomingu na novú pozíciu
                if (dblclick && this.zooming && !keyctrl && !keyalt && !keyshift)
                {
                    this.shift.x = (int)(this.shift.x - (e.X * this.scale) + (this.ClientSize.Width * this.scale) / 2);
                    this.shift.y = (int)(this.shift.y - (e.Y * this.scale) + (this.ClientSize.Height * this.scale) / 2);
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY CTRL+SHIFT+MLEFT Vytvorenie novej nody a prepojenie s existujúcimi
                if (!isreadonly && keyshift && keyctrl && TargetNode == null && this.SelectedNodes.Count() >0 && this.SourceNode == null && e.X == this.startMousePos.x && e.Y == this.startMousePos.y)
                {
                    Node newrec = this.CreateNode(e.X - 10, e.Y - 10, false);
                    foreach (Node rec in this.SelectedNodes)
                    {
                        this.diagram.Connect(rec, newrec, false);
                    }
                    this.SelectOnlyOneNode(newrec);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY CTRL+MLEFT
                // KEY DBLCLICK Vytvorenie noveho obdlznika
                if (!isreadonly && (dblclick || keyctrl) && TargetNode == null && this.SourceNode == null && e.X == this.startMousePos.x && e.Y == this.startMousePos.y)
                {
                    this.CreateNode(e.X - 10, e.Y - 10, false);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG+CTRL Skopirovanie farby a stylu z jednej nody na druhu
                if (!isreadonly && !keyshift && keyctrl && TargetNode != null && this.SourceNode != null && this.SourceNode != TargetNode)
                {
                    if (this.SelectedNodes.Count() > 1)
                    {
                        foreach (Node rec in this.SelectedNodes)
                        {
                            rec.color = TargetNode.color;
                            rec.font = TargetNode.font;
                            rec.fontcolor = TargetNode.fontcolor;
                            rec.transparent = TargetNode.transparent;
                            SizeF s = this.diagram.MeasureStringWithMargin(rec.text, rec.font);
                            rec.width = (int)s.Width;
                            rec.height = (int)s.Height;
                        }
                    }

                    if (this.SelectedNodes.Count() == 1 || (this.SelectedNodes.Count() == 0 && this.SourceNode != null))
                    {
                        TargetNode.color = this.SourceNode.color;
                        TargetNode.font = this.SourceNode.font;
                        TargetNode.fontcolor = this.SourceNode.fontcolor;
                        TargetNode.transparent = this.SourceNode.transparent;
                        SizeF s = this.diagram.MeasureStringWithMargin(TargetNode.text, TargetNode.font);
                        TargetNode.width = (int)s.Width;
                        TargetNode.height = (int)s.Height;

                        if (this.SelectedNodes.Count() == 1 && this.SelectedNodes[0] != this.SourceNode)
                        {
                            this.ClearSelection();
                            this.SelectNode(this.SourceNode);
                        }
                    }
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                // KEY DRAG Pridanie spojovaciej čiary
                if (!isreadonly && TargetNode != null && this.SourceNode != null && this.SourceNode != TargetNode)
                {
                    bool arrow = false;
                    if (keyshift)
                    {
                        arrow = true;
                    }

                    if (this.SelectedNodes.Count() > 0)
                    {
                        foreach (Node rec in this.SelectedNodes)
                        {
                            if (rec != TargetNode)
                            {
                                if (keyctrl)
                                {
                                    this.diagram.Connect(TargetNode, rec, arrow);        // spojenie viacerich nody
                                }
                                else
                                {
                                    this.diagram.Connect(rec, TargetNode, arrow);        // spojenie viacerich nody
                                }
                            }
                        }
                    }
                    else
                    {
                        this.diagram.Connect(SourceNode, TargetNode, arrow);  // spojenie jednej vybratej nody
                    }

                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                // KEY CLICK+CTRL pridanie prvku do vyberu
                else if (keyctrl && this.SourceNode == TargetNode && TargetNode != null && !this.isselected(TargetNode))
                {
                    this.SelectNode(TargetNode);
                    this.diagram.InvalidateDiagram();
                }
                // KEY CLICK+CTRL odstránenie prvku z vyberu
                else if (keyctrl && TargetNode != null && (this.SourceNode == TargetNode || this.isselected(TargetNode)))
                {
                    this.RemoveNodeFromSelection(TargetNode);
                    this.diagram.InvalidateDiagram();
                }

            }
            else
            if (buttonright) // KEY MRIGHT
            {
                this.move = false; //zobrazenie popum okna
                if (e.X == this.startMousePos.x && e.Y == this.startMousePos.y && this.startShift.x == this.shift.x && this.startShift.y == this.shift.y)
                {
                    Node temp = this.findNodeInMousePosition(e.X, e.Y);
                    if (this.SelectedNodes.Count() > 0 && !this.isselected(temp))
                    {
                        this.ClearSelection();
                    }

                    if (this.SelectedNodes.Count() == 0 && this.SourceNode != temp)
                    {
                        this.SelectOnlyOneNode(temp);
                    }

                    this.diagram.InvalidateDiagram();
                    PopupMenu.Show(this.Left + e.X, this.Top + e.Y); // [POPUP] show popup
                }
                else { // KEY DRAG+MRIGHT presunutie obrazovky
                    this.shift.x = (int)(this.startShift.x + (e.X - this.startMousePos.x) * this.scale);
                    this.shift.y = (int)(this.startShift.y + (e.Y - this.startMousePos.y) * this.scale);
                    this.diagram.InvalidateDiagram();
                }
            }
            else
            if (buttonmiddle) // MMIDDLE
            {
                // KEY DRAG+MMIDDLE Vytvorenie noveho obdlznika a spojenie s existujucim
                if (!isreadonly && TargetNode != null)
                {
                    this.diagram.Connect(this.CreateNode(+this.shift.x - this.startShift.x + this.startMousePos.x, +this.shift.y - this.startShift.y + this.startMousePos.y), TargetNode);
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

                        this.zoomingScale = this.scale;
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

                        if (this.scale>1)
                            this.zoomingScale = this.scale;
                        else
                            this.zoomingScale = 4;
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

        private bool parseKey(string key, Keys keyData){

            string[] parts = key.Split('+');
            Keys keyCode = 0;
            Keys code = 0;

            foreach (string part in parts) {
                if (part == "CTRL")
                {
                    keyCode = Keys.Control | keyCode;
                    continue;
                }

                if (part == "ALT")
                {
                    keyCode = Keys.Alt | keyCode;
                    continue;
                }

                if (part == "SHIFT")
                {
                    keyCode = Keys.Shift | keyCode;
                    continue;
                }

                if (part == "PAGEUP")
                {
                    keyCode = Keys.PageUp | keyCode;
                    continue;
                }

                if (part == "PAGEDOWN")
                {
                    keyCode = Keys.PageDown | keyCode;
                    continue;
                }

                if (Enum.TryParse(Fonts.FirstCharToUpper(part), out code)) {
                    keyCode = code | keyCode;
                }
            }

            if (keyCode == keyData) {
               return  true;
            }

            return false;
        }

        // EVENT Shortcuts                                                                             // [KEYBOARD] [EVENT]
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.nodeNameEdit.Focused || this.searching)
            {
                return false;
            }

            /*
             * [doc] order : ProcessCmdKey, DiagramApp_KeyDown, DiagramApp_KeyPress, DiagramApp_KeyUp;
             */

            if (parseKey("CTRL+A", keyData) ) // [KEY] [CTRL+A] select all elements
            {
                this.ClearSelection();
                foreach (Node rec in this.diagram.Nodes)
                {
                    if(rec.layer == this.layer){
                        this.SelectNode(rec);
                    }
                }
                this.diagram.InvalidateDiagram();
            }

            if (parseKey("CTRL+L", keyData)) // [KEY] [CTRL+L] zarovnanie vybranych prvkov do roviny
            {
                if (this.SelectedNodes.Count() > 0)
                {
                    this.diagram.AlignToLine(this.SelectedNodes);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
            }

            if (parseKey("CTRL+H", keyData)) // [KEY] [CTRL+H] zarovnanie vybranych prvkov do stlpca
            {
                if (this.SelectedNodes.Count() > 0)
                {
                    this.diagram.AlignToColumn(this.SelectedNodes);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
            }


            if (parseKey("CTRL+K", keyData)) // [KEY] [CTRL+K] zarovnanie vybranych prvkov s pravidelným odstupom
            {
                if (this.SelectedNodes.Count() > 0)
                {
                    this.diagram.AlignCompact(this.SelectedNodes);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
            }

            if (parseKey("CTRL+C", keyData))  // [KEY] [CTRL+C]
            {
				if (this.SelectedNodes.Count() > 0)  // kopirovanie textu objektu
                {
                	DataObject data = new DataObject();

                    string copytext = "";
                    foreach (Node rec in this.SelectedNodes)
                    {
                        copytext = copytext + rec.text + "\n";
                    }

                    data.SetData(copytext);

                    data.SetData("DiagramXml", GetDiagramPart());//create and copy xml

					Clipboard.SetDataObject(data);

					return true;
                }

                return false;
            }

            if (parseKey("CTRL+SHIFT+C", keyData))  // [KEY] [CTRL+SHIFT+C] copy links from selected nodes
            {
                if (this.SelectedNodes.Count() > 0)
                {
                    string copytext = "";
                    foreach (Node rec in this.SelectedNodes)
                    {
						if (rec.link != null)
						{
							copytext = copytext + rec.link;

							if (this.SelectedNodes.Count () > 1) { //separate nodes
								copytext = copytext + "\n";
							}
						}
                    }
                    Clipboard.SetText(copytext);
                }
                return true;
            }

			if (parseKey("CTRL+ALT+SHIFT+C", keyData))  // [KEY] [CTRL+ALT+SHIFT+C] copy notes from selected nodes
			{
				if (this.SelectedNodes.Count() > 0)
				{
					string copytext = "";
					foreach (Node rec in this.SelectedNodes)
					{
						if (rec.note != null)
						{
							copytext = copytext + rec.note;

							if (this.SelectedNodes.Count () > 1) { //separate nodes
								copytext = copytext + "\n";
							}
						}
					}
					Clipboard.SetText(copytext);
				}
				return true;
			}

            if (parseKey("CTRL+V", keyData))  // [KEY] [CTRL+V] [PASTE] Vozenie textu zo schranky
            {
                DataObject retrievedData = (DataObject)Clipboard.GetDataObject();

                if (retrievedData.GetDataPresent("DiagramXml"))  // [PASTE] [DIAGRAM] [CLIPBOARD OBJECT] insert diagram
                {
                    this.AddDiagramPart(retrievedData.GetData("DiagramXml") as string);
                }
                else
                if (retrievedData.GetDataPresent(DataFormats.Text))  // [PASTE] [TEXT] insert text
                {
                    Point ptCursor = Cursor.Position;
                    ptCursor = PointToClient(ptCursor);
                    Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);

                    string ClipText = retrievedData.GetData(DataFormats.Text) as string;

                    if (Network.isURL(ClipText))  // [PASTE] [URL] [LINK] Spracovanie linku zo schranky
                    {
                        newrec.link = ClipText;
                        newrec.text = ClipText;

                        if (!Network.isHttpsURL(ClipText))
                        {
                            try
                            {
                                BackgroundWorker bw = new BackgroundWorker();

                                bw.WorkerReportsProgress = true;

                                bw.DoWork += new DoWorkEventHandler( // spusteny kod
                                delegate(object o, DoWorkEventArgs args)
                                {
                                    newrec.text = Network.GetWebPageTitle(ClipText);

                                });

                                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler( // kod spusteni po dokonceni
                                delegate(object o, RunWorkerCompletedEventArgs args)
                                {
                                    if (newrec.text == null) newrec.text = "url";
                                    SizeF s2 = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                                    newrec.width = (int)s2.Width;
                                    newrec.height = (int)s2.Height;
                                    newrec.color = System.Drawing.ColorTranslator.FromHtml("#F2FFCC");
                                    this.diagram.InvalidateDiagram();
                                });

                                bw.RunWorkerAsync();

                            }
                            catch (Exception ex)
                            {
                                Program.log.write("get link name error: " + ex.Message);
                            }
                        }

                        this.diagram.unsave();
                        SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                        newrec.width = (int)s.Width;
                        newrec.height = (int)s.Height;
                    }
                    else
                    {                                                      // Spracovanie textu zo schranky
                        newrec.text = ClipText;
                        this.diagram.unsave();

						if (File.Exists(ClipText))
                        {
							newrec.text = Path.GetFileName(ClipText);
                            SizeF s2 = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                        	newrec.width = (int)s2.Width;
                        	newrec.height = (int)s2.Height;
							newrec.link = Os.makeRelative(ClipText, this.diagram.FileName);
                            newrec.color = Media.getColor(diagram.options.colorFile);
						}

                        if (Directory.Exists(ClipText))
                        {
                            newrec.text = Path.GetFileName(ClipText);
                            SizeF s2 = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                            newrec.width = (int)s2.Width;
                            newrec.height = (int)s2.Height;
							newrec.link = Os.makeRelative(ClipText, this.diagram.FileName);
                            newrec.color = Media.getColor(diagram.options.colorDirectory);
                        }

                        SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                        newrec.width = (int)s.Width;
                        newrec.height = (int)s.Height;
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
                        Point ptCursor = Cursor.Position;
                        ptCursor = PointToClient(ptCursor);
                        Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);
                        newrec.text = Path.GetFileNameWithoutExtension(file); ;

                        SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                        newrec.width = (int)s.Width;
                        newrec.height = (int)s.Height;

                        // odstranenie absolutnej cesty
                        string ext = "";
                        if (file != "" && File.Exists(file))
                        {
                            ext = Path.GetExtension(file).ToLower();
                        }

                        if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp") // [PASTE] [IMAGE] [FILE NAME] skratenie cesty k suboru
                        {
                            newrec.isimage = true;
                            newrec.imagepath = file;
                            if (this.diagram.FileName != "" && File.Exists(this.diagram.FileName) && file.IndexOf(new FileInfo(this.diagram.FileName).DirectoryName) == 0)
                            {
                                int start = new FileInfo(this.diagram.FileName).DirectoryName.Length;
                                int finish = file.Length - start;
                                newrec.imagepath = "." + file.Substring(start, finish);
                            }
                            newrec.image = new Bitmap(newrec.imagepath);
                            if (ext != ".ico") newrec.image.MakeTransparent(Color.White);
                            newrec.height = newrec.image.Height;
                            newrec.width = newrec.image.Width;
                        }
                        else
                            if (this.diagram.FileName != "" && File.Exists(this.diagram.FileName) && file.IndexOf(new FileInfo(this.diagram.FileName).DirectoryName) == 0) // [PASTE] [FILE] - skratenie cesty k suboru
                        {
                            int start = new FileInfo(this.diagram.FileName).DirectoryName.Length;
                            int finish = file.Length - start;
                            newrec.link = "." + file.Substring(start, finish);
                        }
                        else
                                if (this.diagram.FileName != "" && Directory.Exists(this.diagram.FileName)) // [PASTE] [DIRECTORY] - skatenie cesty k adresaru
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
                            Point ptCursor = Cursor.Position;
                            ptCursor = PointToClient(ptCursor);
                            Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);

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

                        /*

                        // Paste image to file
                        try
                        {
                            if (this.NewFile || this.FileName == "" || File.Exists(this.FileName))
                            {
                                this.save();
                            }

                            if (!this.NewFile && this.FileName != "" && File.Exists(this.FileName))
                            {
                                if (!Directory.Exists(Path.GetDirectoryName(this.FileName) + Path.DirectorySeparatorChar + "images"))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(this.FileName) + Path.DirectorySeparatorChar + "images");
                                }
                                int icount = 0;
                                while (File.Exists(Path.GetDirectoryName(this.FileName) + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "image" + icount.ToString().PadLeft(4, '0') + ".png")) icount++;
                                Image image = (Image)data.GetData(DataFormats.Bitmap, true);
                                image.Save(Path.GetDirectoryName(this.FileName) + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "image" + icount.ToString().PadLeft(4, '0') + ".png", System.Drawing.Imaging.ImageFormat.Png);

                                Point ptCursor = Cursor.Position;
                                ptCursor = PointToClient(ptCursor);
                                TNode newrec = CreateNode(ptCursor.X, ptCursor.Y);
                                newrec.text = "image" + icount.ToString().PadLeft(4, '0') + ".png";

                                Font font = newrec.font;
                                SizeF s = this.MeasureStringWithMargin(newrec.text, newrec.font);
                                newrec.width = (int)s.Width;
                                newrec.height = (int)s.Height;
                                newrec.isimage = true;
                                newrec.imagepath = "." + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "image" + icount.ToString().PadLeft(4, '0') + ".png";
                                newrec.image = new Bitmap(newrec.imagepath);
                                newrec.height = newrec.image.Height;
                                newrec.width = newrec.image.Width;
                                this.unsave();
                                this.diagram.InvalidateDiagram();

                            }
                        }
                        catch (Exception e)
                        {
                            Program.log.write("paste image to file error: " + e.Message);
                        }
                        */
                    }

                }
                return true;
            }

            if (parseKey("CTRL+X", keyData))  // [KEY] [CTRL+X] Copy diagram
            {
                DataObject data = new DataObject();
                if (this.SelectedNodes.Count() > 0)  // kopirovanie textu objektu
                {
                    string copytext = "";
                    foreach (Node rec in this.SelectedNodes)
                    {
                        copytext = copytext + rec.text + "\n";
                    }

                    data.SetData(copytext);

                    data.SetData("DiagramXml", GetDiagramPart());//create and copy xml
                    this.DeleteSelectedNodes(this);
                    this.ClearSelection();
                    this.diagram.InvalidateDiagram();
                }

                Clipboard.SetDataObject(data);

                return true;
            }

			if (parseKey("CTRL+SHIFT+V", keyData))  // [KEY] [CTRL+SHIFT+V] vlozenie textu do poznamky
            {
				DataObject retrievedData = (DataObject)Clipboard.GetDataObject();

				if (retrievedData.GetDataPresent(DataFormats.Text))
				{
					string ClipText = retrievedData.GetData(DataFormats.Text) as string;

	                if (this.SelectedNodes.Count() == 0)
	                {
	                    Point ptCursor = Cursor.Position;
	                    ptCursor = PointToClient(ptCursor);
                        Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);

						newrec.note = ClipText;
                        this.diagram.unsave();
	                }
					else
					if (this.SelectedNodes.Count() == 1)
	                {
						if(this.SelectedNodes[0].note!="")
						{
							this.SelectedNodes[0].note += "\n";
						}

						this.SelectedNodes[0].note += ClipText;
					}
				}

                this.diagram.InvalidateDiagram();
                return true;
            }

            if (parseKey("CTRL+N", keyData))  // [KEY] [CTRL+N] New Diagram
            {
                main.OpenDiagram();
                return true;
            }

            if (parseKey("CTRL+SHIFT+N", keyData))  // [KEY] [CTRL+SHIFT+N] New Diagram
            {
                this.diagram.openDiagramView();
                return true;
            }

            if (parseKey("CTRL+S", keyData))  // [KEY] [CTRL+S] Uloženie okna
            {
                this.save();
                return true;
            }

            if (parseKey("CTRL+O", keyData))  // [KEY] [CTRL+O] Otvorenie diagramu
            {
                openToolStripMenuItem_Click(null, null);
                return true;
            }

            if (parseKey("CTRL+F", keyData))  // [KEY] [CTRL+F] Search form
            {
                this.showSearchPanel();
                return true;
            }

            if (parseKey("CTRL+G", keyData))  // [KEY] [CTRL+G] Evaluate expresion
            {
                if (this.SelectedNodes.Count()==1)
                {
                    /*string result = null;

                    try
                    {

                        Script macro = new Script();
                        result = macro.runScript(this.SelectedNodes[0].text);
                    }
                    catch(Exception ex)
                    {
                        Program.log.write("evaluation error: " + ex.Message);
                    }

					if (result != null) {
	                    Point ptCursor = Cursor.Position;
	                    ptCursor = PointToClient(ptCursor);
	                    Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);

	                    newrec.text = result;
	                    SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
	                    newrec.width = (int)s.Width;
	                    newrec.height = (int)s.Height;
	                    newrec.color = System.Drawing.ColorTranslator.FromHtml("#8AC5FF");

	                    this.diagram.InvalidateDiagram();
					}*/

                    string expression = this.SelectedNodes[0].text;
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
                        Point ptCursor = Cursor.Position;
                        ptCursor = PointToClient(ptCursor);
                        Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);

                        newrec.text = expressionResult;
                        SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                        newrec.width = (int)s.Width;
                        newrec.height = (int)s.Height;
                        newrec.color = System.Drawing.ColorTranslator.FromHtml("#8AC5FF");

                        this.diagram.InvalidateDiagram();
                    }


                    return true;
                }
                else
                if (this.SelectedNodes.Count() >1)  // SUM vypocet sumy viacerich nod ktore obsahuju cisla
                {
                    float sum = 0;
                    Match match = null;
                    foreach (Node rec in this.SelectedNodes)
                    {
                        match = Regex.Match(rec.text, @"([-]{0,1}\d+[\.,]{0,1}\d*)", RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            sum = sum + float.Parse(match.Groups[1].Value.Replace(",", "."), CultureInfo.InvariantCulture);
                        }
                    }

                    Point ptCursor = Cursor.Position;
                    ptCursor = PointToClient(ptCursor);
                    Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);

                    newrec.text = sum.ToString();
                    SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                    newrec.width = (int)s.Width;
                    newrec.height = (int)s.Height;
                    newrec.color = System.Drawing.ColorTranslator.FromHtml("#8AC5FF");

                    this.diagram.InvalidateDiagram();
                    return true;
                }

            }

            if (parseKey("CTRL+D", keyData))  // [KEY] [CTRL+D] Vozenie textu zo schranky
            {
                bool insertdate = true;
                string insertdatestring = "";

                if(this.SelectedNodes.Count()>0)
                {
                    DateTime d1;
                    DateTime d2;

                    bool aretimes = true;
                    foreach (Node rec in this.SelectedNodes) // Loop through List with foreach
                    {
                        if (!Regex.Match(rec.text, @"^[0-9]{2}:[0-9]{2}:[0-9]{2}$", RegexOptions.IgnoreCase).Success)
                        {
                            aretimes = false;
                            break;
                        }
                    }

                    if(aretimes) // ak sú všetky označené elementy časové značky tak ich ščíta
                    {
                        try
                        {
                            TimeSpan timesum = TimeSpan.Parse("00:00:00");
                            foreach (Node rec in this.SelectedNodes)
                            {
                                timesum = timesum.Add(TimeSpan.Parse(rec.text));
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
                    // ak sú označené dva dátumi vyráta medzi nimi rozdiel
                    if (
                        this.SelectedNodes.Count() == 2 &&
                        DateTime.TryParse(this.SelectedNodes[0].text, out d1) &&
                        DateTime.TryParse(this.SelectedNodes[1].text, out d2)
                    )
                    {
                        try
                        {
                            insertdatestring = ((d1<d2)?d2 - d1:d1-d2).ToString();
                            insertdate = false;
                        }
                        catch (Exception ex)
                        {
                            Program.log.write("time diff error: " + ex.Message);
                        }
                    }
                }

                if (insertdate) // vloženie časovej značky na danú pozíciu
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

                Point ptCursor = Cursor.Position;
                ptCursor = PointToClient(ptCursor);
                Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);

                newrec.text = insertdatestring;
                SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                newrec.width = (int)s.Width;
                newrec.height = (int)s.Height;
                newrec.color = System.Drawing.ColorTranslator.FromHtml("#8AC5FF");

                this.diagram.InvalidateDiagram();
                return true;
            }

            if (parseKey("CTRL+P", keyData)) // [KEY] [CTRL+P] Promote node
            {
                if (this.SelectedNodes.Count() == 1)
                {
                    Point ptCursor = Cursor.Position;
                    ptCursor = PointToClient(ptCursor);
                    Node selectedNode = this.SelectedNodes[0];
                    Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);
                    newrec.copyNode(selectedNode, true, true);

                    string expression = newrec.text;
                    string[] days = { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
                    int dayPosition = Array.IndexOf(days, newrec.text);

                    var matchesFloat = Regex.Matches(expression, @"(\d+(?:\.\d+)?)");
                    var matchesDate = Regex.Matches(expression, @"^(\d{4}-\d{2}-\d{2})$");

                    if (dayPosition != -1) { //get next day
                        dayPosition += 1;
                        if (dayPosition == 7) {
                            dayPosition = 0;
                        }

                        newrec.text = days[dayPosition];
                    }
                    else if (matchesDate.Count > 0) // add day to date
                    {
                        DateTime theDate;
                        DateTime.TryParseExact(expression, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out theDate);
                        theDate = theDate.AddDays(1);
                        string dateValue = matchesFloat[0].Groups[1].Value;
                        string newnDateValue = String.Format("{0:yyyy-MM-dd}", theDate);
                        newrec.text = newnDateValue;
                    }
                    else if (matchesFloat.Count > 0) //add to number
                    {
                        string number = matchesFloat[0].Groups[1].Value;
                        string newnumber = (float.Parse(number) + 1).ToString();
                        newrec.text = expression.Replace(number, newnumber);
                    }


                    SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                    newrec.width = (int)s.Width;
                    newrec.height = (int)s.Height;

                    this.diagram.InvalidateDiagram();
                    return true;

                }
                return true;
            }

            if (parseKey("CTRL+R", keyData)) // [KEY] [CTRL+R] Random generator
            {
                Point ptCursor = Cursor.Position;
                ptCursor = PointToClient(ptCursor);
                this.CreateNode(ptCursor.X, ptCursor.Y, true, Encrypt.GetRandomString());

                this.diagram.unsave();
                this.diagram.InvalidateDiagram();
            }

            if (parseKey("F3", keyData)) // [KEY] [F3] Hide background
            {

                bool changed = false;

                if (this.SelectedNodes.Count > 0)
                {
                    //first all hide then show
                    bool allHidden = true;
                    foreach (Node rec in this.SelectedNodes)
                    {
                        if (!rec.transparent)
                        {
                            allHidden = false;
                            break;
                        }
                    }

                    foreach (Node rec in this.SelectedNodes)
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
                if(changed)this.diagram.unsave();
                this.diagram.InvalidateDiagram();

                return true;
            }

            if (parseKey("SHIFT+F3", keyData)) // [KEY] [SHIFT+F3] reverse search
            {
                this.SearchPrev();
            }

            if (parseKey("HOME", keyData)) // KEY [HOME] Vycentrovanie
            {
                this.GoToHome();
                return true;
            }

            if (parseKey("SHIFT+HOME", keyData))  // [KEY] [SHIFT+HOME] Move start point
            {
                this.setCurentPositionAsHomePosition();
            }

            if (parseKey("END", keyData)) // KEY [END] Vycentrovanie
            {
                this.GoToEnd();
                return true;
            }

            if (parseKey("SHIFT+END", keyData))  // [KEY] [SHIFT+END] Move start point
            {
                this.setCurentPositionAsEndPosition();
            }

            /*
             [DOCUMENTATION]
             Shortcut F5
            -otvorenie adresara vybranej nody
            -prejdu sa vybrane nody a ak je to adresar alebo subor otvori sa adresar
            -ak nie su vybrane ziadne nody otvori sa adresar diagrammu
            */
            if (parseKey("F5", keyData)) // KEY [F5] Open link directory or diagram directory
            {
                openLinkDirectory();
                return true;
            }

            if (parseKey("F12", keyData)) // [KEY] [F12] show Debug console
            {
                this.showConsole();
            }

            if (parseKey("CTRL+PAGEUP", keyData)) // KEY CTRL+PAGEUP Posun prvku do popredia
            {
                if (this.SelectedNodes.Count() > 0 )
                {
                    foreach (Node rec in this.SelectedNodes)
                    {
                        var item = rec;
                        int pos = diagram.GetIndexByID(rec.id);
                        this.diagram.Nodes.RemoveAt(pos);
                        this.diagram.Nodes.Insert(this.diagram.Nodes.Count(), item);
                    }
                    this.diagram.InvalidateDiagram();
                }

                return true;
            }

            if (parseKey("CTRL+PAGEDOWN", keyData)) // [KEY] [CTRL+PAGEDOWN] Posun prvku do pozadia
            {
                if (this.SelectedNodes.Count() > 0)
                {
                    foreach (Node rec in this.SelectedNodes)
                    {
                        var item = rec;
                        int pos = diagram.GetIndexByID(rec.id);
                        this.diagram.Nodes.RemoveAt(pos);
                        this.diagram.Nodes.Insert(0, item);
                    }
                    this.diagram.InvalidateDiagram();
                }
                return true;
            }

            if (parseKey("PAGEUP", keyData)) // [KEY] [PAGEUP] Posun obrazovky
            {
                this.shift.y = this.shift.y + this.ClientSize.Height;
                this.diagram.InvalidateDiagram();
                return true;
            }

            if (parseKey("PAGEDOWN", keyData)) // [KEY] [PAGEDOWN] Posun obrazovky
            {
                this.shift.y = this.shift.y - this.ClientSize.Height;
                this.diagram.InvalidateDiagram();
                return true;
            }

            if (parseKey("F2", keyData)) // [KEY] [F2] Editovanie
            {
                if (this.SelectedNodes.Count() == 1)
                {
                    this.diagram.EditNode(this.SelectedNodes[0]);
                }
                return true;
            }

            if (this.nodeNameEdit.Focused)
            {
                return false;
            }

            if (parseKey("ENTER", keyData)) // [KEY] [ENTER] Editovanie nody
            {
                if (this.SelectedNodes.Count() == 1)
                {
                    if (this.SelectedNodes[0].haslayer)
                    {
                        this.LayerIn(this.SelectedNodes[0]);
                    }
                    else
                    {
                        this.diagram.EditNode(this.SelectedNodes[0]);
                    }
                }
                return true;
            }

            if (parseKey("ESCAPE", keyData)) // [KEY] [ESC] Minimalizovanie okna //KEY Esc
            {
                if (!this.diagram.SavedFile && this.diagram.FileName != "")  //treba overit ci existuje a dat dialg na ulozenie ako
                {
                    this.diagram.SaveXMLFile(this.diagram.FileName);
                    this.diagram.NewFile = false;
                    this.diagram.SavedFile = true;
                }
                this.WindowState = FormWindowState.Minimized;
                return true;
            }

            if (parseKey("BACK", keyData)) // [KEY] [BACK] Editovanie nody
            {
                this.LayerOut();
                return true;
            }

            if (parseKey("DELETE", keyData)) // [KEY] [DELETE] zmazanie nody
            {
                this.DeleteSelectedNodes(this);
                return true;
            }

            if (parseKey("LEFT", keyData) || parseKey("SHIFT+LEFT", keyData))  // [KEY] [left] [SHIFT+LEFT] [ARROW] Move node
            {
                if (this.SelectedNodes.Count() > 0)
                {
                    int speed = (keyData == Keys.Left)?this.diagram.options.keyArrowSlowSpeed:this.diagram.options.keyArrowFastSpeed;
                    foreach (Node rec in this.SelectedNodes)
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

                return true;
            }

            if (parseKey("RIGHT", keyData) || parseKey("SHIFT+RIGHT", keyData))  // [KEY] [right] [SHIFT+RIGHT] [ARROW] Move node
            {

                if (this.SelectedNodes.Count() > 0)
                {
                    int speed = (keyData == Keys.Right) ? this.diagram.options.keyArrowSlowSpeed : this.diagram.options.keyArrowFastSpeed;
                    foreach (Node rec in this.SelectedNodes)
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

                return true;
            }

            if (parseKey("UP", keyData) || parseKey("SHIFT+UP", keyData))  // [KEY] [up] [SHIFT+UP] [ARROW] Move node
            {
                if (this.SelectedNodes.Count() > 0)
                {
                    int speed = (keyData == Keys.Up) ? this.diagram.options.keyArrowSlowSpeed : this.diagram.options.keyArrowFastSpeed;
                    foreach (Node rec in this.SelectedNodes)
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

                return true;
            }

            if (parseKey("DOWN", keyData) || parseKey("SHIFT+DOWN", keyData))  // [KEY] [down] [SHIFT+DOWN] [ARROW] Move node
            {
                if (this.SelectedNodes.Count() > 0)
                {
                    int speed = (keyData == Keys.Down) ? this.diagram.options.keyArrowSlowSpeed : this.diagram.options.keyArrowFastSpeed;
                    foreach (Node rec in this.SelectedNodes)
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

                return true;
            }

            if (parseKey("TAB", keyData)) // [KEY] [TAB] zarovnanie vybranych prvkov dolava
            {
                if (this.SelectedNodes.Count() > 1)
                {
                    this.diagram.AlignLeft(this.SelectedNodes);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
                else
                {
                    this.addNodeAfterNode();
                }
            }

            if (parseKey("SHIFT+TAB", keyData))  // [KEY] [SHIFT+TAB] Zarovnanie vybranych prvkov doprava //KEY shift+tab
            {
                if (this.SelectedNodes.Count() > 0)
                {
                    this.diagram.AlignRight(this.SelectedNodes);
                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // EVENT Key down
        public void DiagramApp_KeyDown(object sender, KeyEventArgs e)                                  // [KEYBOARD] [DOWN] [EVENT]
        {
            if (this.nodeNameEdit.Focused || this.searching)
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

            if (this.nodeNameEdit.Focused)
            {
                return;
            }

            if (e.KeyCode == Keys.Space && !this.zooming) // KEY SPACE
            {
                this.selecting = false;
                MoveTimer.Enabled = false;

                this.zooming = true;
                int a = (int)((this.shift.x - (this.ClientSize.Width / 2 * this.scale)));
                int b = (int)((this.shift.y - (this.ClientSize.Height / 2 * this.scale)));
                this.scale = this.zoomingScale;
                this.shift.x = (int)((a + (this.ClientSize.Width / 2 * this.scale)));
                this.shift.y = (int)((b + (this.ClientSize.Height / 2 * this.scale)));

                this.diagram.InvalidateDiagram();
            }

        }

        // EVENT Key up
        public void DiagramApp_KeyUp(object sender, KeyEventArgs e)
        {
            this.keyshift = false;
            this.keyctrl = false;
            this.keyalt = false;

            if (this.nodeNameEdit.Focused || this.searching)
            {
                return;
            }

            if (this.zooming)
            {
                this.zooming = false;

                MoveTimer.Enabled = false;  // zrusenie prebiehajucich operácii
                this.move = false;
                this.addingNode = false;
                this.drag = false;
                this.selecting = false;

                this.shift.x = (int)((this.shift.x + (this.ClientSize.Width / 2) - (this.ClientSize.Width / 2 * this.scale)));
                this.shift.y = (int)((this.shift.y + (this.ClientSize.Height / 2) - (this.ClientSize.Height / 2 * this.scale)));
                this.scale = this.zoomingDefaultScale;
                this.diagram.InvalidateDiagram();
            }
        }                                 // [KEYBOARD] [UP] [EVENT]

        // EVENT Keypress
        public void DiagramApp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.nodeNameEdit.Focused || this.searching)
            {
                return;
            }

            this.key = e.KeyChar;

            if (!this.keyctrl && !this.keyalt)
            {

                if (this.key == '+') // KEY PLUS In to layer
                {
                    if (this.SelectedNodes.Count() == 1 && this.SelectedNodes[0].haslayer)
                    {
                        this.LayerIn(this.SelectedNodes[0]);
                    }
                }
                else
                if (this.key == '-') // KEY MINUS Out to layer
                {
                    this.LayerOut();
                }
                else
                if (this.key != ' ' && this.key != '\t' && this.key != '\r' && this.key != '\n' && this.key != '`' && this.key != (char)27) // KEY OTHER Pisanie textu - Vytvorenie novej nody
                {
                    this.showNodeNamePanel();
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
                    Point ptCursor = Cursor.Position;
                    ptCursor = PointToClient(ptCursor);
                    Node newrec = this.CreateNode(ptCursor.X, ptCursor.Y);
                    newrec.text = Path.GetFileName(file);

                    SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                    newrec.width = (int)s.Width;
                    newrec.height = (int)s.Height;

    				newrec.link = file;
    				if (Directory.Exists(file)) // directory
                    {
    					newrec.link = Os.makeRelative(file, this.diagram.FileName);
                        newrec.color = Media.getColor(diagram.options.colorDirectory);
                    }
    				else
    				if (File.Exists(file))
                    {
                        newrec.color = Media.getColor(diagram.options.colorFile);

                        if (this.diagram.FileName != "" && File.Exists(this.diagram.FileName)) // DROP FILE - skratenie cesty k suboru
                        {
                            newrec.link = Os.makeRelative(file, this.diagram.FileName);
                        }

                        string ext = "";
                        ext = Path.GetExtension(file).ToLower();

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
                                if (File.Exists(newrec.link) && Path.GetExtension(newrec.link).ToLower() == ".exe")// extract icon
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

        // EVENT Resize                                                                                // [RESIZE] [EVENT]
        public void DiagramApp_Resize(object sender, EventArgs e)
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

        // EVENT MOVE TIMER Posunutie okna ked sa pride k okraju
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
        public void LayerIn(Node rec)
        {
            if (rec.id != this.layer)
            {
                if (this.layer == 0)
                {
                    this.firstLayereShift.x = this.shift.x;
                    this.firstLayereShift.y = this.shift.y;
                }
                else
                {
                    this.LayerNode.haslayer = true;
                    this.LayerNode.layershiftx = this.shift.x;
                    this.LayerNode.layershifty = this.shift.y;
                }

                if (rec.haslayer)
                {
                    this.shift.x = rec.layershiftx;
                    this.shift.y = rec.layershifty;
                }
                else
                {
                    rec.haslayer = true;
                    rec.layershiftx = this.shift.x;
                    rec.layershifty = this.shift.y;
                }

                Layers.Add(this.layer);
                this.layer = rec.id;
                this.LayerNode = rec;

                this.diagram.SetTitle();

                this.diagram.InvalidateDiagram();
            }
            else
            {
                this.LayerOut(); // ak je vybrany vstupný bod tak sa nevnoruje ale vychadza sa von
            }
        }

        // LAYER OUT
        public void LayerOut()
        {
            if (this.layer != 0)
            {
                if (this.LayerNode != null)
                {
                    bool hasnode = false;
                    for (int i = diagram.Nodes.Count() - 1; i >= 0; i--)
                    {
                        if (diagram.Nodes[i].layer == this.LayerNode.id)
                        {
                            hasnode = true;
                            break;
                        }
                    }

                    this.LayerNode.haslayer = hasnode;
                    this.LayerNode.layershiftx = this.shift.x;
                    this.LayerNode.layershifty = this.shift.y;
                }

                if (Layers.Count() > 0)
                {
                    this.SelectOnlyOneNode(this.diagram.GetNodeByID(this.layer));
                    this.layer = Layers[Layers.Count() - 1];
                    this.LayerNode = this.diagram.GetNodeByID(this.layer);
                    Layers.RemoveAt(Layers.Count() - 1);

                    if (LayerNode != null)
                    {
                        this.shift.x = LayerNode.layershiftx;
                        this.shift.y = LayerNode.layershifty;
                    }

                    if (this.layer == 0)
                    {
                        this.shift.x = this.firstLayereShift.x;
                        this.shift.y = this.firstLayereShift.y;
                    }
                }
                else
                {
                    this.layer = 0;
                    this.shift.x = this.firstLayereShift.x;
                    this.shift.y = this.firstLayereShift.y;
                    this.ClearSelection();
                    this.LayerNode = null;
                }

                if (this.LayerNode != null)
                {
                    this.shift.x = this.LayerNode.layershiftx;
                    this.shift.y = this.LayerNode.layershifty;
                }

                this.diagram.SetTitle();

                this.diagram.InvalidateDiagram();
            }
        }

        // LAYER MOVE posunie rekurzivne layer a jeho nody
        public void MoveLayer(Node rec, int vectorx, int vectory)
        {
            if (rec != null)
            {
                for (int i = diagram.Nodes.Count() - 1; i >= 0; i--) // Loop through List with foreach
                {
                    if (diagram.Nodes[i].layer == rec.id)
                    {
                        diagram.Nodes[i].position.x += vectorx;
                        diagram.Nodes[i].position.y += vectory;

                        if (diagram.Nodes[i].haslayer)
                        {
                            MoveLayer(diagram.Nodes[i], vectorx, vectory);
                        }
                    }
                }
            }
        }

        // LAYER HISTORY Buld laier history from
        public void BuildLayerHistory(int id)
        {
            Layers.Clear();
            if (this.layer != 0)
            {
                this.LayerNode = this.diagram.GetNodeByID(this.layer);
                int temp = this.layer;
                bool found = false;
                while (temp != 0)
                {
                    found = false;
                    foreach (Node rec in diagram.Nodes)
                    {
                        if (rec.id == temp)
                        {
                            temp = rec.layer;
                            Layers.Add(temp);

                            found = true;
                            break;
                        }
                    }
                    if (!found) break;
                }
                Layers.Reverse(0, Layers.Count());
            }
        }

        public bool isNodeInLayerHistory(Node rec) {
            return ((this.layer != rec.id) && (Layers.IndexOf(rec.id) > -1));
        }

        /*************************************************************************************************************************/

        // EDITPANEL INICIALIZE                                                                            // EDITPANEL
        private void inicializeNodeNamePanel()
        {
            if (nodeNameEdit == null)
            {
                nodeNamePanel = new Panel();
                nodeNamePanel.Name = "nodeNamePanel";
                this.Controls.Add(nodeNamePanel);
                nodeNameEdit = new TextBox();
                nodeNameEdit.Name = "nodeNameEdit";
                nodeNameEdit.Font = this.diagram.FontDefault;
                nodeNameEdit.BorderStyle = BorderStyle.None;
                nodeNameEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nodeNameEdit_KeyDown);
                nodeNameEdit.TextChanged += new EventHandler(nodeNameEdit_TextChanged);
                nodeNameEdit.AcceptsReturn = true;
                nodeNameEdit.AcceptsTab = true;
                nodeNameEdit.Multiline = true;
                nodeNameEdit.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFB8");
                //nodeNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

                nodeNamePanel.Hide();
                nodeNameEdit.Left = 12;
                nodeNameEdit.Top = 8;
                nodeNamePanel.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFB8");
                SizeF s = this.diagram.MeasureStringWithMargin("TEST", nodeNameEdit.Font);
                nodeNamePanel.Height = (int)s.Height;

                nodeNamePanel.Controls.Add(nodeNameEdit);
                nodeNameEdit.Show();
            }
        }

        // EDITPANEL SHOW
        private void showNodeNamePanel()
        {
            if (!nodeNameEdit.Visible)
            {
                Point ptCursor = Cursor.Position;
                ptCursor = PointToClient(ptCursor);
                nodeNamePanel.Left = ptCursor.X;
                nodeNamePanel.Top = ptCursor.Y;
                nodeNamePanel.Width = 100;
                nodeNameEdit.Font = this.diagram.FontDefault;
                nodeNameEdit.Text = "" + (this.key).ToString(); // add first character
                nodeNameEdit.SelectionStart = nodeNameEdit.Text.Length; //move cursor to begining
                editingNodeName = true;
                SizeF s = this.diagram.MeasureStringWithMargin("TEST", nodeNameEdit.Font);
                nodeNamePanel.Height = (int)s.Height;
                nodeNamePanel.Show();
                nodeNameEdit.Focus();
            }
        }

        // EDITPANEL CLOSE
        private void closeNodeNamePanel()
        {
            editingNodeName = false;
            nodeNamePanel.Hide();
        }

        // EDITPANEL SAVE
        private void saveNodeNamePanel()
        {
            Node newrec = this.CreateNode(nodeNamePanel.Left, nodeNamePanel.Top);
            newrec.text = nodeNameEdit.Text;
            SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
            newrec.width = (int)s.Width;
            newrec.height = (int)s.Height;
            nodeNamePanel.Hide();
            editingNodeName = false;

            if (this.prevSelectedNode != null)
            {
                this.diagram.Connect(this.prevSelectedNode, newrec);
                this.prevSelectedNode = null;
            }

            this.diagram.unsave();
            this.diagram.InvalidateDiagram();
        }

        // EDITPANEL RESIZE zmena velkosti panelu ak sa donho píše
        void nodeNameEdit_TextChanged(object sender, EventArgs e)
        {
            System.Drawing.SizeF mySize = new System.Drawing.SizeF();

            // Use the textbox font
            System.Drawing.Font myFont = nodeNameEdit.Font;

            using (Graphics g = this.CreateGraphics())
            {
                // Get the size given the string and the font
                mySize = g.MeasureString(nodeNameEdit.Text, myFont);
            }

            // Resize the textbox
            this.nodeNameEdit.Width = (int)Math.Round(mySize.Width, 0) + 20;
            this.nodeNamePanel.Width = this.nodeNameEdit.Width;
        }

        // EDITPANEL EDIT zachytenie kláves v panely
        private void nodeNameEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) // zrusenie editácie v panely
            {
                this.saveNodeNamePanel();
				this.Focus();
            }

            if (e.KeyCode == Keys.Enter) // zvretie panelu a vytvorenie novej editacie
            {
                this.saveNodeNamePanel();
				this.Focus();
            }

            if (e.KeyCode == Keys.Tab) // zvretie panelu a vytvorenie novej editacie
            {
                this.saveNodeNamePanel();
                this.Focus();
                this.addNodeAfterNode();
            }

            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Tab))
            {
                e.Handled = e.SuppressKeyPress = true;
            }

        }

        /*************************************************************************************************************************/

        // SEARCHPANEL
        public int lastFound = -1;
        public string searchFor = "";
        public SearchPanel searhPanel = null;
        Position currentPosition = new Position();

        public List<int> NodesSearchResult = new List<int>();

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

            for (int i = 0; i < diagram.Nodes.Count(); i++)
            {
                if (this.diagram.Nodes[i].note.ToUpper().IndexOf(searchFor.ToUpper()) != -1 || this.diagram.Nodes[i].text.ToUpper().IndexOf(searchFor.ToUpper()) != -1)
                {
                    foundNodes.Add(this.diagram.Nodes[i]);
                }
            }

            this.searhPanel.highlight(foundNodes.Count() == 0);


            foundNodes.Sort((first, second) =>
            {
                double d1 = first.position.convertTostandard().distance(this.currentPosition);
                double d2 = second.position.convertTostandard().distance(this.currentPosition);
                Program.log.write("[id="+first.id.ToString()+","+second.id.ToString()+", dist= " + d1.ToString() + "," + d2.ToString() + "]");
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

            NodesSearchResult.Clear();
            for (int i = 0; i < foundNodes.Count(); i++)
            {
                NodesSearchResult.Add(foundNodes[i].id);
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

            if (NodesSearchResult.Count() == 0)
                return;

            for (int i = lastFound+1; i < NodesSearchResult.Count(); i++)
            {
                node = this.diagram.GetNodeByID(NodesSearchResult[i]);

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
                    node = this.diagram.GetNodeByID(NodesSearchResult[i]);

                    if (node != null)
                    {
                        lastFound = i;
                        break;
                    }
                }
            }

            if (node != null)
            {
                this.GoToNode(node);
                this.SelectOnlyOneNode(node);
                this.diagram.InvalidateDiagram();
            }

        }

        // SEARCH PREV
        public void SearchPrev()
        {
            Node node = null;

            if (NodesSearchResult.Count() == 0)
                return;

            if (lastFound == -1)
            {
                lastFound = 0;
            }

            for (int i = lastFound-1; i >= 0; i--)
            {
                node = this.diagram.GetNodeByID(NodesSearchResult[i]);

                if (node != null)
                {
                    lastFound = i;
                    break;
                }
            }

            if (node == null)
            {
                for (int i = NodesSearchResult.Count()-1; i >= lastFound; i--)
                {
                    node = this.diagram.GetNodeByID(NodesSearchResult[i]);

                    if (node != null)
                    {
                        lastFound = i;
                        break;
                    }
                }
            }

            if (node != null)
            {
                this.GoToNode(node);
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
                this.searhPanel.Location = new System.Drawing.Point(10, 10);
                this.searhPanel.Name = "panel2";
                this.searhPanel.Size = new System.Drawing.Size(106, 25);
                this.searhPanel.TabIndex = 0;
                this.searhPanel.SearchpanelStateChanged += this.SearchPanelChanged;
                this.Controls.Add(this.searhPanel);
            }

            currentPosition.x = -this.shift.x;
            currentPosition.y = this.shift.y;

            searhPanel.ShowPanel();
            this.searching = true;
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
        }

        /*************************************************************************************************************************/

        // CLIPBOARD PASTE vloží časť zo schranky do otvoreneho diagramu                                   // CLIPBOARD
        public void AddDiagramPart(string DiagramXml)
        {
            //OBSOLATE
            //string FontDefaultString = TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(this.diagram.FontDefault);

            List<Node> NewNodes = new List<Node>();
            List<Line> NewLines = new List<Line>();

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

            string xml = DiagramXml;

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xml), xws))
                {

                    XElement root = XElement.Load(xr);
                    foreach (XElement diagram in root.Elements())
                    {
                        if (diagram.HasElements)
                        {

                            if (diagram.Name.ToString() == "rectangles")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {

                                    if (block.Name.ToString() == "rectangle")
                                    {
                                        Node R = new Node();
                                        R.font = this.diagram.FontDefault;

                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "id")
                                                {
                                                    R.id = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "text")
                                                {
                                                    R.text = el.Value;
                                                }


                                                if (el.Name.ToString() == "note")
                                                {
                                                    R.note = el.Value;
                                                }

                                                if (el.Name.ToString() == "x")
                                                {
                                                    R.position.x = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "y")
                                                {
                                                    R.position.y = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "color")
                                                {
                                                    R.color = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
                                                }


                                                if (el.Name.ToString() == "timecreate")
                                                {
                                                    R.timecreate = el.Value;
                                                }


                                                if (el.Name.ToString() == "timemodify")
                                                {
                                                    R.timemodify = el.Value;
                                                }


                                                if (el.Name.ToString() == "font")
                                                {
                                                    R.font = Fonts.XmlToFont(el);
                                                }


                                                if (el.Name.ToString() == "fontcolor")
                                                {
                                                    R.fontcolor = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "link")
                                                {
                                                    R.link = el.Value;
                                                }

                                                if (el.Name.ToString() == "shortcut")
                                                {
                                                    R.shortcut = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "transparent")
                                                {
                                                    R.transparent = bool.Parse(el.Value);
                                                }


                                                if (el.Name.ToString() == "timecreate")
                                                {
                                                    R.timecreate = el.Value;
                                                }


                                                if (el.Name.ToString() == "timemodify")
                                                {
                                                    R.timemodify = el.Value;
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write(main.translations.dataHasWrongStructure + ": error: " + ex.Message);
                                            }
                                        }

                                        NewNodes.Add(R);
                                    }
                                }
                            }

                            if (diagram.Name.ToString() == "lines")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {
                                    if (block.Name.ToString() == "line")
                                    {
                                        Line L = new Line();
                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "start")
                                                {
                                                    L.start = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "end")
                                                {
                                                    L.end = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "arrow")
                                                {
                                                    L.arrow = el.Value == "1" ? true : false;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write(main.translations.dataHasWrongStructure + ": error: " + ex.Message);
                                            }
                                        }
                                        NewLines.Add(L);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.log.write(main.translations.dataHasWrongStructure + ": error: " + ex.Message);
            }


            List<Node[]> maps = new List<Node[]>();

            Point ptCursor = Cursor.Position;
            ptCursor = PointToClient(ptCursor);
            this.ClearSelection();
            foreach (Node rec in NewNodes)
            {

                Node newrec = this.CreateNode(ptCursor.X + rec.position.x, ptCursor.Y + rec.position.y, false);
                newrec.text = rec.text;
                newrec.font = rec.font;
                SizeF s = this.diagram.MeasureStringWithMargin(newrec.text, newrec.font);
                newrec.width = (int)s.Width;
                newrec.height = (int)s.Height;
                newrec.note = rec.note;
                newrec.color = rec.color;
                newrec.fontcolor = rec.fontcolor;
                newrec.link = rec.link;
                newrec.shortcut = rec.shortcut;
                newrec.transparent = rec.transparent;
                newrec.transparent = rec.transparent;
                newrec.timecreate = rec.timecreate;
                newrec.timemodify = rec.timemodify;

                maps.Add(new Node[2] { rec, newrec });

                this.SelectNode(newrec);
            }

            foreach(Line line in NewLines)
            {
                foreach (Node[] mapbegin in maps)
                {
                    if (line.start == mapbegin[0].id)
                    {
                        foreach (Node[] mapend in maps)
                        {
                            if (line.end == mapend[0].id)
                            {
                                this.diagram.Connect(mapbegin[1], mapend[1], line.arrow);
                            }
                        }
                    }
                }
            }

            this.diagram.unsave();
            this.diagram.InvalidateDiagram();
        }

        // CLIPBOARD COPY vloží vybratu časť do schranky
        public string GetDiagramPart()
        {
            string copyxml = "";

            if (this.SelectedNodes.Count() > 0)
            {
                XElement root = new XElement("diagram");
                XElement rectangles = new XElement("rectangles");
                XElement lines = new XElement("lines");

                int minx = this.SelectedNodes[0].position.x;
                int miny = this.SelectedNodes[0].position.y;
                int minid = this.SelectedNodes[0].id;

                foreach (Node rec in this.SelectedNodes)
                {
                    if (rec.position.x < minx) minx = rec.position.x;
                    if (rec.position.y < miny) miny = rec.position.y;
                    if (rec.id < minid) minid = rec.id;
                }

                foreach (Node rec in this.SelectedNodes)
                {
                    XElement rectangle = new XElement("rectangle");
                    rectangle.Add(new XElement("id", rec.id - minid));
                    rectangle.Add(new XElement("x", rec.position.x - minx));
                    rectangle.Add(new XElement("y", rec.position.y - miny));
                    rectangle.Add(new XElement("text", rec.text));
                    rectangle.Add(new XElement("note", rec.note));
                    rectangle.Add(new XElement("color", System.Drawing.ColorTranslator.ToHtml(rec.color)));
                    rectangle.Add(Fonts.FontToXml(rec.font));
                    rectangle.Add(new XElement("fontcolor", System.Drawing.ColorTranslator.ToHtml(rec.fontcolor)));
                    if (rec.link != "") rectangle.Add(new XElement("link", rec.link));
                    if (rec.shortcut != 0) rectangle.Add(new XElement("shortcut", rec.shortcut));
                    rectangle.Add(new XElement("transparent", rec.transparent));
                    rectangle.Add(new XElement("timecreate", rec.timecreate));
                    rectangle.Add(new XElement("timemodify", rec.timemodify));

                    rectangles.Add(rectangle);
                }

                foreach (Line li in this.diagram.Lines)
                {
                    foreach (Node recstart in this.SelectedNodes)
                    {
                        if (li.start == recstart.id)
                        {
                            foreach (Node recend in this.SelectedNodes)
                            {
                                if (li.end == recend.id)
                                {
                                    XElement line = new XElement("line");
                                    line.Add(new XElement("start", li.start - minid));
                                    line.Add(new XElement("end", li.end - minid));
                                    line.Add(new XElement("arrow", (li.arrow) ? "1" : "0"));
                                    lines.Add(line);
                                }

                            }
                        }
                    }
                }

                root.Add(rectangles);
                root.Add(lines);
                copyxml = root.ToString();
            }

            return copyxml;
        }

        // CLIPBOARD Copy link to clipboard
        public void copyLinkToClipboard(Node node)
        {
            Clipboard.SetText(node.link);
        }

        /*************************************************************************************************************************/

        // SCROLLBAR MOVE LEFT-RIGHT                                                                       // SCROLLBAR
        /* posuva obrazovku v percentach 0-1*/
        public void moveScreenHorizontal(float per)
        {
            int minx = int.MaxValue;
            int maxx = int.MinValue;
            foreach (Node rec in diagram.Nodes)
            {
                if (rec.layer == this.layer || rec.id == this.layer)
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
        /* zistenie pozície v obrazovke v percentach 0-1*/
        public float getPositionHorizontal()
        {
            float per = 0;
            int minx = int.MaxValue;
            int maxx = int.MinValue;
            foreach (Node rec in diagram.Nodes)
            {
                if (rec.layer == this.layer || rec.id == this.layer)
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
            foreach (Node rec in diagram.Nodes)
            {
                if (rec.layer == this.layer || rec.id == this.layer)
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
        /* zistenie pozície v obrazovke v percentach 0-1*/
        public float getPositionVertical()
        {
            float per = 0;
            int miny = int.MaxValue;
            int maxy = int.MinValue;
            foreach (Node rec in diagram.Nodes)
            {
                if (rec.layer == this.layer || rec.id == this.layer)
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

        // [FILE] [CLOSE] - Vycisti  nastavenie do východzieho tavu a prekresli obrazovku
        public void CloseFile()
        {

            this.Text = "Diagram";
            this.shift.x = 0;
            this.shift.y = 0;
            this.ClearSelection();

            this.layer = 0;
            this.LayerNode = null;
            this.Layers.Clear();

            this.diagram.CloseDiagram();

        }

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

        // FILE Open diagram directory
        public void openDiagramDirectory()
        {
            if (!this.diagram.NewFile && File.Exists(this.diagram.FileName))
            {
                try
                {
                    System.Diagnostics.Process.Start(Path.GetDirectoryName(this.diagram.FileName));
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
            if (diagram.Nodes.Count > 0)
            {

                int minx = diagram.Nodes[0].position.x;
                int maxx = diagram.Nodes[0].position.x + diagram.Nodes[0].width;
                int miny = diagram.Nodes[0].position.y;
                int maxy = diagram.Nodes[0].position.y + diagram.Nodes[0].height;

                foreach (Node rec in diagram.Nodes) // Loop through List with foreach
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
                this.PaintDiagram(g, true, -this.shift.x - minx, -this.shift.y - miny);
                g.Dispose();
                bmp.Save(exportFile.FileName, System.Drawing.Imaging.ImageFormat.Png);
                bmp.Dispose();
            }
        }

        // EXPORT Export diagram to txt
        public void exportDiagramToTxt(string filePath)
        {
            if (diagram.Nodes.Count > 0)
            {
                string outtext = "";

                foreach (Node rec in diagram.Nodes)
                {
                    outtext += rec.text + "\n" + (rec.link != "" ? rec.link + "\n" : "") + "\n" + rec.note + "\n---\n";
                }
                System.IO.File.WriteAllText(filePath, outtext);
            }
        }

        /*************************************************************************************************************************/

        // PAINT paint                                                                                  // PAINT
        void PaintDiagram(Graphics gfx, bool export = false, int cx = 0, int cy = 0)
        {
            // cx cy -> correction for export image

            bool isvisible = false; // kresli len prvok ktory je na obrazovke ostatne vynechaj

            float s = this.scale;

            gfx.SmoothingMode = SmoothingMode.AntiAlias;

            System.Drawing.Pen myPen = new System.Drawing.Pen(Color.FromArgb(201, 201, 201), 1);
            System.Drawing.Pen myPen1 = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
            System.Drawing.Pen myPen2 = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

            // DRAW grid
            if (this.diagram.options.grid && !export)
            {
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

            // DRAW lines
            foreach (Line lin in diagram.Lines) // Loop through List with foreach
            {

                Node r1 = lin.startNode;
                Node r2 = lin.endNode;
                if (r1 != null && r2 != null && (r1.layer == this.layer || r1.id == this.layer) && (r2.layer == this.layer || r2.id == this.layer))
                {
                    isvisible = false;
                    if (export && (this.layer == lin.startNode.layer || this.layer == lin.endNode.layer))
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

                        if (lin.arrow)
                        {
                            SolidBrush blueBrush = new SolidBrush(Color.DarkGray);
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
                            gfx.FillPolygon(blueBrush, curvePoints, newFillMode);
                        }
                        else
                        {
                            gfx.DrawLine(
                                myPen1,
                                (this.shift.x + cx + r1.position.x + r1.width / 2) / s,
                                (this.shift.y + cy + r1.position.y + r1.height / 2) / s,
                                (this.shift.x + cx + r2.position.x + r2.width / 2) / s,
                                (this.shift.y + cy + r2.position.y + r2.height / 2) / s
                            );
                        }

                    }
                }
            }

            // DRAW addingnode
            if (!export && this.addingNode && !this.zooming && (this.actualMousePos.x != this.startMousePos.x || this.actualMousePos.y != this.startMousePos.y))
            {
                gfx.DrawLine(
                    myPen1,
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
                    myPen1,
                    new Rectangle(
                        this.shift.x - this.startShift.x + this.startMousePos.x,
                        this.shift.y - this.startShift.y + this.startMousePos.y,
                        20, 20
                    )
                );
            }

            // DRAW nodes
            foreach (Node rec in diagram.Nodes) // Loop through List with foreach
            {
                if (rec.layer == this.layer || rec.id == this.layer)
                {
                    // vylucenie moznosti ktore netreba vykreslovat
                    isvisible = false;
                    if (export && this.layer == rec.layer)
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
                                    /*else // tieto vetvy sú neni potrebné nezrýchlia vykreslovanie
                                    if (0 <= (this.shiftx + rec.x) / s && (this.shiftx + rec.x) / s <= 0 + this.ClientSize.Width && 0 <= (this.shifty + rec.y) / s && (this.shifty + rec.y) / s <= 0 + this.ClientSize.Height)
                                    {
                                        isvisible = true;
                                    }
                                    else
                                        if (0 <= (this.shiftx + rec.x) / s && (this.shiftx + rec.x) / s <= 0 + this.ClientSize.Width && 0 <= (this.shifty + rec.y + rec.height) / s && (this.shifty + rec.y + rec.height) / s <= 0 + this.ClientSize.Height)
                                    {
                                        isvisible = true;
                                    }
                                    else
                                        if (0 <= (this.shiftx + rec.x + rec.width) / s && (this.shiftx + rec.x + rec.width) / s <= 0 + this.ClientSize.Width && 0 <= (this.shifty + rec.height) / s && (this.shifty + rec.height) / s <= 0 + this.ClientSize.Height)
                                    {
                                        isvisible = true;
                                    }
                                    else
                                    if (0 <= (this.shiftx + rec.x + rec.width) / s && (this.shiftx + rec.x + rec.width) / s <= 0 + this.ClientSize.Width && 0 <= (this.shifty + rec.y + rec.height) / s && (this.shifty + rec.y + rec.height) / s <= 0 + this.ClientSize.Height)
                                    {
                                        isvisible = true;
                                    }*/
                                    else
                                    {
                                        isvisible = true;
                                    }

                    if (isvisible)
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
                                gfx.DrawString((rec.position.x).ToString() + "," + (-rec.position.y).ToString(), drawFont, drawBrush, (this.shift.x + rec.position.x) / s, (this.shift.y + rec.position.y - 20) / s);
                            }

                            // DRAW rectangle
                            Rectangle rect1 = new Rectangle(
                                (int)((this.shift.x + cx + rec.position.x) / s),
                                (int)((this.shift.y + cy + rec.position.y) / s),
                                (int)((rec.width) / s),
                                (int)((rec.height) / s)
                            );

                            // DRAW border

                            if (rec.text.Trim() == "") // draw empty point
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

                                if (rec.shortcut > 0 && !export)
                                {
                                    gfx.FillEllipse(
                                        new SolidBrush(rec.color),
                                        new Rectangle(
                                            (int)((this.shift.x + cx + rec.position.x - 5) / s),
                                            (int)((this.shift.y + cy + rec.position.y - 5) / s),
                                            (int)((5) / s),
                                            (int)((5) / s)
                                        )
                                    );
                                }
                            }
                            else
                            {
                                if (!rec.transparent) // draw fill node
                                {
                                    gfx.FillRectangle(new SolidBrush(rec.color), rect1);
                                    if (this.diagram.options.borders) gfx.DrawRectangle(myPen1, rect1);
                                }

                                if (rec.haslayer && !export) // draw layer indicator
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

                                if (rec.shortcut > 0 && !export)
                                {
                                    gfx.FillEllipse(
                                        new SolidBrush(rec.color),
                                        new Rectangle(
                                            (int)((this.shift.x + cx + rec.position.x - 3) / s),
                                            (int)((this.shift.y + cy + rec.position.y - 3) / s),
                                            (int)((6) / s),
                                            (int)((6) / s)
                                        )
                                    );
                                }

                                // DRAW text
                                RectangleF rect2 = new RectangleF(
                                    (int)((this.shift.x + cx + rec.position.x + this.diagram.NodePadding) / s),
                                    (int)((this.shift.y + cy + rec.position.y + this.diagram.NodePadding) / s),
                                    (int)((rec.width - this.diagram.NodePadding) / s),
                                    (int)((rec.height - this.diagram.NodePadding) / s)
                                );
                                gfx.DrawString(rec.text, new Font(rec.font.FontFamily, rec.font.Size / s, rec.font.Style), new SolidBrush(rec.fontcolor), rect2);
                            }
                        }
                    }
                }

            }

            // DRAW select - zvyraznenie výberu viacerich elementov (multiselect)
            if (!export && this.selecting && (this.actualMousePos.x != this.startMousePos.x || this.actualMousePos.y != this.startMousePos.y))
            {
                myPen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);

                int a = (int)(+this.shift.x - this.startShift.x + this.startMousePos.x * this.scale);
                int b = (int)(+this.shift.y - this.startShift.y + this.startMousePos.y * this.scale);
                int c = (int)(this.actualMousePos.x * this.scale);
                int d = (int)(this.actualMousePos.y * this.scale);
                int temp;
                if (c < a) { temp = a; a = c; c = temp; }
                if (d < b) { temp = d; d = b; b = temp; }

                gfx.FillRectangle(new SolidBrush(Color.FromArgb(100, 10, 200, 200)), new Rectangle((int)(a / this.scale), (int)(b / this.scale), (int)((c - a) / this.scale), (int)((d - b) / this.scale)));
            }

            // ZOOMING draw mini screen
            if (!export && this.zooming)
            {
                myPen = new System.Drawing.Pen(System.Drawing.Color.Gray, 1);
                gfx.DrawRectangle(
                    myPen,
                    new Rectangle(
                        (int)(this.ClientSize.Width / 2 - this.ClientSize.Width / 2 / s),
                        (int)(this.ClientSize.Height / 2 - this.ClientSize.Height / 2 / s),
                        (int)(this.ClientSize.Width / s),
                        (int)(this.ClientSize.Height / s)
                    )
                );
            }

            // DRAW coordinates
            if (this.diagram.options.coordinates)
            {
                Font drawFont = new Font("Arial", 10);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                gfx.DrawString((-this.shift.x).ToString() + "," + this.shift.y.ToString() + " (" + this.ClientSize.Width.ToString() + "x" + this.ClientSize.Height.ToString() + ")", drawFont, drawBrush, 10, 10);
            }

            // vykreslenie scrollbarov
            if (!export && bottomScrollBar != null && rightScrollBar != null)
            {
                bottomScrollBar.Paint(gfx);
                rightScrollBar.Paint(gfx);
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

        /*************************************************************************************************************************/

        // NODE create
        public Node CreateNode(int x, int y, bool SelectAfterCreate = true, string text = "")
        {
            var rec = this.diagram.CreateNode((int)(x * this.scale - this.shift.x), (int)(y * this.scale - this.shift.y), this.layer, text);
            if (rec != null)
            {
                if (SelectAfterCreate) this.SelectOnlyOneNode(rec);
            }
            return rec;
        }

        // NODE create after
        private void addNodeAfterNode()
        {
            if (this.SelectedNodes.Count() == 1)
            {
                if (!nodeNameEdit.Visible)
                {
                    this.prevSelectedNode = this.SelectedNodes[0];
                    Point ptCursor = Cursor.Position;
                    ptCursor = PointToClient(ptCursor);
                    nodeNamePanel.Left = this.SelectedNodes[0].position.x + this.shift.x + this.SelectedNodes[0].width + 10;
                    nodeNamePanel.Top = this.SelectedNodes[0].position.y + this.shift.y;
                    nodeNamePanel.Width = 100;
                    nodeNameEdit.Text = "";
                    nodeNameEdit.SelectionStart = nodeNameEdit.Text.Length;
                    editingNodeName = true;
                    nodeNamePanel.Show();
                    nodeNameEdit.Focus();
                }
            }
        }

        // NODE open link directory
        public void openLinkDirectory()
        {
            if (this.SelectedNodes.Count() > 0)
            {
                foreach (Node node in this.SelectedNodes)
                {
                    if (node.link.Trim().Length > 0)
                    {
                        if (Directory.Exists(node.link)) // open directory of selected nods
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
                            if (File.Exists(node.link)) // open directory of selected files
                            {
                                try
                                {
                                    string parent_diectory = new FileInfo(this.SelectedNodes[0].link).Directory.FullName;
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
                if (DiagramView.SelectedNodes.Count() > 0)
                {
                    bool canRefresh = false;
                    for (int i = DiagramView.SelectedNodes.Count() - 1; i >= 0; i--)
                    {
                        if (this.diagram.canDeleteNode(DiagramView.SelectedNodes[i]))
                        {
                            this.diagram.DeleteNode(DiagramView.SelectedNodes[i]);
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

        // NODE Go to node
        public void GoToNode(Node rec)
        {
            if (rec != null)
            {
                this.shift.x = -rec.position.x + this.ClientSize.Width / 2;
                this.shift.y = -rec.position.y + this.ClientSize.Height / 2;
            }
        }

        // NODE Najdenie nody podla pozicie myši
        public Node findNodeInMousePosition(int x, int y)
        {
            return this.diagram.findNodeInPosition((int)(x * this.scale - this.shift.x), (int)(y * this.scale - this.shift.y), this.layer);
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
                if (rec.shortcut > 0) // GO TO LINK
                {
                    Node target = this.diagram.GetNodeByID(rec.shortcut);
                    this.GoToNode(target);
                    this.diagram.InvalidateDiagram();
                }
                else
                if (rec.link.Length > 0)
                {
                    // set current directory to current diagrm file destination
                    if (File.Exists(this.diagram.FileName))
                    {
                        Directory.SetCurrentDirectory(new FileInfo(this.diagram.FileName).DirectoryName);
                    }

                    Match matchFileOpenOnPosition = (new Regex("^([^#]+)#(.*)$")).Match(rec.link.Trim());


                    //- ak je link nastaveny na "script" po dvojkliku sa vyhodnoti telo nody ako macro
                    if (matchFileOpenOnPosition.Success && File.Exists(Path.GetFullPath(matchFileOpenOnPosition.Groups[1].Value)))       // OPEN FILE ON POSITION
                    {
                        try
                        {

                            String fileName = matchFileOpenOnPosition.Groups[1].Value;
                            String searchString = matchFileOpenOnPosition.Groups[2].Value.Trim();

                            Match matchNumber = (new Regex("^(\\d+)$")).Match(searchString);

                            if (!matchNumber.Success)
                            {
                                searchString = Os.fndLineNumber(fileName, searchString).ToString();
                            }

                            String editFileCmd = this.main.parameters.texteditor;
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
                            %TEXT%     - aktualny text nody
                            %NOTE%     - poznamka v node
                            %ID%       - text v id nody
                            %FILENAME% - meno diagramu
                        */

                        string cmd = rec.link;                     // replace variables in link
                        cmd = cmd.Replace("%TEXT%", rec.text);
                        cmd = cmd.Replace("%NOTE%", rec.note);
                        cmd = cmd.Replace("%ID%", rec.id.ToString());
                        cmd = cmd.Replace("%FILENAME%", this.diagram.FileName);

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
