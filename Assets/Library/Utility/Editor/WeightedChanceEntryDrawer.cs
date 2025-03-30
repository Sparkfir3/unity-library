using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

#if !ODIN_INSPECTOR
namespace Sparkfire.Utility.Editor
{
    [CustomPropertyDrawer(typeof(WeightedChance<>.WeightedChanceEntry))]
    public class WeightedChanceEntryDrawer : PropertyDrawer
    {
        /// <summary>
        /// Meta File GUID of WeightedChanceEntry.uxml
        /// </summary>
        private const string VISUAL_TREE_ASSET_GUID = "b768560075390e84b860429afd76e382";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();
            VisualTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(VISUAL_TREE_ASSET_GUID));
            root.Add(treeAsset.Instantiate());

            InitializePercentField(root);
            InitializeValueField(property, root);

            return root;
        }

        private void InitializePercentField(in VisualElement root)
        {
            FloatField percentField = root.Q<FloatField>("Percent");
            percentField.SetEnabled(false);
            percentField.formatString = "P2";
        }

        private void InitializeValueField(in SerializedProperty property, in VisualElement root)
        {
            FloatField baseValueField = root.Q<FloatField>("Value");
            SerializedPropertyType propertyType = property.FindPropertyRelative("value").propertyType;
            BindableElement newValueField = propertyType switch
            {
                SerializedPropertyType.Integer => new IntegerField(),
                SerializedPropertyType.Boolean => new Toggle(),
                SerializedPropertyType.Float => new FloatField(),
                SerializedPropertyType.String => new TextField(),
                SerializedPropertyType.Color => new ColorField(),
                SerializedPropertyType.ObjectReference => new ObjectField(),
                SerializedPropertyType.LayerMask => new LayerMaskField(),
                SerializedPropertyType.Enum => new EnumField(),
                SerializedPropertyType.Vector2 => new Vector2Field(),
                SerializedPropertyType.Vector3 => new Vector3Field(),
                SerializedPropertyType.Vector4 => new Vector4Field(),
                SerializedPropertyType.AnimationCurve => new CurveField(),
                SerializedPropertyType.Quaternion => new Vector3Field(),
                _ => baseValueField
            };
            newValueField.bindingPath = baseValueField.bindingPath;
            newValueField.name = baseValueField.name;
            newValueField.style.flexShrink = 1;
            newValueField.style.flexGrow = 1;
            newValueField.style.width = 100;
            newValueField.style.marginLeft = baseValueField.style.marginLeft.value.value;
            newValueField.style.marginRight = baseValueField.style.marginRight.value.value;

            VisualElement container = root.Q<VisualElement>("Container");
            container.Remove(baseValueField);
            container.Add(newValueField);
        }
    }
}
#endif
