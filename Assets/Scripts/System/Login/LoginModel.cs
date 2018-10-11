//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Wednesday, September 12, 2018
//--------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginModel : Model
{

    public bool accountLoginOk = false;
    public bool enterWorldOk = false;

    public override void Reset()
    {
        accountLoginOk = false;
        enterWorldOk = false;
    }

}





