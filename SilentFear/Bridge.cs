using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bridge : MonoBehaviour
{
    private IObjectPool<Bridge> bridgePool;
    //private string playerCm = "MainCamera";

    public void SetPool(IObjectPool<Bridge> pool)
    {
        bridgePool = pool;
    }

    // Start is called before the first frame update
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameLogic.plrCm))
        {
            Progress.Instance.PlayerInfo.spawnBackgroubd = 1;
            bridgePool.Release(this);
        }
    }
}
