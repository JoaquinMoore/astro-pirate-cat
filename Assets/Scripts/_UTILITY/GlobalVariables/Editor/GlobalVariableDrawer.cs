using Npc;
using Physics.Movement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _UTILITY.GlobalVariables.Editor
{
    public class GlobalVariableDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Contenedor horizontal: label + valor
            var propertyField = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row
                }
            };

            var label = new Label(property.displayName)
            {
                style =
                {
                    flexShrink = 1
                }
            };
            label.AddToClassList("unity-base-field__label");
            label.AddToClassList("unity-base-text-field__label");
            label.AddToClassList("unity-base-field__label--with-dragger");
            label.AddToClassList("unity-property-field__label");
            label.AddToClassList("unity-float-field__label");

            var fieldContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1,
                    flexShrink = 1
                }
            };


            var useConstantProp = property.FindPropertyRelative("useConstant");
            var constantValueProp = property.FindPropertyRelative("constantValue");
            var variableValueProp = property.FindPropertyRelative("variableValue");

            var valueField = new PropertyField
            {
                label = "",
                style =
                {
                    flexGrow = 1
                }
            };

            valueField.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                valueField.MarkDirtyRepaint();
            });

            Button dropdownButton = null;
            dropdownButton = new Button(() =>
            {
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("Constant"), useConstantProp.boolValue, () =>
                {
                    useConstantProp.boolValue = true;
                    property.serializedObject.ApplyModifiedProperties();
                    UpdateValueField();
                });

                menu.AddItem(new GUIContent("Variable"), !useConstantProp.boolValue, () =>
                {
                    useConstantProp.boolValue = false;
                    property.serializedObject.ApplyModifiedProperties();
                    UpdateValueField();
                });

                menu.DropDown(dropdownButton.worldBound);
            })
            {
                style =
                {
                    width = 24,
                    height = 24,
                    marginRight = 20
                },
                iconImage = EditorGUIUtility.IconContent("d_icon dropdown@2x").image as Texture2D
            };

            UpdateValueField();

            fieldContainer.Add(dropdownButton);
            fieldContainer.Add(valueField);

            propertyField.Add(label);
            propertyField.Add(fieldContainer);

            return propertyField;

            void UpdateValueField()
            {
                valueField.Unbind();
                valueField.bindingPath = useConstantProp.boolValue
                    ? constantValueProp.propertyPath
                    : variableValueProp.propertyPath;
                valueField.Bind(property.serializedObject);
            }
        }
    }

    [CustomPropertyDrawer(typeof(GlobalVariable<SteeringMovement.Data>))]
    public class MyIntVariableDrawer : GlobalVariableDrawer { }

    [CustomPropertyDrawer(typeof(GlobalVariable<NPCController.Data>))]
    public class MyFloatVariableDrawer : GlobalVariableDrawer { }
}