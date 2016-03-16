using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Shooter
{
    class Baddies : Fallers
    {
        object baddieLock = new object();
        static Random r = new Random();
        const int maxBaddieSize = 100;
        const int minBaddieSize = 20;

        public bool updatePosition(Control panel)
        {
            foreach (Control p in active)
            {
                p.Location = new Point(p.Left, p.Location.Y + 4);
                if (p.Location.Y > panel.Height)
                {
                    return false;
                }
            }
            return true;
        }

        public int handleShot(Control shot)
        {
            int accumulatedPoints = 0;
            int x = shot.Width + shot.Location.X;
            lock (baddieLock)
            {
                foreach (Control baddie in active)
                {
                    bool case1 = shot.Location.X < baddie.Location.X && x > baddie.Location.X;
                    bool case2 = shot.Location.X > baddie.Location.X && x < baddie.Location.X + baddie.Width;
                    bool case3 = shot.Location.X < baddie.Location.X + baddie.Width && x > baddie.Location.X + baddie.Width;
                    if (case1 || case2 || case3)
                    {
                        scheduleRemoval(baddie);
                        accumulatedPoints++;
                    }
                }
                foreach(Control baddie in removed)
                {
                    active.Remove(baddie);
                    baddie.Parent.Controls.Remove(baddie);
                }
                removed.Clear();
            }
            return accumulatedPoints;
        }

        public void createBaddie(Control gameField)
        {
            int x = r.Next(0, gameField.Width - maxBaddieSize);
            int sz = r.Next(minBaddieSize, maxBaddieSize);
            PictureBox baddie = new PictureBox();
            setImage(baddie, "Shooter.si.png");
            baddie.SendToBack();
            baddie.Height = sz;
            baddie.Width = sz;
            baddie.Location = new Point(x, 0);
            baddie.Enabled = false;
            gameField.Controls.Add(baddie);
            add(baddie);
        }
    }
}
