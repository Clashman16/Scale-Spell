using Behaviours.UI;
using Managers;
using UnityEngine;
using Utils.Callbacks;

namespace Behaviours
{
   public class TitleScreenInitializerBehaviour : MonoBehaviour
   {
      void Start()
      {
         GameStateManager.m_gameStateChanged += CallbacksLibrary.OnGameStateChanged;

         foreach (ScalabbleButtonBehaviour l_button in FindObjectsOfType<ScalabbleButtonBehaviour>(true))
         {
            if (l_button.name == "Play Button")
            {;
               l_button.AddListener(CallbacksLibrary.Restart);
            }
         }

         Destroy(gameObject);
      }
   }
}
