using UnityEngine;

namespace Behaviours
{
   namespace UI
   {
      [RequireComponent(typeof(LineRenderer))]
      public class ShieldBehaviour : MonoBehaviour
      {
         private float m_radius;
         private Transform m_target;

         private float m_timer;
         private int m_timerCoef;

         private void Start()
         {
            LineRenderer l_lineRenderer = GetComponent<LineRenderer>();

            l_lineRenderer.loop = true;
            l_lineRenderer.useWorldSpace = false;
            l_lineRenderer.positionCount = 100;
            l_lineRenderer.startWidth = 0.2f;
            l_lineRenderer.endWidth = 0.2f;
            l_lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            l_lineRenderer.sortingLayerName = "Scaler";

            Sprite l_parentSprite = GetComponentInParent<SpriteRenderer>().sprite;
            float l_spriteWidth = l_parentSprite.rect.width / l_parentSprite.pixelsPerUnit;
            m_radius = l_spriteWidth * transform.lossyScale.x * 2;

            m_target = transform.parent;
            transform.SetParent(null);

            InitGradient(l_lineRenderer);
         }

         private void Update()
         {
            if (m_timer > 1 || m_timer < 0)
            {
               m_timerCoef *= -1;
            }

            transform.position = m_target.position;
            transform.RotateAround(transform.position, transform.forward, 1f);

            LineRenderer l_lineRenderer = GetComponent<LineRenderer>();

            UpdateCircle(l_lineRenderer);

            m_timer += 0.005f * m_timerCoef;
         }

         private void InitGradient(LineRenderer p_renderer)
         {
            Gradient l_gradient = new Gradient();

            GradientColorKey[] l_colorKeys = new GradientColorKey[3];
            l_colorKeys[0] = new GradientColorKey(Color.white, 0);
            l_colorKeys[1] = new GradientColorKey(Color.green, 0.5f);
            l_colorKeys[2] = new GradientColorKey(Color.white, 1);

            GradientAlphaKey[] l_alphaKeys = new GradientAlphaKey[1] { new GradientAlphaKey(1, 1) };

            l_gradient.SetKeys(l_colorKeys, l_alphaKeys);
            p_renderer.colorGradient = l_gradient;
         }

         private void UpdateCircle(LineRenderer p_renderer)
         {
            Vector3[] l_points = new Vector3[p_renderer.positionCount];
            float l_angleStep = 360f / p_renderer.positionCount;

            for (int i = 0; i < p_renderer.positionCount; i++)
            {
               float l_angle = i * l_angleStep * Mathf.Deg2Rad;
               float l_x = Mathf.Cos(l_angle) * m_radius;
               float l_y = Mathf.Sin(l_angle) * m_radius;
               l_points[i] = new Vector3(l_x, l_y, transform.position.z);
            }

            p_renderer.SetPositions(l_points);
         }
      }
   }
}
