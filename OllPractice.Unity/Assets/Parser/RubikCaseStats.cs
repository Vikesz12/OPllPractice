using System;
using System.Collections.Generic;

namespace Parser
{
    [Serializable]
    public class RubikCaseStats
    {
        public List<RubikCaseStat> cases;
    }

    [Serializable]
    public class RubikCaseStat
    {
        public string name;
        public float average;
        public int solveCount;
        public float best;
    }
}
