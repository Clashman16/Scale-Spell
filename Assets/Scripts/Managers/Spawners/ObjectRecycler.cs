using System.Collections.Generic;
using UnityEngine;

namespace Managers.Spawners
{
   public class ObjectRecycler
   {
      private Dictionary<string, List<GameObject>> m_objectsRecycleBin;

      internal Dictionary<string, List<GameObject>> RecycleBin
      {
         get => m_objectsRecycleBin;
      }

      public ObjectRecycler()
      {
         m_objectsRecycleBin = new Dictionary<string, List<GameObject>>();
      }

      private string GetObjectID(string p_objectName)
      {
         string l_id = p_objectName;

         l_id = l_id.Replace("(Clone)", "");

         return l_id;
      }

      internal void AddToRecycleBin(GameObject p_object)
      {
         string l_id = GetObjectID(p_object.name);

         if (m_objectsRecycleBin.ContainsKey(l_id))
         {
            if (m_objectsRecycleBin[l_id].Count == 5)
            {
               Object.Destroy(p_object);
            }
            else
            {
               m_objectsRecycleBin[l_id].Add(p_object);

               p_object.SetActive(false);
            } 
         }
         else
         {
            List<GameObject> l_objects = new List<GameObject>() { p_object };

            m_objectsRecycleBin.Add(l_id, l_objects);

            p_object.SetActive(false);;
         }
      }

      internal GameObject RemoveFromRecycleBin(string p_objectName)
      {
         string l_id = GetObjectID(p_objectName);

         if (m_objectsRecycleBin.ContainsKey(l_id) && m_objectsRecycleBin[l_id].Count > 0)
         {
            GameObject l_object = m_objectsRecycleBin[l_id][0];

            l_object.SetActive(true);

            m_objectsRecycleBin[l_id].RemoveAt(0);

            return l_object;
         }

         return null;
      }

      public bool IsRecycleBinEmpty
      {
         get => m_objectsRecycleBin.Count == 0;
      }
   }
}
