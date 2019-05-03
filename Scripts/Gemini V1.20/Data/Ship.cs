using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gemini120
{
    public class Ship : Orbital
    {

        public long[] StartPosition;
        public long[] DestiPosition;
        public uint Fuel;
        public uint CriuseSpeed;
        public uint MaxSpeed;
        public Planet PlanetLocation;
        public Planet StartPlanet;
        public Planet DestiPlanet;
        public long TravelDistance;
        public float TravelProgress;
        public double TravelTime;
        public bool Moving = false;

        public void SetDesti(Planet planet)
        {
            if (PlanetLocation != null)
            {
                StartPlanet = PlanetLocation;
            }

            DestiPlanet = planet;
            CalculateRoute();
        }

        public void CalculateRoute()
        {
            //StartPosition = StartPlanet.Position;
            //DestiPosition = DestiPlanet.Position;

            //long distance = Ellipse.DistanceBetween2Points(StartPosition[0], StartPosition[1], 
            //    DestiPosition[0], DestiPosition[1]);

            //double travelTime = distance / (double)CriuseSpeed;



            //TravelProgress = 0f;
            //Moving = true;
        }

        public void UpdatePosition(ulong time)
        {

        }

    }


}
