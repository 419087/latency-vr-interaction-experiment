using UnityEngine;

namespace ContactGloveSDK
{
    public class FingerTipCollisionDetection : MonoBehaviour
    {
        public bool isColliding = false;
        Material mat;

        public HandSides hand;


        /// <summary>
        /// Trigger exit
        /// </summary>
        /// <param name="collision">Collider data</param>
        void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.tag == "FingerCollider")
            {
                return;
            }
            isColliding = false;
            mat.color = Color.white;
        }

         void OnTriggerStay(Collider collision)
        {
            if (collision.gameObject.tag == "FingerCollider")
            {
                return;
            }
            isColliding = true;
            mat.color = Color.red;
   
        }

        /// <summary>
        /// Initialization before the first frame update
        /// </summary>
        private void Awake()
        {
            mat = GetComponent<Renderer>().material;
        }
    }
}

