using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Keeps information on bullets and handles simple autonomous operations
 */
public class BruteProjectileScript : MonoBehaviour
{
    private Vector3 _fireDirection = new Vector3();

    public bool inactive = false;

    public void SetFire(Vector3 startVector, Vector3 fireDirection)
    {
        inactive = false;
        _fireDirection = fireDirection;
        transform.position = startVector;
    }

    public void Terminate()
    {
        inactive = true;
        transform.Translate(0f,-10f,0f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (inactive) return;
        var move = _fireDirection * Time.deltaTime;
        transform.Translate(move);
    }
}
