using UnityEngine;
using Photon.Pun;

namespace KanePC
{
    /// <summary>
    /// This class is responsible for controlling the Laser Pointer tool, generating the "laser-beam" and hit point objects, as well
    /// as syncing these objects over the network.
    /// </summary>
    public class LaserPointerController : MonoBehaviourPun, IPunObservable
    {
        #region Public and Private Attributes
        // Public Attributes
        public GameObject laserBeam;
        public GameObject laserHitPoint;
        public GameObject teleportHitPoint;
        public LayerMask validLaserHitLayer;

        // Private Attributes
        private const float LASER_CENTRE_POINT = 1.5f;          // Half the length of the beam (in z-axis), update if this changes
        private Material _NormalLaserHitPoint;                   // Red Laser Point
        private bool _LaserOn;
        private Vector3 _LaserLength;
        private PlayerPCMgrPUN _PlayerMGR;
        private bool _TeleportObjectTriggered;
        private Vector3 _LaserTeleportPointPosition;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            _PlayerMGR = GetComponentInParent<PlayerPCMgrPUN>();
            _NormalLaserHitPoint = laserHitPoint.GetComponent<MeshRenderer>().material;
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                laserBeam.SetActive(false);
                laserHitPoint.SetActive(false);
                teleportHitPoint.SetActive(false);
                _LaserLength = laserBeam.transform.localScale;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                    // Show and update laser beam
                    if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) || (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)))
                    {
                        // Turn on laser
                        if (!laserBeam.activeSelf)
                        {
                            _LaserOn = true;
                            ActivateLaserBeam(_LaserOn);
                        }

                        // Shoot RayCast and check if hit object collider (only applies to valid Layer)
                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, validLaserHitLayer))
                        {
                            // Position and scale the laser
                            laserBeam.transform.position = Vector3.Lerp(transform.position, hit.point, 0.5f);
                            laserBeam.transform.localScale = new Vector3(laserBeam.transform.localScale.x, laserBeam.transform.localScale.y, hit.distance);

                            // Position and show the laser hit point
                            if (hit.collider.CompareTag("Floor"))
                            {
                                // Get hit point and swap laser hit point object with teleport target object
                                laserHitPoint.SetActive(false);
                                teleportHitPoint.transform.position = hit.point;
                                teleportHitPoint.transform.rotation = Quaternion.identity;
                                teleportHitPoint.SetActive(true);

                                // Assign Teleport Point and Enable Teleport Trigger
                                _LaserTeleportPointPosition = hit.point;
                                _TeleportObjectTriggered = true;
                            }
                            else if (hit.collider.CompareTag("Roof"))
                            {
                                // Get hit point and swap laser hit point object with teleport target object
                                laserHitPoint.SetActive(false);
                                teleportHitPoint.transform.position = hit.point;
                                teleportHitPoint.transform.rotation = Quaternion.identity;
                                teleportHitPoint.SetActive(true);

                                // Assign Teleport Point and Enable Teleport Trigger
                                _LaserTeleportPointPosition = hit.point;
                                _TeleportObjectTriggered = true;
                            }
                            else
                            {
                                // Get hit point and swap teleport target object with laser hit point object
                                laserHitPoint.transform.position = hit.point;
                                laserHitPoint.SetActive(true);
                                teleportHitPoint.SetActive(false);

                                // Disable Teleport Trigger
                                _TeleportObjectTriggered = false;
                            }
                        }
                        else
                        {
                            laserHitPoint.SetActive(false);
                            teleportHitPoint.SetActive(false);
                            laserBeam.transform.localScale = _LaserLength;
                            laserBeam.transform.localPosition = Vector3.zero;
                            laserBeam.transform.Translate(Vector3.forward * LASER_CENTRE_POINT);

                            // Disable Teleport Trigger
                            _TeleportObjectTriggered = false;
                        }
                    }
                    else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger) || OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                    {
                        if (_TeleportObjectTriggered)
                        {
                            _PlayerMGR.gameObject.transform.position = _LaserTeleportPointPosition;
                            _TeleportObjectTriggered = false;
                        }
                    }
                    else
                    {
                        // Turn off beam when trigger button is released
                        if (laserBeam.activeSelf)
                        {
                            DeactivateLaser();
                        }
                    }
                }
                else
                {
                    // Turn off beam when player releases laser pointer tool
                    if (laserBeam.activeSelf)
                    {
                        DeactivateLaser();
                    }
                }
        }
        #endregion

        #region Laser Methods
        private void DeactivateLaser()
        {
            _LaserOn = false;
            laserHitPoint.SetActive(false);
            teleportHitPoint.SetActive(false);
            laserBeam.SetActive(false);

            // Disable Teleport Trigger
            _TeleportObjectTriggered = false;
        }

        /// <summary>
        /// Toggles the laser beam on / off
        /// </summary>
        /// <param name="activate"></param>
        private void ActivateLaserBeam(bool activate)
        {
            laserBeam.SetActive(activate);
        }
        #endregion

        #region PUN OnPhotonSerializeView Method
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);                        // Laser tool
                stream.SendNext(transform.rotation);
                stream.SendNext(_LaserOn);                                  // Laser Beam
                stream.SendNext(laserBeam.transform.localScale);            // Update scale when hit / not hit events occur
                stream.SendNext(laserBeam.transform.position);
                stream.SendNext(laserHitPoint.activeSelf);                  // Laser Hit Point
                stream.SendNext(laserHitPoint.transform.position);          // Update laser hit point position
                stream.SendNext(teleportHitPoint.activeSelf);               // Teleport Hit Point
                stream.SendNext(teleportHitPoint.transform.position);       // Update Teleport hit point position
                stream.SendNext(teleportHitPoint.transform.rotation);       // Update Teleport hit point rotation
            }
            else
            {
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();
                ActivateLaserBeam((bool)stream.ReceiveNext());
                laserBeam.transform.localScale = (Vector3)stream.ReceiveNext();
                laserBeam.transform.position = (Vector3)stream.ReceiveNext();
                laserHitPoint.SetActive((bool)stream.ReceiveNext());
                laserHitPoint.transform.position = (Vector3)stream.ReceiveNext();
                teleportHitPoint.SetActive((bool)stream.ReceiveNext());
                teleportHitPoint.transform.position = (Vector3)stream.ReceiveNext();
                teleportHitPoint.transform.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
        #endregion
    }
}