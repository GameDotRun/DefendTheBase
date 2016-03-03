using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    //public static class EnemyPosListener
    //{
    //    List<Vector2> EnemyPositions;

    //    public void EnemyPosListener()
    //    {
    //        EnemyPositions = new List<Vector2>();
    //    }

    //    //dont give this random vectors. things will go wrong
    //    public void AddEnemy(ref Vector2 EnemyPosition) 
    //    {
    //        EnemyPositions.Add(EnemyPosition);
    //    }
    
    
    //}




    class Enemy : ai
    {
        //insert stats: hp, speed, dmg etc.

        Texture2D sprite;
        public Coordinates enemyPos;
        public Vector2 enemyVect, ScreenPos;

        public bool pathFound = false;

        public Enemy() : base(new Coordinates(0,0))
        {
            enemyVect = ScreenPos = new Vector2(0, 0);
            enemyPos = new Coordinates(0, 0);
            sprite = Art.EnemyTex;

        }

        public void Update(Grid.gridFlags endPoint)
        {
            if (!pathFound && endPoint.HasFlag(Grid.gridFlags.endPoint))
            {
                FindPathReset();
                pathFound = FindPath(GameRoot.grid.stopPointCoord, GameRoot.grid.gridSquares, GameRoot.HEIGHT, GameRoot.WIDTH);
            }

            if (pathFound)
            {
               PathMove(GameRoot.grid.stopPointCoord, GameRoot.grid.gridSquares, GameRoot.HEIGHT, GameRoot.WIDTH, ref enemyVect);
                enemyPos = aiPos;
            }

            if (GameRoot.grid.stopPointCoord != null)
            {
                if (enemyVect.X == GameRoot.grid.stopPointCoord.x && enemyVect.Y == GameRoot.grid.stopPointCoord.y)
                {
                    enemyPos = new Coordinates(0, 0);
                    enemyVect = new Vector2(0, 0);
                    PathMoveReset();
                    pathFound = false;
                    aiPos = enemyPos;
                }
            }
            // Get screen pixel position from Grid Coordinates (enemyVect).
            ScreenPos = new Vector2((int)GameRoot.grid.gridBorder.X + (enemyVect.X * GameRoot.SQUARESIZE), (int)GameRoot.grid.gridBorder.Y + (enemyVect.Y * GameRoot.SQUARESIZE));
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, ScreenPos, Color.Aqua);

        }
    }
}
