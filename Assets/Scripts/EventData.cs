using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventData
{
    /// <summary>
    /// イベントの種類
    /// </summary>
    public enum EventType   //【enum】登場させたい情報を、列挙子という形で作成が可能
    {
        Talk,
        Search,
    }

    public EventType eventType;    // イベントの種類
    public int no;                 // 通し番号
    public string title;           // タイトル。NPC の名前、探す対象物の名前、など

    [Multiline]
    public string dialog;          // NPC のメッセージ、対象物のメッセージ、など
    public Sprite eventSprite;     // イベントの画像データ

    // TODO そのほかに追加する場合には以下に補記する

}