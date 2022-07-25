using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveInfo : MonoBehaviour
{
    [SerializeField] Text usage;
    [SerializeField] Text type;

    public void setUsage(string currentUsage, string maxUsage){
        usage.text = currentUsage + "/" + maxUsage;
    }
    public void setType(string typeString){
        type.text = typeString;
    }
}
