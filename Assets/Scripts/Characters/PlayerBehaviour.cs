using System;
using Behaviours.Interactables;
using Behaviours.Map;
using Behaviours.UI;
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
         private bool m_isIntro;
         private float m_shieldTimer = 0f;

         private Action m_loose;
         private Action<bool> m_shieldTimerStartedOrFinished;
         private ShieldBehaviour m_shield;

         public ShieldBehaviour Shield
         {
            get => m_shield;
            set => m_shield = value;
         }

         public bool HasShield
         {
            get => m_hasShield;
            set
            {
               m_hasShield = value;
               m_shieldTimerStartedOrFinished.Invoke(!m_hasShield);
            }
         }

         private void Start()
         {
            m_hasShield = false;
            m_isIntro = true;
            m_loose += CallbacksLibrary.OnPlayerLoose;
            m_shieldTimerStartedOrFinished += CallbacksLibrary.OnShieldTimerStartedOrFinished;
         }

         private void Update()
         {
            if (m_isIntro)
            {
               Vector3 l_position = transform.position;
               l_position.y -= 0.01f;

               if (l_position.y <= 0)
               {
                  m_isIntro = false;
                  l_position.y = 0;
               }

               transform.position = l_position;
            }
            else
            {
               if (GameStateManager.State == GameStateEnum.PLAYING)
               {
                  if (Input.GetMouseButton(0) && !ScalerManager.IsRescaling)
                  {
                     Vector3 l_position = transform.position;
                     l_position.y += 0.01f;
                     transform.position = l_position;
                  }
                  else
                  {
                     Vector3 l_position = transform.position;
                     l_position.y -= 0.01f;
                     transform.position = l_position;
                  }

                  Bounds l_bounds = GetComponent<SpriteRenderer>().bounds;
                  float l_spriteTop = l_bounds.max.y;
                  Vector3 l_spriteTopScreenPosition = Camera.main.WorldToScreenPoint(new Vector3(0, l_spriteTop, 0));

                  if (l_spriteTopScreenPosition.y >= Screen.height)
                  {
                     Vector3 l_position = transform.position;
                     l_position.y -= 0.01f;
                     transform.position = l_position;
                  }

                  if (Input.GetMouseButtonDown(1))
                  {
                     GameStateManager.State = GameStateEnum.PAUSED;
                  }

                  if (m_hasShield)
                  {
                     m_shieldTimer -= Time.deltaTime;
                  }

                  if (m_shieldTimer <= 0)
                  {
                     m_shieldTimer = 60f;
                     m_hasShield = false;
                     if(m_shield != null)
                     {
                        Destroy(m_shield.gameObject);
                     } 
                  }
               }
            }
         }

         private void OnCollisionEnter2D(Collision2D p_collision)
         {
            GameObject l_gameObject = p_collision.gameObject;
            if (!m_hasShield && !l_gameObject.GetComponent<RulerBehaviour>() &&
                                 (l_gameObject.GetComponent<TileBehaviour>() ||
                                 l_gameObject.GetComponent<ObstacleBehaviour>() ||
                                 l_gameObject.GetComponentInParent<ObstacleBehaviour>()))
            {
               m_loose.Invoke();
               m_loose -= CallbacksLibrary.OnPlayerLoose;
               m_shieldTimerStartedOrFinished -= CallbacksLibrary.OnShieldTimerStartedOrFinished;
            }
         }
      }
   }
}
