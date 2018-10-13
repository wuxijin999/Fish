//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, October 10, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

public class DungeonModel : Model
{

    Dictionary<int, Dungeon> dungeons = new Dictionary<int, Dungeon>();

    public override void Reset()
    {
    }

    public void UpdateDungeonInfo(int id, int enterTimes, int highestStar)
    {
        if (dungeons.ContainsKey(id))
        {
            var dungeon = dungeons[id];
            dungeons[id] = dungeon.SetEneterTimes(enterTimes).SetHighestStar(highestStar);
        }
        else
        {
            dungeons[id] = new Dungeon()
            {
                id = id,
                enterTimes = enterTimes,
                highestStar = highestStar
            };
        }
    }

    public bool TryGetDungeon(int id, out Dungeon dungeon)
    {
        return dungeons.TryGetValue(id, out dungeon);
    }

    public struct Dungeon
    {
        public int id;
        public int enterTimes;
        public int highestStar;

        public Dungeon SetEneterTimes(int enterTimes)
        {
            this.enterTimes = enterTimes;
            return this;
        }

        public Dungeon SetHighestStar(int star)
        {
            this.highestStar = star;
            return this;
        }

    }

}





