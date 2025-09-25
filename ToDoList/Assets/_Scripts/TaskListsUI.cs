using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskListsUI : MonoBehaviour
{
    [Header("Tasks")]
    [SerializeField] private Transform taskBarsParent;

    [Header("Create")]
    [SerializeField] private GameObject createTasksPanel;
    [SerializeField] private TMP_InputField inputFieldTaskInput;
    [SerializeField] private Button createToDoTaskButton;
    [Space]
    [Header("Completed")]
    [SerializeField] private Button showCompletedTasks;
    [SerializeField] private Image showCompletedTasksImage;
    [SerializeField] private Color showCompletedColor;
    [SerializeField] private Color showToDoColor;
    [SerializeField] private TextMeshProUGUI showCompletedTasksText;
    [Space]
    [SerializeField] private Button deleteAllCompletedTasksButton;
}
