using Behaviours.Map;
using Behaviours.UI;
using Managers;
using Managers.Spawners;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.Callbacks
{
   public static class CallbacksLibrary
   {
      public static void OnGameStateChanged(GameStateEnum p_currentGameState)
      {
         RectTransform l_pauseMenuTrf = Object.FindObjectsOfType<RectTransform>(true)
            .FirstOrDefault(p_transform => p_transform.name == "Pause Menu");
         if (l_pauseMenuTrf != null)
         {
            Object.FindObjectOfType<Canvas>().sortingLayerName = "UI";
            l_pauseMenuTrf.gameObject.SetActive(p_currentGameState == GameStateEnum.PAUSED);
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
               
         TextMeshProUGUI l_menuTitle = Object.FindObjectsOfType<TextMeshProUGUI>(true).First(p_button => p_button.name == "Menu Title");
         float l_distance = ScoreManagerSingleton.GetInstance().TravelledDistance;
         l_menuTitle.text = string.Concat((l_distance/1000).ToString("F3"), " m");
         l_menuTitle.color = Color.red;

         ScalabbleButtonBehaviour p_resumeButton = Object.FindObjectsOfType<ScalabbleButtonBehaviour>(true).First(p_button => p_button.name == "Resume Button");

         p_resumeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Try Again";
         p_resumeButton.RemoveListener(Resume);
         p_resumeButton.AddListener(Restart);

         ScoreManagerSingleton.Reset();
      }

      public static void OnMeterTravelled(float p_distance)
      {
         ScoreManagerSingleton l_scoreManager = ScoreManagerSingleton.GetInstance();
         l_scoreManager.TravelledDistance += p_distance;
         Object.FindObjectsOfType<TextMeshProUGUI>(true).First(p_label => p_label.name == "Travelled Distance").text =
            (l_scoreManager.TravelledDistance/1000).ToString("F3");
      }
      
      public static void OnIncreasePotionUsed(float p_usedQuantity)
      {
         ScoreManagerSingleton l_scoreManager = ScoreManagerSingleton.GetInstance();
         l_scoreManager.IncreasePotionQuantity -= p_usedQuantity;

         if(l_scoreManager.IncreasePotionQuantity <= 0)
         {
            l_scoreManager.IncreasePotionQuantity = 0;
            ScalerManager.EraseScaler();
         }

         Object.FindObjectsOfType<PotionIndicatorBehaviour>(true).First(p_indicator => p_indicator.name.Contains("Red")).UpdateSprite(true);
      }
      
      public static void OnDecreasePotionUsed(float p_usedQuantity)
      {
         ScoreManagerSingleton l_scoreManager = ScoreManagerSingleton.GetInstance();
         l_scoreManager.DecreasePotionQuantity -= p_usedQuantity;

         if (l_scoreManager.DecreasePotionQuantity <= 0)
         {
            l_scoreManager.DecreasePotionQuantity = 0;
            ScalerManager.EraseScaler();
         }

         Object.FindObjectsOfType<PotionIndicatorBehaviour>(true).First(p_indicator => p_indicator.name.Contains("Blue")).UpdateSprite(false);
      }

      public static void OnShieldTimerStartedOrFinished(bool p_hasShield)
      {
         ObstacleBehaviour[] l_obstacles = MapManagerSingleton.GetInstance().Obstacles.ToArray();
         foreach (ObstacleBehaviour l_obstacle in l_obstacles)
         {
            l_obstacle.EnableCollider(p_hasShield);
         }
      }
   }
}

