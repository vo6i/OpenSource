using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Corn : MonoBehaviour
{
    private IObjectPool<Corn> cornPool;
   // private string playerCm = "MainCamera";
    public void SetPool(IObjectPool<Corn> pool)
    {
        cornPool = pool;
    }

    // Start is called before the first frame update
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameLogic.plrCm))
        {
            Progress.Instance.PlayerInfo.spawnBackgroubd = 1;
            cornPool.Release(this);
        }
    }
}
