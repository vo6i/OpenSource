using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlRovoJob : MonoBehaviour
{
    public static ControlRovoJob Instance;
    public bool haveMissions;
    public static int countCollected;
    public int level = 0;
    public Text overwiewCount;
    public Text levelCount;

    public MissionsGenerator missionsGenerator;
    
    // Start is called before the first frame update
    void Start()
    {
         if (Instance == null)
         {
          //   PlayerPrefs.DeleteAll();
             DontDestroyOnLoad(gameObject);
             Instance = this;
             haveMissions = false;
             level = PlayerPrefs.GetInt("Level");
            // missionsGenerator.GenerateMissions();
            // LoadExtern();
         }
         else
         {
             Destroy(gameObject);
         } 
     //   haveMissions = false;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        levelCount.text = "Level " + level.ToString();
        overwiewCount.text = "Collected " + countCollected.ToString();
        if (haveMissions == false)
        {
            missionsGenerator.GenerateMissions();
            haveMissions = true;
        }

        if(missionsGenerator.missionCountTrash < countCollected)
        {
            level++;
            PlayerPrefs.SetInt("Level", level);
            haveMissions = false;
        }
        
    }
}
