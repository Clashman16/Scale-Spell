using Behaviours.UI;
using UnityEngine;

namespace Managers
{
   public class ScalerManager
   {
      private GameObject m_scalerGameObject;

      private Vector3 m_previousMousePosition;
      public Vector3 PreviousMousePosition
      {
         get { return m_previousMousePosition; }
         set
         {
            m_previousMousePosition = value;
         }
      }

      private bool m_isRescaling = false;
      public bool IsRescaling
      {
         get { return m_isRescaling; }
         set
         {
            m_isRescaling = value;
            MapManagerSingleton.GetInstance().Player.GetAnimator().SetBool("IsSpelling", m_isRescaling);
         }
      }

      public void DrawScaler(SpriteRenderer l_spriteRenderer)
      {
         m_scalerGameObject = new GameObject("Scaler");
         m_scalerGameObject.AddComponent<LineRenderer>();
         ScalerBehaviour l_scaler = m_scalerGameObject.AddComponent<ScalerBehaviour>();
         l_scaler.Init(l_spriteRenderer);
      }

      public void EraseScaler()
      {
         if(m_scalerGameObject != null)
         {
            Object.DestroyImmediate(m_scalerGameObject);
         }
      }

      public void UpdateColor(Color p_color)
      {
         if(m_scalerGameObject != null)
         {
            m_scalerGameObject.GetComponent<LineRenderer>().material.SetColor("_MainColor", p_color);
         }
      }
   }
}
