﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Name
{
	[System.Serializable]
	public class TrainingSet
	{
		public double[] input;
		public double output;

		private string GetDebuggerDisplay()
		{
			return ToString();
		}

	}

	public class Perceptron_sc : MonoBehaviour
	{

		//erorr 
		[SerializeField]
		//public TrainingSet[] ts;
		List<TrainingSet> ts = new List<TrainingSet>();
		double[] weights = { 0, 0 };
		double bias = 0;
		double totalError = 0;

		public GameObject npc;

		public void SendInput(double i1, double i2, double o)
		{
			//react
			double result = CalcOutput(i1, i2);
			Debug.Log(result);
			if (result == 0) //duck for cover
			{
				npc.GetComponent<Animator>().SetTrigger("Crouch");
				npc.GetComponent<Rigidbody>().isKinematic = false;
			}
			else
			{
				npc.GetComponent<Rigidbody>().isKinematic = true;
			}

			//learn from it for next time
			TrainingSet s = new TrainingSet();
			s.input = new double[2] { i1, i2 };
			s.output = o;
			ts.Add(s);
			//Train();
		}
		// from pdf
		double DotProductBias(double[] v1, double[] v2)
		{
			if (v1 == null || v2 == null)
				return -1;

			if (v1.Length != v2.Length)
				return -1;

			double d = 0;
			for (int x = 0; x < v1.Length; x++)
			{
				d += v1[x] * v2[x];
			}

			d += bias;

			return d;
		}
		//from pdf
		double CalcOutput(int i)
		{
			double dp = DotProductBias(weights, ts[i].input);
			if (dp > 0) return (1);
			return (0);
			//return(ActivationFunction(DotProductBias(weights,ts[i].input)));
		}
		//from pdf
		double CalcOutput(double i1, double i2)
		{
			double[] inp = new double[] { i1, i2 };
			double dp = DotProductBias(weights, inp);
			if (dp > 0) return (1);
			return (0);
			//return(ActivationFunction(DotProductBias(weights,inp)));
		}
		//from pdf
		double ActivationFunction(double dp)
		{
			if (dp > 0) return (1);
			return (0);
		}
		//from pdf
		void InitialiseWeights()
		{
			for (int i = 0; i < weights.Length; i++)
			{
				weights[i] = Random.Range(-1.0f, 1.0f);
			}
			bias = Random.Range(-1.0f, 1.0f);
		}

		void UpdateWeights(int j)
		{
			double error = ts[j].output - CalcOutput(j);
			totalError += Mathf.Abs((float)error);
			for (int i = 0; i < weights.Length; i++)
			{
				weights[i] = weights[i] + error * ts[j].input[i];
			}
			bias += error;
		}
		//from pdf
		void Train(int epochs)
		{       //int t = 0; t < ts.Count; t++
			for (int e = 0; e < epochs; e++)
			{
				totalError = 0;
				for (int t = 0; t < ts.Count; t++)
				{
					UpdateWeights(t);
					Debug.Log("W1 : " + (weights[0]) + "W2 : " + (weights[1] + " B : " + bias));
				}
				//UpdateWeights(t);
				Debug.Log("TOTAL ERROR : " + totalError);
			}
		}

		//from pdf
		void Start()
		{
			//InitialiseWeights();
			Train(8);

			Debug.Log("Test 0 0: " + CalcOutput(0, 0));
			Debug.Log("Test 0 1: " + CalcOutput(0, 1));
			Debug.Log("Test 1 0: " + CalcOutput(1, 0));
			Debug.Log("Test 1 1: " + CalcOutput(1, 1));

		}
		void Update()
		{
			if (Input.GetKeyDown("space"))
			{
				InitialiseWeights();
				ts.Clear();
			}
		}
	}
}
