using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace DefendTheBase
{
    public class UI
    {
        
        //Class Variables
        private Vector2 m_position;
        private Texture2D m_txr;

        //Constructor
        public UI(Texture2D txr, int xpos, int ypos)
        {
            m_position = new Vector2(xpos, ypos);
            m_txr = txr;
        }


        //Draw
        public void Draw(SpriteBatch sb)
        {
            if (GameRoot.GameState == GameRoot.gamestate.PlayScreen)
            {
                sb.Draw(m_txr, m_position, Color.White);
            }
        }

        //load (probably wont need, but kept incase
        public void LoadContent()
        {
            
        }

        public void Update()
        {


        }



    }
}
