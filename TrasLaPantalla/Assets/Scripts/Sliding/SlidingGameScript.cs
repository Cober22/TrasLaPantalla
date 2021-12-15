using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlidingGameScript : MonoBehaviour
{
    [SerializeField] 
    private Transform emptySpace;

    [SerializeField]
    private float distanceTiles;

    [SerializeField]
    private TilesScript[] tiles;
    private Camera _camera;
    private int emptySpaceIndex = 8;
    private bool isFinished = false;

    [SerializeField]
    private GameObject endPanel;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        Shuffle();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit)
            {
                if(Vector2.Distance(emptySpace.position, hit.transform.position) < distanceTiles)
                {
                    Vector2 lastEmptyPosition = emptySpace.position;
                    TilesScript thisTile = hit.transform.GetComponent<TilesScript>();
                    emptySpace.position = thisTile.targetPosition;
                    thisTile.targetPosition = lastEmptyPosition;
                    int tileIndex = findIndex(thisTile);
                    tiles[emptySpaceIndex] = tiles[tileIndex];
                    tiles[tileIndex] = null;
                    emptySpaceIndex = tileIndex;
                }
            }
        }
        if (!isFinished)
        {
            int correctTiles = 0;
            foreach (var a in tiles)
            {
                if (a != null)
                {
                    if (a.inRightPlace)
                        correctTiles++;
                }
            }
            if (correctTiles == tiles.Length - 1)
            {
                isFinished = true;
                StartCoroutine(FinishGame());
            }
        }
        
    }
    IEnumerator FinishGame() {

        yield return new WaitForSeconds(4f);
        if (DontDestroy.lettergram)
            ManageAppScenes.lettergramScene.SetActive(true);
        else if (DontDestroy.maiwer)
            ManageAppScenes.maiwerScene.SetActive(true);
        else if (DontDestroy.whosapp)
            ManageAppScenes.whosappScene.SetActive(true);

        ChatBoxManager.sceneName = "";
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Shuffle()
    {
        int invertion;
        do
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] != null)
                {
                    var lastPos = tiles[i].targetPosition;
                    int randomIndex = Random.Range(0, 7);
                    tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                    tiles[randomIndex].targetPosition = lastPos;
                    var tile = tiles[i];
                    tiles[i] = tiles[randomIndex];
                    tiles[randomIndex] = tile;
                }
            }
            invertion = GetInversions();
            Debug.Log("Shuffle");
        } while (invertion % 2 != 0);

        
    }

    public int findIndex(TilesScript ts)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i] != null)
            {
                if(tiles[i] == ts)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }

}
