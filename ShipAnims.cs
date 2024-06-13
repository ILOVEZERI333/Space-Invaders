using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class ShipAnims
    {
        #region fields and properties
        private List<Texture2D> frames;

        public List<Texture2D> rocketFrames
        {
            get { return frames; }
        }
        #endregion


        #region constructor
        public ShipAnims(Game1 game, Ships ship)
        {
            frames = new List<Texture2D>();
            for (int i = 0; i < 4; i++)
            {
                frames.Add(game.Content.Load<Texture2D>($"Exhaust-Normal-{i + 1}"));
            }
            
        }
        #endregion
    }
}
