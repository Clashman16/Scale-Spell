using System;
using Utils.Callbacks;

namespace Managers
{
    public class ScoreManager
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
        public float IncreasePotionQuantity
        {
            get { return m_increasePotionQuantity; }
            set { m_increasePotionQuantity = value; }
        }
        
        private float m_decreasePotionQuantity;
        public float DecreasePotionQuantity
        {
            get { return m_decreasePotionQuantity; }
            set { m_decreasePotionQuantity = value; }
        }

        public ScoreManager()
        {
            m_travelledDistance = 0;
            m_increasePotionQuantity = 100;
            m_decreasePotionQuantity = 100;
            m_onTravelled += CallbacksLibrary.OnMeterTravelled;
        }
    }
}
