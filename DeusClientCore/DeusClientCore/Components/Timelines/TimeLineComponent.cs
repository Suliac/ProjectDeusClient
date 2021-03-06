﻿using DeusClientCore.Events;
using DeusClientCore.Exceptions;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public abstract class TimeLineComponent<T> : DeusComponent, IViewableComponent
    {
        private const bool WANT_DATA_BEFORE_TIMESTAMP = true;

        /// <summary>
        /// The data we want with a timestamp
        /// </summary>
        public List<DataTimed<T>> m_dataWithTime = new List<DataTimed<T>>();

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

        public TimeLineComponent(bool needRealtimeUpdateView, uint identifier, uint objectIdentifier, EComponentType type, DataTimed<T> origin, DataTimed<T> destination = null) : base(identifier, objectIdentifier, type)
        {
            RealtimeViewUpdate = needRealtimeUpdateView;

            m_componentType = type;

            if (origin != null)
                InsertData(origin);
            if (destination != null)
                InsertData(destination);
        }

        public void InsertData(T data, uint timeStampMs)
        {
            InsertData(new DataTimed<T>(data, timeStampMs));
        }

        public void InsertData(DataTimed<T> dataTimed)
        {
            if (dataTimed == null || dataTimed.Data == null)
                throw new DeusException("Try to insert null value");

            // We delete all the futur datas that arn't valid anymore
            m_dataWithTime.RemoveAll(dt => dt.TimeStampMs >= dataTimed.TimeStampMs);

            // then we add our data
            m_dataWithTime.Add(dataTimed);
        }

        public object GetViewValue(uint timeStampMs = 0)
        {
            uint currentTimeStamp = timeStampMs <= 0 ? TimeHelper.GetUnixMsTimeStamp() : timeStampMs;

            DataTimed<T> beforeTimeStamp = GetValueAtTime(currentTimeStamp, WANT_DATA_BEFORE_TIMESTAMP);
            DataTimed<T> afterTimeStamp = GetValueAtTime(currentTimeStamp, !WANT_DATA_BEFORE_TIMESTAMP);

            if (beforeTimeStamp != null && afterTimeStamp != null) // we are between 2 value -> interpolate
            {
                //Console.WriteLine("Interpolate");
                return Interpolate(beforeTimeStamp, afterTimeStamp, currentTimeStamp);
            }
            else if (beforeTimeStamp != null) // only data before timestamp -> Extrapolate
                return Extrapolate(beforeTimeStamp, currentTimeStamp);
            else // no data found or only after the timestamp -> return null; 
                return null;
        }

        protected void SendViewPacket(object currentValue)
        {
            // Notify the view, that component value has just changed : use this only if your component isn't getting in realtime informations
            PacketUpdateViewObject feedBackPacket = new PacketUpdateViewObject();
            feedBackPacket.ObjectId = ObjectIdentifier;
            feedBackPacket.ComponentId = UniqueIdentifier;
            feedBackPacket.NewValue = currentValue;
            EventManager.Get().EnqueuePacket(0, feedBackPacket);
        }

        protected DataTimed<T> GetValueAtTime(uint timeStampMs, bool wantDataBeforeTimestamp)
        {
            List<DataTimed<T>> ordered = null;
            if (wantDataBeforeTimestamp)
            {
                if (m_dataWithTime.Count == 0)
                    return null;

                ordered = m_dataWithTime.OrderByDescending(dt => dt.TimeStampMs).ToList();
                for (int i = 0; i < ordered.Count; i++)
                {
                    if (ordered[i].TimeStampMs <= timeStampMs)
                        return ordered[i];
                }
            }
            else
            {
                if (m_dataWithTime.Count == 0)
                    return null;

                ordered = m_dataWithTime.OrderBy(dt => dt.TimeStampMs).ToList();
                for (int i = 0; i < ordered.Count; i++)
                {
                    if (ordered[i].TimeStampMs > timeStampMs)
                        return ordered[i];
                }
            }

            return null;
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

        protected abstract T Interpolate(DataTimed<T> dataBeforeTimestamp, DataTimed<T> dataAfterTimestamp, uint currentMs);
        protected abstract T Extrapolate(DataTimed<T> dataBeforeTimestamp, uint currentMs);
    }
}
