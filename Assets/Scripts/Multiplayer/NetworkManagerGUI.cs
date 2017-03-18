/* (C) Aurimas Blažulionis
 * NetworkManagerGUI component
 * Provides GUI using new UI system for NetworkManager
*/

using System;
using System.ComponentModel;
using UnityEngine.UI;
using System.Collections.Generic;

#if ENABLE_UNET

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerGUI")]
    [RequireComponent(typeof(NetworkManager))]

    public class NetworkManagerGUI : MonoBehaviour
    {
        public NetworkManager manager;
        [SerializeField] public bool showGUI = true;
        [SerializeField] public int offsetX;
        [SerializeField] public int offsetY;

		public static NetworkManagerGUI singleton;

		public GameObject startPanel;
		public GameObject mmPanel;
		public GameObject connectPanel;
		public GameObject connectedPanel;
		public GameObject connectedServerPanel;
		public GameObject readyPanel;
		public GameObject pickMatchPanel;
		public List<GameObject> matchObjects;
		public GameObject matchPrefab;
		public GameObject changeServerPanel;
		public UI.Text connectingClient;
		public UI.Text connectedClient;
		public UI.InputField matchNameField;
		public UI.InputField ipAddr;
		private bool profileChosen = false;
		private bool chosen;


        // Runtime variable
        bool m_ShowServer;

        void Awake()
        {
			singleton = this;
			matchObjects = new List<GameObject> ();
            manager = GetComponent<NetworkManager>();
        }

		public void HostButton () {
			if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
				manager.StartHost();
		}

		public void ClientButton () {
			manager.StartClient();
		}

		public void ServerButton () {
			if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
				manager.StartServer();
		}

		public void StopClient () {
			manager.StopClient();
		}

		public void StopHost () {
			manager.StopHost();
		}

		public void StartMachmaker () {
			if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
				manager.StartMatchMaker();
		}

		public void ClientReady () {
			ClientScene.Ready(manager.client.connection);

			if (ClientScene.localPlayers.Count == 0)
			{
				ClientScene.AddPlayer(0);
			}
		}

		public void CreateMatch () {
			manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
		}

		public void FindMatches () {

			while (matchObjects.Count > 0) {
				Destroy (matchObjects [0]);
				matchObjects.RemoveAt (0);
			}

			manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);

		}

		public void OnMatchesListed () {

			for (int i = 0; i < manager.matches.Count; i++) {
				matchObjects.Add ((GameObject)Instantiate (matchPrefab, pickMatchPanel.GetComponentInChildren<ScrollRect> ().content));
				matchObjects [i].GetComponent<RectTransform> ().anchoredPosition = new Vector2 (pickMatchPanel.GetComponentInChildren<ScrollRect> ().content.GetComponent<RectTransform> ().anchoredPosition.x, -(matchObjects [i].GetComponent<RectTransform> ().rect.height + 5f) * i);
				matchObjects [i].GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
				matchObjects [i].GetComponentInChildren<Text> ().text = manager.matches [i].name;
			}

			if (matchObjects.Count > 0)
				pickMatchPanel.GetComponentInChildren<ScrollRect> ().content.sizeDelta = new Vector2 (pickMatchPanel.GetComponentInChildren<ScrollRect> ().content.sizeDelta.x, (matchObjects[0].GetComponent<RectTransform> ().rect.height + 5f) * manager.matches.Count);
		}

		public void JoinMatch (int matchID) {
			manager.matchName = manager.matches[matchID].name;
			manager.matchMaker.JoinMatch(manager.matches[matchID].networkId, "", "", "", 0, 0, manager.OnMatchJoined);
		}

		public void JoinMatch (GameObject matchGO) {
			for (int i = 0; i < matchObjects.Count; i++) {
				if (matchGO == matchObjects[i]) {
					JoinMatch (i);
					break;
				}
			}
		}

		public void BackToMatchMenu () {
			manager.matches = null;
		}

		public void ChangeMMServer () {
			m_ShowServer = !m_ShowServer;
		}

		public void Local () {
			manager.SetMatchHost("localhost", 1337, false);
			m_ShowServer = false;
		}

		public void Internet () {
			manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
			m_ShowServer = false;
		}

		public void Staging () {
			manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
			m_ShowServer = false;
		}

		public void DisableMatchmaker () {
			manager.StopMatchMaker();
		}

        void Update()
        {
            if (!showGUI)
                return;

			if (!chosen) {
				startPanel.SetActive (false);
				connectPanel.SetActive (false);
				mmPanel.SetActive (false);
				connectedPanel.SetActive (false);
				readyPanel.SetActive (false);
				pickMatchPanel.SetActive (false);
				changeServerPanel.SetActive (false);
				connectedServerPanel.SetActive (false);
				return;
			}

            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            const int spacing = 24;

            bool noConnection = (manager.client == null || manager.client.connection == null ||
                                 manager.client.connection.connectionId == -1);

            if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
            {
                if (noConnection)
                {
                    
					startPanel.SetActive (true);
					connectPanel.SetActive (false);
					mmPanel.SetActive (false);
					connectedPanel.SetActive (false);
					readyPanel.SetActive (false);
					pickMatchPanel.SetActive (false);
					changeServerPanel.SetActive (false);
					connectedServerPanel.SetActive (false);
					manager.networkAddress = ipAddr.text;
                    
                }
                else
                {

					startPanel.SetActive (false);
					connectPanel.SetActive (true);
					mmPanel.SetActive (false);
					connectedPanel.SetActive (false);
					readyPanel.SetActive (false);
					pickMatchPanel.SetActive (false);
					changeServerPanel.SetActive (false);
					connectedServerPanel.SetActive (false);
					connectingClient.text = "Connecting to " + manager.networkAddress + ":" + manager.networkPort + "..";


                }
            }
            else
            {
                if (NetworkServer.active)
                {
					startPanel.SetActive (false);
					connectPanel.SetActive (false);
					mmPanel.SetActive (false);
					connectedPanel.SetActive (true);
					connectedServerPanel.SetActive (false);
					readyPanel.SetActive (false);
					pickMatchPanel.SetActive (false);
					changeServerPanel.SetActive (false);
                    string serverMsg = "Server: port=" + manager.networkPort;
                    if (manager.useWebSockets)
                    {
                        serverMsg += " (Using WebSockets)";
                    }
                    //GUI.Label(new Rect(xpos, ypos, 300, 20), serverMsg);
                }
                if (manager.IsClientConnected())
                {
                    connectedClient.text = "Client: address=" + manager.networkAddress + " port=" + manager.networkPort;
                }
            }

            if (manager.IsClientConnected() && !ClientScene.ready)
            {
				startPanel.SetActive (false);
				connectPanel.SetActive (false);
				mmPanel.SetActive (false);
				connectedPanel.SetActive (false);
				connectedServerPanel.SetActive (false);
				readyPanel.SetActive (true);
				pickMatchPanel.SetActive (false);
				changeServerPanel.SetActive (false);
            }

            if (NetworkServer.active || manager.IsClientConnected())
            {
				startPanel.SetActive (false);
				connectPanel.SetActive (false);
				mmPanel.SetActive (false);
				connectedServerPanel.SetActive (true);
				connectedPanel.SetActive (false);
				readyPanel.SetActive (false);
				pickMatchPanel.SetActive (false);
				changeServerPanel.SetActive (false);
            }

            if (!NetworkServer.active && !manager.IsClientConnected() && noConnection)
            {

                if (manager.matchMaker != null)
                {
                    if (manager.matchInfo == null)
                    {
                        if (manager.matches == null)
                        {
							startPanel.SetActive (false);
							connectPanel.SetActive (false);
							mmPanel.SetActive (true);
							connectedPanel.SetActive (false);
							readyPanel.SetActive (false);
							pickMatchPanel.SetActive (false);
							changeServerPanel.SetActive (false);
							connectedServerPanel.SetActive (false);

							manager.matchName = matchNameField.text;
                        }
                        else
                        {
							startPanel.SetActive (false);
							connectPanel.SetActive (false);
							mmPanel.SetActive (true);
							connectedPanel.SetActive (false);
							readyPanel.SetActive (false);
							pickMatchPanel.SetActive (true);
							changeServerPanel.SetActive (false);
							connectedServerPanel.SetActive (false);

                           /* for (int i = 0; i < manager.matches.Count; i++)
                            {
                                var match = manager.matches[i];
                                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
                                {
                                    manager.matchName = match.name;
                                    manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                                }
                                ypos += spacing;
                            }*/
                        }
                    }

                    if (m_ShowServer)
                    {

						startPanel.SetActive (false);
						connectPanel.SetActive (false);
						mmPanel.SetActive (true);
						connectedPanel.SetActive (false);
						readyPanel.SetActive (false);
						pickMatchPanel.SetActive (false);
						changeServerPanel.SetActive (true);
						connectedServerPanel.SetActive (false);
                        ypos += spacing;
                    }

                    //GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + manager.matchMaker.baseUri);
                }
            }
        }
    }
}
#endif //ENABLE_UNET
