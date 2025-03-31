using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulesObjectsBuffer : MonoBehaviour
{
    [field: SerializeField] public GameObject Camera { get; private set; }
    [field: SerializeField] public GameObject Player { get; private set; }
}
