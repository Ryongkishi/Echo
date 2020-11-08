using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlHuman : BaseHuman
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if(Input.GetMouseButton(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hits;
            Physics.Raycast(ray,out hits);
            if(hits.collider.tag == "Terrain")
            {
                MoveTo(hits.point);
                NetManaager.Send("Enter|127.1.1.1,200,300,400,45");
            }
        }
    }
}
