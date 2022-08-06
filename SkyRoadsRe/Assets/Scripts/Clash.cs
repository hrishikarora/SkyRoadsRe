using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Clash : MonoBehaviour
{
    [SerializeField]ParticleSystem blast;
    [SerializeField]GameObject success;
    [SerializeField]GameObject lose;
    [SerializeField]GameObject canvas;
    [SerializeField]GameObject button;
    private void OnTriggerEnter(Collider other) {
        canvas.SetActive(true);
        if(other.gameObject.tag=="FinishLine")
        {
            success.SetActive(true);
            button.SetActive(true);
            this.gameObject.GetComponent<PlayerController>().enabled=false;

        }
        else
        {

        var temp= Instantiate(blast,transform.position,Quaternion.identity);
        
        temp.Play();
        Destroy(this.gameObject);
        lose.SetActive(true);
        button.SetActive(true);
        }
    }


    public void MainMenuu()
    {
        SceneManager.LoadScene(0);
    }
}
