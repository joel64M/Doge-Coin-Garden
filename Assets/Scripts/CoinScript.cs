using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] float rotSpeed=100f;
    [SerializeField] float sinSpeed = 10f;
    [SerializeField] float sinHeight = 1.2f;

    float yOffset = 0f;
    void Start()
    {
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),1, Mathf.RoundToInt(transform.position.z));
        yOffset = transform.position.y;
        GameManager.instance.InitializeCoin();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0, Space.World);
        transform.localPosition = new Vector3(transform.position.x, yOffset+Mathf.Sin(Time.time * sinSpeed) * sinHeight, transform.position.z) ;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.CoinCollected();
            transform.gameObject.SetActive(false);
        }
    }
}
