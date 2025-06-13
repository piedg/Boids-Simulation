using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidsManager : MonoBehaviour
{
   [SerializeField] private BoidSettings settings;
   [SerializeField] Slider cohesionSlider;
   [SerializeField] Slider separationSlider;
   [SerializeField] Slider alignmentSlider;
   [SerializeField] Slider currentBoidsSlider;
   [SerializeField] Text currentBoidsText;
   [SerializeField] Text fpsCounterText;
   [SerializeField] private List<GameObject> Boids = new List<GameObject>();
   
   [Range(0, 100)]
   [SerializeField] private int currentBoids;
   
   private float _deltaTime = 0.0f;

   private void Start()
   {
      cohesionSlider.value = settings.CohesionRadius;
      separationSlider.value = settings.SeparationRadius;
      alignmentSlider.value = settings.AlignmentRadius;
      currentBoidsSlider.value = currentBoids;
      
      cohesionSlider.onValueChanged.AddListener(OnCohesionChanged);
      separationSlider.onValueChanged.AddListener(OnSeparationChanged);
      alignmentSlider.onValueChanged.AddListener(OnAlignmentChanged);
      currentBoidsSlider.onValueChanged.AddListener(OnCurrentBoidsChanged);
   }

   private void Update()
   {
       for (int i = 0; i < Boids.Count; i++)
       {
           Boids[i].SetActive(i < currentBoids);
       }
       
       _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
       fpsCounterText.text = FPSCounter();
   }

   private void OnSeparationChanged(float newValue)
   {
       settings.SeparationRadius = (int)newValue;
   }
   
   private void OnCohesionChanged(float newValue)
   {
       settings.CohesionRadius = (int)newValue;
   }
   
   private void OnAlignmentChanged(float newValue)
   {
       settings.AlignmentRadius = (int)newValue;
   }
   
   private void OnCurrentBoidsChanged(float newValue)
   {
       currentBoids = (int)newValue;
       currentBoidsText.text = $"Current Boids: {currentBoids}";
   }

   private string FPSCounter()
   {
       float fps = 1.0f / _deltaTime;
       return $"FPS: {fps:0.}";
   }
}
