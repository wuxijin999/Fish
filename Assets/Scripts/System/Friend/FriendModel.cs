//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, October 08, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

public class FriendModel
{
    Dictionary<int, Friend> friends = new Dictionary<int, Friend>();
    Dictionary<int, BlackFriend> blackFriends = new Dictionary<int, BlackFriend>();

    public void UpdateFriendInfo(int playerId)
    {
        var friend = friends.ContainsKey(playerId) ? this.friends[playerId] : this.friends[playerId] = new Friend(playerId);
        friend.name = "aaa";
        friend.level = UnityEngine.Random.Range(1, 99);
    }

    public void DeleteFriend(int playerId)
    {
        if (friends.ContainsKey(playerId))
        {
            friends.Remove(playerId);
        }
    }

    public Friend GetFriend(int playerId)
    {
        return null;
    }

    public List<int> GetFriends()
    {
        return new List<int>(friends.Keys);
    }

    public void UpdateBlackFriendInfo(int playerId)
    {
        var friend = blackFriends.ContainsKey(playerId) ? this.blackFriends[playerId] : this.blackFriends[playerId] = new BlackFriend(playerId);
        friend.name = "aaa";
        friend.level = UnityEngine.Random.Range(1, 99);
    }

    public void DeleteBlackFriend(int playerId)
    {
        if (blackFriends.ContainsKey(playerId))
        {
            blackFriends.Remove(playerId);
        }
    }

    public BlackFriend GetBlackFriend(int playerId)
    {
        return null;
    }

    public List<int> GetBlackFriends()
    {
        return new List<int>(blackFriends.Keys);
    }

    public class Friend
    {
        public readonly int playerId;
        public string name;
        public int level;
        public int offlineTime;

        public Friend(int playerId)
        {
            this.playerId = playerId;
        }
    }

    public class BlackFriend
    {
        public readonly int playerId;
        public string name;
        public int level;

        public BlackFriend(int playerId)
        {
            this.playerId = playerId;
        }
    }

}





