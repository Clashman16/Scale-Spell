using Behaviours.Characters;
using Behaviours.Map;
using Behaviours.Map.Obstacles;
using System.Collections.Generic;
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
         float l_y = Screen.height / 2;

         if(p_prefabName == "Clouds" || p_prefabName == "Pyramid")
         {
            PlayerBehaviour l_player = MapManagerSingleton.GetInstance().Player;
            Vector3 l_playerPosition = l_player.transform.position;
            float l_playerScreenY = Camera.main.WorldToScreenPoint(l_playerPosition).y;

            if(l_playerScreenY > l_y)
            {
               l_y = l_playerScreenY;
            }
         }

         return l_y;
      }
   }
}
