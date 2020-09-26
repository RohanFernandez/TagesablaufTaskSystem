using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public class GameEventContainer
    {
        /// <summary>
        /// The event list
        /// </summary>
        private System.Action<EventHash> m_Event = null;

        private GAME_EVENT_TYPE m_GameEventType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="a_GameEventType"></param>
        public GameEventContainer(GAME_EVENT_TYPE a_GameEventType)
        {
            m_GameEventType = a_GameEventType;
        }

        /// <summary>
        /// Adds specified callback to event
        /// </summary>
        /// <param name="a_Callback"></param>
        public void addCallback(System.Action<EventHash> a_Callback)
        {
            m_Event += a_Callback;
        }

        /// <summary>
        /// Removes specified callback from event
        /// </summary>
        /// <param name="a_Callback"></param>
        public void removeCallback(System.Action<EventHash> a_Callback)
        {
            m_Event -= a_Callback;
        }

        public void dispatch(EventHash a_EventHash)
        {
            if (m_Event != null)
            {
                m_Event.Invoke(a_EventHash);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Returns object name and method names
        /// </summary>
        /// <returns></returns>
        public string getLog()
        {
            System.Text.StringBuilder l_strReturnLog = new System.Text.StringBuilder("EVENT_TYPE: "+m_GameEventType.ToString());

            if (m_Event != null)
            {
                System.Delegate[] l_DelegateArray = m_Event.GetInvocationList();
                int l_iDelegateCount = l_DelegateArray.Length;
                for (int l_iDelegateIndex = 0; l_iDelegateIndex < l_iDelegateCount; l_iDelegateIndex++)
                {
                    System.Delegate l_CurrentDelegate = l_DelegateArray[l_iDelegateIndex];
                    l_strReturnLog.Append("\n");
                    l_strReturnLog.Append(l_iDelegateIndex);
                    l_strReturnLog.Append(": Object: ");
                    l_strReturnLog.Append(l_CurrentDelegate.Target.GetType().Name);
                    l_strReturnLog.Append(", MethodName: ");
                    l_strReturnLog.Append(l_CurrentDelegate.Method.Name);
                }
            }
            return l_strReturnLog.ToString();
        }
#endif
    }
}