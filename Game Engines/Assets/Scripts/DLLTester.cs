using UnityEngine;
using DLLGameCore;

public class DLLTester : MonoBehaviour
{
    private int Start()
    {
        // Unity creates MonoBehaviours via AddComponent
        CoreMechanics core = gameObject.AddComponent<CoreMechanics>();

        Debug.Log(core.Add(21, 4));
        Debug.Log(core.Subtract(21, 4));
        Debug.Log(core.Multiply(21, 4));
        Debug.Log(core.Divide(21, 4));

        return 0;
    }
}