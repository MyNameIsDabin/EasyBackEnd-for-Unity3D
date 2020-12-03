using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

/*
 * 차트 매니저는 뒤끝 version2 기준으로 작성되었습니다.
 * https://developer.thebackend.io/unity3d/guide/chart/getChartv1_1/
 */

namespace BackEndExtends
{
    public class ChartManager : Singleton<ChartManager>
    {
        private Dictionary<string, IList> db = new Dictionary<string, IList>();
        private Dictionary<Type, string> dbNamesByType = new Dictionary<Type, string>();

        public List<T> GetDataList<T>(string dbName = null)
        {
            dbName = (dbName != null) ? dbName : dbNamesByType[typeof(T)];
            return db[dbName] as List<T>;
        }

        public T GetDataById<T>(int id, string dbName = null)
        {
            dbName = (dbName != null) ? dbName : dbNamesByType[typeof(T)];
            return (T)db[dbName][id];
        }

        public void LoadChartDatas<T>(string chartName)
        {
            BackendReturnObject returnChartList = Backend.Chart.GetChartList();
            if (returnChartList.IsSuccess())
            {
                LitJson.JsonData json = returnChartList.GetReturnValuetoJSON();
                bool foundedChart = false;
                for (int i = 0; i < json["rows"].Count; i++)
                {
                    LitJson.JsonData row = json["rows"][i];
                    string name = row["chartName"]["S"].ToString();

                    if (name == chartName)
                    {
                        if (row["selectedChartFileId"].ContainsKey("N"))
                        {
                            foundedChart = true;
                            string selectedChartFileId = row["selectedChartFileId"]["N"].ToString();
                            BackendReturnObject returnChartContents = Backend.Chart.GetChartContents(selectedChartFileId);

                            if (returnChartContents.IsSuccess())
                            {
                                string jsonStr = BackendReturnObject.Flatten(returnChartContents.GetReturnValuetoJSON()["rows"]).ToJson();
                                List<T> dataList = JsonToDataList<T>(jsonStr);

                                dbNamesByType.Add(typeof(T), chartName);
                                db.Add(chartName, dataList);
                            }
                            else
                            {
                                Debug.LogWarning(chartName + "차트 내용을 불러올 수 없습니다");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("적용중인 " + chartName + "가 없습니다");
                        }
                    }
                }

                if (!foundedChart)
                {
                    Debug.LogWarning(chartName + "차트를 찾을 수 없습니다");
                }
            }
            else
            {
                Debug.LogWarning("차트가 목록을 받아오지 못했습니다");
            }
        }

        private List<T> JsonToDataList<T>(string jsonString)
        {
            return JsonUtility.FromJson<DataList<T>>("{ \"dataList\": " + jsonString + "}").ToList();
        }
    }
}
