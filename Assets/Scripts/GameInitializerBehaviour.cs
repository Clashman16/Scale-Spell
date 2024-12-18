using Behaviours.UI;
using Managers;
using Managers.Spawners;
using UnityEngine;
using UnityEngine.UI;
using Utils.Callbacks;

namespace Behaviours
{
    public class GameInitializerBehaviour : MonoBehaviour
    {
        void Start()
        {
         TileSpawnerSingleton l_tileSpawner = TileSpawnerSingleton.GetInstance();
         l_tileSpawner.m_timerFinished += CallbacksLibrary.OnTimeSpawnerTimerFinished;
         l_tileSpawner.TimeBeforeSpawn = 0;

         foreach (ScalabbleButtonBehaviour l_button in FindObjectsOfType<ScalabbleButtonBehaviour>(true))
            {
                if (l_button.name == "Resume Button")
                {
                    l_button.AddListener(CallbacksLibrary.Resume);
                }
                else
                {
                    l_button.AddListener(CallbacksLibrary.GoToTitlescreen);
                }
            }

         foreach (PotionIndicatorBehaviour l_potionIndicator in FindObjectsOfType<PotionIndicatorBehaviour>(true))
         {
            l_potionIndicator.Init(l_potionIndicator.name.Contains("Red") ? true : false);
         }

         DestroyImmediate(gameObject);
        }
    }
}