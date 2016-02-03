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
    class Enemy
    {
        //insert stats: hp, speed, dmg etc.


        Texture2D sprite;
        Coordinates enemyPos;
        Vector2 enemyVect;
        ai pathFinder;

        bool pathFound = false;
        bool destReached = false;

        public Enemy()
        {
            enemyVect = new Vector2(0, 0);
            enemyPos = new Coordinates(0, 0);
            pathFinder = new ai(enemyPos, Game1.DEFAULYDIST);
            sprite = Game1.art.enemyTexReturn;

            
    
        }

        public void Update(Grid.gridFlags endPoint)
        {
            if (!pathFound && endPoint.HasFlag(Grid.gridFlags.endPoint))
                pathFound = pathFinder.FindPath(Game1.grid.stopPointCoord, Game1.grid.gridSquares, Game1.HEIGHT, Game1.WIDTH);

            if (pathFound)
            {
                pathFinder.PathMove(Game1.grid.stopPointCoord, Game1.grid.gridSquares, Game1.HEIGHT, Game1.WIDTH, ref enemyVect);
                enemyPos = pathFinder.aiPos;
            }

            if (Game1.grid.stopPointCoord != null)
            {
                if (enemyPos.x == Game1.grid.stopPointCoord.x && enemyPos.y == Game1.grid.stopPointCoord.y)
                {
                    enemyPos = new Coordinates(0, 0);
                    enemyVect = new Vector2(0, 0);
                    pathFinder.Reset();
                    pathFinder.aiPos = enemyPos;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, new Vector2((int)Game1.grid.gridBorder.X + (enemyVect.X * Game1.SQUARESIZE), (int)Game1.grid.gridBorder.Y + (enemyVect.Y * Game1.SQUARESIZE)), Color.Aqua);  
        
        }
   
    }
}
