using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{

    GameObject body = null;
    Dictionary<string, GameObject> clothes = new Dictionary<string, GameObject>();
    Animator animator;

    public bool done { get; private set; }

    public void Assemble(Parts parts)
    {

    }


    public struct Parts
    {
        public int body;
        public int[] clothes;
        public int animator;
        public int[] effect;
    }

}


