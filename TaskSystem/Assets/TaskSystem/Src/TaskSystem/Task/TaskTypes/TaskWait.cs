using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public class TaskWait : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_WAIT_TIME = "WaitTime";
        #endregion ATTRIBUTE_KEY

        private float m_fWaitTime = 0.0f;
        private float m_fTimePassed = 0.0f;

        public override void onInitialize()
        {
            base.onInitialize();

            m_fWaitTime = getFloat(ATTRIBUTE_WAIT_TIME);
        }

        public override void onExecute()
        {
            base.onExecute();
            m_fTimePassed = 0.0f;
        }

        public override void onUpdate()
        {
            base.onUpdate();
            m_fTimePassed += Time.deltaTime;
            if (m_fTimePassed >= m_fWaitTime)
            {
                onComplete();
            }
        }
    }
}