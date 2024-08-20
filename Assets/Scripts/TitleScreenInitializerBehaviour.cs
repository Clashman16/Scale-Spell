using Behaviours.Managers;
using Behaviours.Managers.Spawners;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.Callbacks;

namespace Behaviours
{
    public class TitleScreenInitializerBehaviour : MonoBehaviour
    {
        void Start()
        {
            GameStateManager.m_gameStateChanged += CallbacksLibrary.OnGameStateChanged;

            foreach (Button l_button in FindObjectsOfType<Button>(true))
            {
                if (l_button.name == "Play Button")
                {
                    l_button.onClick.AddListener(CallbacksLibrary.Restart);
                }
            }

            DestroyImmediate(gameObject);
        }
    }
}
