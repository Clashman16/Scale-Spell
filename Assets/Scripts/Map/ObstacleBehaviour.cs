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
         }

         private void Update()
         {
            if (GameStateManager.State == GameStateEnum.PLAYING)
            {
               Vector3 l_position = transform.position;

               float l_distance = 0.005f;

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
