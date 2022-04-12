using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameData : MonoBehaviour
{
    public static GameData instance;        // シングルトンデザインパターンのクラスにするための変数
    public int randomEncountRate;           // エンカウントの発生率
    public bool isEncouting;                // エンカウントしている状態かどうかの判定用。true の場合エンカウントしている状態
    public bool isDebug;                    // デバッグ用の変数。true ならば、エンカウントしている状態をリセットできる

    private Vector3 currentPlayerPos;       // エンカウント時の Player の位置情報を保持するための変数
    private Vector2 currentLookDirection;   // エンカウント時の Player の向き情報を保持するための変数

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
    /// エンカウント時のプレイヤーキャラの位置と方向の情報を保持
    /// </summary>
    /// <param name="encountPlayerPos"></param>
    /// <param name="encountLookDirection"></param>
    public void SetEncountPlayerPosAndDirection(Vector3 encountPlayerPos, Vector2 encountLookDirection)
    {
        // エンカウント時のプレイヤーキャラの位置と方向の情報が引数で届くので、それらを変数に保持
        currentPlayerPos = encountPlayerPos;
        currentLookDirection = encountLookDirection;

        Debug.Log("プレイヤーのエンカウント位置情報更新");
    }

    /// <summary>
    /// エンカウント時のプレイヤーキャラの位置情報を戻す
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurrentPlayerPos()
    {
        // 保持している情報を戻す
        return currentPlayerPos;
    }

    /// <summary>
    /// エンカウント時のプレイヤーキャラの方向を戻す
    /// </summary>
    /// <returns></returns>
    public Vector2 GetCurrentLookDirection()
    {
        // 保持している情報を戻す
        return currentLookDirection;
    }
}


