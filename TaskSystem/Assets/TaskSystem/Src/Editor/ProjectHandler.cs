using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

#if UNITY_EDITOR
namespace FRETBUZZ
{
    [ExecuteInEditMode]
    public class ProjectHandler : EditorWindow
    {
        /// <summary>
        /// The singleton, makes sure there is only a single
        /// </summary>
        private static ProjectHandler s_Instance = null;

        #region SCRIPTABLE OBJECT CREATION

        private static string m_strTaskListXMLSrc = string.Empty;
        private static string m_strTaskListAssetDest = string.Empty;

        private const string DEFAULT_TASK_LIST_ASSET_DEST = "Assets\\TaskSystem\\Example\\Resources\\TaskListAssetDest";
        private const string DEFAULT_TASK_LIST_XML_SRC = "Assets\\TaskSystem\\Example\\TaskListXMLSrc";

        private const string XML_EXTENSION = ".xml";

        public enum ASSET_TYPE
        { 
            TASK_LIST       = 1
        }

        static void CreateAllScriptableObject()
        {
            CreateAssetsOfType(m_strTaskListXMLSrc + "\\", ASSET_TYPE.TASK_LIST);
        }

        /// <summary>
        /// Creates assets from the given XML folder
        /// </summary>
        /// <param name="a_strXMLPath"></param>
        /// <param name="a_AssetType"></param>
        private static void CreateAssetsOfType(string a_strXMLPath, ASSET_TYPE a_AssetType)
        {
            DirectoryInfo l_info = new DirectoryInfo(a_strXMLPath);
            FileInfo[] l_fileInfo = l_info.GetFiles();
            foreach (FileInfo l_file in l_fileInfo)
            {
                if (l_file.FullName.Contains(".xml.meta"))
                {
                    continue;
                }
                else if (l_file.FullName.Contains(".xml"))
                {
                    string l_strFileName = l_file.Name.Remove(l_file.Name.Length - XML_EXTENSION.Length, XML_EXTENSION.Length);

                    switch (a_AssetType)
                    {
                        case ASSET_TYPE.TASK_LIST:
                            {
                                CreateTaskListScriptableObject(l_file.FullName, l_strFileName);
                                break;
                            }
                    }
                }
                else
                {
                    Debug.LogError("ProjectHandler::CreateAllScriptableObject:: Cannot create scriptable object with file with name at location '" + l_file.FullName + "'");
                }
            }
        }

        /// <summary>
        /// Creates task list asset with given name and stores it into the location
        /// </summary>
        /// <param name="a_strAssetNameToSave"></param>
        static void CreateTaskListScriptableObject(string a_strDataPathName, string a_strAssetName)
        {
            UnityEditor.AssetDatabase.CreateAsset(TaskList.GetTaskListFromXML(a_strDataPathName), m_strTaskListAssetDest + "\\" + a_strAssetName + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }

        #endregion SCRIPTABLE OBJECT CREATION

        #region SEQUENCE EXECUTION



        #endregion SEQUENCE EXECUTION

        #region EDITOR POP UP WINDOW

        [UnityEditor.MenuItem("Task System//Manager")]
        private static void OpenPopUpWindow()
        {
            if (s_Instance == null)
            {
                s_Instance = (ProjectHandler)EditorWindow.GetWindow(typeof(ProjectHandler));
                s_Instance.titleContent = new GUIContent("Manager");
                s_Instance.Show();
            }
        }

        Rect m_RectSequenceExecute;
        string m_strSequenceToExecute = string.Empty;

        void OnGUI()
        {
            GUILayout.Space(20.0f);

            GUILayout.Label("-----\tCREATION\t-----", EditorStyles.boldLabel);

            GUILayout.Label("Task List XML Source Path (ex 'Assets\\FOLDER_NAME')", EditorStyles.boldLabel);

            if (string.IsNullOrEmpty(m_strTaskListXMLSrc))
            {
                m_strTaskListXMLSrc = DEFAULT_TASK_LIST_XML_SRC;
            }

            m_strTaskListXMLSrc = EditorGUILayout.TextField(m_strTaskListXMLSrc);

            GUILayout.Label("Task List Asset Destination Path (ex 'Assets\\FOLDER_NAME') : ", EditorStyles.boldLabel);

            if (string.IsNullOrEmpty(m_strTaskListAssetDest))
            {
                m_strTaskListAssetDest = DEFAULT_TASK_LIST_ASSET_DEST;
            }

            m_strTaskListAssetDest = EditorGUILayout.TextField(m_strTaskListAssetDest);

            GUILayout.Space(8.0f);
            if (GUILayout.Button("Create All Scriptable Objects", GUILayout.Width(250)))
            {
                CreateAllScriptableObject();
            }

            GUILayout.Space(30.0f);

            GUILayout.Label("-----\tEXECUTION\t-----", EditorStyles.boldLabel);
            m_strSequenceToExecute = EditorGUILayout.TextField("Sequence Name:", m_strSequenceToExecute);
            if (GUILayout.Button("Execute Sequence", GUILayout.Width(120)))
            {
                TaskManager.ExecuteSequence(m_strSequenceToExecute);
            }
            if (GUILayout.Button("Stop Sequence", GUILayout.Width(120)))
            {
                TaskManager.StopSequence(m_strSequenceToExecute);
            }
            if (GUILayout.Button("Stop All", GUILayout.Width(120)))
            {
                TaskManager.StopAll();
            }
            if (GUILayout.Button("Log Tasks", GUILayout.Width(120)))
            {
                TaskManager.LogRunningSequences();
            }

            GUILayout.Space(10.0f);
        }

        #endregion EDITOR POP UP WINDOW
    }
}
#endif