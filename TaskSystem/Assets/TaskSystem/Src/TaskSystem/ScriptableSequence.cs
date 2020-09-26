using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    [System.Serializable]
    public class ScriptableSequence
    {
        #region Attribute Keys
        public const string KEY_SEQUENCE_TYPE   = "SequenceType";
        public const string KEY_SEQUENCE_ID     = "ID";
        public const string VALUE_SIMPLE_SEQUENCE = "SimpleSequence";
        #endregion Attribute Keys

        /// <summary>
        /// The type of sequence
        /// </summary>
        [SerializeField]
        public string m_strSequenceType = string.Empty;

        /// <summary>
        /// The unique sequence ID in the task list
        /// </summary>
        [SerializeField]
        public string m_strSequenceID = string.Empty;

        /// <summary>
        /// The total number of tasks in this sequence
        /// </summary>
        [SerializeField]
        public int m_iTaskCount = 0;

        /// <summary>
        /// The list of all tasks that are included in this sequence 
        /// </summary>
        [SerializeField]
        public List<ScriptableTask> m_lstTasks = null;

        /// <summary>
        /// Initializes all task in this sequence
        /// </summary>
        public void initialize()
        {
            for (int l_iTaskIndex = 0; l_iTaskIndex < m_iTaskCount; l_iTaskIndex++)
            {
                m_lstTasks[l_iTaskIndex].initialize();
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Parses and creates a sequence from the xml node
        /// </summary>
        /// <param name="a_SequenceNode"></param>
        /// <returns></returns>
        public static ScriptableSequence GetSequenceFromXML(System.Xml.XmlNode a_SequenceNode)
        {
            string l_strSequenceId = string.Empty;
            string l_strSequenceType = VALUE_SIMPLE_SEQUENCE;

            int l_iAttributesCount = a_SequenceNode.Attributes.Count;
            for (int l_iAttributeIndex = 0; l_iAttributeIndex < l_iAttributesCount; l_iAttributeIndex++)
            {
                System.Xml.XmlAttribute l_Attribute = a_SequenceNode.Attributes[l_iAttributeIndex];
                if (l_Attribute.Name.Equals(KEY_SEQUENCE_ID, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strSequenceId = l_Attribute.Value;
                }
                else if (l_Attribute.Name.Equals(KEY_SEQUENCE_TYPE, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strSequenceType = l_Attribute.Value;
                }
            }

            System.Xml.XmlNodeList l_lstTaskNodes = a_SequenceNode.ChildNodes;
            int l_iTaskCount = l_lstTaskNodes.Count;

            // Parse tasks
            List<ScriptableTask> l_lstTaskBase = new List<ScriptableTask>(l_iTaskCount);
            for (int l_iTaskIndex = 0; l_iTaskIndex < l_iTaskCount; l_iTaskIndex++)
            {
                System.Xml.XmlNode l_TaskNode = l_lstTaskNodes[l_iTaskIndex];
                l_lstTaskBase.Add(ScriptableTask.GetTaskFromXML(l_TaskNode));
            }

            ScriptableSequence l_Sequence = new ScriptableSequence();
            l_Sequence.m_strSequenceID = l_strSequenceId;
            l_Sequence.m_iTaskCount = l_iTaskCount;
            l_Sequence.m_lstTasks = l_lstTaskBase;
            l_Sequence.m_strSequenceType = l_strSequenceType;

            return l_Sequence;
        }
#endif

    }
}