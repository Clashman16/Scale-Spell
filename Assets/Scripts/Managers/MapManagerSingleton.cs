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

      private List<ObstacleGroundedBehaviour> m_obstaclesGrounded;
      public List<ObstacleGroundedBehaviour> ObstaclesGrounded
      {
         get => m_obstaclesGrounded;
      }

      private List<ObstacleFlyingBehaviour> m_obstaclesFlying;
      public List<ObstacleFlyingBehaviour> ObstaclesFlying
      {
         get => m_obstaclesFlying;
      }

      private MapManagerSingleton()
      {
         m_tiles = new List<TileBehaviour>();
         m_obstaclesGrounded = new List<ObstacleGroundedBehaviour>();
         m_obstaclesFlying = new List<ObstacleFlyingBehaviour>();
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

