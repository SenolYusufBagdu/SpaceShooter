// Player.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    public class Player
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle rect;
        public bool alive = true;

        float speed = 250f;

        public Player(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            this.rect = new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ks.IsKeyDown(Keys.Right)) position.X += speed * dt;
            if (ks.IsKeyDown(Keys.Left)) position.X -= speed * dt;
            if (ks.IsKeyDown(Keys.Up)) position.Y -= speed * dt;
            if (ks.IsKeyDown(Keys.Down)) position.Y += speed * dt;

            if (position.X < 0) position.X = 0;
            if (position.X > 736) position.X = 736;
            if (position.Y < 0) position.Y = 0;
            if (position.Y > 536) position.Y = 536;

            rect = new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 64, 64), Color.White);
        }
    }
}