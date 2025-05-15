using Behaviours.Characters;
using Managers;
using UnityEngine;

namespace Behaviours
{
   namespace Interactables
   {
      public class RulerBehaviour : MonoBehaviour
      {
         private bool m_isIncreaseMagic;
         private Gradient m_gradient;

         private float m_timer;
         private int m_timerCoef;

         public void Init(bool p_isIncreaseMagic)
         {
            m_isIncreaseMagic = p_isIncreaseMagic;

            m_gradient = new Gradient();

            GradientColorKey[] l_colorKeys = new GradientColorKey[2];
            l_colorKeys[0] = new GradientColorKey(Color.white, 0);

            l_colorKeys[1] = p_isIncreaseMagic ? new GradientColorKey(Color.red, 1) : new GradientColorKey(Color.blue, 1);

            GradientAlphaKey[] l_alphaKeys = new GradientAlphaKey[1] { new GradientAlphaKey(1, 1) };

            m_gradient.SetKeys(l_colorKeys, l_alphaKeys);

            m_timer = 0;
            m_timerCoef = 1;
         }

         private void FixedUpdate()
         {
            if (m_timer > 1 || m_timer < 0)
            {
               m_timerCoef *= -1;
            }

            GetComponent<SpriteRenderer>().color = m_gradient.Evaluate(m_timer);
            m_timer += 0.005f * m_timerCoef;
         }

         private void OnCollisionStay2D(Collision2D p_collision)
         {
            PlayerBehaviour l_player = p_collision.gameObject.GetComponent<PlayerBehaviour>();

            if (l_player != null)
            {
               if (m_isIncreaseMagic)
               {
                  ScoreManagerSingleton.GetInstance().OnIncreasePotionUsed.Invoke(-0.3f);
               }
               else
               {
                  ScoreManagerSingleton.GetInstance().OnDecreasePotionUsed.Invoke(-0.3f);
               }
            }

            gameObject.SetActive(false);
         }
      }
   }
}
