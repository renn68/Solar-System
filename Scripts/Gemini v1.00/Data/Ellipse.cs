using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gemini100
{

    public static class Ellipse
    {

        public static long[] CalculatePoint(float position, float xAxis, float yAxis, ulong orbit)
        {
            double angle = 2 * Math.PI * position;
            double _x = Math.Sin(angle) * xAxis * orbit;
            double _y = Math.Cos(angle) * yAxis * orbit;
            long x = (long)_x;
            long y = (long)_y;
            return new long[] { x, y, 0 };
        }

        public static long[] CalculatePointLong(double position, long x1, long y1, long x2, long y2, ulong orbit)
        {

            double a = (double)orbit / 2f;                                                              // Semimajor axis
            double x0 = (double)(x1 + x2) / 2f;                                                         // Center x-value
            double y0 = (double)(y1 + y2) / 2f;                                                         // Center y-value
            double f = Math.Sqrt(Math.Pow((double)(x1 - x0), 2f) + Math.Pow((double)(y1 - y0), 2f));    // Distance from center to focus
            double b = Math.Sqrt(a * a - f * f);                                                        // Semiminor axis
            double phi = Math.Atan2((double)(y2 - y1), (double)(x2 - x1));                              // Angle between major axis and x-axis

            // Parametric plot in t
            double t = 2 * Math.PI * position;

            double _x = x0 + a * Math.Cos(t) * Math.Cos(phi) - b * Math.Sin(t) * Math.Sin(phi);
            double _y = y0 + a * Math.Cos(t) * Math.Sin(phi) + b * Math.Sin(t) * Math.Cos(phi);

            long x = (long)_x;
            long y = (long)_y;

            return new long[] { x, y, 0 };
        }

        public static float[] CalculatePointFloat(float position, float x1, float y1, float x2, float y2, float orbit)
        {

            float a = orbit / 2f;                                                                  // Semimajor axis
            float x0 = (x1 + x2) / 2f;                                                             // Center x-value
            float y0 = (y1 + y2) / 2f;                                                             // Center y-value
            float f = Mathf.Sqrt(Mathf.Pow((x1 - x0), 2f) + Mathf.Pow((y1 - y0), 2f));        // Distance from center to focus
            float b = Mathf.Sqrt(a * a - f * f);                                                            // Semiminor axis
            float phi = Mathf.Atan2((y2 - y1), (x2 - x1));                                  // Angle between major axis and x-axis

            // Parametric plot in t
            float t = 2 * Mathf.PI * position;

            float x = x0 + a * Mathf.Cos(t) * Mathf.Cos(phi) - b * Mathf.Sin(t) * Mathf.Sin(phi);
            float y = y0 + a * Mathf.Cos(t) * Mathf.Sin(phi) + b * Mathf.Sin(t) * Mathf.Cos(phi);



            return new float[] { x, 0f, y };
        }

        public static Vector3 CalculatePointVector3(float position, float x1, float y1, float x2, float y2, float orbit)
        {

            float a = orbit / 2f;                                                                  // Semimajor axis
            float x0 = (x1 + x2) / 2f;                                                             // Center x-value
            float y0 = (y1 + y2) / 2f;                                                             // Center y-value
            float f = Mathf.Sqrt(Mathf.Pow((x1 - x0), 2f) + Mathf.Pow((y1 - y0), 2f));        // Distance from center to focus
            float b = Mathf.Sqrt(a * a - f * f);                                                            // Semiminor axis
            float phi = Mathf.Atan2((y2 - y1), (x2 - x1));                                  // Angle between major axis and x-axis

            // Parametric plot in t
            float t = 2 * Mathf.PI * position;

            float x = x0 + a * Mathf.Cos(t) * Mathf.Cos(phi) - b * Mathf.Sin(t) * Mathf.Sin(phi);
            float y = y0 + a * Mathf.Cos(t) * Mathf.Sin(phi) + b * Mathf.Sin(t) * Mathf.Cos(phi);



            return new Vector3(x, 0f, y);
        }

        public static long DistanceBetween2Points(long x1, long y1, long x2, long y2)
        {
            return (long)Math.Sqrt(Math.Pow(((double)x2 - (double)x1), 2f) +
                    Math.Pow(((double)y2 - (double)y1), 2f));
        }

    }
}
