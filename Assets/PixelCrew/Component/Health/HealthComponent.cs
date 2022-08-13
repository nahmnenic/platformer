using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHealth;
        [SerializeField] private HealthChangeEvent _onChange;

        public void ModifyHealth(int healthDelta)
        {
            if (_health <= 0) return;

            _health += healthDelta;

            _onChange?.Invoke(_health);

            if (healthDelta < 0)
            {
                _onDamage?.Invoke();
            }

            if (healthDelta > 0)
            {
                _onHealth?.Invoke();
            }

            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }
        public void SetHealth(int health)
        {
            _health = health;
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
}