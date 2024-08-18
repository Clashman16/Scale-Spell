using Behaviours.Managers;
using Behaviours.Managers.Spawners;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.Callbacks;

public class GameInitializerBehaviour : MonoBehaviour
{
    void Start()
    {
      GameStateManager.m_gameStateChanged += CallbacksLibrary.OnGameStateChanged;

      TileSpawner l_tileSpawner = MapManagerSingleton.GetInstance().TileSpawner();
      l_tileSpawner.m_timerFinished += CallbacksLibrary.OnTimeSpawnerTimerFinished;
      l_tileSpawner.TimeBeforeSpawn = 0;

      FindObjectsOfType<Button>(true).First(p_button => p_button.name == "Resume Button").onClick.AddListener(CallbacksLibrary.Resume);

      DestroyImmediate(gameObject);
   }
}
