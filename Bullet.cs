// Bullet.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    public class Bullet
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle rect;
        public bool active = true;

        float speed = 400f;

        public Bullet(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            this.rect = new Rectangle((int)position.X, (int)position.Y, 16, 32);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y -= speed * dt;
            if (position.Y < -32) active = false;
            rect = new Rectangle((int)position.X, (int)position.Y, 16, 32);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 16, 32), Color.White);
        }
    }
}