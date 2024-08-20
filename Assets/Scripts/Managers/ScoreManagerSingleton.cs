using System;
using Utils.Callbacks;

namespace Managers
{
    public class ScoreManagerSingleton
    {
        private float m_travelledDistance;
        
        private Action<float> m_onTravelled;
        public Action<float> OnTravelled()
        {
            return m_onTravelled;
        }
        
        public float TravelledDistance
        {
            get { return m_travelledDistance; }
            set { m_travelledDistance = value; }
        }
        
        private float m_increasePotionQuantity;
        
        private Action<float> m_onIncreasePotionUsed;
        public Action<float> OnIncreasePotionUsed
        {
            get{return m_onIncreasePotionUsed;}
        }
        
        public float IncreasePotionQuantity
        {
            get { return m_increasePotionQuantity; }
            set { m_increasePotionQuantity = value; }
        }
        
        private float m_decreasePotionQuantity;
        
        private Action<float> m_onDecreasePotionUsed;
        public Action<float> OnDecreasePotionUsed
        {
            get{return m_onDecreasePotionUsed;}
        }
        
        public float DecreasePotionQuantity
        {
            get { return m_decreasePotionQuantity; }
            set { m_decreasePotionQuantity = value; }
        }

        private ScoreManagerSingleton()
        {
            m_travelledDistance = 0;
            m_increasePotionQuantity = 100;
            m_decreasePotionQuantity = 100;
            m_onTravelled += CallbacksLibrary.OnMeterTravelled;
            m_onIncreasePotionUsed += CallbacksLibrary.OnIncreasePotionUsed;
            m_onDecreasePotionUsed += CallbacksLibrary.OnDecreasePotionUsed;
        }

        private static ScoreManagerSingleton m_instance = null;

        public static ScoreManagerSingleton GetInstance()
        {
            if (m_instance == null)
            {
                m_instance = new ScoreManagerSingleton();
            }
            return m_instance;
        }
    }
}
