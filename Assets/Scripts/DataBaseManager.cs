using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    [SerializeField]
    private EventDataSO eventDataSO;    //スクリプタブル・オブジェクトを登録して管理するための変数を EventDataSO 型で宣言

    [SerializeField]
    private ItemDataSO itemDataSO;      //スクリプタブル・オブジェクトを登録して管理するための変数を ItemDataSO 型で宣言

    // ItemType ごとに分類した List（各アイテムの詳細がわかる）
    [SerializeField]
    private List<ItemDataSO.ItemData> BukiItemDatasList = new List<ItemDataSO.ItemData>();
    [SerializeField]
    private List<ItemDataSO.ItemData> BouguItemDatasList = new List<ItemDataSO.ItemData>();
    [SerializeField]
    private List<ItemDataSO.ItemData> DouguItemDatasList = new List<ItemDataSO.ItemData>();
    [SerializeField]
    private List<ItemDataSO.ItemData> DaijinamonoItemDatasList = new List<ItemDataSO.ItemData>();

    // ItemType ごとに分類したアイテムの名前の List（各アイテムの名前のみがわかる）
    [SerializeField]
    private List<string> BukiItemNamesList = new List<string>();　//　<=　ItemType が Equip である文字列だけを管理する List 
    [SerializeField]
    private List<string> BouguItemNamesList = new List<string>();
    [SerializeField]
    private List<string> DouguItemNamesList = new List<string>();
    [SerializeField]
    private List<string> DaijinamonoItemNamesList = new List<string>();

    void Awake()
    {
        //instance 変数が null (空っぽ) である場合には、DataBaseManager クラス(this)を代入する。
        if (instance == null)
        {
            instance = this;
            //【DontDestroyOnLoad】シーン遷移をしても破壊されてないゲームオブジェクト
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //２つ目以降の複数の DataBaseManager クラスが存在する(nullではない)場合は、
            //その DataBaseManager クラスのゲームオブジェクトを破壊する。
            Destroy(gameObject);
        }
        // アイテムの種類別の List を作成
        CreateItemTypeList();

        // アイテムの名前をアイテムの種類ごとに分類して List を作成
        CreateItemNamesListFromItemData();

    }

    /// <summary>
    /// NPC 用のデータから EventData を取得
    /// </summary>
    /// <param name="npcEvent"></param>
    /// <returns></returns>
    public EventData GetEventDataFromNPCEvent(int npcEventNo)
    {

        // EventDataSO スクリプタブル・オブジェクトの eventDatasList の中身(EventData)を１つずつ順番に取り出して、eventData 変数に代入
        // 【foreach】繰り返しループ処理を行う。：foreach(型名 オブジェクト名 in コレクション)
        foreach (EventData eventData in eventDataSO.eventDatasList)
        {
            // eventData の情報を判定し、EventType が Talk かつ、引数で取得している npcEventNo と同じ場合
            if (eventData.eventType == EventData.EventType.Talk && eventData.no == npcEventNo)
            {
                // 該当する EventData であると判定し、その情報を戻す
                return eventData;
            }
        }
        // 上記の処理の結果、該当する EvenData の情報がスクリプタブル・オブジェクト内にない場合には、null を戻す
        return null;
    }

    /// <summary>
    /// EventDataSO から指定した EventType と EventNo の EventData を照合して取得
    /// </summary>
    /// <param name="searchEventType"></param>
    /// <param name="searchEventNo"></param>
    /// <returns></returns>
    public EventData GetEventDataFromEvnetTypeAndEventNo(EventData.EventType searchEventType, int searchEventNo)
    {

        foreach (EventData eventData in eventDataSO.eventDatasList)
        {
            if (eventData.eventType == searchEventType && eventData.no == searchEventNo)
            {
                return eventData;
            }
        }
        return null;
    }

    /// <summary>
    /// ItemNo から ItemData を取得
    /// </summary>
    /// <param name="itemNo"></param>
    /// <returns></returns>
    public ItemDataSO.ItemData GetItemDataFromItemNo(int itemNo)
    {
        foreach (ItemDataSO.ItemData itemData in itemDataSO.itemDataList)
        {
            if (itemData.itemNo == itemNo)
            {
                return itemData;
            }
        }
        return null;
    }

    /// <summary>
    /// ItemName から ItemData を取得
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public ItemDataSO.ItemData GetItemDataFromItemName(ItemName itemName)
    {
        return itemDataSO.itemDataList.FirstOrDefault(x => x.itemName == itemName);
    }

    /// <summary>
    /// ItemData の 最大要素数を取得
    /// </summary>
    /// <returns></returns>
    public int GetItemDataSOCount()
    {
        return itemDataSO.itemDataList.Count;
    }

    /// <summary>


    /// <summary>
    /// アイテムの種類別の List を作成
    /// </summary>
    private void CreateItemTypeList()
    {

        // ItemDataSO スクリプタブル・オブジェクト内の ItemData の情報を１つずつ順番に取り出して itemData 変数に代入
        foreach (ItemDataSO.ItemData itemData in itemDataSO.itemDataList)
        {

            // 現在取り出している itemData 変数の ItemType の値がどの case と合致するかを判定
            switch (itemData.itemType)
            {

                // itemData.itemType == ItemType.Buki の場合には、BukiItemDatasList 変数に itemData 変数の値を追加する
                case ItemType.Buki:
                    BukiItemDatasList.Add(itemData);
                    break;

                case ItemType.Bougu:
                    BouguItemDatasList.Add(itemData);
                    break;

                case ItemType.Dougu:
                    DouguItemDatasList.Add(itemData);
                    break;

                case ItemType.Daijinamono:
                    DaijinamonoItemDatasList.Add(itemData);
                    break;
            }
        }
    }

    /// <summary>
    /// アイテムの名前をアイテムの種類ごとに分類して List を作成
    /// </summary>
    private void CreateItemNamesListFromItemData()
    {

        // ItemName 型の enum に登録されている列挙子をすべて取り出して文字列の配列にして取得し、values 変数に代入
        string[] values = Enum.GetNames(typeof(ItemName));

        // ItemDataSO スクリプタブル・オブジェクト内の ItemData の情報を１つずつ順番に取り出して itemData 変数に代入
        foreach (ItemDataSO.ItemData itemData in itemDataSO.itemDataList)
        {

            // values 配列変数の中を先頭から順番に検索し、現在取り出している itemData 変数の itemName と合致した値があれば
            // TODO 山浦先生に質問する。(x => x == itemData.itemName.ToString())←ここの情報の処理方法がわからない
            // 【string.IsNullOrEmpty()】()内の文字列がnullか空の文字列か判別する

            if (!string.IsNullOrEmpty(values.FirstOrDefault(x => x == itemData.itemName.ToString())))
            {

                // 最初に合致した値を文字列として代入
                string itemName = itemData.itemName.ToString();

                // 現在取り出している itemData 変数の ItemType の値がどの case と合致するかを判定(ここは上のメソッドの switch 文と同じ分岐にしても実装できます。学習のためにキャスト処理を入れています)
                // enum 武器 → 【cast】型変換→(int) 0 処理している
                switch ((int)itemData.itemType)
                {

                    // itemData.itemType == 0(ItemType 型の列挙子の最初のもの)
      
                    case 0:
                        BukiItemNamesList.Add(itemName);
                        break;
                    case 1:
                        BouguItemNamesList.Add(itemName);
                        break;
                    case 2:
                        DouguItemNamesList.Add(itemName);
                        break;
                    case 3:
                        DaijinamonoItemNamesList.Add(itemName);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// EventData の最大要素数を取得
    /// </summary>
    /// <returns></returns>
    public int GetEventDataSOCount()
    {
        return eventDataSO.eventDatasList.Count;
    }


}
