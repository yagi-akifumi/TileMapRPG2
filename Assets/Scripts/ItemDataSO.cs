using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO")]
public class ItemDataSO : ScriptableObject
{

    /// <summary>
    /// アイテムのデータ
    /// </summary>
    [Serializable]
    public class ItemData
    {
        public int itemNo;             // アイテムの通し番号
        public Sprite itemSprite;      // アイテムの Image 画像
        public ItemName itemName;      // アイテムの名前
        public ItemType itemType;      // アイテムの種類
        public string itemInfo;        // アイテムの情報

        // TODO 必要な情報を追加する

    }

    public List<ItemData> itemDataList = new List<ItemData>();
}
