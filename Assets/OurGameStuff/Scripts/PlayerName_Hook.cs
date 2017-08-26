using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerName_Hook : Prototype.NetworkLobby.LobbyHook {
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
        Prototype.NetworkLobby.LobbyPlayer lobby = lobbyPlayer.GetComponent<Prototype.NetworkLobby.LobbyPlayer>();
        PlayerAssignGet name = gamePlayer.GetComponent<PlayerAssignGet>();
        name.playerName = lobby.nameInput.text;
    }
	
}
