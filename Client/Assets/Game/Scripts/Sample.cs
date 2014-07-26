using UnityEngine;
using System.Collections;

public class Sample : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int a = Yuuki.Helper.Foo();
        Debug.Log("Hello World = " + a);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
