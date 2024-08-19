using Behaviours;
using UnityEngine;

namespace Managers
{
   public static class ScalerManager
   {
      private static GameObject m_scaler;

      public static void DrawScaler(SpriteRenderer l_spriteRenderer)
      {
         m_scaler = new GameObject("Scaler");
         m_scaler.AddComponent<LineRenderer>();
         ScalerBehaviour l_scaler = m_scaler.AddComponent<ScalerBehaviour>();
         l_scaler.Init(l_spriteRenderer);
      }

      public static void EraseScaler()
      {
         Object.DestroyImmediate(m_scaler);
      }
   }
}
