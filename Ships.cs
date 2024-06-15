using Microsoft.Xna.Framework.Graphics;
using SharpDX.MediaFoundation;
using Space_Invaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    internal class Ships
    {

        #region fields and properties
        private Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2 (621/Game1.matrixScale,757/Game1.matrixScale);
        private Texture2D shipTexture;
        private List<Texture2D> exhaustTextures = new List<Texture2D>();
        private int healthPoints = 100;
        private List<Texture2D> healthTextures;
        private List<Bullet> shipBullets = new List<Bullet>();
        private float speed = 1.5f;
        private AnimManager animationManager;
        private Texture2D exhaustTexture;
        Game1 game;
        private bool isInBorder;


        public Texture2D ExhaustTexture{ get { return exhaustTexture; } }

        public List<Bullet> Bullets { get { return shipBullets; } }

        public Microsoft.Xna.Framework.Vector2 Position { get { return position; } }

        public Texture2D ShipTexture{ get { return shipTexture; } }

        public int HealthPoints { get { return healthPoints; } }

        public List<Texture2D> HealthTextures { get {  return healthTextures; } }

        public Texture2D bulletTexture { get { return bulletTexture; } }

        public bool IsWithinBorder { get { return isInBorder; } }
        #endregion

        #region constructors

        public Ships(Game1 game)
        {
            this.game = game;
            exhaustTextures.Add( game.Content.Load<Texture2D>("Exhaust-Normal-1"));
            exhaustTextures.Add(game.Content.Load<Texture2D>("Exhaust-Normal-2"));
            exhaustTextures.Add(game.Content.Load<Texture2D>("Exhaust-Normal-3"));
            exhaustTextures.Add(game.Content.Load<Texture2D>("Exhaust-Normal-4"));

            animationManager = new AnimManager(30, exhaustTextures);
        }

        //default ship will be created at spawn location with no powers

        #endregion

        #region methods

        public void shoot(ShipBulletTypes bullet) 
        {
            switch (bullet) 
            {
                case ShipBulletTypes.Bullet:
                    shipBullets.Add(new Bullet(this, game));
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            exhaustTexture = animationManager.Update(gameTime);
        }

        public void inBorder()
        {
            isInBorder = true;
        }

        public void outBorder()
        {
            isInBorder = false;
        }

        public void takeDamage(int damage)
        {
            
        }

        public void move(Keys[] state, Rectangle border)
        {
            if (isInBorder) 
            {
                foreach (var key in state)
                {
                    if (key == (Keys.A))
                    {
                        shipTexture = game.Content.Load<Texture2D>("JetFighter-3");
                        position += new Microsoft.Xna.Framework.Vector2(-speed, 0);
                    }
                    if (key == (Keys.D))
                    {
                        shipTexture = game.Content.Load<Texture2D>("JetFighter-2");
                        position += new Microsoft.Xna.Framework.Vector2(speed, 0);
                    }
                    if (key == Keys.S)
                    {
                        shipTexture = game.Content.Load<Texture2D>("JetFighter-1");
                        position += new Microsoft.Xna.Framework.Vector2(0, speed);
                    }
                    if (key == Keys.W)
                    {
                        shipTexture = game.Content.Load<Texture2D>("JetFighter-1");
                        position += new Microsoft.Xna.Framework.Vector2(0, -speed);
                    }
                }
            }
            else 
            {
                if (border.Top >= (int)position.Y)
                {
                    position += new Microsoft.Xna.Framework.Vector2(0, 1);
                }
                if (border.Left >= (int)position.X)
                {
                    position += new Microsoft.Xna.Framework.Vector2(1, 0);
                }
                if (border.Bottom <= (int)position.Y)
                {
                    position += new Microsoft.Xna.Framework.Vector2(0, -1);
                }
                if (border.Right <= (int)position.X)
                {
                    position += new Microsoft.Xna.Framework.Vector2(-1, 0);
                }

            }
        }

        public void changeTexture(Texture2D texture)
        {
            if (texture  != shipTexture) 
            {
                shipTexture = texture;
            }
        }
        #endregion


    }

    public enum ShipBulletTypes
    {
        Bullet, Flameshot,Missle
    }
}

