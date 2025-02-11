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
    static bool _canSave = true;


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
                _canSave = false;
                return;
            case PlayModeStateChange.EnteredEditMode:
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
            Debug.Log("Saving Scene");
        }
    }

    private static void SaveScene()
    {
        EditorSceneManager.SaveOpenScenes();
    }

}

#endif