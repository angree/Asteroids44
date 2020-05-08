using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_script : MonoBehaviour
{
    Global_Variables _GlobalVariables;
    GameObject GC;

    General_settings _General_settings;
    GameObject GS;

    GameObject asteroid_explosion;

    private GameObject this_object;
    private Vector3 scaleChange;
    private Rigidbody m_Rigidbody;

    public GameObject[] asteroid_array = new GameObject[12];
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

    private float pos_x_limit = 107.5f;
    private float pos_y_limit = 62.2f;
    private float min_speed = 2f;
    private float max_speed = 7f;
    private float life_time = 0f;
    private float[] asteroid_size_modify = new float[12];

    public float crystal_probability = 0f;
    public float bitcoin_probability = 0f;
    public float bomb_probability = 0f;
    public int asteroid_model = 6;
    public float asteroid_size = 5f;
    public float asteroid_hp = 100;
    public int crystal_type = 0;
    public float asteroid_hit = 20f;
    public float  base_hp = 5f;
    public float drop_chance = 18f;
    public float drop_chance_bitcoin = 38f;
    public float drop_chance_drop_per_level = 3f;
    public float drop_chance_btc_drop_per_level = 3f;
    public float drop_chance_min = 3f;
    public float drop_chance_bomb = 5f;
    public float drop_chance_bomb_drop_per_level = 0.5f;
    public float drop_chance_multi_per_player = 0.3f;
    public float velocity_x = 0f;
    public float velocity_y = 0f;

    void Start()
    {

        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();

        GS = GameObject.Find("Settings_holder");
        if (GS != null) _General_settings = GS.GetComponent<General_settings>();

        asteroid_explosion = _GlobalVariables.asteroid_explosion;
        for (int i = 6; i <= 11; i++)
        {
            asteroid_array[i] = _GlobalVariables.asteroid_prefab[i];
        }

        asteroid_health = _GlobalVariables.asteroid_health;
        asteroid_shot = _GlobalVariables.asteroid_shot;
        asteroid_max_health = _GlobalVariables.asteroid_max_health;
        asteroid_bitcoin = _GlobalVariables.asteroid_bitcoin;

        drop_chance = _GlobalVariables.drop_chance;
        drop_chance_drop_per_level = _GlobalVariables.drop_chance_drop_per_level;

        drop_chance_btc_drop_per_level = _GlobalVariables.drop_chance_btc_drop_per_level;
        drop_chance_bitcoin = _GlobalVariables.drop_chance_bitcoin;

        drop_chance_bomb = _GlobalVariables.drop_chance_bomb;
        drop_chance_bomb_drop_per_level = _GlobalVariables.drop_chance_bomb_drop_per_level;

        drop_chance_min = _GlobalVariables.drop_chance_min;
        drop_chance_multi_per_player = _GlobalVariables.drop_chance_multi_per_player;


        asteroid_crystal_red = _GlobalVariables.asteroid_crystal_red;
        asteroid_crystal_green = _GlobalVariables.asteroid_crystal_green;
        asteroid_crystal_yellow = _GlobalVariables.asteroid_crystal_yellow;
        bitcoin_coin = _GlobalVariables.bitcoin_coin;
        bomb_model = _GlobalVariables.bomb_model;
        shot_split_model = _GlobalVariables.shot_split_model;

        min_speed = (3f + _GlobalVariables.level_number)*2.5f;
        max_speed = (4f + (_GlobalVariables.level_number*1.5f))*2.5f;

        this_object = this.gameObject;
        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.AddForce(transform.up * Random.Range(450f*10f, 750f * 10f));
        asteroid_hp = (base_hp * asteroid_size) * (1 + ((_GlobalVariables.level_number - 1) * 0.4f));
        if(_GlobalVariables.level_number >=9)
        {
            for(int i = 9; i <= _GlobalVariables.level_number; i++)
            {
                asteroid_hp = asteroid_hp * 1.1f;
            }
        }
        //equalizing visual sizes of asteroids
        asteroid_size_modify[6] = 1f; asteroid_size_modify[7] = 1.1f; asteroid_size_modify[8] = 1.2f;
        asteroid_size_modify[9] = 1.05f; asteroid_size_modify[10] = 0.75f; asteroid_size_modify[11] = 1.15f;
    }

    private int last_player_to_hit_asteroid = 0;
    void OnTriggerEnter(Collider col)
    {
        bool hit_by_player = false;
        if (life_time > 0.1f)
        {
            if (col.GetComponent<BulletScript>() != null)
            {
                asteroid_hp -= asteroid_hit;
                //Debug.Log("player " + col.GetComponent<BulletScript>().player_number + " hit the asteroid!");
                last_player_to_hit_asteroid = col.GetComponent<BulletScript>().player_number;
                hit_by_player = true;
            }
        }
        if (col.GetComponent<bomb_collider_dummy>() != null)
        {
            asteroid_hp -= 1000f;
            //Debug.Log("explosion from player" + col.GetComponent<bomb_collider_dummy>().player_number + " hit the asteroid!");
            last_player_to_hit_asteroid = col.GetComponent<bomb_collider_dummy>().player_number;
            hit_by_player = true;
        }
        if (hit_by_player)
        {
            if (_GlobalVariables.asteroid_explosion_timer > _GlobalVariables.player_score_multipler_last_refresh[last_player_to_hit_asteroid] + _GlobalVariables.score_multipler_time_limit)
            {
                _GlobalVariables.player_score_multipler[last_player_to_hit_asteroid] = 1.0f;
            }
            _GlobalVariables.player_score_multipler_last_refresh[last_player_to_hit_asteroid] = _GlobalVariables.asteroid_explosion_timer;
        }

    }

    void Update()
    {

        _GlobalVariables.asteroid_timer = 0f;

        
        if(asteroid_size>0)
        {
            scaleChange = new Vector3(0.5f * asteroid_size * 10f * asteroid_size_modify[asteroid_model], 0.5f * asteroid_size * 10f * asteroid_size_modify[asteroid_model], 0.5f * asteroid_size * 10f * asteroid_size_modify[asteroid_model]);
            if (crystal_type == 1) scaleChange = new Vector3(0.5f * 1.7f * 10f, 0.5f * 1.7f * 10f, 0.5f * 1.7f * 10f);
            if (crystal_type == 2) scaleChange = new Vector3(0.5f * 2.3f * 10f, 0.5f * 2.3f * 10f, 0.5f * 2.3f * 10f);
            if (crystal_type == 3) scaleChange = new Vector3(0.5f * 2.0f * 10f, 0.5f * 2.0f * 10f, 0.5f * 2.0f * 10f);
            this_object.transform.localScale = scaleChange;
            m_Rigidbody.mass = 2.5f * asteroid_size;
            pos_x_limit = (10.75f + ((asteroid_size-1) * 0.185f))*10f;
            pos_y_limit = (6.22f + ((asteroid_size-1) * 0.16f))*10f;
        }

        if (transform.localPosition.x < -pos_x_limit) transform.localPosition = new Vector3(transform.localPosition.x + (2 * pos_x_limit), transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.x > pos_x_limit) transform.localPosition = new Vector3(transform.localPosition.x - (2 * pos_x_limit), transform.localPosition.y, transform.localPosition.z);

        if (transform.localPosition.y < -pos_y_limit) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (2 * pos_y_limit), transform.localPosition.z);
        if (transform.localPosition.y > pos_y_limit) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - (2 * pos_y_limit), transform.localPosition.z);

        float rand = Random.Range(0f, 100f);
        //Debug.Log("random split = "+rand);
        float bitcoin_to_crystal_ratio = drop_chance_bitcoin / drop_chance;
        float bomb_to_crystal_ratio = drop_chance_bomb / drop_chance;

        float drop_chance_calc = (drop_chance - (drop_chance_drop_per_level * (_GlobalVariables.level_number - 1)));
        if (drop_chance_calc < drop_chance_min) drop_chance_calc = drop_chance_min;
        drop_chance_calc = drop_chance_calc * (1 + (drop_chance_multi_per_player * (_GlobalVariables.how_many_players - 1)));

        float drop_chance_calc_btc = (drop_chance_bitcoin - (drop_chance_btc_drop_per_level * (_GlobalVariables.level_number - 1)));
        if (drop_chance_calc_btc < (drop_chance_min * bitcoin_to_crystal_ratio)) drop_chance_calc_btc = (drop_chance_min * bitcoin_to_crystal_ratio);
        drop_chance_calc_btc = drop_chance_calc_btc * (1 + (drop_chance_multi_per_player * (_GlobalVariables.how_many_players - 1)));

        float drop_chance_calc_bomb = (drop_chance_bomb - (drop_chance_bomb_drop_per_level * (_GlobalVariables.level_number - 1)));
        if (drop_chance_calc_bomb < (drop_chance_min * bomb_to_crystal_ratio)) drop_chance_calc_bomb = (drop_chance_min * bomb_to_crystal_ratio);
        drop_chance_calc_bomb = drop_chance_calc_bomb * (1 + (drop_chance_multi_per_player * (_GlobalVariables.how_many_players - 1)));
        
        crystal_probability = drop_chance_calc;
        bitcoin_probability = drop_chance_calc_btc;
        bomb_probability = drop_chance_calc_bomb;

        if (asteroid_hp<=0)
        {
            int asteroid_random_number = Random.Range(6, 12);
            if (asteroid_size < 2.5f)
            {
                _GlobalVariables.Score_account[last_player_to_hit_asteroid] += (200f*_GlobalVariables.level_number_score_calc) *_GlobalVariables.player_score_multipler[last_player_to_hit_asteroid];
                //SPAWN CRYSTALS
                if (crystal_type == 1)
                {
                    //red
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_crystal_red, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                }
                else if (crystal_type == 2)
                {
                    //yellow
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_crystal_yellow, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                }
                else if (crystal_type == 3)
                {
                    //green
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_crystal_green, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                }
                else if (crystal_type == 4)
                {
                    //btc
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(bitcoin_coin, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                }
                else if (crystal_type == 5)
                {
                    //bomb
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(bomb_model, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                }
                else if (crystal_type == 6)
                {
                    //shot split crystal
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(shot_split_model, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                }
            }
            else if (asteroid_size==3f)
            {
                _GlobalVariables.Score_account[last_player_to_hit_asteroid] += (300f * _GlobalVariables.level_number_score_calc) *_GlobalVariables.player_score_multipler[last_player_to_hit_asteroid];
                //here add other crystals if needed

                if (rand<= drop_chance_calc)
                {
                    //red hp
                    //Debug.Log("> HP ASTEROID");
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_health, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                }
                else if ((rand > drop_chance_calc) && (rand <= (drop_chance_calc * 2)))
                {
                    //yellow shot
                    //Debug.Log("> shot ASTEROID");
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_shot, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                }
                else if ((rand > (drop_chance_calc * 2)) && (rand <= (drop_chance_calc * 3)))
                {
                    //green max hp
                    //Debug.Log("> max HP ASTEROID");
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_max_health, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                }
                else if ((rand > (drop_chance_calc * 3)) && (rand <= (drop_chance_calc * 3.5f)))
                {
                    //asteroid blue crystal (shot split)
                    //Debug.Log("> SHOT SPLIT ASTEROID");
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                    last_gameobject.GetComponent<Asteroid_script>().crystal_type = 6;
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                }
                else if ((rand > (drop_chance_calc * 3.5f )) && (rand <= ((drop_chance_calc * 3.5f ) + drop_chance_calc_btc)))
                {
                    //btc
                    //Debug.Log("> BITCOIN ASTEROID");
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_bitcoin, transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;

                }
                else if ((rand > ((drop_chance_calc * 3.5f) + drop_chance_calc_btc)) && (rand <= (((drop_chance_calc * 3.5f) + drop_chance_calc_btc) + drop_chance_calc_bomb)))
                {
                    //btc
                    //Debug.Log("> BOMB ASTEROID");
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                    last_gameobject.GetComponent<Asteroid_script>().crystal_type = 5;

                    asteroid_random_number = Random.Range(6, 12);
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;

                }
                else
                {
                    //normal
                    GameObject last_gameobject;
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;

                    asteroid_random_number = Random.Range(6, 12);
                    last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                    last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                    last_gameobject.GetComponent<Asteroid_script>().asteroid_size = 2;
                }
            }
            else if (asteroid_size > 2f)
            {
                _GlobalVariables.Score_account[last_player_to_hit_asteroid] += (asteroid_size*100f * _GlobalVariables.level_number_score_calc)*_GlobalVariables.player_score_multipler[last_player_to_hit_asteroid];
                GameObject last_gameobject;
                last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                last_gameobject.GetComponent<Asteroid_script>().asteroid_size = asteroid_size-1f;

                asteroid_random_number = Random.Range(6, 12);
                last_gameobject = Instantiate(asteroid_array[asteroid_random_number], transform.position, transform.rotation);
                last_gameobject.transform.parent = GameObject.Find("zero_object").transform;
                last_gameobject.GetComponent<Asteroid_script>().asteroid_size = asteroid_size - 1f;

            }
            
            GameObject last_gameobject2;

            bool explosion_allowed = false;
            int max_explosions = 10;
            for (int i = _GlobalVariables.last_asteroid_explosion_pos; i > (_GlobalVariables.last_asteroid_explosion_pos - max_explosions); i--)
            {
                int current_pos = i;
                if (current_pos < 0) current_pos += 10;
                if (_GlobalVariables.last_asteroid_explosion[current_pos] < _GlobalVariables.asteroid_explosion_timer) explosion_allowed = true;
            }
            if(explosion_allowed==true)
            {
                last_gameobject2 = Instantiate(asteroid_explosion, transform.position, transform.rotation);
                last_gameobject2.transform.parent = GameObject.Find("zero_object").transform;
                _GlobalVariables.last_asteroid_explosion_pos++;
                if (_GlobalVariables.last_asteroid_explosion_pos > 9) _GlobalVariables.last_asteroid_explosion_pos -= 10;
                _GlobalVariables.last_asteroid_explosion[_GlobalVariables.last_asteroid_explosion_pos] = _GlobalVariables.asteroid_explosion_timer;
            }
            _GlobalVariables.player_score_multipler[last_player_to_hit_asteroid] += _GlobalVariables.score_multiupler_up_per_hit;
            if (_GlobalVariables.player_score_multipler[last_player_to_hit_asteroid] > 50f) _GlobalVariables.player_score_multipler[last_player_to_hit_asteroid] = 50f;
            //Debug.Log("increasing player " + last_player_to_hit_asteroid + " multipler to " + _GlobalVariables.player_score_multipler[last_player_to_hit_asteroid]);
            Destroy(this.gameObject);
            
        }

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);

        velocity_x = m_Rigidbody.velocity.x;
        velocity_y = m_Rigidbody.velocity.y;
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
            m_Rigidbody.velocity = new Vector3(velocity_x* velocity_modifier, velocity_y * velocity_modifier, 0f);
            velocity_x = m_Rigidbody.velocity.x;
            velocity_y = m_Rigidbody.velocity.y;
        }

        //counting HP damage
        velocity_x = m_Rigidbody.velocity.x;
        velocity_y = m_Rigidbody.velocity.y;
        //count moved to player_01_move script
        
        life_time += Time.deltaTime;
    }
}
