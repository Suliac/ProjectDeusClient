using DeusClientCore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public abstract class GamePart : IExecutable
    {
        private List<DeusObject> m_deusObjects;

        public GamePart()
        {
            m_deusObjects = new List<DeusObject>();
        }
        
        public void Update(decimal deltatimeMs)
        {
            foreach (var deusObject in m_deusObjects)
                deusObject.Update(deltatimeMs);
        }
        
        protected bool AddObject(DeusObject newObject)
        {
            if(!m_deusObjects.Any(d => d.UniqueIdentifier == newObject.UniqueIdentifier))
            {
                m_deusObjects.Add(newObject);
                return true;
            }

            return false;
        }

        protected bool RemoveObject(uint objectToDeleteId)
        {
            if (m_deusObjects.Any(d => d.UniqueIdentifier == objectToDeleteId))
            {
                DeusObject toDelete = m_deusObjects.FirstOrDefault(d => d.UniqueIdentifier == objectToDeleteId);
                m_deusObjects.Remove(toDelete);
                return true;
            }

            return false;
        }

        public abstract void Start();

        public abstract void Stop();

        protected abstract void ManagePacket(object sender, SocketPacketEventArgs e);

        protected virtual void OnUpdate(decimal deltatimeMs)
        {
            
        }
    }
}
