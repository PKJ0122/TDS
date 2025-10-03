using System.Collections.Generic;
using UnityEngine;

public static class WaitForSecondsCaching
{
    static Dictionary<float, WaitForSeconds> s_caches;

    public static WaitForSeconds Get(float time)
    {
        if (s_caches == null)
        {
            s_caches = new();
        }

        if (!s_caches.ContainsKey(time))
        {
            s_caches.Add(time, new WaitForSeconds(time));
        }

        return s_caches[time];
    }
}
