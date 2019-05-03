using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemini120
{
    public class SolarSystem
    {
        public SolarSystem(int id)
        {
            SolarSystemID = id;
            Planets = new List<Planet>();
            Moons = new List<Moon>();
            Ships = new List<Ship>();

        }

        public int SolarSystemID { get; private set; }
        public Sun Sun;
        public List<Planet> Planets;
        public List<Moon> Moons;
        public List<Ship> Ships;
        public ulong EpochTime = 0;

        private double orbitsVariance = 0.2f;
        private double[] firstOrbits = { 0.5f, 1f, 2f, 4f };
        private double orbitBase = 2.0f;

        //private ulong[] moonsOrbits = { 20000000, 2000000000, 500000000, 700000000, 40000000, // 20 mill, , 2bill, 500mill, 700mill, 4bill
        //    10000000, 30000000, 50000000, 70000000, 900000000, // 20mill, 40mill,60mill,80mill,100mill,
        //    };

        private readonly double[] moonsOrbit = { 0.001, 0.005, 0.001, 0.003, 0.05, 0.01, 0.05, 0.07, 0.1, 0.03 };
        // private readonly double[] moonsOrbit = { 0.001};



        private float moonChance = 0.02f;
        private float moonChanceAdditional = 0.2f;
        private int moonLimit = 20;


        public void Generate()
        {
            Sun = new Sun("Sol");
            Sun.Generate();
            GeneratePlanets();
            GenerateMoons();

        }

        private void GeneratePlanets()
        {
            int _count = 0;
            for (int i = 0; i < firstOrbits.Length; i++)
            {
                Planets.Add(new Planet(_count, Sun));
                Planets[_count].Generate(firstOrbits[i] * (1f - orbitsVariance), 
                    firstOrbits[i] * (1f + orbitsVariance), 10f, true);
                _count++;
            }

            bool previousOrbit = false;
            for (int i = 3; i < 20; i++)
            {
                if (Random.Range(0f, 1f) > 0.2f && !previousOrbit)
                {
                    Planets.Add(new Planet(_count, Sun));
                    Planets[_count].Generate(i * orbitBase * (1f - orbitsVariance), 
                        (i * orbitBase * (1f + orbitsVariance)));
                    _count++;
                    previousOrbit = true;
                }
                else
                {
                    previousOrbit = false;
                }

            }

        }

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
                        Moons.Add(new Moon(y, Planets[i]));
                        if (y > 9)
                        {
                            float _orbit = Random.Range(0.001f, 0.1f);
                            Moons[_count].Generate(
                                (0.8f * _orbit),
                                (1.2f * _orbit), 25f);
                            _count++;
                            continue;
                        }
                        Moons[_count].Generate(0.8f * moonsOrbit[y],
                            1.2f * moonsOrbit[y], 55f);
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
            foreach (var ship in Ships)
            {
                if (ship.Moving)
                {
                    ship.UpdatePosition(EpochTime);
                }
            }
        }
        
    }
}
