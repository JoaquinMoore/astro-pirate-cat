using System;
using UnityEngine;
using UnityEngine.Events;

namespace HealthSystem
{
    public class Health : MonoBehaviour, IHurtable, IHealable
    {
        [Serializable]
        public struct Data
        {
            [field: SerializeField, Min(0)]
            public int MaxHealth { get; private set; }
        }

        [SerializeField]
        private Data _data;

        [SerializeField, Min(0)]
        private int _currentHealth;

        public UnityEvent OnDie, OnHeal, OnDamage;

        private int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _data.MaxHealth);
            }
        }


        public int PublicCurrentHealth => _currentHealth;
        public int PublicMaxHealth => _data.MaxHealth;
        /// <summary>
        /// Reduce la vida. En caso de que llegue a 0, muere.
        /// </summary>
        /// <param name="damage">Cantidad de vida a reducir.</param>
        public void Hurt(int damage)
        {
            CurrentHealth -= Mathf.Abs(damage);

            if (CurrentHealth <= 0)
            {
                Debug.Log("die");
                OnDie?.Invoke();
            }
            else
            {
                OnDamage?.Invoke();
            }
        }




        /// <summary>
        /// Aumenta la vida.
        /// </summary>
        /// <param name="amount">Cantidad de vida a aumentar.</param>
        public void Heal(int amount)
        {
            CurrentHealth += Mathf.Abs(amount);
            OnHeal?.Invoke();
        }

        private void OnValidate()
        {
            _currentHealth = Mathf.Min(_currentHealth, _data.MaxHealth);
        }

        private void Awake()
        {
            CurrentHealth = _data.MaxHealth;
        }

        public GameObject getGameObject()
        {
            return gameObject;
        }
    }
}
