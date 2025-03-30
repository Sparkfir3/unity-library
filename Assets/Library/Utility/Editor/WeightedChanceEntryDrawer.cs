using UnityEditor;
using UnityEngine.UIElements;

namespace Sparkfire.Utility.Editor
{
    [CustomPropertyDrawer(typeof(WeightedChanceEntry<>))]
    public class WeightedChanceEntryDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();
            VisualTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Library/Utility/Editor/WeightedChanceEntry.uxml");
            root.Add(treeAsset.Instantiate());

            root.Q<FloatField>("Percent").SetEnabled(false);
            root.Q<FloatField>("Percent").formatString = "P2";

            return root;
        }
    }
}
