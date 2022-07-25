using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Type.Weakness(Element.Water, Element.Fire));
        Debug.Log(Type.Weakness(Element.Grass, Element.Fire));
        Debug.Log(Type.Weakness(Element.Fire, Element.Fire));
    }

}
