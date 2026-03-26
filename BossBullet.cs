// BossBullet.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    public class BossBullet
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 direction;
        public Rectangle rect;
        public bool active = true;

        float speed = 250f;

        public BossBullet(Texture2D texture, Vector2 position, Vector2 direction)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.rect = new Rectangle((int)position.X, (int)position.Y, 16, 32);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += direction * speed * dt;

            // ekran dışına çıkınca sil
            if (position.Y > 620 || position.X < -32 || position.X > 832)
                active = false;

            rect = new Rectangle((int)position.X, (int)position.Y, 16, 32);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 16, 32), Color.Red);
        }
    }
}