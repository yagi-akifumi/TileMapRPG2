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


    void Start()
    {

        // このスクリプトがアタッチされているゲームオブジェクトにアタッチされているコンポーネントの中から、<指定>したコンポーネントの情報を取得して、左辺に用意した変数に代入
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        // InputManager の Horizontal に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        horizontal = Input.GetAxis("Horizontal");

        // InputManager の Vertical に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        vertical = Input.GetAxis("Vertical");
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
}