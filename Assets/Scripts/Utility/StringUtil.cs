using UnityEngine;
using System.Collections;
using System.Text;

public class StringUtil
{
    public static readonly string[] splitSeparator = new string[] { "|" };
    static StringBuilder m_StringBuilder = new StringBuilder();

    static object lockObject = new object();

    public static string Contact(params object[] objects)
    {
        lock (lockObject)
        {
            m_StringBuilder.Remove(0, m_StringBuilder.Length);
            for (int i = 0; i < objects.Length; ++i)
            {
                if (objects[i] != null)
                {
                    m_StringBuilder.Append(objects[i]);
                }
            }
            return m_StringBuilder.ToString();
        }
    }

}
