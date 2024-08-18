using Behaviours.Managers;
using static GameStateManager;

namespace Utils.Callbacks
{
   public static class CallbacksLibrary
   {
      public static void OnGameStateChanged(GameStateEnum p_previousGameState)
      {
         //if (p_previousGameState == GameStateEnum.TITLESCREEN)
         //{
         //   MapManagerSingleton.GetInstance().SpawnTile();
         //}
      }

      public static void OnTimeSpawnerTimerFinished()
      {
         MapManagerSingleton.GetInstance().TileSpawner().Spawn();
      }
   }
}

