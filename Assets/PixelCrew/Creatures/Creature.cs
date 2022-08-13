using PixelCrew.Components;
using UnityEngine;


namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] public float _speed = 1f;
        [SerializeField] protected float _jumpPower = 1f;
        [SerializeField] private float _damageJumpPower = 1.5f;
        //[SerializeField] private int _damage = 1;

        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;

        [Header("Particles")]
        [SerializeField] protected SpawnListComponent _particles;

        protected Rigidbody2D _rigidbody;
        protected Animator _animator;
        protected PlaySoundsComponent _sounds;
        protected Vector2 _direction;
        private Vector3 scaleBuffer;
        protected bool _isGrounded;
        private bool _isJumping;
        public GrowthComponent growth;

        private static readonly int isGroundKey = Animator.StringToHash("is-ground");
        private static readonly int isRunningKey = Animator.StringToHash("is-running");
        private static readonly int verticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int hitKey = Animator.StringToHash("hit");
        private static readonly int deadHitKey = Animator.StringToHash("dead-hit");
        private static readonly int attackKey = Animator.StringToHash("attack");


        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            scaleBuffer = transform.localScale;
            growth = GetComponent<GrowthComponent>();
            _sounds = GetComponent<PlaySoundsComponent>();
        }

        protected virtual void Update()
        {
            _isGrounded = _groundCheck.IsTouchingLayer;
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();

            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetBool(isRunningKey, _direction.x != 0);
            _animator.SetBool(isGroundKey, _isGrounded);
            _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);

            UpdateSpriteDirection(_direction);
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void Growth(Vector3 newScale)
        {
            scaleBuffer = newScale;
            transform.localScale = scaleBuffer;
        }
        public void UpdateSpriteDirection(Vector2 _direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (_direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier * scaleBuffer.x, scaleBuffer.y, 1);
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier * scaleBuffer.x, scaleBuffer.y, 1);
            }
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (_isGrounded)
            {
                yVelocity += _jumpPower;
                DoJumpVfx();
            }
            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            _particles.Spawn("Jump");
            _sounds.Play("Jump");
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _isJumping = false;
            }
            if (isJumpPressing)
            {
                _isJumping = true;
                var isFalling = _rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        public virtual void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(hitKey);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpPower);
        }


        public void DeadHit()
        {
            _animator.SetTrigger(deadHitKey);
        }

        public virtual void AttackAnimation()
        {
            _animator.SetTrigger(attackKey);
            _sounds.Play("Melee");
        }

        public void OnAttackRange()
        {
            _attackRange.Check();
        }
    }
}
