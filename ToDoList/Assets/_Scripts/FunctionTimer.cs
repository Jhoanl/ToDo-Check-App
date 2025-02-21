using System;
using System.Collections.Generic;
using UnityEngine;

public class FunctionTimer
{

    private static List<FunctionTimer> activeTimerList;
    private static GameObject initGameObject;
    private static string defaultFunctionObjectName;
    private static void InitIfNeedIt()
    {
        if (initGameObject == null)
        {
            initGameObject = new GameObject("FunctionTimer_InitGameObject");
            activeTimerList = new List<FunctionTimer>();
            defaultFunctionObjectName = "FunctionTimer Object ";
        }
    }

    public static FunctionTimer Create(Action action, float timer)
    {
        return Create(action, timer, string.Empty, false);
    }

    public static FunctionTimer Create(Action action, float timer, bool useUnscaledTime)
    {
        return Create(action, timer, string.Empty, useUnscaledTime);
    }

    public static FunctionTimer Create(Action action, float timer, string timerName, bool useUnscaledTime = false)
    {
        InitIfNeedIt();
        GameObject gameObject = new GameObject(defaultFunctionObjectName + timerName, typeof(MonoBehaviourHook));

        FunctionTimer functionTimer = new FunctionTimer(action, timer, timerName, gameObject, useUnscaledTime);

        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;

        activeTimerList.Add(functionTimer);

        return functionTimer;
    }

    private static void RemoveTimer(FunctionTimer functionTimer)
    {
        InitIfNeedIt();
        activeTimerList.Remove(functionTimer);
    }

    private class MonoBehaviourHook : MonoBehaviour
    {
        public Action onUpdate;
        private void Update()
        {
            onUpdate?.Invoke();
        }
    }

    private static void StopAllTimersWithName(string timerName)
    {
        for (int i = 0; i < activeTimerList.Count; i++)
        {
            if (activeTimerList[i].timerName == timerName)
            {
                activeTimerList[i].DestroySelf();
                i--;
            }
        }
    }


    private Action action;
    private GameObject gameObject;
    private float timer;
    private string timerName;
    private bool useUnscaledTime;

    private FunctionTimer(Action action, float timer, string timerName, GameObject gameObject, bool useUnscaledTime)
    {
        this.action = action;
        this.timer = timer;
        this.gameObject = gameObject;
        this.timerName = timerName;
        this.useUnscaledTime = useUnscaledTime;
    }

    private void Update()
    {
        if (useUnscaledTime)
        {
            timer -= Time.unscaledDeltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            action();
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        RemoveTimer(this);
        if (gameObject != null)
            UnityEngine.Object.Destroy(gameObject);
    }
}

