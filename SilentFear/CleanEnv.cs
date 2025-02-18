using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanEnv : MonoBehaviour
{
    public GameObject envMap;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
            Progress.Instance.PlayerInfo.spawnBackgroubd = 1; 
            Destroy(envMap);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Progress.Instance.PlayerInfo.spawnBackgroubd = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
