using Behaviours.Map;

namespace Managers.Spawners.Utils
{
   public abstract class ObstacleSpawnerUtils
   {
      internal abstract string GetPrefabName(bool p_bool, EnvironmentEnum p_tileType);

      internal abstract float GetYCoordinate(string p_prefabName);
   }
}
