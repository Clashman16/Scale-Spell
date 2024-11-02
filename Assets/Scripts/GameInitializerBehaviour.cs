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
            TileSpawner l_tileSpawner = MapManagerSingleton.GetInstance().TileSpawner();
            l_tileSpawner.m_timerFinished += CallbacksLibrary.OnTimeSpawnerTimerFinished;
            l_tileSpawner.TimeBeforeSpawn = 0;

            foreach (Button l_button in FindObjectsOfType<Button>(true))
            {
                if (l_button.name == "Resume Button")
                {
                    l_button.onClick.AddListener(CallbacksLibrary.Resume);
                }
                else
                {
                    l_button.onClick.AddListener(CallbacksLibrary.GoToTitlescreen);
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