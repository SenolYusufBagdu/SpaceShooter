// Boss.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class Boss
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle rect;
        public bool alive = true;
        public int health = 10;

        float speed = 150f;
        float direction = 1f;

        public List<BossBullet> bossBullets = new List<BossBullet>();
        Texture2D bulletTexture;
        float shootTimer = 0f;
        float shootInterval; // her boss'ta azalır

        public Boss(Texture2D texture, Texture2D bulletTexture, Vector2 position, float shootBonus = 0f)
        {
            this.texture = texture;
            this.bulletTexture = bulletTexture;
            this.position = position;
            // baz ateş aralığı 1.5, her boss'ta 0.3 azalır (min 0.4)
            this.shootInterval = System.Math.Max(0.4f, 1.5f - shootBonus);
            this.rect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position.X += speed * direction * dt;
            if (position.X > 672) direction = -1f;
            if (position.X < 0) direction = 1f;

            rect = new Rectangle((int)position.X, (int)position.Y, 128, 128);

            shootTimer += dt;
            if (shootTimer >= shootInterval)
            {
                Vector2 bossCenter = new Vector2(position.X + 64, position.Y + 128);
                Vector2 playerCenter = new Vector2(playerPosition.X + 32, playerPosition.Y + 32);
                Vector2 dir = playerCenter - bossCenter;
                dir.Normalize();
                bossBullets.Add(new BossBullet(bulletTexture, bossCenter, dir));
                shootTimer = 0f;
            }

            foreach (var bb in bossBullets) bb.Update(gameTime);
            bossBullets.RemoveAll(bb => !bb.active);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 128, 128), Color.White);

            // can barı
            Texture2D red = new Texture2D(sb.GraphicsDevice, 1, 1);
            Texture2D green = new Texture2D(sb.GraphicsDevice, 1, 1);
            red.SetData(new[] { Color.Red });
            green.SetData(new[] { Color.Green });

            int barWidth = 128;
            int healthWidth = (int)(barWidth * (health / 10f));
            sb.Draw(red, new Rectangle((int)position.X, (int)position.Y - 15, barWidth, 8), Color.White);
            sb.Draw(green, new Rectangle((int)position.X, (int)position.Y - 15, healthWidth, 8), Color.White);

            foreach (var bb in bossBullets) bb.Draw(sb);
        }
    }
}
