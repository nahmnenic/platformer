using PixelCrew.Creatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class GrowthComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 scaleChange;

        private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void Growth()
        {
            _hero.Growth(scaleChange);
        }
    }
}

