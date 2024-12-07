using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Behaviours
{
   namespace UI
   {
      [RequireComponent(typeof(BoxCollider2D))]
      public class ScalabbleButtonBehaviour : MonoBehaviour
      {
         private Action m_action;

         private float m_minX;
         private float m_maxX;
         private float m_minY;
         private float m_maxY;

         public void AddListener(Action p_action)
         {
            m_action += p_action;
         }

         public void RemoveListener(Action p_action)
         {
            m_action -= p_action;
         }

         private void Start()
         {
            Vector3[] l_worldCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(l_worldCorners);

            m_minX = l_worldCorners[0].x;
            m_maxX = l_worldCorners[2].x;
            m_minY = l_worldCorners[0].y;
            m_maxY = l_worldCorners[2].y;
         }

         private void Update()
         {
            Vector3 l_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (l_mousePosition.x >= m_minX && l_mousePosition.x <= m_maxX
               && l_mousePosition.y >= m_minY && l_mousePosition.y <= m_maxY)
            {
               Vector3 l_scale = transform.localScale;
               l_scale -= new Vector3(0.005f, 0.005f, 0.005f);
               transform.localScale = l_scale;

               if (l_scale.x <= 0)
               {
                  m_action.Invoke();
               }
            }
            else
            {
               transform.localScale = Vector3.one;
            }
         }
      }
   }
}
