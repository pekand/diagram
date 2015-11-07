using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram
{
    public class Popup : ContextMenuStrip
    {
        public DiagramView diagramView = null;       // diagram ktory je previazany z pohladom

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
        private System.Windows.Forms.ToolStripMenuItem layerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setStartPositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem releaseNoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;

        public Popup(System.ComponentModel.IContainer container, DiagramView diagramView) : base(container)
        {
            this.diagramView = diagramView;

            InitializeComponent();

#if DEBUG
            this.consoleToolStripMenuItem.Visible = true;
            this.coordinatesToolStripMenuItem.Visible = true;
#endif

            this.gridToolStripMenuItem.Checked = this.diagramView.diagram.options.grid;
            this.bordersToolStripMenuItem.Checked = this.diagramView.diagram.options.borders;
            this.coordinatesToolStripMenuItem.Checked = this.diagramView.diagram.options.coordinates;
            this.readonlyToolStripMenuItem.Checked = this.diagramView.diagram.options.readOnly;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
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
            this.groupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.releaseNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            // 
            // PopupMenu
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.Name = "contextMenuStrip1";
            this.Size = new System.Drawing.Size(165, 280);
            this.Opening += new System.ComponentModel.CancelEventHandler(this.PopupMenu_Opening);
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
            this.leftToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.leftToolStripMenuItem.Text = "Left";
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            // 
            // rightToolStripMenuItem
            // 
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            this.rightToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.rightToolStripMenuItem.Text = "Right";
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
            // 
            // toLineToolStripMenuItem
            // 
            this.toLineToolStripMenuItem.Name = "toLineToolStripMenuItem";
            this.toLineToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.toLineToolStripMenuItem.Text = "To line";
            this.toLineToolStripMenuItem.Click += new System.EventHandler(this.toLineToolStripMenuItem_Click);
            // 
            // inColumnToolStripMenuItem
            // 
            this.inColumnToolStripMenuItem.Name = "inColumnToolStripMenuItem";
            this.inColumnToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.inColumnToolStripMenuItem.Text = "In column";
            this.inColumnToolStripMenuItem.Click += new System.EventHandler(this.inColumnToolStripMenuItem_Click);
            // 
            // groupToolStripMenuItem
            // 
            this.groupToolStripMenuItem.Name = "groupToolStripMenuItem";
            this.groupToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.groupToolStripMenuItem.Text = "Group";
            this.groupToolStripMenuItem.Click += new System.EventHandler(this.groupToolStripMenuItem_Click);
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
            this.releaseNoteToolStripMenuItem,
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
            // releaseNoteToolStripMenuItem
            // 
            this.releaseNoteToolStripMenuItem.Name = "releaseNoteToolStripMenuItem";
            this.releaseNoteToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.releaseNoteToolStripMenuItem.Text = "Release Note";
            this.releaseNoteToolStripMenuItem.Click += new System.EventHandler(this.releaseNoteToolStripMenuItem_Click);
        }
        #endregion

        /*************************************************************************************************************************/

        // MENU Manage                                                                                // POPUP MENU
        public void PopupMenu_Opening(object sender, CancelEventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() == 0)
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

            if (this.diagramView.SelectedNodes.Count() == 1)
            {
                editToolStripMenuItem.Visible = true;
                colorToolStripMenuItem.Visible = true;
                linkToolStripMenuItem.Visible = this.diagramView.SelectedNodes[0].link.Trim() != "";
                copylinkToolStripMenuItem.Enabled = this.diagramView.SelectedNodes[0].link.Trim() != "";
                openlinkToolStripMenuItem.Enabled = this.diagramView.SelectedNodes[0].link.Trim() != "";
                toolStripMenuItem2.Visible = true;//separator
                alignToolStripMenuItem.Visible = false;
                openDirectoryToolStripMenuItem1.Visible = false;
                if (this.diagramView.SelectedNodes[0].link.Trim().Length > 0 && File.Exists(this.diagramView.SelectedNodes[0].link))
                    openDirectoryToolStripMenuItem1.Visible = true;

            }

            if (this.diagramView.SelectedNodes.Count() > 1)
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

            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                bool hasShortcut = false;
                foreach (Node node in this.diagramView.SelectedNodes)
                {
                    if (node.shortcut > 0)
                    {
                        hasShortcut = true;
                        break;
                    }
                }

                if (hasShortcut)
                {
                    removeShortcutToolStripMenuItem.Visible = true;
                }
            }

            if (this.diagramView.diagram.password == "")
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
            if (this.diagramView.SelectedNodes.Count() == 1)
            {
                this.diagramView.diagram.EditNode(this.diagramView.SelectedNodes[0]);
            }
        }

        // MENU Link Open
        public void openlinkToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.OpenLinkAsync(this.diagramView.SelectedNodes[0]);
            }
        }

        // MENU Link Copy
        public void copylinkToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.copyLinkToClipboard(this.diagramView.SelectedNodes[0]);
            }
        }

        // MENU New
        public void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.main.OpenDiagram();
        }

        // MENU Save
        public void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.save();
        }

        // MENU Save As
        public void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.saveas();
        }

        // MENU export to png
        private void exportToPngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.exportFile.ShowDialog() == DialogResult.OK)
            {
                this.diagramView.exportDiagramToPng();
            }
        }

        // MENU export to txt
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.saveTextFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.diagramView.exportDiagramToTxt(this.diagramView.saveTextFileDialog.FileName);
            }
        }

        // MENU Open
        public void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.open();
        }

        // MENU Open Directory  - otvory adresar v ktorom sa nachadza prave otvreny subor
        public void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.openDiagramDirectory();
        }

        // MENU Encription
        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.main.newPasswordForm == null)
            {
                this.diagramView.main.newPasswordForm = new NewPasswordForm(this.diagramView.main);
            }

            this.diagramView.main.newPasswordForm.Clear();
            this.diagramView.main.newPasswordForm.ShowDialog();
            if (!this.diagramView.main.newPasswordForm.cancled)
            {
                this.diagramView.diagram.password = this.diagramView.main.newPasswordForm.GetPassword();
                this.diagramView.diagram.unsave();
            }
        }

        // MENU Change password
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.main.changePasswordForm == null)
            {
                this.diagramView.main.changePasswordForm = new ChangePasswordForm(this.diagramView.main);
            }

            this.diagramView.main.changePasswordForm.Clear();
            this.diagramView.main.changePasswordForm.oldpassword = this.diagramView.diagram.password;
            this.diagramView.main.changePasswordForm.ShowDialog();
            if (!this.diagramView.main.changePasswordForm.cancled)
            {
                this.diagramView.diagram.password = this.diagramView.main.changePasswordForm.GetPassword();
                this.diagramView.diagram.unsave();
            }
        }

        // MENU Console
        public void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.showConsole();
        }

        // MENU Exit
        public void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // MENU Center
        public void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.GoToHome();
        }

        // MENU set home position
        private void setStartPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.setCurentPositionAsHomePosition();
        }

        // MENU Read only
        public void readonlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.readOnly = this.readonlyToolStripMenuItem.Checked;
        }

        // MENU Grid check
        public void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.grid = this.gridToolStripMenuItem.Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Option Borders
        public void bordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.borders = this.bordersToolStripMenuItem.Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Option Default font
        public void defaultFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.selectDefaultFont();
        }

        // MENU coordinates
        public void coordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.coordinates = this.coordinatesToolStripMenuItem.Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Change color
        public void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.selectColor();
        }

        // MENU VIEW NEW VIEW
        private void newViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // otvorenie novej insancie DiagramView
            this.diagramView.diagram.openDiagramView();
        }

        // MENU Layer In
        public void inToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() == 1)
            {
                this.diagramView.LayerIn(this.diagramView.SelectedNodes[0]);
            }
        }

        // MENU Layer Out
        public void outToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.diagramView.LayerOut();
        }

        // MENU align left
        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignLeft(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align right
        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignRight(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to line
        private void toLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignToLine(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to column
        private void inColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignToColumn(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to group to column
        private void groupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignCompact(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU remove shortcut
        private void removeShortcutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.removeShortcuts(this.diagramView.SelectedNodes);
            }
        }

        // MENU open directory for file in link
        private void openDirectoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.diagramView.openLinkDirectory();
        }

        // MENU reset font
        private void resetFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.ResetFont(this.diagramView.SelectedNodes);
            }
            else
            {
                this.diagramView.diagram.ResetFont();
            }
        }

        // MENU Homepage navštíviť domovskú stránku
        private void visitWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Network.openUrl(this.diagramView.main.options.home_page);
        }

        // MENU Show release note
        private void releaseNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string releasNotePath = Os.getCurrentApplicationDirectory() + Os.getSeparator() + this.diagramView.main.options.release_note;
            if (Os.FileExists(releasNotePath))
            {
                string releaseNoteUrl = "file:///" + Os.toBackslash(Os.getCurrentApplicationDirectory()) + "/" + this.diagramView.main.options.release_note;
                Network.openUrl(releaseNoteUrl);
                Program.log.write("open release note: " + releasNotePath);
            }
            else
            {
                Program.log.write("open release note: error: file not exist" + releasNotePath);
            }
        }

        // MENU About navštíviť domovskú stránku
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.main.aboutForm == null)
            {
                this.diagramView.main.aboutForm = new AboutForm(this.diagramView.main);
            }

            this.diagramView.main.aboutForm.Show();
        }

    }
}
