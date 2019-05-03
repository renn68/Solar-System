using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemini120
{
    public static class GalaxyConfig
    {
        // config settings
        public static int GalaxySize = 10;
    }

    public class Galaxy
    {
        public Galaxy()
        {
            SolarSystems = new List<SolarSystem>();
        }

        private List<SolarSystem> SolarSystems;


        public void Generate()
        {

            // UnityEngine.Random.InitState(6801);

            for (int i = 0; i < GalaxyConfig.GalaxySize; i++)
            {
                SolarSystems.Add(new SolarSystem(i));
            }

        }

    }
}
