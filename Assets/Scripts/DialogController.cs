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
    private string titleName = "dog";

    [SerializeField]
    private string dialog = "ワンワン!";

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
        txtTitleName.text = titleName;
    }

    /// <summary>
    /// ダイアログの表示
    /// </summary>
    public void DisplayDialog()
    {
        canvasGroup.DOFade(1.0f, 0.5f);
        txtDialog.DOText(dialog, 1.0f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// ダイアログの非表示
    /// </summary>
    public void HideDialog()
    {
        canvasGroup.DOFade(0.0f, 0.5f);
        txtDialog.text = "";
    }
}
