using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TerroristSpotter : MonoBehaviour {

    /// <summary>
    /// Position de départ du raycast par rapport à l'objet
    /// </summary>
    public Vector3 RaycastStart;

    /// <summary>
    /// Angle de vue (en degrés)
    /// </summary>
    public float ViewAngle;

    /// <summary>
    /// Gameobject du joueur
    /// </summary>
    public GameObject Player;

    /// <summary>
    /// Evenement déclenché lorsque le joueur est repéré
    /// </summary>
    [SerializeField]
    public UnityEvent<GameObject> OnSpotted;

    /// <summary>
    /// Evenement déclenché lorsque le joueur n'est plus visible
    /// </summary>
    [SerializeField]
    public UnityEvent<GameObject> OnLost;

    // Indique si le joueur est visible par le terroriste
    private bool _spotted = true;

    /// <summary>
    /// Indique si le joueur est visible par le terroriste
    /// </summary>
    public bool Spotted { get { return _spotted; } }

    /// <summary>
    /// Affiche le raycast
    /// </summary>
    public bool DisplayRaycast;

    // Update is called once per frame
    void Update () {
        Vector3 rayStart = transform.position + transform.rotation * RaycastStart;
        RaycastHit[] hits;
        Ray ray = new Ray(rayStart, Player.transform.position - rayStart);
        bool collided = false;
        if (Vector3.Angle(transform.forward, Player.transform.position - rayStart) <= ViewAngle && (hits = Physics.RaycastAll(ray)).Length > 0)
        {
            List<RaycastHit> tmp = new List<RaycastHit>(hits);
            tmp.Sort((a, b) => (rayStart - a.point).magnitude - (rayStart - b.point).magnitude <= 0f ? -1 : 1);
            foreach (RaycastHit r in tmp)
            {
                if (r.collider.tag != "IgnoreRaycast")
                {
                    if (r.collider.gameObject == Player)
                    {
                        collided = true;
                        if (!_spotted)
                        {
                            _spotted = true;
                            if (OnSpotted != null)
                                OnSpotted.Invoke(gameObject);
                            Debug.Log("Spotted!");
                        }
                    }
                    break;
                }
            }
        }
        if (!collided && _spotted)
        {
            _spotted = false;
            if (OnLost != null)
                OnLost.Invoke(gameObject);
            Debug.Log("Lost!");
        }
        if (DisplayRaycast)
            DrawLine(transform.position + RaycastStart, Player.transform.position, Color.red, Time.deltaTime);
    }

    // Material for group displaying
    private static Material lineMaterial = null;

    // lineMaterial accessor
    public static Material LineMaterial
    {
        get
        {
            CreateLineMaterial();
            return lineMaterial;
        }
    }

    // Draw the patch group
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f, float width = 0.1f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = LineMaterial;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    // Create the matrial
    private static void CreateLineMaterial()
    {
        if (lineMaterial == null)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }
}
