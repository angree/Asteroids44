using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LeoLuz.PlugAndPlayJoystick;

public class Global_Variables : MonoBehaviour
{
    public int android = 0;

    General_settings _General_settings;
    GameObject GS;

    GameObject last_gameobject;
    GameObject[] Revive_option_in_panel = new GameObject[5];
    GameObject[] Panel_gfx = new GameObject[5];
    GameObject[] Press_Fire_msg = new GameObject[5];
    GameObject[] Ship_gameobject = new GameObject[5];
    GameObject[] Player_btc_text = new GameObject[5];
    AudioSource audioData;

    [HideInInspector]
    public shop_ship_script _Shop_ship;
    [HideInInspector]
    public GameObject Shop_ship_go;
    public GameObject teleport;
    public GameObject[] asteroid_prefab = new GameObject[12];
    public GameObject asteroid_health;
    public GameObject asteroid_shot;
    public GameObject asteroid_max_health;
    public GameObject asteroid_bitcoin;
    public GameObject asteroid_crystal_red;
    public GameObject asteroid_crystal_green;
    public GameObject asteroid_crystal_yellow;
    public GameObject bitcoin_coin;
    public GameObject bomb_model;
    public GameObject shot_split_model;
    public GameObject asteroid_explosion;
    public GameObject bomb_explosion;
    public GameObject ship_fire;
    public GameObject ship_smoke;
    public GameObject cam;
    public GameObject pause_panel;
    public GameObject[] Planet_host = new GameObject[25];
    public Text level_text;
    public Text level_text2;
    public Text[] Score_text = new Text[5];
    public Vector3 main_camera_pos;
    public AudioClip bitcoin_audioclip;
    public AudioClip[] teleport_sound = new AudioClip[5];

    [HideInInspector]
    public int[] menu_position = new int[5];
    [HideInInspector]
    public float[] menu_position_trailing = new float[5];
    public bool pause = false;
    public float shop_ship_main_engines = 0f;
    public float shop_ship_rear_engines = 0f;
    public float drop_chance = 18f;
    public float drop_chance_drop_per_level = 3f;
    public float drop_chance_bitcoin = 38f;
    public float drop_chance_btc_drop_per_level = 3f;
    public float drop_chance_bomb = 5f;
    public float drop_chance_bomb_drop_per_level = 0.5f;
    public float drop_chance_min = 3f;
    public float drop_chance_multi_per_player = 0.3f;
    public float red_crystal_cost = 1.0f;
    public float green_crystal_cost = 1.0f;
    public float yellow_crystal_cost = 2.0f;
    public float revive_cost = 1.0f;
    public int how_many_players = 4;
    public float wait_between_objects = 10f;
    public float reduce_time_wait_per_level = 1f;

    public bool[] player_active = new bool[5];
    public float[] Energy_Player = new float[5];
    public float[] Player_Flame_Number = new float[5];
    public float[] Max_Energy_Player = new float[5];
    public float[] Speed_Shot = new float[5];
    public float[] Score_account = new float[5];
    public float[] Score_account_last = new float[5];
    public float[] player_btc = new float[5];
    public float[] split_shot = new float[5];
    public float[] player_score_multipler = new float[5];
    public float[] player_score_multipler_last_refresh = new float[5];
    public float score_multipler_time_limit = 1f;
    public float score_multiupler_up_per_hit = 0.1f;
    public int level_number = 0;
    public int level_number_score_calc = 0;
    public int objects_increase_per_level = 3;

    public int[] shop_button_pressed = new int[5];
    public float[] press_left = new float[5];
    public float[] press_right = new float[5];
    public float[] press_up = new float[5];
    public float[] press_down = new float[5];
    public float[] press_b1 = new float[5];
    public float[] press_b2 = new float[5];
    public float[] last_asteroid_explosion = new float[10];
    public float[] last_shot_player = new float[5];
    public int current_planet = 1;
    public int[] planet_history = new int[25];
    public int planet_history_pos = 0;

    private int number_of_objects = 0;
    private int last_number_of_objects = 0;
    private float timer = 0;
    private float new_level_timer = 0;
    public float asteroid_timer = 0f;
    private bool full_game_started = false;
    public int shop_time = 0;
    public int shop_2_phase = 0;
    public int planet_zoom_phase = 0;
    public float asteroid_explosion_timer = 0f;
    public int last_asteroid_explosion_pos = 0;

    void Start()
    {
        GS = GameObject.Find("Settings_holder");
        if (GS != null)
        {
            _General_settings = GS.GetComponent<General_settings>();
            full_game_started = true;
        }

        current_planet = 1;
        pause_panel = GameObject.Find("Pause");
        pause_panel.SetActive(false);
        
        for (int i = 1; i <= 15; i++)
        {
            string string_number = "" + i;
            if (i < 10) string_number = "0" + i;
            Planet_host[i] = GameObject.Find("Planet_Level_" + string_number);
            if (Planet_host[i] == null) Debug.Log("Planet " + string_number + " not found");
            Planet_host[i].SetActive(false);
        }
        main_camera_pos = cam.transform.position;

        //variable init
        player_active[1] = true; player_active[2] = false; player_active[3] = false; player_active[4] = false;

        for (int i = 1; i <= 4; i++) Energy_Player[i] = 10f;
        for (int i = 1; i <= 4; i++) Max_Energy_Player[i] = 10f;
        for (int i = 1; i <= 4; i++) Speed_Shot[i] = 0.5f;
        for (int i = 1; i <= 4; i++) last_shot_player[i] = 0f;
        for (int i = 1; i <= 4; i++) Score_text[i] = GameObject.Find("Player_" + i).GetComponent<Text>();
        for (int i = 1; i <= 4; i++) Revive_option_in_panel[i] = GameObject.Find("text_revive_p" + i);
        audioData = GetComponent<AudioSource>();

        if (!full_game_started)
        {
            for (int i = 1; i <= 4; i++) { if (how_many_players >= i) player_active[i] = true; }
        }

        Panel_gfx[1] = GameObject.Find("Panel_red_01"); Panel_gfx[2] = GameObject.Find("Panel_yellow_02"); Panel_gfx[3] = GameObject.Find("Panel_green_03"); Panel_gfx[4] = GameObject.Find("Panel_blue_04");
        Player_btc_text[1] = GameObject.Find("player_1_btc"); Player_btc_text[2] = GameObject.Find("player_2_btc"); Player_btc_text[3] = GameObject.Find("player_3_btc"); Player_btc_text[4] = GameObject.Find("player_4_btc");
        Ship_gameobject[1] = GameObject.Find("Ship_red"); Ship_gameobject[2] = GameObject.Find("Ship_yellow"); Ship_gameobject[3] = GameObject.Find("Ship_blue"); Ship_gameobject[4] = GameObject.Find("Ship_green");
        Press_Fire_msg[1] = GameObject.Find("Press_fire_1"); Press_Fire_msg[2] = GameObject.Find("Press_fire_2"); Press_Fire_msg[3] = GameObject.Find("Press_fire_3"); Press_Fire_msg[4] = GameObject.Find("Press_fire_4");

        for (int i = 1; i <= 4; i++) Panel_gfx[i].SetActive(false);

        Shop_ship_go = GameObject.Find("Ship_shop");
        if (Shop_ship_go != null)
        {
            _Shop_ship = Shop_ship_go.GetComponent<shop_ship_script>();
        }

        if (full_game_started)
        {
            for (int i = 1; i <= 4; i++) player_active[i] = false;
            how_many_players = 0;
            if (_General_settings.player_controller_scheme[1] != 8) { player_active[1] = true; how_many_players++; }
            if (_General_settings.player_controller_scheme[2] != 8) { player_active[2] = true; how_many_players++; }
            if (_General_settings.player_controller_scheme[3] != 8) { player_active[3] = true; how_many_players++; }
            if (_General_settings.player_controller_scheme[4] != 8) { player_active[4] = true; how_many_players++; }
        }
        for (int i = 1; i <= 4; i++) player_button_pressed[i] = false;

        for (int i = 1; i <= 4; i++) { if (player_active[i] == true) { Press_Fire_msg[i].GetComponent<Text>().text = "PRESS FIRE WHEN READY!"; Press_Fire_msg[i].SetActive(true); } else { Press_Fire_msg[i].SetActive(false); player_button_pressed[i] = true; } }

        for (int i = 1; i <= 4; i++) split_shot[i] = 2f;


    }

    public bool[] player_button_pressed = new bool[5];
    [HideInInspector]
    public bool fire_phase_passed = false;
    bool skip_first_shop = false;

    float first_game_over = -1f;
    int hiscore_entered = 0;  //0 - not 1- entering now 2- entered
    int[] hiscore_position = new int[5];
    int[] entering_position = new int[5];
    float[] hiscore_last_change_letter = new float[5];
    float blinking_cursor = 0f;

    public float x_axis = 0f;
    public float y_axis = 0f;
    public bool button_1 = false;
    public bool button_2 = false;
    float hiscore_time_left = 100f;

    void Update()
    {
        x_axis = 0f;
        y_axis = 0f;
        button_1 = false;
        button_2 = false;

        if (android == 1)
        {
            x_axis = LeoLuz.PlugAndPlayJoystick.AnalogicKnob.x_axis;
            y_axis = LeoLuz.PlugAndPlayJoystick.AnalogicKnob.y_axis;
            button_1 = LeoLuz.PlugAndPlayJoystick.UIButtonToButton.button_pressed[1];
            button_2 = LeoLuz.PlugAndPlayJoystick.UIButtonToButton.button_pressed[2];
            if (button_1 == true) Debug.Log(">>>>>>>> BUTTON 1");
        }

        level_number_score_calc = level_number;
        if (level_number_score_calc > 8) level_number_score_calc = (int)(8f + (((float)level_number_score_calc - 8f) * 0.1f));
        for (int i = 1; i <= 4; i++)
        {
            int Score_account_divide = (int)(Score_account[i] / 1000000);
            int Score_account_last_divide = (int)(Score_account_last[i] / 1000000);
            if ((Score_account[i] > Score_account_last[i]) && (Score_account_divide > Score_account_last_divide))
            {
                player_btc[i] += 1f;
            }
            Score_account_last[i] = Score_account[i];
        }

        asteroid_explosion_timer += Time.deltaTime;

        for (int i = 1; i <= 4; i++)
        {
            if (asteroid_explosion_timer > player_score_multipler_last_refresh[i] + score_multipler_time_limit) player_score_multipler[i] = 1f;
        }

        blinking_cursor += Time.deltaTime; while (blinking_cursor > 1f) blinking_cursor -= 1f;

        main_camera_pos = cam.transform.position;

        bool players_alive = false;
        for (int i = 1; i <= 4; i++) { if ((player_active[i]) && (Energy_Player[i] > 0)) players_alive = true; if (Energy_Player[i] < 0) Energy_Player[i] = 0; }

        if (players_alive == true) { first_game_over = -1f; hiscore_entered = 0; }
        if ((players_alive == false) && (hiscore_entered != 1)) { level_text.text = "GAME OVER"; if (first_game_over == -1f) first_game_over = 0f; }     //CENTRE TEXT
        if ((hiscore_entered == 0) && (players_alive == false))
        {
            bool no_hiscore_to_enter = true;
            for (int i = 1; i <= 4; i++)
            {
                if (player_active[i] == true)
                {
                    hiscore_position[i] = 1;
                    while (Score_account[i] < _General_settings.Hi_score_points[hiscore_position[i]])
                    {
                        hiscore_position[i]++;
                        if (hiscore_position[i] > 10) break;
                    }
                    if (hiscore_position[i] <= 10) no_hiscore_to_enter = false;
                    for (int i2 = 1; i2 < i; i2++)
                    {
                        if ((hiscore_position[i2] >= hiscore_position[i]) && (hiscore_position[i2] < 11)) hiscore_position[i2]++;
                        //here do a checkup of other players after moving scores
                    }
                    for (int temp_place = 10; temp_place >= hiscore_position[i]; temp_place--)
                    {
                        if (temp_place != 10)
                        {
                            _General_settings.Hi_score_points[temp_place + 1] = _General_settings.Hi_score_points[temp_place];
                            _General_settings.Hi_score_names[temp_place + 1] = _General_settings.Hi_score_names[temp_place];
                        }
                        if (temp_place == hiscore_position[i])
                        {
                            _General_settings.Hi_score_points[temp_place] = Score_account[i];
                            _General_settings.Hi_score_names[temp_place] = "AAA";
                            entering_position[i] = 0;
                            hiscore_entered = 1;
                        }
                    }
                }
            }
            if (no_hiscore_to_enter) hiscore_entered = 2;

        }

        if (hiscore_entered == 0)
        {
            hiscore_time_left = 100f;
        }
        //HISCORE START
        if ((hiscore_entered == 1))
        {
            _General_settings.normal_joypad_controls_forced = true;
            for (int i = 1; i <= 4; i++)
            {
                //entering score here
                if ((hiscore_position[i] > 10)) entering_position[i] = 3;
                if ((hiscore_position[i] < 11) && (hiscore_position[i] > 0) && (player_active[i] == true))
                {
                    hiscore_last_change_letter[i] += Time.deltaTime;
                    Press_Fire_msg[i].SetActive(true);
                    char[] chars3 = _General_settings.Hi_score_names[hiscore_position[i]].ToCharArray();
                    bool not_pressed = true;
                    if ((press_up[i] > 0.5f) && (hiscore_last_change_letter[i] > 0.3f)) { chars3[entering_position[i]]++; hiscore_last_change_letter[i] = 0f; }
                    if ((press_down[i] > 0.5f) && (hiscore_last_change_letter[i] > 0.3f)) { chars3[entering_position[i]]--; hiscore_last_change_letter[i] = 0f; }
                    if ((press_b1[i] > 0.5f) && (hiscore_last_change_letter[i] > 2.0f)) { entering_position[i]++; hiscore_last_change_letter[i] = 0f; if (entering_position[i] > 3) entering_position[i] = 3; }
                    if ((press_b2[i] > 0.5f) && (hiscore_last_change_letter[i] > 2.0f)) { entering_position[i]--; hiscore_last_change_letter[i] = 0f; if (entering_position[i] < 0) entering_position[i] = 0; }
                    if ((press_up[i] > 0.5f) || (press_down[i] > 0.5f) || (press_b1[i] > 0.5f) || (press_b2[i] > 0.5f)) not_pressed = false;
                    if (not_pressed == true) hiscore_last_change_letter[i] = 10f;

                    if (entering_position[i] < 3)
                    {
                        if (chars3[entering_position[i]] == (char)64) chars3[entering_position[i]] = (char)46; //<65
                        if (chars3[entering_position[i]] == (char)45) chars3[entering_position[i]] = (char)90; //<46
                        if (chars3[entering_position[i]] == (char)47) chars3[entering_position[i]] = (char)65; //>46
                        if (chars3[entering_position[i]] == (char)91) chars3[entering_position[i]] = (char)65; //>90
                    }
                    _General_settings.Hi_score_names[hiscore_position[i]] = new string(chars3);

                    if ((blinking_cursor > 0.7f) && (entering_position[i] < 3)) chars3[entering_position[i]] = '_';
                    Press_Fire_msg[i].GetComponent<Text>().text = "Enter your name:\n" + hiscore_position[i] + ". " + contract_score(_General_settings.Hi_score_points[hiscore_position[i]]) + "\n" + new string(chars3);
                }
            }
            bool hs_finished = true;
            for (int i = 1; i <= 4; i++)
            {
                if ((entering_position[i] < 3) && (player_active[i] == true))
                {
                    hs_finished = false;
                }
            }
            hiscore_time_left -= Time.deltaTime;
            if (hiscore_time_left < 0f)
            {
                hiscore_time_left = 0f;
                hs_finished = true;
                for (int i = 1; i <= 4; i++)
                {
                    entering_position[i] = 3;
                }
            }
            level_text.text = Mathf.Round(hiscore_time_left) + " S";

            if (hs_finished == true) { _General_settings.save_scores = true; hiscore_entered = 2; }
        }
        if (hiscore_entered == 2) { if (GS != null) _General_settings.normal_joypad_controls_forced = false; }
        //HISCORE END

        if ((first_game_over >= 0f) && (hiscore_entered == 2)) first_game_over += Time.deltaTime;
        if ((first_game_over > 5f) && (hiscore_entered == 2))
        {
            _General_settings.title_mode = 0; _General_settings.loaded_scene = 1; SceneManager.LoadScene(1);
        }

        if (!(player_button_pressed[1] && player_button_pressed[2] && player_button_pressed[3] && player_button_pressed[4]))
        {
            for (int i = 1; i <= 4; i++) { if (player_button_pressed[i] == true) Press_Fire_msg[i].SetActive(false); }
        }
        else
        {
            if (fire_phase_passed == false)
            {
                fire_phase_passed = true;
                for (int i = 1; i <= 4; i++) Press_Fire_msg[i].SetActive(false);
            }
            for (int i = 1; i <= 4; i++) { if (Energy_Player[i] > Max_Energy_Player[i]) Energy_Player[i] = Max_Energy_Player[i]; }

            if (players_alive) level_text.text = "LEVEL " + level_number;      //CENTRE TEXT
            level_text2.text = "";                          //UP TEXT
            if (level_number == 0) level_text.text = "PREPARE...";
            if (new_level_timer > 8f)
            {
                if ((!((shop_time == 2) && (shop_2_phase == 5))) && (players_alive == true))
                    level_text.text = "";
                if (level_number > 0) level_text2.text = "LEVEL " + level_number;
                else level_text2.text = "";
            }

            if ((number_of_objects == 0) && (last_number_of_objects != number_of_objects))
            {
                //if last object spawned wait for 10 secs (Zero timer)
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
            if ((number_of_objects == 0) && (timer > 10f) && (asteroid_timer > 2f) && (shop_time == 0))
            {
                timer = 10f;
                //new level
                shop_time = 1;
                if ((level_number == 0) && (skip_first_shop)) shop_time = 4;
                else shop_time = 1;

            }
            if ((number_of_objects == 0) && (level_number == 0) && (asteroid_timer > 2f) && (shop_time == 0))
            {
                timer = 10f;
                shop_time = 1;
                if ((level_number == 0) && (skip_first_shop)) shop_time = 1;
                else shop_time = 1;

            }
            //flight
            if (shop_time == 1)
            {
                shop_ship_main_engines = 0f;
                shop_ship_rear_engines = 2f;

                float flying_time = timer - 10f;
            }
            //flight end
            if (shop_time == 2)
            {

                GameObject last_gameobject;

                //shop_2_phase 1 to 4
                //disapearing players
                for (int i = 1; i <= 4; i++)
                {
                    if ((timer > 15f + (0.5f * i)) && (shop_2_phase == (i - 1)))
                    {
                        shop_ship_main_engines = 0f;
                        shop_ship_rear_engines = 0f;
                        if ((player_active[i]) && (Energy_Player[i] > 0))
                        {
                            last_gameobject = Instantiate(teleport, Ship_gameobject[i].transform.position, Ship_gameobject[i].transform.rotation);
                            AudioSource.PlayClipAtPoint(teleport_sound[i], main_camera_pos);
                            Ship_gameobject[i].transform.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                            Ship_gameobject[i].transform.localPosition = new Vector3(Ship_gameobject[i].transform.localPosition.x - 1000.0f - (500f * i), Ship_gameobject[i].transform.localPosition.y, Ship_gameobject[i].transform.localPosition.z);
                        }
                        shop_2_phase = i;
                    }
                }
                if ((timer > 17.5f) && (shop_2_phase == 4))
                {
                    for (int i = 1; i <= 4; i++) { if (player_active[i] && (Energy_Player[i] > 0)) { Panel_gfx[i].SetActive(true); menu_position[i] = 0; } else menu_position[i] = -1; }
                    shop_2_phase = 5;
                    timer = 0f;
                    for (int i = 1; i <= 4; i++) shop_button_pressed[i] = 0;
                    planet_zoom_phase = 0;
                }
                //SHOP UI ACTIVE BELOW
                if (shop_2_phase == 5)
                {
                    if (GS != null) _General_settings.normal_joypad_controls_forced = true;
                    //close shop on game startup (level 0)
                    if ((level_number == 0) && (planet_zoom_phase == 0))
                    {
                        for (int i = 1; i <= 4; i++) { menu_position[i] = -1; }
                    }
                    //planet fly away start
                    if ((timer <= 5f))
                    {
                        planet_zoom_phase = 0;
                        float timer_go = 5f - timer;
                        float timer_go_copy = (((5f - timer_go) * (5f - timer_go) * (5f - timer_go)) * (24)); //od 0 do 3000
                        shop_ship_main_engines = timer_go_copy / 3000f;
                        shop_ship_rear_engines = 0f;
                        if (timer_go_copy < 0f) timer_go_copy = 0f;
                        Planet_host[current_planet].transform.localPosition = new Vector3(Planet_host[current_planet].transform.localPosition.x, -timer_go_copy, Planet_host[current_planet].transform.localPosition.z);
                    }
                    if ((planet_zoom_phase == 0) && (timer > 5f))
                    {
                        shop_ship_main_engines = 1f;
                        shop_ship_rear_engines = 0f;
                        Planet_host[current_planet].SetActive(false);

                        current_planet = 0;
                        bool current_planet_exists = true;
                        int while_error_count = 0;
                        while (current_planet_exists)
                        {
                            current_planet = Random.Range(1, 16);
                            current_planet_exists = false;
                            for (int i = 1; i <= 10; i++)
                            {
                                int cur_pos = planet_history_pos - i;
                                if (cur_pos < 0) cur_pos += 10;
                                if (current_planet == planet_history[cur_pos]) current_planet_exists = true;
                            }
                            while_error_count++;
                        }
                        if (while_error_count > 500) Debug.Log("error in random planet number generator");
                        planet_history[planet_history_pos] = current_planet;
                        planet_history_pos++;
                        if (current_planet > 15) current_planet = 1;
                        Planet_host[current_planet].transform.localPosition = new Vector3(Planet_host[current_planet].transform.localPosition.x, 2000, Planet_host[current_planet].transform.localPosition.z);
                        Planet_host[current_planet].SetActive(true);
                        planet_zoom_phase = 1;
                    }
                    if ((timer > 35f) && timer <= 40f)
                    {
                        float timer_go = timer - 35f;
                        float timer_go_copy = (((5f - timer_go) * (5f - timer_go) * (5f - timer_go)) * (24)); //od 3000 do zera
                        if (timer_go_copy < 0f) timer_go_copy = 0f;
                        shop_ship_main_engines = 0f;
                        shop_ship_rear_engines = timer_go_copy / 3000f;
                        Planet_host[current_planet].transform.localPosition = new Vector3(Planet_host[current_planet].transform.localPosition.x, timer_go_copy, Planet_host[current_planet].transform.localPosition.z);
                    }
                    //planet fly away end
                    for (int i = 1; i <= 4; i++)
                    {
                        if (menu_position[i] == -1) Panel_gfx[i].SetActive(false);
                        else Player_btc_text[i].GetComponent<Text>().text = "" + Mathf.Round(player_btc[i]) + ".0";
                    }
                    bool any_player_dead = false;
                    for (int i = 1; i <= 4; i++) if (player_active[i] && (Energy_Player[i] <= 0)) any_player_dead = true;
                    for (int i = 1; i <= 4; i++)
                    {
                        if (any_player_dead == true) Revive_option_in_panel[i].SetActive(true);
                        else Revive_option_in_panel[i].SetActive(false);
                    }
                    if (any_player_dead == false) for (int i = 1; i <= 4; i++) { if (menu_position[i] >= 4) menu_position[i] = 3; }
                    //crystal purchases
                    for (int i = 1; i <= 4; i++) { if (shop_button_pressed[i] == 1) { if (player_btc[i] >= 1f) { last_gameobject = Instantiate(asteroid_crystal_red, Ship_gameobject[i].transform.position, transform.rotation); last_gameobject.transform.parent = GameObject.Find("zero_object").transform; shop_button_pressed[i] = 0; player_btc[i] -= 1f; } } }
                    for (int i = 1; i <= 4; i++) { if (shop_button_pressed[i] == 2) { if (player_btc[i] >= 1f) { last_gameobject = Instantiate(asteroid_crystal_green, Ship_gameobject[i].transform.position, transform.rotation); last_gameobject.transform.parent = GameObject.Find("zero_object").transform; shop_button_pressed[i] = 0; player_btc[i] -= 1f; } } }
                    for (int i = 1; i <= 4; i++) { if (shop_button_pressed[i] == 3) { if (player_btc[i] >= 2f) { last_gameobject = Instantiate(asteroid_crystal_yellow, Ship_gameobject[i].transform.position, transform.rotation); last_gameobject.transform.parent = GameObject.Find("zero_object").transform; shop_button_pressed[i] = 0; player_btc[i] -= 2f; } } }
                    //revive players
                    for (int i = 1; i <= 4; i++)
                    {
                        if (shop_button_pressed[i] == 4)
                        {
                            if (player_btc[i] >= 1f)
                            {
                                shop_button_pressed[i] = 0; player_btc[i] -= 1f;
                                for (int i2 = 1; i2 <= 4; i2++)
                                {
                                    if (player_active[i2] && (Energy_Player[i2] <= 0))
                                    {
                                        Energy_Player[i2] = 40;
                                    }
                                }
                                for (int i2 = 1; i2 <= 4; i2++) { if (player_active[i2] && (Energy_Player[i2] > 0)) { Panel_gfx[i2].SetActive(true); if (menu_position[i2] == -1) menu_position[i2] = 0; } }
                                AudioSource.PlayClipAtPoint(bitcoin_audioclip, main_camera_pos);
                            }
                        }
                    }

                    if ((menu_position[1] == -1) && (menu_position[2] == -1) && (menu_position[3] == -1) && (menu_position[4] == -1) && (timer > 5f)) { if (timer < 35f) timer = 35f; }
                    if (timer > 40f) shop_2_phase = 6;
                    if ((40f - timer) < 1.5f) level_text.text = Mathf.Round(40f - timer) + " SEC LEFT";
                    else level_text.text = Mathf.Round(40f - timer) + " SECS LEFT";
                    if (level_number == 0)
                    {
                        float time_left = (40f - timer);
                        if (time_left >= 30f) time_left -= 30f;
                        if ((time_left) < 1.5f) level_text.text = Mathf.Round(time_left) + " SEC LEFT";
                        else level_text.text = Mathf.Round(time_left) + " SECS LEFT";
                    }
                }
                else { if (GS != null) _General_settings.normal_joypad_controls_forced = false; }
                //SHOP UI OFF
                if (shop_2_phase == 6)
                {
                    shop_ship_main_engines = 0f;
                    shop_ship_rear_engines = 0f;
                    Panel_gfx[1].SetActive(false);
                    Panel_gfx[2].SetActive(false);
                    Panel_gfx[3].SetActive(false);
                    Panel_gfx[4].SetActive(false);
                    shop_2_phase = 7;
                    timer = 15f;
                }
                //teleport players to the space
                for (int i = 1; i <= 4; i++)
                {
                    if ((timer > 15f + (i * 0.5f)) && (shop_2_phase == 7 + (i - 1)))
                    {
                        if ((player_active[i]) && (Energy_Player[i] > 0))
                        {
                            Ship_gameobject[i].transform.localPosition = new Vector3(Ship_gameobject[i].transform.localPosition.x + 1000.0f + (500f * i), Ship_gameobject[i].transform.localPosition.y, Ship_gameobject[i].transform.localPosition.z);
                            last_gameobject = Instantiate(teleport, Ship_gameobject[i].transform.position, Ship_gameobject[i].transform.rotation);
                            AudioSource.PlayClipAtPoint(teleport_sound[i], main_camera_pos);
                        }
                        shop_2_phase = 7 + i;
                    }

                }

                if ((timer > 17.5f) && (shop_2_phase == 11))
                {
                    shop_time = 3;
                }
            }
            else shop_2_phase = 0;
            if (shop_time == 3)
            {

                for (int i = 1; i <= 4; i++) Panel_gfx[i].SetActive(false);
                //shop_flying_away
                shop_ship_main_engines = 2f;
                shop_ship_rear_engines = 0f;


            }
            if (shop_time == 4)
            {
                //new level
                shop_time = 0;
                level_number++;
                new_level_timer = 0;
                number_of_objects = level_number * objects_increase_per_level;
                timer = 100f;
            }

            if (number_of_objects > 0)
            {
                float end_waiting_time = wait_between_objects - (reduce_time_wait_per_level * level_number);
                end_waiting_time = end_waiting_time * (5f - how_many_players);

                if (end_waiting_time < 2f) end_waiting_time = 2f;
                if ((timer > end_waiting_time) || (asteroid_timer > 2f))
                {
                    int horizontal_spawn = Random.Range(0, 6);
                    int asteroid_random_number = Random.Range(6, 12);
                    //Debug.Log("asteroid number = "+asteroid_random_number);
                    if (horizontal_spawn == 0) //left
                    {
                        last_gameobject = Instantiate(asteroid_prefab[asteroid_random_number], new Vector3(-11.38f * 10f, Random.Range(-6.85f * 10f, 6.85f * 10f), 0), Quaternion.identity);
                        last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                        last_gameobject.transform.localPosition = new Vector3(-11.38f * 10f, Random.Range(-6.85f * 10f, 6.85f * 10f), 0);
                    }
                    if (horizontal_spawn == 1) //right
                    {
                        last_gameobject = Instantiate(asteroid_prefab[asteroid_random_number], new Vector3(11.38f * 10f, Random.Range(-6.85f * 10f, 6.85f * 10f), 0), Quaternion.identity);
                        last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                        last_gameobject.transform.localPosition = new Vector3(11.38f * 10f, Random.Range(-6.85f * 10f, 6.85f * 10f), 0);
                    }
                    if ((horizontal_spawn == 2) || (horizontal_spawn == 3)) //up
                    {
                        last_gameobject = Instantiate(asteroid_prefab[asteroid_random_number], new Vector3(Random.Range(-11.38f * 10f, 11.38f * 10f), 6.85f * 10f, 0), Quaternion.identity);
                        last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                        last_gameobject.transform.localPosition = new Vector3(Random.Range(-11.38f * 10f, 11.38f * 10f), 6.85f * 10f, 0);
                    }
                    if ((horizontal_spawn == 4) || (horizontal_spawn == 5)) //down
                    {
                        last_gameobject = Instantiate(asteroid_prefab[asteroid_random_number], new Vector3(Random.Range(-11.38f * 10f, 11.38f * 10f), -6.85f * 10f, 0), Quaternion.identity);
                        last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                        last_gameobject.transform.localPosition = new Vector3(Random.Range(-11.38f * 10f, 11.38f * 10f), -6.85f * 10f, 0);
                    }
                    timer = 0f;
                    number_of_objects--;
                }
            }
            last_number_of_objects = number_of_objects;
            new_level_timer += Time.deltaTime;


            for (int i = 1; i <= 4; i++)
            {
                string current_score = "" + Score_account[i].ToString("#");
                if (Score_account[i] == 0) current_score = "0";
                if (Score_account[i] >= 1000000000)
                {
                    float current_score_float = (float)Score_account[i] / 1000000000;
                    current_score = current_score_float.ToString("#.000") + " BILLIONS";
                }
                else if (Score_account[i] >= 100000000)
                {
                    float current_score_float = (float)Score_account[i] / 1000000;
                    current_score = current_score_float.ToString("#.0") + " MILLIONS";
                }
                else if (Score_account[i] >= 10000000)
                {
                    float current_score_float = (float)Score_account[i] / 1000000;
                    current_score = current_score_float.ToString("#.00") + " MILLIONS";
                }
                else if (Score_account[i] >= 1000000)
                {
                    float current_score_float = (float)Score_account[i] / 1000000;
                    current_score = current_score_float.ToString("#.000") + " MILLIONS";
                }
                Score_text[i].text = "PLAYER " + i + "  SHOT: " + (Mathf.Round((1f / Speed_Shot[i]) * 10f) / 10) + "/sec   BITCOINS: " + player_btc[i] + "   ENERGY: " + Mathf.Round(Energy_Player[i]) + "/" + Max_Energy_Player[i] + "   SCORE: " + current_score;
            }
            for (int i = 1; i <= 4; i++) { if ((player_active[i] == false)) Score_text[i].text = ""; }

            for (int i = 1; i <= 4; i++) { if ((Energy_Player[i] / Max_Energy_Player[i]) > 0.3) Player_Flame_Number[i] = Random.Range(1, 4); }

            asteroid_timer += Time.deltaTime;
            if (GS != null) _General_settings.joypad_timer += Time.deltaTime;
        }
        if ((!fire_phase_passed) && (_General_settings.keyboard_escape > 0.3f))
        {
            _General_settings.title_mode = 0; _General_settings.loaded_scene = 1; SceneManager.LoadScene(1);
        }
        if ((fire_phase_passed) && (hiscore_entered < 1) && (_General_settings.keyboard_escape > 0.3f))
        {
            if (_General_settings.prev_keyboard_escape != _General_settings.keyboard_escape)
            {
                if (Time.timeScale == 0f)
                {
                    pause = false;
                    pause_panel.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    pause = true;
                    pause_panel.SetActive(true);
                    Time.timeScale = 0f;
                }
                _General_settings.prev_keyboard_escape = _General_settings.keyboard_escape;

            }
        }
    }

    string contract_score(float score_input)
    {
        string current_score = "" + score_input.ToString("#");
        if (score_input == 0) current_score = "0";
        if (score_input >= 1000000000)
        {
            float current_score_float = score_input / 1000000000;
            current_score = current_score_float.ToString("#.000") + " BILLIONS";
        }
        else if (score_input >= 100000000)
        {
            float current_score_float = score_input / 1000000;
            current_score = current_score_float.ToString("#.0") + " MILLIONS";
        }
        else if (score_input >= 10000000)
        {
            float current_score_float = score_input / 1000000;
            current_score = current_score_float.ToString("#.00") + " MILLIONS";
        }
        else if (score_input >= 1000000)
        {
            float current_score_float = score_input / 1000000;
            current_score = current_score_float.ToString("#.000") + " MILLIONS";
        }
        return current_score;
    }

}
