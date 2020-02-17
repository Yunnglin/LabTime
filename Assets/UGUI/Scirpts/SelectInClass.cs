using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectInClass : MonoBehaviour
{

    public GameObject Main;
    public GameObject Menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Selected()
    {
        Menu.SetActive(true);
        Main.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
