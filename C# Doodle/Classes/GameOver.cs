using System;
using System.Drawing;
using System.Windows.Forms;

namespace C__Doodle.Classes
{
    public class GameOver
    {
        private Form1 form;
        private PictureBox gameOverScreen;
        public Button playButton;
        public GameOver(Form1 form)
        {
            this.form = form;
            InitializeComponents();
        }
        private void InitializeComponents()
        {
            gameOverScreen = new PictureBox();
            gameOverScreen.Image=Properties.Resources.GameOver;
            gameOverScreen.SizeMode = PictureBoxSizeMode.AutoSize;
            gameOverScreen.Visible = false;
            playButton = new Button();
            playButton.BackColor = Color.FromArgb(208, 249, 255);
            playButton.Text = "Играть";
            playButton.Location = new Point(160, 600);
            playButton.Size = new Size(100, 30);
            playButton.Click += new EventHandler(PlayButtonClick);
            playButton.Visible = false;
            form.Controls.Add(gameOverScreen);
            form.Controls.Add(playButton);
        }
        public void ShowGameOverScreen(int score)
        {
            playButton.Visible = true;
            gameOverScreen.Visible = true;
            form.isGameStart= false;
        }
        private void PlayButtonClick(object sender, EventArgs e)
        {
            form.isGameStart = true;
            form.Init();
            form.timer1 = new Timer();
            form.timer1.Interval = 15;
            form.timer1.Tick += new EventHandler(form.Update);
            form.timer1.Start();
            form.KeyDown += new KeyEventHandler(form.OnKeyboardPressed);
            form.KeyUp += new KeyEventHandler(form.OnKeyboardUp);
            form.BackgroundImage = Properties.Resources.Back;
            form.Paint += new PaintEventHandler(form.OnRepaint);
            foreach (Control control in form.Controls)
                if (control is Button)
                    control.Visible = false;
            form.Focus();
            gameOverScreen.Visible=false;
        }
    }
}
