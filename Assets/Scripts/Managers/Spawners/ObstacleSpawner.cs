using Behaviours.Characters;
using Behaviours.Map;
using Managers.Spawners.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utils;

namespace Managers.Spawners
{
   public class ObstacleSpawner
   {
      private readonly string m_prefabsPath = "Prefabs/Map/Obstacles";
      private readonly string m_spritesPath = "Sprites/Map/Obstacles";

      private PotionSpawner m_potionSpawner;

      public ObstacleSpawner()
      {
         m_potionSpawner = new PotionSpawner();
      }

      internal void Spawn(Transform p_tileTransform, EnvironmentEnum p_tileType, TileBehaviour p_lastTile)
      {
         MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
         List<ObstacleGroundedBehaviour> l_obstaclesGrounded = l_mapManager.ObstaclesGrounded;
         List<ObstacleFlyingBehaviour> l_obstaclesFlying = l_mapManager.ObstaclesFlying;
         List<ObstacleBehaviour> l_obstacles = new List<ObstacleBehaviour>();
         l_obstacles.AddRange(l_obstaclesGrounded);
         l_obstacles.AddRange(l_obstaclesFlying);

         ObstacleBehaviour l_obstacle;
         PlayerBehaviour l_player = l_mapManager.Player;

         string l_prefabName;

         bool l_isFlying = RandomIsFlyingInstance(l_obstacles);

         if(l_isFlying)
         {
            ObstacleFlyingSpawnerUtils l_utils = new ObstacleFlyingSpawnerUtils();

            bool l_withBullet = l_utils.RandomWithBullet(l_obstaclesFlying);
            bool l_isCloud = l_utils.RandomIsCloud(l_obstaclesFlying);
            l_prefabName = l_utils.GetPrefabName(l_isCloud, p_tileType);

            GameObject l_spawnedObject = Object.Instantiate(Resources.Load<GameObject>(Path.Combine(m_prefabsPath, l_prefabName)));

            Vector3 l_position = l_spawnedObject.transform.position;
            l_position.x = p_tileTransform.transform.position.x;
            l_position.y = l_utils.GetYCoordinate(l_prefabName);
            l_spawnedObject.transform.position = l_position;

            ObstacleFlyingBehaviour l_obstacleFlying = l_spawnedObject.GetComponent<ObstacleFlyingBehaviour>();
            l_obstacleFlying.Init(l_withBullet);
            l_obstacle = l_obstacleFlying;
         }
         else
         {
            ObstacleGroundedSpawnerUtils l_utils = new ObstacleGroundedSpawnerUtils();

            bool l_isBig = l_utils.RandomSize(p_tileTransform, l_obstaclesGrounded);
            ObstacleType l_obstacleType = l_utils.RandomObstacleType(l_obstaclesGrounded);

            int l_travelledDistance = Mathf.RoundToInt(ScoreManagerSingleton.GetInstance().TravelledDistance);
            bool l_hasObstacle = p_lastTile != null && !p_lastTile.HasObstacle;
            if (!l_player.HasShield && l_obstacleType == ObstacleType.HARMFUL && l_hasObstacle && (l_travelledDistance % 5 == 0 || l_travelledDistance % 10 == 0))
            {
               m_potionSpawner.Spawn(p_lastTile.transform);
            }

            l_prefabName = l_utils.GetPrefabName(l_isBig, p_tileType);

            GameObject l_spawnedObject = Object.Instantiate(Resources.Load<GameObject>(Path.Combine(m_prefabsPath, l_prefabName)));

            Vector3 l_position = l_spawnedObject.transform.position;
            l_position.x = p_tileTransform.transform.position.x;
            l_position.y = l_utils.GetYCoordinate(l_prefabName);
            l_spawnedObject.transform.position = l_position;

            string l_spriteName = l_prefabName.ToLower();
            l_spriteName = l_spriteName.Replace(" ", "-");

            ObstacleGroundedBehaviour l_obstacleGrounded = l_spawnedObject.GetComponent<ObstacleGroundedBehaviour>();
            l_obstacleGrounded.Init(l_obstacleType, l_isBig, Path.Combine(m_spritesPath, l_spriteName));
            l_obstacle = l_obstacleGrounded;
         }

         if(l_player.HasShield)
         {
            l_obstacle.EnableCollider(false);
         }
      }

      private int IsFlyingInstanceMajority(List<ObstacleBehaviour> p_obstacles)
      {
         Dictionary<int, int> l_count = new Dictionary<int, int>();
         int l_majority = -1;

         foreach (ObstacleBehaviour l_obstacle in p_obstacles)
         {
            int l_isFlyingId = l_obstacle.IsFlying ? 1 : 0;

            if (l_count.ContainsKey(l_isFlyingId))
            {
               l_count[l_isFlyingId]++;

               if (l_count[l_isFlyingId] > l_count[l_majority])
               {
                  l_majority = l_isFlyingId;
               }
            }
            else
            {
               l_count[l_isFlyingId] = 1;

               if (l_majority == -1)
               {
                  l_majority = l_isFlyingId;
               }
            }
         }

         return l_majority;
      }

      private bool RandomIsFlyingInstance(List<ObstacleBehaviour> p_obstacles)
      {
         int l_maxIsFlyingExclusive = 2;

         int l_isFlyingId = Random.Range(0, l_maxIsFlyingExclusive);

         if (p_obstacles.Count != 0)
         {
            int l_lastIsBigId = p_obstacles[p_obstacles.Count - 1].IsFlying ? 1 : 0;
            l_isFlyingId = RandomIntHelper.GetRandomValue(l_lastIsBigId, IsFlyingInstanceMajority(p_obstacles), l_maxIsFlyingExclusive);
         }

         return l_isFlyingId == 0 ? false : true;
      }
   }
}
