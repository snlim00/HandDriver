using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    [SerializeField] private HandleController handle;

    private float speed
    {
        get { return basicSpeed * speedMult; }
    }
    private float basicSpeed = 6;
    private float speedMult = 1;
    private float increaseSpeedPerSec = 0.02f;
    private Camera mainCam;

    bool inRoad = false;
    [SerializeField] private float scorePerSec = 50;

    private float maxHp = 100;
    private float curHp;
    private float damagePerSec = 33;
    private bool isFirstDamage = true;
    [SerializeField] private Color startColor;
    [SerializeField] private Color deadColor;

    private AudioSource audioSource;

    private bool isDead = false;

    private void Awake()
    {
        mainCam = Camera.main;

        audioSource = GetComponent<AudioSource>();

    }

    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;

        GameInfo.S.score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Handling();

        IncreaseSpeed();

        CheckRoad();
        RoadProcess();
    }

    private void Move()
    {
        if (isDead == true)
            return;

        transform.Translate(new Vector2(0, transform.eulerAngles.z).normalized * speed * Time.deltaTime);
        mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + 1, -10);
    }

    private void Handling()
    {
        if (handle.isHandling == false || isDead == true)
            return;

        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + (handle.rot * Time.deltaTime));
    }

    private void IncreaseSpeed()
    {
        basicSpeed += increaseSpeedPerSec * Time.deltaTime;
    }

    private void CheckRoad()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.up, 1);

        inRoad = false;
        for (int i = 0; i < hit.Length; ++i)
        {
            if(hit[i].transform.tag == TAG.ROAD)
            {
                inRoad = true;
            }
        }
    }

    private void RoadProcess()
    {
        if (handle.rot == 0 || isDead == true)
            return;


        if(inRoad == false)
        {
            GameInfo.S.score -= scorePerSec / 2 * Time.deltaTime;
            speedMult = 0.7f;
            Damage();
        }
        else
        {
            isFirstDamage = true;
            speedMult = 1f;
            GameInfo.S.score += scorePerSec * Time.deltaTime;
        }
    }

    private void Damage()
    {
        if (isDead == true)
            return;


        if (isFirstDamage == true)
        {
            isFirstDamage = false;
            audioSource.Play();
        }

        curHp -= damagePerSec * Time.deltaTime;

        mainCam.backgroundColor = Color.Lerp(startColor, deadColor, 1 - (curHp / maxHp));
        
        if(curHp <= 0)
        {
            if (isDead == false)
            {
                isDead = true;
                StartCoroutine(Dead());
            }
        }
    }

    private IEnumerator Dead()
    {
        scorePerSec = 0;
        speedMult = 0;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;

            mainCam.backgroundColor = Color.Lerp(deadColor, Color.black, t);

            yield return null;
        }

        GameInfo.S.GameOver();
    }
}
