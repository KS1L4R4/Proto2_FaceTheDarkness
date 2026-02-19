using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimonManager : MonoBehaviour
{
    public MeshRenderer[] colours;
    public AudioSource[] buttonSounds;

    private int colourSelect;
    public float stayLit;
    private float stayLitCounter;
    private bool emissionOff;

    public float waitBetweenLights;
    private float waitBetweenCounter;

    private bool shouldBeLit;
    private bool shouldBeDark;

    public List<int> activeSequence;
    private int positionInSequence;

    private bool simonGameActive;
    private int inputInSequence;

    public AudioSource correctAudio;
    public AudioSource incorrectAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldBeLit)
        {
            stayLitCounter -= Time.deltaTime;

            if (stayLitCounter < 0)
            {
                emissionOff = false;
          
                colours[activeSequence[positionInSequence]].material.DisableKeyword("_EMISSION");
                //buttonSounds[activeSequence[positionInSequence]].Stop();
                shouldBeLit = false;

                shouldBeDark = true;
                waitBetweenCounter = waitBetweenLights;

                positionInSequence++;
            }
        }

        if (shouldBeDark)
        {
            waitBetweenCounter -= Time.deltaTime;

            if(positionInSequence >= activeSequence.Count)
            {
                shouldBeDark = false;
                simonGameActive = true;
            }
            else
            {
                if (waitBetweenCounter < 0)
                {

                    colours[activeSequence[positionInSequence]].material.EnableKeyword("_EMISSION");
                    buttonSounds[activeSequence[positionInSequence]].Play();

                    stayLitCounter = stayLit;

                    shouldBeLit = true;
                    shouldBeDark = false;

                    emissionOff = true;
                }
            }
        }
    }

    public void StartSimonGame()
    {
        activeSequence.Clear();

        positionInSequence = 0;
        inputInSequence = 0;

        colourSelect = Random.Range(0, colours.Length-1);

        activeSequence.Add(colourSelect);

        colours[activeSequence[positionInSequence]].material.EnableKeyword("_EMISSION");
        buttonSounds[activeSequence[positionInSequence]].Play();

        stayLitCounter = stayLit;

        shouldBeLit = true;

        emissionOff = true;
    }

    public void ColourPressed(int whichBtn)
    {
        if(simonGameActive = true)
        {
            if (activeSequence[inputInSequence] == whichBtn)
            {
                Debug.Log("Correccc");

                inputInSequence++;

                if (inputInSequence >= activeSequence.Count)
                {
                    positionInSequence = 0;
                    inputInSequence = 0;

                    colourSelect = Random.Range(0, colours.Length - 1);

                    activeSequence.Add(colourSelect);

                    colours[activeSequence[positionInSequence]].material.EnableKeyword("_EMISSION");
                    buttonSounds[activeSequence[positionInSequence]].Play();

                    stayLitCounter = stayLit;

                    shouldBeLit = true;

                    simonGameActive = false;

                    correctAudio.Play();
                }
            }
            else
            {
                Debug.Log("Incorrec");
                incorrectAudio.Play();
                simonGameActive = false;
            }

        }
    }
}
