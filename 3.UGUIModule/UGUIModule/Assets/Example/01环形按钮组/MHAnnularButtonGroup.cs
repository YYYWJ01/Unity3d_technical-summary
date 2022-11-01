using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 环形按钮组
namespace MHU3D
{
    public class MHAnnularButtonGroup : MonoBehaviour
    {
        public List<RectTransform> list;
        public float radius = 0;
        public float angle = 40;
    
        private float width = 0;
        private Vector3 dir = Vector3.zero;
        private float halfAngle = 20;
    
        void Awake () {
            list = new List<RectTransform>();
            Vector3 pos = transform.localPosition;
            Vector3 temp = new Vector3(pos.x, pos.y - 100, pos.z) - pos;
            dir = temp.normalized;
            halfAngle = angle / 2;
        }
        [SerializeField]
        private bool isOrder = false;
    
        public bool IsOrder
        {
            set
            {
                isOrder = value;
            }
        }
    
        public void AddList(RectTransform rect)
        {
            list.Add(rect);
            rect.SetParent(transform);
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
            rect.localPosition = Vector3.zero;
            if (width == 0)
            {
                width = rect.sizeDelta.y / 2;
            }
            if (radius == 0)
            {
                radius = GetComponent<RectTransform>().sizeDelta.y / 2 - width;
            }
        }
    
        public void Refresh()
        {
            Vector3 pos = transform.localPosition;
            Vector3 temp = new Vector3(pos.x, pos.y - 100, pos.z) - pos;
            dir = temp.normalized;
            halfAngle = angle / 2;
            int count = list.Count;
            int middle = Mathf.CeilToInt(count / 2);
            int remainder = count % 2;
            for (int i = 0; i < list.Count; i++)
            {
                Vector3 s = Vector3.zero;
                int index = i + 1;
            
                if (remainder == 0)
                {   
                    if (index <= middle)
                    {
                        if (isOrder)
                        {
                            index=middle - index+1;
                        }
                        s = Quaternion.Euler(0, 0, -angle * index + halfAngle) * dir;
                    }
                    else
                    {
                        index = index - middle;
                        s = Quaternion.Euler(0, 0, angle * index - halfAngle) * dir;
                    }
                }
                else
                {
                    if (index <= middle)
                    {
                        if (isOrder)
                        {
                            index = middle - index + 1;
                        }
                        s = Quaternion.Euler(0, 0, -angle * index) * dir;
                    }
                    else if (index == middle + 1)
                    {
                        s = Quaternion.Euler(0, 0, 0) * dir;
                    }
                    else if(index > middle + 1)
                    {
                        index = index - middle - 1;
                        s = Quaternion.Euler(0, 0, angle * index) * dir;
                    }
                }
                list[i].localPosition = (s * radius);
            }
        }
    
        public void OnValidate()
        {
            Refresh();
        }
    
        public void Clear()
        {
            list.Clear();
            width = 0;
        }
    
        private void OnDestroy()
        {
            foreach (var item in list)
            {
                GameObject.Destroy(item.gameObject);
            }
            list.Clear();
            width = 0;
        }
    }
}

