using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destination;

        public void Teleport(GameObject target)
        {
            target.transform.position = _destination.position;
        }
    }
}