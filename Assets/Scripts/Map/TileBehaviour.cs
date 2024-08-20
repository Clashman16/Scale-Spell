using Behaviours.Characters;
using Managers;
using Managers.Spawners;
using UnityEngine;

namespace Behaviours
{
   namespace Map
   {
      public class TileBehaviour : MonoBehaviour
      {
         private bool m_hasObstacle;

         public bool HasObstacle()
         {
            return m_hasObstacle;
         }

         public void Init(EnvironmentEnum p_environnement, int p_length, bool p_hasObstacle = false)
         {
            m_hasObstacle = p_hasObstacle;

            ChangeSprite(p_environnement);
            Resize(p_length);
         }

         private void Resize(int p_length)
         {
            Vector3 l_scale = transform.localScale;
            l_scale.x = p_length;
            transform.localScale = l_scale;
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

         void Update()
         {
            if(GameStateManager.State == GameStateEnum.PLAYING)
            {
               Vector3 l_position = transform.position;

               float l_distance = 0.005f;

               TileSpawner l_tileSpawner = MapManagerSingleton.GetInstance().TileSpawner();

               if (l_tileSpawner.LastTile() == this)
               {
                  l_tileSpawner.TimeBeforeSpawn -= l_distance;
               }
               
               l_position.x -= l_distance;
               transform.position = l_position;

               Bounds l_bounds = GetComponent<SpriteRenderer>().bounds;

               PlayerBehaviour l_player = FindObjectOfType<PlayerBehaviour>();
               if (l_player.GetComponent<SpriteRenderer>().bounds.min.x > l_bounds.min.x)
               {
                  float l_travelledDistance = l_player.GetComponent<SpriteRenderer>().bounds.min.x - l_bounds.min.x;
                  l_player.GetScoreManager().OnTravelled().Invoke(l_travelledDistance );
               }

               Plane[] l_planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
               if (!GeometryUtility.TestPlanesAABB(l_planes, l_bounds))
               {
                  DestroyImmediate(gameObject);
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

