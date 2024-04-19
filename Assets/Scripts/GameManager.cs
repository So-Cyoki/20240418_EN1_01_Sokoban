using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int[] map;
    int arryIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        map = new int[] { 1, 0, 0, 0, 0 };
    }

    // Update is called once per frame
    void Update()
    {

    if(Input.GetKeyDown(KeyCode.UpArrow)){
            int temp = map[arryIndex];
            map[arryIndex] = map[arryIndex+1];
            map[arryIndex+1]=temp;
            arryIndex++;
    }
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ",";
        }
        Debug.Log(debugText);
    }
}
