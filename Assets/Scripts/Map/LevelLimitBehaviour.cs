using Behaviours.Map.Obstacles;
using Managers;
using Managers.Spawners;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
   namespace Map
   {
      public class LevelLimitBehaviour : MonoBehaviour
      {
         private void OnCollisionEnter2D(Collision2D p_collision)
         {
            GameObject l_collidedObject = p_collision.gameObject;
            MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();

            TileBehaviour l_tile = l_collidedObject.GetComponent<TileBehaviour>();
            if (l_tile != null)
            {
               List<TileBehaviour> l_tiles = l_mapManager.Tiles;
               l_tiles.Remove(l_tile);

               TileSpawnerSingleton.Instance.AddToRecycleBin(l_collidedObject);
            }

            ObstacleFlyingBehaviour l_flyingObstacle = l_collidedObject.GetComponent<ObstacleFlyingBehaviour>();
            if (l_flyingObstacle != null)
            {
               l_mapManager.ObstaclesFlying.Remove(l_flyingObstacle);

               CloudBehaviour l_cloud = l_flyingObstacle.GetComponent<CloudBehaviour>();
               if (l_cloud != null)
               {
                  l_mapManager.Clouds.Remove(l_cloud);
               }

               TileSpawnerSingleton.Instance.ObstacleSpawner.ObstacleFlyingSpawnerUtils.AddToRecycleBin(l_collidedObject);
            }

            ObstacleGroundedBehaviour l_groundedObstacle = l_collidedObject.GetComponent<ObstacleGroundedBehaviour>();
            if (l_groundedObstacle != null)
            {
               l_mapManager.ObstaclesGrounded.Remove(l_groundedObstacle);

               TileSpawnerSingleton.Instance.ObstacleSpawner.ObstacleGroundedSpawnerUtils.AddToRecycleBin(l_collidedObject);
            }
         }
      }
   }
}
