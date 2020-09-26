using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    [System.Serializable]
    public class KeyValueAttribute
    {
        /// <summary>
        /// Key of the attribute in the xml
        /// </summary>
        [SerializeField]
        public string m_strKey = string.Empty;

        /// <summary>
        /// The value of the attribute
        /// </summary>
        [SerializeField]
        public string m_strValue = string.Empty;
    }
}