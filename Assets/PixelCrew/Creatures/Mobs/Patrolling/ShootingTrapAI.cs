using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [Header("Melle")]
        [SerializeField] private CoolDown _meleeCoolDown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private CoolDown _rangeCoolDown;
        [SerializeField] private SpawnComponent _rangeAttack;

        private Animator _animator;
        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(_vision.IsTouchingLayer)
            {
                if(_meleeCanAttack.IsTouchingLayer)
                {
                    if(_meleeCoolDown.IsReady)
                        MeleeAttack();
                    return;
                }

                if(_rangeCoolDown.IsReady)
                    RangeAttack();
            }
        }

        private void MeleeAttack()
        {
            _meleeCoolDown.Reset();
            _animator.SetTrigger(Melee);
        }

        private void RangeAttack()
        {
            _rangeCoolDown.Reset();
            _animator.SetTrigger(Range);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}

