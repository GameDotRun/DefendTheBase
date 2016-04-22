using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public static class TroopManager
    {
        static List<Troop> Troops = new List<Troop>();
        static List<string> TroopIDs = new List<string>();

        /// <summary>
        /// Destroys enemies and cleans up references in other lists of said enemy
        /// </summary>
        public static void DestroyTroop(int total)
        {
            int totalKill = Troops.Count - 1 - total;

            for(int i = Troops.Count - 1; i > totalKill; i--)
            {
                string tid = Troops[i].troopID;
                TroopListener.RemoveTroop(tid);

                int index = Troops.FindIndex(item => string.Compare(item.troopID, tid, 0) == 0);

                if (index >= 0)
                    Troops.RemoveAt(index);

                int index2 = TroopIDs.FindIndex(item => string.Compare(item, tid, 0) == 0);

                if (index2 >= 0)
                    TroopIDs.RemoveAt(index2);
            }
        }

        public static void SpawnTroop()
        {
            Troops.Add(new infantry(CreateID()));
        }

        public static void Update(GameTime gt)
        {
            foreach (Troop troop in Troops)
                troop.Update(gt);
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (Troop troop in Troops)
            {
                troop.Draw(sb);
            }
        }

        static string CreateID()
        {
            bool IsUnique = false;
            string ID = "";
            while (!IsUnique)
            {
                ID = GameManager.rnd.Next(0, 10).ToString() + GameManager.rnd.Next(0, 100000).ToString();

                foreach (string id in TroopIDs)
                    if (id == ID)
                    {
                        IsUnique = false;
                        break;
                    }
                    else IsUnique = true;

                if (TroopIDs.Count() == 0)
                    IsUnique = true;
            }

            TroopIDs.Add(ID);

            return ID;
        }


    }
}
