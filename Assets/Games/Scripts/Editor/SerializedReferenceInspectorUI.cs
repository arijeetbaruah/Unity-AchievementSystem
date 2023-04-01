using System;
using UnityEditor;
using UnityEngine;

public static class SerializedReferenceInspectorUI
{
    public static void ShowContextMenuForManagedReferenceOnMouseMiddleButton(this SerializedProperty property, Rect position)
    {
        var e = Event.current;
        if (e.type != EventType.MouseDown || !position.Contains(e.mousePosition) || e.button != 2)
            return;

        ShowContextMenuForManagedReference(property);
    }

    /// Must be drawn before DefaultProperty in order to receive input
    public static void DrawSelectionButtonForManagedReference(this SerializedProperty property, Rect position)
    {
        var backgroundColor = new Color(0f, 0.8f, 0.15f, 0.67f);

        var buttonPosition = position;
        buttonPosition.x += EditorGUIUtility.labelWidth + 1 * EditorGUIUtility.standardVerticalSpacing;
        buttonPosition.width = position.width - EditorGUIUtility.labelWidth - 1 * EditorGUIUtility.standardVerticalSpacing;
        buttonPosition.height = EditorGUIUtility.singleLineHeight;

        var storedIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        var storedColor = GUI.backgroundColor;
        GUI.backgroundColor = backgroundColor;

        var names = GetSplitNamesFromTypename(property.managedReferenceFullTypename);
        var className = string.IsNullOrEmpty(names.ClassName) ? "Null (Assign)" : names.ClassName;
        var assemblyName = names.AssemblyName;
        if (GUI.Button(buttonPosition, new GUIContent(className, className + "  ( " + assemblyName + " )")))
        {
            property.ShowContextMenuForManagedReference();
        }

        GUI.backgroundColor = storedColor;
        EditorGUI.indentLevel = storedIndent;
    }
    public static void ShowContextMenuForManagedReference(this SerializedProperty property)
    {
        var context = new GenericMenu();
        FillContextMenu();
        context.ShowAsContext();

        void FillContextMenu()
        {
            context.AddItem(new GUIContent("Null"), false, MakeNull);
            var realPropertyType = GetRealTypeFromTypename(property.managedReferenceFieldTypename);
            if (realPropertyType == null)
            {
                Debug.LogError("Can not get type from");
                return;
            }

            var types = TypeCache.GetTypesDerivedFrom(realPropertyType);
            foreach (var currentType in types)
            {
                AddContextMenu(currentType);
            }

            void MakeNull()
            {
                property.serializedObject.Update();
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo(); // undo is bugged
            }

            void AddContextMenu(Type type)
            {
                var assemblyName = type.Assembly.ToString().Split('(', ',')[0];
                var entryName = type + "  ( " + assemblyName + " )";
                context.AddItem(new GUIContent(entryName), false, AssignNewInstanceOfType, type);
            }

            void AssignNewInstanceOfType(object typeAsObject)
            {
                var type = (Type)typeAsObject;
                var instance = Activator.CreateInstance(type);
                property.serializedObject.Update();
                property.managedReferenceValue = instance;
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo(); // undo is bugged
            }
        }
    }

    public static Type GetRealTypeFromTypename(string stringType)
    {
        var names = GetSplitNamesFromTypename(stringType);
        var realType = Type.GetType($"{names.ClassName}, {names.AssemblyName}");
        return realType;
    }
    public static (string AssemblyName, string ClassName) GetSplitNamesFromTypename(string typename)
    {
        if (string.IsNullOrEmpty(typename))
            return ("", "");

        var typeSplitString = typename.Split(char.Parse(" "));
        var typeClassName = typeSplitString[1];
        var typeAssemblyName = typeSplitString[0];
        return (typeAssemblyName, typeClassName);
    }
}
