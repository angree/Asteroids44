using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System;

public class General_settings : MonoBehaviour
{

    [HideInInspector]
    public Text credits_text;
    [HideInInspector]
    public Text select_game_text;
    [HideInInspector]
    public GameObject select_game;
    [HideInInspector]
    public GameObject crystal_info;
    [HideInInspector]
    public GameObject loading;
    [HideInInspector]
    public GameObject floppy;

    [HideInInspector]
    public GameObject button_single;
    [HideInInspector]
    public GameObject button_coop;
    [HideInInspector]
    public GameObject button_death;

    GameObject Controller_Button_P1;
    GameObject Controller_Button_P2;
    GameObject Controller_Button_P3;
    GameObject Controller_Button_P4;

    GameObject Joypad_P1;
    GameObject Joypad_P2;
    GameObject Joypad_P3;
    GameObject Joypad_P4;

    GameObject[] button_rt = new GameObject[5];
    GameObject[] button_lt = new GameObject[5];
    GameObject[] button_up = new GameObject[5];
    GameObject[] button_down = new GameObject[5];

    Text Joypad_P1_no;
    Text Joypad_P2_no;
    Text Joypad_P3_no;
    Text Joypad_P4_no;

    GameObject KB_P1;
    GameObject KB_P2;
    GameObject KB_P3;
    GameObject KB_P4;

    GameObject Start_Game;

    Text P1_Fire1; Text P1_Fire2; Text P1_Arrow_left; Text P1_Arrow_down; Text P1_Arrow_right; Text P1_Arrow_up;
    Text P2_Fire1; Text P2_Fire2; Text P2_Arrow_left; Text P2_Arrow_down; Text P2_Arrow_right; Text P2_Arrow_up;
    Text P3_Fire1; Text P3_Fire2; Text P3_Arrow_left; Text P3_Arrow_down; Text P3_Arrow_right; Text P3_Arrow_up;
    Text P4_Fire1; Text P4_Fire2; Text P4_Arrow_left; Text P4_Arrow_down; Text P4_Arrow_right; Text P4_Arrow_up;


    [Serializable]
    public class HiScores
    {
        public float[] Hi_score_points = new float[11];
        public string[] Hi_score_names = new string[11];
    }

    [HideInInspector]
    public bool save_scores = false;
    [HideInInspector]
    public float[] joypad_last_call = new float[5];
    [HideInInspector]
    public int[] joypad_id = new int[5];
    [HideInInspector]
    public int[] player_controller_scheme = new int[5];
    [HideInInspector]
    public float prev_keyboard_escape = 0f;

    public int[] player_controller_profile = new int[5];
    public float[] Hi_score_points = new float[11];
    public string[] Hi_score_names = new string[11];
    public int number_of_players = 0;
    public int game_mode = 0;
    public int title_mode = 0;
    public int game_mode_selected = 0;
    public int loaded_scene = 0;
    public int prev_loaded_scene = 0;
    public float drop_chance_multi_per_player = 0f;

    public float joypad_timer = 0f;
    public float[] joypad_x = new float[5];
    public float[] joypad_y = new float[5];
    public float[] joypad_button1 = new float[5];
    public float[] joypad_button2 = new float[5];

    public float[] keyboard_x = new float[5];
    public float[] keyboard_y = new float[5];
    public float[] keyboard_button1 = new float[5];
    public float[] keyboard_button2 = new float[5];

    public float keyboard_escape = 0f;

    public int game_mode_select_position = 0;
    public bool normal_joypad_controls_forced = false;

    int last_menu_move = 0;
    float time_passed = 0f;
    float title_mode3_last = 0f;
    float scene_0_runtime = 0f;

    void Start()
    {
        HiScores loadedData = DataSaver.loadData<HiScores>("hiscores");
        if (loadedData == null)
        {
            for (int i = 1; i <= 10; i++) { Hi_score_names[i] = "ANN"; Hi_score_points[i] = 1000 * (11 - i); }
            //Debug.Log("save not found. generating default");
        }
        else
        {
            Hi_score_points = loadedData.Hi_score_points;
            Hi_score_names = loadedData.Hi_score_names;
            //Debug.Log("loading save");
        }
        for (int i = 1; i <= 4; i++) joypad_last_call[i] = -1f;

    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (save_scores == true)
        {
            HiScores saveData = new HiScores();
            saveData.Hi_score_points = Hi_score_points;
            saveData.Hi_score_names = Hi_score_names;
            DataSaver.saveData(saveData, "hiscores");
            save_scores = false;
            //Debug.Log("saving hiscores");
        }
        if (loaded_scene == 0)  //PSI-Q LOGO
        {
            if ((keyboard_escape > 0.3f) || (keyboard_button1[1] > 0.3f) || (keyboard_button1[2] > 0.3f) || (keyboard_button1[3] > 0.3f) || (Input.GetMouseButtonDown(0)) || (joypad_button1[1] > 0.3f) || (joypad_button1[2] > 0.3f) || (joypad_button1[3] > 0.3f) || (joypad_button1[4] > 0.3f))
            {
                scene_0_runtime = 4.01f;
                prev_keyboard_escape = keyboard_escape;
            }
            if (scene_0_runtime > 4f)
            {
                SceneManager.LoadScene(1);
                loaded_scene = 1;
                //Debug.Log("loading scene 1");
            }
            scene_0_runtime += Time.deltaTime;
        }
        else
        {
            //start moved here
            if ((loaded_scene == 1) && ((prev_loaded_scene == 0) || (prev_loaded_scene == 2))) //TITLE SCENE
            {
                time_passed = 0f;
                //variable init
                player_controller_scheme[1] = 1; player_controller_scheme[2] = 8; player_controller_scheme[3] = 8; player_controller_scheme[4] = 8;
                //variable init end

                crystal_info = GameObject.Find("Crystal_info");
                loading = GameObject.Find("LOADING");
                floppy = GameObject.Find("Floppy");
                credits_text = GameObject.Find("Credits").GetComponent<Text>();
                select_game_text = GameObject.Find("Select_game").GetComponent<Text>();

                select_game = GameObject.Find("Select_game");
                select_game.SetActive(false);
                crystal_info.SetActive(false);

                button_single = GameObject.Find("Button_single");
                button_single.SetActive(false);
                button_coop = GameObject.Find("Button_coop");
                button_coop.SetActive(false);
                button_death = GameObject.Find("Button_death");
                button_death.SetActive(false);
                loading.SetActive(false);
                floppy.SetActive(false);

                Controller_Button_P1 = GameObject.Find("Controller_Button_P1");
                Controller_Button_P1.SetActive(false);
                Controller_Button_P2 = GameObject.Find("Controller_Button_P2");
                Controller_Button_P2.SetActive(false);
                Controller_Button_P3 = GameObject.Find("Controller_Button_P3");
                Controller_Button_P3.SetActive(false);
                Controller_Button_P4 = GameObject.Find("Controller_Button_P4");
                Controller_Button_P4.SetActive(false);

                Joypad_P1_no = GameObject.Find("Joypad_P1_no").GetComponent<Text>();
                Joypad_P2_no = GameObject.Find("Joypad_P2_no").GetComponent<Text>();
                Joypad_P3_no = GameObject.Find("Joypad_P3_no").GetComponent<Text>();
                Joypad_P4_no = GameObject.Find("Joypad_P4_no").GetComponent<Text>();

                Joypad_P1 = GameObject.Find("Joypad_P1");
                button_rt[1] = GameObject.Find("01_rt"); button_lt[1] = GameObject.Find("01_lt"); button_up[1] = GameObject.Find("01_up"); button_down[1] = GameObject.Find("01_down");
                Joypad_P2 = GameObject.Find("Joypad_P2");
                button_rt[2] = GameObject.Find("02_rt"); button_lt[2] = GameObject.Find("02_lt"); button_up[2] = GameObject.Find("02_up"); button_down[2] = GameObject.Find("02_down");
                Joypad_P3 = GameObject.Find("Joypad_P3");
                button_rt[3] = GameObject.Find("03_rt"); button_lt[3] = GameObject.Find("03_lt"); button_up[3] = GameObject.Find("03_up"); button_down[3] = GameObject.Find("03_down");
                Joypad_P4 = GameObject.Find("Joypad_P4");
                button_rt[4] = GameObject.Find("04_rt"); button_lt[4] = GameObject.Find("04_lt"); button_up[4] = GameObject.Find("04_up"); button_down[4] = GameObject.Find("04_down");

                Joypad_P1.SetActive(false);
                Joypad_P2.SetActive(false);
                Joypad_P3.SetActive(false);
                Joypad_P4.SetActive(false);

                P1_Fire1 = GameObject.Find("P1_Fire1").GetComponent<Text>(); P1_Fire2 = GameObject.Find("P1_Fire2").GetComponent<Text>(); P1_Arrow_left = GameObject.Find("P1_Arrow_left").GetComponent<Text>();
                P1_Arrow_down = GameObject.Find("P1_Arrow_down").GetComponent<Text>(); P1_Arrow_right = GameObject.Find("P1_Arrow_right").GetComponent<Text>(); P1_Arrow_up = GameObject.Find("P1_Arrow_up").GetComponent<Text>();

                P2_Fire1 = GameObject.Find("P2_Fire1").GetComponent<Text>(); P2_Fire2 = GameObject.Find("P2_Fire2").GetComponent<Text>(); P2_Arrow_left = GameObject.Find("P2_Arrow_left").GetComponent<Text>();
                P2_Arrow_down = GameObject.Find("P2_Arrow_down").GetComponent<Text>(); P2_Arrow_right = GameObject.Find("P2_Arrow_right").GetComponent<Text>(); P2_Arrow_up = GameObject.Find("P2_Arrow_up").GetComponent<Text>();

                P3_Fire1 = GameObject.Find("P3_Fire1").GetComponent<Text>(); P3_Fire2 = GameObject.Find("P3_Fire2").GetComponent<Text>(); P3_Arrow_left = GameObject.Find("P3_Arrow_left").GetComponent<Text>();
                P3_Arrow_down = GameObject.Find("P3_Arrow_down").GetComponent<Text>(); P3_Arrow_right = GameObject.Find("P3_Arrow_right").GetComponent<Text>(); P3_Arrow_up = GameObject.Find("P3_Arrow_up").GetComponent<Text>();

                P4_Fire1 = GameObject.Find("P4_Fire1").GetComponent<Text>(); P4_Fire2 = GameObject.Find("P4_Fire2").GetComponent<Text>(); P4_Arrow_left = GameObject.Find("P4_Arrow_left").GetComponent<Text>();
                P4_Arrow_down = GameObject.Find("P4_Arrow_down").GetComponent<Text>(); P4_Arrow_right = GameObject.Find("P4_Arrow_right").GetComponent<Text>(); P4_Arrow_up = GameObject.Find("P4_Arrow_up").GetComponent<Text>();

                KB_P1 = GameObject.Find("KB_P1");
                KB_P1.SetActive(false);
                KB_P2 = GameObject.Find("KB_P2");
                KB_P2.SetActive(false);
                KB_P3 = GameObject.Find("KB_P3");
                KB_P3.SetActive(false);
                KB_P4 = GameObject.Find("KB_P4");
                KB_P4.SetActive(false);

                Start_Game = GameObject.Find("Start_Game");
                Start_Game.SetActive(false);
                prev_loaded_scene = loaded_scene;

                title_mode = 0;
            }
            if ((loaded_scene == 2) && (prev_loaded_scene == 1))
            {
                prev_loaded_scene = loaded_scene;
            }
            if ((title_mode == 0) && (loaded_scene == 1))
            {

                loading.SetActive(false);
                floppy.SetActive(false);

                select_game.SetActive(false);
                button_single.SetActive(false);
                button_coop.SetActive(false);
                button_death.SetActive(false);

                Controller_Button_P1.SetActive(false);
                Controller_Button_P2.SetActive(false);
                Controller_Button_P3.SetActive(false);
                Controller_Button_P4.SetActive(false);

                Joypad_P1.SetActive(false);
                Joypad_P2.SetActive(false);
                Joypad_P3.SetActive(false);
                Joypad_P4.SetActive(false);

                KB_P1.SetActive(false);
                KB_P2.SetActive(false);
                KB_P3.SetActive(false);
                KB_P4.SetActive(false);

                Start_Game.SetActive(false);

                //intro credits
                if (time_passed <= 0f) credits_text.text = "";
                if (time_passed > 0f)
                {
                    string hiscores = "HIGH SCORES:\n";
                    for (int i = 1; i <= 10; i++)
                    {
                        hiscores += i + ". " + Hi_score_names[i] + "  " + Hi_score_points[i].ToString("#"); ;
                        if (i != 10) hiscores += "\n";
                    }
                    credits_text.text = hiscores;
                }
                if (time_passed > 8f) credits_text.text = "";
                if (time_passed > 9f) credits_text.text = "Programming:\n\nGrzegorz Korycki\n\n\n";
                if (time_passed > 12f) credits_text.text = "";
                if (time_passed > 13f) credits_text.text = "Music by:\n\nPink of Abyss\n\n(Manfred Linzer)\n\n\n";
                if (time_passed > 16f) credits_text.text = "";
                if (time_passed > 17f) credits_text.text = "Sounds by:\n\nErdie\nNexotron\nWolfsinger\nAnoesj\nSuonho\nLeszek Szary\nJulien Matthey\nDJ Froyd\n\n";
                if (time_passed > 20f) credits_text.text = "";
                if (time_passed > 21f)
                {
                    credits_text.text = "GAME BONUSES:\n\n\n\n\n\n\n\n\n\n";
                    crystal_info.SetActive(true);
                }
                if (time_passed > 28f) { credits_text.text = ""; crystal_info.SetActive(false); }
                if (time_passed > 29f) time_passed = 0f;
                if ((keyboard_button1[1] > 0.3f) || (keyboard_button1[2] > 0.3f) || (keyboard_button1[3] > 0.3f) || (Input.GetMouseButtonDown(0)) || (joypad_button1[1] > 0.3f) || (joypad_button1[2] > 0.3f) || (joypad_button1[3] > 0.3f) || (joypad_button1[4] > 0.3f))
                {
                    game_mode_selected = 0;
                    title_mode = 1;
                    game_mode_select_position = 1;
                    last_menu_move = 5;
                }
            }
            //game mode select
            if ((title_mode == 1) && (loaded_scene == 1))
            {
                crystal_info.SetActive(false);
                select_game.SetActive(true);
                button_single.SetActive(false);
                button_coop.SetActive(true);
                button_death.SetActive(false);

                credits_text.text = "";
                select_game_text.text = "Select Game Type:";
                if (((keyboard_x[1] < -0.3f) || (keyboard_x[2] < -0.3f) || (keyboard_x[3] < -0.3f) || (joypad_x[1] < -0.3f) || (joypad_x[2] < -0.3f) || (joypad_x[3] < -0.3f) || (joypad_x[4] < -0.3f))) { if (last_menu_move != 1) game_mode_select_position--; last_menu_move = 1; }
                else if (((keyboard_x[1] > 0.3f) || (keyboard_x[2] > 0.3f) || (keyboard_x[3] > 0.3f) || (joypad_x[1] > 0.3f) || (joypad_x[2] > 0.3f) || (joypad_x[3] > 0.3f) || (joypad_x[4] > 0.3f))) { if (last_menu_move != 2) game_mode_select_position++; last_menu_move = 2; }
                else if ((keyboard_button1[1] > 0.3f) || (keyboard_button1[2] > 0.3f) || (keyboard_button1[3] > 0.3f) || (joypad_button1[1] > 0.3f) || (joypad_button1[2] > 0.3f) || (joypad_button1[3] > 0.3f) || (joypad_button1[4] > 0.3f)) { if (last_menu_move != 5) game_mode_selected = game_mode_select_position; last_menu_move = 5; game_mode_select_position = 1; }
                else { last_menu_move = 0; }
                if (game_mode_select_position < 2) game_mode_select_position = 2;
                if (game_mode_select_position > 2) game_mode_select_position = 2;

                if (game_mode_selected >= 1)
                {
                    title_mode = 2;
                    game_mode_select_position = 7;
                }

                if (keyboard_escape >= 0.3f)
                {
                    title_mode = 0;
                    prev_keyboard_escape = keyboard_escape;
                }

            }

            //controller select
            if ((title_mode == 2) && (loaded_scene == 1))
            {
                //chose controllers menu
                select_game.SetActive(false);
                button_single.SetActive(false);
                button_coop.SetActive(false);
                button_death.SetActive(false);
                select_game_text.text = "Choose Controllers:";

                if (game_mode_selected >= 1)
                {
                    if (game_mode_selected == 1)
                    {
                        player_controller_scheme[2] = 8; player_controller_scheme[3] = 8; player_controller_scheme[4] = 8;
                    }
                    if ((player_controller_scheme[1] == 8) && (player_controller_scheme[2] == 8) && (player_controller_scheme[3] == 8) && (player_controller_scheme[4] == 8))
                        Start_Game.SetActive(false);
                    else
                        Start_Game.SetActive(true);

                    Controller_Button_P1.SetActive(true);
                    if (player_controller_scheme[1] <= 4)
                    {
                        Joypad_P1.SetActive(true);
                        KB_P1.SetActive(false);
                        Joypad_P1_no.text = "" + player_controller_scheme[1];
                        if (player_controller_profile[1] == 0) { button_rt[1].SetActive(true); button_lt[1].SetActive(true); button_up[1].SetActive(false); button_down[1].SetActive(false); }
                        else { button_rt[1].SetActive(false); button_lt[1].SetActive(false); button_up[1].SetActive(true); button_down[1].SetActive(true); }
                    }
                    else
                    {
                        Joypad_P1.SetActive(false);
                        KB_P1.SetActive(true);
                        if (player_controller_scheme[1] == 5)
                        {
                            P1_Fire1.GetComponent<Text>().text = "A";
                            P1_Fire2.GetComponent<Text>().text = "S";
                            P1_Arrow_left.GetComponent<Text>().text = "D";
                            P1_Arrow_down.GetComponent<Text>().text = "F";
                            P1_Arrow_right.GetComponent<Text>().text = "G";
                            P1_Arrow_up.GetComponent<Text>().text = "R";
                        }
                        if (player_controller_scheme[1] == 6)
                        {
                            P1_Fire1.GetComponent<Text>().text = "K";
                            P1_Fire2.GetComponent<Text>().text = "L";
                            P1_Arrow_left.GetComponent<Text>().text = "LEFT";
                            P1_Arrow_down.GetComponent<Text>().text = "DOWN";
                            P1_Arrow_right.GetComponent<Text>().text = "RIGHT";
                            P1_Arrow_up.GetComponent<Text>().text = "UP";
                        }
                        if (player_controller_scheme[1] == 7)
                        {
                            P1_Fire1.GetComponent<Text>().text = "Num 1";
                            P1_Fire2.GetComponent<Text>().text = "Num 2";
                            P1_Arrow_left.GetComponent<Text>().text = "Num 4";
                            P1_Arrow_down.GetComponent<Text>().text = "Num 5";
                            P1_Arrow_right.GetComponent<Text>().text = "Num 6";
                            P1_Arrow_up.GetComponent<Text>().text = "Num 8";
                        }
                        if (player_controller_scheme[1] == 8)
                        {
                            Joypad_P1.SetActive(false);
                            KB_P1.SetActive(false);
                        }
                    }
                }
                if (game_mode_selected >= 2)
                {
                    Controller_Button_P2.SetActive(true);
                    Controller_Button_P3.SetActive(true);
                    Controller_Button_P4.SetActive(true);
                    if (player_controller_scheme[2] <= 4)
                    {
                        Joypad_P2.SetActive(true);
                        KB_P2.SetActive(false);
                        Joypad_P2_no.text = "" + player_controller_scheme[2];
                        if (player_controller_profile[2] == 0) { button_rt[2].SetActive(true); button_lt[2].SetActive(true); button_up[2].SetActive(false); button_down[2].SetActive(false); }
                        else { button_rt[2].SetActive(false); button_lt[2].SetActive(false); button_up[2].SetActive(true); button_down[2].SetActive(true); }
                    }
                    else
                    {
                        Joypad_P2.SetActive(false);
                        KB_P2.SetActive(true);
                    }
                    if (player_controller_scheme[2] == 5)
                    {
                        P2_Fire1.GetComponent<Text>().text = "A";
                        P2_Fire2.GetComponent<Text>().text = "S";
                        P2_Arrow_left.GetComponent<Text>().text = "D";
                        P2_Arrow_down.GetComponent<Text>().text = "F";
                        P2_Arrow_right.GetComponent<Text>().text = "G";
                        P2_Arrow_up.GetComponent<Text>().text = "R";
                    }
                    if (player_controller_scheme[2] == 6)
                    {
                        P2_Fire1.GetComponent<Text>().text = "K";
                        P2_Fire2.GetComponent<Text>().text = "L";
                        P2_Arrow_left.GetComponent<Text>().text = "LEFT";
                        P2_Arrow_down.GetComponent<Text>().text = "DOWN";
                        P2_Arrow_right.GetComponent<Text>().text = "RIGHT";
                        P2_Arrow_up.GetComponent<Text>().text = "UP";
                    }
                    if (player_controller_scheme[2] == 7)
                    {
                        P2_Fire1.GetComponent<Text>().text = "Num 1";
                        P2_Fire2.GetComponent<Text>().text = "Num 2";
                        P2_Arrow_left.GetComponent<Text>().text = "Num 4";
                        P2_Arrow_down.GetComponent<Text>().text = "Num 5";
                        P2_Arrow_right.GetComponent<Text>().text = "Num 6";
                        P2_Arrow_up.GetComponent<Text>().text = "Num 8";
                    }
                    if (player_controller_scheme[2] == 8)
                    {
                        Joypad_P2.SetActive(false);
                        KB_P2.SetActive(false);
                    }

                    if (player_controller_scheme[3] <= 4)
                    {
                        Joypad_P3.SetActive(true);
                        KB_P3.SetActive(false);
                        Joypad_P3_no.text = "" + player_controller_scheme[3];
                        if (player_controller_profile[3] == 0) { button_rt[3].SetActive(true); button_lt[3].SetActive(true); button_up[3].SetActive(false); button_down[3].SetActive(false); }
                        else { button_rt[3].SetActive(false); button_lt[3].SetActive(false); button_up[3].SetActive(true); button_down[3].SetActive(true); }
                    }
                    else
                    {
                        Joypad_P3.SetActive(false);
                        KB_P3.SetActive(true);
                    }
                    if (player_controller_scheme[3] == 5)
                    {
                        P3_Fire1.GetComponent<Text>().text = "A";
                        P3_Fire2.GetComponent<Text>().text = "S";
                        P3_Arrow_left.GetComponent<Text>().text = "D";
                        P3_Arrow_down.GetComponent<Text>().text = "F";
                        P3_Arrow_right.GetComponent<Text>().text = "G";
                        P3_Arrow_up.GetComponent<Text>().text = "R";
                    }
                    if (player_controller_scheme[3] == 6)
                    {
                        P3_Fire1.GetComponent<Text>().text = "K";
                        P3_Fire2.GetComponent<Text>().text = "L";
                        P3_Arrow_left.GetComponent<Text>().text = "LEFT";
                        P3_Arrow_down.GetComponent<Text>().text = "DOWN";
                        P3_Arrow_right.GetComponent<Text>().text = "RIGHT";
                        P3_Arrow_up.GetComponent<Text>().text = "UP";
                    }
                    if (player_controller_scheme[3] == 7)
                    {
                        P3_Fire1.GetComponent<Text>().text = "Num 1";
                        P3_Fire2.GetComponent<Text>().text = "Num 2";
                        P3_Arrow_left.GetComponent<Text>().text = "Num 4";
                        P3_Arrow_down.GetComponent<Text>().text = "Num 5";
                        P3_Arrow_right.GetComponent<Text>().text = "Num 6";
                        P3_Arrow_up.GetComponent<Text>().text = "Num 8";
                    }
                    if (player_controller_scheme[3] == 8)
                    {
                        Joypad_P3.SetActive(false);
                        KB_P3.SetActive(false);
                    }

                    if (player_controller_scheme[4] <= 4)
                    {
                        Joypad_P4.SetActive(true);
                        KB_P4.SetActive(false);
                        Joypad_P4_no.text = "" + player_controller_scheme[4];
                        if (player_controller_profile[4] == 0) { button_rt[4].SetActive(true); button_lt[4].SetActive(true); button_up[4].SetActive(false); button_down[4].SetActive(false); }
                        else { button_rt[4].SetActive(false); button_lt[4].SetActive(false); button_up[4].SetActive(true); button_down[4].SetActive(true); }
                    }
                    else
                    {
                        Joypad_P4.SetActive(false);
                        KB_P4.SetActive(true);
                    }
                    if (player_controller_scheme[4] == 5)
                    {
                        P4_Fire1.GetComponent<Text>().text = "A";
                        P4_Fire2.GetComponent<Text>().text = "S";
                        P4_Arrow_left.GetComponent<Text>().text = "D";
                        P4_Arrow_down.GetComponent<Text>().text = "F";
                        P4_Arrow_right.GetComponent<Text>().text = "G";
                        P4_Arrow_up.GetComponent<Text>().text = "R";
                    }
                    if (player_controller_scheme[4] == 6)
                    {
                        P4_Fire1.GetComponent<Text>().text = "K";
                        P4_Fire2.GetComponent<Text>().text = "L";
                        P4_Arrow_left.GetComponent<Text>().text = "LEFT";
                        P4_Arrow_down.GetComponent<Text>().text = "DOWN";
                        P4_Arrow_right.GetComponent<Text>().text = "RIGHT";
                        P4_Arrow_up.GetComponent<Text>().text = "UP";
                    }
                    if (player_controller_scheme[4] == 7)
                    {
                        P4_Fire1.GetComponent<Text>().text = "Num 1";
                        P4_Fire2.GetComponent<Text>().text = "Num 2";
                        P4_Arrow_left.GetComponent<Text>().text = "Num 4";
                        P4_Arrow_down.GetComponent<Text>().text = "Num 5";
                        P4_Arrow_right.GetComponent<Text>().text = "Num 6";
                        P4_Arrow_up.GetComponent<Text>().text = "Num 8";
                    }
                    if (player_controller_scheme[4] == 8)
                    {
                        Joypad_P4.SetActive(false);
                        KB_P4.SetActive(false);
                    }
                }
                else
                {
                    Joypad_P2.SetActive(false);
                    KB_P2.SetActive(false);
                    Joypad_P3.SetActive(false);
                    KB_P3.SetActive(false);
                    Joypad_P3.SetActive(false);
                    KB_P3.SetActive(false);
                }
                title_mode3_last = 0f;
                if (((keyboard_x[1] < -0.3f) || (keyboard_x[2] < -0.3f) || (keyboard_x[3] < -0.3f) || (joypad_x[1] < -0.3f) || (joypad_x[2] < -0.3f) || (joypad_x[3] < -0.3f) || (joypad_x[4] < -0.3f))) { if (last_menu_move != 1) if (game_mode_select_position != 6) game_mode_select_position--; last_menu_move = 1; }
                else if (((keyboard_x[1] > 0.3f) || (keyboard_x[2] > 0.3f) || (keyboard_x[3] > 0.3f) || (joypad_x[1] > 0.3f) || (joypad_x[2] > 0.3f) || (joypad_x[3] > 0.3f) || (joypad_x[4] > 0.3f))) { if (last_menu_move != 2) if (game_mode_select_position != 6) game_mode_select_position++; last_menu_move = 2; }
                else if (((keyboard_y[1] > 0.3f) || (keyboard_y[2] > 0.3f) || (keyboard_y[3] > 0.3f) || (joypad_y[1] > 0.3f) || (joypad_y[2] > 0.3f) || (joypad_y[3] > 0.3f) || (joypad_y[4] > 0.3f))) { if (last_menu_move != 3) game_mode_select_position = 1; last_menu_move = 3; }
                else if (((keyboard_y[1] < -0.3f) || (keyboard_y[2] < -0.3f) || (keyboard_y[3] < -0.3f) || (joypad_y[1] < -0.3f) || (joypad_y[2] < -0.3f) || (joypad_y[3] < -0.3f) || (joypad_y[4] < -0.3f))) { if (last_menu_move != 4) game_mode_select_position = 6; last_menu_move = 4; }
                else if ((keyboard_button1[1] > 0.3f) || (keyboard_button1[2] > 0.3f) || (keyboard_button1[3] > 0.3f) || (joypad_button1[1] > 0.3f) || (joypad_button1[2] > 0.3f) || (joypad_button1[3] > 0.3f) || (joypad_button1[4] > 0.3f)) { if ((last_menu_move != 5) && (game_mode_select_position <= 4)) change_controller(game_mode_select_position); last_menu_move = 5; if (game_mode_select_position == 6) title_mode = 3; }
                else { last_menu_move = 0; }
                if (game_mode_select_position == 5) game_mode_select_position = 4;
                if (game_mode_select_position < 1) game_mode_select_position = 1;
                if ((player_controller_scheme[1] == 8) && (player_controller_scheme[2] == 8) && (player_controller_scheme[3] == 8) && (player_controller_scheme[4] == 8) && (game_mode_select_position == 6)) game_mode_select_position = 1;
                if (keyboard_escape > 0.3f)
                {
                    title_mode = 0;
                    prev_keyboard_escape = keyboard_escape;
                }

            }
            if ((title_mode == 3))
            {
                crystal_info.SetActive(false);
                select_game.SetActive(false);
                button_single.SetActive(false);
                button_coop.SetActive(false);
                button_death.SetActive(false);

                select_game.SetActive(false);
                button_single.SetActive(false);
                button_coop.SetActive(false);
                button_death.SetActive(false);
                select_game_text.text = "PLEASE WAIT";

                Controller_Button_P1.SetActive(false);
                Controller_Button_P2.SetActive(false);
                Controller_Button_P3.SetActive(false);
                Controller_Button_P4.SetActive(false);

                Joypad_P1.SetActive(false);
                KB_P1.SetActive(false);
                Joypad_P2.SetActive(false);
                KB_P2.SetActive(false);
                Joypad_P3.SetActive(false);
                KB_P3.SetActive(false);
                Joypad_P3.SetActive(false);
                KB_P3.SetActive(false);

                loading.SetActive(true);
                floppy.SetActive(true);
                Start_Game.SetActive(false);

                if (title_mode3_last > 0.3f)
                    title_mode = 4;
                title_mode3_last += Time.deltaTime;
            }

            if ((title_mode == 4))
            {

                title_mode = 2;
                number_of_players = 1;
                game_mode = 1;
                title_mode = 0;
                loaded_scene = 2;
                SceneManager.LoadScene(2);
            }
        }

        if (prev_keyboard_escape != keyboard_escape)
        {
            if (keyboard_escape >= 0.3f)
            {
                if((title_mode == 0)&&(loaded_scene==1))
                {
                    Application.Quit();
                }
            }
            if (keyboard_escape < 0.3f) prev_keyboard_escape = keyboard_escape;
        }

        if(loaded_scene == 2)
        {
            //Time.timeScale = 0;
        }
        time_passed += Time.deltaTime;
    }

    public void change_controller(int changed_controller)
    {
        int while_count = 100;
        bool slot_free = false;
        while (slot_free == false)
        {
            //player_controller_profile
            if (changed_controller == 1)
            {
                if (player_controller_scheme[1] >= 5) player_controller_scheme[1]++;
                else { if (player_controller_profile[1] == 0) player_controller_profile[1]++; else { player_controller_profile[1] = 0; player_controller_scheme[1]++; } }
                if (player_controller_scheme[1] > 8) player_controller_scheme[1] = 1;
                if (((player_controller_scheme[1] == player_controller_scheme[2]) || (player_controller_scheme[1] == player_controller_scheme[3]) || (player_controller_scheme[1] == player_controller_scheme[4])) && (player_controller_scheme[1] != 8)) slot_free = false;
                else slot_free = true;
            }
            if (changed_controller == 2)
            {
                if (player_controller_scheme[2] >= 5) player_controller_scheme[2]++;
                else { if (player_controller_profile[2] == 0) player_controller_profile[2]++; else { player_controller_profile[2] = 0; player_controller_scheme[2]++; } }
                if (player_controller_scheme[2] > 8) player_controller_scheme[2] = 1;
                if (((player_controller_scheme[2] == player_controller_scheme[1]) || (player_controller_scheme[2] == player_controller_scheme[3]) || (player_controller_scheme[2] == player_controller_scheme[4])) && (player_controller_scheme[2] != 8)) slot_free = false;
                else slot_free = true;
            }
            if (changed_controller == 3)
            {
                if (player_controller_scheme[3] >= 5) player_controller_scheme[3]++;
                else { if (player_controller_profile[3] == 0) player_controller_profile[3]++; else { player_controller_profile[3] = 0; player_controller_scheme[3]++; } }
                if (player_controller_scheme[3] > 8) player_controller_scheme[3] = 1;
                if (((player_controller_scheme[3] == player_controller_scheme[1]) || (player_controller_scheme[3] == player_controller_scheme[2]) || (player_controller_scheme[3] == player_controller_scheme[4])) && (player_controller_scheme[3] != 8)) slot_free = false;
                else slot_free = true;
            }
            if (changed_controller == 4)
            {
                if (player_controller_scheme[4] >= 5) player_controller_scheme[4]++;
                else { if (player_controller_profile[4] == 0) player_controller_profile[4]++; else { player_controller_profile[4] = 0; player_controller_scheme[4]++; } }
                if (player_controller_scheme[4] > 8) player_controller_scheme[4] = 1;
                if (((player_controller_scheme[4] == player_controller_scheme[1]) || (player_controller_scheme[4] == player_controller_scheme[2]) || (player_controller_scheme[4] == player_controller_scheme[3])) && (player_controller_scheme[4] != 8)) slot_free = false;
                else slot_free = true;
            }
            while_count--;
            if (while_count < 0) break;
        }

    }
    public void Start_game()
    {
        title_mode = 3;
    }

}
