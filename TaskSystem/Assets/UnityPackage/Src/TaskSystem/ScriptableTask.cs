using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    [System.Serializable]
    public class ScriptableTask
    {
        #region Attribute Keys
        public const string KEY_TASK_TYPE = "TaskType";
        public const string KEY_TASK_ID =   "ID";
        #endregion Attribute Keys

        /// <summary>
        /// Unique ID of the task in the sequence
        /// </summary>
        [SerializeField]
        public string m_strTaskID = string.Empty;

        /// <summary>
        /// The type of the task
        /// </summary>
        [SerializeField]
        public string m_strTaskType = string.Empty;

        /// <summary>
        /// The total number of attributes in this task
        /// </summary>
        [SerializeField]
        public int m_iAttributeCount = 0;

        /// <summary>
        /// The list of all attributes
        /// </summary>
        [SerializeField]
        private List<KeyValueAttribute> m_lstTaskAttributes = null;

        /// <summary>
        /// The hash of TaskAttributes taken from the list for easier access from the derived class.
        /// </summary>
        public Hashtable m_hashAttributes = null;

        /// <summary>
        /// Sets the List of all attributes into a hashtable for easy access
        /// </summary>
        public void initialize()
        {
            m_hashAttributes = new Hashtable(m_iAttributeCount);
            for (int l_iAttributesIndex = 0; l_iAttributesIndex < m_iAttributeCount; l_iAttributesIndex++)
            {
                KeyValueAttribute l_CurrentAttribute = m_lstTaskAttributes[l_iAttributesIndex];
                m_hashAttributes.Add(l_CurrentAttribute.m_strKey, l_CurrentAttribute.m_strValue);
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Creates a task base from the xml node
        /// creates and returns a task base and not its derived type
        /// because its derived type is not serialized into the serialized object
        /// </summary>
        /// <param name="a_TaskNode"></param>
        /// <returns></returns>
        public static ScriptableTask GetTaskFromXML(System.Xml.XmlNode a_TaskNode)
        {
            System.Xml.XmlAttributeCollection l_TaskAttributes = a_TaskNode.Attributes;
            int l_iAttributeCount = l_TaskAttributes.Count;

            string l_strTaskType = string.Empty;
            string l_strTaskID = string.Empty;

            List<KeyValueAttribute> l_lstAttributes = new List<KeyValueAttribute>(l_iAttributeCount);
            for (int l_iAttributeIndex = 0; l_iAttributeIndex < l_iAttributeCount; l_iAttributeIndex++)
            {
                System.Xml.XmlAttribute l_CurrentTaskAttribute = l_TaskAttributes[l_iAttributeIndex];

                KeyValueAttribute l_TaskAttribute = new KeyValueAttribute();
                l_TaskAttribute.m_strKey = l_CurrentTaskAttribute.Name;
                l_TaskAttribute.m_strValue = l_CurrentTaskAttribute.Value;

                if (l_TaskAttribute.m_strKey.Equals(KEY_TASK_TYPE, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strTaskType = l_TaskAttribute.m_strValue;
                }
                else if (l_TaskAttribute.m_strKey.Equals(KEY_TASK_ID, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strTaskID = l_TaskAttribute.m_strValue;
                }

                l_lstAttributes.Add(l_TaskAttribute);
            }

            ScriptableTask l_TaskBase = new ScriptableTask();
            l_TaskBase.m_iAttributeCount = l_iAttributeCount;
            l_TaskBase.m_lstTaskAttributes = l_lstAttributes;
            l_TaskBase.m_strTaskType = l_strTaskType;
            l_TaskBase.m_strTaskID = l_strTaskID;

            return l_TaskBase;
        }
#endif
    }
}