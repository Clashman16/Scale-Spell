using System;

public static class GameStateManager
{
   public enum GameStateEnum
   {
      TITLESCREEN, PLAYING, PAUSED
   }

   private static GameStateEnum m_state = GameStateEnum.PLAYING;

   public static GameStateEnum State
   {
      get { return m_state; }
      set {
         m_gameStateChanged.Invoke(m_state);
         m_state = value;
      }
   }

   public static Action<GameStateEnum> m_gameStateChanged;
}
