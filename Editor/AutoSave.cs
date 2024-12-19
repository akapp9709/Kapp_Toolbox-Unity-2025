using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoSave
{
    private static double saveInterval = 120;
    private static double lastSaveTime;

    static AutoSave()
    {
        lastSaveTime = EditorApplication.timeSinceStartup;
        EditorApplication.update += OnEditorUpdate;
    }

    private static void OnEditorUpdate()
    {
        if (EditorApplication.timeSinceStartup - lastSaveTime > saveInterval)
        {
            SaveScene();
            lastSaveTime = EditorApplication.timeSinceStartup;
        }
    }

    private static void SaveScene()
    {
        EditorSceneManager.SaveOpenScenes();
    }

}