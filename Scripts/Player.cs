using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Speed = 0.5f;
    public LineRenderer LineRenderer;

    public GameObject ButtonPause;
    public GameObject Menu_Pause;
    public GameObject ButtonShield;
    public GameObject ButtonNext;
    public GameObject Particle;

    public AudioSource audioSource;
    public AudioClip audio_damage;
    public AudioClip audio_firework;
    public AudioClip audio_ui;
    public AudioClip audio_move;
    public AudioClip audio_shield;
    public GameObject Firework;

    public GameObject CameraAlpha;


    public GameObject End;

    Rigidbody Rb;
    public int Hint_step;
    Vector3 way;
    bool Pause = false;
    Vector3 BeginPosition;
    Vector3[] LineRendererCopy;
    bool Death = false;

    public float RechargeShield = 2f;
    float TimeDeath;
    float incShield;
    bool activeShield = false;
    bool retention = false;

    public Material MatNormal;
    public Material MatShield;

    float TimeStartWay = 2f;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        BeginPosition = transform.position;

    }

    void Update()
    {
        if (TimeStartWay > 0)
        {
            if (LineRenderer.positionCount <= 3) Reload();
            TimeStartWay -= Time.deltaTime;
            if (Hint_step != LineRenderer.positionCount - 1)
            {

                StartGame();
                Firework.SetActive(false);

            }
        }
        else if (Hint_step >= 0)
        {
            way = new Vector3(LineRenderer.GetPosition(Hint_step).x,
                        LineRenderer.GetPosition(Hint_step).y,
                        LineRenderer.GetPosition(Hint_step).z);

            if (Hint_step < LineRenderer.positionCount - 1)
            {
                LineRenderer.SetPosition(Hint_step + 1, transform.position);
            }

            if (Rb.transform.position == way)
            {
                if (Hint_step < LineRenderer.positionCount - 1)
                {
                    LineRenderer.positionCount = Hint_step + 1;
                }
                if (Hint_step == 0)
                {
                    Firework.SetActive(true);
                    ButtonNext.SetActive(true);
                    audioSource.PlayOneShot(audio_firework);
                }
                Hint_step--;
            }
            //audioSource.PlayOneShot(audio_move);
            Rb.MovePosition(Vector3.MoveTowards(transform.position, way, Time.deltaTime * Speed));
        }

        if (retention)
            if (incShield > 0)
            {
                incShield -= Time.deltaTime;
            }
            else
            {
                retention = false;
                gameObject.GetComponent<Renderer>().material = MatNormal;
                audioSource.PlayOneShot(audio_shield);
                activeShield = false;
            }


    }

    public void TouchPause()
    {
        audioSource.PlayOneShot(audio_ui);
        if (Pause)
        {
            ButtonPause.SetActive(true);
            Menu_Pause.SetActive(false);
            ButtonShield.SetActive(true);
            Time.timeScale = 1;
        }
        else
        {
            ButtonShield.SetActive(false);
            ButtonPause.SetActive(false);
            Menu_Pause.SetActive(true);
            Time.timeScale = 0;
        }
        Pause = !Pause;


    }

    public void DownShield()
    {
        activeShield = true;
        gameObject.GetComponent<Renderer>().material = MatShield;
        incShield = RechargeShield;
        retention = true;
    }

    public void UpShield()
    {
        activeShield = false;
        gameObject.GetComponent<Renderer>().material = MatNormal;
        incShield = 0;
        retention = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Danger" && !activeShield)
        {
            //CameraAlpha.gameObject.SetActive(true);
            //CameraAlpha.GetComponent<CameraAlpha>().Alpha();

            for (int i = 1; i < Random.Range(20f, 30f); i++)
            {
                GameObject Part = Instantiate(Particle, transform.position, Quaternion.identity);
                Destroy(Part, Random.Range(1f, 4f));
            }

            DeathPlayer();

        }

    }

    void StartGame()
    {
        Hint_step = LineRenderer.positionCount - 1;
        End.transform.position = new Vector3(LineRenderer.GetPosition(0).x,
                LineRenderer.GetPosition(0).y,
                LineRenderer.GetPosition(0).z);
        LineRendererCopy = new Vector3[LineRenderer.positionCount];
        LineRenderer.GetPositions(LineRendererCopy);
    }

    void Restart()
    {
        //CameraAlpha.gameObject.SetActive(true);
        // CameraAlpha.GetComponent<CameraAlpha>().Alpha();

        LineRenderer.positionCount = LineRendererCopy.Length;
        LineRenderer.SetPositions(LineRendererCopy);
        Rb.transform.position = BeginPosition;
        TimeStartWay = 3f;
        retention = false;
        activeShield = false;
        Pause = false;
    }

    void DeathPlayer()
    {
        TimeDeath = 3f;
        Death = true;
        audioSource.PlayOneShot(audio_damage);
        Restart(); 
    }

    public void Exit()
    {
        audioSource.PlayOneShot(audio_ui);
        StartCoroutine(Coroutine(audio_ui.length));
        
        Application.Quit();
    }

    public void Reload()
    {
        audioSource.PlayOneShot(audio_ui);
        StartCoroutine(Coroutine(audio_ui.length));

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Next()
    {
        audioSource.PlayOneShot(audio_ui);
        StartCoroutine(Coroutine(audio_ui.length));

        StaticCells.CellsSize++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Coroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        yield return null;
    }
}
