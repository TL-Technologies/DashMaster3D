using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class BlockInfo
{
    public int blockAmount;
    public int blockDifficulty;
}
public class RandomMapController : MonoBehaviourSingleton<RandomMapController>
{
    public List<Block> ArrayBlockBegin;
    public List<Block> ArrayBlockEasy;
    public List<Block> ArrayBlockHard;
    public List<Block> ArrayBlockSpecial;
    public List<Obstacle> ObjectVatCan;
    public List<Obstacle> ObjectParkour;
    public List<ListBlockLevel> listBlockLevels;
    public GameObject objectCoin;
    public int[] blockAmountPerRow;
    public float offsetToPlayerPos;
    public Vector2 distanceBetweenEachBlock = new Vector2(4, 10);
    public GameObject winBlock;

    public GameObject[] allBlocks;
    int appearCount = 0;
    public Transform player;

    private BlockInfo[] blocksInfo;
    private int amountBlockNormal;
    private int amountBlockHard;
    // Start is called before the first frame update
    void Start()
    {
        calculateBlockDifficulty(listBlockLevels[PlayerprefSave.IdMap()].levelOfDifficult, listBlockLevels[PlayerprefSave.IdMap()].totalBlock);
        generateRandomBlock();
    }
    void generateRandomBlock()
    {
        //int maxBlockHorizontal = int.Parse(maxBlockNgang.text);
        //int maxBlockVertical = int.Parse(maxBlockDoc.text);
        //int totalMaxBlock = int.Parse(tongBlock.text);
        int maxBlockHorizontal = listBlockLevels[PlayerprefSave.IdMap()].amountBlockHorizotal;
        int maxBlockVertical = listBlockLevels[PlayerprefSave.IdMap()].amountBlockVertical;
        int totalMaxBlock = listBlockLevels[PlayerprefSave.IdMap()].totalBlock;
        float posY = -2;
        GameObject objBeginBlock = getRandomBlock(true);
        objBeginBlock.transform.position = new Vector3(0, posY, offsetToPlayerPos);
        amountBlockNormal--;
        blockAmountPerRow = myRandomList(totalMaxBlock, maxBlockVertical, maxBlockHorizontal);
        for (int i = 1; i < maxBlockVertical; i++)
        {

            float posZ = distanceBetweenEachBlock.y * i + offsetToPlayerPos;
            if (blockAmountPerRow[i] % 2 == 0)
            {
                //int middlePos = 0;
                for (int j = 0; j < blockAmountPerRow[i] / 2; j++)
                {
                    GameObject leftBlockOJ = getRandomBlock(false);
                    GameObject rightBlockOJ = getRandomBlock(false);
                    int posX = (int)distanceBetweenEachBlock.x * -j - (int)distanceBetweenEachBlock.x / 2;

                    leftBlockOJ.transform.position = new Vector3(posX, posY, posZ);
                    rightBlockOJ.transform.position = new Vector3(-posX, posY, posZ);
                }
            }
            else
            {
                GameObject middleBlock = getRandomBlock(false);
                middleBlock.transform.position = new Vector3(0, posY, posZ);
                for (int j = 0; j < (blockAmountPerRow[i] - 1) / 2; j++)
                {
                    GameObject leftBlockOJ = getRandomBlock(false);
                    GameObject rightBlockOJ = getRandomBlock(false);
                    int posX = (int)distanceBetweenEachBlock.x * (j + 1);

                    leftBlockOJ.transform.position = new Vector3(posX, posY, posZ);
                    rightBlockOJ.transform.position = new Vector3(-posX, posY, posZ);
                }
            }
        }
        GameObject objWin = Instantiate(winBlock);
        objWin.transform.position = new Vector3(0, posY, distanceBetweenEachBlock.y * maxBlockVertical + offsetToPlayerPos);
        Debug.Log(amountBlockNormal);
        UIController.Instance.SetInfoTienTrinh(objWin.transform.position.z);
    }

    int[] myRandomList(int sum, int n, int maxBlockHorizontal)
    {
        //int maxBlockHorizontal = 4;

        int totalCounter = n;

        int[] rngArr = new int[n];
        for (int i = 0; i < n; i++)
        {
            rngArr[i] = 1;
        }

        while (totalCounter < sum)
        {
            int rng = Random.Range(1, n);
            if (rngArr[rng] < maxBlockHorizontal)
            {
                rngArr[rng]++;
                totalCounter++;
            }
        }

        //for (int j = 0; j < n; j++)
        //{
        //    Debug.Log(rngArr[j]);
        //}
        return rngArr;
    }
    GameObject getRandomBlock(bool isBegin)
    {
        if (isBegin)
        {
            if (PlayerprefSave.IdMap() == 0)
                return Instantiate(ArrayBlockBegin[0].objBlock);
            else
                return Instantiate(ArrayBlockBegin[Random.Range(1, ArrayBlockBegin.Count)].objBlock);
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                if (amountBlockNormal > 0)
                {
                    amountBlockNormal--;
                    return Instantiate(ArrayBlockEasy[Random.Range(0, ArrayBlockEasy.Count)].objBlock);
                }
                else
                {
                    amountBlockHard--;
                    return Instantiate(ArrayBlockHard[Random.Range(0, ArrayBlockHard.Count)].objBlock);
                }
            }
            else
            {
                if (PlayerprefSave.IdMap() > 2)
                {
                    if (amountBlockHard > 0)
                    {
                        amountBlockHard--;
                        if (Random.Range(0, 10) == 5)
                        {
                            return Instantiate(ArrayBlockSpecial[Random.Range(0, ArrayBlockSpecial.Count)].objBlock);
                        }
                        else
                        {
                            return Instantiate(ArrayBlockHard[Random.Range(0, ArrayBlockHard.Count)].objBlock);
                        }
                    }
                    else
                    {
                        amountBlockNormal--;
                        return Instantiate(ArrayBlockEasy[Random.Range(0, ArrayBlockEasy.Count)].objBlock);
                    }
                }
                else
                {
                    if (amountBlockHard > 0)
                    {
                        amountBlockHard--;
                        return Instantiate(ArrayBlockHard[Random.Range(0, ArrayBlockHard.Count)].objBlock);
                    }
                    else
                    {
                        amountBlockNormal--;
                        return Instantiate(ArrayBlockEasy[Random.Range(0, ArrayBlockEasy.Count)].objBlock);
                    }
                }
            }
        }
    }


    //private void Update()
    //{
    //    blocksAppear();
    //}

    #region Optimize
    void blocksAppear()
    {
        if (appearCount < allBlocks.Length)
        {
            if (allBlocks[appearCount].transform.position.z - player.position.z < 20)
            {
                allBlocks[appearCount].SetActive(true);
                appearCount++;
            }
        }
    }

    #endregion
    void calculateBlockDifficulty(float difficulty, int totalBlock)
    {
        if (difficulty < 2)
        {
            if (difficulty < 1) difficulty = 1;
            blocksInfo = new BlockInfo[2];
            for (int i = 0; i < blocksInfo.Length; i++)
            {
                blocksInfo[i] = new BlockInfo();
                blocksInfo[i].blockDifficulty = (int)difficulty + i;
            }
            blocksInfo[0].blockAmount = Mathf.RoundToInt(2 * totalBlock - difficulty * totalBlock);
            blocksInfo[1].blockAmount = totalBlock - blocksInfo[0].blockAmount;
            amountBlockNormal = blocksInfo[0].blockAmount;
            amountBlockHard = blocksInfo[1].blockAmount;
            Debug.Log("số block dễ: "+blocksInfo[0].blockAmount);
            Debug.Log("số block khó: "+blocksInfo[1].blockAmount);
        }
        else
        {
            blocksInfo = new BlockInfo[3];
            for (int i = 0; i < blocksInfo.Length; i++)
            {
                blocksInfo[i] = new BlockInfo();
                blocksInfo[i].blockDifficulty = (int)difficulty - 1 + i;
                int k = Mathf.RoundToInt(difficulty * 10 - (int)difficulty * 10);

                switch (i)
                {
                    case 0:
                        if (k == 5)
                            blocksInfo[i].blockAmount = Mathf.RoundToInt(totalBlock * (2 * difficulty - 7) / 3 * -1);
                        else if (k > 5)
                            blocksInfo[i].blockAmount = 0;
                        else
                            blocksInfo[i].blockAmount = 2;
                        break;
                    case 1:
                        if (k == 5)
                            blocksInfo[i].blockAmount = Mathf.RoundToInt(totalBlock * (difficulty - 2) / 3);
                        else if (k > 5)
                            blocksInfo[i].blockAmount = Mathf.RoundToInt(totalBlock / 3);
                        else
                            blocksInfo[i].blockAmount = Mathf.RoundToInt(4 * totalBlock - totalBlock * difficulty - 4);
                        break;
                    case 2:
                        blocksInfo[i].blockAmount = totalBlock - blocksInfo[0].blockAmount - blocksInfo[1].blockAmount;
                        break;
                }


                Debug.Log(blocksInfo[i].blockAmount);
            }
        }



    }
}

public class blockSorter : IComparer
{

    // Calls CaseInsensitiveComparer.Compare on the transform Z position.
    int IComparer.Compare(System.Object x, System.Object y)
    {
        return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).transform.localPosition.z, ((GameObject)y).transform.localPosition.z));
    }
}
[System.Serializable]
public class Block
{
    public int levelAppear;
    public GameObject objBlock;
}
[System.Serializable]
public class Obstacle
{
    public float obsWidth;
    public GameObject objObstacle;
}
[System.Serializable]
public class ListBlockLevel
{
    [Tooltip("chay tu 1 den 2")]
    public float levelOfDifficult;
    public int amountBlockVertical;
    public int amountBlockHorizotal;
    public int totalBlock;
}