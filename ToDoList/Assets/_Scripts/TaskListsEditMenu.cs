using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskListsEditMenu : MonoBehaviour
{
    [Header("Edit Menu")]
    [SerializeField] private GameObject editMenu;
    [Space]
    [SerializeField] private Button closeEditMenuBut;
    [SerializeField] private Button renameTaskListButton;
    [SerializeField] private Button deleteTaskListButton;
    [Space]
    [SerializeField] private TextMeshProUGUI taskListNameText;

    private TaskListButton taskListButton;
    private TasksListsUI tasksListsUI;

    private void Awake()
    {
        closeEditMenuBut.onClick.AddListener(CloseEditMenu);

        renameTaskListButton.onClick.AddListener(RenameTaskList);
        deleteTaskListButton.onClick.AddListener(DeleteTaskList);
    }

    public void Initialize(TasksListsUI tasksListsUI)
    {
        this.tasksListsUI = tasksListsUI;
    }

    private void RenameTaskList()
    {
        if(taskListButton == null)
            return;

        GameUI.instance.InputTextPanel.Show("Change task lists Name",
        taskListButton.TasksList.taskListName, (x) =>
        {
            taskListButton.TasksList.taskListName = x;
            taskListButton.UpdateUI();
            tasksListsUI.Save();
        });
    }

    private void DeleteTaskList()
    {
        if (taskListButton == null)
            return;

        GameUI.instance.CurModalPanel.ShowConfirm("Delete Task List",
            $"Do you want to erase task list: {taskListButton.TasksList.taskListName} And all its tasks?",
            null,() => tasksListsUI.DeleteTaskList(taskListButton), null);
    }

    public void CloseEditMenu()
    {
        editMenu.SetActive(false);
    }

    public void Seek(TaskListButton taskListButton)
    {
        if (taskListButton == null)
            return;

        this.taskListButton = taskListButton;
        this.taskListNameText.text = "Lista: " + taskListButton.TasksList.taskListName;

        editMenu.SetActive(true);
    }
}
