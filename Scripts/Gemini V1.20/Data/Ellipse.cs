using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gemini120
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

        public static PositionStrut CalculatePointDouble(double position, double x1, double z1, double x2, double z2, double orbit)
        {

            double a = orbit / 2f;                                                              // Semimajor axis
            double x0 = (x1 + x2) / 2f;                                                         // Center x-value
            double z0 = (z1 + z2) / 2f;                                                         // Center y-value
            double f = Math.Sqrt(Math.Pow((x1 - x0), 2f) + Math.Pow((z1 - z0), 2f));            // Distance from center to focus
            double b = Math.Sqrt(a * a - f * f);                                                // Semiminor axis
            double phi = Math.Atan2((z2 - z1), (x2 - x1));                                      // Angle between major axis and x-axis

            // Parametric plot in t
            double t = 2 * Math.PI * position;

            double x = x0 + a * Math.Cos(t) * Math.Cos(phi) - b * Math.Sin(t) * Math.Sin(phi);
            double z = z0 + a * Math.Cos(t) * Math.Sin(phi) + b * Math.Sin(t) * Math.Cos(phi);

            return new PositionStrut { x = x, y = 0, z = z };
        }

        public static PositionStrut CalculatePointDouble(double position, double x1, double y1, double z1, 
            double x2, double z2, double orbit, RotationStrut rotation)
        {

            double a = orbit / 2f;                                                              // Semimajor axis
            double x0 = (x2) / 2f;                                                         // Center x-value
            double z0 = (z2) / 2f;                                                         // Center y-value
            double f = Math.Sqrt(Math.Pow((0f - x0), 2f) + Math.Pow((0f - z0), 2f));            // Distance from center to focus
            double b = Math.Sqrt(a * a - f * f);                                                // Semiminor axis
            double phi = Math.Atan2((z2), (x2));                                      // Angle between major axis and x-axis

            // Parametric plot in t
            double t = 2 * Math.PI * position;

            double x3 = x0 + a * Math.Cos(t) * Math.Cos(phi) - b * Math.Sin(t) * Math.Sin(phi);
            double z3 = z0 + a * Math.Cos(t) * Math.Sin(phi) + b * Math.Sin(t) * Math.Cos(phi);
            double y3 = 0f;

            // X axis rotation
            double y4 = y3 * Mathf.Cos(rotation.x) - z3 * Mathf.Sin(rotation.x);
            double z4 = z3 * Mathf.Cos(rotation.x) + y3 * Mathf.Sin(rotation.x);
            // Z axis Rotation
            double y5 = y4 * Mathf.Cos(rotation.z) - x3 * Mathf.Sin(rotation.z);
            double x4 = x3 * Mathf.Cos(rotation.z) + y4 * Mathf.Sin(rotation.z);


            double x = x4 + x1;
            double y = y5 + y1;
            double z = z4 + z1;

            return new PositionStrut { x = x, y = y, z = z };
        }

        public static double EllipsePerimeter(double x1, double y1, double x2, double y2, double orbit)
        {
            double a = orbit / 2f;                                                              // Semimajor axis
            double x0 = (x1 + x2) / 2f;                                                         // Center x-value
            double y0 = (y1 + y2) / 2f;                                                         // Center y-value
            double f = Math.Sqrt(Math.Pow((x1 - x0), 2f) + Math.Pow((y1 - y0), 2f));            // Distance from center to focus
            double b = Math.Sqrt(a * a - f * f);                                                // Semiminor axis

            double t = Math.Pow(((a - b) / (a + b)), 2f);

            double perimeter = Math.PI * (a + b) *
                (1 + 3*t/(10 + Math.Sqrt(4 - 3*t)));

            return perimeter;
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

        public static Vector3 CalculatePointVector3(double position, double x1, double y1, double z1,
            double x2, double z2, double orbit, RotationStrut rotation)
        {

            PositionStrut pos = CalculatePointDouble(position, x1, y1, z1, x2, z2, orbit, rotation);

            return new Vector3((float)pos.x, (float)pos.y, (float)pos.z);
        }

        public static long DistanceBetween2Points(long x1, long y1, long x2, long y2)
        {
            return (long)Math.Sqrt(Math.Pow(((double)x2 - (double)x1), 2f) +
                    Math.Pow(((double)y2 - (double)y1), 2f));
        }

    }
}
