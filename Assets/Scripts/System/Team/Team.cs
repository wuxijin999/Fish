//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, September 28, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : Presenter<Team>
{

    TeamModel model = new TeamModel();
    public readonly TeamBrief teamBrief = new TeamBrief();
    public readonly Teammates teammates = new Teammates();
    public readonly IntProperty theirTeams = new IntProperty();

    public override void OpenWindow(int functionId = 0)
    {
    }

    public override void CloseWindow()
    {
    }

    public void CreateTeam()
    {

    }

    public void JoinTeam(int teamId)
    {

    }

    public void LeaveTeam()
    {

    }

    public List<int> GetInvitablePlayers()
    {
        return null;
    }

    public void InvitePlayer(int playerId)
    {

    }

    public void KickPlayer(int playerId)
    {

    }

    public void ChangeTeammateJob(int playerId, int job)
    {

    }

    public void AcceptApplication(int teamId)
    {

    }

    public void RejectApplication(int teamId)
    {

    }

    public void AcceptInvitation(int teamId)
    {

    }

    public void RejectInvitation(int teamId)
    {

    }

    public class TeamBrief
    {
        public readonly StringProperty name = new StringProperty();
        public readonly IntProperty target = new IntProperty(0);
    }

    public class Teammates
    {
        public readonly IntProperty mate1 = new IntProperty(0);
        public readonly IntProperty mate2 = new IntProperty(0);
        public readonly IntProperty mate3 = new IntProperty(0);
        public readonly IntProperty mate4 = new IntProperty(0);

        public void SetDirty(int index, int playerId)
        {
            switch (index)
            {
                case 1:
                    mate1.value = playerId;
                    mate1.dirty = true;
                    break;
                case 2:
                    mate2.value = playerId;
                    mate2.dirty = true;
                    break;
                case 3:
                    mate3.value = playerId;
                    mate3.dirty = true;
                    break;
                case 4:
                    mate4.value = playerId;
                    mate4.dirty = true;
                    break;
                default:
                    break;
            }
        }
    }

}





