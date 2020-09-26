using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public interface ITask : IReusable
    {
        void onStartInitialization(Hashtable a_hashAttributes);
        void onStartExecution(System.Action a_SequenceCallbackTaskComplete);
        void onComplete();
        void onUpdate();
        string getTaskType();
    }
}