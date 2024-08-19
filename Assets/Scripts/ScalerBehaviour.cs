using UnityEngine;

namespace Behaviours
{
   public class ScalerBehaviour : MonoBehaviour
   {
      private SpriteRenderer m_target;
      private LineRenderer m_lineRenderer;

      public void Init(SpriteRenderer p_target)
      {
         m_target = p_target;

         m_lineRenderer = GetComponent<LineRenderer>();
         m_lineRenderer.sortingLayerName = "Scaler";
         
         m_lineRenderer.material = new Material(Shader.Find("Standard"));
         m_lineRenderer.material.color = Color.red;
         
         m_lineRenderer.startWidth = 0.5f;
         m_lineRenderer.endWidth = 0.05f;

         UpdateScalerPosition();
      }

      private void UpdateScalerPosition()
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
      }

      void Update()
      {
         UpdateScalerPosition();
      }
   }
}
