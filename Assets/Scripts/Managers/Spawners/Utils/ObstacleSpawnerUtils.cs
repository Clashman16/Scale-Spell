using Behaviours.Map;
using Behaviours.Map.Obstacles;
using UnityEngine;

namespace Managers.Spawners.Utils
{
   public abstract class ObstacleSpawnerUtils : ObjectRecycler
   {
      private const string m_prefabsPath = "Prefabs/Map/Obstacles";

      public string PrefabsPath
      {
         get => m_prefabsPath;
      }

      public ObstacleSpawnerUtils() : base()
      {

      }

      internal abstract string GetPrefabName(bool p_bool, EnvironmentEnum p_tileType);

      internal abstract float GetYCoordinate(string p_prefabName);

      internal abstract ObstacleBehaviour Spawn(Transform p_tileTransform, EnvironmentEnum p_tileType);
   }
}
