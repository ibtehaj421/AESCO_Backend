namespace ASCO.DTOs.Crew
{
    public class CrewStatisticsDto
    {
        public int TotalCrewMembers { get; set; }
        public int ActiveCrewMembers { get; set; }
        public int InactiveCrewMembers { get; set; }
        public Dictionary<string, int> CrewByNationality { get; set; } = new();
        public Dictionary<string, int> CrewByRank { get; set; } = new();
        public Dictionary<string, int> CrewByJobType { get; set; } = new();
        public Dictionary<string, int> CrewByStatus { get; set; } = new();
        public int TotalCertifications { get; set; }
        public int ExpiredCertifications { get; set; }
        public int ExpiringSoonCertifications { get; set; }
        public int TotalTrainings { get; set; }
        public int TotalEvaluations { get; set; }
        public int TotalPayrollRecords { get; set; }
        public int TotalExpenseReports { get; set; }
    }
}
