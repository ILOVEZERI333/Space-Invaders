using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Space_Invaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Bullet
    {
        #region fields and properties
        private Vector2 _position;
        private float _velocity;
        private List<Texture2D> texture2Ds = new List<Texture2D>();
        private Texture2D _texture;
        private bool hit = false;
        private List<Texture2D> bulletHitTextures = new List<Texture2D>();
        private int millisecondsPerFrame = 300;
        private AnimManager animationManager;
        private AnimManager hitAnimationManager;
        private bool firstTime = true;
        private bool finished;
        private Song hitSound;
        private bool hitDamageComplete = false;

        public Vector2 Position { get { return _position; } }

        public bool HitDamageComplete { get { return hitDamageComplete; } }

        public Texture2D Texture { get { return _texture; } }

        public bool Finished { get { return finished; } }

        public bool Hit {  get { return hit; } }

        public Song HitSound { get { return hitSound; } }
        #endregion

        #region events
        
        #endregion

        #region constructor
        public Bullet(Ships ship, Song hitSound,List<Texture2D> bulletTexture, List<Texture2D> bulletHitTextures)
        {
            _position = ship.Position + new Vector2(ship.ShipTexture.Width/2.1f);
            _velocity = -2;
            foreach (var texture in bulletTexture)
            {
                texture2Ds.Add(texture);
            }
            foreach (var texture in bulletHitTextures)
            {
                this.bulletHitTextures.Add(texture);
            }
            _texture = texture2Ds[0];
            this.hitSound = hitSound;
            hitAnimationManager = new AnimManager(175, bulletHitTextures);
            animationManager = new AnimManager(300, texture2Ds);
        }

        public Bullet(Enemy enemy, Song hitSound,List<Texture2D> bulletTexture, List<Texture2D> bulletHitTextures)
        {
            _position = enemy.Position + new Vector2(0, enemy.Texture.Height / 2.5f);
            _velocity = 1;
            foreach (var texture in bulletTexture)
            {
                texture2Ds.Add(texture);
            }
            foreach (var texture in bulletHitTextures)
            {
                this.bulletHitTextures.Add(texture);
            }
            _texture = texture2Ds[0];
            this.hitSound = hitSound;
            animationManager = new AnimManager(300, bulletTexture);
            hitAnimationManager = new AnimManager(200, bulletHitTextures);
        }
        #endregion

        #region methods
        public void Update(GameTime gameTime, int Yvelocity) 
        {
            _velocity = Yvelocity;
            if (!hit)
            {
                _texture = animationManager.Update(gameTime);
            }
            else
            {
                _velocity = 0;
                if (firstTime)
                {
                    _position += new Vector2(-2, Yvelocity * 6f);
                }
                _texture = hitAnimationManager.Update(gameTime);
                if (_texture == bulletHitTextures[2])
                {
                    hitAnimationManager.Stop();
                    finished = true;
                }
                firstTime = false;
            }
                
            //if animation is on last frame,stop
            _position += new Vector2(0, _velocity);
        }

        public void DamageComplete()
        {
            hitDamageComplete = true;
        }

        public static void CleanUpBullets(List<Bullet> bullets)
        {
            if (bullets.Count > 0)
            {
                for (int i = bullets.Count; i >= 0; i--)
                {
                    //if out of range dont do anything
                    if (i - 1 != -1)
                    {
                        if (bullets[i - 1].Hit)
                        {
                            MediaPlayer.Play(bullets[i - 1].HitSound);
                            //remove bullet from list if done with animation
                            if (bullets[i - 1].Finished)
                            {
                                bullets.Remove(bullets[i - 1]);
                            }

                        }
                    }

                }
            }
        }

        public bool CheckCollisions(Rectangle hitBox)
        {
            if (hitBox.Contains(_position))
            {
                hit = true;
                _velocity = 0;
                return true;
            }
            else return false;
        }


        #endregion
    }
}
