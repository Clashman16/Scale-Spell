using Behaviours.Map;
using System;
using UnityEngine;

namespace Behaviours
{
   namespace Managers.Spawners
   {
      public class TileSpawner
      {
         private EnvironmentEnum m_lastTileType;
         private int m_lastLength;

         private TileBehaviour m_lastTile;

         public TileBehaviour LastTile()
         {
            return m_lastTile;
         }

         private float m_timeBeforeSpawn;
         public Action m_timerFinished;

         public float TimeBeforeSpawn
         {
            get { return m_timeBeforeSpawn; }
            set
            {
               m_timeBeforeSpawn = value;
               if(m_timeBeforeSpawn <= 0)
               {
                  m_timerFinished.Invoke();
               }
            }
         }

         public TileSpawner()
         {
            m_lastTileType = EnvironmentEnum.NONE;
            m_lastLength = 0;
         }

         public void Spawn()
         {
            GameObject l_tile = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Map/Tile"));

            

            m_lastTileType = RandomTileType();
            m_lastLength = RandomLength();
           

            m_lastTile = l_tile.GetComponent<TileBehaviour>();
            m_lastTile.Init(m_lastTileType, m_lastLength);

            float l_tileWidth = l_tile.GetComponent<SpriteRenderer>().bounds.size.x;

            Vector3 l_screenPosition = new Vector3(Screen.width, 0, Camera.main.nearClipPlane);
            Vector3 l_worldPosition = Camera.main.ScreenToWorldPoint(l_screenPosition);
            Vector3 l_spawnPosition = l_worldPosition + new Vector3(l_tileWidth / 2, 0.5f, 0);
            l_tile.transform.position = l_spawnPosition;

            m_timeBeforeSpawn = l_tileWidth;
         }

         public int RandomLength()
         {
            int l_length = 0;

            int l_trialsCount = 3;
            while (l_trialsCount > 0)
            {
               l_length = UnityEngine.Random.Range(1, 5);

               if (l_length != m_lastLength)
               {
                  l_trialsCount = 0;
               }
               else
               {
                  l_trialsCount--;
               }
            }
            return l_length;
         }

         public EnvironmentEnum RandomTileType()
         {
            int l_envId = 0;

            int l_trialsCount = 3;
            while (l_trialsCount > 0)
            {
               l_envId = UnityEngine.Random.Range(0, 3);

               if (l_envId != (int)m_lastTileType)
               {
                  l_trialsCount = 0;
               }
               else
               {
                  l_trialsCount--;
               }
            }
            return (EnvironmentEnum)l_envId;
         }
      }
   }
}

