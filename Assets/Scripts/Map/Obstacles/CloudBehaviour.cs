using Managers;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Behaviours
{
   namespace Map.Obstacles
   {
      public class CloudBehaviour : MonoBehaviour
      {
         private readonly string m_spritesPath = "Sprites/Map/Obstacles/clouds";

         private int m_spriteId;

         public int SpriteId
         {
            get => m_spriteId;
         }

         void Start()
         {
            MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();

            m_spriteId = RandomSpriteId(l_mapManager.Clouds);

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

         private int RandomSpriteId(List<CloudBehaviour> p_clouds)
         {
            int l_maxIdExclusive = 3;
            int l_spriteId = Random.Range(0, l_maxIdExclusive);

            if (p_clouds.Count != 0)
            {
               int l_lastId = p_clouds[p_clouds.Count - 1].SpriteId;
               l_spriteId = RandomIntHelper.GetRandomValue(l_lastId, SpriteIdMajority(p_clouds), l_maxIdExclusive);
            }

            return l_spriteId;
         }
      }
   }
}
