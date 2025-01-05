using Behaviours.Characters;
using Behaviours.Map;
using Managers.Spawners;
using System.Collections.Generic;
using UnityEngine;

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

      private PlayerBehaviour m_player;

      public PlayerBehaviour Player
      {
         get => m_player;
      }

      private MapManagerSingleton()
      {
         m_tiles = new List<TileBehaviour>();
         m_obstaclesGrounded = new List<ObstacleGroundedBehaviour>();
         m_obstaclesFlying = new List<ObstacleFlyingBehaviour>();
         m_player = Object.FindObjectOfType<PlayerBehaviour>();
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

