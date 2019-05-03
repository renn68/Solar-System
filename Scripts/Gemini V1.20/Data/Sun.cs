using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gemini120
{

    public class Sun : Orbital
    {

        public Sun(string name)
        {
            Name = name;
            Position = new PositionStrut { x = 0f, y = 0f, z = 0f };
            Rotation = new RotationStrut { x = 0f, y = 0f, z = 0f };

        }

        public string Name;

        public float Temperature { get; private set; }
        public double Radius { get; private set; }

        private readonly float minStandardSizeSun = 0.1f;
        private readonly float maxStandardSizeSun = 25f;
        private readonly float minGiantSizeSun = 20f;
        private readonly float maxGiantSizeSun = 80f;
        private readonly float minDwarfSizeSun = 0.01f;
        private readonly float maxDwarfSizeSun = 0.12f;

        // private readonly double baseSizeSun = 700000;
        private readonly float[] sunRadiusChance = { 0.2f, 0.8f };



        public void Generate()
        {

            OrbitalID = 0;
            Radius = sunRadius();
            Mass = sunMass((float)Radius);

        }

        private int sunTemp(float size)
        {
            if (size < 1f)
            {
                return UnityEngine.Random.Range(1000, 3800);
            }
            if (size < 25f)
            {
                return UnityEngine.Random.Range(3300, 7500);
            }
            if (UnityEngine.Random.Range(0f, 1f) > 0.35f)
            {
                return UnityEngine.Random.Range(3000, 7500);

            }
            return UnityEngine.Random.Range(7500, 100000);
        }

        private float sunRadius()
        {
            float radiusChance = UnityEngine.Random.Range(0f, 1f);


            if (radiusChance < sunRadiusChance[0])
            {
                return UnityEngine.Random.Range(minDwarfSizeSun, maxDwarfSizeSun);
            }

            if (radiusChance < sunRadiusChance[1])
            {
                return UnityEngine.Random.Range(minStandardSizeSun, maxStandardSizeSun);
            }

            return UnityEngine.Random.Range(minGiantSizeSun, maxGiantSizeSun);

        }

        private float sunMass(float radius)
        {
            return UnityEngine.Random.Range(radius * 0.8f, radius * 1.2f);
        }
    }
}
