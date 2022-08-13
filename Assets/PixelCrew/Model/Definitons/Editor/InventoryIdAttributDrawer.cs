using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Model
{
    [CustomPropertyDrawer(typeof(InventoryIdAtributte))]
    public class InventoryIdAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defs = DefFacade.I.Items.ItemsForEditor;
            var ids = new List<string>();

            foreach (var itemDefs in defs)
            {
                ids.Add(itemDefs.Id);
            }

            var index = Mathf.Max(ids.IndexOf(property.stringValue), 0);

            index = Mathf.Max(EditorGUI.Popup(position, property.displayName, index, ids.ToArray()), 0);
            property.stringValue = ids[index];
        }
    }
}
