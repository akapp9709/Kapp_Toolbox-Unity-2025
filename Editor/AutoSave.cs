#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoSave
{
    private static double saveInterval = 120;
    private static double lastSaveTime;
    static bool _canSave = false;


    static AutoSave()
    {
        lastSaveTime = EditorApplication.timeSinceStartup;
        EditorApplication.update += OnEditorUpdate;
        EditorApplication.playModeStateChanged += ChangeEditorState;
    }

    private static void ChangeEditorState(PlayModeStateChange change)
    {
        switch (change)
        {
            case PlayModeStateChange.EnteredPlayMode:
                Debug.Log("Entering Play");
                _canSave = false;
                return;
            case PlayModeStateChange.EnteredEditMode:
                Debug.Log("Exiting Play");
                _canSave = true;
                return;
            default:
                return;

        }
    }

    private static void OnEditorUpdate()
    {
        if (EditorApplication.timeSinceStartup - lastSaveTime > saveInterval && _canSave)
        {
            SaveScene();
            lastSaveTime = EditorApplication.timeSinceStartup;
        }
    }

    private static void SaveScene()
    {
        EditorSceneManager.SaveOpenScenes();
    }
#endif
}