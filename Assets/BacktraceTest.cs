using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Backtrace.Unity;
using Backtrace.Unity.Model;
using System;

public class BacktraceTest : MonoBehaviour
{
	BacktraceClient backtraceClient;

	// Start is called before the first frame update
    void Start()
	{
		backtraceClient = GameObject.Find("Backtrace").GetComponent<BacktraceClient>();
	    try{
	    	throw new NullReferenceException("This is a test");
	    }
	    catch(Exception exception){
	    	var report = new BacktraceReport(exception);
		    backtraceClient.Send(report);   
	    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
