using System;


namespace Managers
{
    public static class GameStateManager
    {
        private static GameStateEnum m_state = GameStateEnum.PLAYING;

        public static GameStateEnum State
        {
            get { return m_state; }
            set
            {
                m_gameStateChanged.Invoke(value);
                m_state = value;
            }
        }

        public static Action<GameStateEnum> m_gameStateChanged;
    }

    public enum GameStateEnum
    {
        PLAYING,
        PAUSED
    }
}