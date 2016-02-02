using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PathFinding
{
    public class Art
    {
        public enum TrenchEnum
        { 
            Horizontal,
            Vertical,
            TUp,
            TDown,
            TLeft,
            TRight,
            EUp,
            EDown,
            ERight,
            ELeft,
            trenchX
        }

        Texture2D[] trenchTexs;
        Texture2D groundTexs;

        public Art()
        { 
            trenchTexs = new Texture2D[11];
        }

        public void Load(ContentManager content)
        {

            for (int i = 0; i < trenchTexs.Count(); i++ )
                trenchTexs[i] = content.Load<Texture2D>("trenchArt/trench_" + i);

            groundTexs = content.Load<Texture2D>("dirt");
        }

        public Texture2D groundTextureReturn
        {
              get {return groundTexs;}
        }   

        public Texture2D getTrenchTex(TrenchEnum trenchEnum)
        {
            Texture2D a = trenchTexs[0];

            switch(trenchEnum)
            {

                case TrenchEnum.EUp:
                    return trenchTexs[0];
                    break;

                case TrenchEnum.ERight:
                    return trenchTexs[1];
                    break;

                case TrenchEnum.ELeft:
                    return trenchTexs[2];
                    break;

                case TrenchEnum.EDown:
                    return trenchTexs[3];
                    break;

                case TrenchEnum.TUp:
                    return trenchTexs[4];
                    break;

                case TrenchEnum.TRight:
                    return trenchTexs[5];
                    break;

                case TrenchEnum.TLeft:
                    return trenchTexs[6];
                    break;

                case TrenchEnum.TDown:
                    return trenchTexs[7];
                    break;

                case TrenchEnum.trenchX:
                    return trenchTexs[8];
                    break;

                case TrenchEnum.Vertical:
                    return trenchTexs[9];
                    break;

                case TrenchEnum.Horizontal:
                    return trenchTexs[10];
                    break;


            }

            return a;
        }

    }
}
