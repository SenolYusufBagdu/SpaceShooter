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
        Texture2D bossTexture;
        Texture2D heartTexture;
        Texture2D spaceTexture;

        Player player;
        List<Enemy> enemies = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();
        List<PowerUp> powerUps = new List<PowerUp>();
        Boss boss;

        float bulletCooldown = 0f;
        float enemySpawnTimer = 0f;
        float enemySpawnInterval = 1.5f;
        int enemiesKilled = 0;

        bool bossActive = false;
        int bossesDefeated = 0;
        int roundsCompleted = 0;

        float enemySpeedBonus = 0f;
        float bossShootBonus = 0f;

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
            playerTexture = Content.Load<Texture2D>("player1");
            enemyTexture = Content.Load<Texture2D>("enemy");
            bulletTexture = Content.Load<Texture2D>("bullet");
            bossTexture = Content.Load<Texture2D>("boss");
            heartTexture = Content.Load<Texture2D>("kalp");
            spaceTexture = Content.Load<Texture2D>("space");
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

            if (!bossActive)
            {
                enemySpawnTimer += dt;
                if (enemySpawnTimer >= enemySpawnInterval)
                {
                    int x = random.Next(0, 736);
                    enemies.Add(new Enemy(enemyTexture, new Vector2(x, -64), enemySpeedBonus));
                    enemySpawnTimer = 0f;
                }
            }

            foreach (var b in bullets) b.Update(gameTime);
            foreach (var e in enemies) e.Update(gameTime);
            foreach (var p in powerUps) p.Update(gameTime);

            if (bossActive && boss != null && boss.alive)
                boss.Update(gameTime, player.position);

            bool spawnBoss = false;
            bool bossKilled = false;

            foreach (var b in bullets)
            {
                foreach (var e in enemies)
                {
                    if (b.active && e.alive && b.rect.Intersects(e.rect))
                    {
                        b.active = false;
                        e.alive = false;
                        score += 10;
                        enemiesKilled++;
                        if (enemiesKilled >= 10 && !bossActive)
                            spawnBoss = true;
                    }
                }
            }

            if (bossActive && boss != null && boss.alive)
            {
                foreach (var b in bullets)
                {
                    if (b.active && boss.alive && b.rect.Intersects(boss.rect))
                    {
                        b.active = false;
                        boss.health--;
                        score += 50;
                        if (boss.health <= 0)
                        {
                            boss.alive = false;
                            bossKilled = true;
                        }
                    }
                }

                foreach (var bb in boss.bossBullets)
                {
                    if (bb.active && bb.rect.Intersects(player.rect))
                    {
                        bb.active = false;
                        player.health--;
                        if (player.health <= 0) gameOver = true;
                    }
                }
            }

            foreach (var e in enemies)
            {
                if (e.alive && e.rect.Intersects(player.rect))
                {
                    e.alive = false;
                    player.health--;
                    if (player.health <= 0) gameOver = true;
                }
            }

            // oyuncu vs kalp
            foreach (var p in powerUps)
            {
                if (p.active && p.rect.Intersects(player.rect))
                {
                    p.active = false;
                    if (player.health < 3)
                        player.health++;
                }
            }

            // mermi vs kalp
            foreach (var b in bullets)
            {
                foreach (var p in powerUps)
                {
                    if (b.active && p.active && b.rect.Intersects(p.rect))
                    {
                        b.active = false;
                        p.active = false;
                        if (player.health < 3)
                            player.health++;
                    }
                }
            }

            bullets.RemoveAll(b => !b.active);
            enemies.RemoveAll(e => !e.alive);
            powerUps.RemoveAll(p => !p.active);

            if (spawnBoss)
            {
                bossActive = true;
                enemies.Clear();
                boss = new Boss(bossTexture, bulletTexture,
                    new Vector2(336, 50), bossShootBonus);
            }

            if (bossKilled)
            {
                bossesDefeated++;
                roundsCompleted++;
                enemySpeedBonus += 20f;
                bossShootBonus += 0.3f;
                bossActive = false;
                enemiesKilled = 0;
                boss = null;
                enemies.Clear();

                // her boss öldüğünde, can azaldıysa kalp spawn et
                if (player.health < 3)
                {
                    int x = random.Next(50, 700);
                    powerUps.Add(new PowerUp(heartTexture, new Vector2(x, -48)));
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // arka plan
            _spriteBatch.Draw(spaceTexture,
                new Rectangle(0, 0, 800, 600), Color.White);

            if (!gameOver)
            {
                player.Draw(_spriteBatch);
                foreach (var b in bullets) b.Draw(_spriteBatch);
                foreach (var e in enemies) e.Draw(_spriteBatch);
                foreach (var p in powerUps) p.Draw(_spriteBatch);
                if (bossActive && boss != null && boss.alive)
                    boss.Draw(_spriteBatch);
            }
            else
            {
                Texture2D t = new Texture2D(GraphicsDevice, 1, 1);
                t.SetData(new[] { Color.White });
                _spriteBatch.Draw(t, new Rectangle(250, 250, 300, 80), Color.DarkRed);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void RestartGame()
        {
            bullets.Clear();
            enemies.Clear();
            powerUps.Clear();
            score = 0;
            enemiesKilled = 0;
            bossesDefeated = 0;
            roundsCompleted = 0;
            enemySpawnTimer = 0f;
            bulletCooldown = 0f;
            bossActive = false;
            gameOver = false;
            boss = null;
            enemySpeedBonus = 0f;
            bossShootBonus = 0f;
            player = new Player(playerTexture, new Vector2(368, 500));
        }
    }
}