using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BoomerangScript : MonoBehaviour
    {
        public bool active = false;
        public Transform objectTransform;
        private BruteScript _targetBrute = null;
        private MainCharacterScript _mainGuy;
        private float speed;
        private bool _returning = false;
        public void SetFire(BruteScript target, float boomerangSpeed, MainCharacterScript mainGuy)
        {
            _targetBrute = target;
            transform.position = mainGuy.transform.position;
            speed = boomerangSpeed;
            _mainGuy = mainGuy;
            _returning = false;
            active = true;
        }


        private void MoveTo(Vector3 target)
        {
            
            var difference = target - transform.position;
            var t = (difference.normalized) * (speed * (_returning?2f:1f) * Time.deltaTime); // returns faster
            var nx = -t.z;
            var nz = t.x;
            transform.Translate(t.x+nx,0f,t.z+nz);
            var rotY = objectTransform.rotation.eulerAngles.y;
            objectTransform.rotation = Quaternion.Euler(90f,rotY + 720f*Time.deltaTime,0f);

            if (difference.magnitude < .2f)
            {
                if (_returning)
                {
                    Terminate();
                }else
                {
                    _returning = true;
                }
            }
            
            
        }

        private void Terminate()
        {
            active = false;
            transform.Translate(0f,-10f,0f);
        }
        
        
        
        
        private void Update()
        {
            if (active)
            {
                MoveTo(_returning ? _mainGuy.transform.position:_targetBrute.transform.position);
                
                
            }
            
            
        }
    }
}