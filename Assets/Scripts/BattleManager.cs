using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private Button btnBattleEnd;

    void Start()
    {
        // ボタンのOnClickイベントに OnClickBattleEnd メソッドを追加する
        // ボタンを押下した際に実行するメソッドを登録だけなので、この時点ではメソッドは実行されない
        btnBattleEnd.onClick.AddListener(OnClickBattleEnd);
    }

    /// <summary>
    /// バトル終了ボタン押下時の処理
    /// </summary>
    private void OnClickBattleEnd()
    {
        SceneStateManager.instance.NextScene(SceneStateManager.SceneType.Main);
    }
}

