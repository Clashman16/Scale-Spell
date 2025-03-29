using Managers;
using Managers.Spawners;
using UnityEngine;

namespace Behaviours
{
   public class GameInitializerBehaviour : MonoBehaviour
   {
      void Start()
      {
         TileSpawnerSingleton l_tileSpawner = TileSpawnerSingleton.Instance;
         l_tileSpawner.TimeBeforeSpawn = 0;

         UIManagerSingleton.GetInstance().Init();

         DestroyImmediate(gameObject);
      }
   }
}