using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Weapons.Projectile;
using UnityEngine;

namespace Assets.Scripts.World.Items
{
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

        private void Reset()
        {
            _renderer.sprite = States[0];
            _collider.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                _renderer.sprite = States[1];
                col.GetComponent<IDamagable>().TakeDamage(Damage);
                _collider.enabled = false;
                Invoke(nameof(Reset), 5);
            }
        }
    }
}
