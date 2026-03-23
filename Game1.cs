// Game1.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerTexture;
        Texture2D enemyTexture;
        Texture2D bulletTexture;

        Player player;
        List<Enemy> enemies = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();

        float bulletCooldown = 0f;
        float enemySpawnTimer = 0f;
        float enemySpawnInterval = 1.5f;

        int score = 0;
        bool gameOver = false;

        System.Random random = new System.Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTexture = Content.Load<Texture2D>("player1"); // player1.png
            enemyTexture = Content.Load<Texture2D>("enemy");
            bulletTexture = Content.Load<Texture2D>("bullet");

            player = new Player(playerTexture, new Vector2(368, 500));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            if (gameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                    RestartGame();
                return;
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            player.Update(gameTime);

            bulletCooldown -= dt;
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && bulletCooldown <= 0f)
            {
                bullets.Add(new Bullet(bulletTexture,
                    new Vector2(player.position.X + 24, player.position.Y)));
                bulletCooldown = 0.3f;
            }

            enemySpawnTimer += dt;
            if (enemySpawnTimer >= enemySpawnInterval)
            {
                int x = random.Next(0, 736);
                enemies.Add(new Enemy(enemyTexture, new Vector2(x, -64)));
                enemySpawnTimer = 0f;
            }

            foreach (var b in bullets) b.Update(gameTime);
            foreach (var e in enemies) e.Update(gameTime);

            foreach (var b in bullets)
            {
                foreach (var e in enemies)
                {
                    if (b.active && e.alive && b.rect.Intersects(e.rect))
                    {
                        b.active = false;
                        e.alive = false;
                        score += 10;
                    }
                }
            }

            foreach (var e in enemies)
            {
                if (e.alive && e.rect.Intersects(player.rect))
                {
                    gameOver = true;
                }
            }

            bullets.RemoveAll(b => !b.active);
            enemies.RemoveAll(e => !e.alive);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            if (!gameOver)
            {
                player.Draw(_spriteBatch);
                foreach (var b in bullets) b.Draw(_spriteBatch);
                foreach (var e in enemies) e.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void RestartGame()
        {
            bullets.Clear();
            enemies.Clear();
            score = 0;
            enemySpawnTimer = 0f;
            bulletCooldown = 0f;
            player = new Player(playerTexture, new Vector2(368, 500));
            gameOver = false;
        }
    }
}
