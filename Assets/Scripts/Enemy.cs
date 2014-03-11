using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public Transform shooter;
    public Transform bulletPrefab;
    public float speed = 2f;
    Vector3 direct = -Vector3.forward;
    Vector3 orgPos;

	void Start () {
        StartCoroutine(StartBulletPattern());
        InvokeRepeating("ChangeDirect", 0f, 2f);
        orgPos = transform.position;
    }
	
	void Update () {
        Vector3 pos = transform.position + direct * speed * Time.deltaTime;
        //pos.x = Mathf.Clamp(pos.x, -4f, 4f);
        //pos.z = Mathf.Clamp(pos.z, -4f, 4f);
        pos = orgPos + Vector3.ClampMagnitude(pos - orgPos, 4f);
        transform.position = pos;
	}

    void ChangeDirect()
    {
        //direct = Vector3.forward * (1 - Random.Range(0, 2)%2 * 2) + Vector3.right * (1 - Random.Range(0, 2)%2 * 2);
        direct = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.forward;
    }

    IEnumerator StartBulletPattern()
    {
        StartCoroutine(Blast(shooter, bulletPrefab, 20, 1, 120, 0.1f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Blast(shooter, bulletPrefab, 20, 1, 120, 0.1f));
        yield return new WaitForSeconds(6f);
        StartCoroutine(Spiral(shooter, bulletPrefab, 20, 2, 0.1f, true));
        StartCoroutine(Burst(shooter, bulletPrefab, 20, 5, 1.0f));
        yield return new WaitForSeconds(6f);
        StartCoroutine(Burst(shooter, bulletPrefab, 20, 3, 1.0f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Burst(shooter, bulletPrefab, 20, 3, 1.0f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(Burst(shooter, bulletPrefab, 20, 3, 1.0f));
        StartCoroutine(Flower(shooter, bulletPrefab, 5, 6, 5, 0.1f));
        yield return new WaitForSeconds(8f);
        StartCoroutine(StartBulletPattern());
    }

    IEnumerator Blast( Transform shooter, Transform bulletTrans, int shotNum, int volly, float spread, float shotTime)
    {
	    float bulletRot = shooter.eulerAngles.y;	//The y-axis rotation in degrees.
	    if ( shotNum <= 1 )
	    {
		    // Just fire straight.
		    Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, bulletRot, 0)); 
	    }
	    else
	    {
		    while(volly > 0)
		    {
			    bulletRot = bulletRot - (spread/2);		//Offset the bullet rotation so it will start on one side of the z-axis and end on the other.
			    for( var i = 0; i < shotNum; i++ )
			    {
				    Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, bulletRot, 0)); // Spawn the bullet with our rotation.
				    bulletRot += spread/(shotNum-1);	//Increment the rotation for the next shot.
				    if(shotTime > 0)
				    {
					    yield return new WaitForSeconds( shotTime );	//Wait time between shots.
				    }
			    }
			    bulletRot = shooter.eulerAngles.y; // Reset the default angle.
			    volly--;
		    }
	    }
    }

    IEnumerator Spiral( Transform shooter, Transform bulletTrans, int shotNum, int volly, float shotTime, bool clockwise )
    {
	    float bulletRot = shooter.eulerAngles.y;	//The y-axis rotation in degrees.
	    while(volly > 0)
	    {
		    for( var i = 0; i < shotNum; i++)
		    {
			    Instantiate(bulletTrans, shooter.position, Quaternion.Euler(0, bulletRot, 0)); // Spawn the bullet with our rotation.
			    if(clockwise)
			    {
				    bulletRot += 360.0f/shotNum;		//Increment the rotation for the next shot.
			    }
			    else
			    {
				    bulletRot -= 360.0f/shotNum;		//Increment the rotation for the next shot.
			    }
			    if(shotTime > 0)
			    {
				    yield return new WaitForSeconds( shotTime );	//Wait time between shots.
			    }
		    }
		    volly--;	//Subtract from volly.
	    }
    }

    IEnumerator Burst( Transform shooter, Transform bulletTans, int shotNum, int volly, float vollyTime )
    {
        float bulletRot = 0.0f;	//The y-axis rotation in degrees.
	    while(volly > 0)
	    {
		    for( var i = 0; i < shotNum; i++)
		    {
			    Instantiate(bulletTans, shooter.position, Quaternion.Euler(0, bulletRot, 0));	//Spawn the bullet with our rotation.
			    bulletRot += 360.0f/shotNum;		//Increment the rotation for the next shot.
		    }
		    bulletRot = 0.0f;
		    volly--;
		    yield return new WaitForSeconds( vollyTime );
	    }
    }

    IEnumerator Flower( Transform shooter, Transform bulletTrans, float flowerTime, int directions, float rotTime, float waitTime )
    {
	    float bulletRot = 0.0f;
	    while( flowerTime > 0 )
	    {
		    for( var i = 0; i < directions; i++)
		    {
			    Instantiate( bulletTrans, shooter.position, Quaternion.Euler( 0, bulletRot, 0));		//Spawn the bullet with our rotation;
			    bulletRot += 360.0f/directions;
		    } 
		    bulletRot += rotTime; 
		    if( bulletRot > 360)
		    {
			    bulletRot -= 360;
		    }
		    else if( bulletRot < 0 )
		    {
			     bulletRot += 360;
		    }
		    flowerTime -= waitTime;
		    yield return new WaitForSeconds( waitTime );
	    }
    }
}
