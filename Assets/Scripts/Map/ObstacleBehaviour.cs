using Behaviours.Interactables;
using Managers;
using UnityEngine;

namespace Behaviours
{
   namespace Map
   {
      public class ObstacleBehaviour : ScalablePartBehaviour
      {
         private ObstacleType m_obstacleType;

         public ObstacleType GetObstacleType()
         {
            return m_obstacleType;
         }

         public void Init(ObstacleType p_obstacleType, string p_spritePath)
         {
            m_obstacleType = p_obstacleType;

            SpriteRenderer l_renderer = GetComponent<SpriteRenderer>();
            l_renderer.sprite = Resources.LoadAll<Sprite>(p_spritePath)[(int) m_obstacleType];

            if(m_obstacleType != ObstacleType.INBETWEEN)
            {
               DestroyImmediate(transform.GetChild(0).gameObject);
               
               if(m_obstacleType == ObstacleType.HARMLESS)
               {
                  DestroyImmediate(GetComponent<Collider2D>());
               }
            }
            else
            {
               l_renderer.sprite = Resources.LoadAll<Sprite>(p_spritePath)[(int)ObstacleType.HARMLESS];
               DestroyImmediate(GetComponent<Collider2D>());
            }

            DestroyRulers();
            InitRulers();
         }

         private void InitRulers()
         {
            RulerBehaviour[] l_rulers = GetComponentsInChildren<RulerBehaviour>();
            for (int l_i = 0; l_i < l_rulers.Length; l_i++)
            {
               l_rulers[l_i].Init(ScoreManagerSingleton.GetInstance().IncreasePotionQuantity < ScoreManagerSingleton.GetInstance().DecreasePotionQuantity);
            }
         }

         private void DestroyRulers()
         {
            if ((ScoreManagerSingleton.GetInstance().IncreasePotionQuantity >= 0.5 &&
                 ScoreManagerSingleton.GetInstance().DecreasePotionQuantity >= 0.5) ||
                m_obstacleType == ObstacleType.HARMFUL)
            {
               RulerBehaviour[] l_rulers = GetComponentsInChildren<RulerBehaviour>();
               for (int l_i = 0; l_i < l_rulers.Length; l_i++)
               {
                  DestroyImmediate(l_rulers[l_i].gameObject);
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
                        DestroyImmediate(l_rulers[Random.Range(0, l_rulers.Length)].gameObject);
                     }
                     else
                     {
                        for (int l_i = 0; l_i < l_rulers.Length; l_i++)
                        {
                           DestroyImmediate(l_rulers[l_i].gameObject);
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
                        DestroyImmediate(l_rulers[Random.Range(0, l_rulers.Length)].gameObject);
                     }
                     else
                     {
                        for (int l_i = 0; l_i < l_rulers.Length; l_i++)
                        {
                           DestroyImmediate(l_rulers[l_i].gameObject);
                        }
                     }
                  }
               }
            }
         }

         private void Update()
         {
            if (GameStateManager.State == GameStateEnum.PLAYING)
            {
               Vector3 l_position = transform.position;

               float l_distance = Time.deltaTime;

               l_position.x -= l_distance;
               transform.position = l_position;

               Plane[] l_planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

               if (!GeometryUtility.TestPlanesAABB(l_planes, GetComponent<SpriteRenderer>().bounds))
               {
                  DestroyImmediate(gameObject);
               }
            }
         }
      }

      public enum ObstacleType
      {
         HARMFUL, INBETWEEN, HARMLESS
      }
   }
}
