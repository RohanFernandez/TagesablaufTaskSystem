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
}
