using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Vector2 Position { get { return _position; } }

        public Texture2D Texture { get { return _texture; } }

        public Rectangle CurrentFrame { get { return currentFrame; } }

        public Rectangle HitBox{ get { return hitBox; } }
        #endregion


        #region constructor
        public Enemy(Game1 game, Vector2 position)
        {
            _texture = game.Content.Load<Texture2D>("enemies");
            _position = position;
            

            _rectangles.Add(new Rectangle(0,0,_texture.Bounds.Width / 3, _texture.Bounds.Height));
            _rectangles.Add(new Rectangle((_texture.Bounds.Width / 3) * 2, 0, _texture.Bounds.Width / 3, _texture.Bounds.Height));
            locationPoint = new Point((int)position.X - 4, (int)position.Y);
            hitBox = new Rectangle(locationPoint, new Point(currentFrame.Width, currentFrame.Height));
           
            currentFrame = _rectangles[0];
        }
        #endregion

        public void Update(GameTime gameTime)
        {
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
                hitBox = new Rectangle(locationPoint, new Point(currentFrame.Width, currentFrame.Height));
            }
        }

    }
}
