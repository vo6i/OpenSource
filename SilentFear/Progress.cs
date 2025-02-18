//using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public int connectedAccount;
    public int bearS;
    public int bestScore;
    public int spawnBackgroubd;
    public int gunNumber;
    public int gameOver;
}

public class Progress : MonoBehaviour
{

    public PlayerInfo PlayerInfo;
   /* [DllImport("__Internal")]
    private static extern void SaveExtern(string date);
    [DllImport("__Internal")]
    private static extern void LoadExtern(); */
    [SerializeField] TextMeshProUGUI _playerInfoText;
    public static Progress Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
          // This is function only for YANDEX GAMES BUT I NOT LOVE THIS  LoadExtern();
            //#if UNITY_WEBGL
            // LoadExtern();
            //#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        /*  if(PlayerInfo.gameOver == 1)
          {
              Save();  // This is function only for YANDEX GAMES BUT I NOT LOVE THIS
          }
          /*if (Input.GetKeyDown(KeyCode.Backspace))
          {
              PlayerInfo = new PlayerInfo();
              Save();
          }*/
    }

    public void Save()
    {
        string jsonString = JsonUtility.ToJson(PlayerInfo);
//#if UNITY_WEBGL
     // This is function only for YANDEX GAMES BUT I NOT LOVE THIS    SaveExtern(jsonString);
//#endif
    }

    public void SetPlayerInfo(string value)
    {
        PlayerInfo = JsonUtility.FromJson<PlayerInfo>(value);
        if (_playerInfoText)
        {
            _playerInfoText.text = PlayerInfo.bestScore + "\n";//+ PlayerInfo.Level + "\n" + PlayerInfo.currentLevel + "\n" + PlayerInfo.totalExperience;
        } 
    }

}
