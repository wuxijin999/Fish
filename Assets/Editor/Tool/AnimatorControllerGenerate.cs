using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class AnimatorControllerGenerate
{


    public static void GenerateAnimator(AnimatorGenerateConfig config, string newControllerPath)
    {
        if (config == null)
        {
            return;
        }

        var templatePath = StringUtil.Contact("Assets/Editor/AnimatorControllerTemplate/", config.controllerTemplate, ".controller");
        AssetDatabase.CopyAsset(templatePath, newControllerPath);
        var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(newControllerPath);

        var stateMachine = controller.layers[0].stateMachine;
        HandleStateMachine(string.Empty, stateMachine, config);

        EditorUtility.SetDirty(controller);
    }

    protected static void HandleStateMachine(string modelName, AnimatorStateMachine stateMachine, AnimatorGenerateConfig config)
    {
        if (stateMachine.states == null || stateMachine.states.Length == 0)
        {
            return;
        }

        AnimatorState state = null;
        Motion motion = null;

        for (int i = 0; i < stateMachine.states.Length; ++i)
        {
            state = stateMachine.states[i].state;

            var index = config.stateClips.FindIndex((AnimatorGenerateConfig.StateToClip stateToClip) => { return stateToClip.state == state.name; });
            if (index == -1)
            {
                Debug.LogWarningFormat("没有找到状态机上名称为: {0}在{1}里的对应配置. ", state.name, config.name);
                continue;
            }

            motion = state.motion;
            if (motion is BlendTree)
            {
            }
            else
            {
                var clipName = config.stateClips[index].clip;
                state.motion = GetAnimationClip(modelName, clipName);

                if (state.motion == null)
                {
                    Debug.LogWarningFormat("为{0}状态设置动画失败!!!", state.name);
                }
            }
        }

        foreach (var childStateMachine in stateMachine.stateMachines)
        {
            HandleStateMachine(modelName, childStateMachine.stateMachine, config);
        }
    }

    static AnimationClip GetAnimationClip(string _fbx, string _clipName)
    {
        return null;
    }

}
