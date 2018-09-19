using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actor
{
    public class ActorEngine : MonoBehaviour
    {
        public static ActorEngine Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            if (FindObjectOfType<ActorEngine>() == null)
            {
                var gameObject = new GameObject("ActorEngine");
                Instance = gameObject.AddComponent<ActorEngine>();
                GameObject.DontDestroyOnLoad(gameObject);
            }
        }

        List<ActorBase> actorBases = new List<ActorBase>();
        public void Register(ActorBase actorBase)
        {
            if (!this.actorBases.Contains(actorBase))
            {
                this.actorBases.Add(actorBase);
            }
        }

        public void UnRegister(ActorBase actorBase)
        {
            if (this.actorBases.Contains(actorBase))
            {
                this.actorBases.Remove(actorBase);
            }
        }

        private void FixedUpdate()
        {
            foreach (var item in this.actorBases)
            {
                try
                {
                    if (item != null && item.enable)
                    {
                        item.OnFixedUpdate();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        private void Update()
        {
            foreach (var item in this.actorBases)
            {
                try
                {
                    if (item != null && item.enable)
                    {
                        item.OnUpdate1();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            foreach (var item in this.actorBases)
            {
                try
                {
                    if (item != null && item.enable)
                    {
                        item.OnUpdate2();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        private void LateUpdate()
        {
            foreach (var item in this.actorBases)
            {
                try
                {
                    if (item != null && item.enable)
                    {
                        item.OnLateUpdate1();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            foreach (var item in this.actorBases)
            {
                try
                {
                    if (item != null && item.enable)
                    {
                        item.OnLateUpdate2();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }


    }
}

