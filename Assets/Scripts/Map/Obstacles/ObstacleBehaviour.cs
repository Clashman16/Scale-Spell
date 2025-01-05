using Behaviours.Interactables;
using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
   namespace Map.Obstacles
   {
      public abstract class ObstacleBehaviour : ScalablePartBehaviour
      {
         private bool m_isFlying;

         public bool IsFlying
         {
            get => m_isFlying;
            set => m_isFlying = value;
         }

         public abstract void Start();

         public abstract void Update();

         public abstract void EnableCollider(bool p_enable);
      }
   }
}
