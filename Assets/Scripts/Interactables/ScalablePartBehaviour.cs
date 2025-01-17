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
            m_previousScale = m_originalScale;
         }
         
         private void OnMouseDown()
         {
            if (GameStateManager.State == GameStateEnum.PLAYING && (ScoreManagerSingleton.GetInstance().DecreasePotionQuantity > 0 || ScoreManagerSingleton.GetInstance().IncreasePotionQuantity > 0))
            {
               ScalerManager l_scalerManager = UIManagerSingleton.GetInstance().ScalerManager;
               l_scalerManager.DrawScaler(GetComponent<SpriteRenderer>());
               l_scalerManager.PreviousMousePosition = Input.mousePosition;
            }
         }

         private void OnMouseExit()
         {
            ScalerManager l_scalerManager = UIManagerSingleton.GetInstance().ScalerManager;

            if (!l_scalerManager.IsRescaling)
            {
               l_scalerManager.EraseScaler();
            }
         }

         private void OnMouseUp()
         {
            Vector3 l_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            l_mousePos.z = 0;

            ScalerManager l_scalerManager = UIManagerSingleton.GetInstance().ScalerManager;
            if (GetComponent<SpriteRenderer>().bounds.Contains(l_mousePos) || l_scalerManager.IsRescaling)
            {
               l_scalerManager.EraseScaler();
               l_scalerManager.IsRescaling = false;
            }
         }

         private void OnMouseDrag()
         {
            if (GameStateManager.State == GameStateEnum.PLAYING)
            {
               Vector3 l_mousePosition = Input.mousePosition;

               ScalerManager l_scalerManager = UIManagerSingleton.GetInstance().ScalerManager;

               if (l_mousePosition.y < l_scalerManager.PreviousMousePosition.y && ScoreManagerSingleton.GetInstance().DecreasePotionQuantity > 0)
               {
                  if(transform.localScale.x > 0.001f && transform.localScale.y > 0.001f && transform.localScale.z > 0.001f)
                  {
                     transform.localScale -= new Vector3(0.01f, 0.01f, 0);
                     l_scalerManager.UpdateColor(Color.blue);

                     float l_actualScale = transform.lossyScale.x;
                     float l_difference = Mathf.Abs(m_previousScale - l_actualScale);
                     float l_scaledDifference = l_difference * 0.1f;
                     Debug.Log(l_scaledDifference);
                     if(l_scaledDifference > 0.001f)
                     {
                        ScoreManagerSingleton.GetInstance().OnDecreasePotionUsed.Invoke(l_difference);
                        m_previousScale = l_actualScale;
                     }
                  }
               }
               else if (l_mousePosition.y > l_scalerManager.PreviousMousePosition.y && ScoreManagerSingleton.GetInstance().IncreasePotionQuantity > 0)
               {
                  transform.localScale += new Vector3(0.01f, 0.01f, 0);
                  l_scalerManager.UpdateColor(Color.red);

                  float l_actualScale = transform.lossyScale.x;
                  float l_difference = Mathf.Abs(m_previousScale - l_actualScale);
                  float l_scaledDifference = l_difference * 0.1f;
                  Debug.Log(l_scaledDifference);
                  if (l_scaledDifference > 0.001f)
                  {
                     ScoreManagerSingleton.GetInstance().OnDecreasePotionUsed.Invoke(l_difference);
                     m_previousScale = l_actualScale;
                  }
               }

               GetComponent<SpriteRenderer>().sortingLayerName = "Obstacles";
               l_scalerManager.PreviousMousePosition = l_mousePosition;
               l_scalerManager.IsRescaling = true;
            }
         }
      }
   }
}
