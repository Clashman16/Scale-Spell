using Behaviours.Characters;
using UnityEngine;

namespace Behaviours
{
    namespace Interactables
    {
        public class RulerBehaviour : MonoBehaviour
        {
            private bool m_isIncreaseMagic;
            
            public void Init(bool p_isIncreaseMagic)
            {
                m_isIncreaseMagic = p_isIncreaseMagic;
            }
            
            private void OnCollisionStay2D(Collision2D p_collision)
            {
                PlayerBehaviour l_player = p_collision.gameObject.GetComponent<PlayerBehaviour>();
                
                if (l_player != null)
                {
                    if(m_isIncreaseMagic)
                    {
                        l_player.GetScoreManager().OnIncreasePotionUsed().Invoke(-0.05f);
                    }
                    else
                    {
                        l_player.GetScoreManager().OnDecreasePotionUsed().Invoke(-0.05f);
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
