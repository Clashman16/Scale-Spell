using Managers;
using UnityEngine;

namespace Behaviours
{
   namespace Interactables
   {
      public class ScalablePartBehaviour : MonoBehaviour
      {
         private void OnMouseEnter()
         {
            ScalerManager.DrawScaler(GetComponent<SpriteRenderer>());
         }

         private void OnMouseExit()
         {
            ScalerManager.EraseScaler();
         }
      }
   }
}
