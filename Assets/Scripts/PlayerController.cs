using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [Header("移動速度")]
    public float moveSpeed;

    private Rigidbody2D rb;                      // コンポーネントの取得用

    private float horizontal;                    // x 軸(水平・横)方向の入力の値の代入用
    private float vertical;                      // y 軸(垂直・縦)方向の入力の値の代入用

    private Animator anim;　　　　　　　　　　　 // コンポーネントの取得用
    private Vector2 lookDirection = new Vector2(0, -1.0f);   // キャラの向きの情報の設定用

    void Start()
    {

        // このスクリプトがアタッチされているゲームオブジェクトにアタッチされているコンポーネントの中から、<指定>したコンポーネントの情報を取得して、左辺に用意した変数に代入
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        // InputManager の Horizontal に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        horizontal = Input.GetAxis("Horizontal");

        // InputManager の Vertical に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        vertical = Input.GetAxis("Vertical");

        // キャラの向いている方向と移動アニメの同期
        SyncMoveAnimation();
    }

    void FixedUpdate()
    {

        // 移動
        Move();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {

        // 斜め移動の距離が増えないように正規化処理を行い、単位ベクトルとする(方向の情報は持ちつつ、距離による速度差をなくして一定値にする)
        Vector3 dir = new Vector3(horizontal, vertical, 0).normalized;

        // velocity(速度)に新しい値を代入して、ゲームオブジェクトを移動させる
        rb.velocity = dir * moveSpeed;
    }

    /// <summary>
    /// キャラの向いている方向と移動アニメの同期
    /// </summary>
    private void SyncMoveAnimation()
    {
        // 移動のキー入力値を代入
        Vector2 move = new Vector2(horizontal, vertical);

        // いずれかのキー入力があるか確認
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            // 向いている方向を更新
            lookDirection.Set(move.x, move.y);

            // 正規化
            lookDirection.Normalize();

            // キー入力の値と Blend Tree で設定した移動アニメ用の値を確認し、移動アニメを再生
            anim.SetFloat("Look X", lookDirection.x);
            anim.SetFloat("Look Y", lookDirection.y);
        }

        // 停止アニメーション用
        anim.SetFloat("Speed", move.magnitude);

    }
}