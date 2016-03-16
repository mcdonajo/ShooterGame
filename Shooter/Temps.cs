using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Shooter
{
    class Temps
    {
        protected List<Control> active = new List<Control>();
        protected List<Control> removed = new List<Control>();

        public void add(Control c)
        {
            active.Add(c);
        }

        protected void scheduleRemoval(Control c)
        {
            removed.Add(c);
        }

        public int activeCount()
        {
            return active.Count;
        }

        public void clear()
        {
            removed.AddRange(active);
            foreach (Control c in removed)
            {
                active.Remove(c);
            }
            removed.Clear();
        }

        public static void setImage(PictureBox pb, string resource)
        {
            System.IO.Stream file = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
            pb.Image = Image.FromStream(file);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
