using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

/** Handles coin tweening and holds information
 * 
 */
public class CoinScript : MonoBehaviour
{
    public GameObject coin;
    private float _coinPeriod = 2f;
    private float _coinTime = 0f;
    [NonSerialized]
    public float SpawnRadius = 30f;
    
    
    void Update()
    {
        var alpha = _coinTime / _coinPeriod;
        _coinTime = (_coinTime+Time.deltaTime)%_coinPeriod;
        coin.transform.localPosition = new Vector3(0f,2f+ math.sin(alpha*6.282f),0f);
        coin.transform.rotation = Quaternion.Euler(0f,360f*alpha,0f);
    }
    
    

    public void SetNewPosition(Vector3 mainPos)
    {
        var rc = Random.insideUnitCircle*SpawnRadius;
        var newPos = mainPos + new Vector3(rc.x,0f,rc.y);
        _coinTime = Random.Range(0f, _coinPeriod);
        transform.position = newPos;
    }

    public void ResetCoin(Vector3 mainPos)
    {
        SetNewPosition(mainPos);
    }
}
