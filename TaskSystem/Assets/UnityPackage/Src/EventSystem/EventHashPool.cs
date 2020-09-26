using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public class EventHashPool : ObjectPool<EventHash>
    {
        public EventHashPool(int a_iStartSize = 0)
            : base(a_iStartSize)
        {
        }
    }
}