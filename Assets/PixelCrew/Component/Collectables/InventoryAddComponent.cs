using PixelCrew.Creatures;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryIdAtributte] [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetComponent<Hero>();

            if (hero != null)
            {
                hero.AddInInventory(_id, _count);
            }
        }
    }
}