using Behaviours.Characters;
using Behaviours.Interactables;
using Managers;
using Managers.Spawners;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Behaviours
{
   namespace Map
   {
      public class TileBehaviour : MonoBehaviour
      {
         private TileData m_data;

         public TileData Data
         {
            get => m_data;
            set
            {
               m_data = value;
               
               ChangeSprite();
               Resize();

               InitRuler();
               DestroyRuler();
            }
         }

         public bool IsFullyVisible
         {
            get
            {
               Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
               return screenPoint.x > 0 && screenPoint.x < 1;
            }
         }

         private void Resize()
         {
            RulerBehaviour l_ruler = GetComponentInChildren<RulerBehaviour>();
            if (l_ruler != null)
            {
               Transform l_rulerTrf = l_ruler.transform;
               l_rulerTrf.SetParent(null);
            }
            
            Vector3 l_scale = transform.localScale;
            l_scale.x = m_data.Length;
            transform.localScale = l_scale;

            if (l_ruler != null)
            {
               Transform l_rulerTrf = l_ruler.transform;
               l_rulerTrf.SetParent(transform);
            }
         }

         private void ChangeSprite()
         {
            EnvironmentEnum l_type = m_data.Type;
            Color l_color = Color.white;

            switch(l_type)
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
            RulerBehaviour l_ruler = GetComponentInChildren<RulerBehaviour>();
            if(l_ruler != null)
            {
               if ((ScoreManagerSingleton.GetInstance().IncreasePotionQuantity >= 0.5 &&
                 ScoreManagerSingleton.GetInstance().DecreasePotionQuantity >= 0.5) ||
                m_data.HasObstacle)
               {
                  DestroyImmediate(l_ruler.gameObject);
               }
               else
               {
                  if (ScoreManagerSingleton.GetInstance().IncreasePotionQuantity > 0.7)
                  {
                     int l_chance = Random.Range(0, 10);
                     if (l_chance >= 2 && l_chance <= 6)
                     {
                        DestroyImmediate(l_ruler.gameObject);
                     }
                  }
                  if (ScoreManagerSingleton.GetInstance().DecreasePotionQuantity > 0.7)
                  {
                     int l_chance = Random.Range(0, 10);
                     if (l_chance >= 2 && l_chance <= 6)
                     {
                        DestroyImmediate(l_ruler.gameObject);
                     }
                  }
               }
            }
         }

         private void InitRuler()
         {
            RulerBehaviour l_ruler = GetComponentInChildren<RulerBehaviour>();
            if(l_ruler != null)
            {
               l_ruler.Init(ScoreManagerSingleton.GetInstance().IncreasePotionQuantity < ScoreManagerSingleton.GetInstance().DecreasePotionQuantity);
            }
         }

         private void Start()
         {
            if(m_data == null)
            {
               m_data = new TileData(false, EnvironmentEnum.GRASS, 1);

               MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
               List<TileBehaviour> l_tiles = l_mapManager.Tiles;
               if (l_tiles.Count == 0)
               {
                  l_tiles.Add(this);
               }
            }
         }

         private void Update()
         {
            if(GameStateManager.State == GameStateEnum.PLAYING)
            {
               Vector3 l_position = transform.position;

               float l_distance = Time.deltaTime;

               TileSpawnerSingleton l_tileSpawner = TileSpawnerSingleton.Instance;

               MapManagerSingleton l_mapManager = MapManagerSingleton.GetInstance();
               List<TileBehaviour> l_tiles = l_mapManager.Tiles;

               if (l_tiles[l_tiles.Count-1] == this)
               {
                  l_tileSpawner.TimeBeforeSpawn -= l_distance;

                  if(IsFullyVisible)
                  {
                     TileSpawnerSingleton.Instance.SpawnFirstWaitingTile();
                  }
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

