using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

/** Script for the main character
 * handles simple autonomous tasks and animation
 */
public class MainCharacterScript : MonoBehaviour
{
    public Transform attackRadius;
    public Image healthBar;
    public float boomerangTimer = 0f;
    public GameObject theGuy;
    public Animator theGuyAnimator;
    

    public float NormalDyingTime()
    {
        return theGuyAnimator.GetCurrentAnimatorStateInfo(0).IsName("MainCharacterDeath") ? 
            theGuyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime : 0f;
    }
    
    public void SetHealth(float f)
    {
        healthBar.fillAmount = f;
        healthBar.color = f<.25f ? Color.red : Color.green;
    }

    public void StartDeath()
    {
        theGuyAnimator.SetInteger("state", 2);
    }

    public void SetAttackRadius(float f)
    {
        attackRadius.transform.localScale = new Vector3(f*2f, 0f, f*2f);
    }


    public void MoveCharacter(Vector2 v2)
    {
        var angle = -math.atan2(v2.y, v2.x);
        
        transform.Translate(v2.x,0f,v2.y);
        theGuy.transform.rotation = Quaternion.Euler(0f,angle/3.141f*180f + 90f,0f);
    }

    public void IsMoving(bool b)
    {
        theGuyAnimator.SetInteger("state",b?1:0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
