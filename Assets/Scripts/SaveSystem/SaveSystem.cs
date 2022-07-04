using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    public static void SaveNewGame() => SaveProgress(true);
    public static void SaveProgress(bool newGame=false)
    {
        if (!newGame&&!SceneManager.GetSceneByName("_MainManor").isLoaded)
        {
            Debug.LogError("Save Unsuccessful outside of Main Manor");
            return;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath+"/Butler.clue";

        FileStream stream = new FileStream(path, FileMode.Create);

        ProgressFile file = new ProgressFile();
        if(newGame) file.Fresh = true;
        else file.Save();

        formatter.Serialize(stream, file);
        stream.Close();
    }

    public static void LoadProgress()
    {
        if (!SceneManager.GetSceneByName("_MainManor").isLoaded)
        {
            Debug.LogWarning("Load Unsuccessful outside of Main Manor");
            return;
        }
        ProgressFile lastSave = GetSavedProgress();
        if (lastSave == null) return;
        lastSave.Load();
    }
    public static ProgressFile GetSavedProgress()
    {
        string path = Application.persistentDataPath + "/Butler.clue";

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file exists");
            return null;
        }
        else
        {
            BinaryFormatter formatter= new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ProgressFile file = formatter.Deserialize(stream) as ProgressFile;

            stream.Close();
            return file;
        }

    }
}
