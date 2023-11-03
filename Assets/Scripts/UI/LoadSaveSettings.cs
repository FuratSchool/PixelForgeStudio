using System.IO;
using UnityEngine;

public static class LoadSaveSettings
{
    public static SettingsData LoadData()
    {
        var saveFilePath = Application.persistentDataPath + "/Settings.json";
        Debug.Log("Tried loading file from: " + saveFilePath);
        if (File.Exists(saveFilePath))
        {
            var loadSettings = File.ReadAllText(saveFilePath);
            var settings = JsonUtility.FromJson<SettingsData>(loadSettings);
            return settings;
        }
        else
        {
            return NewData();
        }
    }
    
    public static void SaveData(SettingsData settings)
    {
        var saveFilePath = Application.persistentDataPath + "/Settings.json";
        var saveSettingsData = JsonUtility.ToJson(settings);
        File.WriteAllText(saveFilePath, saveSettingsData);
    }
    
    public static SettingsData NewData()
    {
        var settings = new SettingsData("",1920,1080,0,0,0,true,false,false,5f,5f);
        SaveData(settings);
        return settings;
    }
}
