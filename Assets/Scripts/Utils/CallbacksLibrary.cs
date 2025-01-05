using Behaviours.Map.Obstacles;
using Behaviours.UI;
using Managers;
using Managers.Spawners;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

namespace Utils.Callbacks
{
   public static class CallbacksLibrary
   {
      public static void OnGameStateChanged(GameStateEnum p_currentGameState)
      {
         UIManagerSingleton l_uiManagerSingleton = UIManagerSingleton.GetInstance();
         if (l_uiManagerSingleton.CanDisplayPauseMenu)
         {
            l_uiManagerSingleton.DisplayPauseMenu(p_currentGameState == GameStateEnum.PAUSED);
         }
      }

      public static void Resume()
      {
         GameStateManager.State = GameStateEnum.PLAYING;
      }

      public static void Restart()
      {
         SceneManager.LoadScene("Level", LoadSceneMode.Single);
         GameStateManager.State = GameStateEnum.PLAYING;
      }

      public static void GoToTitlescreen()
      {
         SceneManager.LoadScene("Titlescreen", LoadSceneMode.Single);
         GameStateManager.State = GameStateEnum.PAUSED;
      }

      public static void OnTimeSpawnerTimerFinished()
      {
         TileSpawnerSingleton.GetInstance().Spawn();
      }

      public static void OnPlayerLoose()
      {
         GameStateManager.State = GameStateEnum.PAUSED;
               
         float l_distance = ScoreManagerSingleton.GetInstance().TravelledDistance/1000;

         UIManagerSingleton l_uiManagerSingleton = UIManagerSingleton.GetInstance();
         l_uiManagerSingleton.DisplayScoreOnGameOverMenu(l_distance);
         ScalableButtonBehaviour l_resumeButton = l_uiManagerSingleton.ResumeButton;

         l_resumeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Try Again";
         l_resumeButton.RemoveListener(Resume);
         l_resumeButton.AddListener(Restart);

         ScoreManagerSingleton.Reset();
         MapManagerSingleton.Reset();
      }

      public static void OnMeterTravelled(float p_distance)
      {
         ScoreManagerSingleton l_scoreManager = ScoreManagerSingleton.GetInstance();
         l_scoreManager.TravelledDistance += p_distance;
         UIManagerSingleton.GetInstance().UpdateDistanceDisplay(l_scoreManager.TravelledDistance / 1000);
      }
      
      public static void OnIncreasePotionUsed(float p_usedQuantity)
      {
         ScoreManagerSingleton l_scoreManager = ScoreManagerSingleton.GetInstance();
         l_scoreManager.IncreasePotionQuantity -= p_usedQuantity;

         UIManagerSingleton l_uiManager = UIManagerSingleton.GetInstance();

         if (l_scoreManager.IncreasePotionQuantity <= 0)
         {
            l_scoreManager.IncreasePotionQuantity = 0;
            l_uiManager.ScalerManager.EraseScaler();
         }

         l_uiManager.RedPotionIndicator.UpdateSprite(true);
      }
      
      public static void OnDecreasePotionUsed(float p_usedQuantity)
      {
         ScoreManagerSingleton l_scoreManager = ScoreManagerSingleton.GetInstance();
         l_scoreManager.DecreasePotionQuantity -= p_usedQuantity;

         UIManagerSingleton l_uiManager = UIManagerSingleton.GetInstance();

         if (l_scoreManager.DecreasePotionQuantity <= 0)
         {
            l_scoreManager.DecreasePotionQuantity = 0;
            l_uiManager.ScalerManager.EraseScaler();
         }

         l_uiManager.BluePotionIndicator.UpdateSprite(false);
      }

      public static void OnShieldTimerStartedOrFinished(bool p_hasShield)
      {
         MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
         List<ObstacleGroundedBehaviour> l_obstaclesGrounded = l_mapManager.ObstaclesGrounded;
         List<ObstacleFlyingBehaviour> l_obstaclesFlying = l_mapManager.ObstaclesFlying;
         List<ObstacleBehaviour> l_obstacles = new List<ObstacleBehaviour>();
         l_obstacles.AddRange(l_obstaclesGrounded);
         l_obstacles.AddRange(l_obstaclesFlying);
         foreach (ObstacleBehaviour l_obstacle in l_obstacles)
         {
            l_obstacle.EnableCollider(p_hasShield);
         }
      }
   }
}

