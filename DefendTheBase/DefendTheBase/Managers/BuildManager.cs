﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flextensions;

namespace DefendTheBase
{
    public static class BuildManager
    {

        public static int ManPower = 0;
        public static int Resources = 0;


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
            else if (GameManager.BuildState == GameManager.BuildStates.Repair)
                Repair();
            else if (GameManager.BuildState != GameManager.BuildStates.Nothing)
                BuildTower();
        }

        public static void Delete()
        {
            bool IStower = false;

            if (GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building != Squares.BuildingType.None)
            {
               
                    foreach (Tower tower in TowerListener.TowersList)
                    {
                        if (tower.towerCoords.CoordEqual(GameManager.mouseSqrCoords))
                        {
                            if (tower.TowerType == "Gun")
                            {

                                GameManager.BuildState = GameManager.BuildStates.TowerGun;
                                GameManager.CostGet();
                            }
                            else if (tower.TowerType == "Rocket")
                            {
                                GameManager.BuildState = GameManager.BuildStates.TowerRocket;
                                GameManager.CostGet();
                            }
                            else if (tower.TowerType == "SAM")
                            {
                                GameManager.BuildState = GameManager.BuildStates.TowerSAM;
                                GameManager.CostGet();
                            }
                            else if (tower.TowerType == "Tesla")
                            {
                                GameManager.BuildState = GameManager.BuildStates.TowerSAM;
                                GameManager.CostGet();
                            }


                            BuildManager.RemoveTowerFromSquare(tower);
                            GameManager.ObjectWasDemolished(tower.TowerType);
                            TowerManager.DestroyTower(tower.TowerID);
                            IStower = true;
                            break;
                        }

                    }

                    if (!GameManager.mouseSqrCoords.CoordEqual(new Coordinates(GameManager.WIDTH - 2, GameManager.HEIGHT - 1)) && !GameManager.mouseSqrCoords.CoordEqual(new Coordinates(GameManager.WIDTH - 1, GameManager.HEIGHT - 1)) && !GameManager.mouseSqrCoords.CoordEqual(new Coordinates(GameManager.WIDTH - 1, GameManager.HEIGHT - 2)) && !GameManager.mouseSqrCoords.CoordEqual(new Coordinates(GameManager.WIDTH - 3, GameManager.HEIGHT - 1)) && !GameManager.mouseSqrCoords.CoordEqual(new Coordinates(GameManager.WIDTH - 1, GameManager.HEIGHT - 3)))
                    {

                        if (GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building == Squares.BuildingType.Concrete && !IStower)
                        {
                            GameManager.BuildState = GameManager.BuildStates.Concrete;
                            GameManager.CostGet();
                            GameManager.ObjectWasDemolished("Concrete");
                        }
                        else if (GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building == Squares.BuildingType.Trench && !IStower)
                        {
                            GameManager.BuildState = GameManager.BuildStates.Trench;
                            GameManager.CostGet();
                            GameManager.ObjectWasDemolished("Trench");
                        }


                        GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].typeOfSquare = Squares.SqrFlags.Unoccupied;
                        GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building = Squares.BuildingType.None;
                        GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].sqrEdited = true;

                        GameManager.BuildState = GameManager.BuildStates.Destroy;
                    }
            }   
        }

        static void Upgrade()
        {
            GameManager.CostGet();

            if (GameManager.Manpower >= ManPower && GameManager.Resources >= Resources)
            {
                foreach (Tower tower in TowerListener.TowersList)
                {
                    if (tower.towerCoords.CoordEqual(GameManager.mouseSqrCoords) && tower.Level < 4)
                    {
                        tower.LevelUp();
                        GameManager.TowerWasUpgraded();
                    }
                }
            }

            else ResourceManpowerNotification();
        }

        static void Repair()
        {
            GameManager.CostGet();

            if (GameManager.Manpower >= ManPower && GameManager.Resources >= Resources)
            {
                foreach (Tower tower in TowerListener.TowersList)
                {
                    if (tower.towerCoords.CoordEqual(GameManager.mouseSqrCoords))
                    {
                        if (tower.Health != tower.HEALTHDEF)
                        {
                            int temp = tower.HEALTHDEF - tower.Health;

                            temp = temp / 2;

                            tower.Health += temp;

                            GameManager.TowerWasRepaired();
                        }
                    }
                }
            }

            else ResourceManpowerNotification();
        
        }

        // as towers are not static towers should be treated as normal objects rather than elements of squares. 
        static void BuildTower()
        {
            string TowerType = BuildToTowerType();

            GameManager.CostGet();

            if (!GameManager.mouseSqrCoords.CoordEqual(GameManager.ENDPOINT))
            {
                if (GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building == Squares.BuildingType.Concrete)
                {
                    if (GameManager.Manpower >= ManPower && GameManager.Resources >= Resources)
                    {
                        if (GridManager.InaccessibleSquareCheck(GameManager.grid.gridSquares, GameManager.mouseSqrCoords))
                            TowerManager.SpawnTower(TowerType, GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].PixelScreenPos, GameManager.mouseSqrCoords);
                        else BlockedNotification();

                    }

                    else ResourceManpowerNotification();
                }

                else NeedConcreteNotification();

            }
            

           

        }

        //Squares will handle the static objects that dont interact. eg. concrete This just sets the correct square with the flags.
        static void BuildTrench()
        {
            GameManager.CostGet();

            if (GridManager.HasNeighbour(Squares.BuildingType.Trench, GameManager.mouseSqrCoords) && GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building == Squares.BuildingType.None)
            {
                if (GameManager.Manpower >= ManPower && GameManager.Resources >= Resources)
                {
                    if (GridManager.InaccessibleSquareCheck(GameManager.grid.gridSquares, GameManager.mouseSqrCoords) && !GameManager.mouseSqrCoords.CoordEqual(GameManager.ENDPOINT))
                    {
                        GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].typeOfSquare |= Squares.SqrFlags.Wall;
                        GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building = Squares.BuildingType.Trench;
                        GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].sqrEdited = true;

                        GameManager.BaseWasBuilt("Trench");

                    }

                    else BlockedNotification();

                    GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].sqrEdited = true;
                }

                else ResourceManpowerNotification();
            }

            else NextToTrenchNotification();
        }

        
        static void BuildConcrete()
        {
            GameManager.CostGet();

            if (!GameManager.mouseSqrCoords.CoordEqual(GameManager.ENDPOINT))
            {
                if (GridManager.HasNeighbour(Squares.BuildingType.Trench, GameManager.mouseSqrCoords))
                {
                    if (GameManager.Manpower >= ManPower && GameManager.Resources >= Resources)
                    {

                        if (GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].typeOfSquare != Squares.SqrFlags.Concrete)
                        {
                            GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].typeOfSquare = Squares.SqrFlags.Occupied;
                            GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].typeOfSquare = Squares.SqrFlags.Concrete;
                            GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].Building = Squares.BuildingType.Concrete;
                            GameManager.grid.gridSquares[(int)GameManager.mouseSqrCoords.x, (int)GameManager.mouseSqrCoords.y].sqrEdited = true;

                            GameManager.BaseWasBuilt("Concrete");
                        }
                    }

                    else ResourceManpowerNotification();
                }

                else NextToTrenchNotification();

            }
        }

        static void ResourceManpowerNotification()
        { 
            if (GameManager.Manpower < ManPower)
                PopUpNotificationManager.Add(new PopUpNotificationText(PopUpNotificationManager.NoManpower, GameManager.MouseScreenPos, Color.Red));
            else if (GameManager.Resources < Resources)
                PopUpNotificationManager.Add(new PopUpNotificationText(PopUpNotificationManager.NoResources, GameManager.MouseScreenPos, Color.Red));
        }

        static void NextToTrenchNotification()
        {
            PopUpNotificationManager.Add(new PopUpNotificationText(PopUpNotificationManager.NextToTrench, GameManager.MouseScreenPos, Color.Red));
        }

        static void NeedConcreteNotification()
        {
            PopUpNotificationManager.Add(new PopUpNotificationText(PopUpNotificationManager.NeedConcrete, GameManager.MouseScreenPos, Color.Red));
        }

        static void BlockedNotification()
        {
            PopUpNotificationManager.Add(new PopUpNotificationText(PopUpNotificationManager.CantPlaceTrench, GameManager.MouseScreenPos, Color.Red));
        }

        public static void RemoveTowerFromSquare(Tower tower)
        {
            GameManager.grid.gridSquares[(int)tower.towerCoords.x, (int)tower.towerCoords.y].typeOfSquare = Squares.SqrFlags.Unoccupied;
            GameManager.grid.gridSquares[(int)tower.towerCoords.x, (int)tower.towerCoords.y].Building = Squares.BuildingType.None;
        
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
