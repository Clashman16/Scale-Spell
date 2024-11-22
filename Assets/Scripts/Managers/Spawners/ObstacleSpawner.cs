using Behaviours.Map;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Managers.Spawners
{
   public class ObstacleSpawner
   {
      private readonly string m_prefabsPath = "Prefabs/Map/Obstacles";
      private readonly string m_spritesPath = "Sprites/Map/Obstacles";

      internal void Spawn(Transform p_tileTransform, EnvironmentEnum p_tileType)
      {
         string l_prefabName = GetPrefabName(p_tileTransform, p_tileType);

         GameObject l_obstacle = Object.Instantiate(Resources.Load<GameObject>(Path.Combine(m_prefabsPath, l_prefabName)));

         Vector3 l_position = l_obstacle.transform.position;
         l_position.x = p_tileTransform.transform.position.x;
         l_position.y = GetYCoordinate(l_prefabName);
         l_obstacle.transform.position = l_position;

         string l_spriteName = l_prefabName.ToLower();
         l_spriteName = l_spriteName.Replace(" ", "-");
         ObstacleType obstacleType = GetObstacleType();
         l_obstacle.GetComponent<ObstacleBehaviour>().Init(obstacleType, Path.Combine(m_spritesPath, l_spriteName));
      }

      private string GetPrefabName(Transform p_tileTransform, EnvironmentEnum p_tileType)
      {
         string l_prefabName = "";
         int p_length = (int)p_tileTransform.localScale.x;

         if (p_length == 1)
         {
            l_prefabName += "Little";
         }
         else
         {
            int l_chance = Random.Range(0, p_length);
            l_prefabName += l_chance >= p_length / 2 ? "Little" : "Big";
         }

         l_prefabName += " ";

         switch (p_tileType)
         {
            case EnvironmentEnum.GRASS:
               l_prefabName += "Tree";
               break;
            case EnvironmentEnum.BRICKS:
               l_prefabName += "Tower";
               break;
         }

         return l_prefabName;
      }

      private float GetYCoordinate(string p_prefabName)
      {
         float l_y;

         switch (p_prefabName)
         {
            case "Big Tree":
               l_y = -1.15f;
               break;
            case "Little Tree":
               l_y = -0.85f;
               break;
            case "Big Tower":
               l_y = -0.75f;
               break;
            case "Little Tower":
               l_y = -1.62f;
               break;
            default:
               l_y = -0.05f;
               break;
         }
         return l_y;
      }

      private ObstacleType GetObstacleType()
      {
         ObstacleType l_obstacleType;

         if (ScoreManagerSingleton.GetInstance().IncreasePotionQuantity < 0.5 ||
                 ScoreManagerSingleton.GetInstance().DecreasePotionQuantity < 0.5)
         {
            if (Random.Range(0, 10) >= 5)
            {
               l_obstacleType = (ObstacleType)Random.Range(1, 3);
            }
            else
            {
               l_obstacleType = ObstacleType.HARMFUL;
            }
         }
         else
         {
            ObstacleBehaviour[] l_obstacles = Object.FindObjectsOfType<ObstacleBehaviour>();
            int[] l_typesCount = { 0, 0, 0 };

            foreach (ObstacleBehaviour l_obstacle in l_obstacles)
            {
               l_typesCount[(int)l_obstacle.GetObstacleType()] += 1;
            }

            if (l_typesCount[0] == l_typesCount[1] && l_typesCount[1] == l_typesCount[2])
            {
               l_obstacleType = (ObstacleType)Random.Range(0, 3);
            }
            else if (l_typesCount[0] == l_typesCount[1] && l_typesCount[2] > l_typesCount[1])
            {
               l_obstacleType = (ObstacleType)Random.Range(0, 2);
            }
            else if (l_typesCount[1] == l_typesCount[2] && l_typesCount[0] > l_typesCount[1])
            {
               l_obstacleType = (ObstacleType)Random.Range(1, 3);
            }
            else if (l_typesCount[0] == l_typesCount[2] && l_typesCount[1] > l_typesCount[0])
            {
               int l_randomValue = Random.Range(0, 2);
               if (l_randomValue == 1)
               {
                  l_randomValue = 2;
               }

               l_obstacleType = (ObstacleType)l_randomValue;
            }
            else
            {
               l_obstacleType = (ObstacleType)l_typesCount.Min();
            }
         }

         return l_obstacleType;
      }
   }
}
