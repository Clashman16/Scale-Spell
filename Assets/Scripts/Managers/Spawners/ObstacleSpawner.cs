using Behaviours.Characters;
using Behaviours.Map;
using Behaviours.Map.Obstacles;
using Managers.Spawners.Utils;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Managers.Spawners
{
   public class ObstacleSpawner
   {
      private PotionSpawner m_potionSpawner;

      private ObstacleFlyingSpawnerUtils m_flyingUtils;

      public ObstacleFlyingSpawnerUtils ObstacleFlyingSpawnerUtils
      {
         get => m_flyingUtils;
      }

      private ObstacleGroundedSpawnerUtils m_groundedUtils;

      public ObstacleGroundedSpawnerUtils ObstacleGroundedSpawnerUtils
      {
         get => m_groundedUtils;
      }

      public ObstacleSpawner()
      {
         m_potionSpawner = new PotionSpawner();
         m_flyingUtils = new ObstacleFlyingSpawnerUtils();
         m_groundedUtils = new ObstacleGroundedSpawnerUtils();
      }

      internal void Spawn(Transform p_tileTransform, EnvironmentEnum p_tileType, TileBehaviour p_lastTile)
      {
         bool l_isFlying = RandomIsFlyingInstance();

         ObstacleBehaviour l_obstacle;

         if (l_isFlying)
         {
            l_obstacle = m_flyingUtils.Spawn(p_tileTransform, p_tileType);
         }
         else
         {
            l_obstacle = m_groundedUtils.Spawn(p_tileTransform, p_tileType);
         }

         MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
         PlayerBehaviour l_player = l_mapManager.Player;

         if (!l_isFlying)
         {
            ObstacleType l_obstacleType = ((ObstacleGroundedBehaviour)l_obstacle).Type;

            int l_travelledDistance = Mathf.RoundToInt(ScoreManagerSingleton.GetInstance().TravelledDistance);
            bool l_hasObstacle = p_lastTile != null && !p_lastTile.Data.HasObstacle;
            if (!l_player.HasShield && l_obstacleType == ObstacleType.HARMFUL && l_hasObstacle && (l_travelledDistance % 5 == 0 || l_travelledDistance % 10 == 0))
            {
               m_potionSpawner.Spawn(p_lastTile.transform);
            }
         }
         
         if (l_player.HasShield)
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

      private bool RandomIsFlyingInstance()
      {
         MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
         List<ObstacleGroundedBehaviour> l_obstaclesGrounded = l_mapManager.ObstaclesGrounded;
         List<ObstacleFlyingBehaviour> l_obstaclesFlying = l_mapManager.ObstaclesFlying;
         List<ObstacleBehaviour> l_obstacles = new List<ObstacleBehaviour>();
         l_obstacles.AddRange(l_obstaclesGrounded);
         l_obstacles.AddRange(l_obstaclesFlying);

         int l_maxIsFlyingExclusive = 2;

         int l_isFlyingId = Random.Range(0, l_maxIsFlyingExclusive);

         if (l_obstacles.Count != 0)
         {
            int l_lastIsBigId = l_obstacles[l_obstacles.Count - 1].IsFlying ? 1 : 0;
            l_isFlyingId = RandomIntHelper.GetRandomValue(l_lastIsBigId, IsFlyingInstanceMajority(l_obstacles), l_maxIsFlyingExclusive);
         }

         return l_isFlyingId == 0 ? false : true;
      }
   }
}
