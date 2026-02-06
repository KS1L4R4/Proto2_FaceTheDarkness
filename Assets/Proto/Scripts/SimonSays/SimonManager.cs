using UnityEngine;

public class SimonManager : MonoBehaviour
{
    public Material[] colours;

    private int colourSelect;
    public float stayLit;
    private float stayLitCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (stayLitCounter > 0)
        {
            stayLitCounter -= Time.deltaTime;
        }
        if (stayLitCounter <= 0)
        {
            colours[colourSelect].DisableKeyword("_EMISSION");
        }
    }

    public void StartSimonGame()
    {
        colourSelect = Random.Range(0, colours.Length-1);

        colours[colourSelect].EnableKeyword("_EMISSION");

        stayLitCounter = stayLit;
    }

    public void ColourPressed(int whichBtn)
    {
        if (colourSelect == whichBtn)
        {
            Debug.Log("Correccc");
        }
        else
        {
            Debug.Log("Incorrec");
        }
    }
}
