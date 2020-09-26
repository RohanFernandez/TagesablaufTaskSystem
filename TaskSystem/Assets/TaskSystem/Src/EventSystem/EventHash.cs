using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public class EventHash : Hashtable, IReusable
    {
        public void onRetrievedFromPool()
        {
            
        }

        public void onReturnedToPool()
        {
            
        }
    }
}