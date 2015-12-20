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
        private System.Windows.Forms.ColorDialog DColor;
        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.FontDialog DFont;
        private System.Windows.Forms.ColorDialog DFColor;
        private System.Windows.Forms.RichTextBox TextFormTextBox;
        private System.Windows.Forms.RichTextBox TextFormNoteTextBox;
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

            this.SuspendLayout();
            //
            // textformtabs
            //
            this.textformtabs.Controls.Add(this.mainTab);
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
            this.TextFormTextBox.DetectUrls = false;

            //
            // TextFormNoteTextBox
            //
            this.TextFormNoteTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextFormNoteTextBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TextFormNoteTextBox.Location = new System.Drawing.Point(0, 0);
            this.TextFormNoteTextBox.Name = "TextFormNoteTextBox";
            this.TextFormNoteTextBox.Size = new System.Drawing.Size(379, 413);
            this.TextFormNoteTextBox.TabIndex = 0;
            this.TextFormNoteTextBox.DetectUrls = false;
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
                this.textBoxScriptId.Text = this.rec.scriptid;
                this.TextFormTextBox.Select();
                this.DColor.Color = this.rec.color;
            }

            if (this.diagram.isReadOnly())
            {
                this.TextFormTextBox.ReadOnly = true;
                this.TextFormNoteTextBox.ReadOnly = true;
            }
            else
            {
                this.TextFormTextBox.ReadOnly = false;
                this.TextFormNoteTextBox.ReadOnly = false;
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
                    rec.scriptid != this.textBoxScriptId.Text
                )
                {
                    changed = true;
                    DateTime dt = DateTime.Now;
                    rec.timemodify = String.Format("{0:yyyy-M-d HH:mm:ss}", dt);
                }

                rec.text = this.TextFormTextBox.Text;
                rec.note = this.TextFormNoteTextBox.Text;
                rec.scriptid = this.textBoxScriptId.Text;
                if (rec.embeddedimage && rec.image!=null)
                {
                    rec.isimage = true;
                }
                else
                if (Os.FileExists(rec.imagepath))
                {
                    rec.isimage = true;
                    rec.image = new Bitmap(rec.imagepath);
                    rec.height = rec.image.Height;
                    string ext = "";
                    ext = Os.getExtension(rec.imagepath).ToLower();
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

    }
}
