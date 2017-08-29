using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseSpawner : MonoBehaviour {

    private static int basesSpawned = 0;

    public GameObject baseHighlight;
    public PlayerBase baseToSpawn;

    private GameObject baseHighlightInstance;
    private Renderer baseHighlightRenderer;
    private float baseHighlightHeight;

    private bool isSpawning;
    private PlayerMoney playermoney;

    private void Start()
    {
        isSpawning = false;
        playermoney = GetComponent<PlayerMoney>();
    }

    void OnEnable()
    {
        PlayerInput.OnSpawnBaseInput += UseSpawnInput;
    }

    void OnDisable()
    {
        PlayerInput.OnSpawnBaseInput -= UseSpawnInput;
    }

    void UseSpawnInput()
    {
        if (playermoney == null)
        {
            Debug.Log("Returning: BaseSpawner doesn't have Player Money!");
            return;
        }
        
        // If you are not yet spawning a base, start searching for a place to spawn.
        if (!isSpawning && playermoney.Money >= baseToSpawn.BaseCost)
        {
            isSpawning = true;
            SpawnHighlight();
            StartCoroutine(SearchingForSpawnPoint());
        }
        // Else spawn a base where currently searching.
        else if (isSpawning)
        {
            StopCoroutine(SearchingForSpawnPoint());
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(baseHighlightInstance.transform.position, out navMeshHit, 1.0f, NavMesh.AllAreas))
            {
                PlayerBase playerBase = Instantiate(baseToSpawn, baseHighlightInstance.transform.position, Quaternion.identity);
                basesSpawned++;
                playermoney.SubtractMoney(baseToSpawn.BaseCost);
                Destroy(baseHighlightInstance);

                BaseManager.instance.PlayerBases.Add(playerBase);
                playerBase.gameObject.name = "Base " + basesSpawned;
                baseHighlightInstance = null;
                isSpawning = false;
            }
        }
    }

    void SpawnHighlight()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            baseHighlightInstance = Instantiate(baseHighlight, hit.point, Quaternion.identity);
        baseHighlightRenderer = baseHighlightInstance.GetComponent<Renderer>();
        baseHighlightHeight = baseHighlightRenderer.bounds.extents.y;
        StartCoroutine(SearchingForSpawnPoint());
    }

    IEnumerator SearchingForSpawnPoint()
    {
        while(isSpawning)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                baseHighlightInstance.transform.position = new Vector3(hit.point.x, hit.point.y + baseHighlightHeight, hit.point.z);
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(hit.point, out navMeshHit, 1.0f, NavMesh.AllAreas))
                {
                    baseHighlightRenderer.material.color = new Color(0, 0, 1, 0.5f);
                }
                else
                {
                    baseHighlightRenderer.material.color = new Color(1, 0, 0, 0.5f);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
