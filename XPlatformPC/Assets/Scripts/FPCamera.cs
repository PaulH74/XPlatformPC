using UnityEngine;
using Photon.Pun;

namespace XPlatformPC
{
    public class FPCamera : MonoBehaviourPun
    {
        public float sensitivity = 5.0f;
        public float smoothing = 2.0f;
        // the chacter is the capsule
        public GameObject character;
        public PhotonView phView;
        
        // get the incremental value of mouse moving
        private Vector2 mouseLook;
        // smooth the mouse moving
        private Vector2 smoothV;
        

        // Update is called once per frame
        void Update()
        {
            if (phView.IsMine)
            {
                Vector2 mouseDirection = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
                mouseDirection = Vector2.Scale(mouseDirection, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
                
                // Smoothing
                smoothV.x = Mathf.Lerp(smoothV.x, mouseDirection.x, 1f / smoothing);
                smoothV.y = Mathf.Lerp(smoothV.y, mouseDirection.y, 1f / smoothing);
                
                // Incrementally add to the camera look
                mouseLook += smoothV;
                
                // Rotate Camera and GameObject accordingly
                transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);        // vector3.right = x-axis
                character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
            }
        }
    }
}
