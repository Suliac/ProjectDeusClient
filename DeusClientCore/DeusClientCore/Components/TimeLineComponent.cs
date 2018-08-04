using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public abstract class TimeLineComponent<T> : DeusComponent
    {
        /// <summary>
        /// The data we want with a timestamp
        /// </summary>
        protected List<DataTimed<T>> m_dataWithTime = new List<DataTimed<T>>();

        /// <summary>
        /// The number of datas we save in time
        /// </summary>
        private const uint MAX_DATAS_SAVED = 10000;

        /// <summary>
        /// Extrapolate the value for a timestamp
        /// </summary>
        /// <param name="timeStampMs">The time (in ms) at which we want to have our value</param>
        /// <returns>The value extrapolate</returns>
        public abstract T ExtrapolateValue(double timeStampMs);

        protected override void OnUpdate(decimal deltatimeMs)
        {
            // We clean our old datas
            while(m_dataWithTime.Count > Math.Max(0, MAX_DATAS_SAVED))
                m_dataWithTime.RemoveAt(0);
        }

        protected override void OnStop()
        {
            m_dataWithTime.Clear();
        }

        public void InsertData(T data, double timeStampMs)
        {
            m_dataWithTime.Add(new DataTimed<T>(data, timeStampMs));
        }

        public void InsertData(DataTimed<T> dataTimed)
        {
            m_dataWithTime.Add(dataTimed);
        }
    }
}
