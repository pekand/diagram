using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Diagram
{
    public class TextForm : Form
    {
        public Main main = null;

        private System.Windows.Forms.TabControl textformtabs;
        private System.Windows.Forms.TabPage mainTab;
        private System.Windows.Forms.TabPage optiontab;
        private System.Windows.Forms.Panel CPanel;
        private System.Windows.Forms.Button ColorPickButton;
        private System.Windows.Forms.ColorDialog DColor;
        private System.Windows.Forms.TextBox LinkTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.CheckBox TransparentCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ImageTextBox;
        private System.Windows.Forms.Button FontButton;
        private System.Windows.Forms.FontDialog DFont;
        private System.Windows.Forms.ColorDialog DFColor;
        private System.Windows.Forms.Button FontColorButton;
        private System.Windows.Forms.Panel FontColorPanel;
        private System.Windows.Forms.TextBox ImageHeight;
        private System.Windows.Forms.TextBox ImageWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox embeddedimageCheckBox;
        private System.Windows.Forms.TextBox editModification;
        private System.Windows.Forms.TextBox editCreation;
        private System.Windows.Forms.Label labelModification;
        private System.Windows.Forms.Label labelCreation;
        private System.Windows.Forms.RichTextBox TextFormTextBox;
        private System.Windows.Forms.RichTextBox TextFormNoteTextBox;
        private System.Windows.Forms.TabPage scriptTab;
        private System.Windows.Forms.Label labelScriptId;
        private System.Windows.Forms.TextBox textBoxScriptId;

        /*************************************************************************************************************************/

        // ATTRIBUTES Diagram
        public Diagram diagram = null;       // diagram ktory je previazany z pohladom

        public Node rec;

        public TextForm(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.textformtabs = new System.Windows.Forms.TabControl();
            this.mainTab = new System.Windows.Forms.TabPage();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TextFormTextBox = new System.Windows.Forms.RichTextBox();
            this.TextFormNoteTextBox = new System.Windows.Forms.RichTextBox();
            this.optiontab = new System.Windows.Forms.TabPage();
            this.editModification = new System.Windows.Forms.TextBox();
            this.editCreation = new System.Windows.Forms.TextBox();
            this.labelModification = new System.Windows.Forms.Label();
            this.labelCreation = new System.Windows.Forms.Label();
            this.embeddedimageCheckBox = new System.Windows.Forms.CheckBox();
            this.ImageHeight = new System.Windows.Forms.TextBox();
            this.ImageWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FontColorButton = new System.Windows.Forms.Button();
            this.FontColorPanel = new System.Windows.Forms.Panel();
            this.FontButton = new System.Windows.Forms.Button();
            this.TransparentCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ImageTextBox = new System.Windows.Forms.TextBox();
            this.LinkTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CPanel = new System.Windows.Forms.Panel();
            this.ColorPickButton = new System.Windows.Forms.Button();
            this.scriptTab = new System.Windows.Forms.TabPage();
            this.labelScriptId = new System.Windows.Forms.Label();
            this.textBoxScriptId = new System.Windows.Forms.TextBox();
            this.DColor = new System.Windows.Forms.ColorDialog();
            this.DFont = new System.Windows.Forms.FontDialog();
            this.DFColor = new System.Windows.Forms.ColorDialog();
            this.textformtabs.SuspendLayout();
            this.mainTab.SuspendLayout();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.optiontab.SuspendLayout();
            this.scriptTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // textformtabs
            // 
            this.textformtabs.Controls.Add(this.mainTab);
            this.textformtabs.Controls.Add(this.optiontab);
            this.textformtabs.Controls.Add(this.scriptTab);
            this.textformtabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textformtabs.Location = new System.Drawing.Point(0, 0);
            this.textformtabs.Name = "textformtabs";
            this.textformtabs.SelectedIndex = 0;
            this.textformtabs.Size = new System.Drawing.Size(393, 517);
            this.textformtabs.TabIndex = 0;
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.SplitContainer1);
            this.mainTab.Location = new System.Drawing.Point(4, 22);
            this.mainTab.Name = "mainTab";
            this.mainTab.Padding = new System.Windows.Forms.Padding(3);
            this.mainTab.Size = new System.Drawing.Size(385, 491);
            this.mainTab.TabIndex = 0;
            this.mainTab.Text = "Main";
            this.mainTab.UseVisualStyleBackColor = true;
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.Location = new System.Drawing.Point(3, 3);
            this.SplitContainer1.Name = "SplitContainer1";
            this.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.TextFormTextBox);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.TextFormNoteTextBox);
            this.SplitContainer1.Size = new System.Drawing.Size(379, 485);
            this.SplitContainer1.SplitterDistance = 68;
            this.SplitContainer1.TabIndex = 0;
            // 
            // TextFormTextBox
            // 
            this.TextFormTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFormTextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TextFormTextBox.Location = new System.Drawing.Point(0, 0);
            this.TextFormTextBox.Name = "TextFormTextBox";
            this.TextFormTextBox.Size = new System.Drawing.Size(379, 68);
            this.TextFormTextBox.TabIndex = 0;
            // 
            // TextFormNoteTextBox
            // 
            this.TextFormNoteTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFormNoteTextBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TextFormNoteTextBox.Location = new System.Drawing.Point(0, 0);
            this.TextFormNoteTextBox.Name = "TextFormNoteTextBox";
            this.TextFormNoteTextBox.Size = new System.Drawing.Size(379, 413);
            this.TextFormNoteTextBox.TabIndex = 0;
            // 
            // optiontab
            // 
            this.optiontab.Controls.Add(this.editModification);
            this.optiontab.Controls.Add(this.editCreation);
            this.optiontab.Controls.Add(this.labelModification);
            this.optiontab.Controls.Add(this.labelCreation);
            this.optiontab.Controls.Add(this.embeddedimageCheckBox);
            this.optiontab.Controls.Add(this.ImageHeight);
            this.optiontab.Controls.Add(this.ImageWidth);
            this.optiontab.Controls.Add(this.label4);
            this.optiontab.Controls.Add(this.label3);
            this.optiontab.Controls.Add(this.FontColorButton);
            this.optiontab.Controls.Add(this.FontColorPanel);
            this.optiontab.Controls.Add(this.FontButton);
            this.optiontab.Controls.Add(this.TransparentCheckBox);
            this.optiontab.Controls.Add(this.label2);
            this.optiontab.Controls.Add(this.ImageTextBox);
            this.optiontab.Controls.Add(this.LinkTextBox);
            this.optiontab.Controls.Add(this.label1);
            this.optiontab.Controls.Add(this.CPanel);
            this.optiontab.Controls.Add(this.ColorPickButton);
            this.optiontab.Location = new System.Drawing.Point(4, 22);
            this.optiontab.Name = "optiontab";
            this.optiontab.Padding = new System.Windows.Forms.Padding(3);
            this.optiontab.Size = new System.Drawing.Size(385, 491);
            this.optiontab.TabIndex = 1;
            this.optiontab.Text = "Option";
            this.optiontab.UseVisualStyleBackColor = true;
            // 
            // editModification
            // 
            this.editModification.Location = new System.Drawing.Point(107, 271);
            this.editModification.Name = "editModification";
            this.editModification.ReadOnly = true;
            this.editModification.Size = new System.Drawing.Size(107, 20);
            this.editModification.TabIndex = 16;
            // 
            // editCreation
            // 
            this.editCreation.Location = new System.Drawing.Point(107, 240);
            this.editCreation.Name = "editCreation";
            this.editCreation.ReadOnly = true;
            this.editCreation.Size = new System.Drawing.Size(107, 20);
            this.editCreation.TabIndex = 16;
            // 
            // labelModification
            // 
            this.labelModification.AutoSize = true;
            this.labelModification.Location = new System.Drawing.Point(18, 277);
            this.labelModification.Name = "labelModification";
            this.labelModification.Size = new System.Drawing.Size(89, 13);
            this.labelModification.TabIndex = 15;
            this.labelModification.Text = "Modification time:";
            // 
            // labelCreation
            // 
            this.labelCreation.AutoSize = true;
            this.labelCreation.Location = new System.Drawing.Point(18, 247);
            this.labelCreation.Name = "labelCreation";
            this.labelCreation.Size = new System.Drawing.Size(71, 13);
            this.labelCreation.TabIndex = 15;
            this.labelCreation.Text = "Creation time:";
            // 
            // embeddedimageCheckBox
            // 
            this.embeddedimageCheckBox.AutoSize = true;
            this.embeddedimageCheckBox.Location = new System.Drawing.Point(74, 199);
            this.embeddedimageCheckBox.Name = "embeddedimageCheckBox";
            this.embeddedimageCheckBox.Size = new System.Drawing.Size(108, 17);
            this.embeddedimageCheckBox.TabIndex = 14;
            this.embeddedimageCheckBox.Text = "Embedded image";
            this.embeddedimageCheckBox.UseVisualStyleBackColor = true;
            this.embeddedimageCheckBox.Click += new System.EventHandler(this.embeddedimageCheckBox_Click);
            // 
            // ImageHeight
            // 
            this.ImageHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ImageHeight.Location = new System.Drawing.Point(255, 165);
            this.ImageHeight.Name = "ImageHeight";
            this.ImageHeight.ReadOnly = true;
            this.ImageHeight.Size = new System.Drawing.Size(90, 26);
            this.ImageHeight.TabIndex = 13;
            // 
            // ImageWidth
            // 
            this.ImageWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ImageWidth.Location = new System.Drawing.Point(112, 164);
            this.ImageWidth.Name = "ImageWidth";
            this.ImageWidth.ReadOnly = true;
            this.ImageWidth.Size = new System.Drawing.Size(90, 26);
            this.ImageWidth.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(211, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Height:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Width:";
            // 
            // FontColorButton
            // 
            this.FontColorButton.Location = new System.Drawing.Point(87, 61);
            this.FontColorButton.Name = "FontColorButton";
            this.FontColorButton.Size = new System.Drawing.Size(75, 23);
            this.FontColorButton.TabIndex = 9;
            this.FontColorButton.Text = "Font Color";
            this.FontColorButton.UseVisualStyleBackColor = true;
            this.FontColorButton.Click += new System.EventHandler(this.FontColor_Click);
            // 
            // FontColorPanel
            // 
            this.FontColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FontColorPanel.Location = new System.Drawing.Point(18, 61);
            this.FontColorPanel.Name = "FontColorPanel";
            this.FontColorPanel.Size = new System.Drawing.Size(53, 23);
            this.FontColorPanel.TabIndex = 8;
            // 
            // FontButton
            // 
            this.FontButton.Location = new System.Drawing.Point(202, 61);
            this.FontButton.Name = "FontButton";
            this.FontButton.Size = new System.Drawing.Size(75, 23);
            this.FontButton.TabIndex = 7;
            this.FontButton.Text = "Font";
            this.FontButton.UseVisualStyleBackColor = true;
            this.FontButton.Click += new System.EventHandler(this.FontButton_Click);
            // 
            // TransparentCheckBox
            // 
            this.TransparentCheckBox.AutoSize = true;
            this.TransparentCheckBox.Location = new System.Drawing.Point(205, 34);
            this.TransparentCheckBox.Name = "TransparentCheckBox";
            this.TransparentCheckBox.Size = new System.Drawing.Size(83, 17);
            this.TransparentCheckBox.TabIndex = 6;
            this.TransparentCheckBox.Text = "Transparent";
            this.TransparentCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Image:";
            // 
            // ImageTextBox
            // 
            this.ImageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ImageTextBox.Location = new System.Drawing.Point(71, 132);
            this.ImageTextBox.Name = "ImageTextBox";
            this.ImageTextBox.Size = new System.Drawing.Size(295, 26);
            this.ImageTextBox.TabIndex = 4;
            // 
            // LinkTextBox
            // 
            this.LinkTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LinkTextBox.Location = new System.Drawing.Point(71, 90);
            this.LinkTextBox.Name = "LinkTextBox";
            this.LinkTextBox.Size = new System.Drawing.Size(295, 26);
            this.LinkTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Link:";
            // 
            // CPanel
            // 
            this.CPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CPanel.Location = new System.Drawing.Point(18, 27);
            this.CPanel.Name = "CPanel";
            this.CPanel.Size = new System.Drawing.Size(53, 23);
            this.CPanel.TabIndex = 1;
            // 
            // ColorPickButton
            // 
            this.ColorPickButton.Location = new System.Drawing.Point(87, 27);
            this.ColorPickButton.Name = "ColorPickButton";
            this.ColorPickButton.Size = new System.Drawing.Size(75, 23);
            this.ColorPickButton.TabIndex = 0;
            this.ColorPickButton.Text = "Color";
            this.ColorPickButton.UseVisualStyleBackColor = true;
            this.ColorPickButton.Click += new System.EventHandler(this.ColorPickButton_Click);
            // 
            // scriptTab
            // 
            this.scriptTab.Controls.Add(this.labelScriptId);
            this.scriptTab.Controls.Add(this.textBoxScriptId);
            this.scriptTab.Location = new System.Drawing.Point(4, 22);
            this.scriptTab.Margin = new System.Windows.Forms.Padding(2);
            this.scriptTab.Name = "scriptTab";
            this.scriptTab.Padding = new System.Windows.Forms.Padding(2);
            this.scriptTab.Size = new System.Drawing.Size(385, 491);
            this.scriptTab.TabIndex = 2;
            this.scriptTab.Text = "Scripts";
            this.scriptTab.UseVisualStyleBackColor = true;
            // 
            // labelScriptId
            // 
            this.labelScriptId.AutoSize = true;
            this.labelScriptId.Location = new System.Drawing.Point(17, 14);
            this.labelScriptId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelScriptId.Name = "labelScriptId";
            this.labelScriptId.Size = new System.Drawing.Size(19, 13);
            this.labelScriptId.TabIndex = 2;
            this.labelScriptId.Text = "Id:";
            // 
            // textBoxScriptId
            // 
            this.textBoxScriptId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxScriptId.Location = new System.Drawing.Point(52, 6);
            this.textBoxScriptId.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxScriptId.Name = "textBoxScriptId";
            this.textBoxScriptId.Size = new System.Drawing.Size(304, 26);
            this.textBoxScriptId.TabIndex = 0;
            // 
            // DFont
            // 
            this.DFont.Color = System.Drawing.SystemColors.ControlText;
            // 
            // TextForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 517);
            this.Controls.Add(this.textformtabs);
            this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
            this.KeyPreview = true;
            this.Name = "TextForm";
            this.Text = "Edit";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TextForm_FormClosed);
            this.Load += new System.EventHandler(this.TextForm_Load);
            this.Resize += new System.EventHandler(this.TextForm_Resize);
            this.textformtabs.ResumeLayout(false);
            this.mainTab.ResumeLayout(false);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.ResumeLayout(false);
            this.optiontab.ResumeLayout(false);
            this.optiontab.PerformLayout();
            this.scriptTab.ResumeLayout(false);
            this.scriptTab.PerformLayout();
            this.ResumeLayout(false);

        }

        public void TextForm_Load(object sender, EventArgs e)
        {
            if (this.rec != null)
            {
                this.TextFormTextBox.Text = this.rec.text;
                this.TextFormNoteTextBox.Text = this.rec.note;
                this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2;
                this.Top = Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2;
                this.textformtabs.SelectedTab = this.mainTab;
                this.LinkTextBox.Text = this.rec.link;
                this.textBoxScriptId.Text = this.rec.scriptid;
                this.TextFormTextBox.Select();
                this.CPanel.BackColor = this.rec.color;
                this.DColor.Color = this.rec.color;
                this.FontColorPanel.BackColor = this.rec.fontcolor;
                this.ImageTextBox.Text = this.rec.imagepath;
                this.TransparentCheckBox.Checked = this.rec.transparent;
                this.embeddedimageCheckBox.Checked = this.rec.embeddedimage;
                this.editCreation.Text = this.rec.timecreate;
                this.editModification.Text = this.rec.timemodify;
                if (this.rec.isimage)
                {
                    if (this.rec.iheight == 0 && this.rec.iheight == 0)
                    {
                        this.ImageWidth.Text = this.rec.width.ToString();
                        this.ImageHeight.Text = this.rec.height.ToString();
                    }
                    else
                    {
                        this.ImageWidth.Text = this.rec.iwidth.ToString();
                        this.ImageHeight.Text = this.rec.iheight.ToString();
                    }
                }
                else
                {
                    this.ImageWidth.Text = "";
                    this.ImageHeight.Text = "";
                    this.ImageWidth.Enabled = false;
                    this.ImageWidth.Enabled = false;
                }
            }

        }

        public void setDiagram(Diagram diagram)
        {
            this.diagram = diagram;
        }

        public Diagram getDiagram()
        {
            return this.diagram;
        }

        public void SetFocus()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.BringToFront();
                this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        public void TextForm_Resize(object sender, EventArgs e)
        {
            this.TextFormTextBox.Height = this.ClientSize.Height - 100;
            this.LinkTextBox.Width = this.ClientSize.Width - 150;
            this.ImageTextBox.Width = this.ClientSize.Width - 150;
            this.textBoxScriptId.Width = this.ClientSize.Width - 150;
        }

        // Save data and update main form
        public void SaveNode()
        {
            if (!this.diagram.options.readOnly)
            {
                bool changed = false;
                if
                (
                    rec.text != this.TextFormTextBox.Text ||
                    rec.note != this.TextFormNoteTextBox.Text ||
                    rec.link != this.LinkTextBox.Text ||
                    rec.scriptid != this.textBoxScriptId.Text ||
                    rec.transparent != this.TransparentCheckBox.Checked ||
                    rec.imagepath != this.ImageTextBox.Text
                )
                {
                    changed = true;
                    DateTime dt = DateTime.Now;
                    rec.timemodify = String.Format("{0:yyyy-M-d HH:mm:ss}", dt);
                }

                rec.text = this.TextFormTextBox.Text;
                rec.note = this.TextFormNoteTextBox.Text;
                rec.link = this.LinkTextBox.Text;
                rec.scriptid = this.textBoxScriptId.Text;
                rec.transparent = this.TransparentCheckBox.Checked;
                rec.imagepath = this.ImageTextBox.Text;
                rec.embeddedimage = this.embeddedimageCheckBox.Checked;
                if (rec.embeddedimage && rec.image!=null)
                {
                    rec.isimage = true;
                }
                else
                if (File.Exists(rec.imagepath))
                {
                    rec.isimage = true;
                    rec.image = new Bitmap(rec.imagepath);
                    rec.height = rec.image.Height;
                    string ext = "";
                    ext = Path.GetExtension(rec.imagepath).ToLower();
                    if (ext != ".ico") rec.image.MakeTransparent(Color.White);
                    rec.width = rec.image.Width;
                }
                else
                {
                    rec.isimage = false;
                }
                if (!rec.isimage)
                {
                    SizeF s = this.diagram.MeasureStringWithMargin(rec.text, rec.font);
                    rec.height = (int)s.Height;
                    rec.width = (int)s.Width;
                }

                if(changed)
                    this.diagram.unsave();

                this.diagram.InvalidateDiagram();
            }
        }

        public void TextForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SaveNode();
            this.diagram.EditNodeClose(this.rec);

            this.diagram.TextWindows.Remove(this);
            main.TextWindows.Remove(this);
            this.diagram.CloseDiagram();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                this.SaveNode();
                this.diagram.save();
                return true;
            }

            if (keyData == Keys.Escape)
            {
                this.SaveNode();
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void ColorPickButton_Click(object sender, EventArgs e)
        {
            DColor.Color = rec.color;
            if (DColor.ShowDialog() == DialogResult.OK)
            {
                if (!this.diagram.options.readOnly)
                {
                    this.rec.color = DColor.Color;
                    this.CPanel.BackColor = DColor.Color;
                    this.diagram.InvalidateDiagram();
                }
            }
        }

        private void FontButton_Click(object sender, EventArgs e)
        {
            DFont.Font = rec.font;
            if (DFont.ShowDialog() == DialogResult.OK)
            {
                if (!this.diagram.options.readOnly)
                {
                    this.rec.font = DFont.Font;
                    SizeF s = this.diagram.MeasureStringWithMargin(rec.text, rec.font);
                    rec.width = (int)s.Width;
                    rec.height = (int)s.Height;
                    this.diagram.InvalidateDiagram();
                }
            }
        }

        private void FontColor_Click(object sender, EventArgs e)
        {
            DFColor.Color = rec.fontcolor;
            if (DFColor.ShowDialog() == DialogResult.OK)
            {
                if (!this.diagram.options.readOnly)
                {
                    this.rec.fontcolor = DFColor.Color;
                    this.FontColorPanel.BackColor = DFColor.Color;
                    this.diagram.InvalidateDiagram();
                }
            }
        }

        private void embeddedimageCheckBox_Click(object sender, EventArgs e)
        {
            this.diagram.unsave();
        }
        
    }
}
