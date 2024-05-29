using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FoodSustain.Util
{
    internal static class Curves
    {

        public static float moodCurve(float max, float intensivity, float parameteValue, float maxParameteValue)
        {
            return Mathf.Max(1, (max * Mathf.Exp(-intensivity * parameteValue)+1) - max * Mathf.Exp(-intensivity * maxParameteValue));
        }

    }
}
