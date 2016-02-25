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
    [System.ComponentModel.DesignerCategory("Code")]

    public class Popup : ContextMenuStrip
    {
        public DiagramView diagramView = null;       // diagram ktory je previazany z pohladom

        Dictionary<string, ToolStripMenuItem> items = new Dictionary<string, ToolStripMenuItem>();
        Dictionary<string, ToolStripSeparator> separators = new Dictionary<string, ToolStripSeparator>();

        public Popup(System.ComponentModel.IContainer container, DiagramView diagramView) : base(container)
        {
            this.diagramView = diagramView;

            InitializeComponent();

#if DEBUG
            items["consoleItem"].Visible = true;
            items["coordinatesItem"].Visible = true;
#endif
            items["restoreWindowItem"].Checked = this.diagramView.diagram.options.restoreWindow;
            items["gridItem"].Checked = this.diagramView.diagram.options.grid;
            items["bordersItem"].Checked = this.diagramView.diagram.options.borders;
            items["coordinatesItem"].Checked = this.diagramView.diagram.options.coordinates;
            items["readonlyItem"].Checked = this.diagramView.diagram.options.readOnly;
        }
			
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
          
            //
            // editItem
            //
            items.Add("editItem", new System.Windows.Forms.ToolStripMenuItem());
            items["editItem"].Name = "editItem";
            items["editItem"].Text = "Edit";
            items["editItem"].Click += new System.EventHandler(this.editItem_Click);
            //
            // colorItem
            //
            items.Add("colorItem", new System.Windows.Forms.ToolStripMenuItem());
            items["colorItem"].Name = "colorItem";
            items["colorItem"].Text = "Color";
            items["colorItem"].Click += new System.EventHandler(this.colorItem_Click);
            //
            // removeShortcutItem
            //
            items.Add("removeShortcutItem", new System.Windows.Forms.ToolStripMenuItem());
            items["removeShortcutItem"].Name = "removeShortcutItem";
            items["removeShortcutItem"].Text = "Remove shortcut";
            items["removeShortcutItem"].Click += new System.EventHandler(this.removeShortcutItem_Click);
            //
            // openlinkItem
            //
            items.Add("openlinkItem", new System.Windows.Forms.ToolStripMenuItem());
            items["openlinkItem"].Name = "openlinkItem";
            items["openlinkItem"].Text = "Open";
            items["openlinkItem"].Click += new System.EventHandler(this.openlinkItem_Click);
            //
            // openLinkDirectoryItem
            //
            items.Add("openLinkDirectoryItem", new System.Windows.Forms.ToolStripMenuItem());
            items["openLinkDirectoryItem"].Name = "openLinkDirectoryItem";
            items["openLinkDirectoryItem"].Text = "Open directory";
            items["openLinkDirectoryItem"].Click += new System.EventHandler(this.openLinkDirectoryItem_Click);
            //
            // linkItem
            //
            items.Add("linkItem", new System.Windows.Forms.ToolStripMenuItem());
            items["linkItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["openlinkItem"],
                items["openLinkDirectoryItem"]
            });
            items["linkItem"].Name = "linkItem";
            items["linkItem"].Text = "Link";
            //
            // leftItem
            //
            items.Add("leftItem", new System.Windows.Forms.ToolStripMenuItem());
            items["leftItem"].Name = "leftItem";
            items["leftItem"].Text = "Left";
            items["leftItem"].Click += new System.EventHandler(this.leftItem_Click);
            //
            // rightItem
            //
            items.Add("rightItem", new System.Windows.Forms.ToolStripMenuItem());
            items["rightItem"].Name = "rightItem";
            items["rightItem"].Text = "Right";
            items["rightItem"].Click += new System.EventHandler(this.rightItem_Click);
            //
            // toLineItem
            //
            items.Add("toLineItem", new System.Windows.Forms.ToolStripMenuItem());
            items["toLineItem"].Name = "toLineItem";
            items["toLineItem"].Text = "To line";
            items["toLineItem"].Click += new System.EventHandler(this.toLineItem_Click);
            //
            // inColumnItem
            //
            items.Add("inColumnItem", new System.Windows.Forms.ToolStripMenuItem());
            items["inColumnItem"].Name = "inColumnItem";
            items["inColumnItem"].Text = "In column";
            items["inColumnItem"].Click += new System.EventHandler(this.inColumnItem_Click);
            //
            // groupVericalItem
            //
            items.Add("groupVericalItem", new System.Windows.Forms.ToolStripMenuItem());
            items["groupVericalItem"].Name = "groupVericalItem";
            items["groupVericalItem"].Text = "Group vertical";
            items["groupVericalItem"].Click += new System.EventHandler(this.groupVericalItem_Click);
            //
            // groupHorizontalItem
            //
            items.Add("groupHorizontalItem", new System.Windows.Forms.ToolStripMenuItem());
            items["groupHorizontalItem"].Name = "groupHorizontalItem";
            items["groupHorizontalItem"].Text = "Group horizontal";
            items["groupHorizontalItem"].Click += new System.EventHandler(this.groupHorizontalItem_Click);
            //
            // alignItem
            //
            items.Add("alignItem", new System.Windows.Forms.ToolStripMenuItem());
            items["alignItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["leftItem"],
                items["rightItem"],
                items["toLineItem"],
                items["inColumnItem"],
                items["groupVericalItem"],
                items["groupHorizontalItem"]
            });
            items["alignItem"].Name = "alignItem";
            items["alignItem"].Text = "Align";
            //
            // quickActionSeparator
            //
            separators.Add("quickActionSeparator", new System.Windows.Forms.ToolStripSeparator());
            separators["quickActionSeparator"].Name = "quickActionSeparator";
            //
            // newItem
            //
            items.Add("newItem", new System.Windows.Forms.ToolStripMenuItem());
            items["newItem"].Name = "newItem";
            items["newItem"].Text = "New";
            items["newItem"].Click += new System.EventHandler(this.newItem_Click);
            //
            // saveItem
            //
            items.Add("saveItem", new System.Windows.Forms.ToolStripMenuItem());
            items["saveItem"].Name = "saveItem";
            items["saveItem"].Text = "Save";
            items["saveItem"].Click += new System.EventHandler(this.saveItem_Click);
            //
            // saveAsItem
            //
            items.Add("saveAsItem", new System.Windows.Forms.ToolStripMenuItem());
            items["saveAsItem"].Name = "saveAsItem";
            items["saveAsItem"].Text = "Save As";
            items["saveAsItem"].Click += new System.EventHandler(this.saveAsItem_Click);
            //
            // textItem
            //
            items.Add("textItem", new System.Windows.Forms.ToolStripMenuItem());
            items["textItem"].Name = "textItem";
            items["textItem"].Text = "Text";
            items["textItem"].Click += new System.EventHandler(this.textItem_Click);
            //
            // exportToPngItem
            //
            items.Add("exportToPngItem", new System.Windows.Forms.ToolStripMenuItem());
            items["exportToPngItem"].Name = "exportToPngItem";
            items["exportToPngItem"].Text = "Export to png";
            items["exportToPngItem"].Click += new System.EventHandler(this.exportToPngItem_Click);
            //
            // exportItem
            //
            items.Add("exportItem", new System.Windows.Forms.ToolStripMenuItem());
            items["exportItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["textItem"],
                items["exportToPngItem"]
            });
            items["exportItem"].Name = "exportItem";
            items["exportItem"].Text = "Export";
            //
            // openItem
            //
            items.Add("openItem", new System.Windows.Forms.ToolStripMenuItem());
            items["openItem"].Name = "openItem";
            items["openItem"].Text = "Open";
            items["openItem"].Click += new System.EventHandler(this.openItem_Click);
            //
            // exitItem
            //
            items.Add("exitItem", new System.Windows.Forms.ToolStripMenuItem());
            items["exitItem"].Name = "exitItem";
            items["exitItem"].Text = "Exit";
            items["exitItem"].Click += new System.EventHandler(this.exitItem_Click);
            //
            // fileItem
            //
            items.Add("fileItem", new System.Windows.Forms.ToolStripMenuItem());
            items["fileItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["newItem"],
                items["saveItem"],
                items["saveAsItem"],
                items["exportItem"],
                items["openItem"],
                items["exitItem"]
            });
            items["fileItem"].Name = "fileItem";
            items["fileItem"].Text = "File";
            //
            // copyItem
            //
            items.Add("copyItem", new System.Windows.Forms.ToolStripMenuItem());
			items["copyItem"].Name = "copyItem";
            items["copyItem"].Text = "Copy";
            items["copyItem"].Click += new System.EventHandler(this.copyItem_Click);
            //
            // cutItem
            //
            items.Add("cutItem", new System.Windows.Forms.ToolStripMenuItem());
			items["cutItem"].Name = "cutItem";
            items["cutItem"].Text = "Cut";
            items["cutItem"].Click += new System.EventHandler(this.cutItem_Click);
            //
            // pasteItem
            //
            items.Add("pasteItem", new System.Windows.Forms.ToolStripMenuItem());
			items["pasteItem"].Name = "pasteItem";
            items["pasteItem"].Text = "Paste";
            items["pasteItem"].Click += new System.EventHandler(this.pasteItem_Click);
            //
            // editSeparator
            //
            separators.Add("editSeparator", new System.Windows.Forms.ToolStripSeparator());
            separators["editSeparator"].Name = "editSeparator";
            //
            // copyLinkItem
            //
            items.Add("copyLinkItem", new System.Windows.Forms.ToolStripMenuItem());
            items["copyLinkItem"].Name = "copyLinkItem";
            items["copyLinkItem"].Text = "Copy link";
            items["copyLinkItem"].Click += new System.EventHandler(this.copyLinkItem_Click);
            //
            // copyNoteItem
            //
            items.Add("copyNoteItem", new System.Windows.Forms.ToolStripMenuItem());
            items["copyNoteItem"].Name = "copyNoteItem";
            items["copyNoteItem"].Text = "Copy note";
            items["copyNoteItem"].Click += new System.EventHandler(this.copyNoteItem_Click);
            //
            // pasteToLinkItem
            //
            items.Add("pasteToLinkItem", new System.Windows.Forms.ToolStripMenuItem());
            items["pasteToLinkItem"].Name = "pasteToLinkItem";
            items["pasteToLinkItem"].Text = "Paste to link";
            items["pasteToLinkItem"].Click += new System.EventHandler(this.pasteToLinkItem_Click);
            //
            // pasteToNoteItem
            //
            items.Add("pasteToNoteItem", new System.Windows.Forms.ToolStripMenuItem());
            items["pasteToNoteItem"].Name = "pasteToNoteItem";
            items["pasteToNoteItem"].Text = "Paste to note";
            items["pasteToNoteItem"].Click += new System.EventHandler(this.pasteToNoteItem_Click);
            //
            // editMenuItem
            //
            items.Add("editMenuItem", new System.Windows.Forms.ToolStripMenuItem());
            items["editMenuItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["copyItem"],
                items["cutItem"],
                items["pasteItem"],
                separators["editSeparator"],
                items["copyLinkItem"],
                items["copyNoteItem"],
                items["pasteToLinkItem"],
                items["pasteToNoteItem"]
            });
            items["editMenuItem"].Name = "editMenuItem";
            items["editMenuItem"].Text = "Edit";
            //
            // transparentItem
            //
            items.Add("transparentItem", new System.Windows.Forms.ToolStripMenuItem());
            items["transparentItem"].Name = "transparentItem";
            items["transparentItem"].Text = "Transparent";
            items["transparentItem"].Click += new System.EventHandler(this.transparentItem_Click);
            //
            // fontItem
            //
            items.Add("fontItem", new System.Windows.Forms.ToolStripMenuItem());
            items["fontItem"].Name = "fontItem";
            items["fontItem"].Text = "Font";
            items["fontItem"].Click += new System.EventHandler(this.fontItem_Click);
            //
            // fontColorItem
            //
            items.Add("fontColorItem", new System.Windows.Forms.ToolStripMenuItem());
            items["fontColorItem"].Name = "fontColorItem";
            items["fontColorItem"].Text = "Font color";
            items["fontColorItem"].Click += new System.EventHandler(this.fontColorItem_Click);
            //
            // editLinkItem
            //
            items.Add("editLinkItem", new System.Windows.Forms.ToolStripMenuItem());
            items["editLinkItem"].Name = "editLinkItem";
            items["editLinkItem"].Text = "Edit link";
            items["editLinkItem"].Click += new System.EventHandler(this.editLinkItem_Click);
            //
            // bringTopItem
            //
            items.Add("bringTopItem", new System.Windows.Forms.ToolStripMenuItem());
            items["bringTopItem"].Name = "bringTopItem";
            items["bringTopItem"].Text = "Bring to top";
            items["bringTopItem"].Click += new System.EventHandler(this.bringTopItem_Click);
            //
            // bringBottomItem
            //
            items.Add("bringBottomItem", new System.Windows.Forms.ToolStripMenuItem());
            items["bringBottomItem"].Name = "bringBottomItem";
            items["bringBottomItem"].Text = "Bring to bottom";
            items["bringBottomItem"].Click += new System.EventHandler(this.bringBottomItem_Click);
            //
            // protectItem
            //
            items.Add("protectItem", new System.Windows.Forms.ToolStripMenuItem());
            items["protectItem"].Name = "protectItem";
            items["protectItem"].Text = "Protect";
            items["protectItem"].Click += new System.EventHandler(this.protectItem_Click);
            //
            // nodeItem
            //
            items.Add("nodeItem", new System.Windows.Forms.ToolStripMenuItem());
            items["nodeItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["transparentItem"],
                items["fontItem"],
                items["fontColorItem"],
                items["editLinkItem"],
                items["bringTopItem"],
                items["bringBottomItem"],
                items["protectItem"]
            });
            items["nodeItem"].Name = "nodeItem";
            items["nodeItem"].Text = "Node";
            //
            // lineColorItem
            //
            items.Add("lineColorItem", new System.Windows.Forms.ToolStripMenuItem());
            items["lineColorItem"].Name = "lineColorItem";
            items["lineColorItem"].Text = "Color";
            items["lineColorItem"].Click += new System.EventHandler(this.lineColorItem_Click);
            //
            // lineWidthItem
            //
            items.Add("lineWidthItem", new System.Windows.Forms.ToolStripMenuItem());
            items["lineWidthItem"].Name = "lineWidthItem";
            items["lineWidthItem"].Text = "Width";
            items["lineWidthItem"].Click += new System.EventHandler(this.lineWidthItem_Click);
            //
            // lineItem
            //
            items.Add("lineItem", new System.Windows.Forms.ToolStripMenuItem());
            items["lineItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["lineColorItem"],
                items["lineWidthItem"]
            });
            items["lineItem"].Name = "lineItem";
            items["lineItem"].Text = "Line";
            //
            // imageAddItem
            //
            items.Add("imageAddItem", new System.Windows.Forms.ToolStripMenuItem());
            items["imageAddItem"].Name = "imageAddItem";
            items["imageAddItem"].Text = "Add image";
            items["imageAddItem"].Click += new System.EventHandler(this.imageAddItem_Click);
            //
            // imageRemoveItem
            //
            items.Add("imageRemoveItem", new System.Windows.Forms.ToolStripMenuItem());
            items["imageRemoveItem"].Name = "imageRemoveItem";
            items["imageRemoveItem"].Text = "Remove image";
            items["imageRemoveItem"].Click += new System.EventHandler(this.imageRemoveItem_Click);
            //
            // imageEmbeddedItem
            //
            items.Add("imageEmbeddedItem", new System.Windows.Forms.ToolStripMenuItem());
            items["imageEmbeddedItem"].Name = "imageEmbeddedItem";
            items["imageEmbeddedItem"].Text = "Embed image";
            items["imageEmbeddedItem"].Click += new System.EventHandler(this.imageEmbeddedItem_Click);
            //
            // imageItem
            //
            items.Add("imageItem", new System.Windows.Forms.ToolStripMenuItem());
            items["imageItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["imageAddItem"],
                items["imageRemoveItem"],
                items["imageEmbeddedItem"]
            });
            items["imageItem"].Name = "imageItem";
            items["imageItem"].Text = "Image";
            //
            // deploayAttachmentItem
            //
            items.Add("deploayAttachmentItem", new System.Windows.Forms.ToolStripMenuItem());
            items["deploayAttachmentItem"].Name = "deploayAttachmentItem";
            items["deploayAttachmentItem"].Text = "Deploy attachment";
            items["deploayAttachmentItem"].Click += new System.EventHandler(this.deploayAttachmentItem_Click);
            //
            // includeFileItem
            //
            items.Add("includeFileItem", new System.Windows.Forms.ToolStripMenuItem());
            items["includeFileItem"].Name = "includeFileItem";
            items["includeFileItem"].Text = "Add file";
            items["includeFileItem"].Click += new System.EventHandler(this.includeFileItem_Click);
            //
            // includeDirectoryItem
            //
            items.Add("includeDirectoryItem", new System.Windows.Forms.ToolStripMenuItem());
            items["includeDirectoryItem"].Name = "includeDirectoryItem";
            items["includeDirectoryItem"].Text = "Add directory";
            items["includeDirectoryItem"].Click += new System.EventHandler(this.includeDirectoryItem_Click);
            //
            // removeAttachmentItem
            //
            items.Add("removeAttachmentItem", new System.Windows.Forms.ToolStripMenuItem());
            items["removeAttachmentItem"].Name = "removeAttachmentItem";
            items["removeAttachmentItem"].Text = "Remove";
            items["removeAttachmentItem"].Click += new System.EventHandler(this.removeFileItem_Click);
            //
            // attachmentItem
            //
            items.Add("attachmentItem", new System.Windows.Forms.ToolStripMenuItem());
            items["attachmentItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["deploayAttachmentItem"],
                items["includeFileItem"],
                items["includeDirectoryItem"],
                items["removeAttachmentItem"]
            });
            items["attachmentItem"].Name = "attachmentItem";
            items["attachmentItem"].Text = "Attachment";
            //
            // newViewItem
            //
            items.Add("newViewItem", new System.Windows.Forms.ToolStripMenuItem());
            items["newViewItem"].Name = "newViewItem";
            items["newViewItem"].Text = "New View";
            items["newViewItem"].Click += new System.EventHandler(this.newViewItem_Click);
            //
            // centerItem
            //
            items.Add("centerItem", new System.Windows.Forms.ToolStripMenuItem());
            items["centerItem"].Name = "centerItem";
            items["centerItem"].Text = "Center";
            items["centerItem"].Click += new System.EventHandler(this.centerItem_Click);
            //
            // setStartPositionItem
            //
            items.Add("setStartPositionItem", new System.Windows.Forms.ToolStripMenuItem());
            items["setStartPositionItem"].Name = "setStartPositionItem";
            items["setStartPositionItem"].Text = "Set start position";
            items["setStartPositionItem"].Click += new System.EventHandler(this.setStartPositionItem_Click);
            //
            // viewItem
            //
            items.Add("viewItem", new System.Windows.Forms.ToolStripMenuItem());
            items["viewItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["newViewItem"],
                items["centerItem"],
                items["setStartPositionItem"]
            });
            items["viewItem"].Name = "viewItem";
            items["viewItem"].Text = "View";
            //
            // inItem
            //
            items.Add("inItem", new System.Windows.Forms.ToolStripMenuItem());
            items["inItem"].Name = "inItem";
            items["inItem"].Text = "In";
            items["inItem"].Click += new System.EventHandler(this.inItem_Click);
            //
            // outItem
            //
            items.Add("outItem", new System.Windows.Forms.ToolStripMenuItem());
            items["outItem"].Name = "outItem";
            items["outItem"].Text = "Out";
            items["outItem"].Click += new System.EventHandler(this.outItem_Click);
            //
            // layerItem
            //
            items.Add("layerItem", new System.Windows.Forms.ToolStripMenuItem());
            items["layerItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["inItem"],
                items["outItem"]
            });
            items["layerItem"].Name = "layerItem";
            items["layerItem"].Text = "Layer";
            //
            // helpSeparator
            //
            separators.Add("helpSeparator", new System.Windows.Forms.ToolStripSeparator());
            separators["helpSeparator"].Name = "helpSeparator";
            //
            // openDiagramDirectoryItem
            //
            items.Add("openDiagramDirectoryItem", new System.Windows.Forms.ToolStripMenuItem());
            items["openDiagramDirectoryItem"].Name = "openDiagramDirectoryItem";
            items["openDiagramDirectoryItem"].Text = "Open Directory";
            items["openDiagramDirectoryItem"].Click += new System.EventHandler(this.openDiagramDirectoryItem_Click);
            //
            // toolsItem
            //
            items.Add("toolsItem", new System.Windows.Forms.ToolStripMenuItem());
            items["toolsItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["openDiagramDirectoryItem"]
            });
            items["toolsItem"].Name = "toolsItem";
            items["toolsItem"].Text = "Tools";
            //
            // encryptItem
            //
            items.Add("encryptItem", new System.Windows.Forms.ToolStripMenuItem());
            items["encryptItem"].Name = "encryptItem";
            items["encryptItem"].Text = "Encrypt";
            items["encryptItem"].Click += new System.EventHandler(this.encryptItem_Click);
            //
            // changePasswordItem
            //
            items.Add("changePasswordItem", new System.Windows.Forms.ToolStripMenuItem());
            items["changePasswordItem"].Name = "changePasswordItem";
            items["changePasswordItem"].Text = "Change password";
            items["changePasswordItem"].Click += new System.EventHandler(this.changePasswordItem_Click);
            //
            // readonlyItem
            //
            items.Add("readonlyItem", new System.Windows.Forms.ToolStripMenuItem());
            items["readonlyItem"].CheckOnClick = true;
            items["readonlyItem"].Name = "readonlyItem";
            items["readonlyItem"].Text = "Read only";
            items["readonlyItem"].Click += new System.EventHandler(this.readonlyItem_Click);
            //
            // restoreWindowItem
            //
            items.Add("restoreWindowItem", new System.Windows.Forms.ToolStripMenuItem());
            items["restoreWindowItem"].Checked = true;
            items["restoreWindowItem"].CheckOnClick = true;
            items["restoreWindowItem"].CheckState = System.Windows.Forms.CheckState.Checked;
            items["restoreWindowItem"].Name = "restoreWindowItem";
            items["restoreWindowItem"].Text = "Restore window";
            items["restoreWindowItem"].Click += new System.EventHandler(this.restoreWindowItem_Click);
            //
            // gridItem
            //
            items.Add("gridItem", new System.Windows.Forms.ToolStripMenuItem());
            items["gridItem"].Checked = true;
            items["gridItem"].CheckOnClick = true;
            items["gridItem"].CheckState = System.Windows.Forms.CheckState.Checked;
            items["gridItem"].Name = "gridItem";
            items["gridItem"].Text = "Grid";
            items["gridItem"].Click += new System.EventHandler(this.gridItem_Click);
            //
            // coordinatesItem
            //
            items.Add("coordinatesItem", new System.Windows.Forms.ToolStripMenuItem());
            items["coordinatesItem"].CheckOnClick = true;
            items["coordinatesItem"].Name = "coordinatesItem";
            items["coordinatesItem"].Text = "Coordinates";
            items["coordinatesItem"].Visible = false;
            items["coordinatesItem"].Click += new System.EventHandler(this.coordinatesItem_Click);
            //
            // bordersItem
            //
            items.Add("bordersItem", new System.Windows.Forms.ToolStripMenuItem());
            items["bordersItem"].CheckOnClick = true;
            items["bordersItem"].Name = "bordersItem";
            items["bordersItem"].Text = "Borders";
            items["bordersItem"].Click += new System.EventHandler(this.bordersItem_Click);
            //
            // defaultFontItem
            //
            items.Add("defaultFontItem", new System.Windows.Forms.ToolStripMenuItem());
            items["defaultFontItem"].Name = "defaultFontItem";
            items["defaultFontItem"].Text = "Default font";
            items["defaultFontItem"].Click += new System.EventHandler(this.defaultFontItem_Click);
            //
            // resetFontItem
            //
            items.Add("resetFontItem", new System.Windows.Forms.ToolStripMenuItem());
            items["resetFontItem"].Name = "resetFontItem";
            items["resetFontItem"].Text = "Reset font";
            items["resetFontItem"].Click += new System.EventHandler(this.resetFontItem_Click);
            //
            // optionItem
            //
            items.Add("optionItem", new System.Windows.Forms.ToolStripMenuItem());
            items["optionItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["encryptItem"],
                items["changePasswordItem"],
                items["readonlyItem"],
                items["restoreWindowItem"],
                items["gridItem"],
                items["coordinatesItem"],
                items["bordersItem"],
                items["defaultFontItem"],
                items["resetFontItem"]
            });
            items["optionItem"].Name = "optionItem";
            items["optionItem"].Text = "Option";
            //
            // consoleItem
            //
            items.Add("consoleItem", new System.Windows.Forms.ToolStripMenuItem());
            items["consoleItem"].Name = "consoleItem";
            items["consoleItem"].Text = "Debug Console";
            items["consoleItem"].Visible = false;
            items["consoleItem"].Click += new System.EventHandler(this.consoleItem_Click);
            //
            // visitWebsiteItem
            //
            items.Add("visitWebsiteItem", new System.Windows.Forms.ToolStripMenuItem());
            items["visitWebsiteItem"].Name = "visitWebsiteItem";
            items["visitWebsiteItem"].Text = "Visit homesite";
            items["visitWebsiteItem"].Click += new System.EventHandler(this.visitWebsiteItem_Click);
            //
            // releaseNoteItem
            //
            items.Add("releaseNoteItem", new System.Windows.Forms.ToolStripMenuItem());
            items["releaseNoteItem"].Name = "releaseNoteItem";
            items["releaseNoteItem"].Text = "Release Note";
            items["releaseNoteItem"].Click += new System.EventHandler(this.releaseNoteItem_Click);
            //
            // aboutItem
            //
            items.Add("aboutItem", new System.Windows.Forms.ToolStripMenuItem());
            items["aboutItem"].Name = "aboutItem";
            items["aboutItem"].Text = "About";
            items["aboutItem"].Click += new System.EventHandler(this.aboutItem_Click);
            //
            // helpItem
            //
            items.Add("helpItem", new System.Windows.Forms.ToolStripMenuItem());
            items["helpItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["consoleItem"],
                items["visitWebsiteItem"],
                items["releaseNoteItem"],
                items["aboutItem"]
            });
            items["helpItem"].Name = "helpItem";
            items["helpItem"].Text = "Help";
            //
            // PopupMenu
            //
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["editItem"],
                items["colorItem"],
                items["removeShortcutItem"],
                items["linkItem"],
                items["alignItem"],
                separators["quickActionSeparator"],
                items["fileItem"],
                items["editMenuItem"],
                items["nodeItem"],
                items["lineItem"],
                items["imageItem"],
                items["attachmentItem"],
                items["viewItem"],
                items["layerItem"],
                separators["helpSeparator"],
                items["toolsItem"],
                items["optionItem"],
                items["helpItem"]
			});
            this.Name = "popupMenu";
            this.Opening += new System.ComponentModel.CancelEventHandler(this.PopupMenu_Opening);
            //
            // Popup
            //
            this.ResumeLayout(false);
        }

        /*************************************************************************************************************************/

        // MENU Manage                                                                                // POPUP MENU
        public void PopupMenu_Opening(object sender, CancelEventArgs e)
        {
            bool readOnly = this.diagramView.diagram.isReadOnly();

            items["editItem"].Visible = !readOnly;
            items["colorItem"].Visible = !readOnly;
            items["linkItem"].Visible = !readOnly;
            items["openlinkItem"].Enabled = !readOnly;
            separators["quickActionSeparator"].Visible = !readOnly;
            items["alignItem"].Visible = !readOnly;
            items["removeShortcutItem"].Visible = !readOnly;
            items["openLinkDirectoryItem"].Visible = !readOnly;
            items["copyItem"].Enabled = !readOnly;
            items["cutItem"].Enabled = !readOnly;
            items["pasteItem"].Enabled = !readOnly;
            items["pasteToLinkItem"].Enabled = !readOnly;
            items["pasteToNoteItem"].Enabled = !readOnly;
            items["setStartPositionItem"].Enabled = !readOnly;
            items["copyLinkItem"].Enabled = !readOnly;
            items["copyNoteItem"].Enabled = !readOnly;
            items["encryptItem"].Enabled = !readOnly;
            items["changePasswordItem"].Enabled = !readOnly;
            items["defaultFontItem"].Enabled = !readOnly;
            items["resetFontItem"].Enabled = !readOnly;
            items["restoreWindowItem"].Enabled = !readOnly;
            items["gridItem"].Enabled = !readOnly;
            items["coordinatesItem"].Enabled = !readOnly;
            items["bordersItem"].Enabled = !readOnly;
            items["transparentItem"].Checked = !readOnly;
            items["transparentItem"].Enabled = !readOnly;
            items["imageAddItem"].Enabled = !readOnly;
            items["imageRemoveItem"].Enabled = !readOnly;
            items["imageEmbeddedItem"].Enabled = !readOnly;
            items["fontItem"].Enabled = !readOnly;
            items["fontColorItem"].Enabled = !readOnly;
            items["editLinkItem"].Enabled = !readOnly;
            items["bringTopItem"].Enabled = !readOnly;
            items["bringBottomItem"].Enabled = !readOnly;
            items["lineColorItem"].Enabled = !readOnly;
            items["includeFileItem"].Enabled = !readOnly;
            items["includeDirectoryItem"].Enabled = !readOnly;
            items["removeAttachmentItem"].Enabled = !readOnly;
            items["protectItem"].Enabled = !readOnly;

            // NEW FILE
            if (this.diagramView.diagram.isNew())
            {
                items["openDiagramDirectoryItem"].Enabled = false;
            }
            else
            {
                items["openDiagramDirectoryItem"].Enabled = true;
            }

            if (readOnly)
            {
                return;
            }

            items["imageAddItem"].Enabled = true;

            // SELECTION EMPTY
            if (this.diagramView.selectedNodes.Count() == 0)
            {
                items["editItem"].Visible = false;
                items["colorItem"].Visible = false;
                items["linkItem"].Visible = false;
                items["openlinkItem"].Enabled = false;
                separators["quickActionSeparator"].Visible = false;//separator
                items["alignItem"].Visible = false;
                items["removeShortcutItem"].Visible = false;
                items["openLinkDirectoryItem"].Visible = false;
                items["copyItem"].Enabled = false;
                items["cutItem"].Enabled = false;
                items["copyLinkItem"].Enabled = false;
                items["copyNoteItem"].Enabled = false;
                items["transparentItem"].Checked = false;
                items["transparentItem"].Enabled = false;
                items["imageRemoveItem"].Enabled = false;
                items["imageEmbeddedItem"].Enabled = false;
                items["fontItem"].Enabled = false;
                items["fontColorItem"].Enabled = false;
                items["editLinkItem"].Enabled = false;
                items["bringTopItem"].Enabled = false;
                items["bringBottomItem"].Enabled = false;
                items["lineColorItem"].Enabled = false;
                items["protectItem"].Enabled = false;
            }

            // SELECTION NOT EMPTY
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                items["editItem"].Visible = true;
                items["colorItem"].Visible = true;
                separators["quickActionSeparator"].Visible = true;//separator
                items["copyItem"].Enabled = true;
                items["cutItem"].Enabled = true;
                items["copyLinkItem"].Enabled = true;
                items["copyNoteItem"].Enabled = true;
                items["transparentItem"].Checked = this.diagramView.isSelectionTransparent();
                items["transparentItem"].Enabled = true;
                items["imageAddItem"].Enabled = true;
                items["imageRemoveItem"].Enabled = this.diagramView.hasSelectionImage();
                items["imageEmbeddedItem"].Enabled = this.diagramView.hasSelectionNotEmbeddedImage();
                items["fontItem"].Enabled = true;
                items["fontColorItem"].Enabled = true;
                items["editLinkItem"].Enabled = false;
                items["bringTopItem"].Enabled = true;
                items["bringBottomItem"].Enabled = true;
                items["protectItem"].Enabled = true;
            }

            // SELECTION ONE
            if (this.diagramView.selectedNodes.Count() == 1)
            {
                items["linkItem"].Visible = this.diagramView.selectedNodes[0].link.Trim() != "";
                items["copyLinkItem"].Enabled = this.diagramView.selectedNodes[0].link.Trim() != "";
                items["openlinkItem"].Enabled = this.diagramView.selectedNodes[0].link.Trim() != "";
                items["alignItem"].Visible = false;
                items["openLinkDirectoryItem"].Visible = false;
                if (this.diagramView.selectedNodes[0].link.Trim().Length > 0 && Os.FileExists(this.diagramView.selectedNodes[0].link))
                    items["openLinkDirectoryItem"].Visible = true;
                items["editLinkItem"].Enabled = true;
                items["lineColorItem"].Enabled = false;
            }

            // SELECTION MORE THEN ONE
            if (this.diagramView.selectedNodes.Count() > 1)
            {
                items["linkItem"].Visible = false;
                items["openlinkItem"].Enabled = false;
                items["alignItem"].Visible = true;
                items["removeShortcutItem"].Visible = false;
                items["openLinkDirectoryItem"].Visible = false;
                items["lineColorItem"].Enabled = true;
            }

            // REMOVE SHORTCUT
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                bool hasShortcut = false;
                foreach (Node node in this.diagramView.selectedNodes)
                {
                    if (node.shortcut > 0)
                    {
                        hasShortcut = true;
                        break;
                    }
                }

                items["removeShortcutItem"].Visible = false;
                if (hasShortcut)
                {
                    items["removeShortcutItem"].Visible = true;
                }
            }

            // PASSWORD IS SET
            if (this.diagramView.diagram.password == "")
            {
                items["changePasswordItem"].Visible = false;
                items["encryptItem"].Visible = true;
            }
            else
            {
                items["changePasswordItem"].Visible = true;
                items["encryptItem"].Visible = false;
            }

            // ATTACHMENT
            if (this.diagramView.hasSelectionAttachment())
            {
                items["deploayAttachmentItem"].Enabled = true;
                items["removeAttachmentItem"].Enabled = true;
            }
            else
            {
                items["deploayAttachmentItem"].Enabled = false;
                items["removeAttachmentItem"].Enabled = false;
            }

            // CLIPBOARD
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();
            if (retrievedData.GetDataPresent("DiagramXml")
            || retrievedData.GetDataPresent(DataFormats.Text)
            || Clipboard.ContainsFileDropList()
            || Clipboard.GetDataObject() != null)
            {
                items["pasteItem"].Text = "Paste";
                items["pasteItem"].Enabled = true;

                if (retrievedData.GetDataPresent("DiagramXml"))
                {
                    items["pasteItem"].Text += " diagram";
                }
                else
                if (retrievedData.GetDataPresent(DataFormats.Text))
                {
                    items["pasteItem"].Text += " text";
                }
                else
                if (Clipboard.ContainsFileDropList())
                {
                    items["pasteItem"].Text += " files";
                }
                else
                if (Clipboard.GetDataObject() != null)
                {
                    IDataObject data = Clipboard.GetDataObject();
                    if (data.GetDataPresent(DataFormats.Bitmap))
                    {
                        items["pasteItem"].Text += " image";
                    }
                }
            }
            else
            {
                items["pasteItem"].Enabled = false;
            }
        }

        // QUICK ACTIONS

        // MENU Edit
        public void editItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                foreach (Node node in this.diagramView.selectedNodes)
                {
                    this.diagramView.diagram.EditNode(node);
                }
            }
        }

        // MENU Change color
        public void colorItem_Click(object sender, EventArgs e)
        {
            this.diagramView.selectColor();
        }

        // MENU remove shortcut
        private void removeShortcutItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.removeShortcuts(this.diagramView.selectedNodes);
            }
        }

        // LINK

        // MENU Link Open
        public void openlinkItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.OpenLinkAsync(this.diagramView.selectedNodes[0]);
            }
        }

        // MENU open directory for file in link
        private void openLinkDirectoryItem_Click(object sender, EventArgs e)
        {
            this.diagramView.openLinkDirectory();
        }

        // ALIGN

        // MENU align right
        private void rightItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignRight(this.diagramView.selectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to line
        private void toLineItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignToLine(this.diagramView.selectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to column
        private void inColumnItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignToColumn(this.diagramView.selectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to group to column
        private void groupVericalItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignCompact(this.diagramView.selectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to group to column
        private void groupHorizontalItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignCompactLine(this.diagramView.selectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align left
        private void leftItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignLeft(this.diagramView.selectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // FILE

        // MENU New
        public void newItem_Click(object sender, EventArgs e)
        {
            this.diagramView.main.OpenDiagram();
        }

        // MENU Save
        public void saveItem_Click(object sender, EventArgs e)
        {
            this.diagramView.save();
        }

        // MENU Save As
        public void saveAsItem_Click(object sender, EventArgs e)
        {
            this.diagramView.saveas();
        }

        // MENU export to txt
        private void textItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.saveTextFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.diagramView.exportDiagramToTxt(this.diagramView.saveTextFileDialog.FileName);
            }
        }

        // MENU export to png
        private void exportToPngItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.exportFile.ShowDialog() == DialogResult.OK)
            {
                this.diagramView.exportDiagramToPng();
            }
        }

        // MENU Open
        public void openItem_Click(object sender, EventArgs e)
        {
            this.diagramView.open();
        }

        // MENU Exit
        public void exitItem_Click(object sender, EventArgs e)
        {
            this.diagramView.Close();
        }

        // EDIT

        // MENU Copy
        public void copyItem_Click(object sender, EventArgs e)
        {
            this.diagramView.copy();
        }

        // MENU cut
        public void cutItem_Click(object sender, EventArgs e)
        {
            this.diagramView.cut();
        }

        // MENU paste
        public void pasteItem_Click(object sender, EventArgs e)
        {
            this.diagramView.paste(new Position(this.diagramView.startMousePos));
        }

        // MENU Copy link
        public void copyLinkItem_Click(object sender, EventArgs e)
        {
            this.diagramView.copyLink();
        }

        // MENU Copy note
        public void copyNoteItem_Click(object sender, EventArgs e)
        {
            this.diagramView.copyNote();
        }

        // MENU Copy link
        public void pasteToLinkItem_Click(object sender, EventArgs e)
        {
            this.diagramView.pasteToLink();
        }

        // MENU Copy note
        public void pasteToNoteItem_Click(object sender, EventArgs e)
        {
            this.diagramView.pasteToNote();
        }

        // NODE

        // MENU NODE transparent
        private void transparentItem_Click(object sender, EventArgs e)
        {
            this.diagramView.makeSelectionTransparent();
        }

        // MENU NODE set font
        private void fontItem_Click(object sender, EventArgs e)
        {
            this.diagramView.selectFont();
        }

        // MENU NODE set font color
        private void fontColorItem_Click(object sender, EventArgs e)
        {
            this.diagramView.selectFontColor();
        }

        // MENU NODE edit node link
        private void editLinkItem_Click(object sender, EventArgs e)
        {
            this.diagramView.editLink();
        }

        // MENU NODE edit node link
        private void bringTopItem_Click(object sender, EventArgs e)
        {
            this.diagramView.moveNodesToForeground();
        }

        // MENU NODE edit node link
        private void bringBottomItem_Click(object sender, EventArgs e)
        {
            this.diagramView.moveNodesToBackground();
        }

        // MENU NODE protect sesitive data in node name
        private void protectItem_Click(object sender, EventArgs e)
        {
            this.diagramView.protectNodes();
        }

        // MENU LINE select line color
        private void lineColorItem_Click(object sender, EventArgs e)
        {
            this.diagramView.changeLineColor();
        }

        // MENU LINE select line color
        private void lineWidthItem_Click(object sender, EventArgs e)
        {
            this.diagramView.changeLineWidth();
        }

        // ATTACHMENT

        // MENU NODE add image
        private void imageAddItem_Click(object sender, EventArgs e)
        {
            this.diagramView.addImage();
        }

        // MENU NODE image remove from diagram
        private void imageRemoveItem_Click(object sender, EventArgs e)
        {
            this.diagramView.removeImagesFromSelection();
        }

        // MENU NODE image embedded to diagram
        private void imageEmbeddedItem_Click(object sender, EventArgs e)
        {
            this.diagramView.makeImagesEmbedded();
        }

        // MENU NODE deploy attachment to system
        private void deploayAttachmentItem_Click(object sender, EventArgs e)
        {
            this.diagramView.attachmentDeploy();
        }

        // MENU NODE add file attachment to diagram
        private void includeFileItem_Click(object sender, EventArgs e)
        {
            this.diagramView.attachmentAddFile(new Position(this.diagramView.startMousePos));
        }

        // MENU NODE add directory attachment to diagram
        private void includeDirectoryItem_Click(object sender, EventArgs e)
        {
            this.diagramView.attachmentAddDirectory(new Position(this.diagramView.startMousePos));
        }

        // MENU NODE remove included data
        private void removeFileItem_Click(object sender, EventArgs e)
        {
            this.diagramView.attachmentRemove();
        }

        // VIEW

        // MENU VIEW NEW VIEW
        private void newViewItem_Click(object sender, EventArgs e)
        {
            // otvorenie novej insancie DiagramView
            this.diagramView.diagram.openDiagramView();
        }

        // MENU Center
        public void centerItem_Click(object sender, EventArgs e)
        {
            this.diagramView.GoToHome();
        }

        // MENU set home position
        private void setStartPositionItem_Click(object sender, EventArgs e)
        {
            this.diagramView.setCurentPositionAsHomePosition();
        }

        // LAYER

        // MENU Layer In
        public void inItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() == 1)
            {
                this.diagramView.LayerIn(this.diagramView.selectedNodes[0]);
            }
        }

        // MENU Layer Out
        public void outItem_Click(object sender, EventArgs e)
        {
            this.diagramView.LayerOut();
        }

        // TOOLS

        // MENU Open Directory  - otvory adresar v ktorom sa nachadza prave otvreny subor
        public void openDiagramDirectoryItem_Click(object sender, EventArgs e)
        {
            this.diagramView.openDiagramDirectory();
        }

        // OPTIONS

        // MENU Encription
        private void encryptItem_Click(object sender, EventArgs e)
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
        private void changePasswordItem_Click(object sender, EventArgs e)
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

        // MENU Read only
        public void readonlyItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.readOnly = items["readonlyItem"].Checked;
        }

        // MENU restore window position
        public void restoreWindowItem_Click(object sender, EventArgs e)
        {
            this.diagramView.rememberPosition(items["restoreWindowItem"].Checked);
        }

        // MENU Grid check
        public void gridItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.grid = items["gridItem"].Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU coordinates
        public void coordinatesItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.coordinates = items["coordinatesItem"].Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Option Borders
        public void bordersItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.borders = items["bordersItem"].Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Option Default font
        public void defaultFontItem_Click(object sender, EventArgs e)
        {
            this.diagramView.selectDefaultFont();
        }

        // MENU reset font
        private void resetFontItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.ResetFont(this.diagramView.selectedNodes);
            }
            else
            {
                this.diagramView.diagram.ResetFont();
            }
        }

        // HELP

        // MENU Console
        public void consoleItem_Click(object sender, EventArgs e)
        {
            this.diagramView.showConsole();
        }

        // MENU visit homepage
        private void visitWebsiteItem_Click(object sender, EventArgs e)
        {
            Network.openUrl(this.diagramView.main.options.home_page);
        }

        // MENU Show release note
        private void releaseNoteItem_Click(object sender, EventArgs e)
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

        // MENU show About form
        private void aboutItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.main.aboutForm == null)
            {
                this.diagramView.main.aboutForm = new AboutForm(this.diagramView.main);
            }

            this.diagramView.main.aboutForm.Show();
        }
    }
}
