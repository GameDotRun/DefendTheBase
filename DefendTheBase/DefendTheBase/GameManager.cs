using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public class GameManager
    {
        public enum GameStates
        {
            StartScreen,
            PlayScreen,
            EndScreen,
            Highscore,
            InfoScreen,
        }

        public enum BuildStates
        {
            // This decides what a mouse click does.
            Nothing,
            Destroy,
            Upgrade,
            Trench,
            Concrete,
            TowerGun,
            TowerRocket,
            TowerSAM,
            TowerTesla
        }
        private float m_manPower;
        private int m_resources;

        public static GameStates GameState;
        public static BuildStates BuildState;
        public float Manpower { get { return m_manPower; } }
        public int Resources { get { return m_resources; } }

        public void ModifyManpower(float value)
        {
            m_manPower += value;
        }

        public void ModifyResources(int value)
        {
            m_resources += value;
        }
    }
}
