using C__Doodle.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C__Doodle
{
    public partial class Form1 : Form
    {
        public Player player;
        public Timer timer1;
        public bool isGameStart = false;
        private bool isPaused = false;
        private PictureBox pauseOverlay;
        public SoundPlayer backMusicPlayer;
        private GameOver gameOver;
        private MenuScreen menuScreen;
        private GameRender gameRender;
        public Form1()
        {
            InitializeComponent();
            CreateMenu();
            this.KeyPreview = true;
            gameOver = new GameOver(this);
            menuScreen = new MenuScreen(this);
            gameRender = new GameRender(this);
        }
        private void CreateMenu()
        {
            BackgroundImage = Properties.Resources.Menu;
            label3.Visible = false;
        }
        private void GameOver()
        {
            gameOver.playButton.BringToFront();
            gameOver.ShowGameOverScreen(PlatformController.score);
            label4.Visible = true;
            label1.Visible = false;
            label2.Visible = false;
            label4.BackColor = Color.FromArgb(208, 249, 255);
            label4.Text = "" + PlatformController.score;
            label4.AutoSize = true;
            label4.BringToFront();
            timer1.Stop();
        }
        public void Init()
        {
            PlatformController.platforms = new List<Platform>();
            PlatformController.AddPlatform(new PointF(100, 400));
            PlatformController.startPlatformPosY = 400;
            PlatformController.score = 0;
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
            label4.Visible = false;
            label1.Visible = true;
            label2.Visible = true;
            pauseOverlay.BringToFront();
            player = new Player();
        }
        public void OnKeyboardUp(object sender, KeyEventArgs e)
        {
            player.physics.dx = 0;
            player.sprite = Properties.Resources.Personage;
            switch (e.KeyCode.ToString())
            {
                case "Space":
                    PlatformController.CreateBullet(new PointF(player.physics.transform.position.X
                        + player.physics.transform.size.Width / 2, player.physics.transform.position.Y));
                    SoundPlayer shoot = new SoundPlayer(Properties.Resources.Shoot);
                    shoot.Play();
                    break;
            }
        }
        public void OnKeyboardPressed(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
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
                        label3.Text = "" + PlatformController.score;
                        label3.AutoSize = true;
                        label3.BringToFront();
                        timer1.Enabled = false;
                        pauseOverlay.Visible = true;
                    }
                    else
                    {
                        label3.Visible = false;
                        timer1.Enabled = true;
                        pauseOverlay.Visible = false;
                    }
                    break;
            }
        }
        public void Update(object sender, EventArgs e)
        {
            if ((player.physics.transform.position.Y >= PlatformController.platforms[0].transform.position.Y + 200)
             || (player.physics.StandartCollidePlayerWithObjects(true, false)))
            {
                GameOver();
                timer1.Stop();
            }
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
                    if (PlatformController.enemies[i].physics.StandartCollide())
                    {
                        PlatformController.RemoveEnemy(i);
                        SoundPlayer killEnemy = new SoundPlayer(Properties.Resources.DeathEnemy);
                        killEnemy.Play();
                        PlatformController.score += 15;
                        break;
                    }
            }
            if (player.physics.transform.position.X < 0)
                player.physics.transform.position.X = this.ClientSize.Width - player.physics.transform.size.Width;
            if (player.physics.transform.position.X + player.physics.transform.size.Width > this.ClientSize.Width)
                player.physics.transform.position.X = 0;
            player.physics.AddPhysics();
            FollowPlayer();
            Invalidate();
        }
        public async void PlaySound(SoundPlayer soundPlayer)
        {
            await Task.Run(() => soundPlayer.PlaySync());
        }
        public void FollowPlayer()
        {
            var offset = 400 - (int)player.physics.transform.position.Y;
            player.physics.transform.position.Y += offset;
            for (int i = 0; i < PlatformController.platforms.Count; i++)
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
        public void OnRepaint(object sender, PaintEventArgs e)
        {
            gameRender.Paint(e.Graphics);
            label1.Text = "Score - " + PlatformController.score;
        }
    }
}
