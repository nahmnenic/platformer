using PixelCrew.Creatures;
using UnityEngine;

namespace PixelCrew.Components
{
    public class LimitBulletComponent : MonoBehaviour
    {
        [SerializeField] private int _numBullet;

        private Hero _hero;

        private void Start()
        {
            _hero = GetComponent<Hero>();
        }

        public void Limit()
        {
            _hero.LimitBullet(_numBullet);
        }
    }
}
