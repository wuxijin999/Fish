//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, October 08, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : Presenter<Friend>
{

    FriendModel model = new FriendModel();
    Dictionary<int, FriendBrief> friendBriefs = new Dictionary<int, FriendBrief>();
    ListProperty<StrangerBrief> strangerBriefs = new ListProperty<StrangerBrief>();

    public void AddFriend(int playerId)
    {
        model.DeleteBlackFriend(playerId);
        model.UpdateFriendInfo(playerId);
        var brief = friendBriefs.ContainsKey(playerId) ? friendBriefs[playerId] : friendBriefs[playerId] = new FriendBrief(playerId);
        brief.level.value = 1;
        brief.name.value = "吊打小朋友";
        brief.offlineTime.value = 0;
    }

    public void DeleteFriend(int playerId)
    {

        model.DeleteFriend(playerId);
        if (friendBriefs.ContainsKey(playerId))
        {
            friendBriefs.Remove(playerId);
        }
    }

    public void AddBlackList(int playerId)
    {
        model.DeleteFriend(playerId);
        model.UpdateBlackFriendInfo(playerId);
    }

    public void QueryStranger()
    {
    }

    public void QueryStranger(int playerId)
    {

    }

    public void ApplyForFriend(int playerId)
    {

    }

    public List<int> GetFriends()
    {
        return model.GetFriends();
    }

    public int FindFriend(int playerId)
    {
        var friendList = model.GetFriends();
        return friendList.Find(x => { return x == playerId; });
    }

    public void UpdateFriendInfo(int playerId)
    {
        model.UpdateFriendInfo(playerId);
        var brief = friendBriefs.ContainsKey(playerId) ? friendBriefs[playerId] : friendBriefs[playerId] = new FriendBrief(playerId);
        brief.level.value = 1;
        brief.name.value = "吊打小朋友";
        brief.offlineTime.value = 0;
    }

    public void UpdateFriendInfoes(List<int> playerIds)
    {
        foreach (var item in playerIds)
        {
            UpdateFriendInfo(item);
        }
    }

    public void OnUpdateStrangers()
    {
        strangerBriefs.Clear();
    }

    public class FriendBrief
    {
        public readonly int playerId;
        public readonly StringProperty name = new StringProperty();
        public readonly IntProperty level = new IntProperty(0);
        public readonly IntProperty offlineTime = new IntProperty(0);

        public FriendBrief(int playerId)
        {
            this.playerId = playerId;
        }
    }

    public struct StrangerBrief
    {
        public int playerId;
        public int name;
        public int level;
    }

}





