﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private List<Texture2D> frames = new List<Texture2D>();
        private int indexer;
        private bool active = true;
        #endregion

        #region constructor
        public AnimManager(int milliPerFrame, List<Texture2D> frames) 
        {
            millisecondsPerFrame = milliPerFrame;
            this.frames = frames;
        }
        #endregion

        #region methods

        public Texture2D? Update(GameTime gameTime)
        {
            if (active)
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (millisecondsPerFrame < timeSinceLastFrame)
                {
                    indexer++;
                    if (indexer == frames.Count - 1)
                    {
                        indexer = 0;
                    }
                    timeSinceLastFrame = 0;
                }

                return frames[indexer];
            }
            else
                //return a rectangle that is basically nothing (null makes .Draw method draw entire sprite)
                return null;
            
        }

        public void Stop()
        {
            active = false;
        }
        #endregion

    }
}
