using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Diagram
{
    // map node structure for copy paste operation
    public struct BreadcrumbItem
    {
        public int left;
        public int top;
        public int width;
        public int height;
        public string name;
    }

    public class Breadcrumbs
    {
        public bool isVisible = true;
         
        public DiagramView diagramView = null;

        List<BreadcrumbItem> items = new List<BreadcrumbItem>();

        Font font = null;
        SolidBrush brush = new SolidBrush(Color.Gray);

        int left = 10;
        int top = 10;
        int width = 0;
        int height = 0;
        int itemSpace = 5;

        public Breadcrumbs(DiagramView diagramView)
        {
            this.diagramView = diagramView;
            this.font = new Font("Arial", 12);
        }

        public void Update()
        {
            this.width = 0;
            this.height = 0;
            this.isVisible = false;

            if (this.diagramView.layersHistory != null 
                && this.diagramView.layersHistory.Count > 1)
            {
                this.items.Clear();

                foreach (Layer layer in this.diagramView.layersHistory)
                {
                    BreadcrumbItem item = new BreadcrumbItem();

                    if (layer.parentNode != null)
                    {
                        item.name = layer.parentNode.name;
                    }
                    else
                    {
                        item.name = "Home";
                    }

                    if (item.name.Length > 10) {
                        item.name = item.name.Substring(0, 9);
                    }

                    SizeF s = Fonts.MeasureString(item.name, this.font);
                    item.left = this.width;
                    item.top = 0;
                    item.height = (int)s.Height;
                    item.width = (int)s.Width;
                    this.width += item.width + itemSpace;

                    if (this.height < item.height)
                    {
                        this.height = item.height;
                    }

                    this.items.Add(item);
                }

                this.isVisible = true;
            }
        }

        // EVENT Paint                                                                                 
        public void Draw(Graphics g)
        {
            if (!this.isVisible)
            {
                return;
            }

            this.left = this.diagramView.ClientSize.Width - this.width - 10;
            this.top = 10;

            // draw bar
            Rectangle bar = new Rectangle(this.left, this.top, this.width, this.height);
            g.FillRectangle(new SolidBrush(Color.FromArgb(50, 0, 0, 0)), bar);

            // Draw node names
            foreach (BreadcrumbItem item in items)
            {
                g.DrawString(
                    item.name,
                    this.font,
                    this.brush,
                    new PointF(this.left + item.left, this.top)
                );
            }
        }
    }
}
