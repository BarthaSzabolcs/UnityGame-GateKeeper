using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Nincs kedvem szétkommentezni hogy fel lehessen fogni. Nem kell ide benézni :D
public class FairyController : MonoBehaviour
{
    public string attackAudio;

    public int maxSpeed;
    public int attackSpeed;
    
    public Sprite CuteFairy;
    public Sprite AttackFairy;

    public DamageZoneData data;

    private Rigidbody2D rgBD;
    private SpriteRenderer spriteRenderer;
    private string rotation;
    private float attackDistance = 10;
    private float enemyDistance;
    private int moveForce = 10;
    private GameObject attackThis;
    private bool didCollide = false;

    // Start is called before the first frame update
    void Start()
    {
        rgBD = this.GetComponent<Rigidbody2D>();
        attackThis = GameObject.Find("Player");
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        rotation = CheckRotation();
        SetRotation(rotation);
    }

    // Update is called once per frame
    void Update()
    {
        enemyDistance = Mathf.Abs(this.transform.position.x - attackThis.transform.position.x);
        //Debug.Log(enemyDistance);

        if (enemyDistance <= attackDistance && !didCollide)
        {
            Attack();
        }
        else if (enemyDistance > attackDistance && !didCollide)
        {
            MoveTowards();
            rotation = CheckRotation();
            SetRotation(rotation);
        }else if (enemyDistance <= attackDistance && didCollide)
        {
            FlyUp();
            if (this.transform.position.y >= 30)
            {
                StartCoroutine(MoveAway());
            }
        }
        
    }

    IEnumerator MoveAway()
    {
        if (rotation == "left")
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            rgBD.velocity = new Vector2(-1 * maxSpeed, 1.0f);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            rgBD.velocity = new Vector2(1 * maxSpeed, 1.0f);
        }
        yield return new WaitForSeconds(2.0f);
        didCollide = false;
    }

    private void FlyUp()
    {
        spriteRenderer.sprite = CuteFairy;
        if (rotation == "right")
        {
            this.transform.rotation = Quaternion.Euler(0, 0, +70);
            rgBD.velocity = new Vector2(1 * attackSpeed / 2, +3.0f * attackSpeed / 2);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180, +70);
            rgBD.velocity = new Vector2(-1 * attackSpeed / 2, +3.0f * attackSpeed / 2);
        }
    }

    void Attack()
    {
        AudioManager.Instance.PlaySound(attackAudio);
        spriteRenderer.sprite = AttackFairy;
        if (rotation == "right")
        {
            this.transform.rotation = Quaternion.Euler(0, 0, -70);
            rgBD.velocity = new Vector2(1 * attackSpeed/2, -3.0f * attackSpeed/2);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180, -70);
            rgBD.velocity = new Vector2(-1 * attackSpeed / 2, -3.0f * attackSpeed / 2);
        }
    }

    private void MoveTowards()
    {
        if (rotation == "left")
        {
            rgBD.velocity = new Vector2(-1 * maxSpeed, 3.0f);
        }
        else
        {
            rgBD.velocity = new Vector2(1 * maxSpeed, 3.0f);
        }
        
    }

    private string CheckRotation()
    {
        string rot;

        if (attackThis.transform.position.x > this.transform.position.x)
        {
            rot = "right";
        }
        else
        {
            rot = "left";
        }

        return rot;
    }

    private void SetRotation(string rot)
    {
        if (rot == "right")
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rot == "left")
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }else
        {
            // w/e
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(data.damage);
            didCollide = true;
        }

        if (collision.gameObject.name == "Floor")
        {
            didCollide = true;
        }
    }
}
