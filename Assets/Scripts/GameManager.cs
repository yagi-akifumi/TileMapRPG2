using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<TreasureBox> treasureBoxesList = new List<TreasureBox>();

    // ゲーム画面に存在する NonPlayerCharacter を登録する
    [SerializeField]
    private List<NonPlayerCharacter> nonPlayerCharactersList = new List<NonPlayerCharacter>();
    // UIManager ゲームオブジェクトをアサインし、UIManager スクリプトを代入
    [SerializeField]
    private UIManager uiManager;                                                                   



    IEnumerator Start()
    {
        GameData.instance.LoadGetSearchEventNums();
        yield return StartCoroutine(CheckTresureBoxes());

        // 固定型の会話ウインドウを利用するか確認
        if (GameData.instance.useTalkWindowType == GameData.TalkWindowType.Fixed)
        {

            // 各 NonPlayerCharacter の会話ウインドウの表示設定を固定型に変更
            yield return StartCoroutine(SwitchTalkWindowType());
        }

    }

    /// <summary>
    /// すでに獲得している宝箱であるかどうかを判定
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckTresureBoxes()
    {

        foreach (TreasureBox treasureBox in treasureBoxesList)
        {
            treasureBox.SetUpTresureBox();

            treasureBox.SwitchStateTresureBox(GameData.instance.getSearchEventNumsList.Contains(treasureBox.GetTresureEventNum()));
        }

        yield return null;
    }

    /// <summary>
    /// 会話ウインドウの設定を固定型に変更
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwitchTalkWindowType()
    {
        // GameManager クラスの NPC 用の List に登録されているすべての NPC を対象として処理を行う
        for (int i = 0; i < nonPlayerCharactersList.Count; i++)
        {
            // 各 NPC において固定型の会話ウインドウを利用するための設定を行う
            nonPlayerCharactersList[i].SetUpFixedTalkWindow(uiManager);
        }

        yield return null;
    }

}