using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dechecteria
{
    public class Creature : MonoBehaviour
    {

	    // Use this for initialization
	    void Start () {

	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }

        public void Move(float x, float y)
        {
            transform.position = new Vector3(x, transform.position.y, y);

        }
    }
}
