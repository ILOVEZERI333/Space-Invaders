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
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace SpaceInvaders
{
    internal class Ships
    {

        #region fields and properties
        private Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2 (621/Game1.matrixScale,757/Game1.matrixScale);
        private Texture2D shipTexture;
        private List<Texture2D> exhaustTextures = new List<Texture2D>();
        private int healthStage = 5;
        private List<Texture2D> healthTextures;
        private List<Bullet> shipBullets = new List<Bullet>();
        private float speed = 1.5f;
        public AnimManager animationManager;
        private Texture2D exhaustTexture;
        Game1 game;
        private bool isInBorder;
        List<Texture2D> bulletTextures = new List<Texture2D>();
        List<Texture2D> bulletHitTextures = new List<Texture2D>();
        private Rectangle hitBox;


        public Rectangle HitBox { get { return hitBox; } }

        public Texture2D ExhaustTexture{ get { return exhaustTexture; } }

        public List<Bullet> Bullets { get { return shipBullets; } }

        public Microsoft.Xna.Framework.Vector2 Position { get { return position; } }

        public Texture2D ShipTexture{ get { return shipTexture; } }

        public int HealthStage { get { return healthStage; } }

        public List<Texture2D> HealthTextures { get {  return healthTextures; } }

        public Texture2D bulletTexture { get { return bulletTexture; } }

        #endregion

        #region constructors

        public Ships(Game1 game)
        {
            this.game = game;
            exhaustTextures.Add( game.Content.Load<Texture2D>("Exhaust-Normal-1"));
            exhaustTextures.Add(game.Content.Load<Texture2D>("Exhaust-Normal-2"));
            exhaustTextures.Add(game.Content.Load<Texture2D>("Exhaust-Normal-3"));
            exhaustTextures.Add(game.Content.Load<Texture2D>("Exhaust-Normal-4"));
            bulletTextures.Add(game.Content.Load<Texture2D>("Bullets-1"));
            bulletTextures.Add(game.Content.Load<Texture2D>("Bullets-2"));
            bulletHitTextures.Add(game.Content.Load<Texture2D>("Impact-Ricochet-1"));
            bulletHitTextures.Add(game.Content.Load<Texture2D>("Impact-Ricochet-2"));
            bulletHitTextures.Add(game.Content.Load<Texture2D>("Impact-Ricochet-3"));
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = false;
            animationManager = new AnimManager(30, exhaustTextures);
            shipTexture = game.Content.Load<Texture2D>("JetFighter-1");
            hitBox = new Rectangle(new Point((int)position.X, (int)position.Y),new Point(shipTexture.Width - 3, shipTexture.Height));
        }

        //default ship will be created at spawn location with no powers

        #endregion

        #region methods

        public void shoot(ShipBulletTypes bullet, Game1 game) 
        {
            
            
            switch (bullet) 
            {
                case ShipBulletTypes.Bullet:
                    shipBullets.Add(new Bullet(this, game.Content.Load<Song>("8-bit explosion"),bulletTextures, bulletHitTextures));
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            hitBox = new Rectangle(new Point((int)position.X - 5, (int)position.Y), new Point(shipTexture.Width - 3, shipTexture.Height));
            exhaustTexture = animationManager.Update(gameTime);
            foreach (var bullet in shipBullets)
            {
                bullet.Update(gameTime, -2);
            }

            Bullet.CleanUpBullets(Bullets);
            

        }

        public void inBorder()
        {
            isInBorder = true;
        }

        public void outBorder()
        {
            isInBorder = false;
        }

        public void takeDamage(int stage)
        {
            if (healthStage - stage > 0) 
            {
                healthStage -= stage;
            }
            else 
            {
                healthStage = 0;
            }
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

