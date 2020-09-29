using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceAnimation_Player2 : MonoBehaviour
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

            animator.SetTrigger("dance1trigger");
           // GetComponent<Renderer>(enabled) = !GetComponent<Renderer>.enabled;   
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            animator.SetTrigger("dance2trigger");
           // GetComponent<Renderer>(enabled) = !GetComponent<Renderer>.enabled;
        }
    }
}
