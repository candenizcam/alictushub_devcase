using System;
using UnityEngine;


/** This class is only to hoist animation events
 */
public class BruteInnerScript : MonoBehaviour
{
    public Action ThrowAction = ()=>{};
    public Action BruteDeadAction = () => { };
    

    public void Throw()
    {
        ThrowAction();
    }

    public void BruteDead()
    {
        BruteDeadAction();
    }
}
