using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseSpawner : MonoBehaviour {

    private static int basesSpawned = 0;

    public GameObject baseHighlight;
    public PlayerBase baseToSpawn;

    private GameObject baseHighlightInstance;
    private Renderer baseHighlightRenderer;
    private float baseHighlightHeight;

    [SerializeField]
    private Text buttonText;

    private bool isSpawning;
    private PlayerMoney playermoney;

    TerrainPiece selectedTerrainPiece = null;

    private void Start()
    {
        isSpawning = false;

        buttonText.text = "Base Cost: " + baseToSpawn.BaseCost;
        playermoney = GetComponent<PlayerMoney>();
    }

    void OnEnable()
    {
        PlayerInput.OnMouseClick += MouseClickInput;
    }

    void OnDisable()
    {
        PlayerInput.OnMouseClick -= MouseClickInput;
    }

    void MouseClickInput()
    {
        // Spawn a base when currently searching.
        if (isSpawning && selectedTerrainPiece.isOccupied == false)
        {
            StopCoroutine(SearchingForSpawnPoint());
            PlayerBase playerBase = Instantiate(baseToSpawn, baseHighlightInstance.transform.position, Quaternion.identity);
            basesSpawned++;
            playermoney.SubtractMoney(baseToSpawn.BaseCost);
            Destroy(baseHighlightInstance);

            BaseManager.instance.PlayerBases.Add(playerBase);
            playerBase.gameObject.name = "Base " + basesSpawned;
            playerBase.OccupiedTerrainPiece = selectedTerrainPiece;
            playerBase.OccupiedTerrainPiece.isOccupied = true;
            baseHighlightInstance = null;
            isSpawning = false;
        }
    }

    public void StartSearching()
    {
        if (playermoney == null)
        {
            Debug.Log("Not enough Money!");
            return;
        }

        // If you are not yet spawning a base, start searching for a place to spawn.
        if (!isSpawning && playermoney.Money >= baseToSpawn.BaseCost)
        {
            isSpawning = true;
            StartCoroutine(SearchingForSpawnPoint());
        }
    }

    IEnumerator SearchingForSpawnPoint()
    {
        while (isSpawning)
        {
            RaycastHit hit;
            if (baseHighlightInstance == null)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    baseHighlightInstance = Instantiate(baseHighlight, hit.point, Quaternion.identity);
                    baseHighlightRenderer = baseHighlightInstance.GetComponent<Renderer>();
                    baseHighlightHeight = baseHighlightRenderer.bounds.extents.y;
                }
            }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && baseHighlightInstance != null)
            {
                if (selectedTerrainPiece = hit.collider.gameObject.GetComponent<TerrainPiece>())
                {
                    Vector3 hightlightPosition = new Vector3(selectedTerrainPiece.NavPoint.position.x, baseHighlightHeight, selectedTerrainPiece.NavPoint.position.z);
                    baseHighlightInstance.transform.position = hightlightPosition;
                    if (!selectedTerrainPiece.isOccupied)
                        baseHighlightRenderer.material.color = new Color(0, 0, 1, 0.5f);
                    else
                        baseHighlightRenderer.material.color = new Color(1, 0, 0, 0.5f);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
