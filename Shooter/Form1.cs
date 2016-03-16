using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shooter
{
    public partial class Form1 : Form
    {
        Lasers lasers = new Lasers();
        Baddies baddies = new Baddies();

        PictureBox shooterPanel;
        int ammo = 100;
        int points = 0;
        bool inProgress = false;

        public Form1()
        {
            InitializeComponent();
            createShooterPanel();
            label2.Text = ammo.ToString();
            label5.Text = points.ToString();
            linkLabel1.Text = "Start";
        }

        private void setup()
        {
            label3.Visible = false;
            ammo = 100;
            points = 0;
            lasers.clear();
            baddies.clear();
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            badguyTimer.Start();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            shooterPanel.Location = new Point(e.X - (shooterPanel.Width/2),shooterPanel.Location.Y);
        }

        private void createShooterPanel()
        {
            shooterPanel = new PictureBox();
            Temps.setImage(shooterPanel, "Shooter.28006.png");
            shooterPanel.Height = 256;
            shooterPanel.Width = 256;
            shooterPanel.Enabled = false;
            shooterPanel.Location = new Point(panel1.Width / 2 - 32, panel1.Height - shooterPanel.Height);
            shooterPanel.BackColor = Color.Black;
            panel1.Controls.Add(shooterPanel);
            shooterPanel.BringToFront();
        }

        private void shoot(int width)
        {
            Control laser = lasers.createLaser(panel1, shooterPanel, width);
            timer1.Start();
            points += baddies.handleShot(laser);
            label5.Text = points.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lasers.activeCount() == 0 && baddies.activeCount() == 0)
                timer1.Stop();
            processLasers();
            processBaddies();
        }

        void processLasers()
        {
            lasers.updatePosition();
        }

        void processBaddies()
        {
            if (!baddies.updatePosition(panel1))
                endGame();
        }

        private void endGame()
        {
            this.panel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            timer1.Stop();
            badguyTimer.Stop();
            label3.Visible = true;
            linkLabel1.Text = "Restart";
            inProgress = false;
            Cursor.Show();
        }

        private void removeList(List<Control> items, List<Control> activeItems)
        {
            foreach (Control p in items)
            {
                panel1.Controls.Remove(p);
                activeItems.Remove(p);
                p.Dispose();
            }
            items.Clear();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    if (ammo > 0)
                    {
                        shoot(10);
                        ammo -= 1;
                    }
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    if (ammo > 4)
                    {
                        shoot(150);
                        ammo -= 5;
                    }
                    break;
            }
            label2.Text = ammo.ToString();
        }

        private void badguyTimer_Tick(object sender, EventArgs e)
        {
            baddies.createBaddie(panel1);
            timer1.Start();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            shooterPanel.Location = new Point(shooterPanel.Location.X, panel1.Height - shooterPanel.Height);
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            setup();
            Cursor.Hide();
            inProgress = true;
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!inProgress)
            {
                Cursor.Show();
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (panel3.Visible)
                {
                    Cursor.Hide();
                    panel3.Visible = false;
                    panel3.SendToBack();
                    this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
                    this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
                    timer1.Start();
                    badguyTimer.Start();
                }
                else
                {
                    timer1.Stop();
                    badguyTimer.Stop();
                    Cursor.Show();
                    panel3.Visible = true;
                    panel3.BringToFront();
                    this.panel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
                    this.panel1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
                }
            }
        }
    }
}
