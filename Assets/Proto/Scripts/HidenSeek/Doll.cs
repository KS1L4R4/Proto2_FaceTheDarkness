using System.Collections.Generic;
using UnityEngine;

public class Doll : MonoBehaviour
{
    int found = 0;
    public GameObject drop;
    public List<Transform> locationTargets;
    public Transform dropLocation;
    public Transform player;
    AudioSource laugh;
    public Animator animator;

    public void Start()
    {
        laugh = GetComponent<AudioSource>();

        int start = Random.Range(0, locationTargets.Count);
        transform.position = locationTargets[start].position;
        locationTargets.Remove(locationTargets[start]);
    }

    public void Update()
    {
        transform.LookAt(player);
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            found++;
            if (found == 3)
            {
                DropItem();
                MessageManager.Instance.ShowKeyAppearedMessage();
                gameObject.SetActive(false);
                return;
            }
            int next = Random.Range(0, locationTargets.Count -1);
            transform.position = locationTargets[next].position;
            locationTargets.Remove(locationTargets[next]);
        }

        
    }
    public void DropItem()
    {
        Instantiate(drop,dropLocation.transform.position,Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            laugh.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            laugh.Stop();
        }
    }
    /*public void Disapear()
    {
        DropItem();
                MessageManager.Instance.ShowKeyAppearedMessage();
        gameObject.SetActive(false);
    } */
}

//Sound Effect by <a href="https://pixabay.com/users/phatphrogstudio-54178146/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=477944">PhatPhrogStudio</a> from <a href="https://pixabay.com//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=477944">Pixabay</a>
//Sound Effect by PhatPhrogStudio from Pixabay
