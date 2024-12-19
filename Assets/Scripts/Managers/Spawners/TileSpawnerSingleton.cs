using Behaviours.Map;
using System;
using System.Linq;
using UnityEngine;

namespace Managers.Spawners
{
   public sealed class TileSpawnerSingleton
   {
      private EnvironmentEnum m_newTileType;
      private int m_newLength;

      private TileBehaviour m_newTile;

      private int m_tileCount;

      private ObstacleSpawner m_obstacleSpawner;

      public TileBehaviour NewTile()
      {
         return m_newTile;
      }

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
         m_tileCount = 0;
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

      public void Spawn()
      {
         m_tileCount++;

         GameObject l_tile = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Map/Tile"));

         m_newTileType = RandomTileType();
         m_newLength = RandomLength();

         TileBehaviour l_lastTile = m_newTile;

         bool l_hasObstacle = (m_tileCount % 3 == 0 || m_tileCount % 5 == 0) && !l_lastTile.HasObstacle &&
                              m_newTileType != EnvironmentEnum.SAND;

         m_newTile = l_tile.GetComponent<TileBehaviour>();
         m_newTile.Init(m_newTileType, m_newLength, l_hasObstacle);

         float l_tileWidth = l_tile.GetComponent<Transform>().lossyScale.x;

         Vector3 l_screenPosition = new Vector3(Screen.width, 0, Camera.main.nearClipPlane);
         Vector3 l_worldPosition = Camera.main.ScreenToWorldPoint(l_screenPosition);
         Vector3 l_spawnPosition = l_worldPosition + new Vector3(l_tileWidth / 2, 0.5f, 0);
         m_newTile.transform.position = l_spawnPosition;

         m_timeBeforeSpawn = l_tileWidth;

         Bounds l_tileBound = l_tile.GetComponent<SpriteRenderer>().bounds;

         ObstacleBehaviour[] l_obstacles = MapManagerSingleton.GetInstance().Obstacles.ToArray();
         if (l_obstacles.Any(p_obstacle => p_obstacle.GetComponentsInChildren<SpriteRenderer>().Any(p_renderer => p_renderer.bounds.max.x >= l_tileBound.min.x)))
         {
            l_hasObstacle = false;
            m_newTile.HasObstacle = false;
         }

         if (l_hasObstacle)
         {
            ObstacleBehaviour l_obstacle = m_obstacleSpawner.Spawn(l_tile.transform, m_newTileType, l_lastTile);
            MapManagerSingleton.GetInstance().Obstacles.Add(l_obstacle);
         }
      }

      public int RandomLength()
      {
         int l_length = 0;

         int l_trialsCount = 3;
         while (l_trialsCount > 0)
         {
            l_length = UnityEngine.Random.Range(1, 5);

            if (l_length != m_newLength)
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

      private EnvironmentEnum RandomTileType()
      {
         int l_envId = 0;

         int l_trialsCount = 3;
         while (l_trialsCount > 0)
         {
            l_envId = UnityEngine.Random.Range(0, 3);

            if (l_envId != (int)m_newTileType)
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