using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    [Header("会話イベント判定用")]
    public bool isTalking;         // true の場合は会話イベント中であるように扱う

    private DialogController dialogController; // DialogController スクリプトの情報を代入するための変数

    private Vector3 defaultPos; //プレイヤーの位置を確認してウインドウを出す位置(プレイヤーの位置が)
    private Vector3 offsetPos;  //プレイヤーの位置を確認してウインドウを出す位置

    // EventDataのEventTypeのTalkを会話イベントとして設定
    private EventData.EventType eventType = EventData.EventType.Talk;

    // この番号と上記の EventType を使って、スクリプタブル・オブジェクト内から会話イベントのデータを取得します
    [SerializeField, Header("NPC 会話イベントの通し番号")]
    private int npcTalkEventNo;
    [SerializeField, Header("NPC 会話イベントのデータ")]
    private EventData eventData;

    void Start()
    {
        // 子オブジェクトにアタッチされている DialogController スクリプトを取得して変数に代入
        // 【GetComponentInChildren】指定したゲームオブジェクトの子オブジェクトをすべて検索し、＜型＞で指定したコンポーネントの情報を取得するメソッド。
        dialogController = GetComponentInChildren<DialogController>();
        // defaultPos:Y座標をそのまま配置する
        defaultPos = dialogController.transform.position;
        // offsetPos:Y座標を- 5.0f 低い位置に配置する
        offsetPos = new Vector3(dialogController.transform.position.x, dialogController.transform.position.y - 5.0f, dialogController.transform.position.z);

        // DataBaseManager に登録してあるスクリプタブル・オブジェクトを検索し、指定した通し番号の EventData を NPC 用の EventData として取得して代入
        eventData = DataBaseManager.instance.GetEventDataFromNPCEvent(npcTalkEventNo);

    }

    /// <summary>
    /// 会話開始
    /// </summary>

    public void PlayTalk(Vector3 playerPos)
    {
        // 会話イベントを行っている状態にする
        isTalking = true;

        // もし、NPCのY座標の位置がプレイヤーのY座標より高いなら
        if (playerPos.y < transform.position.y)
        {
            // ダイアログコントローラーは offsetPosを使う
            dialogController.transform.position = offsetPos;
        }
        else
        {
            // ダイアログコントローラーは defaultPosを使う
            dialogController.transform.position = defaultPos;
        }


        // TODO プレイヤーの位置を確認してウインドウを出す位置を決定する

        // eventDataから会話イベントのウインドウを表示する
        dialogController.DisplayDialog(eventData);

    }

    /// <summary>
    /// 会話終了
    /// </summary>
    public void StopTalk()
    {
        // 会話イベントをしていない状態にする
        isTalking = false;

        // TODO 会話イベントのウインドウを閉じる

        // 会話イベントのウインドウを閉じる
        dialogController.HideDialog();

    }
}
