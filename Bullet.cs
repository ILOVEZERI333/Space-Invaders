﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Bullet
    {
        #region fields and properties
        private Vector2 _position;
        private Vector2 _oldPosition;
        private float _velocity;
        private List<Texture2D> texture2Ds = new List<Texture2D>();
        private Texture2D _texture;
        private int counter = 0;
        private bool hit = false;
        private List<Texture2D> bulletHitTextures = new List<Texture2D>();
        private int millisecondsPerFrame = 300;
        private AnimManager animationManager;


        public Vector2 Position { get { return _position; } }

        public Vector2 OldPosition { get { return _oldPosition; } }

        public Texture2D Texture { get { return _texture; } }

        public bool Hit { get { return hit; } }
        #endregion

        #region events
        
        #endregion

        #region constructor
        public Bullet(Ships ship, Game1 game)
        {
            _position = ship.Position + new Vector2(ship.ShipTexture.Width/2.1f);
            _oldPosition = _position;
            _velocity = -2;
            texture2Ds.Add(game.Content.Load<Texture2D>("Bullets-1"));
            texture2Ds.Add(game.Content.Load<Texture2D>("Bullets-2"));
            _texture = texture2Ds[0];
            bulletHitTextures.Add(game.Content.Load<Texture2D>("Impact-Ricochet-1"));
            bulletHitTextures.Add(game.Content.Load<Texture2D>("Impact-Ricochet-2"));
            bulletHitTextures.Add(game.Content.Load<Texture2D>("Impact-Ricochet-3"));
            animationManager = new AnimManager(300, texture2Ds);
        }
        #endregion

        #region methods
        public void Update(GameTime gameTime) 
        {
            _texture = animationManager.Update(gameTime);
            //if animation is on last frame,stop
            if (_texture == bulletHitTextures[2])
            {
            animationManager.Stop();
            }
            _position += new Vector2(0, _velocity);
        }

        public void CheckCollisions(Rectangle hitBox)
        {
            if (hitBox.Contains(_position))
            {
                hit = true;
                _velocity = 0;
                animationManager = new AnimManager(400, bulletHitTextures);
            }
        }


        #endregion
    }
}
