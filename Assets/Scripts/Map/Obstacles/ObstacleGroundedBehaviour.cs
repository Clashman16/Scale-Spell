using Behaviours.Interactables;
using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
   namespace Map.Obstacles
   {
      public class ObstacleGroundedBehaviour : ObstacleBehaviour
      {
         private ObstacleType m_type;

         public ObstacleType Type
         {
            get => m_type;
         }

         private bool m_isBig;

         public bool IsBig
         {
            get => m_isBig;
         }

         public override void Start()
         {
            IsFlying = false;
            MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
            List<ObstacleGroundedBehaviour> l_obstacles = l_mapManager.ObstaclesGrounded;
            l_obstacles.Add(this);
         }

         public override void Update()
         {
            if (GameStateManager.State == GameStateEnum.PLAYING)
            {
               Vector3 l_position = transform.position;

               float l_distance = Time.deltaTime;

               l_position.x -= l_distance;
               transform.position = l_position;
            }
         }

         public void Init(ObstacleType p_obstacleType, bool p_isBig, string p_spritePath)
         {
            m_type = p_obstacleType;
            m_isBig = p_isBig;

            SpriteRenderer l_renderer = GetComponent<SpriteRenderer>();
            l_renderer.sprite = Resources.LoadAll<Sprite>(p_spritePath)[(int) m_type];

            bool l_enableCollider = true;

            if(m_type != ObstacleType.INBETWEEN)
            {
               transform.GetChild(0).gameObject.SetActive(false);
               
               if(m_type == ObstacleType.HARMLESS)
               {
                  l_enableCollider = false;
               }
            }
            else
            {
               l_renderer.sprite = Resources.LoadAll<Sprite>(p_spritePath)[(int)ObstacleType.HARMLESS];
               l_enableCollider = false;
            }

            DisableRulers();
            InitRulers();

            EnableCollider(l_enableCollider);
         }

         private void InitRulers()
         {
            RulerBehaviour[] l_rulers = GetComponentsInChildren<RulerBehaviour>();
            for (int l_i = 0; l_i < l_rulers.Length; l_i++)
            {
               l_rulers[l_i].Init(ScoreManagerSingleton.GetInstance().IncreasePotionQuantity < ScoreManagerSingleton.GetInstance().DecreasePotionQuantity);
            }
         }

         private void DisableRulers()
         {
            if ((ScoreManagerSingleton.GetInstance().IncreasePotionQuantity >= 0.5 &&
                 ScoreManagerSingleton.GetInstance().DecreasePotionQuantity >= 0.5) ||
                m_type == ObstacleType.HARMFUL)
            {
               RulerBehaviour[] l_rulers = GetComponentsInChildren<RulerBehaviour>();
               for (int l_i = 0; l_i < l_rulers.Length; l_i++)
               {
                  l_rulers[l_i].gameObject.SetActive(false);
               }
            }
            else
            {
               if (ScoreManagerSingleton.GetInstance().IncreasePotionQuantity > 0.7)
               {
                  RulerBehaviour[] l_rulers = GetComponentsInChildren<RulerBehaviour>();
                  if(l_rulers != null && l_rulers.Length > 0)
                  {
                     int l_chance = Random.Range(0, 10);
                     if (l_chance >= 2 && l_chance <= 6)
                     {
                        l_rulers[Random.Range(0, l_rulers.Length)].gameObject.SetActive(false);
                     }
                     else
                     {
                        for (int l_i = 0; l_i < l_rulers.Length; l_i++)
                        {
                           l_rulers[l_i].gameObject.SetActive(false);
                        }
                     }
                  }
               }
               if (ScoreManagerSingleton.GetInstance().DecreasePotionQuantity > 0.7)
               {
                  RulerBehaviour[] l_rulers = GetComponentsInChildren<RulerBehaviour>();
                  if (l_rulers != null && l_rulers.Length > 0)
                  {
                     int l_chance = Random.Range(0, 10);
                     if (l_chance >= 2 && l_chance <= 6)
                     {
                        l_rulers[Random.Range(0, l_rulers.Length)].gameObject.SetActive(false);
                     }
                     else
                     {
                        for (int l_i = 0; l_i < l_rulers.Length; l_i++)
                        {
                           l_rulers[l_i].gameObject.SetActive(false);
                        }
                     }
                  }
               }
            }
         }

         public override void EnableCollider(bool p_enable)
         {
            Collider2D l_collider = GetComponent<Collider2D>();

            if (l_collider == null)
            {
               for (int l_i = 0; l_i < transform.childCount; l_i++)
               {
                  Transform l_child = transform.GetChild(l_i);

                  if (l_child.GetComponent<RulerBehaviour>() == null)
                  {
                     l_collider = l_child.GetComponent<Collider2D>();
                  }
               }
            }

            if (l_collider != null)
            {
               gameObject.layer = LayerMask.NameToLayer(p_enable ? "Collidable" : "NotCollidable");
            }
         }
      }

      public enum ObstacleType
      {
         HARMFUL, INBETWEEN, HARMLESS
      }
   }
}
