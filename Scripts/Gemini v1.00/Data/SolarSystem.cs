using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemini100
{
    public class SolarSystem
    {
        public SolarSystem(int id)
        {
            SolarSystemID = id;
            Planets = new List<Planet>();
            Moons = new List<Planet>();
            Ships = new List<Ship>();

        }

        public int SolarSystemID { get; private set; }
        public int PlanetMin = 2;
        public int PlanetMax = 6;
        public List<Planet> Planets;
        public List<Planet> Moons;
        public List<Ship> Ships;
        public ulong EpochTime = 0;

        public void Generate()
        {

            GenerateSun();
            GeneratePlanets();
            GenerateMoons();
            // DebugMoons();

        }

        private void DebugMoons()
        {
            foreach (var moon in Moons)
            {
                Debug.Log(moon.OrbitTime);
                Debug.Log(moon.Orbit);
            }
        }


        private ulong[] firstOrbits = { 30000000000, 50000000000, 70000000000, 90000000000 };
        private ulong tenBill = 10000000000;
        private const ulong orbitBase = 50000000000; // 50 billion

        private void GeneratePlanets()
        {
            int _count = 0;
            for (int i = 0; i < firstOrbits.Length; i++)
            {
                Debug.Log(i);
                Planets.Add(new Planet(_count));
                Planets[_count].Generate(firstOrbits[i] - tenBill, firstOrbits[i] + tenBill, 0, 0, true);
                _count++;
            }

            for (int i = 1; i < 30; i++)
            {
                if (Random.Range(0f, 1f) > 0.2f)
                {
                    Debug.Log(i);
                    Planets.Add(new Planet(_count));
                    Planets[_count].Generate((ulong)i * orbitBase, (ulong)(i + 1) * orbitBase, 0, 0);
                    _count++;
                }

            }

        }

        private float moonChance = 0.2f;
        private float moonChanceAdditional = 0.2f;
        private int moonLimit = 20;

        private ulong[] moonsOrbits = { 20000000, 2000000000, 500000000, 700000000, 40000000, // 1bill, 2bill, 500mill, 700mill, 4bill
            10000000, 30000000, 50000000, 70000000, 900000000, // 20mill, 40mill,60mill,80mill,100mill,
            };

        private void GenerateMoons()
        {
            int _count = 0;

            for (int i = 0; i < Planets.Count; i++)
            {
                int moonCount = 0;
                for (int y = 0; y < moonLimit; y++)
                {
                    if (i < 2 && y > 4)
                    {
                        break;
                    }
                    float _moonChance = (moonChance * (1 + (moonCount * moonChanceAdditional)));
                    if (Random.Range(0f, 1f) > _moonChance)
                    {
                        Moons.Add(new Planet(y, Planets[i]));
                        if (y > 9)
                        {
                            Moons[_count].GenerateMoon(
                                (ulong)(0.8f * (10000000 * (ulong)Random.Range(10, 1000))),
                                (ulong)(1.2f * (10000000 * (ulong)Random.Range(10, 1000))),
                            Planets[i].Position[0], Planets[i].Position[1], Planets[i].Mass);
                            _count++;
                            continue;
                        }
                        Moons[_count].GenerateMoon((ulong)0.8 * moonsOrbits[y],
                            (ulong)1.2 * moonsOrbits[y], 
                            Planets[i].Position[0], Planets[i].Position[1], Planets[i].Mass);
                        _count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void AdanceTime(ulong time)
        {
            EpochTime = time;
        }

        public void Rotate()
        {
            foreach (var planet in Planets)
            {
                planet.UpdatePosition(EpochTime);
            }
            foreach (var moon in Moons)
            {
                moon.UpdatePosition(EpochTime);
            }
            //foreach (var ship in Ships)
            //{
            //    if (ship.Moving)
            //    {
            //        ship.UpdatePosition(EpochTime);
            //    }
            //}
        }
        
        // Sun Stats Earth, radius 700,000km, size 1.7e - 30
        
        private readonly float minStandardSizeSun = 0.1f;
        private readonly float maxStandardSizeSun = 25f;
        private readonly float minGiantSizeSun = 20f;
        private readonly float maxGiantSizeSun = 80f;
        private readonly float minDwarfSizeSun = 0.01f;
        private readonly float maxDwarfSizeSun = 0.12f;
        private readonly ulong baseSizeSun = 700000;
        private readonly float[] sunRadiusDist = { 0.2f, 0.8f };


        public struct SunData
        {
            public float SizeFactor;
            public ulong Radius;
            public float Mass;
            public int Temperature;
            public string name;
        }

        public SunData Sun;


        public void GenerateSun()
        {
            Sun = new SunData
            {
                SizeFactor = sunRadius(),
                Radius = (ulong)(baseSizeSun * Sun.SizeFactor),
                Mass = sunMass(Sun.SizeFactor),
                Temperature = sunTemp(Sun.SizeFactor),
                name = "Sun-" + SolarSystemID.ToString()
            };

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


            if (radiusChance < sunRadiusDist[0]) {
                return UnityEngine.Random.Range(minDwarfSizeSun, maxDwarfSizeSun);
            }

            if (radiusChance < sunRadiusDist[1])
            {
                return UnityEngine.Random.Range(minStandardSizeSun, maxStandardSizeSun);
            }

            return UnityEngine.Random.Range(minGiantSizeSun, maxGiantSizeSun);

        }

        private float sunMass(float radius)
        {
            return UnityEngine.Random.Range(radius*0.8f, radius*1.2f);
        }


    }
}
