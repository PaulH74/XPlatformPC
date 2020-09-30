using UnityEngine;
using Photon.Pun;

public class FPController : MonoBehaviourPun
{
    public float speed = 5.0f;
    private float translation;
    private float straffe;

    // Use this for initialization
    void Start()
    {
        if (photonView.IsMine)
        {
            // turn off the cursor
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            // Input.GetAxis() is used to get the user's input
            // You can furthor set it on Unity. (Edit, Project Settings, Input)
            translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            transform.Translate(straffe, 0, translation);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // turn on the cursor
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Input.GetKey(KeyCode.R))
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                Application.Quit();
            }
        }
    }
}