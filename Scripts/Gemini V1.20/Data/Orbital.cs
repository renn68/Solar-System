using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gemini120
{

    public struct PositionStrut
    {
        public double x;
        public double y;
        public double z;
    }

    public struct RotationStrut
    {
        public float x;
        public float y;
        public float z;

    }

    public class Orbital
    {

        public const double AU = 149597871000.0;
        public const float UISCALE = 30000000; //300 million
        public const double ONEYEAR = (float)(60 * 60 * 24 * 365);

        public Orbital Parent { get; protected set; }

        public float Mass { get; protected set; }
        public double Velocity { get; protected set; }
        public PositionStrut Vector { get; protected set; }
        public bool Generated { get; protected set; }

        public int OrbitalID { get; protected set; }

        public PositionStrut Position { get; protected set; }
        public RotationStrut Rotation { get; protected set; }


        public Vector3 GetPositionVector3()
        {
            float x = (float)(Position.x / UISCALE);
            float y = (float)(Position.y / UISCALE);
            float z = (float)(Position.z / UISCALE);

            return new Vector3(x, y, z);
        }

        public float GetXFloat()
        {
            return (float)(Position.x / UISCALE);
        }

        public float GetZFloat()
        {
            return (float)(Position.z / UISCALE);
        }

    }
}
