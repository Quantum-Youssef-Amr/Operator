using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class SaveEngine : MonoBehaviour
{
    public bool useEncryption = false;
    public static SaveEngine Instance { get; private set; }
    public Data Data { get; set; }

    private string PATH;
    private readonly string SaveFile = "Save.ysa";
    private readonly string EncryptionKey = "YoussefAmr";

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("there is more than one Save engine");
            return;
        }
        Instance = this;
        PATH = Application.persistentDataPath;
        LoadData();

        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        Data = new();
    }

    public void SaveData()
    {
        string m_saveFullPath = Path.Combine(PATH, SaveFile);
        string m_saveData = JsonUtility.ToJson(Data, true);
        string m_EncryptedData = useEncryption ? EncryptDecryptData(m_saveData) : m_saveData;

        using (FileStream stream = new(m_saveFullPath, FileMode.OpenOrCreate))
        {
            using (StreamWriter writer = new(stream))
            {
                writer.Write(m_EncryptedData);
            }
        }

    }

    public void LoadData()
    {
        string m_saveFullPath = Path.Combine(PATH, SaveFile);
        if (!File.Exists(m_saveFullPath))
        {
            NewGame();
            return;
        }

        string m_loadedEncryptedData = "";
        using (FileStream stream = new(m_saveFullPath, FileMode.Open))
        {
            using (StreamReader reader = new(stream))
            {
                m_loadedEncryptedData = reader.ReadToEnd();
            }
        }

        string m_decryptedData = useEncryption ? EncryptDecryptData(m_loadedEncryptedData) : m_loadedEncryptedData;
        Data m_loadedData = JsonUtility.FromJson<Data>(m_decryptedData);
        Data = m_loadedData;
    }

    public bool IsSaved()
    {
        return File.Exists(Path.Join(PATH, SaveFile));
    }

    private string EncryptDecryptData(string EncryptedData)
    {
        StringBuilder m_sb = new();
        int m_keyIndex = 0;
        foreach (char c in EncryptedData)
        {
            m_sb.Append((char)(c ^ EncryptionKey[m_keyIndex]));
            m_keyIndex = ++m_keyIndex % EncryptionKey.Length;
        }

        return m_sb.ToString();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveData();
    }

    void OnApplicationQuit()
    {
        SaveData();
    }
}
