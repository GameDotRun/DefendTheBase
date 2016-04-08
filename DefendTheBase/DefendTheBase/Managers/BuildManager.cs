using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flextensions;

namespace DefendTheBase
{
    public static class BuildManager
    {
        public static void Build()
        {
            if (GameManager.BuildState == GameManager.BuildStates.Concrete)
                BuildConcrete();
            else if (GameManager.BuildState == GameManager.BuildStates.Trench)
                BuildTrench();
            else if (GameManager.BuildState == GameManager.BuildStates.Upgrade)
                Upgrade();
            else if (GameManager.BuildState == GameManager.BuildStates.Destroy)
                Delete();
            else if (GameManager.BuildState != GameManager.BuildStates.Nothing)
                BuildTower();
        }

        public static void Delete()
        { 
            if(GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building == Squares.BuildingType.Tower)
            {
                foreach (Tower tower in TowerListener.TowersList)
                {
                    if (tower.towerCoords.CoordEqual(GameManager.mouseSqrCoords))
                    {
                        tower.Health = 0;
                    }
                }

            }

            else if (GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building == Squares.BuildingType.Concrete)
            {
                GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].typeOfSquare = Squares.SqrFlags.Unoccupied;
                GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building = Squares.BuildingType.None;
                GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].sqrEdited = true;
            }
        }

        static void Upgrade()
        {
            foreach (Tower tower in TowerListener.TowersList)
            {
                if (tower.towerCoords.CoordEqual(GameManager.mouseSqrCoords))
                {
                    tower.LevelUp();
                }
            }
        
        }

        // as towers are not static towers should be treated as normal objects rather than elements of squares. 
        static void BuildTower()
        {
            string TowerType = BuildToTowerType();

            TowerManager.SpawnTower(TowerType, GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].PixelScreenPos, GameManager.mouseSqrCoords);
        
        }

        //Squares will handle the static objects that dont interact. eg. concrete This just sets the correct square with the flags.
        static void BuildTrench()
        {
            if (GridManager.InaccessibleSquareCheck(GameRoot.grid.gridSquares, GameManager.mouseSqrCoords))
            {
                GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].typeOfSquare |= Squares.SqrFlags.Wall;
                GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building = Squares.BuildingType.Trench;
                GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].sqrEdited = true;
            }

            GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].sqrEdited = true;
        }

        
        static void BuildConcrete()
        {
            GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].typeOfSquare = Squares.SqrFlags.Occupied;
            GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].typeOfSquare = Squares.SqrFlags.Concrete;
            GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building = Squares.BuildingType.Concrete;
            GameRoot.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].sqrEdited = true;
        }

        public static void RemoveTowerFromSquare(Tower tower)
        {
            GameRoot.grid.gridSquares[(int)tower.towerCoords.x, (int)tower.towerCoords.y].typeOfSquare = Squares.SqrFlags.Unoccupied;
            GameRoot.grid.gridSquares[(int)tower.towerCoords.x, (int)tower.towerCoords.y].Building = Squares.BuildingType.None;
        
        }

        static string BuildToTowerType()
        {
            string Type;

            switch (GameManager.BuildState)
            { 
                case GameManager.BuildStates.TowerGun:
                    Type = "Gun";
                    return Type;

                case GameManager.BuildStates.TowerRocket:
                    Type = "Rocket";
                    return Type;

                case GameManager.BuildStates.TowerSAM:
                    Type = "SAM";
                    return Type;

                case GameManager.BuildStates.TowerTesla:
                    Type = "Tesla";
                    return Type;

                default:
                    return "Gun";
            }
        }

    }
}
