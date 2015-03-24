using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3.Models
{
    public class RootObject
    {
        public List<JsonData> JsonData { get; set; }
    }
    public class MetaData
    {
        public string Timestamp { get; set; }
    }

    public class Innings
    {
        public string TeamId { get; set; }
        public string TotalRuns { get; set; }
        public string TotalWickets { get; set; }
        public string Overs { get; set; }
        public string InningDeclare { get; set; }
    }

    public class Batsman
    {
        public string BatsmanName { get; set; }
        public string Runs { get; set; }
        public string BallsFaced { get; set; }
    }

    public class Bowler
    {
        public string BowlerName { get; set; }
        public string Overs { get; set; }
        public string RunsGiven { get; set; }
        public string Wickets { get; set; }
    }

    public class CricketMatch
    {
        public int TournamentId { get; set; }
        public int SeriesInstanceId { get; set; }
        public int MatchId { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        public string MatchStatus { get; set; }
        public string EditorialText { get; set; }
        public int WinningTeamId { get; set; }
        public int BattingTeamId { get; set; }
        public int BattingFirstTeamId { get; set; }
        public List<Innings> innings { get; set; }
        public List<Batsman> Batsmen { get; set; }
        public List<Bowler> Bowlers { get; set; }
    }

    public class JsonData
    {
        public MetaData metaData { get; set; }
        public List<CricketMatch> cricketMatches { get; set; }
        public List<object> footballMatches { get; set; }
    }

    
}
