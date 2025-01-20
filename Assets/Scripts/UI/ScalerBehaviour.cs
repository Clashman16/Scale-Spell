using UnityEngine;

namespace Behaviours
{
   namespace UI
   {
      public class ScalerBehaviour : MonoBehaviour
      {
         private SpriteRenderer m_target;
         private LineRenderer m_lineRenderer;
         private Transform m_spellOrigin;

         public void Init(SpriteRenderer p_target, Transform p_spellOrigin)
         {
            m_target = p_target;

            m_lineRenderer = GetComponent<LineRenderer>();

            m_lineRenderer.material = new Material(Shader.Find("Custom/DashedLine"));
            m_lineRenderer.material.SetColor("_MainColor", Color.gray);

            m_lineRenderer.startWidth = 0.1f;
            m_lineRenderer.endWidth = 0.1f;

            m_spellOrigin = p_spellOrigin;
            m_spellOrigin.GetComponent<LineRenderer>().material.SetColor("_MainColor", Color.gray); ;

            UpdateScalerPosition();
         }

         private void UpdateScalerPosition()
         {
            if (m_target != null)
            {
               Vector3[] l_corners = new Vector3[5];

               Bounds l_bounds = m_target.bounds;
               l_corners[0] = new Vector3(l_bounds.min.x, l_bounds.min.y, 0);
               l_corners[1] = new Vector3(l_bounds.max.x, l_bounds.min.y, 0);
               l_corners[2] = new Vector3(l_bounds.max.x, l_bounds.max.y, 0);
               l_corners[3] = new Vector3(l_bounds.min.x, l_bounds.max.y, 0);
               l_corners[4] = l_corners[0];

               m_lineRenderer.positionCount = l_corners.Length;
               m_lineRenderer.SetPositions(l_corners);
               m_lineRenderer.sortingLayerName = "Scaler";

               LineRenderer l_spellRenderer = m_spellOrigin.GetComponent<LineRenderer>();
               l_spellRenderer.positionCount = 2;
               l_spellRenderer.SetPosition(0, m_spellOrigin.position);

               Vector3 l_spellEnd = (l_corners[3] + l_corners[4]) / 2f;
               l_spellRenderer.SetPosition(1, l_spellEnd);
            }
         }

         void Update()
         {
            UpdateScalerPosition();
         }
      }
   }
}
