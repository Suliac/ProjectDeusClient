using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class ExecutableObjectsHolder<T> : IExecutable where T : IIdentifiable, IExecutable
    {
        protected List<T> m_holdedObjects;

        public ExecutableObjectsHolder(ICollection<T> objects = null)
        {
            m_holdedObjects = new List<T>();
            if (objects != null)
                m_holdedObjects.AddRange(objects);

        }

        protected bool AddObject(T newObject)
        {
            if (!m_holdedObjects.Any(obj => obj.UniqueIdentifier == newObject.UniqueIdentifier))
            {
                newObject.Start();
                m_holdedObjects.Add(newObject);
                return true;
            }

            return false;
        }

        protected bool RemoveObject(uint objectToDeleteId)
        {
            if (m_holdedObjects.Any(d => d.UniqueIdentifier == objectToDeleteId))
            {
                m_holdedObjects.FirstOrDefault(d => d.UniqueIdentifier == objectToDeleteId).Stop(); // stop gameobject
                m_holdedObjects.Remove(m_holdedObjects.FirstOrDefault(d => d.UniqueIdentifier == objectToDeleteId)); // delete
                return true;
            }

            return false;
        }

        public void Update(decimal deltatimeMs)
        {
            // we execute children specific behavior
            OnUpdate(deltatimeMs);

            // then we call the methods for the objects holded
            foreach (var holdedObject in m_holdedObjects)
                holdedObject.Update(deltatimeMs);
        }

        public void Stop()
        {
            // we execute children specific behavior
            OnStop();

            // then we call the methods for the objects holded
            foreach (var holdedObject in m_holdedObjects)
                holdedObject.Stop();
        }

        public void Start()
        {
            // we execute children specific behavior
            OnStart();

            // then we call the methods for the objects holded
            foreach (var holdedObject in m_holdedObjects)
                holdedObject.Start();
        }

        protected virtual void OnUpdate(decimal deltatimeMs)
        {
            // We let the children classes decide if they want to implement OnUpdate or not
        }

        protected virtual void OnStart()
        {
            // We let the children classes decide if they want to implement OnStart or not
        }

        protected virtual void OnStop()
        {
            // We let the children classes decide if they want to implement OnStop or not
        }
    }
}
