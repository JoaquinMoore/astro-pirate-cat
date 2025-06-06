using System;
using UnityEngine;
using UnityEngine.Events;

namespace HealthSystem
{
    public class Health : MonoBehaviour, IHurtable, IHealable
    {
        [SerializeField] private Data _data;

        public UnityEvent OnDie, OnHeal, OnDamage;

        private int _currentHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0, _data.MaxHealth);
        }

        public int PublicMaxHealth => _data.MaxHealth;

        private void Awake()
        {
            CurrentHealth = _data.MaxHealth;
        }

        private void OnValidate()
        {
            _currentHealth = Mathf.Min(_currentHealth, _data.MaxHealth);
        }

        public void Heal(int amount)
        {
            CurrentHealth += Mathf.Abs(amount);
            OnHeal?.Invoke();
        }

        public void Hurt(int damage)
        {
            CurrentHealth -= Mathf.Abs(damage);

            if (CurrentHealth <= 0)
                OnDie?.Invoke();
            else
                OnDamage?.Invoke();
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        [Serializable]
        private struct Data
        {
            [field: SerializeField]
            [field: Min(0)]
            public int MaxHealth { get; private set; }
        }

        void IHurtable.Hurt(float damage)
        {
            throw new NotImplementedException();
        }
    }
}