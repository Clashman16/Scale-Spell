using Behaviours.Map;

namespace Utils
{
   public class TileData
   {
      bool m_hasObstacle;
      EnvironmentEnum m_type;
      int m_length;

      public TileData(bool p_hasObstacle, EnvironmentEnum p_type, int p_length)
      {
         m_hasObstacle = p_hasObstacle;
         m_type = p_type;
         m_length = p_length;
      }

      public bool HasObstacle
      {
         get => m_hasObstacle;
         set => m_hasObstacle = value;
      }

      public EnvironmentEnum Type
      {
         get => m_type;
      }

      public int Length
      {
         get => m_length;
      }
   }
}
