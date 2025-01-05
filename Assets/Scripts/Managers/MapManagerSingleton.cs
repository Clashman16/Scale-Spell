using Behaviours.Map;
using Managers.Spawners;
using System.Collections.Generic;

namespace Managers
{
   public sealed class MapManagerSingleton
   {
      private TileSpawnerSingleton m_tileSpawner;

      private List<TileBehaviour> m_tiles;
      public List<TileBehaviour> Tiles
      {
         get => m_tiles;
      }

      private List<ObstacleGroundBehaviour> m_obstacles;
      public List<ObstacleGroundBehaviour> Obstacles
      {
         get => m_obstacles;
      }

      private MapManagerSingleton()
      {
         m_tiles = new List<TileBehaviour>();
         m_obstacles = new List<ObstacleGroundBehaviour>();
      }

      private static MapManagerSingleton m_instance = null;

      public static MapManagerSingleton GetInstance()
      {
         if (m_instance == null)
         {
            m_instance = new MapManagerSingleton();
         }
         return m_instance;
      }

      public static void Reset()
      {
         m_instance = null;
      }
   }
}

