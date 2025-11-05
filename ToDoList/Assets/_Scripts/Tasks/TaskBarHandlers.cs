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

    public static int ChangeTaskColor(Task task)
    {
        int colorIndex = task.taskColorIndex;

        colorIndex++;

        if (colorIndex > 10)
        {
            colorIndex = 0;
        }

        return colorIndex;
    }

    public static Color GetTaskColor(TaskColor color)
    {
        return color switch
        {
            TaskColor.Green => Color.green,
            TaskColor.Blue => new Color(0, .5f, 1),
            TaskColor.Cyan => Color.cyan,
            TaskColor.Yellow => Color.yellow,
            TaskColor.Orange => new Color(1, .5f, 0),
            TaskColor.Gray => Color.gray,
            TaskColor.DarkGreen => new Color(0, .6f, .1f),
            TaskColor.DarkYellow => new Color(.8f, .5f, 0),
            TaskColor.Purple => new Color(.7f, 0, 1),
            TaskColor.Red => Color.red,
            _ => Color.white,
        };
    }
}
