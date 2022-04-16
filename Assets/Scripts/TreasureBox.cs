using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{

    [Header("宝箱イベント判定用")]
    public bool isOpen;

    [SerializeField]
    private DialogController dialogController;

    private Vector3 defaultPos;
    private Vector3 offsetPos;

    private EventData.EventType eventType = EventData.EventType.Search;

    [Header("宝箱イベントの通し番号")]
    public int treasureEventNo;

    [SerializeField, Header("宝箱イベントのデータ")]
    private EventData eventData;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;


    void Start()
    {

        // 探索イベントの準備
        SetUpTresureBox();
    }

    /// <summary>
    /// 探索イベントの準備
    /// </summary>
    public void SetUpTresureBox()
    {
        isOpen = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        dialogController = GetComponentInChildren<DialogController>();

        defaultPos = dialogController.transform.position;
        offsetPos = new Vector3(dialogController.transform.position.x, dialogController.transform.position.y - 5.0f, dialogController.transform.position.z);

        // 対象物 の EventData を取得
        eventData = DataBaseManager.instance.GetEventDataFromEvnetTypeAndEventNo(eventType, treasureEventNo);
    }

    /// <summary>
    /// 探索開始
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="playerController"></param>
    public void OpenTresureBox(Vector3 playerPos, PlayerController playerController)
    {

        if (this.playerController == null)
        {
            this.playerController = playerController;
        }

        isOpen = true;

        if (playerPos.y < transform.position.y)
        {
            dialogController.transform.position = offsetPos;
        }
        else
        {
            dialogController.transform.position = defaultPos;
        }

        Debug.Log("探索イベント用の会話ウインドウを開く");
    }

    /// <summary>
    /// 探索終了
    /// </summary>
    public void CloseTreasureBox()
    {

        playerController.IsTalking = false;

        Debug.Log("探索イベント用の会話ウインドウを閉じる");
    }
}