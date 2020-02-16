using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectPhysics : MonoBehaviour
{

    public GameObject phy;
    public GameObject Menu;
    public GameObject chem;
    public GameObject Main;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChemSelected()
    {
        chem.SetActive(true);
        Menu.SetActive(false);

    }

    public void Selected()
    {    
        phy.SetActive(true);
        Menu.SetActive(false);

    }
    public void Return()
    {
        Main.SetActive(true);
        Menu.SetActive(false);

    }

    public void ReturnInC()
    {
        Menu.SetActive(true);
        phy.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
