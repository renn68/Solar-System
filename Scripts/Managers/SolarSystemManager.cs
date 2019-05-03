using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.FastLineRenderer;
using Gemini120;

public class SolarSystemManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

        planetIDGameObjectMap = new Dictionary<int, GameObject>();
        moonIDGameObjectMap = new Dictionary<int, GameObject>();

        Generate();
	}

    SolarSystem solarsystem;
    Vector3[] planetLocs;
    Vector3[] moonLocs;
    public double PositionScale = 1; // 100 Billion
    public GameObject PlanetPrefab;
    public GameObject MoonPrefab;
    public GameObject ShipPrefab;

    public FastLineRenderer FastLinePlanetline;
    public FastLineRenderer FastLineMoonline;


    private float orbitLineRadius = 115f;
    private float orbitLineScale = 6f;

    Dictionary<int, GameObject> planetIDGameObjectMap;
    Dictionary<int, GameObject> moonIDGameObjectMap;

    public double EpochTime = 0f;
    public bool Paused = false;

    private Vector3[][] orbitsMatrixPlanets;
    private Vector3[][] orbitsMatrixMoons;
    private int segments = 256;
    private int segmentsMoons = 128;

    private uint TimeStep = 60 * 60 * 24;

    void Update ()
    {
        if (!Paused)
        {
            AdvanceTime(TimeStep * Time.deltaTime);
        }
    }


    private void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
  
    }


    public void AdvanceTime(float advanceTime = 60)
    {
        EpochTime += advanceTime;
        solarsystem.AdanceTime((ulong)EpochTime);
        solarsystem.Rotate();
        PlanetPosition();
        MoonPosition();
        UpdatePlanetRender();
        UpdateMoonRender();
        //CacheOrbitsMoons();
        //UpdateOrbitsMoons();
    }

    public void Generate()
    {
        solarsystem = new SolarSystem(1);
        solarsystem.Generate();
        planetLocs = new Vector3[solarsystem.Planets.Count];
        moonLocs = new Vector3[solarsystem.Moons.Count];
        // DebugSolarSystem();
        PlanetPosition();
        MoonPosition();
        DrawPlanets();
        DrawMoons();

        orbitsMatrixPlanets = new Vector3[solarsystem.Planets.Count][];
        orbitsMatrixMoons = new Vector3[solarsystem.Moons.Count][];


        CacheOrbits();
        RenderCachedOrbitsLR();
    }

    private void OnGUI()
    {
        // RenderCachedOrbitsGL();
    }

    private void DebugSolarSystem()
    {
        Debug.Log(solarsystem.Sun.Name);
        Debug.Log(solarsystem.Sun.Temperature);
        for (int i = 0; i < solarsystem.Planets.Count; i++)
        {
            Debug.Log(solarsystem.Planets[i].OrbitalID);
            Debug.Log(solarsystem.Planets[i].Orbit);
            Debug.Log(solarsystem.Planets[i].Position.x);
            Debug.Log(solarsystem.Planets[i].Position.y);
            Debug.Log(solarsystem.Planets[i].Position.z);

        }
    }

    private void UpdatePlanetRender()
    {
        for (int i = 0; i < planetLocs.Length; i++)
        {
            GameObject go = planetIDGameObjectMap[i];
            go.transform.position = planetLocs[i];
        }
    }

    private void UpdateMoonRender()
    {
        for (int i = 0; i < moonLocs.Length; i++)
        {
            GameObject go = moonIDGameObjectMap[i];
            go.transform.position = moonLocs[i];
        }
    }


    private void PlanetPosition()
    {
        for (int i = 0; i < solarsystem.Planets.Count; i++)
        {
            planetLocs[i] = solarsystem.Planets[i].GetPositionVector3();
        }
    }

    private void MoonPosition()
    {
        for (int i = 0; i < solarsystem.Moons.Count; i++)
        {
            moonLocs[i] = solarsystem.Moons[i].GetPositionVector3();
        }
    }

    private void DrawPlanets()
    {
        for (int i = 0; i < planetLocs.Length; i++)
        {
            GameObject go;
            go = Instantiate(PlanetPrefab, 
                planetLocs[i], 
                this.transform.rotation,
                this.transform);
            go.name = solarsystem.Planets[i].OrbitalID.ToString();
            planetIDGameObjectMap[i] = go;
        }
    }

    private void DrawMoons()
    {
        for (int i = 0; i < moonLocs.Length; i++)
        {
            GameObject go;
            go = Instantiate(MoonPrefab,
                moonLocs[i],
                this.transform.rotation,
                this.transform);
            go.name = solarsystem.Moons[i].Parent.OrbitalID.ToString() + 
                "-" + solarsystem.Moons[i].OrbitalID.ToString();
            moonIDGameObjectMap[i] = go;
        }
    }

    #region oldEllipse
    //private void RenderOrbitFL()
    //{
    //    FastLineOrbitLine.Reset();

    //    for (int i = 0; i < orbitsMatrixPlanets.GetLength(0); i++)
    //    {
    //        DrawEllipseFL(
    //            obitalIDGameObjectMap[i].transform,
    //            i,
    //            segments
    //            );
    //    }
    //}




    //public void DrawEllipseFL(
    //    Transform transformParent,
    //    int index, int segments)
    //{

    //    GameObject go = new GameObject();
    //    go.name = "orbit";
    //    go.transform.SetParent(transformParent);

    //    FastLineRenderer lr = FastLineRenderer.CreateWithParent(go, FastLineOrbitLine);
    //    lr.Reset();

    //    List<Vector3> points = new List<Vector3>();

    //    Debug.Log(orbitsMatrixPlanets[index, 0][0]);

    //    for (int i = 0; i < segments; i++)
    //    {
    //        points.Add(new Vector3(
    //            orbitsMatrixPlanets[index, i][0], 
    //            orbitsMatrixPlanets[index, i][1], 
    //            orbitsMatrixPlanets[index, i][2]));
    //    }
    //    points.Add(points[0]);

    //    FastLineRendererProperties props = new FastLineRendererProperties();

    //    props.Radius = 100f;
    //    props.Color = Color.white;
    //    props.LineJoin = FastLineRendererLineJoin.None;
    //    lr.AddLine(props, points, null);
    //    //lr.ScreenRadiusMultiplier = 0.2f;

    //    lr.Apply();


    //    //  go;
    //}
    //private void RenderOrbitFL()
    //{
    //    FastLineOrbitline.Reset();

    //    for (int i = 0; i < solarsystem.Planets.Count; i++)
    //    {
    //        DrawEllipseFL(
    //            obitalIDGameObjectMap[i].transform,
    //            1f,
    //            1f,
    //            (float)(solarsystem.Planets[i].Orbit / PositionScale)
    //            );
    //    }
    //}




    //public void DrawEllipseFL(
    //    Transform transformParent,
    //    float xAxis, float yAxis, float size, int segments = 64)
    //{

    //    GameObject go = new GameObject();
    //    go.name = "orbit";
    //    go.transform.SetParent(transformParent);

    //    FastLineRenderer lr = FastLineRenderer.CreateWithParent(go, FastLineOrbitline);

    //    List<Vector3> points = new List<Vector3>();

    //    for (int i = 0; i < segments; i++)
    //    {
    //        float[] p1 = CalculatePoint(xAxis, yAxis, size, (float)i / (float)(segments + 1));
    //        points.Add(new Vector3(p1[0], p1[1], p1[2]));
    //    }
    //    /// points.Add(points[0]);

    //    FastLineRendererProperties props = new FastLineRendererProperties();

    //    props.Radius = 1f;
    //    props.Color = Color.white;
    //    props.LineJoin = FastLineRendererLineJoin.Round;
    //    lr.AddLine(props, points, null);
    //    lr.ScreenRadiusMultiplier = 0.1f;

    //    lr.Apply();


    //    //  go;
    //}

    //public void RenderOrbitGL()
    //{
    //    // Debug.Log("post render");
    //    for (int i = 0; i < solarsystem.Planets.Count; i++)
    //    {
    //        DrawEllipseGL(
    //            obitalIDGameObjectMap[i].transform,
    //            solarsystem.Planets[i].xAxis,
    //            solarsystem.Planets[i].yAxis,
    //            (float)(solarsystem.Planets[i].Orbit / PositionScale)
    //            );
    //    }
    //}

    //private void DrawEllipseGL(
    //    Transform transformParent,
    //    float xAxis, float yAxis, float size, int segments = 128)
    //{
    //    OrbitLineMat.SetPass(0);
    //    GL.PushMatrix();
    //    // GL.LoadOrtho();
    //    GL.Begin(GL.LINE_STRIP);
    //    GL.Color(Color.white);
    //    for (int i = 0; i < segments + 1; i++)
    //    {
    //        float[] p1 = CalculatePoint(xAxis, yAxis, size, (float)i / (float)segments);
    //        // float[] p2 = CalculatePoint(xAxis, yAxis, size, (float)i + 1 / (float)segments);
    //        GL.Vertex3(p1[0], p1[1], p1[2]);
    //        // GL.Vertex3(p2[0], p2[1], p2[2]);
    //    }
    //    GL.End();
    //    GL.PopMatrix();
    //}

    //public float[] CalculatePoint(float xAxis, float yAxis, float size, float position)
    //{
    //    float angle = 2 * Mathf.PI * position;
    //    float x = Mathf.Sin(angle) * xAxis * size;
    //    float y = Mathf.Cos(angle) * yAxis * size;
    //    return new float[] { x, 0, y };
    //}

    //public void RenderCachedOrbitsGL()
    //{
    //    if (orbitsMatrixPlanets == null || orbitsMatrixMoons == null )
    //    {
    //        Debug.Log("Orbits not Cached");
    //    }

    //    OrbitLineMat.SetPass(0);

    //    for (int i = 0; i < orbitsMatrixPlanets.GetLength(0); i++)
    //    {
    //        GL.PushMatrix();
    //        GL.Begin(GL.LINE_STRIP);
    //        GL.Color(Color.white);
    //        for (int y = 0; y < orbitsMatrixPlanets.GetLength(1); y++)
    //        {
    //            // GL.Vertex3(orbitsMatrixPlanets[i,y][0], orbitsMatrixPlanets[i, y][1], orbitsMatrixPlanets[i, y][2]);
    //        }
    //        GL.End();
    //        GL.PopMatrix();
    //    }

    //    for (int i = 0; i < orbitsMatrixMoons.GetLength(0); i++)
    //    {
    //        GL.PushMatrix();
    //        GL.Begin(GL.LINE_STRIP);
    //        GL.Color(Color.white);
    //        for (int y = 0; y < orbitsMatrixMoons.GetLength(1); y++)
    //        {
    //            // GL.Vertex3(orbitsMatrixMoons[i, y][0], orbitsMatrixMoons[i, y][1], orbitsMatrixMoons[i, y][2]);
    //        }
    //        GL.End();
    //        GL.PopMatrix();
    //    }


    //}

    #endregion



    private void CacheOrbits()
    {


        for (int i = 0; i < solarsystem.Planets.Count; i++)
        {
            orbitsMatrixPlanets[i] = new Vector3[segments + 1];
        }
        for (int i = 0; i < solarsystem.Moons.Count; i++)
        {
            orbitsMatrixMoons[i] = new Vector3[segmentsMoons + 1];
        }

        CacheOrbitsPlanets();
        CacheOrbitsMoons();
    }


    private void CacheOrbitsPlanets()
    {
        for (int i = 0; i < solarsystem.Planets.Count; i++)
        {
            
            for (int y = 0; y < segments + 1; y++)
            {
                orbitsMatrixPlanets[i][y] = Ellipse.CalculatePointVector3(
                    ((float)y / (float)segments) + (float)solarsystem.Planets[i].CurrentPositionOrbit,
                    0f,
                    0f,
                    0f,
                    solarsystem.Planets[i].GetXAxisFociFloat(),
                    solarsystem.Planets[i].GetZAxisFociFloat(),
                    solarsystem.Planets[i].GetOrbitFloat(),
                    solarsystem.Planets[i].Rotation);
            }
        }
    }

    private void CacheOrbitsMoons()
    {
        for (int i = 0; i < solarsystem.Moons.Count; i++)
        {

            for (int y = 0; y < segmentsMoons + 1; y++)
            {
                orbitsMatrixMoons[i][y] = Ellipse.CalculatePointVector3(
                    ((float)y / (float)segmentsMoons) + (float)solarsystem.Moons[i].CurrentPositionOrbit,
                    0f,
                    0f,
                    0f,
                    solarsystem.Moons[i].GetXAxisFociFloat(),
                    solarsystem.Moons[i].GetZAxisFociFloat(),
                    solarsystem.Moons[i].GetOrbitFloat(),
                    solarsystem.Moons[i].Rotation);
            }
        }
    }

    public void RenderCachedOrbitsLR()
    {
        RenderPlanetOrbits();
        RenderMoonOrbits();

    }

    public void RenderPlanetOrbits()
    {

        for (int i = 0; i < orbitsMatrixPlanets.GetLength(0); i++)
        {
            FastLinePlanetline.Reset();
            GameObject go = new GameObject("Orbit");
            go.transform.SetParent(planetIDGameObjectMap[i].transform);
            go.transform.localPosition = new Vector3(0f, 0f, 0f);

            FastLineRenderer lr = FastLineRenderer.CreateWithParent(go, FastLinePlanetline);

            FastLineRendererProperties props = new FastLineRendererProperties();

            props.Radius = orbitLineRadius;
            props.LineJoin = FastLineRendererLineJoin.AttachToPrevious;
            lr.UseWorldSpace = true;
            // props.Color = new Color(0.67f, 0.66f, 0.50f, 0.25f);
            lr.AddLine(props, orbitsMatrixPlanets[i], null);
            lr.ScreenRadiusMultiplier = orbitLineScale;

            lr.Apply();
        }

        //for (int i = 0; i < orbitsMatrixPlanets.GetLength(0); i++)
        //{
        //    GameObject go = new GameObject("Orbit");
        //    go.transform.SetParent(planetIDGameObjectMap[i].transform);
        //    go.transform.localPosition = new Vector3(0f, 0f, 0f);


        //    LineRenderer lr = Instantiate<LineRenderer>(LineOrbits, go.transform);
        //    lr.enabled = true;

        //    lr.name = "Orbit-" + solarsystem.Planets[i].OrbitalID;
        //    orbitsPlanets.Add(lr);
        //    lr.startWidth = 1f;
        //    lr.endWidth = 1f;
        //    lr.positionCount = segments;
        //    lr.SetPositions(orbitsMatrixPlanets[i]);
        //}
    }

    public void RenderMoonOrbits()
    {

        //for (int i = 0; i < orbitsMatrixMoons.GetLength(0); i++)
        //{
        //     LineRenderer lr = Instantiate<LineRenderer>(
        //         LineOrbits, planetIDGameObjectMap[solarsystem.Moons[i].Parent.OrbitalID].transform);
        //    lr.enabled = enable;
        //    lr.useWorldSpace = false;
        //    lr.name = "Orbit-" + solarsystem.Moons[i].Parent.OrbitalID.ToString() + 
        //        "-" + solarsystem.Moons[i].OrbitalID.ToString();
        //    orbitsMoons.Add(lr);
        //    lr.startWidth = 60f;
        //    lr.endWidth = 60f;
        //    lr.startColor = moonsOrbitColour;
        //    lr.endColor = moonsOrbitColour;
        //    lr.positionCount = segmentsMoons;
        //    lr.SetPositions(orbitsMatrixMoons[i]);
        //}

        for (int i = 0; i < orbitsMatrixMoons.GetLength(0); i++)
        {
            FastLineMoonline.Reset();
            GameObject go = new GameObject("Orbit-Moon");
            go.transform.SetParent(planetIDGameObjectMap[solarsystem.Moons[i].Parent.OrbitalID].transform);
            go.transform.localPosition = new Vector3(0f, 0f, 0f);

            FastLineRenderer lr = FastLineRenderer.CreateWithParent(go, FastLineMoonline);

            FastLineRendererProperties props = new FastLineRendererProperties();

            props.Radius = orbitLineRadius;
            props.LineJoin = FastLineRendererLineJoin.AttachToPrevious;
            lr.UseWorldSpace = false;
            // props.Color = new Color(0.51f, 0.67f, 0.50f, 0.25f);
            lr.AddLine(props, orbitsMatrixMoons[i], null);
            lr.ScreenRadiusMultiplier = orbitLineScale;

            lr.Apply();
        }
    }




}

