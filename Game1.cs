using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Space_Invaders
{
    public class Game1 : Game
    {
        #region fields
        Ships ship;
        public static float matrixScale = 4;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MouseState mouseState;
        Keys[] keyboardState;
        private Rectangle border;
        private Vector2 exhaustAnimLocation;
        Texture2D spaceBackground;
        private List<Enemy> enemies;
        private Vector2 defaultEnemyPosition;
        private Vector2 spaceBetweenEnemies;
        private int millisecondsPerEnemyMovement = 900;
        private int millisecondsSinceLastMovement;
        private Vector2 healthPostion = new Vector2(164,225);
        private bool gameOver = false;
        private Texture2D gameLost;
        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spaceBetweenEnemies = new Vector2(45,0);
            defaultEnemyPosition = new Vector2(165,57);
            ship = new Ships(this);
            border = new Rectangle(new Point(0,100),new Point(340,114));
            _graphics.PreferredBackBufferHeight = 960;
            _graphics.PreferredBackBufferWidth = 1440;
            _graphics.ApplyChanges();
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            gameLost = this.Content.Load<Texture2D>("YOU SUCK");
            enemies = new List<Enemy>() {new Enemy(this, defaultEnemyPosition), new Enemy(this, defaultEnemyPosition + spaceBetweenEnemies), new Enemy(this, defaultEnemyPosition - spaceBetweenEnemies), new Enemy(this, defaultEnemyPosition + spaceBetweenEnemies * 2), new Enemy(this, defaultEnemyPosition - spaceBetweenEnemies * 2) };
            spaceBackground = this.Content.Load<Texture2D>("SPACE BACKGROUND");
            ship.changeTexture(this.Content.Load<Texture2D>("JetFighter-1"));
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MediaPlayer.Play(this.Content.Load<Song>("Cool With You"));
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Volume = 0.05f;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            List<Bullet> bulletsToRemove = new List<Bullet>();
            Random random = new Random();
            List<Enemy> enemiesToRemove = new List<Enemy>();
            Keys[] previousState = keyboardState;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState().GetPressedKeys();
            exhaustAnimLocation = new Vector2(ship.Position.X + ship.ShipTexture.Width / 2.5f, ship.Position.Y + ship.ShipTexture.Height);

            Debug.WriteLine($"X:{mouseState.X},  Y:{mouseState.Y}");

            if (ship.HealthStage != 0)
            {
                ship.move(keyboardState, border);
                ship.Update(gameTime);


                millisecondsSinceLastMovement += gameTime.ElapsedGameTime.Milliseconds;
                if (millisecondsSinceLastMovement > millisecondsPerEnemyMovement)
                {
                    int displacement;
                    if (random.Next(2) == 1)
                    {
                        displacement = -4;
                    }
                    else
                        displacement = 4;
                    foreach (var enemy in enemies)
                    {
                        enemy.moveAround(new Vector2(displacement, 0));
                    }
                    millisecondsSinceLastMovement = 0;
                }

                foreach (var enemy in enemies)
                {
                    enemy.shoot(this, gameTime);
                    foreach (var bullet in enemy.Bullets)
                    {

                        if (bullet.CheckCollisions(ship.HitBox))
                        {
                            if (!bullet.HitDamageComplete)
                            {
                                ship.takeDamage(1);
                                bullet.DamageComplete();
                            }

                            bulletsToRemove.Add(bullet);
                        }

                    }
                }


                foreach (var enemy in enemies)
                {
                    enemy.Update(gameTime);
                    foreach (var bullet in ship.Bullets)
                    {
                        if (bullet.CheckCollisions(enemy.HitBox))
                        {
                            enemy.AddBulletOnHit(bullet);
                            enemiesToRemove.Add(enemy);
                        }

                        //Debug.WriteLine($"Bullet Y: {bullet.Position.Y}");
                    }
                    enemy.RemoveHitBullets();

                }

                foreach (var enemy in enemiesToRemove)
                {
                    enemies.Remove(enemy);
                }

                if (keyboardState.Contains(Keys.Space) && !previousState.Contains(Keys.Space))
                {
                    ship.shoot(ShipBulletTypes.Bullet, this);
                }


                if (!border.Contains(ship.Position))
                {
                    ship.outBorder();
                }
                else
                {
                    ship.inBorder();
                }
            }
            
            
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here

            base.Update(gameTime);
        } 

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target,Color.Black, 4f, 1);
            try
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(matrixScale), sortMode: SpriteSortMode.FrontToBack);
                _spriteBatch.Draw(spaceBackground, new Vector2(0,0), null,Color.White, 0f, default, 1f, SpriteEffects.None,0);
                _spriteBatch.Draw(ship.ShipTexture, ship.Position, null,Color.White, 0f , default, 1f, SpriteEffects.None, 0.2f);
                _spriteBatch.Draw(ship.ExhaustTexture, exhaustAnimLocation, Color.White);
                _spriteBatch.Draw(ship.HealthTexture, healthPostion, null, Color.White, 0f, default, 0.23f, 0, 0.21f);
                foreach (var bullet in ship.Bullets) 
                {
                    if (bullet.Texture != null)
                    {
                        _spriteBatch.Draw(bullet.Texture, bullet.Position, null, Color.White, 0f, default, 1f, 0, 0.15f);
                    }
                    
                }
                foreach (var enemy in enemies) 
                {
                    _spriteBatch.Draw(enemy.Texture, enemy.Position, enemy.CurrentFrame,Color.White,0f,default,1f,0,0.16f);
                }
                foreach (var enemy in enemies)
                {
                    foreach (var bullet in enemy.Bullets)
                    {
                        if (bullet.Hit)
                        {
                            _spriteBatch.Draw(bullet.Texture, bullet.Position, null, Color.White, 0f, default, 1f, 0, 0.15f);
                        }
                        else
                            _spriteBatch.Draw(bullet.Texture, bullet.Position, null, Color.White, 0f, default, 0.32f, 0, 0.15f);
                    }
                }
                if (ship.HealthStage == 0)
                {
                    _spriteBatch.Draw(gameLost, new Vector2(100.25f,80), null,Color.White,0f,default,1f,0,0.22f);
                }
            }
            finally 
            {
                _spriteBatch.End();
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
