using Managers;
using Managers.Spawners;
using UnityEngine;
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

         UIManagerSingleton.GetInstance();

         DestroyImmediate(gameObject);
      }
   }
}