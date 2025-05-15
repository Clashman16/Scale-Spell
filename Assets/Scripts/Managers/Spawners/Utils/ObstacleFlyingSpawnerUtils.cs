using Behaviours.Characters;
using Behaviours.Map;
using Behaviours.Map.Obstacles;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utils;

namespace Managers.Spawners.Utils
{
   public class ObstacleFlyingSpawnerUtils : ObstacleSpawnerUtils
   {
      private int BulletMajority(List<ObstacleFlyingBehaviour> p_obstacles)
      {
         Dictionary<int, int> l_count = new Dictionary<int, int>();
         int l_majority = -1;

         foreach (ObstacleFlyingBehaviour l_obstacle in p_obstacles)
         {
            int l_withBulletId = l_obstacle.WithBullet ? 1 : 0;

            if (l_count.ContainsKey(l_withBulletId))
            {
               l_count[l_withBulletId]++;

               if (l_count[l_withBulletId] > l_count[l_majority])
               {
                  l_majority = l_withBulletId;
               }
            }
            else
            {
               l_count[l_withBulletId] = 1;

               if (l_majority == -1)
               {
                  l_majority = l_withBulletId;
               }
            }
         }

         return l_majority;
      }

      internal bool RandomWithBullet(List<ObstacleFlyingBehaviour> p_obstacles)
      {
         int l_maxLengthExclusive = 2;

         int l_withBulletId = Random.Range(0, l_maxLengthExclusive);

         if (p_obstacles.Count != 0)
         {
            int l_lastwithBulletId = p_obstacles[p_obstacles.Count - 1].WithBullet ? 1 : 0;
            l_withBulletId = RandomIntHelper.GetRandomValue(l_lastwithBulletId, BulletMajority(p_obstacles), l_maxLengthExclusive);
         }

         return l_withBulletId == 0 ? false : true;
      }

      private int CloudsMajority(List<ObstacleFlyingBehaviour> p_obstacles)
      {
         Dictionary<int, int> l_count = new Dictionary<int, int>();
         int l_majority = -1;

         foreach (ObstacleFlyingBehaviour l_obstacle in p_obstacles)
         {
            int l_isCatId = l_obstacle.GetComponent<CloudBehaviour>() != null ? 1 : 0;

            if (l_count.ContainsKey(l_isCatId))
            {
               l_count[l_isCatId]++;

               if (l_count[l_isCatId] > l_count[l_majority])
               {
                  l_majority = l_isCatId;
               }
            }
            else
            {
               l_count[l_isCatId] = 1;

               if (l_majority == -1)
               {
                  l_majority = l_isCatId;
               }
            }
         }

         return l_majority;
      }

      internal bool RandomIsCloud(List<ObstacleFlyingBehaviour> p_obstacles)
      {
         int l_maxLengthExclusive = 2;

         int l_isCloudId = Random.Range(0, l_maxLengthExclusive);

         if (p_obstacles.Count != 0)
         {
            int l_lastIsCloudId = p_obstacles[p_obstacles.Count - 1].GetComponent<CloudBehaviour>() != null ? 1 : 0;
            l_isCloudId = RandomIntHelper.GetRandomValue(l_lastIsCloudId, BulletMajority(p_obstacles), l_maxLengthExclusive);
         }

         return l_isCloudId == 0 ? false : true;
      }

      internal override string GetPrefabName(bool p_isCloud, EnvironmentEnum p_tileType)
      {
         string l_prefabName;

         if (p_isCloud)
         {
            return "Clouds";
         }

         switch (p_tileType)
         {
            case EnvironmentEnum.GRASS:
               l_prefabName = "Flower";
               break;
            case EnvironmentEnum.SAND:
               l_prefabName = "Pyramid";
               break;
            default:
               l_prefabName = "Cat";
               break;
         }

         return l_prefabName;
      }

      internal override float GetYCoordinate(string p_prefabName)
      {
         float l_y = 0;

         if (p_prefabName == "Clouds" || p_prefabName == "Pyramid")
         {
            PlayerBehaviour l_player = MapManagerSingleton.GetInstance().Player;
            Vector3 l_playerPosition = l_player.transform.position;

            if(l_playerPosition.y > l_y)
            {
               l_y = l_playerPosition.y;
            }
         }

         return l_y;
      }

      internal override ObstacleBehaviour Spawn(Transform p_tileTransform, EnvironmentEnum p_tileType)
      {
         MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
         List<ObstacleFlyingBehaviour> l_obstacles= l_mapManager.ObstaclesFlying;

         bool l_withBullet = RandomWithBullet(l_obstacles);
         bool l_isCloud = RandomIsCloud(l_obstacles);
         string l_prefabName = GetPrefabName(l_isCloud, p_tileType);

         GameObject l_spawnedObject = null;

         if (!IsRecycleBinEmpty)
         {
            l_spawnedObject = RemoveFromRecycleBin(l_prefabName);
         }

         if (l_spawnedObject == null)
         {
            l_spawnedObject = Object.Instantiate(Resources.Load<GameObject>(Path.Combine(PrefabsPath, l_prefabName)));
         }

         Vector3 l_position = l_spawnedObject.transform.position;
         l_position.x = p_tileTransform.position.x;
         l_position.y = GetYCoordinate(l_prefabName);
         l_spawnedObject.transform.position = l_position;

         ObstacleFlyingBehaviour l_obstacleFlying = l_spawnedObject.GetComponent<ObstacleFlyingBehaviour>();
         l_obstacleFlying.Init(l_withBullet);

         return l_obstacleFlying;
      }
   }
}
