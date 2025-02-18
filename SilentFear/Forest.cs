using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Forest : MonoBehaviour
{
    private IObjectPool<Forest> forestPool;
   // private string playerCm = "MainCamera";
    public void SetPool(IObjectPool<Forest> pool)
    {
        forestPool = pool;
    }

    // Start is called before the first frame update
    

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameLogic.plrCm))
        {
            Progress.Instance.PlayerInfo.spawnBackgroubd = 1;
            forestPool.Release(this);
        }
    }
}
