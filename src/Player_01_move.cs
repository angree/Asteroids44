using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_01_move : MonoBehaviour
{
    Global_Variables _GlobalVariables;
    GameObject GC;
    GameObject last_gameobject;
    General_settings _General_settings;
    GameObject GS;

    public GameObject Front_Thrusters;
    public GameObject Back_Thrusters;
    public GameObject projectile;
    public GameObject Explosion_anim;
    public AudioClip clip_explosion;
    public AudioClip clip_hit;
    public int player_number = 0;

    [HideInInspector]
    public int my_energy = 100;

    AudioSource audioData;
    Rigidbody m_Rigidbody;
    Vector3 m_EulerAngleVelocity_left;
    Vector3 m_EulerAngleVelocity_right;

    float rotation_speed = 200;
    float acceleration_speed = 6;
    float break_speed = 2;
    float last_shot = 1000f;
    float velocity_x = 0f;
    float velocity_y = 0f;

    bool full_game_started = false;
    int joypad_active = 0;
    int current_preset = 0;
    int keyboard_set_active = 0;

    void Start()
    {

        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
        audioData.Pause();

        GS = GameObject.Find("Settings_holder");
        if (GS != null) {
            _General_settings = GS.GetComponent<General_settings>();
            full_game_started = true;
        }

        if(full_game_started)
        {
            current_preset = 0;
            current_preset = _General_settings.player_controller_scheme[player_number];
            
            if (current_preset <= 4)
            {
                joypad_active = current_preset;
            }

            if (current_preset == 5)
            {
                keyboard_set_active = 1;
            }
            if (current_preset == 6)
            {
                keyboard_set_active = 2;
            }
            if (current_preset == 7)
            {
                keyboard_set_active = 3;
            }
        }
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();

        //Set the axis the Rigidbody rotates in (100 in the y axis)
        m_EulerAngleVelocity_left = new Vector3(0, 0, rotation_speed);
        m_EulerAngleVelocity_right = new Vector3(0, 0, -rotation_speed);

        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();

        Front_Thrusters.gameObject.SetActive(false);
        Back_Thrusters.gameObject.SetActive(false);
        if ((player_number == 1) && (_GlobalVariables.player_active[1] == false)) this.gameObject.SetActive(false);
        if ((player_number == 2) && (_GlobalVariables.player_active[2] == false)) this.gameObject.SetActive(false);
        if ((player_number == 3) && (_GlobalVariables.player_active[3] == false)) this.gameObject.SetActive(false);
        if ((player_number == 4) && (_GlobalVariables.player_active[4] == false)) this.gameObject.SetActive(false);

    }

    float last_hit = 10f;
    float last_hit_player = 10f;
    bool last_out_of_hp = false;
    void OnTriggerEnter(Collider col)
    {
        bool out_of_hp = false;
        //hit by asteroid
        if ((col.transform.gameObject.GetComponent<Asteroid_script>() != null) && (last_hit >= 0.2f))
        {
            float asteroid_x_velocity = col.GetComponent<Asteroid_script>().velocity_x;
            float asteroid_y_velocity = col.GetComponent<Asteroid_script>().velocity_y;
            float asteroid_size = col.GetComponent<Asteroid_script>().asteroid_size;
            float x_velocity_difference = asteroid_x_velocity - velocity_x;
            float y_velocity_difference = asteroid_y_velocity - velocity_y;
            float velocity_temp = Mathf.Sqrt((y_velocity_difference * y_velocity_difference) + (x_velocity_difference * x_velocity_difference));
            //hard level
            float player_hit_value = (velocity_temp / 9) * asteroid_size;
            //easy level
            //float player_hit_value = (velocity_temp / 12) * asteroid_size;

            if (player_number > 0) _GlobalVariables.Energy_Player[player_number] -= player_hit_value; if (_GlobalVariables.Energy_Player[player_number] <= 0) out_of_hp = true;
            last_hit = 0f;
            if (out_of_hp == false) AudioSource.PlayClipAtPoint(clip_hit, _GlobalVariables.main_camera_pos);
        } else
        {
            //hit by something else
            if(last_hit_player >= 0.1f)
            {
                int player = 0;
                if (col.transform.gameObject.GetComponent<Ship_dummy_01>() != null) player = 1;
                if (col.transform.gameObject.GetComponent<Ship_dummy_02>() != null) player = 2;
                if (col.transform.gameObject.GetComponent<Ship_dummy_03>() != null) player = 3;
                if (col.transform.gameObject.GetComponent<Ship_dummy_04>() != null) player = 4;
                if (player > 0)
                {
                    AudioSource.PlayClipAtPoint(clip_hit, _GlobalVariables.main_camera_pos);
                    if (player_number > 0) _GlobalVariables.Energy_Player[player_number] -= 1f; if (_GlobalVariables.Energy_Player[player_number] <= 0) out_of_hp = true;
                }
                last_hit_player = 0f;
            }
        }

        if ((out_of_hp==true)&&(last_out_of_hp==false))
        {
            AudioSource.PlayClipAtPoint(clip_explosion, _GlobalVariables.main_camera_pos);
            GameObject last_gameobject = Instantiate(Explosion_anim, transform.position, transform.rotation);
            transform.localPosition = new Vector3(transform.localPosition.x - 5000.0f, transform.localPosition.y, transform.localPosition.z);
            Front_Thrusters.gameObject.SetActive(false);
            Back_Thrusters.gameObject.SetActive(false);
            audioData.Pause();
        }
        last_out_of_hp = out_of_hp;

    }

    int this_key_pressed = 0;
    int last_key_pressed = 0;

    float dead_zone = 0.3f;

    void FixedUpdate()
    {
        //CHECK IF PLAYER IS NOT DEAD
        bool out_of_hp = false;
        switch (player_number)
        {
            case 1: if (_GlobalVariables.Energy_Player[1] <= 0) out_of_hp = true; break;
            case 2: if (_GlobalVariables.Energy_Player[2] <= 0) out_of_hp = true; break;
            case 3: if (_GlobalVariables.Energy_Player[3] <= 0) out_of_hp = true; break;
            case 4: if (_GlobalVariables.Energy_Player[4] <= 0) out_of_hp = true; break;
        }
        if ((out_of_hp == true) && (last_out_of_hp == false))
        {
            AudioSource.PlayClipAtPoint(clip_explosion, _GlobalVariables.main_camera_pos);
            GameObject last_gameobject = Instantiate(Explosion_anim, transform.position, transform.rotation);
            transform.localPosition = new Vector3(transform.localPosition.x - 5000.0f, transform.localPosition.y, transform.localPosition.z);
            Front_Thrusters.gameObject.SetActive(false);
            Back_Thrusters.gameObject.SetActive(false);
            audioData.Pause();
        }

        float speed = GetComponent<Rigidbody>().velocity.magnitude;

        bool allow_move_after_reappearing_already = false;
        if (_GlobalVariables.shop_time == 2) {
            if ((_GlobalVariables.shop_2_phase > 7) && (player_number == 1)) allow_move_after_reappearing_already = true;
            if ((_GlobalVariables.shop_2_phase > 8) && (player_number == 2)) allow_move_after_reappearing_already = true;
            if ((_GlobalVariables.shop_2_phase > 9) && (player_number == 3)) allow_move_after_reappearing_already = true;
            if ((_GlobalVariables.shop_2_phase > 10) && (player_number == 4)) allow_move_after_reappearing_already = true;
        }

        //LOCK PLAYER INSIDE OF THE VISIBLE SPACE (with some margins)
        if(((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already))&&(out_of_hp==false))
        {
            if (transform.localPosition.x < -107.5f) transform.localPosition = new Vector3(transform.localPosition.x + 215.0f, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x > 107.5f) transform.localPosition = new Vector3(transform.localPosition.x - 215.0f, transform.localPosition.y, transform.localPosition.z);

            if (transform.localPosition.y < -62.2f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 124.4f, transform.localPosition.z);
            if (transform.localPosition.y > 62.2f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 124.4f, transform.localPosition.z);
        }

        //control aggregation
        if(_GlobalVariables.android==0)
        {
            if (joypad_active == 0) //KEYBOARD
            {
                //Debug.Log("keyboard detected");
                if (_General_settings.keyboard_x[keyboard_set_active] < -0.5f) _GlobalVariables.press_left[player_number] = 1f; else _GlobalVariables.press_left[player_number] = 0f;
                if (_General_settings.keyboard_x[keyboard_set_active] > 0.5f) _GlobalVariables.press_right[player_number] = 1f; else _GlobalVariables.press_right[player_number] = 0f;
                if (_General_settings.keyboard_y[keyboard_set_active] < -0.5f) _GlobalVariables.press_down[player_number] = 1f; else _GlobalVariables.press_down[player_number] = 0f;
                if (_General_settings.keyboard_y[keyboard_set_active] > 0.5f) _GlobalVariables.press_up[player_number] = 1f;    else _GlobalVariables.press_up[player_number] = 0f;
                if (_General_settings.keyboard_button1[keyboard_set_active] > 0.5f) _GlobalVariables.press_b1[player_number] = 1f; else _GlobalVariables.press_b1[player_number] = 0f;
                if (_General_settings.keyboard_button2[keyboard_set_active] > 0.5f) _GlobalVariables.press_b2[player_number] = 1f; else _GlobalVariables.press_b2[player_number] = 0f;
            }
            else //JOYPAD
            {
                //Debug.Log("joypad detected");
                _GlobalVariables.press_left[player_number] = 0f; _GlobalVariables.press_right[player_number] = 0f; _GlobalVariables.press_up[player_number] = 0f;
                _GlobalVariables.press_down[player_number] = 0f; _GlobalVariables.press_b1[player_number] = 0f; _GlobalVariables.press_b2[player_number] = 0f;
                if (_General_settings.joypad_x[current_preset] < (0f - dead_zone)) { _GlobalVariables.press_left[player_number] = -_General_settings.joypad_x[current_preset] * 1.2f; if (_GlobalVariables.press_left[player_number] > 1f) _GlobalVariables.press_left[player_number] = 1f; }
                if (_General_settings.joypad_x[current_preset] > (0f + dead_zone)) { _GlobalVariables.press_right[player_number] = _General_settings.joypad_x[current_preset] * 1.2f; if (_GlobalVariables.press_right[player_number] > 1f) _GlobalVariables.press_right[player_number] = 1f; }
                if (_General_settings.joypad_y[current_preset] < (0f - dead_zone)) { _GlobalVariables.press_down[player_number] = -_General_settings.joypad_y[current_preset] * 1.2f; if (_GlobalVariables.press_down[player_number] > 1f) _GlobalVariables.press_down[player_number] = 1f; }
                if (_General_settings.joypad_y[current_preset] > (0f + dead_zone)) { _GlobalVariables.press_up[player_number] = _General_settings.joypad_y[current_preset] * 1.2f; if (_GlobalVariables.press_up[player_number] > 1f) _GlobalVariables.press_up[player_number] = 1f; }
                if (_General_settings.joypad_button1[current_preset] > (0.2f)) { _GlobalVariables.press_b1[player_number] = _General_settings.joypad_button1[current_preset]; }
                if (_General_settings.joypad_button2[current_preset] > (0.2f)) { _GlobalVariables.press_b2[player_number] = _General_settings.joypad_button2[current_preset]; }
            }
        } else
        {
            //Debug.Log("android mode");
            _GlobalVariables.press_left[player_number] = 0f; _GlobalVariables.press_right[player_number] = 0f; _GlobalVariables.press_up[player_number] = 0f;
            _GlobalVariables.press_down[player_number] = 0f; _GlobalVariables.press_b1[player_number] = 0f; _GlobalVariables.press_b2[player_number] = 0f;
            if (_GlobalVariables.x_axis < (0f - dead_zone)) { _GlobalVariables.press_left[player_number] = -_GlobalVariables.x_axis * 1.2f; if (_GlobalVariables.press_left[player_number] > 1f) _GlobalVariables.press_left[player_number] = 1f; }
            if (_GlobalVariables.x_axis > (0f + dead_zone)) { _GlobalVariables.press_right[player_number] = _GlobalVariables.x_axis * 1.2f; if (_GlobalVariables.press_right[player_number] > 1f) _GlobalVariables.press_right[player_number] = 1f; }
            if (_GlobalVariables.y_axis < (0f - dead_zone)) { _GlobalVariables.press_down[player_number] = -_GlobalVariables.y_axis * 1.2f; if (_GlobalVariables.press_down[player_number] > 1f) _GlobalVariables.press_down[player_number] = 1f; }
            if (_GlobalVariables.y_axis > (0f + dead_zone)) { _GlobalVariables.press_up[player_number] = _GlobalVariables.y_axis * 1.2f; if (_GlobalVariables.press_up[player_number] > 1f) _GlobalVariables.press_up[player_number] = 1f; }
            if (_GlobalVariables.button_1 == true) { _GlobalVariables.press_b1[player_number] = 1f; }
            if (_GlobalVariables.button_2 == true) { _GlobalVariables.press_b2[player_number] = 1f; }
        }

        if (out_of_hp == false)
        {
            this_key_pressed = 0;
            if (_GlobalVariables.press_left[player_number] > dead_zone)
            {
                this_key_pressed = 1;
                if (((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already)) && (_GlobalVariables.fire_phase_passed == true)) transform.localEulerAngles = new Vector3(0f, 0f, transform.localEulerAngles.z + (130f * Time.deltaTime));
            }
            if (_GlobalVariables.press_right[player_number] > dead_zone)
            {
                this_key_pressed = 2;
                if (((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already)) && (_GlobalVariables.fire_phase_passed == true)) transform.localEulerAngles = new Vector3(0f, 0f, transform.localEulerAngles.z - (130f * Time.deltaTime));
            }
            bool thrusters = false;
            if (_GlobalVariables.press_up[player_number] > dead_zone)
            {
                this_key_pressed = 3;
                if ((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already))
                {
                    if (_GlobalVariables.fire_phase_passed == true)
                    {
                        thrusters = true;
                        audioData.UnPause();
                        GetComponent<Rigidbody>().AddForce(transform.up * acceleration_speed * 10f);
                        Front_Thrusters.gameObject.SetActive(true);
                    }

                }
                else
                {
                    if (this_key_pressed != last_key_pressed)
                    {
                        if (player_number == 1) { _GlobalVariables.menu_position[1]--; if (_GlobalVariables.menu_position[1] == -1) _GlobalVariables.menu_position[1] = 0; }
                        if (player_number == 2) { _GlobalVariables.menu_position[2]--; if (_GlobalVariables.menu_position[2] == -1) _GlobalVariables.menu_position[2] = 0; }
                        if (player_number == 3) { _GlobalVariables.menu_position[3]--; if (_GlobalVariables.menu_position[3] == -1) _GlobalVariables.menu_position[3] = 0; }
                        if (player_number == 4) { _GlobalVariables.menu_position[4]--; if (_GlobalVariables.menu_position[4] == -1) _GlobalVariables.menu_position[4] = 0; }

                    }
                }
            }
            else
            {
                if ((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already))
                {
                    Front_Thrusters.gameObject.SetActive(false);
                }
            }
            if (_GlobalVariables.press_down[player_number] > dead_zone)
            {
                this_key_pressed = 4;
                if ((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already))
                {
                    if (_GlobalVariables.fire_phase_passed == true)
                    {
                        thrusters = true;
                        GetComponent<Rigidbody>().AddForce(-transform.up * break_speed * 10f);
                        Back_Thrusters.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (this_key_pressed != last_key_pressed)
                    {
                        if (player_number == 1) { _GlobalVariables.menu_position[1]++; if (_GlobalVariables.menu_position[1] > 4) _GlobalVariables.menu_position[1] = 4; }
                        if (player_number == 2) { _GlobalVariables.menu_position[2]++; if (_GlobalVariables.menu_position[2] > 4) _GlobalVariables.menu_position[2] = 4; }
                        if (player_number == 3) { _GlobalVariables.menu_position[3]++; if (_GlobalVariables.menu_position[3] > 4) _GlobalVariables.menu_position[3] = 4; }
                        if (player_number == 4) { _GlobalVariables.menu_position[4]++; if (_GlobalVariables.menu_position[4] > 4) _GlobalVariables.menu_position[4] = 4; }
                    }
                }
            }
            else
            {
                if ((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already))
                {
                    Back_Thrusters.gameObject.SetActive(false);
                }
            }
            if (thrusters)
            {
                if ((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already))
                {
                    audioData.UnPause();
                }
            }
            else
            {
                if ((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already))
                {
                    audioData.Pause();
                }
            }
            if (_GlobalVariables.press_b1[player_number] > dead_zone)
            {
                this_key_pressed = 5;
                if ((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already))
                {
                    if (_GlobalVariables.fire_phase_passed == true)
                    {
                        //shot
                        float compare_speed = 0;
                        switch (player_number)
                        {
                            case 1: compare_speed = _GlobalVariables.Speed_Shot[1]; break;
                            case 2: compare_speed = _GlobalVariables.Speed_Shot[2]; break;
                            case 3: compare_speed = _GlobalVariables.Speed_Shot[3]; break;
                            case 4: compare_speed = _GlobalVariables.Speed_Shot[4]; break;
                        }
                        if (last_shot > compare_speed)
                        {
                            GameObject child_gameobject = this.gameObject.transform.GetChild(0).gameObject;
                            last_gameobject = Instantiate(projectile, child_gameobject.transform.position, child_gameobject.transform.rotation);

                            last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                            last_gameobject.transform.position = child_gameobject.transform.position;
                            last_gameobject.transform.localRotation = child_gameobject.transform.localRotation;

                            last_gameobject.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.up * 100f);
                            last_shot = 0f;

                        }
                    }
                    else
                    {
                        _GlobalVariables.player_button_pressed[player_number] = true;
                    }
                }
                else
                {
                    int position_clicked = -1;
                    if (player_number == 1) { position_clicked = _GlobalVariables.menu_position[1]; }
                    if (player_number == 2) { position_clicked = _GlobalVariables.menu_position[2]; }
                    if (player_number == 3) { position_clicked = _GlobalVariables.menu_position[3]; }
                    if (player_number == 4) { position_clicked = _GlobalVariables.menu_position[4]; }
                    if (position_clicked == 0)
                    {
                        if (player_number == 1) { _GlobalVariables.menu_position[1] = -1; }
                        if (player_number == 2) { _GlobalVariables.menu_position[2] = -1; }
                        if (player_number == 3) { _GlobalVariables.menu_position[3] = -1; }
                        if (player_number == 4) { _GlobalVariables.menu_position[4] = -1; }
                    }
                    else
                    {
                        if (this_key_pressed != last_key_pressed)
                        {
                            _GlobalVariables.shop_button_pressed[player_number] = position_clicked;
                        }
                    }
                }
            }
            if (_GlobalVariables.press_b2[player_number] > dead_zone)
            {
                this_key_pressed = 6;
                if ((_GlobalVariables.shop_time != 2) || (allow_move_after_reappearing_already))
                {
                    if (_GlobalVariables.fire_phase_passed == true)
                    {
                        //shot
                        float compare_speed = 0;
                        compare_speed = _GlobalVariables.Speed_Shot[player_number];

                        if (last_shot > (compare_speed*_GlobalVariables.split_shot[player_number]))
                        {
                            GameObject child_gameobject = this.gameObject.transform.GetChild(0).gameObject;
                            float angle_div = 10f + ((_GlobalVariables.split_shot[player_number] - 1) * 15);
                            if (_GlobalVariables.split_shot[player_number] == 2) angle_div = 15;
                            if (angle_div >= 85) angle_div = 85;
                            float angle_per_shot = angle_div / (_GlobalVariables.split_shot[player_number]-1);
                            if (_GlobalVariables.split_shot[player_number] == 2) angle_per_shot = 15f;

                            //Debug.Log("angle_per_shot = " + angle_per_shot);
                            for (int shot_number = 0; shot_number < _GlobalVariables.split_shot[player_number]; shot_number++)
                            {
                                last_gameobject = Instantiate(projectile, child_gameobject.transform.position, transform.localRotation);
                                last_gameobject.transform.rotation = transform.localRotation;

                                last_gameobject.transform.Rotate(0.0f, 0.0f, (angle_div / 2) - (angle_per_shot * shot_number), Space.Self);
                                //Debug.Log("angle_per_shot " + shot_number + " = " + ((angle_div / 2) - (angle_per_shot * shot_number)));

                                last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                                last_gameobject.transform.position = child_gameobject.transform.position;
                                last_gameobject.GetComponent<Rigidbody>().velocity = last_gameobject.transform.TransformDirection(Vector3.up * 100f);
                                last_shot = 0f;

                            }

                        }
                    }
                    else
                    {
                        _GlobalVariables.player_button_pressed[player_number] = true;
                    }
                }
                else
                {
                    int position_clicked = -1;
                    if(player_number>0) position_clicked = _GlobalVariables.menu_position[player_number];
                    if (position_clicked == 0)
                    {
                        _GlobalVariables.menu_position[player_number] = -1;
                    }
                    else
                    {
                        if (this_key_pressed != last_key_pressed)
                        {
                            _GlobalVariables.shop_button_pressed[player_number] = position_clicked;
                        }
                    }
                }
            }
        }

        last_key_pressed = this_key_pressed;

        last_shot += Time.deltaTime;
        last_hit += Time.deltaTime;
        last_hit_player += Time.deltaTime;

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);

        //SHIP SPEED LIMIT (increasing with level)
        Vector3 currentVelocity = m_Rigidbody.velocity;
        Vector3 currentVelocityLocal = transform.InverseTransformDirection(currentVelocity);
        Vector3 newVelocityLocal;
        float velocity_temp = Mathf.Sqrt((currentVelocityLocal.y * currentVelocityLocal.y) + (currentVelocityLocal.x * currentVelocityLocal.x));
        if (velocity_temp != 0)
        {
            float velocity_modifier = 1f;
            float max_speed = 40f + _GlobalVariables.level_number*2f;
            if (velocity_temp > max_speed) velocity_modifier = max_speed / velocity_temp;
            newVelocityLocal = new Vector3(currentVelocityLocal.x * velocity_modifier, currentVelocityLocal.y * velocity_modifier, 0f);
            velocity_x = currentVelocityLocal.x * velocity_modifier;
            velocity_y = currentVelocityLocal.y * velocity_modifier;
            m_Rigidbody.velocity = transform.TransformDirection(newVelocityLocal);
        }

        last_out_of_hp = out_of_hp;
    }
}
