using FRETBUZZ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleManager : MonoBehaviour
{
    [SerializeField]
    private FRETBUZZ.EventManager m_EventManager = null;

    [SerializeField]
    private FRETBUZZ.TaskManager m_TaskManager = null;

    void Awake()
    {
        m_EventManager.initialize();
        m_TaskManager.initialize();
    }

    void OnDestroy()
    {
        m_TaskManager.destroy();
        m_EventManager.destroy();
    }

    public void addTaskList(string a_strTaskAssetPath)
    {
        TaskManager.AddTaskList(a_strTaskAssetPath);
    }

    public void removeTaskList(string a_strTaskListName)
    {
        TaskManager.RemoveTaskList(a_strTaskListName);
    }

    public void setTaskListAsCurrent(string a_strTaskAssetPath)
    {
        TaskManager.SetCurrentTaskList(a_strTaskAssetPath);
    }
}
