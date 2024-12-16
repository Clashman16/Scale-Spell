using Behaviours.Characters;
using Behaviours.UI;
using Managers;
using UnityEngine;

namespace Behaviours
{
   namespace Interactables
   {
      public class PotionBehaviour : MonoBehaviour
      {
         private Gradient m_gradient;

         private float m_timer;
         private int m_timerCoef;

         private void Start()
         {
            m_gradient = new Gradient();

            GradientColorKey[] l_colorKeys = new GradientColorKey[3];
            l_colorKeys[0] = new GradientColorKey(Color.green, 0);
            l_colorKeys[1] = new GradientColorKey(Color.white, 0.5f);
            l_colorKeys[2] = new GradientColorKey(Color.green, 1);

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
               l_player.HasShield = true;
               GameObject l_shieldPrefab = Resources.Load<GameObject>("Prefabs/UI/Shield");
               GameObject l_instantiatedObject = Instantiate(l_shieldPrefab, l_player.transform);
               l_player.Shield = l_instantiatedObject.GetComponent<ShieldBehaviour>();
            }

            Destroy(gameObject);
         }

         private void Update()
         {
            if (GameStateManager.State == GameStateEnum.PLAYING)
            {
               Vector3 l_position = transform.position;

               float l_distance = Time.deltaTime;

               l_position.x -= l_distance;
               transform.position = l_position;

               Plane[] l_planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

               if (!GeometryUtility.TestPlanesAABB(l_planes, GetComponent<SpriteRenderer>().bounds))
               {
                  DestroyImmediate(gameObject);
               }
            }
         }
      }
   }
}
