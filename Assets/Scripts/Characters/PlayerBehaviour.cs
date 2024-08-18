using Behaviours.Managers;
using UnityEngine;

namespace Behaviours
{
   namespace Characters
   {
      public class PlayerBehaviour : MonoBehaviour
      {
         void Update()
         {
            if (GameStateManager.State == GameStateEnum.PLAYING)
            {
               if(Input.GetMouseButton(0))
               {
                  Vector3 l_position = transform.position;
                  l_position.y += Input.mousePosition.y < Screen.height / 2 ? -0.005f : 0.005f;
                  transform.position = l_position;
               }

               if (Input.GetMouseButtonDown(1))
               {
                  GameStateManager.State = GameStateEnum.PAUSED;
               }
            }
         }
      }

   }
}
