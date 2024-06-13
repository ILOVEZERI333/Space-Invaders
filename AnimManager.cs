using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class AnimManager
    {
        #region fields and properties
        private int millisecondsPerFrame;
        private int timeSinceLastFrame;
        private List<Rectangle> frames = new List<Rectangle>();
        #endregion

        #region constructor
        public AnimManager(int milliPerFrame, List<Rectangle> frames) 
        {
            millisecondsPerFrame = milliPerFrame;
            this.frames = frames;
        }
        #endregion

        #region methods
        public void Update(GameTime gameTime)
        {
            
        }
        #endregion

    }
}
