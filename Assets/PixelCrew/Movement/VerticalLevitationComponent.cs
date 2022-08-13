using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Movement
{
    public class VerticalLevitationComponent : MonoBehaviour
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _amplitude = 1f;
        [SerializeField] private bool _randomize;

        private float _originalY;
        private Rigidbody2D _rigibody;
        private float _seed;

        private void Awake()
        {
            _rigibody = GetComponent<Rigidbody2D>();
            _originalY = _rigibody.position.y;
            if (_randomize)
                _seed = Random.value * Mathf.PI * 2;
        }

        private void Update()
        {
            var pos = _rigibody.position;
            pos.y = _originalY + Mathf.Sin(_seed + Time.time * _frequency) * _amplitude;
            _rigibody.MovePosition(pos);
        }
    }

}
