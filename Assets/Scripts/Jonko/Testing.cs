using Jonko;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Start()
    {
        FunctionTimer.Create(() => Debug.Log("Whew"), 2f);        
    }
}
