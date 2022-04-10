using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    [SerializeField]
    private EventDataSO eventDataSO; //スクリプタブル・オブジェクトを登録して管理するための変数を EventDataSO 型で宣言

    private void Awake()
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
}