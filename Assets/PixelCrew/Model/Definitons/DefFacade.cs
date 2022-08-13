using UnityEngine;

namespace PixelCrew.Model
{
    [CreateAssetMenu(menuName = "Defs/DefFacade", fileName = "DefFacade")]
    public class DefFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemDef _items;
        public InventoryItemDef Items => _items;

        private static DefFacade _instance;
        public static DefFacade I => _instance == null ? LoadDef() : _instance;

        public static DefFacade LoadDef()
        {
            return _instance = Resources.Load<DefFacade>("DefFacade");
        }
    }
}
