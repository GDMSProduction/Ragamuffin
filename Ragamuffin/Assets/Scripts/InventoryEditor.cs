using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR

using UnityEditor;
[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor {

    private SerializedProperty itemImagesProperty;
    private SerializedProperty itemProperty;
    private SerializedProperty posprotery;



    private bool[] showItemSlots = new bool[Inventory.numItemSlots];
    private const string inventoryPropItemImagesName = "itemImages";
    private const string inventoryPropItemsName = "items";
    private const string pos2spawn = "pos";
    public void OnEnable()
    {
        itemImagesProperty = serializedObject.FindProperty(inventoryPropItemImagesName);
        itemProperty = serializedObject.FindProperty(inventoryPropItemsName);
        posprotery= serializedObject.FindProperty(pos2spawn);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        for (int i = 0; i < Inventory.numItemSlots; i++)
        {
            ItemSlotGUI(i);
        }
        serializedObject.ApplyModifiedProperties();
    }
    private void ItemSlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
     

     


     
        EditorGUI.indentLevel++;
        
        showItemSlots[index] = EditorGUILayout.Foldout(showItemSlots[index], "Item slot " + index);
        if (showItemSlots[index])
        {
            EditorGUILayout.PropertyField(itemImagesProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(itemProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(posprotery.GetArrayElementAtIndex(index));


        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
#endif