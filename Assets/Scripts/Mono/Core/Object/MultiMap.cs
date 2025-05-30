﻿using System;
using System.Collections.Generic;
// using Nino.Core;

namespace TaoTie
{
    // [NinoIgnore]
    public class MultiMap<T, K>: SortedDictionary<T, List<K>>
    {
        private readonly List<K> Empty = new List<K>();

        public void Add(T t, K k)
        {
            List<K> list;
            this.TryGetValue(t, out list);
            if (list == null)
            {
                list = ObjectPool.Instance.Fetch<List<K>>();
                this.Add(t, list);
            }
            list.Add(k);
        }

        public bool Remove(T t, K k)
        {
            List<K> list;
            this.TryGetValue(t, out list);
            if (list == null)
            {
                return false;
            }
            if (!list.Remove(k))
            {
                return false;
            }
            if (list.Count == 0)
            {
                this.Remove(t);
            }
            return true;
        }

        public new bool Remove(T t)
        {
            List<K> list;
            this.TryGetValue(t, out list);
            if (list == null)
            {
                return false;
            }

            base.Remove(t);
            
            list.Clear();
            ObjectPool.Instance.Recycle(list);
            return true;
        }

        /// <summary>
        /// 不返回内部的list,copy一份出来
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public K[] GetAll(T t)
        {
            List<K> list;
            this.TryGetValue(t, out list);
            if (list == null)
            {
                return new K[0];
            }
            return list.ToArray();
        }

        /// <summary>
        /// 返回内部的list
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public new List<K> this[T t]
        {
            get
            {
                this.TryGetValue(t, out List<K> list);
                return list ?? Empty;
            }
            set
            {
                base[t] = value;
            }
        }

        public K GetOne(T t)
        {
            List<K> list;
            this.TryGetValue(t, out list);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return default;
        }

        public bool Contains(T t, K k)
        {
            List<K> list;
            this.TryGetValue(t, out list);
            if (list == null)
            {
                return false;
            }
            return list.Contains(k);
        }
    }
}