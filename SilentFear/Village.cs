using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Village : MonoBehaviour
{
    private IObjectPool<Village> villagePool;
   // private string playerCm = "MainCamera";
    public void SetPool(IObjectPool<Village> pool)
    {
        villagePool = pool;
    }

    // Start is called before the first frame update
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameLogic.plrCm))
        {
            Progress.Instance.PlayerInfo.spawnBackgroubd = 1;
            villagePool.Release(this);
        }
    }
}
