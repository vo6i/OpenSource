using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnZombies : MonoBehaviour
{
   
    private float spawnTime = 0;
    public Transform[] posSpwn;
    public GameObject zombS;
    public PlayerController playerController;
    public GameObject[] Trash;
    public Transform[] TrashPose;

    private float cosMicTrash = 0;
  
    
       
      // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        if(playerController.health <= 0)
        {
            this.gameObject.SetActive(false);
        }
        spawnTime += Time.deltaTime;
        int rndMnbr = Random.Range(0, posSpwn.Length);
        if(spawnTime > 10f)
        {
            Instantiate(zombS, posSpwn[rndMnbr].position, Quaternion.identity);
            spawnTime = 0;
        }
        else
        {
            spawnTime += Time.deltaTime;
        }
        
        if(cosMicTrash > Random.Range(1.3f, 11f))
        {
            
            Instantiate(Trash[rndMnbr], TrashPose[rndMnbr].position, Quaternion.identity);
            cosMicTrash = 0;
            
        }
        else
        {
            
            cosMicTrash += Time.deltaTime;
        }

        if (Application.targetFrameRate != 30)
        {
            Application.targetFrameRate = 30;
        }
    }
}
