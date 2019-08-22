using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DropData", menuName = "Drop Data", order = 51)]
public class DropData : ScriptableObject
{

        public float MaxVel;
        public float MaxWater;
        public float MaxSediment;
        public float Inertia;
        public float DepSpeed;
        public float EroSpeed;
        public float EvapSpeed;
        public float EroRadius;
        public float MinSlope;
        public int StepsMax;
        public float Gravity;

        public DropData(
            float maxVel, float maxWater, float maxSediment,
            float inertia, float depSpeed, float eroSpeed,
            float evapSpeed, float eroRadius, float minSlope,
            int stepsMax, float gravity)
        {
            MaxVel = maxVel;
            MaxWater = maxWater;
            MaxSediment = maxSediment;
            Inertia = inertia;
            DepSpeed = depSpeed;
            EroSpeed = eroSpeed;
            EvapSpeed = evapSpeed;
            EroRadius = eroRadius;
            MinSlope = minSlope;
            StepsMax = stepsMax;
            Gravity = gravity;
        }
}
