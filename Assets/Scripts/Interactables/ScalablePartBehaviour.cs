using Behaviours.Managers;
using Managers;
using UnityEngine;

namespace Behaviours
{
   namespace Interactables
   {
      public class ScalablePartBehaviour : MonoBehaviour
      {
         private void OnMouseDown()
         {
            if(GameStateManager.State == GameStateEnum.PLAYING)
            {
               ScalerManager.DrawScaler(GetComponent<SpriteRenderer>());
               ScalerManager.PreviousMousePosition = Input.mousePosition;
            }
         }

         private void OnMouseExit()
         {
            if(!ScalerManager.IsRescaling)
            {
               ScalerManager.EraseScaler();
            }
         }

         private void OnMouseUp()
         {
            Vector3 l_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            l_mousePos.z = 0;

            if (GetComponent<SpriteRenderer>().bounds.Contains(l_mousePos) || ScalerManager.IsRescaling)
            {
               ScalerManager.EraseScaler();
               ScalerManager.IsRescaling = false;
            }
         }

         private void OnMouseDrag()
         {
            if (GameStateManager.State == GameStateEnum.PLAYING)
            {
               Vector3 l_mousePosition = Input.mousePosition;

               if (l_mousePosition.y < ScalerManager.PreviousMousePosition.y)
               {
                  if(transform.localScale.x > 0.001f && transform.localScale.y > 0.001f && transform.localScale.z > 0.001f)
                  {
                     transform.localScale -= new Vector3(0.01f, 0.01f, 0);
                     ScalerManager.UpdateColor(Color.blue);
                  }
               }
               else if (l_mousePosition.y > ScalerManager.PreviousMousePosition.y)
               {
                  transform.localScale += new Vector3(0.01f, 0.01f, 0);
                  ScalerManager.UpdateColor(Color.red);
               }

               GetComponent<SpriteRenderer>().sortingLayerName = "Obstacles";
               ScalerManager.PreviousMousePosition = l_mousePosition;
               ScalerManager.IsRescaling = true;
            }
         }
      }
   }
}
