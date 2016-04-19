using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPGEx;

namespace DefendTheBase
{
    /// <summary>
    /// Manages things like spritesheet and simple pixel effects
    /// </summary>
    ///
    public static class EffectManager
    {
        public enum EffectEnums
        {
            Blood,
            Explosion
        }

        static List<Effect> EffectList = new List<Effect>();
        public static List<int> EffectIDs = new List<int>();

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

        public static void Add(Effect effect)
        {
            EffectList.Add(effect);
        }

        public static void Remove(int effectID)
        {
            int index = EffectList.FindIndex(item => string.Compare(item.ID.ToString(), effectID.ToString(), 0) == 0);

            if (index >= 0)
                EffectList.RemoveAt(index);

            int index2 = EffectIDs.FindIndex(item => string.Compare(item.ToString(), effectID.ToString(), 0) == 0);

            if (index2 >= 0)
                EffectIDs.RemoveAt(index);
        
        }


        //create a call for an effect
        public static void EffectCall(EffectManager.EffectEnums Effect, Vector2 Location, bool spritesheet)
        {
            Add(new Effect(Effect, Location, spritesheet));
        }


        public static void Update(GameTime gt)
        {
            foreach (Effect effect in EffectList)
            {
                if (effect.active)
                    effect.Update(gt);

                else
                {
                    EffectManager.Remove(effect.ID);
                    break;
                }
            }
        }

        public static void Draw(SpriteBatch sb)
        {

            foreach (Effect effect in EffectList)
            {
                effect.Draw(sb);
                
            }
        }
    }

    
public class Effect
{
    public int ID;
    float effectLength;

    Texture2D effectTex;
    Vector2 location;
    bool spriteSheet;
    public bool active = true;
    UiTimer Timer;

    int bloodSheetPos = GameManager.rnd.Next(0, 17) * 15;
    
    public Effect(EffectManager.EffectEnums Effect, Vector2 Location, bool Spritesheet)
    {
        effectTex = GetEffect(Effect);
        effectLength = GetEffectLength(Effect);

        location = Location;
        spriteSheet = Spritesheet;
        Timer = new UiTimer(effectLength);
    
    }

    Texture2D GetEffect(EffectManager.EffectEnums Effect)
    {
        switch(Effect)
        {
            case EffectManager.EffectEnums.Blood:
                return Art.BloodSplats;
        }

        return null;
    }

    float GetEffectLength(EffectManager.EffectEnums Effect)
    {
        switch(Effect)
        {
            case EffectManager.EffectEnums.Blood:
                return 5000f;
        }

        return 0f;
    }

    public void Update(GameTime gt)
    {
        if (!Timer.GetActive)
            Timer.ActivateTimer();

        if (Timer.GetActive)
            Timer.TimerUpdate(gt);

        if (Timer.TimeReached())
            active = false;

        if (spriteSheet)
        { 
        
        
        }
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(effectTex, location, new Rectangle(bloodSheetPos, 0, 14, 15), Color.White);
    }


    int CreateID()
    {
        bool IsUnique = false;
        int ID = GameManager.rnd.Next(0, 10) + GameManager.rnd.Next(0, 100000);

        while (!IsUnique)
            {
                ID = GameManager.rnd.Next(0, 10) + GameManager.rnd.Next(0, 100000);

                foreach (int id in EffectManager.EffectIDs)
                    if (id == ID)
                    {
                        IsUnique = false;
                        break;
                    }
                    else
                    {
                        IsUnique = true;
                        return ID;
                    }

                if (EffectManager.EffectIDs.Count() == 0)
                {
                    IsUnique = true;
                    return ID;
                }
            }

        return 0;
        
    }


}


}





