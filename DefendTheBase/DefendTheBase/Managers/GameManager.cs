using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public class GameManager
    {
        private const float DEFAULT_MANPOWER = 100f;
        private const int DEFAULT_RESOURCES = 2500;

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

        private static float m_manPower = DEFAULT_MANPOWER;
        private static int m_resources = DEFAULT_RESOURCES;

        public static GameStates GameState;
        public static BuildStates BuildState;
        public static float Manpower { get { return m_manPower; } }
        public static int Resources { get { return m_resources; } }

        public static void ModifyManpower(float value)
        {
            m_manPower += value;
        }

        public static void ModifyResources(int value)
        {
            m_resources += value;
        }

        public static void ResetValues()
        {
            m_manPower = DEFAULT_MANPOWER;
            m_resources = DEFAULT_RESOURCES;
        }

        public static void EnemyWasDestroyed(string EnemyType)
        {
            //Add modifications for each enemy here
            if (EnemyType == "Tank")
                m_resources += 100;
            else if (EnemyType == "Helicopter")
                m_resources += 100;
            else if (EnemyType == "Transport")
                m_resources += 100;
            else if (EnemyType == "Jeep")
                m_resources += 100;
            else if (EnemyType == "Soldier")
                m_resources += 100;

        
        }

    }
}
