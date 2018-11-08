using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject aaa = null;
            var bbb = aaa as GameObject;

            Debug.Log(bbb);


            GameObject ttt = new GameObject();
            var rrr = ttt as GameObject;

            Debug.Log(rrr);
        }

    }
}
