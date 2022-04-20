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

    //  会話中である場合 true になる、会話状態を表現する変数
    private bool isTalk; 

    private NonPlayerCharacter nonPlayerCharacter;

    // 1文字当たりの表示速度。小さいほど早く表示される
    private float wordSpeed = 0.1f;

    // 会話回数(bool 型でもいいです)
    private int currentTalkCount;

    void Start()
    {
        SetUpDialog();
    }

    /// <summary>
    /// ダイアログの設定
    /// </summary>
    public void SetUpDialog() //public or private
    {
        canvasGroup.alpha = 0.0f;

        // EventData を初期化
        eventData = null;

    }

    /// <summary>
    /// ダイアログの表示
    /// </summary>
    public IEnumerator DisplayDialog(EventData eventData, NonPlayerCharacter nonPlayerCharacter)
    {   //  <=  戻り値を修正してコルーチン・メソッドに変更する。また、第2引数を追加する

        if (this.nonPlayerCharacter == null)
        {　　　　　　　　　　　　//　if 文節を３行分追加します
            this.nonPlayerCharacter = nonPlayerCharacter;
        }

        if (this.eventData == null)
        {
            this.eventData = eventData;
        }

        canvasGroup.DOFade(1.0f, 0.5f);

        txtTitleName.text = this.eventData.title;

        // 会話イベント開始
        isTalk = true;                 //　<=　追加します

        // メッセージ表示
        //yield return StartCoroutine(PlayTalkEventProgress(this.eventData.dialogs));   //　<=　追加します

        // 従来の会話イベントの場合
        if (this.eventData.eventDataDetailsList.Exists(x => x.eventProgressType == EventProgressType.None))
        {
            // TODO 画像データがある場合には、Image 型の変数を宣言フィールドで用意しておいて、 EventDataDetail クラスにある eventSprite を代入する

            // ノーマルの会話イベント
            yield return StartCoroutine(PlayTalkEventProgress(this.eventData.eventDataDetailsList.Find(x => x.eventProgressType == EventProgressType.None).dialogs));
        }
        // 進行型の会話イベントの場合
        else
        {
            // 進行型の会話イベントをクリア済か確認
            if (GameData.instance.CheckClearTalkEventNum(this.eventData.no))
            {
                // TODO 画像データがある場合には、Image 型の変数を宣言フィールドで用意しておいて、 EventDataDetail クラスにある eventSprite を代入する

                // クリア後の会話イベント
                yield return StartCoroutine(PlayTalkEventProgress(this.eventData.eventDataDetailsList.Find(x => x.eventProgressType == EventProgressType.Cleard).dialogs));
            }
            // まだクリアしていない場合
            else
            {
                // イベントの種類を特定。持っているアイテムを消耗するタイプか、持っているだけてよいタイプか判定するために、EventDataDetail を取得
                EventData.EventDataDetail talkEventDataDetail = this.eventData.eventDataDetailsList.Find(x => (x.eventProgressType == EventProgressType.Need || x.eventProgressType == EventProgressType.Permission));

                // はじめて会話している場合
                if (currentTalkCount == 0)
                {
                    // TODO 画像データがある場合には、Image 型の変数を宣言フィールドで用意しておいて、 EventDataDetail クラスにある eventSprite を代入する

                    // 進行型の会話イベントのうち、クリア前の会話イベント
                    yield return StartCoroutine(PlayTalkEventProgress(talkEventDataDetail.dialogs));

                    currentTalkCount++;
                }
                // ２回目以降の場合
                else
                {
                    // 会話イベント達成のフラグの確認
                    bool isNeedItems = false;

                    // 進行型の会話イベントに対して、必要なアイテム(お金や、地点情報など含む)をすべて持っているかどうかを１つずつ確認
                    for (int i = 0; i < talkEventDataDetail.eventItemNames.Length; i++)
                    {
                        // クリアアイテムがない場合
                        if (!GameData.instance.CheckTalkEventItemFromItemInvenry(talkEventDataDetail.eventItemNames[i], talkEventDataDetail.eventItemCounts[i]))
                        {

                            // チェックを終了して、持っていない判定にする
                            break;
                        }
                        else
                        {
                            // クリアアイテムがあり、最後の確認の場合
                            if (i == talkEventDataDetail.eventItemNames.Length - 1)
                            {

                                // クリアに必要なすべてのアイテムを持っている判定にする
                                isNeedItems = true;
                            }
                        }
                    }

                    // クリアアイテムが必要数だけあるか確認
                    if (isNeedItems)
                    {

                        // すべてそろっているなら、クリア判定とて進行型の会話イベントを進める
                        // 初回クリアなら
                        if (!GameData.instance.CheckClearTalkEventNum(this.eventData.no))
                        {

                            // TODO 画像データがある場合には、Image 型の変数を宣言フィールドで用意しておいて、 EventDataDetail クラスにある eventSprite を代入する

                            // クリア達成時の会話イベント
                            yield return StartCoroutine(PlayTalkEventProgress(this.eventData.eventDataDetailsList.Find(x => x.eventProgressType == EventProgressType.Get).dialogs));

                            // アイテム獲得
                            yield return StartCoroutine(GetEventItems(this.eventData.eventDataDetailsList.Find(x => x.eventProgressType == EventProgressType.Get)));

                            // 会話イベントのクリア状態を保存
                            GameData.instance.AddClearTalkEventNum(this.eventData.no);

                            // アイテムを消耗するイベントの場合
                            if (talkEventDataDetail.eventProgressType == EventProgressType.Need)
                            {

                                // 消耗対象をすべて確認
                                for (int i = 0; i < talkEventDataDetail.eventItemNames.Length; i++)
                                {

                                    // TODO 分岐を作成し、お金か、アイテムを減算するようにする
                                    if (talkEventDataDetail.eventItemNames[i] == ItemName.お金)
                                    {

                                        // お金を減算
                                        GameData.instance.CalculateMoney(-talkEventDataDetail.eventItemCounts[i]);
                                    }
                                    else
                                    {

                                        // アイテムを減算
                                        GameData.instance.RemoveItemInventryData(talkEventDataDetail.eventItemNames[i], talkEventDataDetail.eventItemCounts[i]);
                                    }
                                }
                            }

                            // クリアした会話イベントをセーブ 
                            GameData.instance.SaveClearTalkEventNum(this.eventData.no);

                            // インベントリの状態をセーブ
                            GameData.instance.SaveItemInventryDatas();

                            // TODO お金や経験値のセーブ

                        }
                        // クリアアイテムがない場合
                    }
                    else
                    {

                        // TODO 画像データがある場合には、Image 型の変数を宣言フィールドで用意しておいて、 EventDataDetail クラスにある eventSprite を代入する

                        // クリア前の会話イベント
                        yield return StartCoroutine(PlayTalkEventProgress(talkEventDataDetail.dialogs));
                    }
                }
            }
        }

        // 会話イベント終了
        isTalk = false;                                 //　<=　追加します
        //Debug.Log("会話終了");
        // Dialog を閉じる
        nonPlayerCharacter.StopTalk();                  //　<=　追加します

        //txtDialog.DOText(this.eventData.dialog, 1.0f).SetEase(Ease.Linear);    //  <=  複数のページになるので、１ページだけ表示の処理はコメントアウトする
        // TODO 画像データがある場合には、Image 型の変数を用意して eventData.eventSprite を代入する
    }

    /// <summary>
    /// ダイアログの非表示
    /// </summary>
    public void HideDialog()
    {
        if (isTalk)
        {
            return;
        }
        canvasGroup.DOFade(0.0f, 2.5f);
        txtDialog.text = "";
        Debug.Log("Window閉じる");
    }

    /// <summary>
    /// 探索対象を獲得
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="treasureBox"></param>
    /// <returns></returns>
    public IEnumerator DisplaySearchDialog(EventData eventData, TreasureBox treasureBox)
    {
        // 会話ウインドウを表示
        canvasGroup.DOFade(1.0f, 0.5f);
        // タイトルに探索物の名称を表示
        txtTitleName.text = eventData.title;
        // アイテム獲得
        //GetEventItems(eventData);
        // アイテム獲得(コルーチンの呼び出しに変更します)  この処理が終了してアイテムをすべて獲得するまで、下の処理にはいかないようにしています
        yield return StartCoroutine(GetEventItems(eventData.eventDataDetailsList[0]));
        // ダイアログを閉じる
        treasureBox.CloseTreasureBox();
        // 獲得した宝箱の番号を GameData に追加
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
    private IEnumerator GetEventItems(EventData.EventDataDetail eventDataDetail)
    {
        // 獲得したアイテムの名前と数を表示
        //txtDialog.text = eventData.eventItemName.ToString() + " × " + eventData.eventItemCount + " 獲得";
        // GameData にデータを登録　=　これがアイテム獲得の実処理
        //GameData.instance.AddItemInventryData(eventData.eventItemName, eventData.eventItemCount);

        // 獲得したアイテムの種類分だけ繰り返す
        for (int i = 0; i < eventDataDetail.eventItemNames.Length; i++)
        {
            bool isClick = false;

            // 獲得したアイテムの名前と数を表示
            txtDialog.DOText(eventDataDetail.eventItemNames[i].ToString() + " × " + eventDataDetail.eventItemCounts[i] + " 獲得", 1.0f).SetEase(Ease.Linear).OnComplete(() => { isClick = true; });

            // 獲得した種類で分岐
            if (eventDataDetail.eventItemNames[i] == ItemName.お金)
            {
                // TODO お金の加算処理
                GameData.instance.CalculateMoney(eventDataDetail.eventItemCounts[i]);

            }
            else
            if (eventDataDetail.eventItemNames[i] == ItemName.経験値)
            {
                // TODO 経験値の加算処理

            }
            else
            {
                // アイテム獲得
                GameData.instance.AddItemInventryData(eventDataDetail.eventItemNames[i], eventDataDetail.eventItemCounts[i]);
            }
            // アクションボタンを押すと次のメッセージ表示
            yield return new WaitUntil(() => Input.GetButtonDown("Action") && isClick);
            txtDialog.text = "";
            yield return null;
        }
    }

    /// <summary>
    /// 会話イベントのメッセージ再生とページ送り
    /// </summary>
    /// <param name="dialogs"></param>
    /// <returns></returns>
    private IEnumerator PlayTalkEventProgress(string[] dialogs)
    {
        bool isClick = false;
        Debug.Log(dialogs.Length);//ダイアログの配列の最大数を数える。ゼロなら

        // 複数のメッセージを順番に表示
        foreach (string dialog in dialogs)
        {
            Debug.Log(dialog);
            isClick = false;

            // １ページ分の文字を、１文字当たり 0.1 秒ずつかけて等速で表示。表示終了後、isClick を true にして文字が全文表示された状態にする
            txtDialog.DOText(dialog, dialog.Length * wordSpeed).SetEase(Ease.Linear).OnComplete(() => { isClick = true; });

            // １ページの文字が全文表示されている場合かつ、アクションボタンを押すと次のメッセージ表示。それまでは処理を中断して待機する
            yield return new WaitUntil(() => Input.GetButtonDown("Action") && isClick);

            // 次のページのために、現在表示されている文字を消去
            txtDialog.text = "";

            yield return null;
        }
    }
}
