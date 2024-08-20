using Behaviours.Characters;
using Managers;
using UnityEngine;

namespace Behaviours
{
   namespace Interactables
   {
      public class ScalablePartBehaviour : MonoBehaviour
      {
         private float m_originalScale;
         private float m_previousScale;

         private void Start()
         {
            m_originalScale = transform.lossyScale.x;
            m_previousScale = 1;
         }
         
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

                     float l_actualScale = transform.lossyScale.x * 100 / m_originalScale;
                     float l_difference = Mathf.Abs(m_previousScale - l_actualScale);
                     FindObjectOfType<PlayerBehaviour>().GetScoreManager().OnDecreasePotionUsed().Invoke(l_difference);
                     m_previousScale = l_actualScale;
                  }
               }
               else if (l_mousePosition.y > ScalerManager.PreviousMousePosition.y)
               {
                  transform.localScale += new Vector3(0.01f, 0.01f, 0);
                  ScalerManager.UpdateColor(Color.red);
                  
                  float l_actualScale = transform.lossyScale.x * 100 / m_originalScale;
                  float l_difference = Mathf.Abs(m_previousScale - l_actualScale);
                  FindObjectOfType<PlayerBehaviour>().GetScoreManager().OnIncreasePotionUsed().Invoke(l_difference);
                  m_previousScale = l_actualScale;
               }

               GetComponent<SpriteRenderer>().sortingLayerName = "Obstacles";
               ScalerManager.PreviousMousePosition = l_mousePosition;
               ScalerManager.IsRescaling = true;
            }
         }
      }
   }
}
