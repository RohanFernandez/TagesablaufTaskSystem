using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public class EventManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static EventManager s_Instance = null;

        /// <summary>
        /// Dictionary of events to delegate
        /// </summary>
        private Dictionary<GAME_EVENT_TYPE, GameEventContainer> m_dictGameEvents = null;

        /// <summary>
        /// Pool of hashtable used for arguements of an event
        /// </summary>
        private EventHashPool m_HashPool = null;

        /// <summary>
        /// Sets singleton instance
        /// </summary>
        public void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            m_HashPool = new EventHashPool(2);
            m_dictGameEvents = new Dictionary<GAME_EVENT_TYPE, GameEventContainer>(30);
        }

        /// <summary>
        /// Destroys singleton instance
        /// </summary>
        public void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            s_Instance = null;
        }

        /// <summary>
        /// Gets hashtable from the pool
        /// </summary>
        /// <returns></returns>
        public static EventHash GetEventHashtable()
        {
            EventHash l_hash = s_Instance.m_HashPool.getObject();
            l_hash.Clear();
            return l_hash;
        }

        /// <summary>
        /// Returns a hashtable back into the pool
        /// </summary>
        /// <param name="a_Hashtable"></param>
        private void returnHashtableToPool(EventHash a_EventHash)
        {
            m_HashPool.returnToPool(a_EventHash);
        }

        /// <summary>
        /// Subscribes to the game event
        /// </summary>
        public static void SubscribeTo(GAME_EVENT_TYPE a_GameEventType, System.Action<EventHash> a_EventCallback)
        {
            GameEventContainer l_EventContainer = null;
            if(!s_Instance.m_dictGameEvents.TryGetValue(a_GameEventType, out l_EventContainer))
            {
                l_EventContainer = new GameEventContainer(a_GameEventType);
                s_Instance.m_dictGameEvents.Add(a_GameEventType, l_EventContainer);
            }
            l_EventContainer.addCallback(a_EventCallback);
        }

        /// <summary>
        /// Unsubscribes from the game event
        /// </summary>
        public static void UnsubscribeFrom(GAME_EVENT_TYPE a_GameEventType, System.Action<EventHash> a_EventCallback)
        {
            if (s_Instance == null)
            { 
                return;
            }

            GameEventContainer l_EventContainer = null;
            if (s_Instance.m_dictGameEvents.TryGetValue(a_GameEventType, out l_EventContainer))
            {
                l_EventContainer.removeCallback(a_EventCallback);
            }
        }

        /// <summary>
        /// Fires event subscribed
        /// </summary>
        /// <param name="a_GameEventType"></param>
        /// <param name="a_HashtableArgs"></param>
        public static void Dispatch(GAME_EVENT_TYPE a_GameEventType, EventHash a_HashtableArgs)
        {
            GameEventContainer l_EventContainer = null;
            if (s_Instance.m_dictGameEvents.TryGetValue(a_GameEventType, out l_EventContainer))
            {
                l_EventContainer.dispatch(a_HashtableArgs);
            }
            s_Instance.returnHashtableToPool(a_HashtableArgs);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Logs events and types of objects and its method name
        /// </summary>
        public static void LogEvents()
        {
            System.Text.StringBuilder l_stringBuilder = new System.Text.StringBuilder();
            foreach (KeyValuePair < GAME_EVENT_TYPE, GameEventContainer> l_keyValEvent in s_Instance.m_dictGameEvents)
            {
                l_stringBuilder.Append("\n\n");
                l_stringBuilder.Append(l_keyValEvent.Value.getLog());
            }
            Debug.Log(l_stringBuilder.ToString());
        }
#endif
    }
}