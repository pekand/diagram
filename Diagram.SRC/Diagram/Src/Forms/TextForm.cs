using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Diagram
{
    public class TextForm : Form
    {
        public Main main = null;

        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.RichTextBox TextFormTextBox;
        private System.Windows.Forms.RichTextBox TextFormNoteTextBox;

        /*************************************************************************************************************************/

        // ATTRIBUTES Diagram
        public Diagram diagram = null;       // diagram ktory je previazany z pohladom

        public Node node;

        public TextForm(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TextFormTextBox = new System.Windows.Forms.RichTextBox();
            this.TextFormNoteTextBox = new System.Windows.Forms.RichTextBox();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();

            this.SuspendLayout();
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
            // TextForm
            //
            this.Controls.Add(this.SplitContainer1);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 517);
            this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
            this.KeyPreview = true;
            this.Name = "TextForm";
            this.Text = "Edit";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TextForm_FormClosed);
            this.Load += new System.EventHandler(this.TextForm_Load);
            this.Resize += new System.EventHandler(this.TextForm_Resize);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public void TextForm_Load(object sender, EventArgs e)
        {
            if (this.node != null)
            {
                this.TextFormTextBox.Text = this.node.name;
                this.TextFormNoteTextBox.Text = this.node.note;
                this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2;
                this.Top = Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2;
                this.TextFormTextBox.Select();
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
        }

        // Save data and update main form
        public void SaveNode()
        {
            if (!this.diagram.options.readOnly)
            {
                if (
                    node.name != this.TextFormTextBox.Text ||
                    node.note != this.TextFormNoteTextBox.Text
                ) {
                    this.diagram.undo.add("edit", node);
                    node.name = this.TextFormTextBox.Text;
                    node.note = this.TextFormNoteTextBox.Text;
                    node.resize();
                    
                    DateTime dt = DateTime.Now;
                    node.timemodify = String.Format("{0:yyyy-M-d HH:mm:ss}", dt);

                    this.diagram.unsave();
                    this.diagram.InvalidateDiagram();
                }

                
            }
        }

        public void TextForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SaveNode();
            this.diagram.EditNodeClose(this.node);

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
