using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqScript : MonoBehaviour {

	public AudioSource source;

	Vector3 pos;

	private float minHeight = 0.25f;
	private float maxHeight = 5f;
	private float heightDiff;
	private float cubeHeight = 0.1f;

	public int spectrumSize = 64; // must be a power of two
	public int visualizerBandSize = 16; // current number of visualizer pylons
	public int spectrumPerBand;
	public float[] spectrum;
	public float[] visualizerSpectrum;
	public GameObject[] Cubes;

	// Use this for initialization
	void Start () {

		heightDiff = maxHeight - minHeight;

		if (source == null) {
			source = GetComponent<AudioSource> ();
		}

		spectrumPerBand = spectrumSize / visualizerBandSize; // number of spectrum to average per visualizer band

		spectrum = new float[spectrumSize]; // holds spectrum data from AudioSource
		visualizerSpectrum = new float[visualizerBandSize]; // holds spectrum data for visualizer
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < visualizerSpectrum.Length; i++) {
			visualizerSpectrum [i] = 0;
		}
		source.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

		int EQIndex = 0;
		int temp = 0;

		for (int i = 0; i < spectrum.Length - 1; i++) {
			// average the value in the visualizer band
			visualizerSpectrum [EQIndex] += spectrum [i] / spectrumPerBand;
			temp++;

			if (temp >= spectrumPerBand) {
				// move to next visualizer frequency band
				EQIndex++;
				temp = 0;
			}
		}

		for (int i = 0; i < Cubes.Length; i++) {
			float newPos = minHeight + ( Mathf.Clamp(visualizerSpectrum [i] * ((i+1) * (i+1)) * heightDiff, minHeight, maxHeight) );

			pos = Cubes [i].transform.position;
			Cubes[i].transform.position = new Vector3 (pos.x, newPos, pos.z);
		}
	}
}
