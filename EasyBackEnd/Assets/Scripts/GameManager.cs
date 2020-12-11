using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEndExtends;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        EasyBackEnd.Instance.Init(() =>
        {
            //쉽게 게스트 로그인!
            Debug.Log("GuestID : " + EasyBackEnd.Instance.GuestLogin());
        }, () =>
        {
            Debug.Log("실패");
        });
    }

    public void OnClickLoadChartButton()
    {
        //차트 가져오기
        ChartManager.Instance.LoadChartDatas<ItemData>(
            chartName: "chartExample",
            primaryKeys: "itemName" //primaryKey! 등록해놓으면 GetDataByKey 함수로 데이터를 얻을 수 있다
        );

        //불러온 차트의 데이터 조회하기
        List<ItemData> itemDatas = ChartManager.Instance.GetDataList<ItemData>();

        Debug.Log("전체 데이터 조회------");
        foreach (var data in itemDatas)
            Debug.Log(data.itemName);
        Debug.Log("------------------");

        //Primary Key를 이용해서 데이터 접근하기
        ItemData item1 = ChartManager.Instance.GetDataByKey<ItemData>("itemName", "아이템49");
        Debug.Log(item1.itemName);

        //가져온 차트의 특정 index 데이터 접근하기
        ItemData item2 = ChartManager.Instance.GetDataById<ItemData>(2);
        Debug.Log(item2.itemName);
    }
}
