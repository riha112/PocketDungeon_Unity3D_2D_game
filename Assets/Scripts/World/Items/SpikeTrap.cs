using Assets.Scripts.Entity;
using Assets.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
    /// <summary>
    /// Class that handles, damage giving
    /// to character on enter and changes
    /// between two sprite states.
    /// </summary>
    public class SpikeTrap : MonoBehaviour
    {
        public int Damage = 10;
        public Sprite[] States;

        private SpriteRenderer _renderer;
        private BoxCollider2D _collider;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();

            transform.localPosition -= transform.forward * 0.1f;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Can add also "Enemy" tag... maybe later when added
            // path-finding
            if (col.tag != "Player") return;

            // Sets style to be with spikes out
            _renderer.sprite = States[1];
            
            // Deals damage & disables itself, so that damage is only given
            // once per activation
            col.GetComponent<IDamagable>().TakeDamage(Damage);
            _collider.enabled = false;

            // Resets trap after 5 seconds
            Invoke(nameof(Reset), 5);
        }

        private void Reset()
        {
            _renderer.sprite = States[0];
            _collider.enabled = true;
        }

    }
}