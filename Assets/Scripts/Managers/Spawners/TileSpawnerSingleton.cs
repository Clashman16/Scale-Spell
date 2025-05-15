using Behaviours.Map;
using Behaviours.Map.Obstacles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Managers.Spawners
{
   public sealed class TileSpawnerSingleton : ObjectRecycler
   {
      private const string m_tilePrefabPath = "Prefabs/Map/Tile";

      private ObstacleSpawner m_obstacleSpawner;

      public ObstacleSpawner ObstacleSpawner
      {
         get => m_obstacleSpawner;
      }

      private float m_timeBeforeSpawn;

      private List<TileData> m_tilesToSpawn;
      private const int m_tilesToSpawnMaxSize = 5;

      public float TimeBeforeSpawn
      {
         get { return m_timeBeforeSpawn; }
         set
         {
            m_timeBeforeSpawn = value;
            if (m_timeBeforeSpawn <= 0 && m_tilesToSpawn.Count < m_tilesToSpawnMaxSize)
            {
               PrepareTileToSpawn();
            }
         }
      }

      private TileSpawnerSingleton() : base()
      {
         m_obstacleSpawner = new ObstacleSpawner();
         m_tilesToSpawn = new List<TileData>();
      }

      private static TileSpawnerSingleton m_instance = null;

      public static TileSpawnerSingleton Instance
      {
         get
         {
            if (m_instance == null)
            {
               m_instance = new TileSpawnerSingleton();
            }
            return m_instance;
         }
      }

      public static void Reset()
      {
         m_instance = null;
      }

      internal void PrepareTileToSpawn()
      {
         MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
         List<TileBehaviour> l_tiles = l_mapManager.Tiles;

         EnvironmentEnum l_newTileType = RandomTileType(l_tiles);
         int l_newLength = RandomLength(l_tiles);

         TileBehaviour l_lastTile = null;
         if (l_tiles.Count > 0)
         {
            l_lastTile = l_tiles[l_tiles.Count - 1];
         }

         bool l_hasObstacle = (l_tiles.Count % 3 == 0 || l_tiles.Count % 5 == 0) && l_lastTile != null && !l_lastTile.Data.HasObstacle;

         TileData l_data = new TileData(l_hasObstacle, l_newTileType, l_newLength);
         m_tilesToSpawn.Add(l_data);
      }

      internal void SpawnFirstWaitingTile()
      {
         if(m_tilesToSpawn.Count > 0)
         {
            GameObject l_instantiatedTile = null;
            
            if(!IsRecycleBinEmpty)
            {
               l_instantiatedTile = RemoveFromRecycleBin("Tile");
            }

            if(l_instantiatedTile == null)
            {
               l_instantiatedTile = Object.Instantiate(Resources.Load<GameObject>(m_tilePrefabPath));
            }
            
            TileBehaviour l_newTile = l_instantiatedTile.GetComponent<TileBehaviour>();
            l_newTile.Data = m_tilesToSpawn[0];
            m_tilesToSpawn.RemoveAt(0);

            TileData l_data = l_newTile.Data;

            float l_tileWidth = l_instantiatedTile.GetComponent<Transform>().lossyScale.x;
            Vector3 l_spawnPosition;

            MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
            List<TileBehaviour> l_tiles = l_mapManager.Tiles;

            if (l_tiles.Count > 0)
            {
               TileBehaviour l_lastTile = l_tiles[l_tiles.Count - 1];
               float l_lastTileWidth = l_lastTile.GetComponent<SpriteRenderer>().bounds.size.x;
               l_spawnPosition = l_lastTile.transform.position + new Vector3(l_lastTileWidth / 2 + l_tileWidth / 2, 0, 0);
            }
            else
            {
               Vector3 l_screenPosition = new Vector3(Screen.width, 0, Camera.main.nearClipPlane);
               Vector3 l_worldPosition = Camera.main.ScreenToWorldPoint(l_screenPosition);
               l_spawnPosition = l_worldPosition + new Vector3(-l_tileWidth / 2, 0.5f, 0);
            }

            l_newTile.transform.position = l_spawnPosition;

            m_timeBeforeSpawn = l_tileWidth * Time.deltaTime;

            Bounds l_tileBound = l_instantiatedTile.GetComponent<SpriteRenderer>().bounds;

            List<ObstacleGroundedBehaviour> l_obstaclesGrounded = l_mapManager.ObstaclesGrounded;
            List<ObstacleFlyingBehaviour> l_obstaclesFlying = l_mapManager.ObstaclesFlying;
            List<ObstacleBehaviour> l_obstacles = new List<ObstacleBehaviour>();
            l_obstacles.AddRange(l_obstaclesGrounded);
            l_obstacles.AddRange(l_obstaclesFlying);
            if (l_obstacles.Any(p_obstacle => p_obstacle.GetComponentsInChildren<SpriteRenderer>().Any(p_renderer => p_renderer.bounds.max.x >= l_tileBound.min.x)))
            {
               l_data.HasObstacle = false;
               l_newTile.Data = l_data;
            }

            if (l_newTile.Data.HasObstacle)
            {
               TileBehaviour l_lastTile = null;

               if (l_tiles.Count > 0)
               {
                  l_lastTile = l_tiles[l_tiles.Count - 1];
               }
               m_obstacleSpawner.Spawn(l_newTile.transform, l_data.Type, l_lastTile);
            }

            l_tiles.Add(l_newTile);
         }
      }

      private int TileLengthMajority(List<TileBehaviour> p_tiles)
      {
         Dictionary<int, int> l_count = new Dictionary<int, int>();
         int l_majority = -1;

         foreach (TileBehaviour l_tile in p_tiles)
         {
            int l_length = l_tile.Data.Length;
            if (l_count.ContainsKey(l_length))
            {
               l_count[l_length]++;

               if (l_count[l_length] > l_count[l_majority])
               {
                  l_majority = l_length;
               }
            }
            else
            {
               l_count[l_length] = 1;

               if (l_majority == -1)
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

         int l_length = Random.Range(0, l_maxLengthExclusive);

         if (p_tiles.Count != 0)
         {
            int l_lastLength = p_tiles[p_tiles.Count - 1].Data.Length;
            l_length = RandomIntHelper.GetRandomValue(l_lastLength, TileLengthMajority(p_tiles), l_maxLengthExclusive);
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
            EnvironmentEnum l_type = l_tile.Data.Type;
            if (l_count.ContainsKey(l_type))
            {
               l_count[l_type]++;

               if (l_count[l_type] > l_count[(EnvironmentEnum)l_majority])
               {
                  l_majority = (int)l_type;
               }
            }
            else
            {
               l_count[l_type] = 1;

               if (l_majority == -1)
               {
                  l_majority = (int)l_type;
               }
            }
         }

         return l_majority;
      }

      private EnvironmentEnum RandomTileType(List<TileBehaviour> p_tiles)
      {
         int l_enumSize = 3;

         int l_envId = Random.Range(0, l_enumSize);

         if (p_tiles.Count != 0)
         {
            EnvironmentEnum l_lastType = p_tiles[p_tiles.Count - 1].Data.Type;
            l_envId = RandomIntHelper.GetRandomValue((int)l_lastType, TileTypeMajority(p_tiles), l_enumSize);
         }

         return (EnvironmentEnum)l_envId;
      }
   }
}