using System.Net;
using System.Net.Mail;
using System.Threading;
using BearClaw.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Diagnostics;

namespace BearClaw.Common
{
    class Methods
    {
        /// <summary>
        /// 反序列化字符串到对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="str">对象字符串</param>
        /// <returns>反序列化得到的对象</returns>
        public static T Desrialize<T>(string str) where T : class
        {
            return (T)Desrialize(str, typeof(T));
        }

        /// <summary>
        /// 反序列化字符串到对象
        /// </summary>
        /// <param name="str">对象字符串</param>
        /// <param name="type">对象类型</param>
        /// <returns>反序列化得到的对象</returns>
        public static object Desrialize(string str, Type type)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var ser = new XmlSerializer(type);
                var buffer = Encoding.UTF8.GetBytes(str);
                using (var deserms = new MemoryStream(buffer))
                {
                    return ser.Deserialize(deserms);
                }
            }
            return null;
        }

        /// <summary>
        /// 尝试反序列化字符串到对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="str">对象字符串</param>
        /// <param name="defaultValue">返回值</param>
        /// <returns></returns>
        public static bool TryDesrialize<T>(string str, out T defaultValue) where T : class
        {
            defaultValue = default(T);
            if (!string.IsNullOrEmpty(str))
            {
                var ser = new XmlSerializer(typeof(T));
                var buffer = Encoding.UTF8.GetBytes(str);
                using (var deserms = new MemoryStream(buffer))
                {
                    defaultValue = (T)ser.Deserialize(deserms);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 反序列化字符串到对象集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="str">对象字符串</param>
        /// <returns>反序列化得到的对象集合</returns>
        public static ObservableCollection<T> DesrializeCollection<T>(string str) where T : class
        {
            var result = new ObservableCollection<T>();
            if (!string.IsNullOrEmpty(str))
            {
                var doc = XDocument.Parse(str);
                var elements = doc.Root.Elements();
                if (elements != null)
                {
                    foreach (var element in elements)
                    {
                        result.Add(Desrialize<T>(element.ToString()));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 序列化对象到字符串
        /// </summary>
        /// <param name="obj">序列化的目标对象</param>
        /// <returns>序列化完成的字符串</returns>
        public static string Serialize(object obj)
        {
            var ser = new XmlSerializer(obj.GetType());
            using (var serms = new MemoryStream())
            {
                ser.Serialize(serms, obj);
                serms.Position = 0;
                using (var sr = new StreamReader(serms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static ObservableCollection<JobEntity> Compare(IEnumerable<JobEntity> oldList, IEnumerable<JobEntity> newList)
        {
            var result = new ObservableCollection<JobEntity>();
            foreach (var jobEntity in newList)
            {
                var existed = false;
                foreach (var oldItem in oldList)
                {
                    if (oldItem.StrName.Equals(jobEntity.StrName))
                    {
                        existed = true;
                        break;
                    }
                }
                if (!existed)
                {
                    result.Add(jobEntity);
                }
            }
            return result;
        }


    }
}
