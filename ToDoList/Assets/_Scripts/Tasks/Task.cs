using System.Collections.Generic;

[System.Serializable]
public class Task
{
    public string taskName;
    public int taskIdentifier;

    public int taskColorIndex = 0;

    public int taskPriority;
    public bool isCompleted;
}

[System.Serializable]
public class TaskList
{
    public int identifier = 0;
    public string taskListName;
    public int spriteName;

    public int taskColorIndex =0;

    public List<Task> tasks;

    public List<Task> GetTasksDone()
    {
        List<Task> tasksDone = new List<Task>();
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].isCompleted)
                tasksDone.Add(tasks[i]);
        }

        return tasksDone;
    }

    public List<Task> GetTasksPending()
    {
        List<Task> tasksPending = new List<Task>();
        for (int i = 0; i < tasks.Count; i++)
        {
            if (!tasks[i].isCompleted)
                tasksPending.Add(tasks[i]);
        }

        return tasksPending;
    }

    public int GetTasksAmount()
    {
        return tasks.Count;
    }
}

public enum TaskColor
{
    None = 0,
    Green = 1,
    Blue = 2,
    Cyan = 3,
    Yellow = 4,
    Orange = 5,
    Gray = 6,
    DarkGreen = 7,
    DarkYellow = 8,
    Purple = 9,
    Red = 10
}

