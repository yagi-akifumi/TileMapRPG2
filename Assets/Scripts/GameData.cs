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
    }

    [Header("所持アイテムのリスト")]
    public List<ItemInventryData> itemInventryDatasList = new List<ItemInventryData>();

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
    public void SaveItemInventryDatas() {

　　　　// 所持しているアイテムの数だけ処理を行う
        for (int i = 0; i < itemInventryDatasList.Count; i++) {

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

　　// デバッグ用
    private void Update() {

　　　　// デバッグ用　セーブ
        if (Input.GetKeyDown(KeyCode.K) && isDebug) {
            SaveItemInventryDatas();
        }
    }

}


