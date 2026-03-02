// This script loops the side props (shelves and desk) 
//to create an endless environment effect.
using UnityEngine;

public class SidePropLooper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform runner;            // player or main camera
    [SerializeField] private Transform[] props;           // shelves + desk (4 items)

    [Header("Loop Settings")]
    [SerializeField] private float segmentLength = 40f;   // distance between repeating positions (tune this)
    [SerializeField] private float recycleBehind = 15f;   // how far behind runner before recycling

    private int propCount;

    private int Start()
    {
        // If runner not set, try to find the main camera
        if (runner == null)
        {
            runner = Camera.main != null ? Camera.main.transform : null;
        }

        propCount = props != null ? props.Length : 0;

        if (runner == null || propCount == 0)
        {
            Debug.LogError("SidePropLooper: Missing runner or props.");
            return -1;
        }

        return 0;
    }

    private void Update()
    {
        // Safety checks
        if (runner == null || propCount == 0) return;

        for (int i = 0; i < propCount; i++)
        {
            Transform t = props[i];
            if (t == null) continue;

            // If the prop is sufficiently behind the runner, move it forward
            float dz = runner.position.z - t.position.z;
            if (dz > recycleBehind) // Time to recycle
            {
                // Move this prop forward by segmentLength * propCount to loop it
                Vector3 p = t.position;
                p.z += segmentLength * propCount;
                t.position = p;
            }
        }
    }
}