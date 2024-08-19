using System;
using Behaviours.Managers;
using Behaviours.Map;
using Managers;
using UnityEngine;
using Utils.Callbacks;

namespace Behaviours
{
   namespace Characters
   {
      public class PlayerBehaviour : MonoBehaviour
      {
         private bool m_hasShield;
         private Action m_loose;

         private void Start()
         {
            m_hasShield = false;
            m_loose += CallbacksLibrary.OnPlayerLoose;
         }
         private void Update()
         {
            if (GameStateManager.State == GameStateEnum.PLAYING)
            {
               if(Input.GetMouseButton(0) && !ScalerManager.IsRescaling)
               {
                  Vector3 l_position = transform.position;
                  l_position.y += Input.mousePosition.y < Screen.height / 2 ? -0.005f : 0.005f;
                  transform.position = l_position;
               }

               if (Input.GetMouseButtonDown(1))
               {
                  GameStateManager.State = GameStateEnum.PAUSED;
               }
            }
         }
         
         private void OnCollisionEnter2D(Collision2D p_collision)
         {
            GameObject l_gameObject = p_collision.gameObject;
            if (!m_hasShield && (l_gameObject.GetComponent<TileBehaviour>() ||
                                 l_gameObject.GetComponent<ObstacleBehaviour>() ||
                                 l_gameObject.GetComponentInParent<ObstacleBehaviour>()))
            {
               m_loose.Invoke();
            }
         }
      }

   }
}
