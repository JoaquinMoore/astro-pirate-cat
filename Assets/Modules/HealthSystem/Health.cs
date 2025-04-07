using System;
using UnityEngine;
using UnityEngine.Events;

namespace HealthSystem
{
    public class Health : MonoBehaviour, IHurtable, IHealable
    {
        [Serializable]
        private struct Data
        {
            [field: SerializeField, Min(0)]
            public int MaxHealth { get; private set; }
        }

        [SerializeField]
        private Data _data;

        public UnityEvent OnDie, OnHeal, OnDamage;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _data.MaxHealth);
            }
        }

        public int PublicMaxHealth => _data.MaxHealth;

        public void Hurt(int damage)
        {
            CurrentHealth -= Mathf.Abs(damage);

            if (CurrentHealth <= 0)
            {
                OnDie?.Invoke();
            }
            else
            {
                OnDamage?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            CurrentHealth += Mathf.Abs(amount);
            OnHeal?.Invoke();
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void OnValidate()
        {
            _currentHealth = Mathf.Min(_currentHealth, _data.MaxHealth);
        }

        private void Awake()
        {
            CurrentHealth = _data.MaxHealth;
        }

        private int _currentHealth;
    }
}
