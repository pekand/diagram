using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{
    class SearchPanel:Panel
    {
        private object parentComponent;

        public int minimalSize = 100;
        public int maximalSize = 400;

        public delegate void SearchPanelChangedEventHandler(string action, string search);
        public event SearchPanelChangedEventHandler SearchpanelStateChanged;

        public SearchPanel(object parentComponent)
        {
            this.parentComponent = parentComponent;

            InitializeComponent();
            InitializeSearchPanelComponent();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.VisibleChanged += new System.EventHandler(this.panel1_VisibleChanged);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(4, 4);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(100, 15);
            this.textBoxSearch.TabIndex = 0;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            this.textBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearch_KeyDown);
            this.Controls.Add(this.textBoxSearch);
        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSearch;

        private void InitializeSearchPanelComponent()
        {
            
            this.textBoxSearch.Size = new System.Drawing.Size(this.minimalSize, 15);
            this.Size = new System.Drawing.Size(this.minimalSize + 4, 15);
            this.BackColor = System.Drawing.Color.White;

            Form parentForm = (this.parentComponent as Form);
            parentForm.Resize += new System.EventHandler(this.Parent_Resize);

            System.Drawing.Size size = TextRenderer.MeasureText("Text", textBoxSearch.Font);
            textBoxSearch.Height = size.Height;
            this.Height = size.Height + 2;
            
            this.centerPanel();
        }

        

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            string action = "";
            if (e.KeyCode == Keys.Enter)
            {
                action = "search";
            }

            if (e.KeyCode == Keys.Up)
            {
                action = "searchNext";
            }

            if (e.KeyCode == Keys.Down)
            {
                action = "searchPrec";
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
                action = "cancel";
            }

            if (this.SearchpanelStateChanged != null)
                this.SearchpanelStateChanged(action, textBoxSearch.Text);

            this.centerPanel();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            
        }

        public void centerPanel()
        {
            Form parentForm = (this.parentComponent as Form);
            this.Top = parentForm.Height - 100;
            this.Left = parentForm.Width / 2 - this.Width / 2;

            maximalSize = parentForm.Width - 100;

            int newWidth = 0;
            System.Drawing.Size size = TextRenderer.MeasureText(textBoxSearch.Text, textBoxSearch.Font);
            newWidth = size.Width + 5;

            if (minimalSize <= newWidth && newWidth <= maximalSize)
            {
                this.Width = newWidth + 2;
                textBoxSearch.Width = newWidth;
            }
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            if (this.Visible) 
            {
                this.centerPanel();
            }
        }

        private void panel1_VisibleChanged(object sender, EventArgs e)
        {
            this.centerPanel();
        }
    }
}
