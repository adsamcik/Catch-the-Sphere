using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;


public class GameController:MonoBehaviour{
    static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

	/*Need to be set in editor*/
	public Score Score;
	public GameObject FinalResults;
	public GameObject SpawnInfo;
    public GameObject PauseMenu;

	/*Set automagically*/
	GameObject sphere;
	
	public float speed = 2;
	public int Active;
	public int destroyed;

    public bool paused;

    /*Spheres with abilities*/
    public List<GameObject> AbilitySpheres = new List<GameObject>();

	void Start(){
        ChangeSeed();
		StartCoroutine("Spawn");
		Instantiate(Resources.Load("Cube"));
	}

	void Update(){
		SpawnInfo.GetComponent<TextMesh>().text = destroyed + "/" + Active;
	}

    public void Pause()
    {
        paused = !paused;
        PauseMenu.SetActive(!PauseMenu.activeSelf);
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Sphere"))
        {
            Move script = item.GetComponent<Move>();
            script.Pause();
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Cube"))
        {
            box script = item.GetComponent<box>();
            script.Freeze();
        }
    }

	IEnumerator Spawn() {
        if(QualitySettings.GetQualityLevel() < 2) sphere = Resources.Load("SphereLow") as GameObject;
        else if (QualitySettings.GetQualityLevel() == 2) sphere = Resources.Load("SphereMed") as GameObject;
        else if (QualitySettings.GetQualityLevel() == 3) sphere = Resources.Load("SphereHigh") as GameObject;

		while (true)
		{
			yield return new WaitForSeconds(speed);
            if (!paused)
            {
                if (Active < 20 && (Active - destroyed) < 6)
                {
                    Active++;
                    Vector2 Circle = Random.insideUnitCircle*5;
                    if ((Active) % 7 == 0) Instantiate(Resources.Load(AbilitySpheres[Mathf.RoundToInt(Random.Range(0, AbilitySpheres.Count))].name), new Vector3(Circle.x,6,Circle.y), new Quaternion());
                    else Instantiate(sphere, new Vector3(Circle.x, 6, Circle.y), new Quaternion());
                }
            }
		}
	}

	IEnumerator RestartIn()
	{
		yield return new WaitForSeconds(2);
        while (Input.touchCount == 0 || Input.GetMouseButtonDown(0)) yield return new WaitForFixedUpdate();
		Restart();
	}

    public void Results()
    {
        Score.resultsactive = true;
        Score.Summary();
        StartCoroutine("RestartIn");
    }

	void Restart() {
        ChangeSeed();
		destroyed = 0; 
		Active = 0; 
		speed = 2; 
		FinalResults.GetComponent<TextMesh>().text = "";
        Score.NoScore();
	}

    public void AddScore(float scoresend) {
        Score.AddScore(scoresend);
    }

    public void AddScoreNoModifier(float scoresend)
    {
        Score.AddScoreNoModifier(scoresend);
    }

    void ChangeSeed()
    {
        byte[] data = new byte[4];
        rng.GetBytes(data);
        Random.seed = System.BitConverter.ToInt32(data, 0);
    }
}
