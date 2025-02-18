using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsGenerator : MonoBehaviour
{
    public Text textCount;
    public Transform[] missionPositions;
    public GameObject[] itemsTrash;
    //public SpawnGridWithRandomizedPosition spawnTrashPerfect;
    // int trashCounts = 0;
    // int energyBag = 0;
    //int moneyCost = 0;
    public Transform missionDebug;
    public int missionCountTrash = 0;
  //  public RaycastAlignerNoOverlap raycastAlignerNoOverlap;
    // Start is called before the first frame update
    void Start()
    {
        //  missionDebug.position = transform.position;
        // GenerateMissions();   
        
    }

    // Update is called once per frame
    public void GenerateMissions()
    {
        int level = PlayerPrefs.GetInt("Level");
        Pick();
        for(int i = 0; i <= level; i++)
        {
            missionCountTrash = 50 + i * level;
            textCount.text = missionCountTrash.ToString() + " neeeded";
        }
    }

    void Pick()
    {
        int randomIndex = Random.Range(0, missionPositions.Length);
        missionDebug.position = missionPositions[randomIndex].position;
        float posX = missionDebug.position.x;
        float posZ = missionDebug.position.z;


        for (int i = 0; i < missionPositions.Length; i++)
        {
            Vector3 newPosTrash = new Vector3(posX + randomIndex + i * Random.Range(-1, 3), 1, Random.Range(-1, 3) * i + randomIndex + posZ);
            GameObject clone = Instantiate(itemsTrash[randomIndex], newPosTrash, transform.rotation);
        }
           
    }
}
