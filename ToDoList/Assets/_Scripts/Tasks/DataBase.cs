using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public List<TasksList> tasksLists;

    private void Awake()
    {
        
    }

    public void Load(SaveObject saveObject)
    {
        this.tasksLists = saveObject.tasksLists;

        if(tasksLists == null)
            this.tasksLists = new List<TasksList>();

    }
}

[System.Serializable]
public class TasksList
{
    public int identifier = 0;
    public string taskListName;
    public int spriteName;

    public List<Task> tasks;

    public List<Task> GetTasksDone()
    {
        List < Task > tasksDone = new List <Task> ();   
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].isCompleted)
                tasksDone.Add (tasks[i]);
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
