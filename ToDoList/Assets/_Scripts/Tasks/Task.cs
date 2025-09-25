using System.Collections.Generic;

[System.Serializable]
public class Task
{
    public string taskName;
    public int taskIdentifier;

    public int taskPriority;
    public bool isCompleted;
}

[System.Serializable]
public class TasksLists
{
    public string taskListName;
    public int spriteName;

    private List<Task> tasks;
}