using Behaviours;
using UnityEngine;

namespace Managers
{
   public static class ScalerManager
   {
      private static GameObject m_scalerGameObject;

      private static Vector3 m_previousMousePosition;
      public static Vector3 PreviousMousePosition
      {
         get { return m_previousMousePosition; }
         set
         {
            m_previousMousePosition = value;
         }
      }

      private static bool m_isRescaling = false;
      public static bool IsRescaling
      {
         get { return m_isRescaling; }
         set
         {
            m_isRescaling = value;
         }
      }

      public static void DrawScaler(SpriteRenderer l_spriteRenderer)
      {
         m_scalerGameObject = new GameObject("Scaler");
         m_scalerGameObject.AddComponent<LineRenderer>();
         ScalerBehaviour l_scaler = m_scalerGameObject.AddComponent<ScalerBehaviour>();
         l_scaler.Init(l_spriteRenderer);
      }

      public static void EraseScaler()
      {
         if(m_scalerGameObject != null)
         {
            Object.DestroyImmediate(m_scalerGameObject);
         }
      }

      public static void UpdateColor(Color p_color)
      {
         if(m_scalerGameObject != null)
         {
            m_scalerGameObject.GetComponent<LineRenderer>().material.color = p_color;
         }
      }
   }
}
