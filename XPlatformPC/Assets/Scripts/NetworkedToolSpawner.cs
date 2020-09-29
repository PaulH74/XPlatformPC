using Photon.Pun;

namespace KanePC
{
    /// <summary>
    /// This class is responsible for syncing the showing / hiding of players' tools over the network.
    /// </summary>
    public class NetworkedToolSpawner : MonoBehaviourPun
    {
        public void DisplayTool(bool display)
        {
            photonView.RPC("SyncDisplayTool", RpcTarget.All, display);
        }

        /// <summary>
        /// This RPC shows / hides a networked player's tool in the local player's instance, when activated by the networked player's ToolSpawner.cs
        /// </summary>
        /// <param name="display"></param>
        [PunRPC]
        public void SyncDisplayTool(bool display)
        {
            gameObject.SetActive(display);
        }
    }
}