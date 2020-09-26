using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRETBUZZ
{
    public static class GeneralUtils
    {
        #region Hashtable Utils
        /// <summary>
        /// Returns string from hashtable object
        /// Returns string.Empty if object is null
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static string GetString(Hashtable a_Hashtable, string a_strAttributeKey)
        {
            System.Object l_Obj = a_Hashtable[a_strAttributeKey];
            return (l_Obj == null) ? string.Empty : l_Obj.ToString();
        }

        /// <summary>
        /// Returns string from hashtable object
        /// Returns 0 if object is null
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static int GetInt(Hashtable a_Hashtable, string a_strAttributeKey)
        {
            System.Object l_Obj = a_Hashtable[a_strAttributeKey];
            string l_strAtrributeValue = (l_Obj == null) ? null : l_Obj.ToString();

            int l_iReturn = 0;
            if (!string.IsNullOrEmpty(l_strAtrributeValue))
            {
                int.TryParse(l_strAtrributeValue, out l_iReturn);
            }
            return l_iReturn;
        }

        /// <summary>
        /// Returns string from hashtable object
        /// Returns 0.0f if object is null
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static float GetFloat(Hashtable a_Hashtable, string a_strAttributeKey)
        {
            System.Object l_Obj = a_Hashtable[a_strAttributeKey];
            string l_strAtrributeValue = (l_Obj == null) ? null : l_Obj.ToString();

            float l_fReturn = 0.0f;
            if (!string.IsNullOrEmpty(l_strAtrributeValue))
            {
                float.TryParse(l_strAtrributeValue, out l_fReturn);
            }
            return l_fReturn;
        }

        /// <summary>
        /// Gets float from string
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static float GetFloat(string a_strAttributeKey)
        {
            float l_fReturn = 0.0f;
            if (!string.IsNullOrEmpty(a_strAttributeKey))
            {
                float.TryParse(a_strAttributeKey, out l_fReturn);
            }
            return l_fReturn;
        }

        /// <summary>
        /// Returns string from hashtable object
        /// Return false if key is null or empty
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static bool GetBool(Hashtable a_Hashtable,  string a_strAttributeKey, bool a_bDefaultValue = false)
        {
            System.Object l_Obj = a_Hashtable[a_strAttributeKey];
            string l_strAtrributeValue = (l_Obj == null) ? null : l_Obj.ToString();

            bool l_bReturn = a_bDefaultValue;
            if (!string.IsNullOrEmpty(l_strAtrributeValue))
            {
                bool.TryParse(l_strAtrributeValue, out l_bReturn);
            }
            return l_bReturn;
        }

        /// <summary>
        /// Returns vec3 from a string
        /// if unable to parse returns vec0
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static Vector3 GetVec3(Hashtable a_Hashtable, string a_strAttributeKey, Vector3 a_v3Default = default(Vector3))
        {
            System.Object l_Obj = a_Hashtable[a_strAttributeKey];
            string l_strAtrributeValue = (l_Obj == null) ? null : l_Obj.ToString();

            return GetVec3(l_strAtrributeValue, a_v3Default);
        }

        /// <summary>
        /// Gets vec3 from string in format (0.0, 0.0, 0.0)
        /// </summary>
        /// <param name="a_strAtrributeValue"></param>
        /// <param name="a_v3Default"></param>
        /// <returns></returns>
        public static Vector3 GetVec3(string a_strAtrributeValue, Vector3 a_v3Default = default(Vector3))
        {
            Vector3 l_v3Return = a_v3Default;
            if (!string.IsNullOrEmpty(a_strAtrributeValue))
            {
                string[] l_strVec3 = a_strAtrributeValue.Split(',');
                if (l_strVec3.Length == 3)
                {
                    l_v3Return = new Vector3();
                    l_v3Return.x = float.Parse(l_strVec3[0]);
                    l_v3Return.y = float.Parse(l_strVec3[1]);
                    l_v3Return.z = float.Parse(l_strVec3[2]);
                }
            }

            return l_v3Return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetStrArr(Hashtable m_hashAttributes, string a_strAttributeKey, char a_chSeparation)
        {
            System.Object l_Obj = m_hashAttributes[a_strAttributeKey];
            string l_strAtrributeValue = (l_Obj == null) ? null : l_Obj.ToString();
            string[] l_strarrReturn = null;
            if (!string.IsNullOrEmpty(l_strAtrributeValue))
            {
                l_strarrReturn = l_strAtrributeValue.Split(a_chSeparation);
            }
            return l_strarrReturn;
        }

        #endregion Hashtable Utils

        #region Interaction

        /// <summary>
        /// Is the a_iLayer in the layer mask a_LayerMask
        /// </summary>
        /// <param name="a_LayerMask"></param>
        /// <param name="a_iLayer"></param>
        /// <returns></returns>
        public static bool IsLayerInLayerMask(LayerMask a_LayerMask, int a_iLayer)
        {
            return a_LayerMask == (a_LayerMask | (1 << a_iLayer));
        }

        #endregion Interaction
    }
}