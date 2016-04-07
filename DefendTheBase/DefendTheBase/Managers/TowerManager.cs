using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    //Aarons Shizzle Repurposed(Enemy->Tower):
    public static class TowerManager
    {
        public static string[] TypeIDs = { "Gun", "Rocket", "SAM", "Tesla" };
        static List<Tower> Towers = new List<Tower>();
        static List<string> TowerIDs = new List<string>();

        static void DestroyTower(string TowerID)
        {
            TowerListener.Remove(TowerID);

            int index = Towers.FindIndex(item => string.Compare(item.TowerID, TowerID, 0) == 0);

            if (index >= 0)
                Towers.RemoveAt(index);

            int index2 = TowerIDs.FindIndex(item => string.Compare(item, TowerID, 0) == 0);

            if (index2 >= 0)
                TowerIDs.RemoveAt(index2);


        }

        public static void SpawnTower(string TypeID, Vector2 towerVector, Coordinates squareCoords)
        {
            if (TypeID == "Gun")
                Towers.Add(new Tower(CreateID(TypeID), Tower.Type.Gun, towerVector, squareCoords));
            else if (TypeID == "Rocket")
                Towers.Add(new Tower(CreateID(TypeID), Tower.Type.Rocket, towerVector, squareCoords));
            else if (TypeID == "SAM")
                Towers.Add(new Tower(CreateID(TypeID), Tower.Type.SAM, towerVector, squareCoords));
            else if (TypeID == "Tesla")
                Towers.Add(new Tower(CreateID(TypeID), Tower.Type.Tesla, towerVector, squareCoords));


            GameRoot.grid.gridSquares[(int)squareCoords.x, (int)squareCoords.y].typeOfSquare = Squares.SqrFlags.Occupied;
            GameRoot.grid.gridSquares[(int)squareCoords.x, (int)squareCoords.y].typeOfSquare |= Squares.SqrFlags.Wall;
            GameRoot.grid.gridSquares[(int)squareCoords.x, (int)squareCoords.y].typeOfSquare |= Squares.SqrFlags.Concrete;
            GameRoot.grid.gridSquares[(int)squareCoords.x, (int)squareCoords.y].Building = Squares.BuildingType.Tower;
        }

        public static void Update()
        {
            foreach (Tower tower in Towers)
            {
                if (tower.Health <= 0)
                {
                    // DEADED
                    BuildManager.RemoveTowerFromSquare(tower);
                    DestroyTower(tower.TowerID);
                    break;
                }

                else
                    tower.Update();
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (Tower tower in Towers)
            {
                tower.Draw(sb);
            }
        }

        public static void TowerDamaged(int Damage, Tower tower, Projectile.Type projectile)
        {
            //Damage calcualtions should be put here. Not sure on the best way to handle this, could get very large with if's
            tower.Health -= Damage;
        }

        /// <summary>
        /// creates a unique ID for the enemy, if the random ID is not unique it will retry. Chances of this happening more than once are 1/1,000,000
        /// </summary>
        static string CreateID(string TypeID)
        {
            bool IsUnique = false;
            string ID = "";
            while (!IsUnique)
            {
                ID = TypeID + GameRoot.rnd.Next(0, 10).ToString() + GameRoot.rnd.Next(0, 100000).ToString();

                foreach (string id in TowerIDs)
                    if (id == ID)
                    {
                        IsUnique = false;
                        break;
                    }
                    else IsUnique = true;

                if (TowerIDs.Count() == 0)
                    IsUnique = true;
            }

            TowerIDs.Add(ID);

            return ID;
        }
    }
}
