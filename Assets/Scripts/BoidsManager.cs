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
   [SerializeField] private List<GameObject> Boids = new List<GameObject>();
   
   [Range(0, 100)]
   [SerializeField] private int currentBoids;
   private void Start()
   {
      cohesionSlider.value = settings.CohesionRadius;
      separationSlider.value = settings.SeparationRadius;
      alignmentSlider.value = settings.AlignmentRadius;
      
      cohesionSlider.onValueChanged.AddListener(OnCohesionChanged);
      separationSlider.onValueChanged.AddListener(OnSeparationChanged);
      alignmentSlider.onValueChanged.AddListener(OnAlignmentChanged);
   }

   private void Update()
   {
      
       for (int i = 0; i < Boids.Count; i++)
       {
           Boids[i].SetActive(i < currentBoids);
       }
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
}
