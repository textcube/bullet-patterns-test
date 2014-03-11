using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float speed = 10f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 3f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Time.deltaTime * speed;
	}
}
