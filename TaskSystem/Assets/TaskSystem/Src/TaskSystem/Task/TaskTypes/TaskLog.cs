using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public class TaskLog : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_KEY_LOG_TYPE = "LogType";
        private const string ATTRIBUTE_KEY_LOG_MESSAGE = "LogMsg";
        #endregion ATTRIBUTE_KEY

        private const string ATTRIBUTE_VALUE_LOG_ERROR = "ERROR";
        private const string ATTRIBUTE_VALUE_LOG_INFO = "INFO";
        private const string ATTRIBUTE_VALUE_LOG_WARNING = "WARNING";

        private string m_strLogType = string.Empty;
        private string m_strLogMessage = string.Empty;

        public override void onInitialize()
        {
            base.onInitialize();

            m_strLogType = getString(ATTRIBUTE_KEY_LOG_TYPE);
            m_strLogMessage = getString(ATTRIBUTE_KEY_LOG_MESSAGE);
        }

        public override void onExecute()
        {
            base.onExecute();

            if (m_strLogType.Equals(ATTRIBUTE_VALUE_LOG_INFO, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log(m_strLogMessage);
            }
            else if (m_strLogType.Equals(ATTRIBUTE_VALUE_LOG_ERROR, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogError(m_strLogMessage);
            }
            else if (m_strLogType.Equals(ATTRIBUTE_VALUE_LOG_WARNING, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning(m_strLogMessage);
            }

            onComplete();
        }
    }
}