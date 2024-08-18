using Behaviours.Managers;
using Behaviours.Managers.Spawners;
using UnityEngine;
using Utils.Callbacks;

public class GameInitializerBehaviour : MonoBehaviour
{
    void Start()
    {
      GameStateManager.m_gameStateChanged += CallbacksLibrary.OnGameStateChanged;

      TileSpawner l_tileSpawner = MapManagerSingleton.GetInstance().TileSpawner();
      l_tileSpawner.m_timerFinished += CallbacksLibrary.OnTimeSpawnerTimerFinished;
      l_tileSpawner.TimeBeforeSpawn = 0;

      DestroyImmediate(gameObject);
   }
}
