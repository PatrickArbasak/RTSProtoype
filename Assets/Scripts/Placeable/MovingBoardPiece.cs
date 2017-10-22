using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoardPiece : HealthBoardPiece, ISelectable
{
    private List<FloorTile> movementPath = new List<FloorTile>();
    private bool reachedGoal = true;
    private int currentPathIndex = 0;

    [SerializeField] private float movementSpeed = 5;
    private Vector3 startTile;
    private Vector3 endTile;
    private float startTime;
    private float journeyLength;

    public override void Selected()
    {
        base.Selected();
    }

    public override void UnSelected()
    {
        base.UnSelected();
    }

    public void GivePath(List<FloorTile> path)
    {
        movementPath.AddRange(path);
        SetUpNextMovement();
        reachedGoal = false;
    }

    void SetUpNextMovement()
    {
        startTile = transform.position;
        endTile = movementPath[currentPathIndex].NavPoint.position;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startTile, endTile);
    }

    protected override void Update()
    {
        base.Update();

        if (reachedGoal == false)
        {
            // Calculate how far in the journey the piece is at.
            float distCovered = (Time.time - startTime) * movementSpeed;
            float fracJourney = distCovered / journeyLength;
            Vector3 dir = endTile - startTile;
            dir.y = 0;
            gameObject.transform.forward = dir;
            transform.position = Vector3.Lerp(startTile, endTile, fracJourney);

            if (fracJourney >= 1f && currentPathIndex <= movementPath.Count - 1)
            {
                if (currentPathIndex == movementPath.Count - 1)
                {
                    // Finish movement.
                    OccupiedFloorTile = movementPath[currentPathIndex];
                    movementPath[currentPathIndex].IsOccupied = true;
                    currentPathIndex = 0;
                    movementPath.Clear();
                    reachedGoal = true;
                }
                else
                {
                    // Continue movement.
                    currentPathIndex++;
                    SetUpNextMovement();
                }
            }
            
        }
    }
}
