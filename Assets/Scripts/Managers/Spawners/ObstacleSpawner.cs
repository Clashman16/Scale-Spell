using Behaviours.Characters;
using Behaviours.Map;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utils;

namespace Managers.Spawners
{
   public class ObstacleSpawner
   {
      private readonly string m_prefabsPath = "Prefabs/Map/Obstacles";
      private readonly string m_spritesPath = "Sprites/Map/Obstacles";

      private PotionSpawner m_potionSpawner;

      public ObstacleSpawner()
      {
         m_potionSpawner = new PotionSpawner();
      }

      internal void Spawn(Transform p_tileTransform, EnvironmentEnum p_tileType, TileBehaviour p_lastTile)
      {
         List<ObstacleGroundBehaviour> l_obstacles = MapManagerSingleton.GetInstance().Obstacles;
         ObstacleType l_obstacleType = RandomObstacleType(l_obstacles);
         bool l_isBig = RandomSize(p_tileTransform, l_obstacles);
         string l_prefabName = GetPrefabName(l_isBig, p_tileType);

         int l_travelledDistance = Mathf.RoundToInt(ScoreManagerSingleton.GetInstance().TravelledDistance);
         PlayerBehaviour l_player = Object.FindObjectOfType<PlayerBehaviour>();
         bool l_hasObstacle = p_lastTile != null && !p_lastTile.HasObstacle;

         if (!l_player.HasShield && l_obstacleType == ObstacleType.HARMFUL && l_hasObstacle && (l_travelledDistance % 5 == 0 || l_travelledDistance % 10 == 0))
         {
            m_potionSpawner.Spawn(p_lastTile.transform);
         }

         GameObject l_spawnedObject = Object.Instantiate(Resources.Load<GameObject>(Path.Combine(m_prefabsPath, l_prefabName)));

         Vector3 l_position = l_spawnedObject.transform.position;
         l_position.x = p_tileTransform.transform.position.x;
         l_position.y = GetYCoordinate(l_prefabName);
         l_spawnedObject.transform.position = l_position;

         string l_spriteName = l_prefabName.ToLower();
         l_spriteName = l_spriteName.Replace(" ", "-");

         ObstacleGroundBehaviour l_obstacle = l_spawnedObject.GetComponent<ObstacleGroundBehaviour>();
         l_obstacle.Init(l_obstacleType, l_isBig, Path.Combine(m_spritesPath, l_spriteName));

         if(l_player.HasShield)
         {
            l_obstacle.EnableCollider(false);
         }
      }

      private string GetPrefabName(bool p_isBig, EnvironmentEnum p_tileType)
      {
         string l_prefabName = p_isBig ? "Big" : "Little";
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

      private int ObstacleSizeMajority(List<ObstacleGroundBehaviour> p_obstacles)
      {
         Dictionary<int, int> l_count = new Dictionary<int, int>();
         int l_majority = -1;

         foreach (ObstacleGroundBehaviour l_obstacle in p_obstacles)
         {
            int l_isBigId = l_obstacle.IsBig ? 1 : 0;

            if (l_count.ContainsKey(l_isBigId))
            {
               l_count[l_isBigId]++;

               if (l_count[l_isBigId] > l_count[l_majority])
               {
                  l_majority = l_isBigId;
               }
            }
            else
            {
               l_count[l_isBigId] = 1;

               if (l_majority == -1)
               {
                  l_majority = l_isBigId;
               }
            }
         }

         return l_majority;
      }


      private bool RandomSize(Transform p_tileTransform, List<ObstacleGroundBehaviour> p_obstacles)
      {
         int p_length = (int) p_tileTransform.localScale.x;

         int l_isBigId = p_length <= 2 ? 0 : 1;

         if (l_isBigId == 1)
         {
            int l_maxLengthExclusive = 2;

            l_isBigId = Random.Range(0, l_maxLengthExclusive);

            if (p_obstacles.Count != 0)
            {
               int l_lastIsBigId = p_obstacles[p_obstacles.Count - 1].IsBig ? 1 : 0;
               l_isBigId = RandomIntHelper.GetRandomValue(l_lastIsBigId, ObstacleSizeMajority(p_obstacles), l_maxLengthExclusive);
            }
         }

         return l_isBigId == 0 ? false : true;
      }

      private int ObstacleTypeMajority(List<ObstacleGroundBehaviour> p_obstacles)
      {
         Dictionary<ObstacleType, int> l_count = new Dictionary<ObstacleType, int>();
         int l_majority = -1;

         foreach (ObstacleGroundBehaviour l_obstacle in p_obstacles)
         {
            ObstacleType l_type = l_obstacle.Type;
            if (l_count.ContainsKey(l_type))
            {
               l_count[l_obstacle.Type]++;

               if (l_count[l_type] > l_count[(ObstacleType)l_majority])
               {
                  l_majority = (int)l_type;
               }
            }
            else
            {
               l_count[l_type] = 1;

               if (l_majority == -1)
               {
                  l_majority = (int)l_type;
               }
            }
         }

         return l_majority;
      }

      private ObstacleType RandomObstacleType(List<ObstacleGroundBehaviour> p_obstacles)
      {
         int l_envId;
         int l_enumSize = 3;

         ScoreManagerSingleton l_scoreManager = ScoreManagerSingleton.GetInstance();
         float l_increasePotionQuantity = l_scoreManager.IncreasePotionQuantity;
         float l_decreasePotionQuantity = l_scoreManager.DecreasePotionQuantity;

         if (ScoreManagerSingleton.GetInstance().IncreasePotionQuantity < 0.5 ||
                 ScoreManagerSingleton.GetInstance().DecreasePotionQuantity < 0.5)
         {
            if (l_increasePotionQuantity > l_decreasePotionQuantity)
            {
               l_envId = Random.Range(1, l_enumSize);
            }
            else if (l_increasePotionQuantity < l_decreasePotionQuantity)
            {
               l_envId = (int) ObstacleType.HARMFUL;
            }
            else
            {
               l_envId = (int)ObstacleType.INBETWEEN;
            }
         }
         else
         {
            l_envId = Random.Range(0, l_enumSize);

            if (p_obstacles.Count != 0)
            {
               ObstacleType l_lastType = p_obstacles[p_obstacles.Count - 1].Type;
               l_envId = RandomIntHelper.GetRandomValue((int)l_lastType, ObstacleTypeMajority(p_obstacles), l_enumSize);
            }
         }

         return (ObstacleType) l_envId;
      }
   }
}
