using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Space_Invaders;

namespace SpaceInvaders
{
    internal class Enemy
    {
        #region fields and properties
        private Vector2 _position;
        private Texture2D _texture;
        private List<Rectangle> _rectangles = new();
        private Rectangle currentFrame;
        private int timeSinceLastFrame;
        private int millisecondsPerFrame = 350;
        private int anim2FrameCounter;
        private Rectangle hitBox;
        private Point locationPoint;
        private List<Bullet> bullets = new List<Bullet>();
        private List<Texture2D> bulletTextures = new List<Texture2D>();
        private List<Texture2D> bulletHitTextures = new List<Texture2D>();
        private AnimManager animationManager;
        private int millisecondsPerShootingInterval = 750;
        private int millisecondsSinceLastInterval;
        private int millisecondsSinceLastMovement = 0;
        private List<Bullet> bulletsToRemove = new List<Bullet>();

        public Vector2 Position { get { return _position; } }

        public List<Bullet> Bullets { get { return bullets; } }

        public Texture2D Texture { get { return _texture; } }

        public Rectangle CurrentFrame { get { return currentFrame; } }

        public Rectangle HitBox{ get { return hitBox; } }
        #endregion




        public void shoot(Game1 game, GameTime gameTime)
        {
           Random random = new Random();
           millisecondsSinceLastInterval += gameTime.ElapsedGameTime.Milliseconds;

            if (millisecondsSinceLastInterval > millisecondsPerShootingInterval)
            {
                if (random.Next(1, 11) > 6)
                {
                    bullets.Add(new Bullet(this, game.Content.Load<Song>("roblox explosion"), bulletTextures, bulletHitTextures));
                }
                millisecondsSinceLastInterval = 0;

            }
            
          
           
        }

        public bool OutOfBounds(Rectangle border)
        {
            if (_position.X + _texture.Width >= border.X + border.Width)
            {
                return true;
            }
            else if (_position.X <= border.X)
            {
                return true;
            }
            else return false;
        }

        public void moveAround(Vector2 displacement)
        {
            
            _position += displacement;
            hitBox.Offset(displacement);
                
        }

        public void RemoveHitBullets()
        {
            foreach (var bullet in bulletsToRemove)
            {
                if (bullets.Contains(bullet))
                { 
                    bullets.Remove(bullet);
                }
            }
        }

        public void AddBulletOnHit(Bullet bullet)
        {
            if (bullets.Contains(bullet))
            {
                bulletsToRemove.Add(bullet);
            }
        }

        #region constructor
        public Enemy(Game1 game, Vector2 position)
        {
            _texture = game.Content.Load<Texture2D>("enemies");
            _position = position;
            

            _rectangles.Add(new Rectangle(0,0,_texture.Bounds.Width / 3, _texture.Bounds.Height));
            _rectangles.Add(new Rectangle((_texture.Bounds.Width / 3) * 2, 0, _texture.Bounds.Width / 3, _texture.Bounds.Height));
            locationPoint = new Point((int)position.X - 4, (int)position.Y);
            hitBox = new Rectangle(locationPoint, new Point(currentFrame.Width, currentFrame.Height));
            bulletTextures.Add(game.Content.Load<Texture2D>("enemy bullet"));
            for (int i = 0; i < 8; i++) 
            {
                bulletHitTextures.Add(game.Content.Load<Texture2D>($"Explosion-{i + 1}"));
            }

            currentFrame = _rectangles[0];
            animationManager = new AnimManager(50, bulletHitTextures);
            animationManager.Stop();
            
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            //cannot use animation manager since animation is based on rectangles not sprites (1 sheet for animations rather than many seperate pngs)
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                anim2FrameCounter++;
                if (anim2FrameCounter == 2)
                {
                    anim2FrameCounter = 0;
                }
                currentFrame = _rectangles[anim2FrameCounter];
                locationPoint = new Point((int)_position.X - 4, (int)_position.Y);
                hitBox = new Rectangle(locationPoint, new Point(currentFrame.Width, currentFrame.Height));
            }
            foreach (var bullet in bullets) 
            {
                bullet.Update(gameTime, 1);
            }
            Bullet.CleanUpBullets(bullets);
        }
    }
}
