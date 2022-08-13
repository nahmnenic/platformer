using UnityEngine;
using UnityEngine.Events;
using System;

namespace PixelCrew.Components
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objToDestroy;
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(collision.gameObject);
            }
        }

        public void Destroyoject()
        {
            Destroy(_objToDestroy);
        }
    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {

    }
}