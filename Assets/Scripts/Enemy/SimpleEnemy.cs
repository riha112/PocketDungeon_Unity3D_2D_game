using Assets.Scripts.Misc;
using Assets.Scripts.Misc.ObjectManager;
using Assets.Scripts.Repository;
using Assets.Scripts.Repository.Data;
using Assets.Scripts.User.Attributes;
using Assets.Scripts.User.Party;
using Assets.Scripts.Weapons.Projectile;
using Assets.Scripts.World;
using Assets.Scripts.World.Items;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class SimpleEnemy : Entity.Entity, IPartyMember
    {
        #region Info
        public int EnemyDataId;
        protected EnemyData Info { get; set; }
        #endregion

        #region Config
        private static readonly Color DamageColor = Color.black;
        public GameObject AttackEffect;
        public GameObject[] EffectRepository;
        #endregion

        #region Components
        protected Animator Animator;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        #endregion

        #region Target
        private Transform _target = null;
        public Transform Target
        {
            get => _target;
            set
            {
                _target = value;
                TargetData = Target.GetComponent<IDamagable>();
            }
        }
        protected IDamagable TargetData;
        #endregion

        private int ExpDrop => Info.ExpToDrop + Info.ExpMultiplierByLevel * Stats.CurrentLevel;

        private Color _baseColor;
        protected float _currentAttackCooldownTimer;
        private bool _wasAttacked;
        private bool _wasInRange;
        protected bool IsInAttackRange;

        #region IPartyMember parameters
        public float HealthMlt => Health / Stats.MaxHealth;
        public float MagicMlt => Magic / Stats.MaxMagic;
        public string Name => Info.Title;
        public Texture2D Design => Info.Icon;
        #endregion

        /// <summary>
        /// Called when Entity is active. After Awake event.
        /// </summary>
        protected virtual void Start()
        {
            SetComponents();
            Stats.CurrentLevel = GameBalancer.GetBalancedMonsterLevel();
            UpdateMyAttributes();
            SetBaseValues();
        }

        /// <summary>
        /// Updates enemies attributes, by taking base attribute values and
        /// adding level based attributes, thus updating stats to correct value.
        /// </summary>
        protected virtual void UpdateMyAttributes()
        {
            // Used MoveData to keep event subscription and 
            // Copy to not-modify base values within Info.
            AttributeData.MoveData(
                Attributes,
                AttributeData.Copy(Info.BaseAttributes) + AttributeData.Copy(Info.AttributesMultiplierByLevel) * Stats.CurrentLevel
            );
        }

        /// <summary>
        /// Loads parameters
        /// </summary>
        protected virtual void SetComponents()
        {
            Info = EnemyRepository.Repository[EnemyDataId];

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _baseColor = _spriteRenderer.color;

            _rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();

            if (Target is null)
            {
                Target = Util.GetCharacterTransform();
            }

            if (!(Target is null))
            {
                TargetData = Target.GetComponent<IDamagable>();
            }
        }

        protected virtual void SetBaseValues()
        {
            Health = Stats.MaxHealth;
            Magic = Stats.MaxMagic;
        }

        /// <summary>
        /// Changes sprites X axis towards target, to simulate look at my
        /// direction effect
        /// </summary>
        /// <param name="target">To whom should enemy look at</param>
        protected virtual void FlipSpriteDirectionTowards(Transform target)
        {
            _spriteRenderer.flipX = target.transform.position.x < transform.position.x;
        }

        /// <summary>
        /// Attack target when within specific range & cooldown timer is bellow 0
        /// </summary>
        protected virtual void Attack()
        {
            // Updates cooldown timer if still in "cooldown" state
            // then do nothing
            _currentAttackCooldownTimer -= Time.deltaTime;
            if (_currentAttackCooldownTimer > 0) return;

            // Resets cooldown timer
            _currentAttackCooldownTimer = Info.BaseCoolDownForAttack;

            // Loads effect
            InitAttackEffect();

            // Attacks target
            TargetData.TakeDamage(Stats.HitPoints);

            Animator?.SetBool("isWalking", false);
        }

        /// <summary>
        /// Loads effect for attacking
        /// </summary>
        protected virtual void InitAttackEffect()
        {
            if(AttackEffect == null) return;

            var attackEffect = Instantiate(AttackEffect);
            attackEffect.transform.position = transform.position;
            attackEffect.transform.right = Util.LookAt2D(transform, Target);
            Destroy(attackEffect, 0.5f);
        }

        /// <summary>
        /// Called when target is out of range
        /// </summary>
        protected virtual void Idle()
        {
            Animator?.SetBool("isWalking", false);
        }

        /// <summary>
        /// Simple movement straight towards target... not good for
        /// enemies with solid collider. (Used for simple enemies or flying ones)
        /// 
        /// For better movement implementation check <see cref="BetterMovementSimpleEnemy"/>
        /// </summary>
        protected virtual void WalkTowardsTarget()
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.position, Stats.CurrentSpeed * Time.deltaTime);
            Animator?.SetBool("isWalking", true);
        }

        /// <summary>
        /// Called per frame.
        /// Switches between enemies main states: Idle, Walk, Attack
        /// </summary>
        protected void Update()
        {
            // If there is no target, then do nothing
            // TODO: Implement wander mechanic
            if (Target == null)
                return;

            // Calculates distance between target at itself
            var distance = Vector2.Distance(Target.position, transform.position);

            // State changing machine :)
            if (IsIdle(distance))
            {
                Idle();
            }
            else
            {
                _wasInRange = true;
                FlipSpriteDirectionTowards(Target);
                if (distance < Info.AttackDistance)
                {
                    Attack();
                    IsInAttackRange = true;
                }
                else
                {
                    WalkTowardsTarget();
                    IsInAttackRange = false; 
                }
            }
        }

        /// <summary>
        /// Determines whether or not enemy should be in "Idle" state,
        /// by taking distance and base config into account.
        /// </summary>
        /// <param name="distance">Distance between target and itself</param>
        /// <returns>Should enemy be in Idle state</returns>
        public virtual bool IsIdle(float distance)
        {
            // If was attacked and KeepAggro is enabled
            if (Info.KeepAggroIfAttacked && _wasAttacked)
                return false;

            // If was in range and RememberTarget is enabled
            if (Info.KeepAggroAfterFirstInRange && _wasInRange)
                return false;

            return distance > Info.AggroRange;
        }

        /// <inheritdoc cref="Entity"/>
        public override int TakeDamage(int damage)
        {
            var realDamage = base.TakeDamage(damage);

            // Used for enemies with Info.KeepAggro...
            _wasAttacked = true;

            // Loads damage effect
            HitEffect(realDamage);

            return realDamage;
        }

        /// <summary>
        /// Instantiates damage effects
        /// </summary>
        /// <param name="damage">Real damage amount, for indicator</param>
        protected virtual void HitEffect(int damage)
        {
            if(Target == null)
                return;

            _spriteRenderer.color = DamageColor;

            // Damage (Explosion) effect
            var damageEffect = Instantiate(EffectRepository[0]);
            damageEffect.transform.position = transform.position;
            damageEffect.transform.Rotate(Vector3.forward * Random.Range(0, 360));
            Destroy(damageEffect, 1);

            // Damage (Indicator) effect
            DamageIndicator.LoadDamageIndicator(transform, damage);

            // Push away from target
            var direction = Util.LookAt2D(Target, transform);
            _rigidbody?.AddForce(-direction * 100);

            // Reset effect
            Invoke(nameof(AfterHitEffect), 0.15f);
        }

        /// <summary>
        /// Restores color to base, for hit effect
        /// </summary>
        protected virtual void AfterHitEffect()
        {
            _spriteRenderer.color = _baseColor;
        }

        /// <inheritdoc cref="Entity"/>
        protected override void Death()
        {
            InitializeExpPoint();

            // Summoned entities requires to be removed from party list
            // to hide GUI entries.
            DI.Fetch<PartyController>()?.PartyMembers.Remove(this);

            Destroy(gameObject);
        }

        /// <summary>
        /// Loads EXP points at specific point
        /// </summary>
        /// <param name="position">Where to load EXP points</param>
        protected virtual void InitializeExpPoint(Transform position = null)
        {
            // If EXP points to give is 0, then do not load the prefab
            if (ExpDrop <= 0)
                return;

            var expOrb = PrefabRepository.Repository[13].Prefab;
            var expOrbGo = Instantiate(expOrb);
            expOrbGo.transform.position = position == null ? transform.position : position.position;
            expOrbGo.GetComponent<ExperiencePoint>().ExpToAdd = ExpDrop;
        }
    }
}
