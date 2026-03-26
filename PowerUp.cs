// PowerUp.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    public class PowerUp
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle rect;
        public bool active = true;

        float speed = 100f;

        public PowerUp(Texture2D texture, Vector2 position)
        {s
            this.texture = texture;
            this.position = position;
            this.rect = new Rectangle((int)position.X, (int)position.Y, 48, 48);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += speed * dt; // aşağı iner
            if (position.Y > 620) active = false; // ekran dışına çıkınca sil
            rect = new Rectangle((int)position.X, (int)position.Y, 48, 48);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 48, 48), Color.White);
        }
    }
}