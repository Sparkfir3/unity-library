using UnityEditor;
using UnityEngine.UIElements;

#if !ODIN_INSPECTOR
namespace Sparkfire.Utility.Editor
{
    [CustomPropertyDrawer(typeof(WeightedChance<>))]
    public class WeightedChanceDrawer : PropertyDrawer
    {
        /// <summary>
        /// Meta File GUID of WeightedChance.uxml
        /// </summary>
        private const string VISUAL_TREE_ASSET_GUID = "dc7f7db9701d70441974fd371e9b490c";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();
            VisualTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(VISUAL_TREE_ASSET_GUID));
            root.Add(treeAsset.Instantiate());

            root.Q<FloatField>("TotalWeight").SetEnabled(false);
            root.Q<Foldout>("Foldout").text = property.displayName;

            return root;
        }
    }
}
#endif
