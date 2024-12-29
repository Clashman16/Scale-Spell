using Managers;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviours
{
   namespace UI
   {
      public class PotionIndicatorBehaviour : MonoBehaviour
      {
         private Sprite[] m_spritesheet;

         [SerializeField] private Image m_image;

         public void Init(bool p_increasePotion)
         {
            string l_spritesheetName = p_increasePotion ? "red" : "blue";
            string l_path = Path.Combine("Sprites", "UI", string.Concat(l_spritesheetName, "-potion-indicator"));

            m_spritesheet = Resources.LoadAll<Sprite>(l_path);
         }

         public void UpdateSprite(bool p_increasePotion)
         {
            ScoreManagerSingleton l_scoreManager = ScoreManagerSingleton.GetInstance();
            int l_index = Mathf.RoundToInt(10*(p_increasePotion ? l_scoreManager.IncreasePotionQuantity : l_scoreManager.DecreasePotionQuantity));

            if(l_index > 10)
            {
               l_index = 10;
            }

            if (l_index < 0)
            {
               l_index = 0;
            }

            m_image.sprite = m_spritesheet[l_index];
         }
      }
   }
}
