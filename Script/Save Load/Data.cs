using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data 
{
    public string sceneToSave;
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();
    public Dictionary<string, float> floatSavedData = new Dictionary<string, float>();
    
    public void SaveGameScene(GameSceneSO saveScene)
    {
        sceneToSave = JsonUtility.ToJson(saveScene);
    }
    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
}
