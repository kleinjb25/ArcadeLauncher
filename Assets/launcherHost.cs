using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Diagnostics;
using UnityEngine.Video;
public class launcherHost : MonoBehaviour
{
    public VideoPlayer player;
    string Directory = "C:\\ArcadeGamesConfig.txt";
    public string readFile;
    public InputField textInput;
    public List<String> titles;
    public List<String> creators;
    public List<String> descriptions;
    public List<String> images;
    public List<String> playerCounts;
    public List<String> directories;
    public Text titleText;
    public Text creatorText;
    public Text descriptionText;
    public Text playerCountText;
    public RawImage gameImage;
    public Text gameIndexText;
    int gameIndex;
    string[] lines;
    int gameCount;
    public GameObject topShowImage;
    public GameObject mgdcImage;
    float topImageTime;
    public GameObject libraryObject;
    float inputTime;
    public RawImage center;
    public RawImage left1;
    public RawImage left2;
    public RawImage right1;
    public Animator caroAnim;
    public RawImage right2;

    private void Start()
    {
        string path = "Assets/Resources/test.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(Directory);
        readFile = reader.ReadToEnd();
        reader.Close();
        lines = readFile.Split('\n');
        gameCount = (lines.Length / 14)+1;
        for (int i = 0; i<gameCount; i++)
        {
            
            int index = (i) * (14);
            UnityEngine.Debug.Log("INDEX IS " + index);
            titles.Add(lines[index + 2]);
            creators.Add(lines[index + 4]);
            descriptions.Add(lines[index + 6]);
            playerCounts.Add(lines[index + 8]);
            images.Add(lines[index + 10]);
            directories.Add(lines[index + 12]);
        }
        loadGame(0);
    }
    private void Update()
    {
        player.enabled = (!creatorText.text.Contains("Game Design Club"));
        topShowImage.SetActive(Time.realtimeSinceStartup < topImageTime);
        if (Time.realtimeSinceStartup > topImageTime)
        {
            if (!mgdcImage.activeInHierarchy)
            {
                if (Time.realtimeSinceStartup > inputTime)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.G))
                    {
                        inputTime = Time.realtimeSinceStartup + 1f;
                        gameIndex++;
                        caroAnim.Play("caroLeft");
                        loadGame(gameIndex);
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.D))
                    {
                        caroAnim.Play("caroRight");
                        inputTime = Time.realtimeSinceStartup + 1f;
                        gameIndex--;
                        loadGame(gameIndex);
                    }
                }
            }
            if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.K)))
            {
                if (mgdcImage.activeInHierarchy || !creatorText.text.Contains("Game Design Club"))
                {
                    mgdcImage.SetActive(false);
                    UnityEngine.Debug.Log("THE DIRECTORY IS a" + directories[gameIndex] + "asdfasdfasdfasdfasdfasdf");
                    topImageTime = Time.realtimeSinceStartup + 15;
                    runFile(directories[gameIndex]);
                }
                else
                    mgdcImage.SetActive(true);

            }
        }
    }
    public IEnumerator LoadImageFromIndex(int index, RawImage input)
    {
        if (Mathf.Clamp(index, 0, gameCount - 1) != index)
        {
            if (index > Mathf.Clamp(index, 0, gameCount - 1))
                index -= gameCount;
            else
                index += gameCount;
        }
        {
            input.enabled = true;
            string path = images[index];

            int exeIndex = path.LastIndexOf(".png");
            if (exeIndex != -1)
            {
                path = path.Substring(0, exeIndex + 4); // Adding 4 to include ".exe"
            }

            var www = UnityWebRequestTexture.GetTexture("file://" + path);
            yield return www.SendWebRequest();
            Texture2D tex = DownloadHandlerTexture.GetContent(www);
            input.texture = tex;
        }
    }
    public void loadGame(int index)
    {
        gameIndex = index;
        if (index != Mathf.Clamp(index, 0, gameCount - 1))
        {
            if (index > Mathf.Clamp(index, 0, gameCount - 1))
                index -= gameCount;
            else
                index += gameCount;
        }
        
        {
            libraryObject.gameObject.GetComponent<Animator>().Play("NextGame");
            gameIndex = index;
            gameIndexText.text = "GAME " + (index + 1) + "/" + gameCount;
            titleText.text = titles[index];
            creatorText.text = creators[index];
            descriptionText.text = descriptions[index];
            playerCountText.text = playerCounts[index];
            StartCoroutine(LoadImageFromIndex(index-2, left2));
            StartCoroutine(LoadImageFromIndex(index - 1, left1));
            StartCoroutine(LoadImageFromIndex(index, gameImage));
            StartCoroutine(LoadImageFromIndex(index, center));
            StartCoroutine(LoadImageFromIndex(index + 1, right1));
            StartCoroutine(LoadImageFromIndex(index + 2, right2));
        }
    }
    Process p;
    float lastRunTime;
    public void runFile(string inputDirectory)
    {
        if (p != null)
        {
            if (!p.HasExited)
                p.Kill();
        }
        int exeIndex = inputDirectory.LastIndexOf(".exe");
        if (exeIndex != -1)
        {
            inputDirectory = inputDirectory.Substring(0, exeIndex + 4); // Adding 4 to include ".exe"
        }

        p = new Process();
        p.StartInfo.UseShellExecute = true;
        p.StartInfo.FileName = inputDirectory;
        if (p.Start())
        {
            lastRunTime = Time.realtimeSinceStartup + 5;
        }
        
    }
}
