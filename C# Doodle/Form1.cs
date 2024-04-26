using C__Doodle.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace C__Doodle
{
    public partial class Form1 : Form
    {
        Player player;
        Timer timer1;
        public Form1()
        {
            InitializeComponent();
            Init();
            timer1 = new Timer();
            timer1.Interval = 15;
            timer1.Tick += new EventHandler(Update);
            timer1.Start();
            this.KeyDown += new KeyEventHandler(OnKeyboardPressed);
            this.KeyUp += new KeyEventHandler(OnKeyboardUp);
            this.BackgroundImage = Properties.Resources.Back;
            this.Height = 800;
            this.Width = 430;
            this.Paint += new PaintEventHandler(OnRepaint);
        }
        private PictureBox pauseOverlay;
        public void Init()
        {
            PlatformController.platforms = new List<Platform> ();
            PlatformController.AddPlatform(new PointF(100, 400));
            PlatformController.startPlatformPosY = 400;
            PlatformController.score = 0;
            PlatformController.money = 0;
            PlatformController.GenerateStartSequence();
            PlatformController.bullets.Clear();
            PlatformController.enemies.Clear();
            PlatformController.bonuses.Clear();
            pauseOverlay = new PictureBox();
            pauseOverlay.Image = Properties.Resources.Paused;
            pauseOverlay.SizeMode = PictureBoxSizeMode.AutoSize;
            pauseOverlay.Visible = false;
            this.Controls.Add(pauseOverlay);
            label3.Visible = false; 
            pauseOverlay.BringToFront();
            player = new Player();
        }
        private void OnKeyboardUp(object sender, KeyEventArgs e)
        {
            player.physics.dx = 0;
            player.sprite = Properties.Resources.Personage;
            switch(e.KeyCode.ToString())
            {
                case "Space":
                    PlatformController.CreateBullet(new PointF(player.physics.transform.position.X 
                        + player.physics.transform.size.Width / 2, player.physics.transform.position.Y));
                    break;
                //case "Tab":
                //    if (PlatformController.money >= 100)
                //    {
                //        PlatformController.money -= 100;
                //        player.sprite = Properties.Resources.PersonageArmour;
                //        player.physics.usedBonus = true;
                //        System.Timers.Timer timer = new System.Timers.Timer();
                //        timer.Interval = 30000;
                //        player.physics.usedBonus = true;
                //        timer.Elapsed += (s, es) =>
                //        {
                //            player.sprite = Properties.Resources.Personage;
                //            player.physics.usedBonus = false;
                //            timer.Stop();
                //            timer.Dispose();
                //        };
                //        timer.Start();
                //    }
                //    else
                //        PlatformController.money = 1000;
                //    break;
            }
        }
        private bool isPaused = false;
        private void OnKeyboardPressed(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode.ToString())
            {
                case "D":
                    player.physics.dx = 6;
                    break;
                case "A":
                    player.sprite = Properties.Resources.PersonageLeft;
                    player.physics.dx = -6;
                    break;
                case "Space":
                    player.sprite = Properties.Resources.PlayerShot;
                    break;
                case "Escape":
                    Application.Exit();
                    break;
                case "Tab":
                    isPaused = !isPaused;
                    if (isPaused)
                    {
                        label3.Visible = true;
                        label3.BackColor = Color.FromArgb(208, 249, 255);
                        label3.Text =""+ PlatformController.score;
                        label3.AutoSize = true;
                        label3.BringToFront();
                        timer1.Enabled=false;
                        pauseOverlay.Visible = true;
                    }
                    else
                    {
                        label3.Visible=false;
                        timer1.Enabled = true;
                        pauseOverlay.Visible = false;
                    }
                    break;
            }
        }
        private void Update(object sender, EventArgs e)
        {
            this.Text = "C# Doodle";
            if ((player.physics.transform.position.Y >= PlatformController.platforms[0].transform.position.Y + 200)
             || (player.physics.StandartCollidePlayerWithObjects(true, false)))
                Init();
            player.physics.StandartCollidePlayerWithObjects(false, true);
            if (PlatformController.bullets.Count > 0)
            {
                for (int i = 0; i < PlatformController.bullets.Count; i++)
                {
                    if (Math.Abs(PlatformController.bullets[i].physics.transform.position.Y - 
                        player.physics.transform.position.Y) > 500)
                    {
                        PlatformController.RemoveBullet(i);
                        continue;
                    }
                    PlatformController.bullets[i].MoveUp();
                }
            }
            if (PlatformController.enemies.Count > 0)
            {
                for (int i = 0; i < PlatformController.enemies.Count; i++)
                {
                    if (PlatformController.enemies[i].physics.StandartCollide())
                    {
                        PlatformController.RemoveEnemy(i);
                        PlatformController.money+=25;
                        break;
                    }
                }
            }
            player.physics.AddPhysics();
            FollowPlayer();
            Invalidate();
        }
        public void FollowPlayer()
        {
            var offset = 400 - (int)player.physics.transform.position.Y;
            player.physics.transform.position.Y += offset;
            for (int i = 0;i<PlatformController.platforms.Count;i++)
            {
                var platform = PlatformController.platforms[i];
                platform.transform.position.Y += offset;
            }
            for (int i = 0; i < PlatformController.bullets.Count; i++)
            {
                var bullet = PlatformController.bullets[i];
                bullet.physics.transform.position.Y += offset;
            }
            for (int i = 0; i < PlatformController.enemies.Count; i++)
            {
                var enemy = PlatformController.enemies[i];
                enemy.physics.transform.position.Y += offset;
            }
            for (int i = 0; i < PlatformController.bonuses.Count; i++)
            {
                var bonus = PlatformController.bonuses[i];
                bonus.physics.transform.position.Y += offset;
            }
        }
        private void OnRepaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (PlatformController.platforms.Count > 0)
            {
                for (int i = 0; i < PlatformController.platforms.Count; i++)
                    PlatformController.platforms[i].DrawSprite(g);
            }
            if (PlatformController.bullets.Count > 0)
            {
                for (int i = 0; i < PlatformController.bullets.Count; i++)
                    PlatformController.bullets[i].DrawSprite(g);
            }
            if (PlatformController.enemies.Count > 0)
            {
                for (int i = 0; i < PlatformController.enemies.Count; i++)
                    PlatformController.enemies[i].DrawSprite(g);
            }
            if (PlatformController.bonuses.Count > 0)
            {
                for (int i = 0; i < PlatformController.bonuses.Count; i++)
                    PlatformController.bonuses[i].DrawSprite(g);
            }
            label1.Text = "Score - " + PlatformController.score;
            label2.Text = "Money - " + PlatformController.money;
            player.DrawSprite(g);
        }
    }
}
