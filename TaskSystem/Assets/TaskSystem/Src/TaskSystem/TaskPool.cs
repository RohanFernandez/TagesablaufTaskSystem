using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public interface ITaskPool
    {
        void returnToPool(ITask a_TaskBase);
        ITask getTask();

        int getActiveObjectCount();
        int getPooledObjectCount();
    }

    public class TaskPool : ObjectPool<ITask>, ITaskPool
    {
        public TaskPool(string a_strTaskType, int a_iStartSize = 0)
            : base()
        {
            m_Type = System.Type.GetType("FRETBUZZ." + a_strTaskType);

            m_Pool = new Stack<ITask>(a_iStartSize);

            for (int l_iIndex = 0; l_iIndex < a_iStartSize; l_iIndex++)
            {
                createObj();
            }
        }

        public ITask getTask()
        {
            return getObject();
        }

        public int getActiveObjectCount()
        {
            return getActiveList().Count;
        }

        public int getPooledObjectCount()
        {
            return getPooledList().Count;
        }
    }
}