using System;
using System.Drawing;
using System.Windows.Forms;

namespace Diagram
{
    public delegate void PositionChangeEventHandler(object source, PositionEventArgs e);

    public class PositionEventArgs : EventArgs
    {
        private float ScrollPosition;
        public PositionEventArgs(float position)
        {
            ScrollPosition = position;
        }
        public float GetPosition()
        {
            return ScrollPosition;
        }
    }

    public class ScrollBar
    {
        public object parent;   // okno v ktorom je scrollbar vykreslovany

        // position
        public int barx = 10;
        public int bary = 100;
        public int barwidth = 1000;
        public int barheight = 30;

        // orientation - orientácia skrolbaru
        public bool vertical = true;
        public bool horizontal = true;

        // margin
        public int barmarginleft = 40;
        public int barmarginright = 40;
        public int barmarginbottom = 20;

        // track
        public float position = 0.5F;
        public int trackwidth = 50;
        public int trackpos = 150;
        public int trackposold = 150;

        // mouse click
        public bool mousedown = false; // bolo kliknuté do scrollbaru
        public int delta = 0;

        // timer - animácia
        Timer timer = new Timer(); // timer pre animaciu
        public int opacity = 0;
        public bool animation = false; // ci je animacia spustena
        public bool active = false; // je zobrazene skrolovanie?
        public bool fadein = true; // zobrazuje sa scrollbar
        public bool fadeout = false; // skovava sa scrollbar

        // event change position
        public event PositionChangeEventHandler OnChangePosition;

        public ScrollBar(object parent, int width, int height, bool horizontalOrientation = true, float per = 0.5F)
        {
            this.parent = parent;

            ((Form)this.parent).Paint += new System.Windows.Forms.PaintEventHandler(this.PaintEvent);

            vertical = !horizontalOrientation;
            horizontal = horizontalOrientation;

            setPosition(per);

            if (horizontal)
            {
                barx = barmarginleft;
                bary = height - barheight - barmarginbottom;
                barwidth = width - barx - barmarginright;

                trackpos = barx + (int)((barwidth - trackwidth) * position);
            }

            if (vertical)
            {
                bary = barmarginleft;
                barx = width - barheight - barmarginbottom;
                barwidth = barheight;
                barheight = height - bary - barmarginright;

                trackpos = bary + (int)((barheight - trackwidth) * position);
            }

            timer.Tick += new EventHandler(Tick);
            timer.Interval = 50;
            timer.Enabled = false;
        }

        public bool Resize(int width, int height)
        {
            if (horizontal)
            {
                bary = height - barheight - barmarginbottom;
                barwidth = width - barx - barmarginright;
            }

            if (vertical)
            {
                barx = width - barwidth - barmarginbottom;
                barheight = height - bary - barmarginright;
            }

            return true;
        }

        // EVENT Paint                                                                                 // [PAINT] [EVENT]
        public void PaintEvent(object sender, PaintEventArgs e)
        {
            this.Paint(e.Graphics);
        }

        public void Paint(Graphics g)
        {
            if (animation || active || fadeout)
            {

                Rectangle bar = new Rectangle();
                Rectangle tracker = new Rectangle();

                if (horizontal)
                {
                    bar.X = barx;
                    bar.Y = bary;
                    bar.Width = barwidth;
                    bar.Height = barheight;

                    tracker.X = trackpos;
                    tracker.Y = bary;
                    tracker.Width = trackwidth;
                    tracker.Height = barheight;
                }

                if (vertical)
                {
                    bar.X = barx;
                    bar.Y = bary;
                    bar.Width = barwidth;
                    bar.Height = barheight;

                    tracker.X = barx;
                    tracker.Y = trackpos;
                    tracker.Width = barwidth;
                    tracker.Height = trackwidth;
                }
                g.FillRectangle(new SolidBrush(Color.FromArgb(this.opacity, 0, 0, 0)), bar);
                g.FillRectangle(new SolidBrush(Color.FromArgb(this.opacity * 2, 0, 0, 0)), tracker);
            }
        }

        public bool MouseDown(int mx, int my)
        {
            if (barx <= mx && mx <= barx + barwidth && bary <= my && my <= bary + barheight)
            {
                mousedown = true;

                if (horizontal)
                {
                    if (trackpos <= mx && mx <= trackpos + trackwidth) // click on track
                    {
                        delta = trackpos - mx;
                        trackpos = mx + delta;
                    }
                    else
                    {  // click on bar
                        trackpos = mx - trackwidth / 2;
                        delta = -trackwidth / 2;
                    }

                    if (trackpos < barx)
                    {
                        trackpos = barx;
                    }

                    if (barx + barwidth - trackwidth < trackpos)
                    {
                        trackpos = barx + barwidth - trackwidth;
                    }

                    position = (float)(trackpos - barx) / (barwidth - trackwidth);
                }

                if (vertical)
                {
                    if (trackpos <= my && my <= trackpos + trackwidth) // click on track
                    {
                        delta = trackpos - my;
                        trackpos = my + delta;
                    }
                    else
                    {  // click on bar
                        trackpos = my - trackwidth / 2;
                        delta = -trackwidth / 2;
                    }

                    if (trackpos < bary)
                    {
                        trackpos = bary;
                    }

                    if (bary + barheight - trackwidth < trackpos)
                    {
                        trackpos = bary + barheight - trackwidth;
                    }

                    position = (float)(trackpos - bary) / (barheight - trackwidth);
                }

                return true;
            }
            return false;
        }

        public bool MouseMove(int mx, int my)
        {
            if (mousedown) // je kliknuté do skrolbaru
            {

                if (horizontal)
                {
                    trackpos = mx + delta;

                    if (trackpos < barx)
                    {
                        trackpos = barx;
                    }

                    if (barx + barwidth - trackwidth < trackpos)
                    {
                        trackpos = barx + barwidth - trackwidth;
                    }

                    position = (float)(trackpos - barx) / (barwidth - trackwidth);
                }

                if (vertical)
                {
                    trackpos = my + delta;

                    if (trackpos < bary)
                    {
                        trackpos = bary;
                    }

                    if (bary + barheight - trackwidth < trackpos)
                    {
                        trackpos = bary + barheight - trackwidth;
                    }

                    position = (float)(trackpos - bary) / (barheight - trackwidth);
                }

                if (trackposold != trackpos)
                {
                    trackposold = trackpos;

                    if (OnChangePosition != null)
                    {
                        OnChangePosition(this, new PositionEventArgs(position));
                    }

                    return true;
                }

            }
            else // ak sa len prehádz ponad scrollbar mišou
            {
                if (barx <= mx && mx <= barx + barwidth && bary <= my && my <= bary + barheight)
                {
                    if (!active)
                    {
                        active = true;
                        this.fadein = true;
                        this.fadeout = false;
                        timer.Start();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (active)
                    {
                        active = false;
                        this.fadein = false;
                        this.fadeout = true;
                        timer.Start();
                        return true;
                    }
                    else
                    {
                        active = false;
                        return false;
                    }
                }
            }

            return false;
        }

        public bool MouseUp()
        {
            if (mousedown) {
                if (trackposold != trackpos)
                {
                    trackposold = trackpos;

                    if (horizontal)
                    {
                        position = (float)(trackpos - barx) / (barwidth - trackwidth);
                    }


                    if (vertical)
                    {
                        position = (float)(trackpos - bary) / (barheight - trackwidth);
                    }

                    if (OnChangePosition != null)
                    {
                        OnChangePosition(this, new PositionEventArgs(position));
                    }
                }

                mousedown = false;
                return true;
            }

            return false;
        }

        public void Tick(object sender, EventArgs e)
        {
            if (this.fadein)
            {
                if (this.opacity <= 50)
                {
                    this.opacity += 10;
                    this.animation = true;
                }
                else
                {
                    this.animation = false;
                    this.timer.Stop();
                }
            }

            if (this.fadeout)
            {
                if (this.opacity > 0)
                {
                    this.opacity -= 10;
                    this.animation = true;
                }
                else
                {
                    //this.animation = false;
                    this.timer.Enabled = false;
                    this.timer.Stop();
                }
            }

            ((Form)this.parent).Invalidate();
		}

        public void setPosition(float per)
        {
            position = per;
            if (horizontal)
            {
                trackpos = (int)(position * (barwidth - trackwidth) + barx);
            }

            if (vertical)
            {
                trackpos = (int)(position * (barheight - trackwidth) + bary);
            }
        }
    }
}
