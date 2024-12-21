using Behaviours.UI;
using System.Linq;
using TMPro;
using UnityEngine;
using Utils.Callbacks;

namespace Managers
{
   public class UIManagerSingleton
   {
      private static UIManagerSingleton m_instance = null;

      private PotionIndicatorBehaviour m_redPotionIndicator;

      public PotionIndicatorBehaviour RedPotionIndicator
      {
         get => m_redPotionIndicator;
      }

      PotionIndicatorBehaviour m_bluePotionIndicator;

      public PotionIndicatorBehaviour BluePotionIndicator
      {
         get => m_bluePotionIndicator;
      }

      private ScalerManager m_scalerManager;

      public ScalerManager ScalerManager
      {
         get => m_scalerManager;
      }

      private TextMeshProUGUI l_travelledDistanceIndicator;

      ScalabbleButtonBehaviour m_resumeButton;

      public ScalabbleButtonBehaviour ResumeButton
      {
         get => m_resumeButton;
      }

      RectTransform m_pauseMenuTrf;

      public bool CanDisplayPauseMenu => m_pauseMenuTrf != null;

      public static UIManagerSingleton GetInstance()
      {
         if (m_instance == null)
         {
            m_instance = new UIManagerSingleton();
         }
         return m_instance;
      }

      public void Init()
      {
         m_scalerManager = new ScalerManager();

         foreach (RectTransform l_uiElement in Object.FindObjectsByType<RectTransform>(FindObjectsSortMode.None))
         {
            PotionIndicatorBehaviour l_potionIndicator = l_uiElement.GetComponent<PotionIndicatorBehaviour>();
            if (l_potionIndicator != null)
            {
               if (l_potionIndicator.name.Contains("Red"))
               {
                  m_redPotionIndicator = l_potionIndicator;
                  m_redPotionIndicator.Init(true);
               }
               else
               {
                  m_bluePotionIndicator = l_potionIndicator;
                  m_bluePotionIndicator.Init(false);
               }
            }

            ScalabbleButtonBehaviour l_button = l_uiElement.GetComponent<ScalabbleButtonBehaviour>();
            if (l_button != null)
            {
               if (l_button.name == "Resume Button")
               {
                  m_resumeButton = l_button;
                  m_resumeButton.AddListener(CallbacksLibrary.Resume);
               }
               else
               {
                  l_button.AddListener(CallbacksLibrary.GoToTitlescreen);
               }
            }

            TextMeshProUGUI l_label = l_uiElement.GetComponent<TextMeshProUGUI>();

            if (l_label != null && l_label.name == "Travelled Distance")
            {
               l_travelledDistanceIndicator = l_label;
            }

            if (l_uiElement.name == "Pause Menu")
            {
               m_pauseMenuTrf = l_uiElement;
               m_pauseMenuTrf.gameObject.SetActive(false);
            }
         }
      }

      public void UpdateDistanceDisplay(float p_newDistance)
      {
         l_travelledDistanceIndicator.text = p_newDistance.ToString("F3");
      }

      public void DisplayScoreOnGameOverMenu(float p_score)
      {
         TextMeshProUGUI l_menuTitle = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None).First(p_button => p_button.name == "Menu Title");
         l_menuTitle.text = string.Concat(p_score.ToString("F3"), " m");
         l_menuTitle.color = Color.red;
      }

      public void DisplayPauseMenu(bool p_displayed)
      {
         Object.FindObjectOfType<Canvas>().sortingLayerName = "UI";
         m_pauseMenuTrf.gameObject.SetActive(p_displayed);
      }
   }
}
