using Behaviours.Characters;
using UnityEngine;

namespace Managers.Spawners
{
   public class PotionSpawner
   {
      private readonly string m_prefabsPath = "Prefabs/Items/Potion";

      internal void Spawn(Transform p_tileTransform)
      {
         GameObject l_potion = Object.Instantiate(Resources.Load<GameObject>(m_prefabsPath));

         PlayerBehaviour l_player = MapManagerSingleton.GetInstance().Player;
         if (CloseToPlayer())
         {
            l_potion.transform.position = new Vector3(p_tileTransform.position.x, l_player.transform.position.y, p_tileTransform.position.z);
         }
         else
         {
            l_potion.transform.position = new Vector3(p_tileTransform.position.x, GetSymetricalPosition(l_player.transform.position), p_tileTransform.position.z);
         }
      }

      private bool CloseToPlayer()
      {
         int l_chance = Random.Range(0, 3);

         if(l_chance == 2)
         {
            return true;
         }

         return false;
      }

      private float GetSymetricalPosition(Vector3 p_position)
      {
         Vector3 l_screenPosition = Camera.main.WorldToScreenPoint(p_position);
         float l_screenCenterY = Screen.height / 2;
         float l_symmetricY = 2 * l_screenCenterY - l_screenPosition.y;

         Vector3 l_symmetricScreenPosition = new Vector3(l_screenPosition.x, l_symmetricY, l_screenPosition.z);
         Vector3 l_symmetricWorldPosition = Camera.main.ScreenToWorldPoint(l_symmetricScreenPosition);

         return l_symmetricWorldPosition.y;
      }
   }
}
