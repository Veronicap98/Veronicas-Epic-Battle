using Godot;
using System;

public class second : Panel
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public Label label;
	public static string poop;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = (Label)GetNode("Label");
		label.Text = poop;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }


	private void _on_Button_button_down()
	{
		GetTree().ChangeScene("res://main.tscn");
	}
}
