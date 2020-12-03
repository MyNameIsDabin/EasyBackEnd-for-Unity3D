
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BackEndExtends
{
    [Serializable]
    public class DataList<T>
    {
        [SerializeField]
        public List<T> dataList;

        public T GetElement(int index)
        {
            return dataList[index];
        }

        public DataList(List<T> dataList)
        {
            this.dataList = dataList;
        }

        public List<T> ToList()
        {
            return dataList;
        }
    }
}