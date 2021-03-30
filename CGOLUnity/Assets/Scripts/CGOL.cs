using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CGOL : MonoBehaviour
{
    public List<GameObject> allGameObject = new List<GameObject>();
    public GameObject cubePrefab;
    public bool[,] currentGen;
    public int width;
    public int height;
    private System.Random rnd = new System.Random();
    public int GENERATION = 0;
    public bool IsDone = false;

    public void Init(int w, int h)
    {
        width = w;
        height = h;
        SetPlayfield();
        ClearPlayfield();
        InitPlayfield();
        
    }

    public void SetPlayfield()
    {
        currentGen = new bool[width, height];
        allGameObject.Clear();
        allGameObject = new List<GameObject>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                currentGen[x, y] = true;
                GameObject newCell = Instantiate(cubePrefab, new Vector3(x, 0, y), Quaternion.identity);
                newCell.GetComponent<StateMachine>().Init(x, y);
                allGameObject.Add(newCell);
            }
        }

    }

    public void ClearPlayfield()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                currentGen[x, y] = false;
                allGameObject.Where(item => item.GetComponent<StateMachine>().posx == x && item.GetComponent<StateMachine>().posy == y).FirstOrDefault().GetComponent<StateMachine>().SetState(false);
            }
        }
    }

    public void InitPlayfield()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int thresh = Random.Range(0, 100);
                if (thresh > 90)
                {
                    currentGen[x, y] = true;
                    allGameObject.Where(item => item.GetComponent<StateMachine>().posx == x && item.GetComponent<StateMachine>().posy == y).FirstOrDefault().GetComponent<StateMachine>().SetState(true);
                }
            }
        }
    }

    public void ComputeNextGeneration()
    {
        GENERATION++;
        
        
        bool[,] lastGen = currentGen;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int neighbours = GetNeighbours(x, y);

                if (lastGen[x, y] == false)
                {
                    if (neighbours == 3)
                    {
                        currentGen[x, y] = true;
                        allGameObject.Where(item => item.GetComponent<StateMachine>().posx == x && item.GetComponent<StateMachine>().posy == y).FirstOrDefault().GetComponent<StateMachine>().SetState(true);
                    }
                }

                if (lastGen[x, y] == true)
                {
                    if (neighbours < 2)
                    {
                        currentGen[x, y] = false;
                        allGameObject.Where(item => item.GetComponent<StateMachine>().posx == x && item.GetComponent<StateMachine>().posy == y).FirstOrDefault().GetComponent<StateMachine>().SetState(false);
                    }
                    if (neighbours == 2 || neighbours == 3)
                    {
                        currentGen[x, y] = true;
                        allGameObject.Where(item => item.GetComponent<StateMachine>().posx == x && item.GetComponent<StateMachine>().posy == y).FirstOrDefault().GetComponent<StateMachine>().SetState(true);
                    }
                    if (neighbours > 3)
                    {
                        currentGen[x, y] = false;
                        allGameObject.Where(item => item.GetComponent<StateMachine>().posx == x && item.GetComponent<StateMachine>().posy == y).FirstOrDefault().GetComponent<StateMachine>().SetState(false);
                    }
                }

            }
        }
    }

    private int GetNeighbours(int x, int y)
    {
        int n = 0;
        if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
        {
            return 0;
        }

        if (currentGen[x - 1, y - 1] == true)
        {
            n++;
        }

        if (currentGen[x, y - 1] == true)
        {
            n++;
        }

        if (currentGen[x + 1, y - 1] == true)
        {
            n++;
        }

        if (currentGen[x - 1, y] == true)
        {
            n++;
        }

        if (currentGen[x + 1, y] == true)
        {
            n++;
        }

        if (currentGen[x - 1, y + 1] == true)
        {
            n++;
        }

        if (currentGen[x, y + 1] == true)
        {
            n++;
        }

        if (currentGen[x + 1, y + 1] == true)
        {
            n++;
        }

        return n;
    }


    // Start is called before the first frame update
    void Start()
    {
        Init(45, 45);
        StartCoroutine(ComputeGeneration());
    }

    IEnumerator ComputeGeneration()
    {
        Debug.ClearDeveloperConsole();
        Debug.Log("Generation: " + GENERATION);
        ComputeNextGeneration();
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(ComputeGeneration());
    }


        // Update is called once per frame
    void Update()
    {
        
    }
}
