using Observer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    //public GameObject obs;
    //public GameObject coin;

    public float blockWidth = 1;
    public float blockHeight = 3.6f;
    private float obsWidth = 0.3f;
    float playerBaseSpeed = 1.7f;
    public float playerSpeed = 1.7f;
    //float distanceBetweenEachLane = 0.37f;
    bool[,] obsArr;
    int[] columnArr;

    float startingX;
    float startingZ;


    // Start is called before the first frame update
    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.Spawn, (param) => StartSpawn());
        //playerSpeed = 2.65f;
        //StartSpawn();
    }

    void StartSpawn()
    {
        playerSpeed = GameManager.instance.speed_Player + PlayerprefSave.speedUpgrade;
        blockHeight *= transform.localScale.z;
        blockWidth *= transform.localScale.x;
        startingZ = blockHeight / 2 * -1;
        startingX= obsWidth / 2 - blockWidth / 2;
        spawnObs();
    }

    void spawnObs()
    {
        //float startAt = blockHeight / 2 *-1;
        int howManyRow = (int)(blockHeight / playerSpeed);
        int howManyColumn = (int)(blockWidth / obsWidth);

        //random vi tri bat dau
        obsArr = new bool[howManyRow, howManyColumn];
        columnArr = new int[howManyColumn];
        for(int col = 0; col < howManyColumn; col++)
        {
            columnArr[col] = col;
        }
        Shuffle(columnArr);

        float lastZ = startingZ;
        float lastX;
        //columnArr[0] = 0;
        //columnArr[1] = 2;

        //Debug.Log(columnArr[0]);
        //Debug.Log(columnArr[1]);
        int firstRng = Random.Range(1, ((int)(howManyColumn / 2)) + 1);
        for(int f = 0; f < firstRng; f++)
        {
            if (howManyRow == 0) break;
            obsArr[0, columnArr[f]] = true;
        }
        //obsArr[0, columnArr[0]] = true;
        //obsArr[0, columnArr[1]] = true;

        for (int i = 1; i < howManyRow; i++)
        {
            int rng = Random.Range(1, ((int)(howManyColumn / 2)) + 1); //For amount of obs can generate

            //int[] availablePositions;
            List<int> availablePositions = new List<int>();
            for(int j = 0; j < howManyColumn; j++)
            {
                if (!obsArr[i - 1, j])
                {
                    availablePositions.Add(j);
                    //obsArr[i, j] = true;
                    //rng--;
                }

                //if (rng == 0) break;
            }
            Shuffle(availablePositions);
            //Debug.Log(rng);
            for(int k = 0; k < rng; k++)
            {
                int pos = availablePositions[k];
                obsArr[i, availablePositions[k]] = true;
                //availablePositions.RemoveAt(k);
                for(int l = k; l < availablePositions.Count; l++)
                {
                    if (Mathf.Abs(pos - availablePositions[l]) == 1) 
                    {
                        availablePositions.RemoveAt(l);
                        if (availablePositions.Count <= rng) 
                        rng--;
                        break;
                    }
                }
            }
        }
        //random vi tri ket thuc

        //Sinh vat can
        for(int z = 0; z < howManyRow; z++)
        {
            //lastX = startingX;
            if (z == 0) //vat can dau tien chi cach player 1.5m va scale theo speed player
            {
                lastZ += 1.5f * (playerSpeed / playerBaseSpeed);
                //Debug.Log(playerSpeed / playerBaseSpeed + "   " + (1.5f * (playerSpeed / playerBaseSpeed)));
            }
            else
                lastZ += playerSpeed;
            for (int c = 0; c < howManyColumn; c++)
            {
                lastX = startingX + obsWidth * c;
                if (obsArr[z, c])
                {
                    //lastX = startingX + obsWidth * c;
                    GameObject obsOJ = GetObject(true);
                    obsOJ.transform.SetParent(transform);
                    obsOJ.transform.position = new Vector3(lastX, 0, lastZ) + transform.position;
                }
                else
                {
                    //lastX = startingX + obsWidth * c;
                    GameObject coinOJ = Instantiate(RandomMapController.Instance.objectCoin);
                    coinOJ.transform.SetParent(transform);
                    coinOJ.transform.position = new Vector3(lastX, 0, lastZ) + transform.position;
                }
            }
        }
    }
    GameObject GetObject(bool isVatCan)
    {
        if (isVatCan)
        {
            return Instantiate(RandomMapController.Instance.ObjectVatCan[Random.Range(0, 3)].objObstacle);
        }
        else
        {
            return Instantiate(RandomMapController.Instance.ObjectParkour[Random.Range(0, 7)].objObstacle);
        }
    }
    public void Shuffle(int[] colList)
    {
        int tempInt;
        for (int i = 0; i < colList.Length - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, colList.Length);
            tempInt = colList[rnd];
            colList[rnd] = colList[i];
            colList[i] = tempInt;
        }
    }

    public void Shuffle(List<int> colList)
    {
        int tempInt;
        for (int i = 0; i < colList.Count - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, colList.Count);
            tempInt = colList[rnd];
            colList[rnd] = colList[i];
            colList[i] = tempInt;
        }
    }
}

public static class Extensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.ShuffleIterator();
    }

    private static IEnumerable<T> ShuffleIterator<T>(
        this IEnumerable<T> source)
    {
        var buffer = source.ToList();
        for (int i = 0; i < buffer.Count; i++)
        {
            int j = Random.Range(i, buffer.Count);
            yield return buffer[j];

            buffer[j] = buffer[i];
        }
    }
}