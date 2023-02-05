using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TendrilAI : MonoBehaviour
{
    public Transform[] points;

    public bool die = false;
    float children;
    int pointI;
    float randT = 0;
    float setT = 1;
    public float attackRange = 6;
    GameObject player;
    bool moving = false;
    bool attacking = false;
    bool fleeing = false;
    Vector3 target;
    Vector3 home;
    public float speed = 2f;
    int oxygen;
    int targetO;
    public int damage = 10;
    PlayerMovement damagingC;
    // Start is called before the first frame update
    void Start()
    {
        /*player = GameObject.FindGameObjectsWithTag("Respawn")[0];
        children = transform.childCount;
        for(int i = 0; i < children; i++)
        {
            if (transform.GetChild(i).tag == "Point")
            {
                points[pointI] = transform.GetChild(i);
                pointI++;
            }
        }
        points[pointI] = player.transform;*/
        target = points[0].position;
        target.x -= 6;
        home = target;
    }

    // Update is called once per frame
    void Update()
    {
        randT += Time.deltaTime;
        if (randT > setT)
        {
            if (!moving && !attacking && !fleeing)
            {
                setT += 1;
                Teleport(points);
            }
            else
            {
                setT += 5;
            }
        }
        if(die == true) //and animation is complete
        {
            Destroy(gameObject);
        }
        if (fleeing)
        {
            attacking = false;
            moving = false;
            var step = speed * 1.8f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, home, step);
            if (Vector3.Distance(transform.position, target) < 0.001f)
            {
                fleeing = false;
            }
        }
        if (moving)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if (Vector3.Distance(transform.position, target) < 0.001f)
            {
                moving = false;
                target = home;
            }
        }
        if (attacking == true)
        {
            target = AcquireTarget();
            var step = speed * Time.deltaTime;
            var tick = damage * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                oxygen = damagingC.Damage(tick); 
                if(oxygen <= targetO)
                {
                    attacking = false;
                    target = home;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //play animation
        if(collision.tag == "Kill")
            die = true;
        if (collision.tag == "Scare")
            fleeing = true;
    }
    public void Teleport(Transform[] locations)
    {
        float randValue = UnityEngine.Random.value;
        int playC = 0;
        float big = 0;
        float totalC = 0;
        float weightedC = 0.25f;
        bool boolC = false;
        if (randValue > .25f)
        {
            Chance[] chance = new Chance[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].tag == "Player")
                {
                    if (Vector3.Distance(points[i].position, transform.position) < attackRange)
                    {
                        chance[i - playC] = new Chance(points[i].position, Vector3.Distance(points[i].position, transform.position), points[i]);
                        if (chance[i - playC].GetDistance() > big)
                        {
                            big = chance[i - playC].GetDistance();
                        }
                    }
                    else
                    {
                        boolC = true;
                    }
                }
                else if (Vector3.Distance(points[i].position, transform.position) > 1)
                {
                    chance[i - playC] = new Chance(points[i].position, Vector3.Distance(points[i].position, transform.position));
                    if (chance[i - playC].GetDistance() > big)
                    {
                        Debug.Log("Old Big: " + big);
                        big = chance[i - playC].GetDistance();
                        Debug.Log("New Big: " + big);
                    }
                }
                else
                    boolC = true;
                if (boolC)
                {
                    playC++;
                    Array.Resize(ref chance, chance.Length - 1);
                    boolC = false;
                }
            }
            float[] chanceTable = new float[chance.Length];
            Debug.Log("Big: " + big); 
            for (int i = 0; i < chance.Length; i++)
            {
                Debug.Log("");
            }
                for (int i = 0; i < chance.Length; i++)
            {
                chanceTable[i] = (chance[i].GetDistance() / (chance[i].GetDistance() / big));
                totalC += chanceTable[i];
                //Debug.Log(chanceTable[i]);
                //Debug.Log(totalC);
            }
            Debug.Log("Done");
            for (int i = 0; i < chanceTable.Length; i++)
            {
                chanceTable[i] = 0.75f / (chanceTable[i] / totalC) + weightedC;
                weightedC += chanceTable[i];
            }
            float lastValue = 0.25f;
            bool stop = false;
            for (int i = 0; i < chanceTable.Length; i++)
            {
                if(randValue < chanceTable[i] && randValue >= lastValue)
                {
                    if (chance[i].GetP())
                    {
                        Attack(chance[i].GetPl());
                    }
                    else {
                        Move(chance[i].GetLoc());
                    }
                    stop = true;
                }
                lastValue += chanceTable[i];
                if (stop)
                {
                    i = chanceTable.Length;
                }
            }
        }
    }
    public void Move(Vector3 goTo)
    {
        moving = true;
        target = goTo;
    }

    public void Attack(Transform chara)
    {
        attacking = true;
        oxygen = chara.GetComponent<PlayerMovement>().oxygen;
    }
    public Vector3 AcquireTarget()
    {
        return target;
    }
    public class Chance
    {
        private Vector3 location;
        private float distance;
        private bool playing = false;
        private Transform play;

        public Chance(Vector3 l, float d)
        {
            location = l;
            distance = d;
        }
        public Chance(Vector3 l, float d, Transform p)
        {
            location = l;
            distance = d / 2;
            playing = true;
        }

        public float GetDistance()
        {
            if (playing)
            {
                return distance / 2;
            }
            return distance;
        }
        public bool GetP()
        {
            return playing;
        }
        public Vector3 GetLoc()
        {
            return location;
        }
        public Transform GetPl()
        {
            return play;
        }
    }
}
