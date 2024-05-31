using System;
using System.Collections.Generic;
using System.Drawing;

namespace C__Doodle.Classes
{
    public static class PlatformController
    {
        public static List<Platform> platforms;
        public static List<Bullet> bullets = new List<Bullet>();
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Bonus> bonuses = new List<Bonus>();
        public static int startPlatformPosY = 500;
        public static int score = 0;

        public static void AddPlatform(PointF position)
        {
            Platform platform = new Platform(position);
            platforms.Add(platform);
        }

        public static void GenerateStartSequence()
        {
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var x = random.Next(0, 270);
                var y = random.Next(50, 60);
                startPlatformPosY -= y;
                PointF position = new PointF(x, startPlatformPosY);
                Platform platform = new Platform(position);
                platforms.Add(platform);
                startPlatformPosY -= 50;
            }
        }

        public static void CreateEnemy(Platform platform)
        {
            Random random = new Random();
            var enemyType = random.Next(1, 5);
            switch (enemyType)
            {
                case 1:
                    var enemy = new Enemy(new PointF(platform.transform.position.X + (platform.sizeX / 2) -30, platform.transform.position.Y - 50), enemyType);
                    enemies.Add(enemy);
                    break;
                case 2:
                    enemy = new Enemy(new PointF(platform.transform.position.X + (platform.sizeX / 2)-30, platform.transform.position.Y - 50), enemyType);
                    enemies.Add(enemy);
                    break;
                case 3:
                    enemy = new Enemy(new PointF(platform.transform.position.X + (platform.sizeX / 2) - 30, platform.transform.position.Y - 53), enemyType);
                    enemies.Add(enemy);
                    break;
                case 4:
                    enemy = new Enemy(new PointF(platform.transform.position.X + (platform.sizeX / 2) - 30, platform.transform.position.Y - 53), enemyType);
                    enemies.Add(enemy);
                    break;
            }
        }

        public static void CreateBullet(PointF pos)
        {
            var bullet = new Bullet(pos);
            bullets.Add(bullet);
        }

        public static void CreateBonus(Platform platform)
        {
            Random random = new Random();
            var bonusType = random.Next(1, 3);
            switch (bonusType)
            {
                case 1:
                    var bonus = new Bonus(new PointF(platform.transform.position.X + (platform.sizeX / 2)-5 , platform.transform.position.Y -9), bonusType);
                    bonuses.Add(bonus);
                    break;
                case 2:
                    bonus = new Bonus(new PointF(platform.transform.position.X + (platform.sizeX / 2) , platform.transform.position.Y-10), bonusType);
                    bonuses.Add(bonus);
                    break;
            }
        }

        public static void GenerateRandomPlatform()
        {
            ClearPlatforms();
            Random random = new Random();
            var x = random.Next(0, 270);
            PointF position = new PointF(x, startPlatformPosY);
            Platform platform = new Platform(position);
            platforms.Add(platform);
            var enemyCreationChance = random.Next(1, 10);
            if (enemyCreationChance == 1)
                CreateEnemy(platform);
            var bonusCreationChance = random.Next(1, 15);
            if (bonusCreationChance == 1)
                CreateBonus(platform);
        }

        public static void ClearPlatforms()
        {
            for (int i = 0;i < platforms.Count; i++)
                if (platforms[i].transform.position.Y >=700)
                    platforms.RemoveAt(i);
            for (int i = 0; i < enemies.Count; i++)
                if (enemies[i].physics.transform.position.Y >= 700)
                    enemies.RemoveAt(i);
            for (int i = 0; i < bonuses.Count; i++)
                if (bonuses[i].physics.transform.position.Y >= 700)
                    bonuses.RemoveAt(i);
        }

        public static void RemoveEnemy(int i)
        {
            enemies.RemoveAt(i);
        }

        public static void RemoveBullet(int i)
        {
            bullets.RemoveAt(i);
        }
    }
}
