using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gemini120
{
    public class Moon : Planet
    {
        public Moon(int id, Planet parent) : base(id, parent)
        {
            OrbitalID = id;
            Parent = parent;
            xAxisFoci = new double[2];
            zAxisFoci = new double[2];
        }

        protected new float massScale = 0.01f;

    }
}
