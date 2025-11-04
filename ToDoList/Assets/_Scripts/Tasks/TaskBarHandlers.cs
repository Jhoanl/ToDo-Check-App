using System;
using UnityEngine;

public class TaskBarHandlers : MonoBehaviour
{
    public static event Action OnCopyOrPaste;

    public static Task taskCopied;
    public static bool hasTaskCopied;

    public static void CopyTaskBar(Task task)
    {
        taskCopied = task;
        hasTaskCopied=true;

        OnCopyOrPaste?.Invoke();
    }

    public static void SetTaskPasted()
    {
        hasTaskCopied = false;
        taskCopied = null;

        OnCopyOrPaste?.Invoke();
    }

    public static Task GetTaskCopied()
    {
        return taskCopied;
    }
}
