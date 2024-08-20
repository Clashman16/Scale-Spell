using Managers.Spawners;

namespace Managers
{
    public sealed class MapManagerSingleton
    {
        private TileSpawner m_tileSpawner;
         
        public TileSpawner TileSpawner()
        {
            return m_tileSpawner;
        }

        private MapManagerSingleton()
        {
            m_tileSpawner = new TileSpawner();
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
    }
}

