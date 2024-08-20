using Behaviours.Managers;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
         MapManagerSingleton.GetInstance().TileSpawner().Spawn();
      }

      public static void OnPlayerLoose()
      {
         GameStateManager.State = GameStateEnum.PAUSED;
               
         TextMeshProUGUI l_menuTitle = Object.FindObjectsOfType<TextMeshProUGUI>(true).First(p_button => p_button.name == "Menu Title");
         l_menuTitle.text = "You loose!";
         l_menuTitle.color = Color.red;
               
         Button p_resumeButton = Object.FindObjectsOfType<Button>(true).First(p_button => p_button.name == "Resume Button");

         p_resumeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Try Again";
         p_resumeButton.onClick.RemoveListener(Resume);
         p_resumeButton.onClick.AddListener(Restart);
      }
   }
}

