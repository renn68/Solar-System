using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gemini100
{

    public class Planet
    {
        public Planet(int id)
        {
            PlanetID = id;
            Position = new long[3];
            Parent = null;
        }

        public Planet(int id, Planet parent)
        {
            PlanetID = id;
            Position = new long[3];
            Parent = parent;
        }

        public Planet Parent { get; private set; }

        public double OrbitTime;
        public float StartPosition;
        public double CurrentPosition;

        public int PlanetID;
        public ulong Orbit { get; private set; }
        public long[] Position;
        public long xAxisFoci { get; private set; }
        public long yAxisFoci { get; private set; }
        public float Mass;
        private long originX;
        private long originY;

        private const uint baseOrbitTime = 60 * 60 * 24 * 365;
        private const ulong baseOrbit = 149597871000; // 149 billion meters

        private float eccentricityScaleMinor = 30f;
        private float eccentricityScaleMajor = 10f;
        private float eccentricityScaleExtreme = 2f;

        private float massScale = 1.0f;
        private float massChanceSmall = 0.1f;
        private float massChanceSmallAverage = 0.3f;
        private float massChanceAverage = 0.7f;
        private float massChanceLargeAverage = 0.9f;
        // private float massChanceLarge = 1.0f;

        private float uiScale = 30000000; // 100 Million


       public Vector3 GetPositionVector3()
        {
            float x = (float)Position[0] / uiScale;
            float y = (float)Position[2] / uiScale;
            float z = (float)Position[1] / uiScale;

            return new Vector3(x, y, z);
        }

        public float GetXAxisFociFloat()
        {
            return (float)xAxisFoci / uiScale;
        }

        public float GetYAxisFociFloat()
        {
            return (float)yAxisFoci / uiScale;
        }

        public float GetXAxisFociMoonFloat()
        {
            return (float)moonXAxisFoci / uiScale;
        }

        public float GetYAxisFociMoonFloat()
        {
            return (float)moonYAxisFoci / uiScale;
        }

        public float GetOrbitFloat()
        {
            return (float)Orbit / uiScale;
        }

        public float GetXFloat()
        {
            return (float)Position[0] / uiScale;
        }

        public float GetYFloat()
        {
            return (float)Position[1] / uiScale;
        }

        public void Generate(ulong minOrbit, ulong maxOrbit, long oX, long oY, bool noExtreme = false)
        {
            StartPosition = UnityEngine.Random.Range(0f, 1f);
            CurrentPosition = StartPosition;
            float orbitRadiusRange = (float)(maxOrbit - minOrbit);
            float _orbit = UnityEngine.Random.Range(0f, orbitRadiusRange);

            Orbit = minOrbit + (ulong)_orbit;
            double orbitTimeDouble = (Math.Sqrt(Math.Pow(((double)Orbit / (double)baseOrbit), 3f))) * baseOrbitTime;
            OrbitTime = orbitTimeDouble;

            originX = oX;
            originY = oY;

            Mass = CalculateMass();

            float fociScale = CalculateEccentricity(noExtreme);

            xAxisFoci = originX + (long)UnityEngine.Random.Range(-fociScale, fociScale);
            yAxisFoci = originY + (long)UnityEngine.Random.Range(-fociScale, fociScale);

            Position = Ellipse.CalculatePointLong(
                (double)StartPosition,
                originX,
                originY,
                xAxisFoci,
                yAxisFoci,
                Orbit);
        }

        public void UpdatePosition(ulong time)
        {
            if (Parent == null)
            {
                CurrentPosition = StartPosition + ((double)time / OrbitTime);
                Position = Ellipse.CalculatePointLong(
                    (double)CurrentPosition,
                    originX,
                    originY,
                    xAxisFoci,
                    yAxisFoci,
                    Orbit);
                CurrentPosition %= 1f;
            }
            else
            {
                originX = Parent.Position[0];
                originY = Parent.Position[1];
                xAxisFoci = originX + moonXAxisFoci;
                yAxisFoci = originY + moonYAxisFoci;
                CurrentPosition = StartPosition + ((double)time / OrbitTime);
                Position = Ellipse.CalculatePointLong(
                    (double)CurrentPosition,
                    originX,
                    originY,
                    xAxisFoci,
                    yAxisFoci,
                    Orbit);
                CurrentPosition %= 1f;

            }

        }

        private long moonXAxisFoci;
        private long moonYAxisFoci;
        private readonly double GravitationConstant = 6.674e-11;
        private readonly double EarthMass = 5.972e24;


        public void GenerateMoon(ulong minOrbit, ulong maxOrbit, long oX, long oY, float parentMass, bool noExtreme = false)
        {
            StartPosition = UnityEngine.Random.Range(0f, 1f);
            CurrentPosition = StartPosition;

            float orbitRadiusRange = (float)(maxOrbit - minOrbit);
            float _orbit = UnityEngine.Random.Range(0f, orbitRadiusRange);

            Orbit = minOrbit + (ulong)_orbit;

            OrbitTime = Math.PI * Math.Sqrt(Math.Pow(Orbit, 3f) / (GravitationConstant * EarthMass * parentMass));

            originX = oX;
            originY = oY;

            Mass = CalculateMass();

            float fociScale = CalculateEccentricity(noExtreme);

            moonXAxisFoci = (long)UnityEngine.Random.Range(-fociScale, fociScale);
            moonYAxisFoci = (long)UnityEngine.Random.Range(-fociScale, fociScale);

            xAxisFoci = originX + moonXAxisFoci;
            yAxisFoci = originY + moonYAxisFoci;

            Position = Ellipse.CalculatePointLong(
                CurrentPosition,
                originX,
                originY,
                xAxisFoci,
                yAxisFoci,
                Orbit);
        }

        private float CalculateEccentricity(bool noExtreme)
        {
            float r = UnityEngine.Random.Range(0f, 1f);
            if (r > 0.5f)
            {
                return (float)((float)Orbit / eccentricityScaleMinor);
            }

            if (r > 0.3f  || noExtreme)
            {
                return (float)((float)Orbit / eccentricityScaleMajor);
            }


            return (float)((float)Orbit / eccentricityScaleExtreme);
        }

        private float CalculateMass()
        {
            float massChance = UnityEngine.Random.Range(0f, 1f);

            if (massChance > massChanceSmall)
            {
                return UnityEngine.Random.Range(0.0005f * massScale, 0.05f * massScale);
            }
            if (massChance > massChanceSmallAverage)
            {
                return UnityEngine.Random.Range(0.05f * massScale, 1f * massScale);
            }
            if (massChance > massChanceAverage)
            {
                return UnityEngine.Random.Range(1f * massScale, 5f * massScale);
            }
            if (massChance > massChanceLargeAverage)
            {
                return UnityEngine.Random.Range(5f * massScale, 50f * massScale);
            }

            return UnityEngine.Random.Range(50f * massScale, 500f * massScale);
        }

        private float CalculateMassMoon()
        {
            float massChance = UnityEngine.Random.Range(0f, 1f);

            if (massChance > massChanceSmall)
            {
                return UnityEngine.Random.Range(0.0005f * massScale, 0.05f * massScale);
            }
            if (massChance > massChanceSmallAverage)
            {
                return UnityEngine.Random.Range(0.05f * massScale, 1f * massScale);
            }
            if (massChance > massChanceAverage)
            {
                return UnityEngine.Random.Range(1f * massScale, 5f * massScale);
            }
            if (massChance > massChanceLargeAverage)
            {
                return UnityEngine.Random.Range(5f * massScale, 50f * massScale);
            }

            return UnityEngine.Random.Range(50f * massScale, 500f * massScale);
        }
    }
}