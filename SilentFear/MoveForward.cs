using UnityEngine;

public class MoveForward : MonoBehaviour {

    public GameObject[] mapTiles;
    public Transform maPos;
    public Transform cornPos;
    float starttime = 0;
    public bool FrstFld = true;
    public bool ScndFld = true;

    public GameObject crownRaven;
    public Transform ravPos;
    int count;

    private void Start()
    {
        Instantiate(mapTiles[0], maPos.position, Quaternion.identity);
    }

    private void Update()
    {
        starttime += Time.deltaTime;
        

        

        if(starttime > 45f)
        {
            int randomNum = Random.Range(0, 4);
            if (randomNum == 0 )//&& FrstFld == true)
            {
                Instantiate(mapTiles[0], maPos.position, Quaternion.identity);
                starttime = 0;
                //FrstFld = false;
            }
           /* else if(randomNum != 0)
            {
                if(randomNum == 1)
                {
                    Instantiate(mapTiles[1], cornPos.position, Quaternion.identity);
                    starttime = 0;
                }
                else if(randomNum ==2)
                {
                    Instantiate(mapTiles[2], cornPos.position, Quaternion.identity);
                    starttime = 0;
                }
            } */
            if(randomNum == 1 )//&& ScndFld == true)
            {
                Instantiate(mapTiles[1], cornPos.position, Quaternion.identity);
                starttime = 0;
                //ScndFld = false;
            }
            if(randomNum == 2)
            {
                Instantiate(mapTiles[2], cornPos.position, Quaternion.identity);
                starttime = 0;
               // FrstFld = true;
               // ScndFld = true;
            }
            if (randomNum == 3)
            {
                Instantiate(mapTiles[3], cornPos.position, Quaternion.identity);
                starttime = 0;
                // FrstFld = true;
                // ScndFld = true;
            }
        } 
        else
        {
            starttime += Time.deltaTime;
        }

    }
}