using Godot;
using System;
using System.Collections.Generic;

public class Network : Node
{
	[Export]
	private string host = "127.0.0.1";

	[Export]
	private int port = 5000;

	private Dictionary<int, Player> _players;
	
	public Dictionary<int, Player> Players
	{
		get { return _players; }
	}

    public override void _Ready()
    {
		GetTree().Connect("network_peer_connected", this, "NetworkPeerConnected");
		GetTree().Connect("connected_to_server", this, "ConnectedServer");
		
		NetworkedMultiplayerENet peer = new NetworkedMultiplayerENet();

		_players = new Dictionary<int, Player>();

		Error err = peer.CreateServer(port, 2);
		
		GD.Print(err);
		
		if (err != 0)
		{
			GD.Print("Cliente");
			Error rr = peer.CreateClient(host, port);
			GetTree().SetNetworkPeer(peer);
			return;
		}

		GetTree().SetNetworkPeer(peer);
		
		int unique = GetTree().GetNetworkUniqueId();
		string name = GetTree().GetNetworkUniqueId().ToString();

		SpawnPlayer(unique, name);
    }
	
	private void NetworkPeerConnected(int id)
	{
		GD.Print("Network connected");
		GD.Print(id);
	}
	
	private void ConnectedServer()
	{
		GD.Print("Cliente connected");
		RegisterInGame();
	}

	private Player SpawnPlayer(int id, string name)
	{
		var node = (Spatial) GetNode("Players");
		var playerModel = ResourceLoader.Load("res://player.tscn") as PackedScene;

		Player player = (Player) playerModel.Instance();

		if (player != null)
		{
			player.Id = id;
			player.Name = name;
			
			if (id == GetTree().GetNetworkUniqueId())
			{
				player.SetNetworkMaster(id);
				player.Master = true;
			}

			node.AddChild(player);
		}
		
		return player;
	}

	[Remote]
	public void RegisterInGame()
	{
		int unique = GetTree().GetNetworkUniqueId();
		string name = GetTree().GetNetworkUniqueId().ToString();

		Rpc("RegisterNewPlayer", unique, name);
	
		RegisterNewPlayer(unique, name);
	}

	[Remote]
	private void RegisterNewPlayer(int id, string name)
	{
		GD.Print("NEw Player");	
		GD.Print(id);

		if (GetTree().IsNetworkServer())
		{
			RpcId(id, "RegisterNewPlayer", GetTree().GetNetworkUniqueId(), GetTree().GetNetworkUniqueId().ToString());
			
			GD.Print("Teste new player");
			
			foreach (KeyValuePair<int, Player> entry in _players)
			{
				Player player = entry.Value;
				RpcId(id, "RegisterNewPlayer", player.Id, player.Name);
			}
		}

		GD.Print("Teste");
		
		Player cPlayer = SpawnPlayer(id, name);
		_players.Add(id, cPlayer);
	}
	
	[Remote]
	private void UnregisterPlayer(int id)
	{
		var player = (Player) GetNode(id.ToString());
		if (player != null)
		{
			player.QueueFree();
			_players.Remove(player.Id);
		}
	}
}
