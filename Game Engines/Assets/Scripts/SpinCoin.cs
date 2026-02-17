using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class SpinCoin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, Space.World);
    }
}
