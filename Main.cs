using Godot;
using System;

public partial class Main : Node
{
	 // Don't forget to rebuild the project so the editor knows about the new export variable.

	[Export]
	public PackedScene MobScene { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Control>("UserInterface/Retry").Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		// Accept the "ui_accept" action (Enter key or gamepad A button) to restart the game when the retry button is visible.
		if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UserInterface/Retry").Visible)
		{
			// This restarts the current scene.
			GetTree().ReloadCurrentScene();
		}
	}
	
	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnMobTimerTimeout()
	{
		// Create a new instance of the Mob scene.
		Mob mob = MobScene.Instantiate<Mob>();

		// Choose a random location on the SpawnPath.
		// We store the reference to the SpawnLocation node.
		var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		// And give it a random offset.
		mobSpawnLocation.ProgressRatio = GD.Randf();

		Vector3 playerPosition = GetNode<Player>("Player").Position;
		mob.Initialize(mobSpawnLocation.Position, playerPosition);

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
		
		// We connect the mob to the score label to update the score upon squashing one.
		mob.Squashed += GetNode<ScoreLabel>("UserInterface/ScoreLabel").OnMobSquashed;
	}
	
	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnPlayerHit()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Control>("UserInterface/Retry").Show();
	}
}
