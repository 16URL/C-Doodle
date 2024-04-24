using System.Drawing;

namespace C__Doodle.Classes
{
    public class Enemy: Player
    {
        public Enemy(PointF pos, int type) 
        {
            switch (type)
            {
                case 1:
                    sprite = Properties.Resources.Monst;
                    physics = new Physics(pos, new Size(60, 60));
                    break;
                case 2:
                    sprite = Properties.Resources.Monster1;
                    physics = new Physics(pos, new Size(60, 60));
                    break;
            }
        }
    }
}
