using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    static class Sound
    {
        public static SoundEffect ButtonSelect { get; private set; }
        public static SoundEffect GunShot { get; private set; }

        public static SoundEffect Explosion { get; private set; }
        public static SoundEffect WilhelmScream { get; private set; }

        public static Song Music { get; private set; }

        public static void Load(ContentManager content)
        {
            // Sound Effects
            ButtonSelect = content.Load<SoundEffect>("Sound/Select");
            GunShot = content.Load<SoundEffect>("Sound/GunShoot");
            Explosion = content.Load<SoundEffect>("Sound/Explosion");
            WilhelmScream = content.Load<SoundEffect>("Sound/wilhelmScream");
            // Song
            Music = content.Load<Song>("Sound/DanseMacabre");
        }
    }
}
