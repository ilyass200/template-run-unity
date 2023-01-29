using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveManager
{

    const string PATH = "data.json";

    public static void SaveData(GameData Data){
        string path = Application.persistentDataPath + $"/{PATH}";

        try{
            if (File.Exists(path)) File.Delete(path); // If file Deleting old file and writing a new one
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(Data));
        } catch (Exception e) {
            Debug.LogWarning($"Unable to save gameData due to: {e.Message} {e.StackTrace}");
        }
    }

    public static GameData LoadData(){
        string path = Application.persistentDataPath + $"/{PATH}";
        if (!File.Exists(path)) SaveData(new GameData());
        try{
            GameData gameData;
            gameData = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(path));
            return gameData;
        } catch (Exception e){
            Debug.LogWarning($"Failed to load gameData due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
