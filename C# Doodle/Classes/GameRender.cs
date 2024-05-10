using System.Drawing;

namespace C__Doodle.Classes
{
    public class GameRender
    {
        private readonly Form1 form1;
        public GameRender(Form1 form1)
        {
            this.form1 = form1;
        }

        public void Paint(Graphics graphics) 
        {
            if (PlatformController.platforms.Count > 0)
                for (int i = 0; i < PlatformController.platforms.Count; i++)
                    PlatformController.platforms[i].DrawSprite(graphics);
            if (PlatformController.bullets.Count > 0)
                for (int i = 0; i < PlatformController.bullets.Count; i++)
                    PlatformController.bullets[i].DrawSprite(graphics);
            if (PlatformController.enemies.Count > 0)
                for (int i = 0; i < PlatformController.enemies.Count; i++)
                    PlatformController.enemies[i].DrawSprite(graphics);
            if (PlatformController.bonuses.Count > 0)
                for (int i = 0; i < PlatformController.bonuses.Count; i++)
                    PlatformController.bonuses[i].DrawSprite(graphics);
            form1.player.DrawSprite(graphics);
        }
    }
}
