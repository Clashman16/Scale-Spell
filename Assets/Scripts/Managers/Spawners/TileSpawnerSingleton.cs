using Behaviours.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Managers.Spawners
{
   public sealed class TileSpawnerSingleton
   {
      private EnvironmentEnum m_newTileType;
      private int m_newLength;

      private ObstacleSpawner m_obstacleSpawner;

      private float m_timeBeforeSpawn;
      public Action m_timerFinished;

      public float TimeBeforeSpawn
      {
         get { return m_timeBeforeSpawn; }
         set
         {
            m_timeBeforeSpawn = value;
            if (m_timeBeforeSpawn <= 0)
            {
               m_timerFinished.Invoke();
            }
         }
      }

      private TileSpawnerSingleton()
      {
         m_newTileType = EnvironmentEnum.NONE;
         m_newLength = 0;
         m_obstacleSpawner = new ObstacleSpawner();
      }

      private static TileSpawnerSingleton m_instance = null;

      public static TileSpawnerSingleton GetInstance()
      {
         if (m_instance == null)
         {
            m_instance = new TileSpawnerSingleton();
         }
         return m_instance;
      }

      internal void Spawn()
      {
         MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
         List<TileBehaviour> l_tiles = l_mapManager.Tiles;

         GameObject l_instantiatedTile = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Map/Tile"));

         m_newTileType = RandomTileType(l_tiles);
         m_newLength = RandomLength(l_tiles);

         TileBehaviour l_lastTile = null;
         if(l_tiles.Count > 0)
         {
            l_lastTile = l_tiles[l_tiles.Count-1];
         }

         bool l_hasObstacle = (l_tiles.Count % 3 == 0 || l_tiles.Count % 5 == 0) && l_lastTile != null && !l_lastTile.HasObstacle &&
                              m_newTileType != EnvironmentEnum.SAND;

         TileBehaviour l_newTile = l_instantiatedTile.GetComponent<TileBehaviour>();
         l_newTile.Init(m_newTileType, m_newLength, l_hasObstacle);

         float l_tileWidth = l_instantiatedTile.GetComponent<Transform>().lossyScale.x;

         Vector3 l_screenPosition = new Vector3(Screen.width, 0, Camera.main.nearClipPlane);
         Vector3 l_worldPosition = Camera.main.ScreenToWorldPoint(l_screenPosition);
         Vector3 l_spawnPosition = l_worldPosition + new Vector3(l_tileWidth / 2, 0.5f, 0);
         l_newTile.transform.position = l_spawnPosition;

         m_timeBeforeSpawn = l_tileWidth;

         Bounds l_tileBound = l_instantiatedTile.GetComponent<SpriteRenderer>().bounds;

         ObstacleBehaviour[] l_obstacles = MapManagerSingleton.GetInstance().Obstacles.ToArray();
         if (l_obstacles.Any(p_obstacle => p_obstacle.GetComponentsInChildren<SpriteRenderer>().Any(p_renderer => p_renderer.bounds.max.x >= l_tileBound.min.x)))
         {
            l_hasObstacle = false;
            l_newTile.HasObstacle = false;
         }

         if (l_hasObstacle)
         {
            m_obstacleSpawner.Spawn(l_instantiatedTile.transform, m_newTileType, l_lastTile);
         }

         l_tiles.Add(l_newTile);
      }

      private int TileLengthMajority(List<TileBehaviour> p_tiles)
      {
         Dictionary<int, int> l_count = new Dictionary<int, int>();
         int l_majority = -1;

         foreach (TileBehaviour l_tile in p_tiles)
         {
            int l_length = l_tile.Length;
            if (l_count.ContainsKey(l_length))
            {
               l_count[l_length]++;

               if(l_count[l_length] > l_count[l_majority])
               {
                  l_majority = l_length;
               }
            }
            else
            {
               l_count[l_length] = 1;
               
               if(l_majority == -1)
               {
                  l_majority = l_length;
               }
            }
         }

         return l_majority;
      }

      private int RandomLength(List<TileBehaviour> p_tiles)
      {
         int l_maxLengthExclusive = 4;

         int l_length = UnityEngine.Random.Range(0, l_maxLengthExclusive);

         if (p_tiles.Count != 0)
         {
            int l_lastLength = p_tiles[p_tiles.Count - 1].Length;
            l_length = RandomIntHelper.GetRandomValue(l_lastLength, TileTypeMajority(p_tiles), l_maxLengthExclusive);
         }

         l_length += 1;

         return l_length;
      }

      private int TileTypeMajority(List<TileBehaviour> p_tiles)
      {
         Dictionary<EnvironmentEnum, int> l_count = new Dictionary<EnvironmentEnum, int>();
         int l_majority = -1;

         foreach (TileBehaviour l_tile in p_tiles)
         {
            EnvironmentEnum l_type = l_tile.Type;
            if (l_count.ContainsKey(l_type))
            {
               l_count[l_type]++;

               if (l_count[l_type] > l_count[(EnvironmentEnum) l_majority])
               {
                  l_majority = (int) l_type;
               }
            }
            else
            {
               l_count[l_type] = 1;

               if (l_majority == -1)
               {
                  l_majority = (int) l_type;
               }
            }
         }

         return l_majority;
      }

      private EnvironmentEnum RandomTileType(List<TileBehaviour> p_tiles)
      {
         int l_enumSize = 3;

         int l_envId = UnityEngine.Random.Range(0, l_enumSize);

         if(p_tiles.Count != 0)
         {
            EnvironmentEnum l_lastType = p_tiles[p_tiles.Count - 1].Type;
            l_envId = RandomIntHelper.GetRandomValue((int)l_lastType, TileTypeMajority(p_tiles), l_enumSize);
         }

         return (EnvironmentEnum)l_envId;
      }
   }
}