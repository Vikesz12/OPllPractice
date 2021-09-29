using System;
using System.Collections.Generic;

namespace Parser
{
    public static class SelectedRubikCases
    {
        private static readonly List<RubikCaseParser.RubikCase> _selectedCases = new List<RubikCaseParser.RubikCase>();

        public static List<RubikCaseParser.RubikCase> SelectedCases = _selectedCases;

        public static void AddCase(RubikCaseParser.RubikCase rubikCase) => _selectedCases.Add(rubikCase);
        public static void RemoveCase(RubikCaseParser.RubikCase rubikCase) => _selectedCases.Remove(rubikCase);

        public static RubikCaseParser.RubikCase GetRandomCase() => _selectedCases[new Random().Next(_selectedCases.Count)];
    }
}
