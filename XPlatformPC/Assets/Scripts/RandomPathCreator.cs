using UnityEngine;

namespace XPlatformPC
{
    /// <summary>
    /// This class selects a random path from the path array 
    /// </summary>
    public class RandomPathCreator : MonoBehaviour
    {
        public GameObject[] allPaths;

        /// <summary>
        /// Creates a random index value, which is used to select a random path from the array
        /// </summary>
        /// <returns></returns>
        public GameObject GetRandomPath()
        {
            // Get a random index number from array
            int index = Random.Range(0, allPaths.Length);

            return allPaths[index];
        }
    }
}