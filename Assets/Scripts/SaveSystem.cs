using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    private static string file = "/save.bin";

    public static void Save(Jelly player) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + file;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData Load() {
        string path = Application.persistentDataPath + file;
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        } else {
            throw new System.Exception("save file not found");
        }
    }
}
