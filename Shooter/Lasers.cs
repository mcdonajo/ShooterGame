using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Shooter
{
    class Lasers : Projectiles
    {
        object laserLock = new object();

        public void updatePosition()
        {
            lock (laserLock)
            {
                foreach (Control p in active)
                {
                    p.Location = new Point(p.Left, p.Location.Y - 50);
                    if (p.Bottom < 0)
                        scheduleRemoval(p);
                }
                foreach (Control laser in removed)
                {
                    active.Remove(laser);
                    laser.Parent.Controls.Remove(laser);
                }
                removed.Clear();
            }
        }

        public Control createLaser(Control gameField, Control origin, int width)
        {
            Panel laserPanel = new Panel();
            laserPanel.BringToFront();
            laserPanel.Height = gameField.Height - origin.Height;
            laserPanel.Width = width;
            laserPanel.Enabled = false;
            int x = origin.Location.X + (origin.Width / 2) - laserPanel.Width / 2;
            laserPanel.Location = new Point(x, 0);
            laserPanel.BackColor = Color.Red;
            gameField.Controls.Add(laserPanel);
            add(laserPanel);

            return laserPanel;
        }

    }
}
