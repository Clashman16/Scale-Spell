using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Utils
{
   public static class RandomIntHelper
   {
      public static int GetRandomValue(int p_last, int p_majority, int p_count)
      {
         int l_returnValue;

         int l_chance = Random.Range(0, 6);

         switch(l_chance)
         {
            case 0:
               l_returnValue = p_last;
               l_chance = Random.Range(0, 3);
               if (p_last == p_majority && l_chance != 0)
               {
                  l_returnValue = GetRandomValueOutsideList(new List<int>() { p_last }, p_count);
               }
               break;
            case 1:
               l_returnValue = p_majority;
               l_chance = Random.Range(0, 3);
               if (p_last == p_majority && l_chance != 0)
               {
                  l_returnValue = GetRandomValueOutsideList(new List<int>() { p_majority }, p_count);
               }
               break;
            default:
               l_returnValue = GetRandomValueOutsideList(new List<int>() { p_last, p_majority }, p_count);
               break;
         }

         return l_returnValue;
      }

      public static int GetRandomValueOutsideList(List<int> p_list, int p_count)
      {
         int l_returnValue = p_list[0];
         Stopwatch l_stopwatch = Stopwatch.StartNew();

         while (p_list.Contains(l_returnValue))
         {
            l_returnValue = Random.Range(0, p_count);

            if (l_stopwatch.ElapsedMilliseconds > 1000)
            {
               l_stopwatch.Stop();
               if(p_list.Contains(l_returnValue))
               {
                  p_list.Remove(l_returnValue);
               }
            }
         }

         return l_returnValue;
      }
   }
}
