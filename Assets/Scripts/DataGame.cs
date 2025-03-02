using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGame : MonoBehaviour
{
    public static DataGame Instance;
    public DataCharacter dataCharacter;
    void Awake()
    {
        Instance = this;
    }
}
