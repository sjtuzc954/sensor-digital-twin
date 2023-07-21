using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


[System.Serializable]
public class WebPack_Vector3{
    public float x;
    public float y;
    public float z;
    public float score;
}

[System.Serializable]
public class WebPack_NodeInfo{
    public int node_num;
    public int camera_id;
    public long frame_id;
    public WebPack_Vector3[] nodes;
}

public class Human_Skeleton : MonoBehaviour
{
    private static string ip = "0.0.0.0";
    private static int port = 50002;
    private static Socket socket;
    private Thread threadReceive;

    public GameObject[] nodes;
    public GameObject[] bones;

    private Vector2Int[] bnMap;
    private long last_frame_id;
    private float max_endurable_dist;
    public Vector3[] nodePos, lastNodePos;
    public GameObject nodePrefab;
    public GameObject bonePrefab;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new GameObject[25];
        bones = new GameObject[18];
        nodePos = new Vector3[25];
        lastNodePos = new Vector3[25];
        bnMap = new Vector2Int[18];

        last_frame_id = -1;
        max_endurable_dist = 0.5f;
        
        //GameObject.Find("xbot").SetActive(false);
        for (int i = 1; i < 25 ;i ++){
            if (i == 10 || i == 16 || i == 20 || i == 24) continue;
            nodes[i] = GameObject.Instantiate(nodePrefab, new Vector3(0, -10, 0), new Quaternion());
            if (i == 1) nodes[i].transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);
            nodes[i].name = "node-" + i;
        }
        nodes[0] = null;
        nodes[10] = null;
        nodes[16] = null;
        nodes[20] = null;
        nodes[24] = null;
        for (int i = 0; i < 18 ;i ++){
            bones[i] = GameObject.Instantiate(bonePrefab, new Vector3(0, -10, 0), new Quaternion());
            bones[i].name = "bone-" + i;
        }
        bnMap[0] = new Vector2Int(1, 2);
        bnMap[1] = new Vector2Int(2, 5);
        bnMap[2] = new Vector2Int(5, 3);
        bnMap[3] = new Vector2Int(3, 4);
        bnMap[4] = new Vector2Int(5, 6);
        bnMap[5] = new Vector2Int(6, 7);
        bnMap[6] = new Vector2Int(7, 8);
        bnMap[7] = new Vector2Int(8, 9);
        bnMap[8] = new Vector2Int(11, 12);
        bnMap[9] = new Vector2Int(12, 13);
        bnMap[10] = new Vector2Int(13, 14);
        bnMap[11] = new Vector2Int(14, 15);
        bnMap[12] = new Vector2Int(4, 17);
        bnMap[13] = new Vector2Int(17, 18);
        bnMap[14] = new Vector2Int(18, 19);
        bnMap[15] = new Vector2Int(4, 21);
        bnMap[16] = new Vector2Int(21, 22);
        bnMap[17] = new Vector2Int(22, 23);
        
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Any, port));
        threadReceive = new Thread(new ThreadStart(receiveFromClient));
        threadReceive.Start();

        Debug.Log("server initialized successfully!");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 25 ;i ++){
            if (nodes[i] == null) continue;
            nodes[i].transform.localPosition = nodePos[i];
        }
        for (int i = 0; i < 18 ;i ++){
            if(bones[i] == null) continue;
            Vector3 a = nodePos[bnMap[i].x], b = nodePos[bnMap[i].y];
            Vector3 forward = new Vector3(1, 1, 1);
            Vector3 upward = b - a;
            if (upward.x != 0){
                forward.x = -(upward.y + upward.z)/upward.x;
            }else if (upward.y != 0){
                forward.y = -(upward.x + upward.z)/upward.y;
            }else if (upward.z != 0){
                forward.z = -(upward.y + upward.x)/upward.z;
            }
            
            bones[i].transform.localPosition = (a + b) / 2;
            bones[i].transform.localRotation = Quaternion.LookRotation(forward, upward);
            bones[i].transform.localScale = new Vector3(2.5f, upward.magnitude / 2, 2.5f); 
        }

        //Camera.main.transform.localPosition = new Vector3(nodePos[3].x - 1.8f, nodePos[3].y + 1.2f, nodePos[3].z - 0.8f);
    }

    private void SetNodePos(WebPack_NodeInfo ni){
        if (ni.node_num != 25) return;

        if (last_frame_id > ni.frame_id) return;

        if (last_frame_id == ni.frame_id){
            // if (ni.camera_id == 1){
            //     for (int i = 0; i < 25 ;i ++){
            //         Vector3 nodepos = new Vector3(ni.nodes[i].x / 1000, ni.nodes[i].y / 1000, ni.nodes[i].z / 1000);
            //         if ((nodepos - lastNodePos[i]).magnitude <= max_endurable_dist){
            //             nodePos[i] = nodepos;
            //         }
            //     }
            // }else {
            //     for (int i = 0; i < 25 ;i ++){
            //         Vector3 nodepos = new Vector3(ni.nodes[i].x / 1000, ni.nodes[i].y / 1000, ni.nodes[i].z / 1000);
            //         if ((lastNodePos[i] - nodePos[i]).magnitude > max_endurable_dist){
            //             nodePos[i] = nodepos;
            //         }
            //     }
            // }

            for (int i = 0; i < 25 ;i ++){
                Vector3 nodepos = new Vector3(ni.nodes[i].x / 20, ni.nodes[i].y / 20, ni.nodes[i].z / 20);
                float dist = (nodePos[i] - nodepos).magnitude;
                if (dist <= max_endurable_dist){
                    nodePos[i] = (nodePos[i] + nodepos) / 2;
                }else {
                    if ((lastNodePos[i] - nodePos[i]).magnitude > (nodepos - lastNodePos[i]).magnitude){
                        nodePos[i] = nodepos; 
                    }
                }
            }
        }else{
            for (int i = 0; i < 25 ;i ++){
                lastNodePos[i] = nodePos[i];
                //if (ni.nodes[i].score == 0) continue;
                // nodePos[i].x = nodePos[i].x * (1 - ni.nodes[i].score) + (ni.nodes[i].x / 1000 * ni.nodes[i].score);
                // nodePos[i].y = nodePos[i].y * (1 - ni.nodes[i].score) + (ni.nodes[i].y / 1000 * ni.nodes[i].score);
                // nodePos[i].z = nodePos[i].z * (1 - ni.nodes[i].score) + (ni.nodes[i].z / 1000 * ni.nodes[i].score);
                nodePos[i].x = ni.nodes[i].x / 20;
                nodePos[i].y = ni.nodes[i].y / 20;
                nodePos[i].z = ni.nodes[i].z / 20;
            }
            last_frame_id = ni.frame_id;
        }
    }

    void receiveFromClient(){
        int recv;
        byte[] data;

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);
        while(true){
            data = new byte[10240];
            recv = socket.ReceiveFrom(data, ref Remote);
            //Debug.Log(recv.ToString() + " bytes received from " + Remote.ToString() + ":");
            string node_info_str = Encoding.UTF8.GetString(data, 0, recv);
            Debug.Log(node_info_str);

            WebPack_NodeInfo node_info = JsonUtility.FromJson<WebPack_NodeInfo>(node_info_str);
            //Debug.Log(node_info.node_num);
            //Debug.Log(node_info.nodes);
            SetNodePos(node_info);
        }
    }

    // 断开socket连接，并清除线程。在暂停、重置以及编辑器退出时调用
    void SocketQuit()
    {
        //再关闭线程
        if (threadReceive != null)
        {
            threadReceive.Interrupt();
            threadReceive.Abort();
        }
        //最后关闭服务器
        socket.Close();
    }

     void OnApplicationQuit()
    {
        SocketQuit();
    }
}
