using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DefendTheBase
{
    /// <summary>
    /// Manages things like spritesheet and simple pixel effects
    /// </summary>
    public static class EffectManager
    {

        //Saves time not ahving to type these loops for each sheet, BUT each sheet will have to be either horizontal or vertical not both
        public static void spriteSheetUpdate(ref int frame, ref float elasped, float target, int totalFrames, GameTime gt)
        {
            elasped += gt.ElapsedGameTime.Milliseconds;

            if (elasped >= target)
            {
                frame ++;
                elasped = 0;

                if (frame > totalFrames)
                    frame = 0;
            }
        }





    }
}
