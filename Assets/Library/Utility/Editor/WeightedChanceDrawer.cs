using UnityEditor;
using UnityEngine.UIElements;

namespace Sparkfire.Utility.Editor
{
    [CustomPropertyDrawer(typeof(WeightedChance<>))]
    public class WeightedChanceDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();
            VisualTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Library/Utility/Editor/WeightedChance.uxml");
            root.Add(treeAsset.Instantiate());

            root.Q<FloatField>("TotalWeight").SetEnabled(false);
            root.Q<Foldout>("Foldout").text = property.displayName;

            return root;
        }
    }
}
