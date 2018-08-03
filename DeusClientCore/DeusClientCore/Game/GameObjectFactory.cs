using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeusClientCore.Components;

namespace DeusClientCore
{
    public enum EObjectType
    {
        Player
    }

    public class GameObjectFactory
    {
        private static uint m_nextId = 1;

        public GameObject CreateGameObject()
        {
            GameObject gameObject = new GameObject(m_nextId);
            m_nextId++;

            var healthComponent = new HealthTimeLineComponent();
            gameObject.AddComponent(healthComponent);

            return gameObject;
        }
    }
}
