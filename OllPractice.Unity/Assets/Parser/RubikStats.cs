using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Parser
{
    public static class RubikStats
    {
        public static RubikCaseStats PracticeStats;
        public static RubikCaseStats TrainingStats;

        private static readonly string _practiceStatsPath = Application.persistentDataPath + "/PracticeStats.dat";
        private static readonly string _trainingStatsPath = Application.persistentDataPath + "/TrainingStats.dat";

        public static void LoadData()
        {
            PracticeStats = LoadStats(_practiceStatsPath);
            TrainingStats = LoadStats(_trainingStatsPath);
        }

        public static void SaveData()
        {
            SaveStats(_practiceStatsPath, PracticeStats);
            SaveStats(_trainingStatsPath, TrainingStats);
        }

        private static void SaveStats(string filePath, RubikCaseStats data)
        {
            var bf = new BinaryFormatter();
            var file = File.Create(filePath);
            bf.Serialize(file, data);
            file.Close();
        }

        private static RubikCaseStats LoadStats(string filePath)
        {
            if (!File.Exists(filePath)) return new RubikCaseStats{cases = new List<RubikCaseStat>()};

            var bf = new BinaryFormatter();
            var file = File.Open(filePath, FileMode.Open);
            var data = (RubikCaseStats)bf.Deserialize(file);
            file.Close();
            return data;
        }
    }
}
