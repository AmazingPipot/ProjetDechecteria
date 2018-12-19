using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Introduction : MonoBehaviour {
    public Text affText;
    // Use this for initialization

    public GameObject lumiere;
    public GameObject Creature;
    public Sprite lum;
    int index = 0;
    string Intro;
    float sautLigne = 2.0f;
    float sautCaractere = 0.04f;
    bool Acc = false;
    float TimeAttente1;
    float TimeAttente2;
    float T = 8.0f;
    int finClic = 1;
    float randPos;
    float TimeAttente3;
    float TimeAttenteClic;
    int clic = 0;

    float rx, ry, rz;

    bool Effet1 = false, Effet2 = false, Effet3 = false;

    public Camera m_camera;

    float camX, camY, camZ;

    public void OnMouseDown()
    {
        clic += 1;
        print(" CLIC " + clic);
        TimeAttenteClic = 1.0f;
        if (clic >= 2 && finClic == 1)
        {
            Acc = !Acc;
            finClic = 0;

            if (Acc == true)
            {
                sautLigne /= 10;
                sautCaractere /= 10;
            }
            else
            {
                sautLigne *= 10;
                sautCaractere *= 10;
            }
        }  
    }

    void lectureTexte()
    {
        if (index < Intro.Length)
        {
            if (TimeAttente2 < 0.0f)
            {
                //print("Taile texte " + Intro.Length);
                //print("Texte " + Intro.Substring(index, 1) + "Index " + index);

                if (Intro.Substring(index, 1) == "%")
                {
                    Effet2 = true;
                    TimeAttente3 -= Time.deltaTime;

                    if (TimeAttente3 <= 4)
                    {
                        randPos = 3 * TimeAttente3 / (T / 2);
                        Effet3 = true;
                    }
                    else
                    {
                        randPos = 3 * ((T - TimeAttente3) / 4);
                        
                    }
                    m_camera.transform.GetComponent<Transform>().position = new Vector3(camX + Random.Range(-randPos, randPos), camY + Random.Range(-randPos, randPos), camZ);

                    if (TimeAttente3 <= 0.0f)
                    {
                        m_camera.transform.GetComponent<Transform>().position = new Vector3(camX, camY, camZ);
                        affText.text = "";
                        TimeAttente1 = sautLigne;
                        TimeAttente2 = sautCaractere;
                        index++;
                    }

                }
                else if (Intro.Substring(index, 1) == "ù")
                {
                    Effet1 = true;

                    TimeAttente1 = sautLigne;
                    TimeAttente2 = sautCaractere;
                    index++;
                }
                else if (Intro.Substring(index, 1) == "#"/*"\n"*/)
                {
                    TimeAttente1 -= Time.deltaTime;
                    if (TimeAttente1 < 0.0f)
                    {
                        affText.text = "";
                        //affText.text += Intro.Substring(index, 1);
                        index++;
                        TimeAttente1 = sautLigne;
                        TimeAttente2 = sautCaractere;
                    }
                }
                else
                {
                    affText.text += Intro.Substring(index, 1);
                    index++;
                    TimeAttente2 = sautCaractere;
                }
            }
        }
        else
        {
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SceneDebut");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void gestionEffet()
    {
        if (Effet1)
        {
            lumiere.SetActive(true);

            lumiere.transform.localScale = new Vector3(lumiere.transform.localScale.x + 0.1f, lumiere.transform.localScale.y, lumiere.transform.localScale.z);

            if (lumiere.transform.localScale.x > 10.0f)
            {
                Effet1 = false;
            }
        }


        if (Effet2 && !Effet3)
        {
            if (Creature.transform.localScale.x < 4.0f)
            {
                Creature.transform.localScale = new Vector3(Creature.transform.localScale.x + 0.02f, Creature.transform.localScale.y + 0.02f, Creature.transform.localScale.z );
            }
        }

        if (Effet3)
        {
            lumiere.transform.localScale = new Vector3(lumiere.transform.localScale.x - 0.2f, lumiere.transform.localScale.y, lumiere.transform.localScale.z - 1.5f);

            if (lumiere.transform.localScale.x < 4.0f)
            {
                lumiere.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        Intro = "Il y a longtemps, je chérissais les Enfants de l'Homme comme les miens. " +
            "Hélas, cet amour était à sens unique.\n#" +
            "Au fur et à mesure que les Enfants de l'Homme prirent conscience de leur environnement, " +
            "ils ne pensèrent qu'à l'exploiter sans relâche.\n#" +
            "Exploitant ces ressources jusqu'à l'épuisement, " +
            "stérilisant le monde autour d'eux.\n#" +
            "A ce rythme, ils causeront leur propre destruction en emportant toute faune et flore avec eux.\n#" +
            "Je ne l'accepterais point.\n #" +
            "En tant que mère nourricière, " +
            "je ne puis rester sans agir et c'est à moi qu'incombe la tâche de punir ces enfants.\n#" +
            "O toi petiteù chose bien inconsciente du malheur qui se prépare...\n" +
            "Je te charge d'une mission.\n#" +
            "Prend un peu de mon pouvoir, prend un peu de ma tristesse et prend beaucoup de ma colère !\n# %" +
            "Les Hommes vont disparaître !\n#" +
            "A toi petite chose je te confie cette tâche.\n#" +
            "Mais sache qu'il te reste peu de temps, bientôt le monde s'effondrera du fait de l'Homme.\n#"+
            "Détruit leur cité, détruit leurs enfants !\n#" +
            "Purifie ma Terre de leurs méfaits ! Utilise leurs créations pour les détruire !\n#" +
            "TOUS ! \n\n\n";
    }

    void Start () {
        Vector3 v = m_camera.GetComponent<Transform>().position;
        camX = v.x;
        camY = v.y;
        camZ = v.z;

        TimeAttenteClic = 0.0f;
        TimeAttente1 = sautLigne;
        TimeAttente2 = sautCaractere;
        TimeAttente3 = T; 

        affText.text = "";

    }
	
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ChangeScene());
        }

        if (TimeAttenteClic > 0)
        {
            TimeAttenteClic -= Time.deltaTime;
        }
        else if (TimeAttenteClic <= 0.0f)
        {
            TimeAttenteClic = 0.0f;
            clic = 0;
            finClic = 1;
        }

        lectureTexte();
        gestionEffet();
        TimeAttente2 -= Time.deltaTime;
        //Intro = Intro.Remove(0);
	}
}
