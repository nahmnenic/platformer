using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class ShootingAi : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [Header("Range")]
        [SerializeField] private CoolDown _rangeCoolDown;
        [SerializeField] private SpawnComponent _rangeAttack;

        private Animator _animator;
        private static readonly int Attack = Animator.StringToHash("attack");
        private static readonly int die = Animator.StringToHash("Die");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_rangeCoolDown.IsReady)
                    RangeAttack();
            }
        }
        private void RangeAttack()
        {
            _rangeCoolDown.Reset();
            _animator.SetTrigger(Attack);
        }

        public void OnDie()
        {
            _animator.SetBool(die, true);
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}
