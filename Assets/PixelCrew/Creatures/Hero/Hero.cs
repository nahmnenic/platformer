using PixelCrew.Components;
using PixelCrew.model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        [SerializeField] private float _slamDownVelocity = 8f;
        [SerializeField] private LayerCheck _wallCheck;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

        private bool _allowDoubleJump; 
        private bool _isOnWall;

        private float _defultGravityScale;

        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [SerializeField] private ParticleSystem _coinHitParticles;

        [SerializeField] private CoolDown _throwCoolDown;

        [SerializeField] private LimitBulletComponent _bullet;
        [SerializeField] private AnimatorController _disarmed;
        [SerializeField] private AnimatorController _armed;

        private GameSession _session;

        private int SwordCount => _session.Data.Inventory.Count("Sword");
        private int CoinsCount => _session.Data.Inventory.Count("Coins");

        protected override void Awake()
        {
            base.Awake();

            _defultGravityScale = _rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.Data.Inventory.OnChanged += PrintHandlerDebug;


            health.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
            _session.Data.Inventory.OnChanged -= PrintHandlerDebug;
        }

        private void PrintHandlerDebug(string id, int value)
        {
            Debug.Log($"Inventory changed: {id}: {value}");
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();

            var moveToSameDirection = _direction.x * transform.lossyScale.x > 0;
            if(_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                _rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                _rigidbody.gravityScale = _defultGravityScale;
            }

            _animator.SetBool(IsOnWall, _isOnWall);
        }

        public void DamageSlow()
        {
            _speed = _speed * 0.5f;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }


        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!_isGrounded && _allowDoubleJump && !_isOnWall)
            {
                _allowDoubleJump = false;
                DoJumpVfx();
                return _jumpPower;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }
        protected override float CalculateYVelocity()
        {
            if (_isGrounded)
            {
                _allowDoubleJump = true;
            }

            return base.CalculateYVelocity();
        }
        public void SaySomething()
        {
            Debug.Log("Hello Pixel Crew!");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public void LimitBullet(int bullet)
        {
            _session.Data.Bullet -= bullet;
        }

        private void SpawnCoins()
        {
            var numCoinsToSpawn = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToSpawn);

            var burst = _coinHitParticles.emission.GetBurst(0);
            burst.count = numCoinsToSpawn;
            _coinHitParticles.emission.SetBurst(0, burst);

            _coinHitParticles.gameObject.SetActive(true);
            _coinHitParticles.Play();
        }

        public override void AttackAnimation()
        {
            if (SwordCount <= 0) return;

            base.AttackAnimation();
        }

        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        public void Throw()
        {
            if(_throwCoolDown.IsReady && _session.Data.Bullet > 1)
            {
                _animator.SetTrigger(ThrowKey);
                _throwCoolDown.Reset();
            }
        }

        public void OnDoThrow()
        {
            _sounds.Play("Range");
            _particles.Spawn("Throw");
            _bullet.Limit();
        }
    }
}