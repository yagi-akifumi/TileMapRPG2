using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private DialogController dialogController;

    void Start()
    {
        SetUpTalkWindow();
    }

    /// <summary>
    /// 固定型の会話ウインドウの設定
    /// </summary>
    public void SetUpTalkWindow()
    {
        // キャンバスを透明化
        dialogController.SetUpDialog();
    }

    /// <summary>
    /// 固定型の会話ウインドウを表示
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="nonPlayerCharacter"></param>
    public void OpenTalkWindow(EventData eventData, NonPlayerCharacter nonPlayerCharacter)
    {
        StartCoroutine(dialogController.DisplayDialog(eventData, nonPlayerCharacter));    //　<=　呼び出し方法を変更し、第2引数を追加します。
    }

    /// <summary>
        /// 固定型の会話ウインドウを非表示
        /// </summary>
        public void CloseTalkWindow()
    {
        dialogController.HideDialog();
    }
}