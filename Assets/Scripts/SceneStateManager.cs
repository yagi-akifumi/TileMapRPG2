using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    public enum SceneType   //【enum】登場させたい情報を、列挙子という形で作成が可能
    {
        Main,
        Battle,

        // TODO 新しいシーンを作成したら、列挙子にもシーン名を登録する
    }

    void Awake()
    {
        // インスタンスがnullならこのインスタンスを使う。
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // 既にインスタンスがあるなら、このゲームオブジェクトは破壊される。
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 引数で指定したシーンへシーン遷移
    /// </summary>
    /// <param name="nextSceneType"></param>
    public void NextScene(SceneType nextSceneType)
    {
        // シーン名を指定する引数には、enum である SceneType の列挙子を
        // ToString メソッドを使って string 型へキャストして利用
        SceneManager.LoadScene(nextSceneType.ToString());
    }
}
