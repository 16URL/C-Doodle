using System.Drawing;

namespace C__Doodle.Classes
{
    public class Player
    {
        public Physics physics;
        public Image sprite;
        public Player()
        {
            sprite = Properties.Resources.Personage;
            physics = new Physics(new PointF(100, 350), new Size(60, 60));
        }
        public void DrawSprite(Graphics g)
        {
            g.DrawImage(sprite,physics.transform.position.X,
                physics.transform.position.Y,
                physics.transform.size.Width, 
                physics.transform.size.Height); 
        }
    }
}