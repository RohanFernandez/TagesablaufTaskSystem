using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public interface ISequence : IReusable
    {
        void onInitialize(string a_strSequenceID, string a_strSequenceType);

        void onExecute();

        void onComplete();

        void addTask(ITask a_Task);

        void onUpdate();

        string getSequenceID();

        string getSequenceType();
    }
}