using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FruitInfo
{
    public string id;
    public string nama;
    public string penjelasanSingkat;
    public string penjelasanLengkap;
    public string gambar;
}

[System.Serializable]
public class FruitDatabase
{
    public List<FruitInfo> fruits;
}

public class FruitDataManager : MonoBehaviour
{
    public static FruitDataManager Instance;
    public FruitDatabase database;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadDatabase()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("FruitData");
        if (jsonFile != null)
            database = JsonUtility.FromJson<FruitDatabase>(jsonFile.text);
    }

    public FruitInfo GetFruit(string id)
    {
        return database.fruits.Find(f => f.id.ToLower() == id.ToLower());
    }
}