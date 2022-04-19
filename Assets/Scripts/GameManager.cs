using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<TreasureBox> treasureBoxesList = new List<TreasureBox>();

    IEnumerator Start()
    {
        GameData.instance.LoadGetSearchEventNums();

        yield return StartCoroutine(CheckTresureBoxes());
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
}