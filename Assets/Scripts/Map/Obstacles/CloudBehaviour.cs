using Managers;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Behaviours
{
   namespace Map.Obstacles
   {
      public class CloudBehaviour : ObstacleFlyingBehaviour
      {
         private readonly string m_spritesPath = "Sprites/Map/Obstacles/clouds";

         private int m_spriteId;

         public int SpriteId
         {
            get => m_spriteId;
         }

         public override void Init(bool p_withBullet)
         {
            base.Init(p_withBullet);

            MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();

            m_spriteId = RandomSpriteId(l_mapManager);

            SpriteRenderer l_renderer = GetComponent<SpriteRenderer>();
            l_renderer.sprite = Resources.LoadAll<Sprite>(m_spritesPath)[m_spriteId];

            l_mapManager.Clouds.Add(this);
         }

         private int SpriteIdMajority(List<CloudBehaviour> p_clouds)
         {
            Dictionary<int, int> l_count = new Dictionary<int, int>();
            int l_majority = -1;

            foreach (CloudBehaviour l_obstacle in p_clouds)
            {
               int l_spriteId = l_obstacle.m_spriteId;

               if (l_count.ContainsKey(l_spriteId))
               {
                  l_count[l_spriteId]++;

                  if (l_count[l_spriteId] > l_count[l_majority])
                  {
                     l_majority = l_spriteId;
                  }
               }
               else
               {
                  l_count[l_spriteId] = 1;

                  if (l_majority == -1)
                  {
                     l_majority = l_spriteId;
                  }
               }
            }

            return l_majority;
         }

         private int RandomSpriteId(MapManagerSingleton p_mapManager)
         {
            int l_maxIdExclusive = 3;
            int l_spriteId = Random.Range(0, l_maxIdExclusive);

            List<CloudBehaviour> l_clouds = p_mapManager.Clouds;
            int l_lastCloudId = p_mapManager.LastCloudId;

            if (l_clouds.Count != 0)
            {
               l_spriteId = RandomIntHelper.GetRandomValue(l_lastCloudId, SpriteIdMajority(l_clouds), l_maxIdExclusive);
            }
            else if(l_lastCloudId != -1)
            {
               l_spriteId = RandomIntHelper.GetRandomValue(l_lastCloudId, l_spriteId, l_maxIdExclusive);
            }

            p_mapManager.LastCloudId = l_spriteId;

            Debug.Log(l_spriteId);

            return l_spriteId;
         }
      }
   }
}
