using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    public class GameOver : MonoBehaviour
    {
        public AudioClip music, explosion;

        public cameraShake cameraShake;

        public AudioSource gameOver;

        public Text uiText;

        float TimeBeforeWriting = 2.5f;

        private float showSpeed = 0.05f;

        private string showText, uiTextCopy;

        private bool coroutineProtect, loadText, fini = true;

        private void Start()
        {
            TextInformations();
            gameOver.clip = music;
            gameOver.Play();
        }

        private void OnEnable() { uiTextCopy = null; }

        private void Update()
        {
            TimeBeforeWriting -= Time.deltaTime;

            if(TimeBeforeWriting <= 0.0f)
            {
                if (loadText && !coroutineProtect)
                {
                    StartCoroutine(LoadLetters(uiTextCopy));
                    coroutineProtect = true;
                }

                else if (loadText && coroutineProtect) { uiText.text = showText; }

                else if (!loadText && !coroutineProtect)
                {
                    if (uiText.text != uiTextCopy) { TextInformations(); }
                }
            }

            

            if (!gameOver.isPlaying && fini == true)
            {
                StartCoroutine(cameraShake.Shake(2f, 10f));
                gameOver.clip = explosion;
                gameOver.Play();
                cameraShake.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                fini = false;
            }
        }

        private void TextInformations()
        {
            uiTextCopy = uiText.text;
            showText = null;
            uiText.text = null;

            loadText = true;
            coroutineProtect = false;
        }

        private IEnumerator LoadLetters(string completeText)
        {
            int textSize = 0;

            while (textSize < completeText.Length)
            {
                showText += completeText[textSize++];
                yield return new WaitForSeconds(showSpeed);
            }

            coroutineProtect = false;
            loadText = false;
        }
    }
}