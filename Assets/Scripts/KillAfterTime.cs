using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfterTime : MonoBehaviour {

    [SerializeField]
    private int timeToDie = 5;

	void Start () {
        StartCoroutine(KillObject());
	}

    IEnumerator KillObject()
    {
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}
