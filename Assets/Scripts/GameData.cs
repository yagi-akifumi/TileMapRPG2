using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class GameData : MonoBehaviour
{
    public static GameData instance;        // シングルトンデザインパターンのクラスにするための変数
    public int randomEncountRate;           // エンカウントの発生率
    public bool isEncouting;                // エンカウントしている状態かどうかの判定用。true の場合エンカウントしている状態
    public bool isDebug;                    // デバッグ用の変数。true ならば、エンカウントしている状態をリセットできる

    private Vector3 currentPlayerPos;       // エンカウント時の Player の位置情報を保持するための変数
    private Vector2 currentLookDirection;   // エンカウント時の Player の向き情報を保持するための変数

    [System.Serializable]
    public class ItemInventryData
    {
        public ItemName itemName;    // アイテムの名前
        public int count;            // 所持数
        public int number;           // 所持している通し番号

        /// <summary>
        /// ItemInventryData クラスのコンストラクタ
        /// </summary>
        /// <param name="name">アイテムの名前</param>
        /// <param name="value">アイテムの所持数</param>
        /// <param name="num">アイテムを所持した際の通し番号</param>
        public ItemInventryData(ItemName name, int value, int num)
        {
            itemName = name;
            count = value;
            number = num;
        }
    }

    [Header("所持アイテムのリスト")]
    public List<ItemInventryData> itemInventryDatasList = new List<ItemInventryData>();

    [Header("獲得済の探索イベントの番号")]
    public List<int> getSearchEventNumsList = new List<int>();

    void Awake()
    {
        // インスタンスがnullならこのインスタンスを使う。
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // 既にインスタンスがあるなら、このゲームオブジェクトは破壊される。
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// エンカウント時のプレイヤーキャラの位置と方向の情報を保持
    /// </summary>
    /// <param name="encountPlayerPos"></param>
    /// <param name="encountLookDirection"></param>
    public void SetEncountPlayerPosAndDirection(Vector3 encountPlayerPos, Vector2 encountLookDirection)
    {
        // エンカウント時のプレイヤーキャラの位置と方向の情報が引数で届くので、それらを変数に保持
        currentPlayerPos = encountPlayerPos;
        currentLookDirection = encountLookDirection;

        Debug.Log("プレイヤーのエンカウント位置情報更新");
    }

    /// <summary>
    /// エンカウント時のプレイヤーキャラの位置情報を戻す
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurrentPlayerPos()
    {
        // 保持している情報を戻す
        return currentPlayerPos;
    }

    /// <summary>
    /// エンカウント時のプレイヤーキャラの方向を戻す
    /// </summary>
    /// <returns></returns>
    public Vector2 GetCurrentLookDirection()
    {
        // 保持している情報を戻す
        return currentLookDirection;
    }

    /// <summary>
    /// ItemInvetryDatasList の最大数を取得
    /// </summary>
    /// <returns></returns>
    public int GetItemInventryListCount()
    {
        return itemInventryDatasList.Count;
    }

    /// <summary>
    /// ItemInvetryDatasList の中から指定した要素番号の ItemInventryData 情報を取得
    /// </summary>
    /// <param name="no"></param>
    /// <returns></returns>
    public ItemInventryData GetItemInventryData(int no)
    {
        return itemInventryDatasList[no];
    }

    /// <summary>
    /// 所持アイテムのセーブ
    /// </summary>
    public void SaveItemInventryDatas()
    {
        // 所持しているアイテムの数だけ処理を行う
        for (int i = 0; i < itemInventryDatasList.Count; i++)
        {

            // 所持しているアイテムの情報を１つの文字列としてセーブするための準備を行う
            // 【SetString(string Key,string Value)】文字列をセーブ。string Keyを検索して、string Valueがセーブされる文字例
            PlayerPrefs.SetString(itemInventryDatasList[i].itemName.ToString(), itemInventryDatasList[i].itemName.ToString() + "," + itemInventryDatasList[i].count.ToString() + "," + i.ToString());

            Debug.Log("セーブのキー : " + itemInventryDatasList[i].itemName.ToString());
            Debug.Log("セーブ内容 : " + itemInventryDatasList[i].itemName.ToString() + "," + itemInventryDatasList[i].count.ToString() + "," + i.ToString());
        }

        // セーブ
        PlayerPrefs.Save();
        Debug.Log("ItemInventry セーブ完了");
    }

    /// <summary>
    /// 所持アイテムのロード
    /// </summary>
    public void LoadItemInventryDatas()
    {

        // アイテムデータ分だけ繰り返す
        for (int i = 0; i < DataBaseManager.instance.GetItemDataSOCount(); i++)
        {

            // ItemName でセーブしてあるデータが PlayerPrefs 内にあるか
            if (!PlayerPrefs.HasKey(DataBaseManager.instance.GetItemDataFromItemNo(i).itemName.ToString()))
            {

                // セーブデータがなければここで処理を終了し、次のセーブデータを確認する処理へ移る
                continue;
            }

            // セーブされているデータを読み込んで配列に代入
            string[] stringArray = PlayerPrefs.GetString(DataBaseManager.instance.GetItemDataFromItemNo(i).itemName.ToString()).Split(',');

            // セーブデータからアイテムのデータをコンストラクタ・メソッドを利用して復元
            itemInventryDatasList.Add(new ItemInventryData((ItemName)Enum.Parse(typeof(ItemName), stringArray[0]), int.Parse(stringArray[1]), int.Parse(stringArray[2])));
        }

        // 以前に所持していた番号順で所持アイテムの並び順をソート
        itemInventryDatasList = itemInventryDatasList.OrderBy(x => x.number).ToList();

        Debug.Log("ItemInventry ロード完了");
    }

    // デバッグ用
    private void Update()
    {

        // デバッグ用　セーブ
        if (Input.GetKeyDown(KeyCode.K) && isDebug)
        {
            SaveItemInventryDatas();
        }

        // デバッグ用　ロード
        if (Input.GetKeyDown(KeyCode.L) && isDebug)
        {
            LoadItemInventryDatas();
        }

        // デバッグ用　アイテムの追加。所持している場合には加算
        if (Input.GetKeyDown(KeyCode.I) && isDebug)
        {
            // 追加・加算したいアイテムの名前と数を引数に指定してメソッドを呼び出し
            // 引数を変更することで追加・加算するアイテムを指定する
            AddItemInventryData(ItemName.ひのきの棒, 1);
        }

        // デバッグ用　アイテムの削除。所持している場合には減算
        if (Input.GetKeyDown(KeyCode.O) && isDebug)
        {
            // 追加・加算したいアイテムの名前と数を引数に指定してメソッドを呼び出し
            // 引数を変更することで追加・加算するアイテムを指定する
            RemoveItemInventryData(ItemName.ひのきの棒, 1);
        }
    }

    /// <summary>
    /// ItemInvetryData を追加・加算
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="amount"></param>
    public void AddItemInventryData(ItemName itemName, int amount = 1)
    {
        // List の要素を１つずつ確認して、すでに所持しているアイテムか確認
        foreach (ItemInventryData itemInventryData in itemInventryDatasList)
        {

            // 所持しているアイテムの場合
            if (itemInventryData.itemName == itemName)
            {
                // 所持数を加算
                itemInventryData.count += amount;
                Debug.Log("リストに対象アイテムを加算 : " + itemName + " / 合計 : " + itemInventryData.count + " 個");

                // 処理を終了
                return;
            }
        }

        // 所持していないアイテムの場合、新しく所持アイテムとして追加する
        itemInventryDatasList.Add(new ItemInventryData(itemName, amount, itemInventryDatasList.Count));

        Debug.Log("リストに対象アイテムを新規追加 : " + itemName + " / " + amount + " 個");
    }

    /// <summary>
    /// ItemInventryData を減算。0 以下になったら削除
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="amount"></param>
    public void RemoveItemInventryData(ItemName itemName, int amount = 1)
    {
        // List の要素を１つずつ確認して、すでに所持しているアイテムか確認
        foreach (ItemInventryData itemInventryData in itemInventryDatasList)
        {
            // 所持しているアイテムの場合
            if (itemInventryData.itemName == itemName)
            {
                // 所持数を減算
                itemInventryData.count -= amount;
                Debug.Log("リストに対象アイテムを減算 : " + itemName + " / 合計 : " + itemInventryData.count + " 個");

                // 所持数が 0 以下になったら
                if (itemInventryData.count <= 0)
                {
                    // 所持アイテムから削除
                    itemInventryDatasList.Remove(itemInventryData);
                    Debug.Log("リストから対象アイテムを削除 : " + itemName);
                }
                return;
            } 
        }
        Debug.Log("リストに対象アイテムなし");
    }

    /// <summary>
    /// 獲得した探索イベントの番号を保持
    /// </summary>
    public void AddSearchEventNum(int searchEventNum)
    {
        // 引数の探索イベントの番号が List に登録されていない場合(このチェックで重複登録を防いでいる)
        if (!getSearchEventNumsList.Contains(searchEventNum))
        {
            // 獲得した探索イベントの番号を追加
            getSearchEventNumsList.Add(searchEventNum);
        }
    }
}
