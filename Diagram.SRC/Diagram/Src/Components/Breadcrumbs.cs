using System;
using System.Windows.Forms;

namespace Diagram
{
    public class Breadcrumbs : Panel
    {
        public DiagramView diagramView = null;

        public delegate void SearchPanelChangedEventHandler(string action, string search);
        public event SearchPanelChangedEventHandler SearchpanelStateChanged;

        
        public Breadcrumbs(DiagramView diagramView)
        {
            this.diagramView = diagramView;

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
            this.SuspendLayout();
            //
            // SearchPanel
            //
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void InitializeSearchPanelComponent()
        {

            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));

            Form parentForm = (this.diagramView as Form);
        }

        public void centerPanel()
        {
            this.Top = 10;
            this.Left = 10;
        }

        public void ShowPanel()
        {
            this.Show();
            this.centerPanel();
        }

        public void HidePanel()
        {
            this.Hide();
        }
    }
}
