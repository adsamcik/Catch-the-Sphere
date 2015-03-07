using UnityEngine;
using System.Collections;

public class spawned : MonoBehaviour
{
    public int Spawned;
    void Update()
    {
        GetComponent<TextMesh>().text = "Spawned " + Spawned + " spheres";
    }
}
