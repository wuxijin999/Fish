using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/AnimatorControllerTemplate")]
public class AnimatorGenerateConfig : ScriptableObject
{
    public string controllerTemplate;
    public List<StateToClip> stateClips;

    [System.Serializable]
    public struct StateToClip
    {
        public string state;
        public string clip;
    }



}
