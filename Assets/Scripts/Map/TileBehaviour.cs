using Behaviours.Characters;
using Behaviours.Interactables;
using Managers;
using Managers.Spawners;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
   namespace Map
   {
      public class TileBehaviour : MonoBehaviour
      {
         private bool m_hasObstacle;

         public bool HasObstacle
         {
            get => m_hasObstacle;
            set => m_hasObstacle = value;
         }

         private EnvironmentEnum m_type;

         public EnvironmentEnum Type
         {
            get => m_type;
         }

         private int m_length;

         public int Length
         {
            get => m_length;
         }

         public void Init(EnvironmentEnum p_environnement, int p_length, bool p_hasObstacle = false)
         {
            m_hasObstacle = p_hasObstacle;
            m_type = p_environnement;
            m_length = p_length;

            ChangeSprite(p_environnement);
            Resize();

            InitRuler();
            DestroyRuler();
         }

         private void Resize()
         {
            Transform l_rulerTrf = GetComponentInChildren<RulerBehaviour>().transform;
            l_rulerTrf.SetParent(null);

            Vector3 l_scale = transform.localScale;
            l_scale.x = m_length;
            transform.localScale = l_scale;

            l_rulerTrf.SetParent(transform);
         }

         private void ChangeSprite(EnvironmentEnum p_environnement)
         {
            Color l_color = Color.white;

            switch(p_environnement)
            {
               case EnvironmentEnum.BRICKS:
                  l_color = Color.gray;
                  break;
               case EnvironmentEnum.SAND:
                  l_color = Color.yellow;
                  break;
               case EnvironmentEnum.GRASS:
                  l_color = Color.green;
                  break;
            }

            GetComponent<SpriteRenderer>().color = l_color;
         }

         private void DestroyRuler()
         {
            if ((ScoreManagerSingleton.GetInstance().IncreasePotionQuantity >= 0.5 &&
                 ScoreManagerSingleton.GetInstance().DecreasePotionQuantity >= 0.5) ||
                m_hasObstacle)
            {
               RulerBehaviour l_ruler = GetComponentInChildren<RulerBehaviour>();
               DestroyImmediate(l_ruler.gameObject);
            }
            else
            {
               if (ScoreManagerSingleton.GetInstance().IncreasePotionQuantity > 0.7)
               {
                  int l_chance = Random.Range(0, 10);
                  if (l_chance >= 2 && l_chance <= 6)
                  {
                     RulerBehaviour l_ruler = GetComponentInChildren<RulerBehaviour>();
                     DestroyImmediate(l_ruler.gameObject);
                  }
               }
               if (ScoreManagerSingleton.GetInstance().DecreasePotionQuantity > 0.7)
               {
                  RulerBehaviour[] l_rulers = GetComponentsInChildren<RulerBehaviour>();
                  if (l_rulers != null && l_rulers.Length > 0)
                  {
                     int l_chance = Random.Range(0, 10);
                     if (l_chance >= 2 && l_chance <= 6)
                     {
                        RulerBehaviour l_ruler = GetComponentInChildren<RulerBehaviour>();
                        DestroyImmediate(l_ruler.gameObject);
                     }
                  }
               }
            }
         }

         private void InitRuler()
         {
            RulerBehaviour l_ruler = GetComponentInChildren<RulerBehaviour>();
            l_ruler.Init(ScoreManagerSingleton.GetInstance().IncreasePotionQuantity < ScoreManagerSingleton.GetInstance().DecreasePotionQuantity);
         }

         void Update()
         {
            if(GameStateManager.State == GameStateEnum.PLAYING)
            {
               Vector3 l_position = transform.position;

               float l_distance = Time.deltaTime;

               TileSpawnerSingleton l_tileSpawner = TileSpawnerSingleton.GetInstance();

               MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
               List<TileBehaviour> l_tiles = l_mapManager.Tiles;

               if (l_tiles[l_tiles.Count-1] == this)
               {
                  l_tileSpawner.TimeBeforeSpawn -= l_distance;
               }
               
               l_position.x -= l_distance;
               transform.position = l_position;

               Bounds l_bounds = GetComponent<SpriteRenderer>().bounds;

               PlayerBehaviour l_player = l_mapManager.Player;
               if (l_player.GetComponent<SpriteRenderer>().bounds.min.x > l_bounds.min.x)
               {
                  float l_travelledDistance = l_player.GetComponent<SpriteRenderer>().bounds.min.x - l_bounds.min.x;
                  ScoreManagerSingleton.GetInstance().OnTravelled().Invoke(l_travelledDistance);
               }
            }
         }
      }

      public enum EnvironmentEnum
      {
         BRICKS, SAND, GRASS, NONE
      }
   }
}

