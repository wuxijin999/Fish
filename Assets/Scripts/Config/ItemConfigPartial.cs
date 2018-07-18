using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ItemConfig
{

    public static ItemConfig Get(int _param1, int _param2)
    {
        var id = _param1 * 100 + _param2;
        return Get(id);
    }

}
