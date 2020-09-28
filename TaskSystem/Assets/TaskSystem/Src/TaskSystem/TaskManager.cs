using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public class TaskManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static TaskManager s_Instance = null;

        /// <summary>
        /// The list of all tasks list that are currently loaded
        /// </summary>
        [SerializeField]
        private List<TaskList> m_lstLoadedTaskLists = null;

        /// <summary>
        /// The current loaded task list
        /// </summary>
        [SerializeField]
        private TaskList m_CurrentTaskList = null;

        /// <summary>
        /// Dictionary of the level name to its corresponding task list asset
        /// </summary>
        private Dictionary<string, TaskList> m_dictLevelTaskList = null;

        /// <summary>
        /// Manages the tasks pools, creates and reuses the tasks of different types
        /// </summary>
        private TaskPoolManager m_TaskPoolManager = null;

        /// <summary>
        /// Task list assets path
        /// </summary>
        private const string TASK_LIST_ASSETS_PATH = "TaskListAssets";

        /// <summary>
        /// List of all currently running sequences
        /// </summary>
        private List<ISequence> m_lstRunningSequneces = null;

        /// <summary>
        /// Stack of all sequences that ended in the last frame
        /// </summary>
        private Stack<ISequence> m_stackEndedLastFrame = null;

        /// <summary>
        /// Sets singleton instance
        /// </summary>
        public TaskManager initialize()
        {
            if (s_Instance != null)
            {
                return s_Instance;
            }
            s_Instance = this;
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_SEQUENCE_COMPLETE, onSequenceComplete);

            m_TaskPoolManager = new TaskPoolManager();
            m_lstRunningSequneces = new List<ISequence>(10);
            m_stackEndedLastFrame = new Stack<ISequence>(10);

            initializeTaskLists();

            ///initialize dictionary of level type to task list
            int l_iTotalTaskLists = m_lstLoadedTaskLists.Count;
            m_dictLevelTaskList = new Dictionary<string, TaskList>(l_iTotalTaskLists);
            for (int l_iTaskListIndex = 0; l_iTaskListIndex < l_iTotalTaskLists; l_iTaskListIndex++)
            {
                TaskList l_TaskList = m_lstLoadedTaskLists[l_iTaskListIndex];
                m_dictLevelTaskList.Add(l_TaskList.m_strName, l_TaskList);
            }

            return s_Instance;
        }

        /// <summary>
        /// Sets singleton instance to null
        /// </summary>
        public void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_SEQUENCE_COMPLETE, onSequenceComplete);
            s_Instance = null;
        }

        /// <summary>
        /// Intializes all task lists
        /// Creates 
        /// </summary>
        private void initializeTaskLists()
        {
            Object[] l_arrAssets = Resources.LoadAll(TASK_LIST_ASSETS_PATH);
            int l_iAssetCount = l_arrAssets.Length;
            for (int l_iAssetIndex = 0; l_iAssetIndex < l_iAssetCount; l_iAssetIndex++)
            {
                Object l_AssetObject = l_arrAssets[l_iAssetIndex];
                TaskList l_TaskList = MonoBehaviour.Instantiate(l_AssetObject) as TaskList;
                m_lstLoadedTaskLists.Add(l_TaskList);
                l_TaskList.initialize();
            }
        }

        /// <summary>
        /// Sets the tasklist as the current
        /// </summary>
        private void setTaskList(string a_strLevelName)
        {
            if ((m_CurrentTaskList != null) && m_CurrentTaskList.m_strName.Equals(a_strLevelName, System.StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            TaskList l_TaskList = null;
            if (m_dictLevelTaskList.TryGetValue(a_strLevelName, out l_TaskList))
            {
                m_CurrentTaskList = l_TaskList;
                Debug.Log("<color=BLUE>TaskManager::setTaskList:: Setting task list '" + a_strLevelName + "'</color>");
            }
            else
            {
                Debug.Log("<color=ORANGE>TaskManager::setTaskList:: Task list for level type '"+ a_strLevelName + "' does not exist. Current task list unchanged.</color>");
            }
        }

        /// <summary>
        /// Executes the sequence with the ID from the current task list
        /// </summary>
        public static void ExecuteSequence(string a_strSequenceID)
        {
            Debug.Log("<color=BLUE>TaskManager::ExecuteSequence:: Executing sequence with ID : '"+ a_strSequenceID+"'</color>");
            if (string.IsNullOrEmpty(a_strSequenceID))
            {
                return;
            }

            ScriptableSequence l_Sequence = null;
            if (s_Instance.m_CurrentTaskList != null)
            {
                l_Sequence = s_Instance.m_CurrentTaskList.getSequenceWithID(a_strSequenceID);
            }

            if (l_Sequence == null)
            {
                Debug.Log("<color=ORANGE>TaskManager::ExecuteSequence::</color> Failed to find a sequence with ID: '" + a_strSequenceID + "' in task list");
            }
            else
            {
                s_Instance.executeSequence(l_Sequence);
            }
        }

        /// <summary>
        /// Stops execution of the sequence
        /// </summary>
        /// <param name="a_strSequenceID"></param>
        public static void StopSequence(string a_strSequenceID)
        {
            int l_iRunningSequenceCount = s_Instance.m_lstRunningSequneces.Count;
            for (int l_iRunningSequenceIndex = 0; l_iRunningSequenceIndex < l_iRunningSequenceCount; l_iRunningSequenceIndex++)
            {
                if (s_Instance.m_lstRunningSequneces[l_iRunningSequenceIndex].getSequenceID().Equals(a_strSequenceID, System.StringComparison.OrdinalIgnoreCase))
                {
                    s_Instance.stopSequence(s_Instance.m_lstRunningSequneces[l_iRunningSequenceIndex]);
                    break;
                }
            }
        }

        /// <summary>
        /// Executes sequence
        /// Adds sequence to list of executing sequences
        /// </summary>
        /// <param name="a_Sequence"></param>
        private void executeSequence(ScriptableSequence a_Sequence)
        {
            ISequence l_Sequence = m_TaskPoolManager.getSequenceFromPool(a_Sequence);
            m_lstRunningSequneces.Add(l_Sequence);
            l_Sequence.onExecute();
        }

        /// <summary>
        /// Event called on any sequence complete
        /// </summary>
        /// <param name="a_Hashtable"></param>
        public void onSequenceComplete(EventHash a_Hashtable)
        {
            ISequence l_Sequence = (ISequence)a_Hashtable[GameEventTypeConst.ID_SEQUENCE_REF];
            stopSequence(l_Sequence);
        }

        /// <summary>
        /// Stops sequence
        /// Adds sequence to stack that will be removed from running sequences in the next frame
        /// </summary>
        /// <param name="a_Sequence"></param>
        public void stopSequence(ISequence a_Sequence)
        {
            m_stackEndedLastFrame.Push(a_Sequence);
        }

        /// <summary>
        /// Stop all running sequences
        /// </summary>
        public static void StopAll()
        {
            int l_iRunningSequenceCount = s_Instance.m_lstRunningSequneces.Count;
            for (int l_iCurRunningSeqIndex = 0; l_iCurRunningSeqIndex < l_iRunningSequenceCount; l_iCurRunningSeqIndex++)
            {
                s_Instance.stopSequence(s_Instance.m_lstRunningSequneces[l_iCurRunningSeqIndex]);
            }
        }

        void Update()
        {
            if (s_Instance == null) { return; }

            while (m_stackEndedLastFrame.Count != 0)
            {
                ISequence l_Sequence = m_stackEndedLastFrame.Pop();
                if (m_lstRunningSequneces.Remove(l_Sequence))
                {
                    m_TaskPoolManager.returnSequenceToPool(l_Sequence);
                }
            }

            for (int l_iSequenceIndex = 0; l_iSequenceIndex < m_lstRunningSequneces.Count; l_iSequenceIndex++)
            {
                m_lstRunningSequneces[l_iSequenceIndex].onUpdate();
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Prints all running sequences
        /// </summary>
        public static void LogRunningSequences()
        {
            System.Text.StringBuilder l_StringBuilder = new System.Text.StringBuilder(200);
            l_StringBuilder.AppendLine("<color=BLUE> RUNNING SEQUENCES </color>\n");

            int l_iRunningSequenceCount = s_Instance.m_lstRunningSequneces.Count; ;
            for (int l_iSequenceIndex = 0; l_iSequenceIndex < l_iRunningSequenceCount; l_iSequenceIndex++)
            {
                l_StringBuilder.AppendLine(l_iSequenceIndex+": \t"+s_Instance.m_lstRunningSequneces[l_iSequenceIndex].getSequenceID()+ "\n");
            }

            TaskPoolManager.LogPools(l_StringBuilder, s_Instance.m_TaskPoolManager);

            Debug.Log(l_StringBuilder);
        }

#endif
    }
}