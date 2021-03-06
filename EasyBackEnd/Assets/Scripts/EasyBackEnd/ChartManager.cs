﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;
using System.Linq;
using LitJson;
using System.Reflection;
using JetBrains.Annotations;

/*
 * 차트 매니저는 뒤끝 version2 기준으로 작성되었습니다.
 * https://developer.thebackend.io/unity3d/guide/chart/getChartv1_1/
 */

namespace BackEndExtends
{
    public class ChartManager : Singleton<ChartManager>
    {
        private Dictionary<string, Dictionary<string, Dictionary<object, object>>> dataByKey = new Dictionary<string, Dictionary<string, Dictionary<object, object>>>();
        private Dictionary<string, IList> db = new Dictionary<string, IList>();
        private Dictionary<Type, string> dbNamesByType = new Dictionary<Type, string>();
        private JsonData chartListJsonData = null;
        
        public List<T> GetDataList<T>(string dbName = null)
        {
            dbName = dbName ?? dbNamesByType[typeof(T)];
            return db[dbName] as List<T>;
        }

        public T GetDataById<T>(int id, string dbName = null)
        {
            dbName = dbName ?? dbNamesByType[typeof(T)];
            return (T)db[dbName][id];
        }

        public T GetDataByKey<T>(string primaryKey, object value, string dbName = null)
        {
            dbName = dbName ?? dbNamesByType[typeof(T)];
            return (T)dataByKey[dbName][primaryKey][value];
        }

        public ChartManager LoadChartDatas<T>(string chartName = null, params string[] primaryKeys)
        {
            BackendReturnObject retChartList = null;
            bool isSuccessGetChartList = (chartListJsonData != null);

            if (!isSuccessGetChartList)
            {
                retChartList = Backend.Chart.GetChartList();
                isSuccessGetChartList = retChartList.IsSuccess();
            }

            if (isSuccessGetChartList)
            {
                chartListJsonData = chartListJsonData ?? retChartList.GetReturnValuetoJSON();
                bool foundedChart = false;
                for (int i = 0; i < chartListJsonData["rows"].Count; i++)
                {
                    JsonData row = chartListJsonData["rows"][i];
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
                                if (!dataByKey.ContainsKey(chartName))
                                    dataByKey.Add(chartName, new Dictionary<string, Dictionary<object, object>>());

                                JsonData dataRows = returnChartContents.GetReturnValuetoJSON()["rows"];

                                if (primaryKeys.Length > 0)
                                {
                                    for (int j = 0; j < dataRows.Count; j++)
                                    {
                                        JsonData data = dataRows[j];
                                        foreach (var primaryKey in primaryKeys)
                                        {
                                            if (!dataByKey[chartName].ContainsKey(primaryKey))
                                                dataByKey[chartName].Add(primaryKey, new Dictionary<object, object>());

                                            string dataJson = BackendReturnObject.Flatten(data).ToJson();
                                            T toData = JsonUtility.FromJson<T>(dataJson);
                                            FieldInfo field = typeof(T).GetField(primaryKey);
                                            object value = field.GetValue(toData);
                                            dataByKey[chartName][primaryKey].Add(value, toData);
                                        }
                                    }
                                }

                                string jsonStr = BackendReturnObject.Flatten(dataRows).ToJson();
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

            return this;
        }

        private List<T> JsonToDataList<T>(string jsonString)
        {
            return JsonUtility.FromJson<DataList<T>>("{ \"dataList\": " + jsonString + "}").ToList();
        }

        public ChartManager ClearChartListCache()
        {
            chartListJsonData.Clear();
            return this;
        }
    }
}
