﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewWallController : MonoBehaviour
{
    public Vector3 cubeScale;
    private GameObject[] wallArray;

    private List<int[]> WallList;

    private System.Random Generator = new System.Random(42);


    // Use this for initialization
    void Start()
    {
        wallArray = new GameObject[2916];

        Debug.Log("test newWallScript");
        WallList = initWalls(0,0,27,27);
        CreateWalls(WallList);
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    //List<List<int>>
    private List<int[]> initWalls(int Xa, int Ya, int Xb, int Yb)
    {
        List<int[]> CurrentWallList = new List<int[]>();

        if ((Xb-Xa <= 1) ||(Yb-Ya <= 1))
        {
            CurrentWallList.Add(new int[4] {0,0,0,0});
            return (CurrentWallList);
        }
        else
        { 
            int Xd = Generator.Next(Xa + 1, Xb);
            int Yd = Generator.Next(Ya + 1, Yb);
            int Rand = Generator.Next(0, 2);
            if (Rand == 0)
            {
                int R1 = Generator.Next(Xa+1, Xd);
                int R2 = Generator.Next(Xd+1, Xb);
                int R3 = Generator.Next(Ya+1, Yb);

                int[] HorizWall1 = new int[4] { Xa, Yd, R1 - 1, Yd };
                int[] HorizWall2 = new int[4] { R1, Yd, R2 - 1, Yd };
                int[] HorizWall3 = new int[4] { R2, Yd, Xb, Yd };

                int[] VertiWall1 = new int[4] { Xd, Ya, Xd, R3 - 1 };
                int[] VertiWall2 = new int[4] { Xd, R3, Xd, Yb };

                CurrentWallList.AddRange(initWalls(Xa, Ya, Xd, Yd));
                CurrentWallList.AddRange(initWalls(Xa, Yd, Xd, Yb));
                CurrentWallList.AddRange(initWalls(Xd, Ya, Xb, Yd));
                CurrentWallList.AddRange(initWalls(Xd, Yd, Xb, Yb));
                CurrentWallList.Add(HorizWall1);
                CurrentWallList.Add(HorizWall2);
                CurrentWallList.Add(HorizWall3);
                CurrentWallList.Add(VertiWall1);
                CurrentWallList.Add(VertiWall2);
                return (CurrentWallList);
            }

            else
            {
                int R1 = Generator.Next(Xa+1, Xb);
                int R2 = Generator.Next(Ya+1, Yd);
                int R3 = Generator.Next(Yd+1, Yb);

                int[] HorizWall1 = new int[4] { Xa, Yd, R1 - 1, Yd };
                int[] HorizWall2 = new int[4] { R1, Yd, Xb, Yd };

                int[] VertiWall1 = new int[4] { Xd, Ya, Xd, R2 - 1 };
                int[] VertiWall2 = new int[4] { Xd, R2, Xd, R3 - 1 };
                int[] VertiWall3 = new int[4] { Xd, R3, Xd, Yb };

                CurrentWallList.AddRange(initWalls(Xa, Ya, Xd, Yd));
                CurrentWallList.AddRange(initWalls(Xa, Yd, Xd, Yb));
                CurrentWallList.AddRange(initWalls(Xd, Ya, Xb, Yd));
                CurrentWallList.AddRange(initWalls(Xd, Yd, Xb, Yb));
                CurrentWallList.Add(HorizWall1);
                CurrentWallList.Add(HorizWall2);
                CurrentWallList.Add(VertiWall1);
                CurrentWallList.Add(VertiWall2);
                CurrentWallList.Add(VertiWall3);
                return (CurrentWallList);
            }
        }
    }


    public void CreateWalls(List<int[]> WallList)
    {
        int init = 0;
        foreach (int[] list in WallList)
        {
            if (!(list[0] == 0 && list[1] == 0 && list[2] == 0 && list[3] == 0) && !(list[0] == list[2] && list[1] == list[3]))
            {
                Debug.Log(list[0] + " " + list[1] + " " + list[2] + " " + list[3]);

                if(list[0] == list[2])                          //Si il n'y a pas de changements dans les coordonnées selon X
                {
                    int size = Math.Abs(list[3] - list[1]);     //On récupère la différence des coordonnées selon Y
                    cubeScale = new Vector3(1, 10, 10 * size);  //On met à jour la taille du mur à faire
                }
                else                                            //Si il n'y a pas de changements dans les coordonnées selon Y
                {
                    int size = Math.Abs(list[2] - list[0]);     //On récupère la différence des coordonnées selon X
                    cubeScale = new Vector3(10 * size, 10, 1);  //On met à jour la taille du mur à faire
                }


                wallArray[init] = GameObject.CreatePrimitive(PrimitiveType.Cube);

                wallArray[init].transform.position = new Vector3(130f - (list[0] + list[2])* 5, 5f, 134.5f - (list[1] + list[3]) * 5) / 10;
                wallArray[init].transform.localScale = cubeScale / 10;
                wallArray[init].transform.SetParent(gameObject.transform);

                //Surement à supprimer dans cette partie
                wallArray[init].AddComponent<Rigidbody>();
                Rigidbody rig = wallArray[init].GetComponent<Rigidbody>();
                rig.isKinematic = true;
                rig.useGravity = false;
                rig.freezeRotation = true;
                rig.constraints = RigidbodyConstraints.FreezeAll;
                init++;
            }
        }
    }
}

