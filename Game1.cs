using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders;
using System;
using System.Collections.Generic;
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
        private int anim4FrameCounter;
        Texture2D spaceBackground;
        private List<Enemy> enemies;
        private Vector2 defaultEnemyPosition;
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
            enemies = new List<Enemy>() {new Enemy(this, defaultEnemyPosition), new Enemy(this, defaultEnemyPosition + new Vector2(35,0)), new Enemy(this, defaultEnemyPosition - new Vector2(35, 0)) };
            spaceBackground = this.Content.Load<Texture2D>("SPACE BACKGROUND");
            ship.changeTexture(this.Content.Load<Texture2D>("JetFighter-1"));
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            Keys[] previousState = keyboardState;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState().GetPressedKeys();
            exhaustAnimLocation = new Vector2(ship.Position.X + ship.ShipTexture.Width / 2.5f, ship.Position.Y + ship.ShipTexture.Height);
            
            

            
            ship.move(keyboardState, border);
            ship.Update(gameTime);
            foreach (var bullet in ship.Bullets)
            {
                bullet.Update(gameTime);
                if (keyboardState.Contains(Keys.C))
                {
                    Console.WriteLine();
                }
                if (bullet.Position.Y < 75)
                {
                    foreach (var enemy in enemies)
                    {
                        
                    }
                }
            }

#if DEBUG
            
#endif

            //enemy anim frame time 
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
                foreach (var bullet in ship.Bullets)
                {
                    bullet.CheckCollisions(enemy.HitBox);
                    //Debug.WriteLine($"Bullet Y: {bullet.Position.Y}");
                }

            }

            if (keyboardState.Contains(Keys.Space) && !previousState.Contains(Keys.Space)) 
            {
                ship.shoot(ShipBulletTypes.Bullet);
            }


            if (!border.Contains(ship.Position))
            {
                ship.outBorder();
            }
            else 
            {
                ship.inBorder();
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
                foreach (var bullet in ship.Bullets) 
                {
                    _spriteBatch.Draw(bullet.Texture, bullet.Position, null,Color.White, 0f, default, 1f, 0, 0.1f);
                }
                foreach (var enemy in enemies) 
                {
                    _spriteBatch.Draw(enemy.Texture, enemy.Position, enemy.CurrentFrame,Color.White,0f,default,1f,0,0.2f);
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
