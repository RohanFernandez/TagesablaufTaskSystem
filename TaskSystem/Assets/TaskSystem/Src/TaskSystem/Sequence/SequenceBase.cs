using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public class SequenceBase : ISequence
    {
        /// <summary>
        /// List of all tasks to be executed in this sequence
        /// </summary>
        public List<ITask> m_lstTasks = new List<ITask>(5);

        /// <summary>
        /// The name of the sequence class type
        /// </summary>
        private string m_strSequenceType = string.Empty;

        /// <summary>
        /// The index of the currently running task
        /// </summary>
        protected int m_iRunningTask = 0;

        /// <summary>
        /// Total tasks in this sequence
        /// </summary>
        protected int m_iTotalTasks = 0;

        /// <summary>
        /// The unique sequence ID
        /// </summary>
        protected string m_strSequenceID = string.Empty;

        public SequenceBase()
        { 
        }

        public void addTask(ITask a_Task)
        {
            m_lstTasks.Add(a_Task);
        }

        public virtual void onInitialize(string a_strSequenceID, string a_strSequenceType)
        {
            m_iRunningTask = 0;
            m_strSequenceID = a_strSequenceID;
            m_iTotalTasks = m_lstTasks.Count;
            m_strSequenceType = a_strSequenceType;
        }

        /// <summary>
        /// On execution begin of the sequence
        /// </summary>
        public virtual void onExecute()
        {
            executeTask();
        }

        /// <summary>
        /// Callback on sequence complete
        /// </summary>
        public virtual void onComplete()
        {
            EventHash l_hash = EventManager.GetEventHashtable();
            l_hash.Add(GameEventTypeConst.ID_SEQUENCE_REF, this);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_SEQUENCE_COMPLETE, l_hash);
        }

        /// <summary>
        /// Called on returned the sequence back into the pool
        /// </summary>
        public virtual void onReturnedToPool()
        {
            m_lstTasks.Clear();
        }

        public virtual void onRetrievedFromPool()
        {
            
        }

        /// <summary>
        /// Executes the next task in line
        /// </summary>
        public virtual void executeTask()
        {
            if (m_iRunningTask < m_iTotalTasks)
            {
                m_lstTasks[m_iRunningTask].onStartExecution(onTaskComplete);
            }
            else
            {
                onComplete();
            }
        }

        /// <summary>
        /// on any task complete
        /// </summary>
        public virtual void onTaskComplete()
        {
            //execute next task
            m_iRunningTask++;
            executeTask();
        }

        /// <summary>
        /// Updates with delta time on running sequence
        /// </summary>
        /// <param name="a_fDeltaTime"></param>
        public virtual void onUpdate()
        {
            if (m_iRunningTask < m_iTotalTasks)
            {
                m_lstTasks[m_iRunningTask].onUpdate();
            }
        }

        /// <summary>
        /// Returns the unique sequence id
        /// </summary>
        /// <returns></returns>
        public string getSequenceID()
        {
            return m_strSequenceID;
        }

        /// <summary>
        /// Returns the type of sequence
        /// </summary>
        /// <returns></returns>
        public string getSequenceType()
        {
            return m_strSequenceType;
        }
    }
}