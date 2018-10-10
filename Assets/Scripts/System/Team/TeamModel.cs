//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

public class TeamModel : Model
{
    TeamInfo teamInfo;
    List<TheirTeam> theirTeams = new List<TheirTeam>();
    Dictionary<int, Teammate> teammates = new Dictionary<int, Teammate>();

    public override void Reset()
    {
    }

    public void UpdateTeamTarget(int target)
    {
        var temp = teamInfo;
        temp.target = target;
        teamInfo = temp;
    }

    public void UpdateTeamLevelLimit(Int2 levelLimit)
    {
        var temp = teamInfo;
        temp.levelLimit = levelLimit;
        teamInfo = temp;
    }

    public void UpdateTeammate()
    {

    }

    public bool TryGetTeammate(int id, out Teammate teammate)
    {
        return teammates.TryGetValue(id, out teammate);
    }

    public void UpdateThireTeams()
    {
        theirTeams.Clear();
        for (var i = 0; i < 100; i++)
        {
            theirTeams.Add(new TheirTeam());
        }

        theirTeams.Sort(TheirTeam.Compare);
    }

    public struct TeamInfo
    {
        public int target;
        public Int2 levelLimit;
    }

    public struct Teammate
    {
        public int id;
        public string name;
        public int level;
        public int job;
        public int mapId;
        public bool online;
    }

    public struct TheirTeam
    {
        public int mateCount;
        public int captainerId;
        public int captainerName;
        public int target;
        public Int2 levelLimit;

        public static int Compare(TheirTeam lhs, TheirTeam rhs)
        {
            return lhs.mateCount < rhs.mateCount ? -1 : 1;
        }

    }


}





