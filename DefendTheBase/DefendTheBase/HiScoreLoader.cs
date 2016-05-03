using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace DefendTheBase
{
    [Serializable]
    public class HiScoreData
    {
        public int HighestWave, HighestWaveKills, AllTimeKills;

        public HiScoreData LoadData()
        {
            HiScoreData data;

            // Get the path of the save game
            string fullpath = "hiscores.dat";

            // Check to see if the save exists.
            if (!File.Exists(fullpath))
            {
                //If the file doesn't exist, make a fake one...
                CreateFile();
            }

            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate,
            FileAccess.Read);
            try
            {

                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HiScoreData));
                data = (HiScoreData)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file
                stream.Close();
            }

            return (data);
        }

        public void SaveData(HiScoreData data)
        {
            // Get the path of the save game
            string fullPath = "hiscores.dat";


            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullPath, FileMode.Create);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HiScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        
        }

        public void CreateFile()
        {
            // Create dummy data to save.
            HiScoreData data = new HiScoreData();
            data.AllTimeKills = 0;
            data.HighestWave = 0;
            data.HighestWaveKills = 0;

            SaveData(data);
        }

    }

}
