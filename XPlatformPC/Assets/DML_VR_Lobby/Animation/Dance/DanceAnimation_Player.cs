using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanePC
{
    public class DanceAnimation_Player : MonoBehaviour
    {
        Animator animator;
        bool _IsLocked;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            {

                animator.SetTrigger("Trigger_Dance1");
                // GetComponent<Renderer>(enabled) = !GetComponent<Renderer>.enabled;   
            }

            if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
            {
                animator.SetTrigger("Trigger_Dance2");
                // GetComponent<Renderer>(enabled) = !GetComponent<Renderer>.enabled;
            }
        }
    }
}
