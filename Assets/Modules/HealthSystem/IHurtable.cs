using UnityEngine;

namespace HealthSystem
{
    public interface IHurtable
    {
        void Hurt(int damage);
        public GameObject getGameObject();
    }
}
