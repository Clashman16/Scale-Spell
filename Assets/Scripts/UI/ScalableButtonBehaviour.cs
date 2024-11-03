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

         public void AddListener(Action p_action)
         {
            m_action += p_action;
         }

         public void RemoveListener(Action p_action)
         {
            m_action -= p_action;
         }

         private void OnMouseOver()
         {
            Vector3 l_scale = transform.localScale;
            l_scale -= new Vector3(0.005f, 0.005f, 0.005f);
            transform.localScale = l_scale;

            if (l_scale.x <= 0.5f)
            {
               m_action.Invoke();
               transform.localScale = Vector3.one;
            }
         }

         private void OnMouseExit()
         {
            transform.localScale = Vector3.one;
         }
      }
   }
}
