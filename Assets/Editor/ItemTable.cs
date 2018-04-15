using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class ItemTable : EditorWindow
{
    private List<ItemBlueprint> items;
    private ReorderableList reorderableList;

    [MenuItem("Window/Item Editor")]
    public static void Init()
    {
        GetWindow(typeof(ItemTable));
    }

    private void OnEnable()
    {
        items = FindAssetsByType<ItemBlueprint>();

        reorderableList = new ReorderableList(items, typeof(ItemBlueprint), true, true, true, true);
        reorderableList.elementHeight *= 3;

        // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
        // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
        // which is a UnityEngine.Object
        // reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

        // Add listeners to draw events
        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.drawElementCallback += DrawElement;

        reorderableList.onAddCallback += AddItem;
        reorderableList.onRemoveCallback += RemoveItem;
    }

    private void OnGUI()
    {
        // Actually draw the list in the inspector
        reorderableList.DoLayoutList();
    }

    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "Items List");
    }

    private void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        ItemBlueprint item = items[index];

        EditorGUI.BeginChangeCheck();

        int labelWidth = 45;

        Rect labelBox = new Rect(rect.x, rect.y, rect.width / 6, rect.height / 3);
        Rect valueBox = new Rect(rect.x + labelWidth, rect.y, rect.width / 6 - labelWidth, rect.height / 3);
        EditorGUI.LabelField(labelBox, "Name:");
        item.name = EditorGUI.TextField(valueBox, item.name);

        labelBox = new Rect(rect.x, rect.y + rect.height / 3, rect.width / 6, rect.height / 3);
        valueBox = new Rect(rect.x + labelWidth, rect.y + rect.height / 3, rect.width / 6 - labelWidth, rect.height / 3);
        EditorGUI.LabelField(labelBox, "Value:");
        item.value = EditorGUI.IntField(valueBox, item.value);
        
        valueBox = new Rect(rect.x + labelWidth, rect.y + 2 * rect.height / 3, rect.width / 6 - labelWidth, rect.height / 3);
        item.rarity = (Rarity)EditorGUI.EnumPopup(valueBox, item.rarity);

        valueBox = new Rect(rect.x + 0 * rect.width / 6, rect.y + rect.height/3, rect.width / 6, rect.height/3);
        Rect col3 = new Rect(rect.x + 2 * rect.width / 6, rect.y, rect.width / 6, rect.height/3);
        Rect col4 = new Rect(rect.x + 3 * rect.width / 6, rect.y, rect.width / 6, rect.height);
        
        item.rarity = (Rarity)EditorGUI.EnumPopup(col3, item.rarity);
        item.icon = (Sprite)EditorGUI.ObjectField(col4, item.icon, typeof(Sprite), false);
        //item.description = EditorGUI.TextField(col4, item.description);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(this);
        }
    }

    private void AddItem(ReorderableList list)
    {
        var newItem = (ItemBlueprint)ScriptableObject.CreateInstance(typeof(ItemBlueprint));
        items.Add(newItem);
        AssetDatabase.CreateAsset(newItem, "Assets/_ScriptableObjects/Items/Item" + items.Count + ".asset");
        Debug.Log(AssetDatabase.GetAssetPath(newItem));

        EditorUtility.SetDirty(this);
    }

    private void RemoveItem(ReorderableList list)
    {
        items.RemoveAt(list.index);

        EditorUtility.SetDirty(this);
    }

    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
}

