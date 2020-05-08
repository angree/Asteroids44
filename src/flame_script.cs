using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flame_script : MonoBehaviour
{
    Global_Variables _GlobalVariables;
    GameObject GC;

    public int player_number = 1;
    public int flame_number = 1;

    int last_flame_exist = 0;
    //0 - flame/smoke off
    //1 - smoke on
    //2 - flame on

    void Start()
    {
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();
    }

    void destroy_flame()
    {
        if (last_flame_exist > 0)
        {
            GameObject child_gameobject = this.gameObject.transform.GetChild(0).gameObject;
            Destroy(child_gameobject);
            last_flame_exist = 0;
        }
    }

    void Update()
    {
        if (((_GlobalVariables.Energy_Player[player_number] / _GlobalVariables.Max_Energy_Player[player_number]) <= 0.15) && ((_GlobalVariables.Energy_Player[player_number] / _GlobalVariables.Max_Energy_Player[player_number]) > 0) && (_GlobalVariables.player_active[player_number] == true) && (_GlobalVariables.Player_Flame_Number[player_number] == flame_number))
        {
            //create flame (2)
            if (last_flame_exist == 1)
            {
                destroy_flame();
            }
            if (last_flame_exist == 0)
            {
                GameObject last_gameobject = Instantiate(_GlobalVariables.ship_fire, transform.position, transform.rotation);
                last_gameobject.transform.parent = this.gameObject.transform;
                Vector3 scaleChange = new Vector3(1f, 1f, 1f);
                last_gameobject.transform.localScale = scaleChange;
                last_flame_exist = 2;
            }
        }
        else if (((_GlobalVariables.Energy_Player[player_number] / _GlobalVariables.Max_Energy_Player[player_number]) <= 0.3) && ((_GlobalVariables.Energy_Player[player_number] / _GlobalVariables.Max_Energy_Player[player_number]) > 0.15) && (_GlobalVariables.player_active[player_number] == true) && (_GlobalVariables.Player_Flame_Number[player_number] == flame_number))
        {
            //create smoke (1)
            if (last_flame_exist == 2)
            {
                destroy_flame();
            }
            if (last_flame_exist == 0)
            {
                GameObject last_gameobject = Instantiate(_GlobalVariables.ship_smoke, transform.position, transform.rotation);
                last_gameobject.transform.parent = this.gameObject.transform;
                Vector3 scaleChange = new Vector3(1f, 1f, 1f);
                last_gameobject.transform.localScale = scaleChange;
                last_flame_exist = 1;
            }
        } 
        else
        {
            destroy_flame();
        }
    }
}
