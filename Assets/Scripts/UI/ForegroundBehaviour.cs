using UnityEngine;

namespace Behaviours
{
   namespace UI
   {
      public class ForegroundBehaviour : MonoBehaviour
      {
         [SerializeField] private bool m_up;

         void Update()
         {
            RectTransform l_transform = GetComponent<RectTransform>();
            Vector3 l_position = l_transform.position;
            l_position.y += m_up ? 0.01f : -0.01f;
            l_transform.position = l_position;

            Vector3[] l_corners = new Vector3[4];
            Vector3[] l_canvasCorners = new Vector3[4];

            l_transform.GetWorldCorners(l_corners);
            GetComponentInParent<RectTransform>().GetWorldCorners(l_canvasCorners);

            foreach (Vector3 l_corner in l_corners)
            {
               if (l_corner.y < l_canvasCorners[0].y || l_corner.y > l_canvasCorners[2].y)
               {
                  DestroyImmediate(gameObject);
               }
            }
         }
      }
   }
}
