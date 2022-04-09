using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    [Header("会話イベント判定用")]
    public bool isTalking;         // true の場合は会話イベント中であるように扱う

    /// <summary>
    /// 会話開始
    /// </summary>

    public void PlayTalk(Vector3 playerPos)
    {
        // 会話イベントを行っている状態にする
        isTalking = true;

        // TODO プレイヤーの位置を確認してウインドウを出す位置を決定する


        // TODO 会話イベントのウインドウを表示する

        Debug.Log("会話ウインドウを開く");

    }

    /// <summary>
    /// 会話終了
    /// </summary>
    public void StopTalk()
    {
        // 会話イベントをしていない状態にする
        isTalking = false;

        // TODO 会話イベントのウインドウを閉じる

        Debug.Log("会話ウインドウを閉じる");

    }
}
