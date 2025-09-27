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
    public string taskListName;
    public int spriteName;

    public List<Task> tasks;
}
