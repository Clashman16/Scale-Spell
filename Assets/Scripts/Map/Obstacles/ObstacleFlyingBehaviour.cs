using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
   namespace Map
   {
      public class ObstacleFlyingBehaviour : ObstacleBehaviour
      {
         private bool m_withBullet;

         public bool WithBullet
         {
            get => m_withBullet;
         }
         public override void Start()
         {
            IsFlying = true;
            MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
            List<ObstacleFlyingBehaviour> l_obstacles = l_mapManager.ObstaclesFlying;
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

               Plane[] l_planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

               if (!GeometryUtility.TestPlanesAABB(l_planes, GetComponent<SpriteRenderer>().bounds))
               {
                  MapManagerSingleton.GetInstance().ObstaclesFlying.Remove(this);
                  DestroyImmediate(gameObject);
               }
            }
         }

         public void Init(bool p_withBullet)
         {
            m_withBullet = p_withBullet;
         }

         public override void EnableCollider(bool p_enable)
         {
            Collider2D l_collider = GetComponent<Collider2D>();
            l_collider.enabled = p_enable;
         }
      }
   }
}
