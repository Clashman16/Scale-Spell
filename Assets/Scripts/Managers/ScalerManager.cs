using UnityEngine;
using Behaviours.UI;

namespace Managers
{
   public class ScalerManager
   {
      private GameObject m_scalerGameObject;
      private Transform m_spellOrigin;

      private Vector3 m_previousMousePosition;
      public Vector3 PreviousMousePosition
      {
         get { return m_previousMousePosition; }
         set
         {
            m_previousMousePosition = value;
         }
      }

      private bool m_isRescaling = false;
      public bool IsRescaling
      {
         get { return m_isRescaling; }
         set
         {
            m_isRescaling = value;
            MapManagerSingleton.GetInstance().Player.GetAnimator().SetBool("IsSpelling", m_isRescaling);

            if(!m_isRescaling)
            {
               LineRenderer l_spellRenderer = m_spellOrigin.GetComponent<LineRenderer>();
               l_spellRenderer.positionCount = 0;
            }
         }
      }

      public ScalerManager()
      {
         LineRenderer l_spellRenderer = MapManagerSingleton.GetInstance().Player.GetComponentInChildren<LineRenderer>();
         l_spellRenderer.material = new Material(Shader.Find("Custom/DashedLine"));
         l_spellRenderer.material.SetColor("_MainColor", Color.gray);
         l_spellRenderer.startWidth = 0.1f;
         l_spellRenderer.endWidth = 0.1f;

         m_spellOrigin = l_spellRenderer.transform;
      }

      public void DrawScaler(SpriteRenderer l_spriteRenderer)
      {
         m_scalerGameObject = new GameObject("Scaler");
         m_scalerGameObject.AddComponent<LineRenderer>();
         ScalerBehaviour l_scaler = m_scalerGameObject.AddComponent<ScalerBehaviour>();
         l_scaler.Init(l_spriteRenderer, m_spellOrigin);
      }

      public void EraseScaler()
      {
         if(m_scalerGameObject != null)
         {
            Object.DestroyImmediate(m_scalerGameObject);
         }
      }

      public void UpdateColor(Color p_color)
      {
         if(m_scalerGameObject != null)
         {
            m_scalerGameObject.GetComponent<LineRenderer>().material.SetColor("_MainColor", p_color);
         }

         LineRenderer l_spellRenderer = m_spellOrigin.GetComponent<LineRenderer>();
         if (l_spellRenderer != null)
         {
            l_spellRenderer.material.SetColor("_MainColor", p_color);
         }
      }
   }
}
