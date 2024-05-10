using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace C__Doodle.Classes
{
    public class MenuScreen
    {
        private Form1 form;
        private Button playButton;
        private Button exitButton;

        public MenuScreen(Form1 form)
        {
            this.form = form;
            InitializeComponents();
        }
        public void InitializeComponents()
        {
            playButton = new Button();
            playButton.BackColor = Color.FromArgb(208, 249, 255);
            playButton.Text = "Играть";
            playButton.Location = new Point(160, 350);
            playButton.Size = new Size(100, 30);
            playButton.Click += new EventHandler(PlayButtonClick);
            Button exitButton = new Button();
            exitButton.Text = "Выход";
            exitButton.BackColor = Color.FromArgb(208, 249, 255);
            exitButton.Location = new Point(160, 650);
            exitButton.Size = new Size(100, 30);
            exitButton.Click += new EventHandler(ExitButtonClick);
            form.Controls.Add(playButton);
            form.Controls.Add(exitButton);
            //form.backMusicPlayer = new SoundPlayer(Properties.Resources.BackMusic);
            //form.PlaySound(form.backMusicPlayer);
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
        }
        private void ExitButtonClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
