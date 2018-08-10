using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public abstract class TimeLineComponent<T> : DeusComponent, IViewableComponent
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
        /// We want to know if the linked <see cref="DeusViewComponent"/> will have to be updated in realtime :
        /// That means that if realtime update is needed, the <see cref="DeusViewComponent"/> will call directly our GetViewValue() function
        /// without passing bu event queue
        /// </summary>
        public bool RealtimeViewUpdate { get; protected set; }

        public TimeLineComponent(bool needRealtimeUpdateView)
        {
            RealtimeViewUpdate = needRealtimeUpdateView;
        }

        protected override void OnUpdate(decimal deltatimeMs)
        {
            // We clean our old datas
            while (m_dataWithTime.Count > Math.Max(0, MAX_DATAS_SAVED))
                m_dataWithTime.RemoveAt(0);
        }

        protected override void OnStop()
        {
            m_dataWithTime.Clear();
        }

        public void InsertData(T data)
        {
            InsertData(new DataTimed<T>(data, TimeHelper.GetUnixMsTimeStamp() + 200));
        }

        public void InsertData(T data, long timeStampMs)
        {
            InsertData(new DataTimed<T>(data, timeStampMs));
        }

        public void InsertData(DataTimed<T> dataTimed)
        {
            m_dataWithTime.Add(dataTimed);
        }

        public abstract object GetViewValue(long timeStampMs = -1);
    }
}
