using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public interface ISequencePool
    {
        void returnToPool(ISequence a_Sequence);
        ISequence getSequence();

        int getActiveObjectCount();
        int getPooledObjectCount();
    }

    public class SequencePool : ObjectPool<SequenceBase>, ISequencePool
    {
        public SequencePool(int a_iStartSize = 0)
            : base(a_iStartSize)
        {
        }

        public void returnToPool(ISequence a_Sequence)
        {
            base.returnToPool(a_Sequence as SequenceBase);
        }

        public ISequence getSequence()
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