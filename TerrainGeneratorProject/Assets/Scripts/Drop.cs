using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Drop
{

    private static DropData DropData;

    private Vector2Int _pos = new Vector2Int(0,0);
    private Vector2 _dir = new Vector2(0, 0);
    private float _vel = 0f;
    private float _water = 0f;
    private float _sediment = 0f;
    private float currentMaxSed;

    private float previousPointHeight = -333f;

    [SerializeField]
    private DropManager _owner;

    private float _alterAmount;

    private bool isSimulating = false;

    private float eroMultiplier = 0.1f;

    private float _rootTwo = Mathf.Sqrt(2f); //Cache this, as we use it a lot during Move().

    public Drop(int posX, int posY, float water, DropData data, DropManager owner)
    {
        _pos[0] = posX;
        _pos[1] = posY;
        _water = water;
        DropData = data;
        _owner = owner;
        _alterAmount = 0f;
        currentMaxSed = data.MaxSediment;
    }

    //Wrapper for an entire step for this drops simulation. Called by the DropManager above us.
    public void DropUpdate()
    {
        if (isSimulating)
        {
            for (int i = 0; i < DropData.StepsMax; i++)
            {
                //Erode
                Erode();

                //Move
                Move(_owner.GetOwner().GetMapWidth(), _owner.GetOwner().GetMapHeight());

                //Evap
                Evaporate();

                //Deposit
                Deposit();
            }
        }
    }

    private void Erode()
    {
        //Calculate how 'full' we are, as a percentage called C.
        //Then erode the current point by (C/2)% of the difference between this point and the last.
        //If there is no previous point, erode nothing.

        if (currentMaxSed > 0f)
        {
            float capacityUsed = currentMaxSed - _sediment;
            //Debug.Log("CapacityUsed: " + capacityUsed);

            if (capacityUsed > 0f)
            {
                float percentageUsed = capacityUsed / currentMaxSed;
                //Debug.Log("PercentageUsed: " + percentageUsed);

                if (!float.IsNaN(percentageUsed))
                {
                    float difference = previousPointHeight - _owner.GetOwner().SampleCurrentMap(_pos.x, _pos.y);
                    //Debug.Log("Difference: " + difference);

                    if (difference > -200f)
                    {
                        _alterAmount = difference * (1f / percentageUsed) * eroMultiplier;
                        //Debug.Log("AlterAmount: " + _alterAmount);
                        _owner.GetOwner().AlterHeightAtIndex(_pos.x, _pos.y, _alterAmount);
                        _sediment += _alterAmount;
                        _alterAmount = 0f;
                    }
                }
            }
        }
    }
    
    private void Move(int mapWidth, int mapHeight)
    {
        //Change the pos to the lowest of our 4 neighbours. Make sure erosion is done before this.
        //This is subject to change if a more robust eroder is implemented.
        
        // 0 LEFT
        // 1 RIGHT
        // 2 DOWN
        // 3 UP
        // 4 UP + RIGHT
        // 5 DOWN + RIGHT
        // 6 UP + LEFT
        // 7 DOWN + LEFT
        bool[] state = new bool[8];
        state[0] = _pos.x != 0;
        state[1] = _pos.x != mapWidth;
        state[2] = _pos.y != mapHeight;
        state[3] = _pos.y != 0;
        state[4] = state[1] && state[3];
        state[5] = state[1] && state[2];
        state[6] = state[0] && state[3];
        state[7] = state[0] && state[2];
        //state should be moved outside of the function and reset, IDE would not allow it.

        Vector2Int target = _pos;

        float currentValue = _owner.GetOwner().SampleCurrentMap(_pos.x, _pos.y);
        previousPointHeight = currentValue;

        for (int i = 0; i < _owner.neighboursStencil.Length - 1; i++)
        {
            if (state[i])
            {
                int sampleX = _owner.neighboursStencil[i].x + _pos.x;
                int sampleY = _owner.neighboursStencil[i].y + _pos.y;

                float sample = _owner.GetOwner().SampleCurrentMap(sampleX, sampleY);
                
                //If we are testing a diagonal, we need to correct for the extra spatial distance covered.
                //MAGIC NUMBER used. For speed, this code is very hot.
                if (i > 3) sample /= _rootTwo;

                if (sample < currentValue)
                {
                    target = new Vector2Int(sampleX, sampleY);
                    currentValue = sample;
                }
            }
        }
        
        _pos = target;
        
    }

    private void Evaporate()
    {
        //Reduce our current capacity by a small flat amount and a percentage of our current capacity.
        //eg, C *= 0.9; C-1;

        currentMaxSed *= 0.9f;
        currentMaxSed-= 1f;

        if (currentMaxSed < 0f)
        {
            currentMaxSed = 0f;
            isSimulating = false;
        }
    }

    private void Deposit()
    {
        //Raise our current point by "dropping" some of our sediment.
        //Drop any sediment that is currently over our newly lowered capacity.
        //eg, we are carrying 80 sediment, but our capacity has just reduced to 75, drop 5 sediment.
        
        if (currentMaxSed > _sediment) return;

        _alterAmount = Mathf.Abs(currentMaxSed - _sediment);

        if (_alterAmount < 0f)
        {
            Debug.LogError("Negative value given for deposition. Breaking");
            isSimulating = false;
            return;
        }
        
        //Debug.Log("Deposition at X" + _pos.x + " Y" + _pos.y + " for " + _alterAmount);
        
        _owner.GetOwner().AlterHeightAtIndex(_pos.x, _pos.y, _alterAmount * eroMultiplier);
        _sediment -= _alterAmount;
        _alterAmount = 0f;
    }
    
    
    public void Simulate()
    {
        isSimulating = true;
        DropUpdate();
    }
    
}


