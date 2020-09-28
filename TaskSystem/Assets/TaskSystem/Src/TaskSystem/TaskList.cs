using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public class TaskList : ScriptableObject
    {
        /// <summary>
        /// The unique name of the task list
        /// </summary>
        [SerializeField]
        public string m_strName = string.Empty;

        /// <summary>
        /// Total count of sequences in this task list
        /// </summary>
        [SerializeField]
        public int m_iSequenceCount = 0;
        /// <summary>
        /// List of all sequences in the task list
        /// </summary>
        [SerializeField]
        public List<ScriptableSequence> m_lstSequences = null;

        /// <summary>
        /// The dictionary of sequence Id to the sequence base object
        /// </summary>
        public Dictionary<string, ScriptableSequence> m_dictSequence = null;

        /// <summary>
        /// Sets up dictionary of sequence id to sequence base
        /// </summary>
        public void initialize()
        {
            m_dictSequence = new Dictionary<string, ScriptableSequence>(m_iSequenceCount);
            for (int l_iSequenceIndex = 0; l_iSequenceIndex < m_iSequenceCount; l_iSequenceIndex++)
            {
                ScriptableSequence l_SequenceBase = m_lstSequences[l_iSequenceIndex];
                l_SequenceBase.initialize();
                m_dictSequence.Add(l_SequenceBase.m_strSequenceID, l_SequenceBase);
            }
        }

        /// <summary>
        /// Return sequence with ID
        /// </summary>
        /// <returns></returns>
        public ScriptableSequence getSequenceWithID(string a_strSequenceID)
        {
            ScriptableSequence l_ReturnSequence = null;
            if (m_dictSequence != null)
            {
                m_dictSequence.TryGetValue(a_strSequenceID, out l_ReturnSequence);
            }
            return l_ReturnSequence;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Name of the tag in the task list
        /// </summary>
        private const string TAG_TASK_LIST = "TaskList";

        /// <summary>
        /// Creates and returns a Task list from the given XML URL
        /// </summary>
        /// <param name="a_strURL"></param>
        /// <returns></returns>
        public static TaskList GetTaskListFromXML(string a_strURL)
        {
            //ignore comments in xml
            System.Xml.XmlReaderSettings l_readerSettings = new System.Xml.XmlReaderSettings();
            l_readerSettings.IgnoreComments = true;
            System.Xml.XmlReader l_ReadXML = System.Xml.XmlReader.Create(a_strURL, l_readerSettings);

            System.Xml.XmlDocument l_XMLDoc = new System.Xml.XmlDocument();
            l_XMLDoc.Load(l_ReadXML);

            System.Xml.XmlNodeList l_NodeList = l_XMLDoc.GetElementsByTagName(TAG_TASK_LIST);
            if (l_NodeList == null || l_NodeList.Count == 0)
            {
                Debug.LogError("TaskList::GetTaskListFromXML::Could not find node with Tag : "+ TAG_TASK_LIST);
                return null;
            }
            
            System.Xml.XmlNode l_TaskListNode = l_NodeList[0];
            System.Xml.XmlAttribute l_TaskListAttribute = l_TaskListNode.Attributes[0];

            // Name of the task list
            string l_strTaskListName = l_TaskListAttribute.Value;

            // Parse Sequences
            System.Xml.XmlNodeList l_lstSequenceNodes = l_TaskListNode.ChildNodes;
            int l_iSequenceCount = l_lstSequenceNodes.Count;
            List<ScriptableSequence> l_lstSequences = new List<ScriptableSequence>(l_iSequenceCount);

            for (int l_iSequenceIndex = 0; l_iSequenceIndex < l_iSequenceCount; l_iSequenceIndex++)
            {
                System.Xml.XmlNode l_SequenceNode = l_lstSequenceNodes[l_iSequenceIndex];
                l_lstSequences.Add(ScriptableSequence.GetSequenceFromXML(l_SequenceNode));
            }

            ///Create Task list
            TaskList l_TaskList = ScriptableObject.CreateInstance<TaskList>();
            l_TaskList.m_strName = l_strTaskListName;
            l_TaskList.m_lstSequences = l_lstSequences;
            l_TaskList.m_iSequenceCount = l_iSequenceCount;

            return l_TaskList;
        }
#endif
    }
}