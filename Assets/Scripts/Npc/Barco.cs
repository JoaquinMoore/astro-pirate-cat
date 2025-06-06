using HealthSystem;
using UnityEngine;

namespace Npc
{
    public class Barco : MonoBehaviour, IHurtable
    {
        void IHurtable.Hurt(float damage)
        {
            Debug.Log("Me lastimaron" + damage);
        }
    }
}