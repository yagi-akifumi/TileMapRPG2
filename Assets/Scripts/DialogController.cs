using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogController : MonoBehaviour
{
    [SerializeField]
    private Text txtDialog = null;

    [SerializeField]
    private Text txtTitleName = null;

    [SerializeField]
    private CanvasGroup canvasGroup = null;

    [SerializeField]
    private EventData eventData;//NonPlayerCharacter スクリプトから EventData の情報がメソッドの引数を通じて届きますので、それを代入するための変数

    void Start()
    {
        SetUpDialog();
    }

    /// <summary>
    /// ダイアログの設定
    /// </summary>
    private void SetUpDialog()
    {
        canvasGroup.alpha = 0.0f;
        //txtTitleName.text = titleName;

        // EventData を初期化
        eventData = null;

    }

    /// <summary>
    /// ダイアログの表示
    /// </summary>
    public void DisplayDialog(EventData eventData)  //　①　DisplayDialog引数を追加します
    {
        //　②もし、eventDataに情報がないなら、このeventData がeventData になる
        if (this.eventData == null)
        {
            this.eventData = eventData;
        }
        canvasGroup.DOFade(1.0f, 0.5f);
        //　③ Title として表示するタイトル(NPC の名前)の内容を EventData の内容に変更する
        txtTitleName.text = this.eventData.title;
        //　④  Dialog として表示するメッセージの内容を EventData の内容に変更します
        txtDialog.DOText(this.eventData.dialog, 1.0f).SetEase(Ease.Linear);

        // TODO 画像データがある場合には、Image 型の変数を用意して eventData.eventSprite を代入する
    }

    /// <summary>
    /// ダイアログの非表示
    /// </summary>
    public void HideDialog()
    {
        canvasGroup.DOFade(0.0f, 0.5f);
        txtDialog.text = "";
    }

    /// <summary>
    /// 探索対象を獲得
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="treasureBox"></param>
    /// <returns></returns>
    public void DisplaySearchDialog(EventData eventData, TreasureBox treasureBox)
    {
        // 会話ウインドウを表示
        canvasGroup.DOFade(1.0f, 0.5f);
        // タイトルに探索物の名称を表示
        txtTitleName.text = eventData.title;

        // アイテム獲得
        GetEventItems(eventData);

        // TODO 獲得した宝箱の番号を GameData に追加
        GameData.instance.AddSearchEventNum(treasureBox.treasureEventNo);

        // 獲得した宝箱の番号をセーブ
        GameData.instance.SaveSearchEventNum(treasureBox.treasureEventNo);

        // 所持しているアイテムのセーブ
        GameData.instance.SaveItemInventryDatas();

        // TODO お金や経験値のセーブ

    }


    /// <summary>
    /// アイテム獲得
    /// </summary>
    /// <param name="eventData"></param>
    private void GetEventItems(EventData eventData)
    {
        // 獲得したアイテムの名前と数を表示
        txtDialog.text = eventData.eventItemName.ToString() + " × " + eventData.eventItemCount + " 獲得";

        // GameData にデータを登録　=　これがアイテム獲得の実処理
        GameData.instance.AddItemInventryData(eventData.eventItemName, eventData.eventItemCount);
    }

}
