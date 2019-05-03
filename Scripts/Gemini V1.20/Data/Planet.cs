using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gemini120
{

    public class Planet : Orbital
    {

        public Planet(int id, Orbital parent)
        {
            OrbitalID = id;
            Parent = parent;
            xAxisFoci = new double[2];
            zAxisFoci = new double[2];
            Children = new List<Moon>();
        }


        #region Var Declaration

        public double OrbitTime;
        protected double StartPositionOrbit;
        public double CurrentPositionOrbit { get; protected set; }

        public double Orbit { get; protected set; }
        public double[] xAxisFoci { get; protected set; }
        public double[] zAxisFoci { get; protected set; }
        public List<Moon> Children;

        public double Perimeter { get; protected set; }

        // private const uint baseOrbitTime = 60 * 60 * 24 * 365;

        protected float eccentricityScaleMinor = 30f;
        protected float eccentricityScaleMajor = 10f;
        protected float eccentricityScaleExtreme = 2f;

        protected float massScale = 1.0f;
        protected float massChanceSmall = 0.1f;
        protected float massChanceSmallAverage = 0.3f;
        protected float massChanceAverage = 0.7f;
        protected float massChanceLargeAverage = 0.9f;
        // private float massChanceLarge = 1.0f;

        protected readonly double GravitationConstant = 6.674e-11;
        protected readonly double EarthMass = 5.972e24;

        #endregion

        #region Get Funcs
        public float GetXAxisFociFloat()
        {
            return (float)(xAxisFoci[0] / UISCALE);
        }

        public float GetZAxisFociFloat()
        {
            return (float)(zAxisFoci[0] / UISCALE);
        }

        public float GetOrbitFloat()
        {
            return (float)(Orbit / UISCALE);
        }
        #endregion

        public virtual void Generate(double minOrbit, double maxOrbit, float rotationLimits = 10f, bool noExtreme = false)
        {
            StartPositionOrbit = CurrentPositionOrbit = UnityEngine.Random.Range(0f, 1f);

            Orbit = AU * CalculateOrbit(minOrbit, maxOrbit);

            OrbitTime = Math.PI * Math.Sqrt(Math.Pow(Orbit, 3f) / (GravitationConstant * EarthMass * Parent.Mass));
            Debug.Log(OrbitTime);
            Mass = CalculateMass();

            float fociScale = CalculateEccentricity(noExtreme);

            xAxisFoci[0] = (double)UnityEngine.Random.Range(-fociScale, fociScale);
            zAxisFoci[0] = (double)UnityEngine.Random.Range(-fociScale, fociScale);

            xAxisFoci[1] = Parent.Position.x + xAxisFoci[0];
            zAxisFoci[1] = Parent.Position.z + zAxisFoci[0];

            Rotation = CalculateRotation(rotationLimits);

            Position = Ellipse.CalculatePointDouble(
                StartPositionOrbit,
                Parent.Position.x,
                Parent.Position.y,
                Parent.Position.z,
                xAxisFoci[0],
                zAxisFoci[0],
                Orbit,
                Rotation);

            Perimeter = Ellipse.EllipsePerimeter(
                Parent.Position.x,
                Parent.Position.z,
                xAxisFoci[1],
                zAxisFoci[1],
                Orbit);

            Velocity = Perimeter / OrbitTime;

            Generated = true;
        }

        public void UpdatePosition(double time)
        {

            CurrentPositionOrbit = StartPositionOrbit + (time / OrbitTime);
                Position = Ellipse.CalculatePointDouble(
                    CurrentPositionOrbit,
                    Parent.Position.x,
                    Parent.Position.y,
                    Parent.Position.z,
                    xAxisFoci[0],
                    zAxisFoci[0],
                    Orbit,
                    Rotation);
            CurrentPositionOrbit %= 1f;
 
        }

        public void AddChild(Moon child)
        {
            Children.Add(child);
        }

        protected double CalculateOrbit(double min, double max)
        {
            float orbitRadiusRange = (float)(max - min);
            float _orbit = UnityEngine.Random.Range(0f, orbitRadiusRange);
            return min + (ulong)_orbit;
        }

        protected float CalculateEccentricity(bool noExtreme)
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

        protected virtual float CalculateMass()
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

        protected RotationStrut CalculateRotation(float limit)
        {
            return new RotationStrut
            {
                x = UnityEngine.Random.Range(-limit, limit) * Mathf.Deg2Rad,
                y = 0f,
                z = UnityEngine.Random.Range(-limit, limit) * Mathf.Deg2Rad
            };
        }

    }
}