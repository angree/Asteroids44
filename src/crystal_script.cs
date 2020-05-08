using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystal_script : MonoBehaviour
{
    Global_Variables _GlobalVariables;
    GameObject GC;

    public AudioClip clip;
    public int crystal_type = 1;

    Rigidbody m_Rigidbody;

    int crystal_hp = 20;
    float existence_time = 25f;
    float min_speed = 2f;
    float max_speed = 7f;
    bool already_destroyed = false;
    bool sound_played = false;
    bool bomb_inactive = false;
    float explosion_life = 0.5f;
    int player_bomb_explosion = 0;

    void Start()
    {
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();

        min_speed = (2f + _GlobalVariables.level_number * 0f) * 1f;
        max_speed = (2f + (_GlobalVariables.level_number * 0f)) * 1.5f;

        transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        m_Rigidbody = GetComponent<Rigidbody>();
        if (crystal_type == 5) m_Rigidbody = transform.parent.GetComponent<Rigidbody>();
        m_Rigidbody.AddForce(transform.up * Random.Range(20f * 10f, 50f * 10f));
    }


    void OnTriggerEnter(Collider col)
    {
        int player = 0;
        if (col.transform.gameObject.GetComponent<Ship_dummy_01>() != null) player = 1;
        if (col.transform.gameObject.GetComponent<Ship_dummy_02>() != null) player = 2;
        if (col.transform.gameObject.GetComponent<Ship_dummy_03>() != null) player = 3;
        if (col.transform.gameObject.GetComponent<Ship_dummy_04>() != null) player = 4;
        if ((player > 0) && (already_destroyed == false))
        {
            if (sound_played == false)
            {
                AudioSource.PlayClipAtPoint(clip, _GlobalVariables.main_camera_pos);
                sound_played = true;
            }
            //Debug.Log("Is player 1 destroying "+crystal_type+" crystal!");
            if (crystal_type == 1) { _GlobalVariables.Energy_Player[player] += 25; }
            if (crystal_type == 2) { _GlobalVariables.Speed_Shot[player] = transform_shots(_GlobalVariables.Speed_Shot[player]); }
            if (crystal_type == 3)
            {
                float current_percentage = _GlobalVariables.Energy_Player[player] / _GlobalVariables.Max_Energy_Player[player];
                _GlobalVariables.Max_Energy_Player[player] += 5;
                _GlobalVariables.Energy_Player[player] = _GlobalVariables.Max_Energy_Player[player] * current_percentage;
            }
            if (crystal_type == 4) { _GlobalVariables.player_btc[player] += 1f; }
            if (crystal_type == 6) { _GlobalVariables.split_shot[player] += 1f; }
            if (crystal_type != 5) { Destroy(this.gameObject); already_destroyed = true; }
            else
            {
                explosion_life -= 0.01f;
                GameObject last_gameobject2;
                //explo at all 9 bomb colliders
                //(there are 9 explosions, because if main explosion is partly off screen, then another one will be visible on the other side of the screen)
                GameObject explo_temp;
                for (int i = 0; i <= 8; i++)
                {
                    explo_temp = transform.parent.transform.GetChild(i).gameObject;
                    last_gameobject2 = Instantiate(_GlobalVariables.bomb_explosion, explo_temp.transform.position, explo_temp.transform.rotation);
                    last_gameobject2.transform.parent = GameObject.Find("zero_object").transform;
                }

                transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                player_bomb_explosion = player;
                explosion_life -= Time.deltaTime;
                already_destroyed = true;
            }
        }
    }

    void Update()
    {
        if (crystal_type != 5) _GlobalVariables.asteroid_timer = 0f;

        if (crystal_type == 5)
        {
            if (explosion_life < 0.5f) explosion_life -= Time.deltaTime;
            if (explosion_life <= 0.49f)
            {
                //modify all 9 bomb colliders to be bound to current player
                for (int i = 0; i <= 8; i++)
                {
                    transform.parent.transform.GetChild(i).gameObject.SetActive(true);
                    transform.parent.transform.GetChild(i).gameObject.GetComponent<bomb_collider_dummy>().player_number = player_bomb_explosion;
                }
            }
            if (explosion_life <= 0f)
            {
                if (crystal_type != 5) Destroy(this.gameObject);
                else
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        if (_GlobalVariables.shop_2_phase != 5)
        {
            if (transform.localPosition.x < -107.5f) transform.localPosition = new Vector3(transform.localPosition.x + 215.0f, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x > 107.5f) transform.localPosition = new Vector3(transform.localPosition.x - 215.0f, transform.localPosition.y, transform.localPosition.z);

            if (transform.localPosition.y < -62.2f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 124.4f, transform.localPosition.z);
            if (transform.localPosition.y > 62.2f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 124.4f, transform.localPosition.z);
        }

        existence_time -= Time.deltaTime;
        if ((existence_time <= 6) && (existence_time > 3))
        {
            float existence_time_cycle = existence_time;
            while (existence_time_cycle > 0.3f) existence_time_cycle -= 0.3f;
            if (existence_time_cycle > 0.2f) transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            else transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        }
        if (existence_time <= 3)
        {
            float existence_time_cycle = existence_time;
            while (existence_time_cycle > 0.1f) existence_time_cycle -= 0.1f;
            if (existence_time_cycle > 0.05f) transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            else transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        }
        if (existence_time <= 0) Destroy(this.gameObject);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);

        float velocity_x = m_Rigidbody.velocity.x;
        float velocity_y = m_Rigidbody.velocity.y;
        float vel_x_abs = Mathf.Abs(velocity_x);
        float vel_y_abs = Mathf.Abs(velocity_y);

        if (vel_x_abs < (vel_y_abs * 0.2f)) vel_x_abs = vel_y_abs * 0.2f;
        if (vel_y_abs < (vel_x_abs * 0.2f)) vel_y_abs = vel_x_abs * 0.2f;

        if (velocity_x >= 0) velocity_x = vel_x_abs; else velocity_x = -vel_x_abs;
        if (velocity_y >= 0) velocity_y = vel_y_abs; else velocity_y = -vel_y_abs;

        float velocity_temp = Mathf.Sqrt((velocity_y * velocity_y) + (velocity_x * velocity_x));
        if (velocity_temp != 0)
        {
            float velocity_modifier = 1f;
            if (velocity_temp < min_speed) velocity_modifier = min_speed / velocity_temp;
            if (velocity_temp > max_speed) velocity_modifier = max_speed / velocity_temp;
            m_Rigidbody.velocity = new Vector3(velocity_x * velocity_modifier, velocity_y * velocity_modifier, 0f);
            velocity_x = m_Rigidbody.velocity.x;
            velocity_y = m_Rigidbody.velocity.y;
        }
    }

    private float transform_shots(float input)
    {
        float current_shots = 1 / input;
        current_shots += 0.5f;
        float returned = 1 / current_shots;
        return returned;
    }
}
