using Behaviours.Managers;
using System.Linq;
using UnityEngine;

namespace Utils.Callbacks
{
   public static class CallbacksLibrary
   {
      public static void OnGameStateChanged(GameStateEnum p_currentGameState)
      {
         GameObject l_pauseMenu = Object.FindObjectsOfType<RectTransform>(true).First(p_transform => p_transform.name == "Pause Menu").gameObject;
         l_pauseMenu.SetActive(p_currentGameState == GameStateEnum.PAUSED);
      }

      public static void Resume()
      {
         GameStateManager.State = GameStateEnum.PLAYING;
      }

      public static void OnTimeSpawnerTimerFinished()
      {
         MapManagerSingleton.GetInstance().TileSpawner().Spawn();
      }
   }
}

