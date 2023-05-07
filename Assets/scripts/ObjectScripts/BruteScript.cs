using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace DefaultNamespace
{
    /** Script for brutes for animation control, simple tasks and event hoisting from animation
     * 
     */
    public class BruteScript : MonoBehaviour
    {
        public bool active;
        public bool dying = false;
        public Animator bruteAnimator;
        public BruteInnerScript brute;
        public Action<BruteScript> ThrowAction = (BruteScript bs)=>{};
        
        
        private void Start()
        {
            brute.ThrowAction = ThrowFunction;
            brute.BruteDeadAction = Die;
        }

        private void ThrowFunction()
        {
            ThrowAction(this);
        }

        /** This function starts killing animation
         */
        public void Kill()
        {
            dying = true;
            bruteAnimator.SetInteger("state",2);
        }

        /** This function is called when kill animation is over
         */
        public void Die()
        {
            dying = false;
            active = false;
            transform.Translate(0f,-10f,0f);
        }
        
        
        public void SpawnAround(Vector3 v, float dist)
        {
            active = true;
            var f = UnityEngine.Random.Range(2.36f, 7.07f); // generates an angle to spawn the brute 135-405 deg (so it is not visible in the perspective horizon)
            var newPos = (new Vector3(math.cos(f),0f,math.sin(f)))*dist + v; // generates a point that is distance away on that angle
            transform.position = newPos; // sets the position
        }

        public void HandleMovement(Vector3 characterPos, float bruteDisplacement)
        {
            var moveVector = (characterPos - transform.position).normalized * bruteDisplacement;
            transform.Translate(moveVector);
            var angle = -math.atan2(moveVector.z, moveVector.x);
            brute.transform.rotation = Quaternion.Euler(0f,angle/3.141f*180f + 90f,0f);
        }

        /** state 0: running
         * 1: throwing
         * 2: dying
         */
        public void SetState(int state)
        {
            bruteAnimator.SetInteger("state",state);
        }
        
        
        

       
    }
}