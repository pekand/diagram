using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Diagram
{
    [System.ComponentModel.DesignerCategory("Code")]

    public class Popup : ContextMenuStrip //UID5085107645
    {
        public DiagramView diagramView = null;       // diagram ktory je previazany z pohladom

        private Dictionary<string, ToolStripMenuItem> items = new Dictionary<string, ToolStripMenuItem>();
        private Dictionary<string, ToolStripSeparator> separators = new Dictionary<string, ToolStripSeparator>();

        public ToolStripItem[] recentItems = null;
        private System.Windows.Forms.ToolStripMenuItem pluginItems = null;

        public Popup(System.ComponentModel.IContainer container, DiagramView diagramView) : base(container) //UID1752805239
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


        public ToolStripMenuItem GetPluginsItem()
        {
            return this.pluginItems;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() //UID7904769190
        {
            this.SuspendLayout();
          
            //
            // editItem
            //
            items.Add("editItem", new System.Windows.Forms.ToolStripMenuItem());
            items["editItem"].Name = "editItem";
            items["editItem"].Text = "Edit";
            items["editItem"].Click += new System.EventHandler(this.EditItem_Click);
            //
            // colorItem
            //
            items.Add("colorItem", new System.Windows.Forms.ToolStripMenuItem());
            items["colorItem"].Name = "colorItem";
            items["colorItem"].Text = "Color";
            items["colorItem"].Click += new System.EventHandler(this.ColorItem_Click);
            //
            // openlinkItem
            //
            items.Add("openlinkItem", new System.Windows.Forms.ToolStripMenuItem());
            items["openlinkItem"].Name = "openlinkItem";
            items["openlinkItem"].Text = "Open";
            items["openlinkItem"].Click += new System.EventHandler(this.OpenlinkItem_Click);
            //
            // openLinkDirectoryItem
            //
            items.Add("openLinkDirectoryItem", new System.Windows.Forms.ToolStripMenuItem());
            items["openLinkDirectoryItem"].Name = "openLinkDirectoryItem";
            items["openLinkDirectoryItem"].Text = "Open directory";
            items["openLinkDirectoryItem"].Click += new System.EventHandler(this.OpenLinkDirectoryItem_Click);
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
            items["leftItem"].Click += new System.EventHandler(this.LeftItem_Click);
            //
            // rightItem
            //
            items.Add("rightItem", new System.Windows.Forms.ToolStripMenuItem());
            items["rightItem"].Name = "rightItem";
            items["rightItem"].Text = "Right";
            items["rightItem"].Click += new System.EventHandler(this.RightItem_Click);
            //
            // toLineItem
            //
            items.Add("toLineItem", new System.Windows.Forms.ToolStripMenuItem());
            items["toLineItem"].Name = "toLineItem";
            items["toLineItem"].Text = "To line";
            items["toLineItem"].Click += new System.EventHandler(this.ToLineItem_Click);
            //
            // inColumnItem
            //
            items.Add("inColumnItem", new System.Windows.Forms.ToolStripMenuItem());
            items["inColumnItem"].Name = "inColumnItem";
            items["inColumnItem"].Text = "In column";
            items["inColumnItem"].Click += new System.EventHandler(this.InColumnItem_Click);
            //
            // groupVericalItem
            //
            items.Add("groupVericalItem", new System.Windows.Forms.ToolStripMenuItem());
            items["groupVericalItem"].Name = "groupVericalItem";
            items["groupVericalItem"].Text = "Group vertical";
            items["groupVericalItem"].Click += new System.EventHandler(this.GroupVericalItem_Click);
            //
            // groupHorizontalItem
            //
            items.Add("groupHorizontalItem", new System.Windows.Forms.ToolStripMenuItem());
            items["groupHorizontalItem"].Name = "groupHorizontalItem";
            items["groupHorizontalItem"].Text = "Group horizontal";
            items["groupHorizontalItem"].Click += new System.EventHandler(this.GroupHorizontalItem_Click);
            //
            // sortItem
            //
            items.Add("sortItem", new System.Windows.Forms.ToolStripMenuItem());
            items["sortItem"].Name = "sortItem";
            items["sortItem"].Text = "Sort";
            items["sortItem"].Click += new System.EventHandler(this.SortItem_Click);
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
                items["groupHorizontalItem"],
                items["sortItem"]
            });
            items["alignItem"].Name = "alignItem";
            items["alignItem"].Text = "Align";
            //
            // quickActionSeparator
            //
            separators.Add("quickActionSeparator", new System.Windows.Forms.ToolStripSeparator());
            separators["quickActionSeparator"].Name = "quickActionSeparator";
            //
            // copyItem
            //
            items.Add("copyItem", new System.Windows.Forms.ToolStripMenuItem());
            items["copyItem"].Name = "copyItem";
            items["copyItem"].Text = "Copy";
            items["copyItem"].Click += new System.EventHandler(this.CopyItem_Click);
            //
            // cutItem
            //
            items.Add("cutItem", new System.Windows.Forms.ToolStripMenuItem());
            items["cutItem"].Name = "cutItem";
            items["cutItem"].Text = "Cut";
            items["cutItem"].Click += new System.EventHandler(this.CutItem_Click);
            //
            // pasteItem
            //
            items.Add("pasteItem", new System.Windows.Forms.ToolStripMenuItem());
            items["pasteItem"].Name = "pasteItem";
            items["pasteItem"].Text = "Paste";
            items["pasteItem"].Click += new System.EventHandler(this.PasteItem_Click);
            //
            // editSeparator
            //
            separators.Add("editSeparator", new System.Windows.Forms.ToolStripSeparator());
            separators["editSeparator"].Name = "editSeparator";
            //
            // newItem
            //
            items.Add("newItem", new System.Windows.Forms.ToolStripMenuItem());
            items["newItem"].Name = "newItem";
            items["newItem"].Text = "New";
            items["newItem"].Click += new System.EventHandler(this.NewItem_Click);
            //
            // saveItem
            //
            items.Add("saveItem", new System.Windows.Forms.ToolStripMenuItem());
            items["saveItem"].Name = "saveItem";
            items["saveItem"].Text = "Save";
            items["saveItem"].Click += new System.EventHandler(this.SaveItem_Click);
            //
            // saveAsItem
            //
            items.Add("saveAsItem", new System.Windows.Forms.ToolStripMenuItem());
            items["saveAsItem"].Name = "saveAsItem";
            items["saveAsItem"].Text = "Save As";
            items["saveAsItem"].Click += new System.EventHandler(this.SaveAsItem_Click);
            //
            // textItem
            //
            items.Add("textItem", new System.Windows.Forms.ToolStripMenuItem());
            items["textItem"].Name = "textItem";
            items["textItem"].Text = "Text";
            items["textItem"].Click += new System.EventHandler(this.TextItem_Click);
            //
            // exportToPngItem
            //
            items.Add("exportToPngItem", new System.Windows.Forms.ToolStripMenuItem());
            items["exportToPngItem"].Name = "exportToPngItem";
            items["exportToPngItem"].Text = "Export to png";
            items["exportToPngItem"].Click += new System.EventHandler(this.ExportToPngItem_Click);
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
            items["openItem"].Click += new System.EventHandler(this.OpenItem_Click);
            //
            // openItem
            //
            items.Add("recentItem", new System.Windows.Forms.ToolStripMenuItem());
            items["recentItem"].Name = "recentItem";
            items["recentItem"].Text = "Recent";
            //
            // exitItem
            //
            items.Add("exitItem", new System.Windows.Forms.ToolStripMenuItem());
            items["exitItem"].Name = "exitItem";
            items["exitItem"].Text = "Exit";
            items["exitItem"].Click += new System.EventHandler(this.ExitItem_Click);
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
                items["recentItem"],
                items["exitItem"]
            });
            items["fileItem"].Name = "fileItem";
            items["fileItem"].Text = "File";
            
            //
            // undoItem
            //
            items.Add("undoItem", new System.Windows.Forms.ToolStripMenuItem());
            items["undoItem"].Name = "undoItem";
            items["undoItem"].Text = "Undo";
            items["undoItem"].Click += new System.EventHandler(this.UndoItem_Click);
            //
            // redoItem
            //
            items.Add("redoItem", new System.Windows.Forms.ToolStripMenuItem());
            items["redoItem"].Name = "redoItem";
            items["redoItem"].Text = "Redo";
            items["redoItem"].Click += new System.EventHandler(this.RedoItem_Click);
            //
            // copyLinkItem
            //
            items.Add("copyLinkItem", new System.Windows.Forms.ToolStripMenuItem());
            items["copyLinkItem"].Name = "copyLinkItem";
            items["copyLinkItem"].Text = "Copy link";
            items["copyLinkItem"].Click += new System.EventHandler(this.CopyLinkItem_Click);
            //
            // copyNoteItem
            //
            items.Add("copyNoteItem", new System.Windows.Forms.ToolStripMenuItem());
            items["copyNoteItem"].Name = "copyNoteItem";
            items["copyNoteItem"].Text = "Copy note";
            items["copyNoteItem"].Click += new System.EventHandler(this.CopyNoteItem_Click);
            //
            // pasteToLinkItem
            //
            items.Add("pasteToLinkItem", new System.Windows.Forms.ToolStripMenuItem());
            items["pasteToLinkItem"].Name = "pasteToLinkItem";
            items["pasteToLinkItem"].Text = "Paste to link";
            items["pasteToLinkItem"].Click += new System.EventHandler(this.PasteToLinkItem_Click);
            //
            // pasteToNoteItem
            //
            items.Add("pasteToNoteItem", new System.Windows.Forms.ToolStripMenuItem());
            items["pasteToNoteItem"].Name = "pasteToNoteItem";
            items["pasteToNoteItem"].Text = "Paste to note";
            items["pasteToNoteItem"].Click += new System.EventHandler(this.PasteToNoteItem_Click);
            //
            // editMenuItem
            //
            items.Add("editMenuItem", new System.Windows.Forms.ToolStripMenuItem());
            items["editMenuItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["undoItem"],
                items["redoItem"],
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
            items["transparentItem"].Click += new System.EventHandler(this.TransparentItem_Click);
            //
            // fontItem
            //
            items.Add("fontItem", new System.Windows.Forms.ToolStripMenuItem());
            items["fontItem"].Name = "fontItem";
            items["fontItem"].Text = "Font";
            items["fontItem"].Click += new System.EventHandler(this.FontItem_Click);
            //
            // fontColorItem
            //
            items.Add("fontColorItem", new System.Windows.Forms.ToolStripMenuItem());
            items["fontColorItem"].Name = "fontColorItem";
            items["fontColorItem"].Text = "Font color";
            items["fontColorItem"].Click += new System.EventHandler(this.FontColorItem_Click);
            //
            // editLinkItem
            //
            items.Add("editLinkItem", new System.Windows.Forms.ToolStripMenuItem());
            items["editLinkItem"].Name = "editLinkItem";
            items["editLinkItem"].Text = "Edit link";
            items["editLinkItem"].Click += new System.EventHandler(this.EditLinkItem_Click);
            //
            // bringTopItem
            //
            items.Add("bringTopItem", new System.Windows.Forms.ToolStripMenuItem());
            items["bringTopItem"].Name = "bringTopItem";
            items["bringTopItem"].Text = "Bring to top";
            items["bringTopItem"].Click += new System.EventHandler(this.BringTopItem_Click);
            //
            // bringBottomItem
            //
            items.Add("bringBottomItem", new System.Windows.Forms.ToolStripMenuItem());
            items["bringBottomItem"].Name = "bringBottomItem";
            items["bringBottomItem"].Text = "Bring to bottom";
            items["bringBottomItem"].Click += new System.EventHandler(this.BringBottomItem_Click);
            //
            // removeShortcutItem
            //
            items.Add("removeShortcutItem", new System.Windows.Forms.ToolStripMenuItem());
            items["removeShortcutItem"].Name = "removeShortcutItem";
            items["removeShortcutItem"].Text = "Remove shortcut";
            items["removeShortcutItem"].Click += new System.EventHandler(this.RemoveShortcutItem_Click);
            //
            // protectItem
            //
            items.Add("protectItem", new System.Windows.Forms.ToolStripMenuItem());
            items["protectItem"].Name = "protectItem";
            items["protectItem"].Text = "Protect";
            items["protectItem"].Click += new System.EventHandler(this.ProtectItem_Click);
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
                items["removeShortcutItem"],
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
            items["lineColorItem"].Click += new System.EventHandler(this.LineColorItem_Click);
            //
            // lineWidthItem
            //
            items.Add("lineWidthItem", new System.Windows.Forms.ToolStripMenuItem());
            items["lineWidthItem"].Name = "lineWidthItem";
            items["lineWidthItem"].Text = "Width";
            items["lineWidthItem"].Click += new System.EventHandler(this.LineWidthItem_Click);
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
            items["imageAddItem"].Click += new System.EventHandler(this.ImageAddItem_Click);
            //
            // imageRemoveItem
            //
            items.Add("imageRemoveItem", new System.Windows.Forms.ToolStripMenuItem());
            items["imageRemoveItem"].Name = "imageRemoveItem";
            items["imageRemoveItem"].Text = "Remove image";
            items["imageRemoveItem"].Click += new System.EventHandler(this.ImageRemoveItem_Click);
            //
            // imageEmbeddedItem
            //
            items.Add("imageEmbeddedItem", new System.Windows.Forms.ToolStripMenuItem());
            items["imageEmbeddedItem"].Name = "imageEmbeddedItem";
            items["imageEmbeddedItem"].Text = "Embed image";
            items["imageEmbeddedItem"].Click += new System.EventHandler(this.ImageEmbeddedItem_Click);
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
            // polygonCreateItem
            //
            items.Add("polygonCreateItem", new System.Windows.Forms.ToolStripMenuItem());
            items["polygonCreateItem"].Name = "polygonCreateItem";
            items["polygonCreateItem"].Text = "Create polygon";
            items["polygonCreateItem"].Click += new System.EventHandler(this.PolygonCreateItem_Click);
            //
            // polygonRemoveItem
            //
            items.Add("polygonRemoveItem", new System.Windows.Forms.ToolStripMenuItem());
            items["polygonRemoveItem"].Name = "polygonRemoveItem";
            items["polygonRemoveItem"].Text = "Remove polygon";
            items["polygonRemoveItem"].Click += new System.EventHandler(this.PolygonRemoveItem_Click);
            //
            // imageItem
            //
            items.Add("polygonItem", new System.Windows.Forms.ToolStripMenuItem());
            items["polygonItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["polygonCreateItem"],
                items["polygonRemoveItem"]
            });
            items["polygonItem"].Name = "polygonItem";
            items["polygonItem"].Text = "Polygon";
            //
            // deploayAttachmentItem
            //
            items.Add("deploayAttachmentItem", new System.Windows.Forms.ToolStripMenuItem());
            items["deploayAttachmentItem"].Name = "deploayAttachmentItem";
            items["deploayAttachmentItem"].Text = "Deploy attachment";
            items["deploayAttachmentItem"].Click += new System.EventHandler(this.DeploayAttachmentItem_Click);
            //
            // includeFileItem
            //
            items.Add("includeFileItem", new System.Windows.Forms.ToolStripMenuItem());
            items["includeFileItem"].Name = "includeFileItem";
            items["includeFileItem"].Text = "Add file";
            items["includeFileItem"].Click += new System.EventHandler(this.IncludeFileItem_Click);
            //
            // includeDirectoryItem
            //
            items.Add("includeDirectoryItem", new System.Windows.Forms.ToolStripMenuItem());
            items["includeDirectoryItem"].Name = "includeDirectoryItem";
            items["includeDirectoryItem"].Text = "Add directory";
            items["includeDirectoryItem"].Click += new System.EventHandler(this.IncludeDirectoryItem_Click);
            //
            // removeAttachmentItem
            //
            items.Add("removeAttachmentItem", new System.Windows.Forms.ToolStripMenuItem());
            items["removeAttachmentItem"].Name = "removeAttachmentItem";
            items["removeAttachmentItem"].Text = "Remove";
            items["removeAttachmentItem"].Click += new System.EventHandler(this.RemoveFileItem_Click);
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
            items["newViewItem"].Click += new System.EventHandler(this.NewViewItem_Click);
            //
            // centerItem
            //
            items.Add("centerItem", new System.Windows.Forms.ToolStripMenuItem());
            items["centerItem"].Name = "centerItem";
            items["centerItem"].Text = "Center";
            items["centerItem"].Click += new System.EventHandler(this.CenterItem_Click);
            //
            // setStartPositionItem
            //
            items.Add("setStartPositionItem", new System.Windows.Forms.ToolStripMenuItem());
            items["setStartPositionItem"].Name = "setStartPositionItem";
            items["setStartPositionItem"].Text = "Set start position";
            items["setStartPositionItem"].Click += new System.EventHandler(this.SetStartPositionItem_Click);
            //
            // refreshItem
            //
            items.Add("refreshItem", new System.Windows.Forms.ToolStripMenuItem());
            items["refreshItem"].Name = "refreshItem";
            items["refreshItem"].Text = "Refresh";
            items["refreshItem"].Click += new System.EventHandler(this.RefreshItem_Click);
            //
            // viewItem
            //
            items.Add("viewItem", new System.Windows.Forms.ToolStripMenuItem());
            items["viewItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["newViewItem"],
                items["centerItem"],
                items["setStartPositionItem"],
                items["refreshItem"]
            });
            items["viewItem"].Name = "viewItem";
            items["viewItem"].Text = "View";
            //
            // inItem
            //
            items.Add("inItem", new System.Windows.Forms.ToolStripMenuItem());
            items["inItem"].Name = "inItem";
            items["inItem"].Text = "In";
            items["inItem"].Click += new System.EventHandler(this.InItem_Click);
            //
            // outItem
            //
            items.Add("outItem", new System.Windows.Forms.ToolStripMenuItem());
            items["outItem"].Name = "outItem";
            items["outItem"].Text = "Out";
            items["outItem"].Click += new System.EventHandler(this.OutItem_Click);
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
            items["openDiagramDirectoryItem"].Click += new System.EventHandler(this.OpenDiagramDirectoryItem_Click);
            //
            // openDiagramDirectoryItem
            //
            items.Add("splitNodeItem", new System.Windows.Forms.ToolStripMenuItem());
            items["splitNodeItem"].Name = "openDiagramDirectoryItem";
            items["splitNodeItem"].Text = "Split node";
            items["splitNodeItem"].Click += new System.EventHandler(this.SplitNodeItem_Click);

            //
            // toolsItem
            //
            this.pluginItems = new System.Windows.Forms.ToolStripMenuItem();
            items.Add("pluginsItem", this.pluginItems);
            items["pluginsItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            });
            items["pluginsItem"].Name = "pluginsItem";
            items["pluginsItem"].Text = "Plugins";

            //
            // toolsItem
            //
            items.Add("toolsItem", new System.Windows.Forms.ToolStripMenuItem());
            items["toolsItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["openDiagramDirectoryItem"],
                items["splitNodeItem"]
            });
            items["toolsItem"].Name = "toolsItem";
            items["toolsItem"].Text = "Tools";
            //
            // encryptItem
            //
            items.Add("encryptItem", new System.Windows.Forms.ToolStripMenuItem());
            items["encryptItem"].Name = "encryptItem";
            items["encryptItem"].Text = "Encrypt";
            items["encryptItem"].Click += new System.EventHandler(this.EncryptItem_Click);
            //
            // changePasswordItem
            //
            items.Add("changePasswordItem", new System.Windows.Forms.ToolStripMenuItem());
            items["changePasswordItem"].Name = "changePasswordItem";
            items["changePasswordItem"].Text = "Change password";
            items["changePasswordItem"].Click += new System.EventHandler(this.ChangePasswordItem_Click);
            //
            // readonlyItem
            //
            items.Add("readonlyItem", new System.Windows.Forms.ToolStripMenuItem());
            items["readonlyItem"].CheckOnClick = true;
            items["readonlyItem"].Name = "readonlyItem";
            items["readonlyItem"].Text = "Read only";
            items["readonlyItem"].Click += new System.EventHandler(this.ReadonlyItem_Click);
            //
            // restoreWindowItem
            //
            items.Add("restoreWindowItem", new System.Windows.Forms.ToolStripMenuItem());
            items["restoreWindowItem"].Checked = true;
            items["restoreWindowItem"].CheckOnClick = true;
            items["restoreWindowItem"].CheckState = System.Windows.Forms.CheckState.Checked;
            items["restoreWindowItem"].Name = "restoreWindowItem";
            items["restoreWindowItem"].Text = "Restore window";
            items["restoreWindowItem"].Click += new System.EventHandler(this.RestoreWindowItem_Click);
            //
            // gridItem
            //
            items.Add("gridItem", new System.Windows.Forms.ToolStripMenuItem());
            items["gridItem"].Checked = true;
            items["gridItem"].CheckOnClick = true;
            items["gridItem"].CheckState = System.Windows.Forms.CheckState.Checked;
            items["gridItem"].Name = "gridItem";
            items["gridItem"].Text = "Grid";
            items["gridItem"].Click += new System.EventHandler(this.GridItem_Click);
            //
            // coordinatesItem
            //
            items.Add("coordinatesItem", new System.Windows.Forms.ToolStripMenuItem());
            items["coordinatesItem"].CheckOnClick = true;
            items["coordinatesItem"].Name = "coordinatesItem";
            items["coordinatesItem"].Text = "Coordinates";
            items["coordinatesItem"].Visible = false;
            items["coordinatesItem"].Click += new System.EventHandler(this.CoordinatesItem_Click);
            //
            // bordersItem
            //
            items.Add("bordersItem", new System.Windows.Forms.ToolStripMenuItem());
            items["bordersItem"].CheckOnClick = true;
            items["bordersItem"].Name = "bordersItem";
            items["bordersItem"].Text = "Borders";
            items["bordersItem"].Click += new System.EventHandler(this.BordersItem_Click);
            //
            // defaultFontItem
            //
            items.Add("defaultFontItem", new System.Windows.Forms.ToolStripMenuItem());
            items["defaultFontItem"].Name = "defaultFontItem";
            items["defaultFontItem"].Text = "Default font";
            items["defaultFontItem"].Click += new System.EventHandler(this.DefaultFontItem_Click);
            //
            // resetFontItem
            //
            items.Add("resetFontItem", new System.Windows.Forms.ToolStripMenuItem());
            items["resetFontItem"].Name = "resetFontItem";
            items["resetFontItem"].Text = "Reset font";
            items["resetFontItem"].Click += new System.EventHandler(this.ResetFontItem_Click);
            //
            // setIconItem
            //
            items.Add("setIconItem", new System.Windows.Forms.ToolStripMenuItem());
            items["setIconItem"].Name = "setIconItem";
            items["setIconItem"].Text = "Set icon";
            items["setIconItem"].Click += new System.EventHandler(this.SetIconItem_Click);
            //
            // setBackgroundItem
            //
            items.Add("setBackgroundItem", new System.Windows.Forms.ToolStripMenuItem());
            items["setBackgroundItem"].Name = "setBackgroundItem";
            items["setBackgroundItem"].Text = "Set background image";
            items["setBackgroundItem"].Click += new System.EventHandler(this.SetBackgroundItem_Click);
            //
            // openLayerInNewViewItem
            //
            items.Add("openLayerInNewViewItem", new System.Windows.Forms.ToolStripMenuItem());
            items["openLayerInNewViewItem"].Name = "openLayerInNewViewItem";
            items["openLayerInNewViewItem"].Text = "Open layer in new view";
            items["openLayerInNewViewItem"].Click += new System.EventHandler(this.OpenLayerInNewViewItem_Click);
            //
            // openConfigDirItem
            //
            items.Add("openConfigDirItem", new System.Windows.Forms.ToolStripMenuItem());
            items["openConfigDirItem"].Name = "openConfigDirItem";
            items["openConfigDirItem"].Text = "Open config dir";
            items["openConfigDirItem"].Click += new System.EventHandler(this.OpenConfigDirItem_Click);
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
                items["resetFontItem"],
                items["setIconItem"],
                items["setBackgroundItem"],
                items["openLayerInNewViewItem"],
                items["openConfigDirItem"]
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
            items["consoleItem"].Click += new System.EventHandler(this.ConsoleItem_Click);
            //
            // visitWebsiteItem
            //
            items.Add("visitWebsiteItem", new System.Windows.Forms.ToolStripMenuItem());
            items["visitWebsiteItem"].Name = "visitWebsiteItem";
            items["visitWebsiteItem"].Text = "Visit homesite";
            items["visitWebsiteItem"].Click += new System.EventHandler(this.VisitWebsiteItem_Click);
            //
            // aboutItem
            //
            items.Add("aboutItem", new System.Windows.Forms.ToolStripMenuItem());
            items["aboutItem"].Name = "aboutItem";
            items["aboutItem"].Text = "About";
            items["aboutItem"].Click += new System.EventHandler(this.AboutItem_Click);
            //
            // helpItem
            //
            items.Add("helpItem", new System.Windows.Forms.ToolStripMenuItem());
            items["helpItem"].DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                items["consoleItem"],
                items["visitWebsiteItem"],
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
                items["linkItem"],
                items["alignItem"],
                separators["quickActionSeparator"],
                items["copyItem"],
                items["cutItem"],
                items["pasteItem"],
                separators["editSeparator"],
                items["fileItem"],
                items["editMenuItem"],
                items["nodeItem"],
                items["lineItem"],
                items["imageItem"],
                items["polygonItem"],
                items["attachmentItem"],
                items["viewItem"],
                items["layerItem"],
                separators["helpSeparator"],
                items["pluginsItem"],
                items["toolsItem"],
                items["optionItem"],
                items["helpItem"]
			});
            this.Name = "popupMenu";
            this.Opening += new System.ComponentModel.CancelEventHandler(this.PopupMenu_Opening);

            // add plugins to popup
            this.diagramView.main.plugins.PopupAddItemsAction(diagramView, this);


            //
            // Popup
            //
            this.ResumeLayout(false);
        }

        /*************************************************************************************************************************/

        // MENU Manage                                                                                // POPUP MENU After open
        public void PopupMenu_Opening(object sender, CancelEventArgs e) //UID0017225057
        {
            bool readOnly = this.diagramView.diagram.IsReadOnly();

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
            items["undoItem"].Enabled = !readOnly;
            items["redoItem"].Enabled = !readOnly;
            items["copyNoteItem"].Enabled = !readOnly;
            items["splitNodeItem"].Enabled = !readOnly;
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
            items["openLayerInNewViewItem"].Checked = this.diagramView.diagram.options.openLayerInNewView;

            // NEW FILE
            if (this.diagramView.diagram.IsNew())
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
                items["splitNodeItem"].Enabled = false;
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
                items["transparentItem"].Checked = this.diagramView.IsSelectionTransparent();
                items["transparentItem"].Enabled = true;
                items["imageAddItem"].Enabled = true;
                items["imageRemoveItem"].Enabled = this.diagramView.HasSelectionImage();
                items["imageEmbeddedItem"].Enabled = this.diagramView.HasSelectionNotEmbeddedImage();
                items["fontItem"].Enabled = true;
                items["fontColorItem"].Enabled = true;
                items["editLinkItem"].Enabled = false;
                items["bringTopItem"].Enabled = true;
                items["bringBottomItem"].Enabled = true;
                items["protectItem"].Enabled = true;
                items["splitNodeItem"].Enabled = true;
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

            // UNDO REDO
            items["undoItem"].Enabled = this.diagramView.diagram.undoOperations.canUndo();
            items["redoItem"].Enabled = this.diagramView.diagram.undoOperations.canRedo();

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
            if (this.diagramView.diagram.IsEncrypted())
            {
                items["changePasswordItem"].Visible = true;
                items["encryptItem"].Visible = false;
            }
            else
            {
                items["changePasswordItem"].Visible = false;
                items["encryptItem"].Visible = true;
            }

            // ATTACHMENT
            if (this.diagramView.HasSelectionAttachment())
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

            // add recent files
            if (recentItems != null && recentItems.Count() > 0) {
                items["recentItem"].DropDownItems.Clear();
            }

            recentItems = new System.Windows.Forms.ToolStripItem[this.diagramView.main.options.recentFiles.Count()];
            long i = 0;
            foreach (String path in this.diagramView.main.options.recentFiles)
            {
                recentItems[i] = new ToolStripMenuItem
                {
                    Name = "recent" + i + "Item",
                    Text = Os.GetFileNameWithoutExtension(path),
                    Tag = path,
                };
                recentItems[i].Click += new System.EventHandler(this.RecentItem_Click);
                i++;
            }

            items["recentItem"].DropDownItems.AddRange(recentItems);
            
            this.diagramView.main.plugins.PopupOpenAction(diagramView, this);
        }

        // QUICK ACTIONS

        // MENU Edit
        public void EditItem_Click(object sender, EventArgs e) //UID7223285114
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
        public void ColorItem_Click(object sender, EventArgs e) //UID4476972922
        {
            this.diagramView.SelectColor();
        }

        // LINK

        // MENU Link Open
        public void OpenlinkItem_Click(object sender, EventArgs e) //UID8259578882
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.OpenLinkAsync(this.diagramView.selectedNodes[0]);
            }
        }

        // MENU open directory for file in link
        private void OpenLinkDirectoryItem_Click(object sender, EventArgs e) //UID9061148695
        {
            this.diagramView.OpenLinkDirectory();
        }

        // ALIGN

        // MENU align left
        private void LeftItem_Click(object sender, EventArgs e) //UID3723301860
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignLeft(this.diagramView.selectedNodes);
                this.diagramView.diagram.Unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }
        
        // MENU align right
        private void RightItem_Click(object sender, EventArgs e) //UID3530533003
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignRight(this.diagramView.selectedNodes);
                this.diagramView.diagram.Unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to line
        private void ToLineItem_Click(object sender, EventArgs e) //UID3603051682
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignToLine(this.diagramView.selectedNodes);
                this.diagramView.diagram.Unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to column
        private void InColumnItem_Click(object sender, EventArgs e) //UID6421466918
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignToColumn(this.diagramView.selectedNodes);
                this.diagramView.diagram.Unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to group to column
        private void GroupVericalItem_Click(object sender, EventArgs e) //UID5565272429
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignCompact(this.diagramView.selectedNodes);
                this.diagramView.diagram.Unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to group to column
        private void GroupHorizontalItem_Click(object sender, EventArgs e) //UID4865517556
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignCompactLine(this.diagramView.selectedNodes);
                this.diagramView.diagram.Unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // FILE
       
        // MENU export to txt
        private void TextItem_Click(object sender, EventArgs e) //UID6650610161
        {
            if (this.diagramView.saveTextFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.diagramView.ExportDiagramToTxt(this.diagramView.saveTextFileDialog.FileName);
            }
        }

        // MENU export to png
        private void ExportToPngItem_Click(object sender, EventArgs e) //UID4836284357
        {
            if (this.diagramView.exportFile.ShowDialog() == DialogResult.OK)
            {
                this.diagramView.ExportDiagramToPng();
            }
        }

        // MENU Open
        public void RecentItem_Click(object sender, EventArgs e) //UID3280882177
        {
            String path = (string)((ToolStripMenuItem)sender).Tag;
            this.diagramView.OpenDiagramFromFile(path);
        }

        // MENU sort items
        private void SortItem_Click(object sender, EventArgs e) //UID8063135807
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.SortNodes(this.diagramView.selectedNodes);
                this.diagramView.diagram.Unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }
        
        // EDIT

        // MENU Copy
        public void CopyItem_Click(object sender, EventArgs e) //UID5971111226
        {
            this.diagramView.Copy();
        }

        // MENU cut
        public void CutItem_Click(object sender, EventArgs e) //UID3013972101
        {
            this.diagramView.Cut();
        }

        // MENU paste
        public void PasteItem_Click(object sender, EventArgs e) //UID0613534707
        {
            this.diagramView.Paste(new Position(this.diagramView.startMousePos));
        }
                
        // MENU New
        public void NewItem_Click(object sender, EventArgs e) //UID8052745540
        {
            this.diagramView.main.OpenDiagram();
        }
        
        // MENU Save
        public void SaveItem_Click(object sender, EventArgs e) //UID9649741776
        {
            this.diagramView.Save();
        }
        
        // MENU Save As
        public void SaveAsItem_Click(object sender, EventArgs e) //UID3588710029
        {
            this.diagramView.Saveas();
        }
        
        // MENU Open
        public void OpenItem_Click(object sender, EventArgs e) //UID1150105418
        {
            this.diagramView.OpenFileDialog();
        }
        
        // MENU Exit
        public void ExitItem_Click(object sender, EventArgs e) //UID6941677987
        {
            this.diagramView.Close();
        }

        // MENU Undo
        public void UndoItem_Click(object sender, EventArgs e) //UID3923079972
        {
            this.diagramView.diagram.DoUndo(this.diagramView);
        }

        // MENU Redo
        public void RedoItem_Click(object sender, EventArgs e) //UID5586948047
        {
            this.diagramView.diagram.DoRedo(this.diagramView);
        }
        
        // MENU Copy link
        public void CopyLinkItem_Click(object sender, EventArgs e) //UID8346428952
        {
            this.diagramView.CopyLink();
        }

        // MENU Copy note
        public void CopyNoteItem_Click(object sender, EventArgs e) //UID6551958449
        {
            this.diagramView.CopyNote();
        }

        // MENU Copy link
        public void PasteToLinkItem_Click(object sender, EventArgs e) //UID9249220502
        {
            this.diagramView.PasteToLink();
        }

        // MENU Copy note
        public void PasteToNoteItem_Click(object sender, EventArgs e) //UID7415891621
        {
            this.diagramView.PasteToNote();
        }

        // NODE

        // MENU NODE transparent
        private void TransparentItem_Click(object sender, EventArgs e) //UID0380589581
        {
            this.diagramView.MakeSelectionTransparent();
        }

        // MENU NODE set font
        private void FontItem_Click(object sender, EventArgs e) //UID0168436072
        {
            this.diagramView.SelectFont();
        }

        // MENU NODE set font color
        private void FontColorItem_Click(object sender, EventArgs e) //UID8784417583
        {
            this.diagramView.SelectFontColor();
        }

        // MENU NODE edit node link
        private void EditLinkItem_Click(object sender, EventArgs e) //UID9954689045
        {
            this.diagramView.EditLink();
        }

        // MENU NODE edit node link
        private void BringTopItem_Click(object sender, EventArgs e) //UID0233830733
        {
            this.diagramView.MoveNodesToForeground();
        }

        // MENU NODE edit node link
        private void BringBottomItem_Click(object sender, EventArgs e) //UID2153145961
        {
            this.diagramView.MoveNodesToBackground();
        }

        // MENU remove shortcut
        private void RemoveShortcutItem_Click(object sender, EventArgs e) //UID3987406806
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.RemoveShortcuts(this.diagramView.selectedNodes);
            }
        }
        
        // MENU NODE protect sesitive data in node name
        private void ProtectItem_Click(object sender, EventArgs e) //UID9793719013
        {
            this.diagramView.ProtectNodes();
        }

        // MENU LINE select line color
        private void LineColorItem_Click(object sender, EventArgs e) //UID2597097201
        {
            this.diagramView.ChangeLineColor();
        }

        // MENU LINE select line color
        private void LineWidthItem_Click(object sender, EventArgs e) //UID9637709557
        {
            this.diagramView.ChangeLineWidth();
        }

        // IMAGE

        // MENU IMAGE add image
        private void ImageAddItem_Click(object sender, EventArgs e) //UID2469843788
        {
            this.diagramView.AddImage();
        }

        // MENU IMAGE image remove from diagram
        private void ImageRemoveItem_Click(object sender, EventArgs e) //UID5686987514
        {
            this.diagramView.RemoveImagesFromSelection();
        }

        // MENU IMAGE image embedded to diagram
        private void ImageEmbeddedItem_Click(object sender, EventArgs e) //UID5459560991
        {
            this.diagramView.MakeImagesEmbedded();
        }

        // POLYGON

        // MENU POLYGON image embedded to diagram
        private void PolygonCreateItem_Click(object sender, EventArgs e)
        {
            this.diagramView.SwitchPolygon();
        }

        // MENU POLYGON image embedded to diagram
        private void PolygonRemoveItem_Click(object sender, EventArgs e)
        {
            this.diagramView.RemovePolygon();
        }

        // ATTACHMENT

        // MENU NODE deploy attachment to system
        private void DeploayAttachmentItem_Click(object sender, EventArgs e) //UID5589053079
        {
            this.diagramView.AttachmentDeploy();
        }

        // MENU NODE add file attachment to diagram
        private void IncludeFileItem_Click(object sender, EventArgs e) //UID4450324996
        {
            this.diagramView.AttachmentAddFile(new Position(this.diagramView.startMousePos));
        }

        // MENU NODE add directory attachment to diagram
        private void IncludeDirectoryItem_Click(object sender, EventArgs e) //UID3536207534
        {
            this.diagramView.AttachmentAddDirectory(new Position(this.diagramView.startMousePos));
        }

        // MENU NODE remove included data
        private void RemoveFileItem_Click(object sender, EventArgs e) //UID1291567591
        {
            this.diagramView.AttachmentRemove();
        }

        // VIEW

        // MENU VIEW NEW VIEW
        private void NewViewItem_Click(object sender, EventArgs e) //UID5872215491
        {
            // otvorenie novej insancie DiagramView
            this.diagramView.diagram.OpenDiagramView();
        }

        // MENU Center
        public void CenterItem_Click(object sender, EventArgs e) //UID6631268419
        {
            this.diagramView.GoToHome();
        }

        // MENU set home position
        private void SetStartPositionItem_Click(object sender, EventArgs e) //UID2244493951
        {
            this.diagramView.SetCurentPositionAsHomePosition();
        }

        // MENU refresh diagram
        private void RefreshItem_Click(object sender, EventArgs e) //UID7176007006
        {
            this.diagramView.diagram.RefreshAll();
        }

        // LAYER

        // MENU Layer In
        public void InItem_Click(object sender, EventArgs e) //UID9204174888
        {
            if (this.diagramView.selectedNodes.Count() == 1)
            {
                this.diagramView.LayerIn(this.diagramView.selectedNodes[0]);
            }
        }

        // MENU Layer Out
        public void OutItem_Click(object sender, EventArgs e) //UID2210293088
        {
            this.diagramView.LayerOut();
        }

        // TOOLS

        // MENU Open Directory  - otvory adresar v ktorom sa nachadza prave otvreny subor
        public void OpenDiagramDirectoryItem_Click(object sender, EventArgs e) //UID8883607610
        {
            this.diagramView.OpenDiagramDirectory();
        }

        // MENU split node by lines
        public void SplitNodeItem_Click(object sender, EventArgs e) //UID1436745968
        {
            this.diagramView.SplitNode();
        }

        // OPTIONS

        // MENU Encription
        private void EncryptItem_Click(object sender, EventArgs e) //UID3914074702
        {
            if (this.diagramView.diagram.SetPassword())
            {
                this.diagramView.diagram.Unsave();
            }
        }

        // MENU Change password
        private void ChangePasswordItem_Click(object sender, EventArgs e) //UID8349505191
        {
            if (this.diagramView.diagram.ChangePassword())
            {
                this.diagramView.diagram.Unsave();
            }
        }

        // MENU Read only
        public void ReadonlyItem_Click(object sender, EventArgs e) //UID4803037156
        {
            this.diagramView.diagram.options.readOnly = items["readonlyItem"].Checked;
        }

        // MENU restore window position
        public void RestoreWindowItem_Click(object sender, EventArgs e) //UID1381135212
        {
            this.diagramView.RememberPosition(items["restoreWindowItem"].Checked);
        }

        // MENU Grid check
        public void GridItem_Click(object sender, EventArgs e) //UID1216267651
        {
            this.diagramView.diagram.options.grid = items["gridItem"].Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU coordinates
        public void CoordinatesItem_Click(object sender, EventArgs e) //UID1609350123
        {
            this.diagramView.diagram.options.coordinates = items["coordinatesItem"].Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Option Borders
        public void BordersItem_Click(object sender, EventArgs e) //UID5362447098
        {
            this.diagramView.diagram.options.borders = items["bordersItem"].Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Option Default font
        public void DefaultFontItem_Click(object sender, EventArgs e) //UID8175529111
        {
            this.diagramView.SelectDefaultFont();
        }

        // MENU reset font
        private void ResetFontItem_Click(object sender, EventArgs e) //UID6952845622
        {
            if (this.diagramView.selectedNodes.Count() > 0)
            {
                this.diagramView.diagram.ResetFont(this.diagramView.selectedNodes);
            }
            else
            {
                this.diagramView.diagram.ResetFont(
                    this.diagramView.diagram.GetAllNodes(), 
                    this.diagramView.shift, 
                    this.diagramView.currentLayer.id
                );
            }
        }

        // MENU set diagram icon
        private void SetIconItem_Click(object sender, EventArgs e) //UID8410362372
        {
            this.diagramView.SetIcon();
        }

        // MENU set diagram background image
        private void SetBackgroundItem_Click(object sender, EventArgs e) //UID5641879391
        {
            this.diagramView.SetBackgroundImage();
        }

        // MENU set diagram icon
        private void OpenLayerInNewViewItem_Click(object sender, EventArgs e) //UID9903539710
        {
            this.diagramView.diagram.options.openLayerInNewView = !this.diagramView.diagram.options.openLayerInNewView;
            items["openLayerInNewViewItem"].Checked = this.diagramView.diagram.options.openLayerInNewView;
            this.diagramView.diagram.Unsave();
        }
        
        // MENU reset font
        private void OpenConfigDirItem_Click(object sender, EventArgs e) //UID8429947533
        {
            this.diagramView.main.OpenConfigDir();
        }

        // HELP

        // MENU Console
        public void ConsoleItem_Click(object sender, EventArgs e) //UID3607589960
        {
            this.diagramView.main.ShowConsole();
        }

        // MENU visit homepage
        private void VisitWebsiteItem_Click(object sender, EventArgs e) //UID6474020819
        {
            Network.openUrl(this.diagramView.main.options.home_page);
        }

        // MENU show About form
        private void AboutItem_Click(object sender, EventArgs e) //UID7115733373
        {
            this.diagramView.main.ShowAbout();
        }
    }
}
