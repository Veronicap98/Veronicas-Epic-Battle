using Godot;
using System;

public class main : Panel
{
	public TextEdit outputwindow;
	public LineEdit inputwindow;
	public Button button;
	public Label herohplabel, monsterhplabel, xplabel;
	public ProgressBar herohp, monsterhp, xpbar;
	public string name, say, yesorno;
	public int x = 0, escape, score = 0, level = 1, heroStart = 20, monsterStart = 20, lvlXP = 10, monsterStr = 8;
	

	public override void _Ready()
	{
		outputwindow = (TextEdit)GetNode("outputwindow");
		inputwindow = (LineEdit)GetNode("inputwindow");
		button = (Button)GetNode("button");
		herohplabel = (Label)GetNode("herohplabel");
		monsterhplabel = (Label)GetNode("monsterhplabel");
		herohp = (ProgressBar)GetNode("herohp");
		monsterhp = (ProgressBar)GetNode("monsterhp");
		xplabel = (Label)GetNode("xplabel");
		xpbar = (ProgressBar)GetNode("xpbar");
		herohp.Visible = false; herohplabel.Visible = false; monsterhp.Visible = false; monsterhplabel.Visible = false; xpbar.Visible = false; xplabel.Visible = false;
		
		herohplabel.Text = name; monsterhplabel.Text = "Monster HP";

		
		intro();
		
		
	}

	public override void _Process(float delta)
	{
		outputwindow.Text = say;
		herohplabel.Text = $"{name} HP";
		xplabel.Text = $"{name} Exp. Points";
		outputwindow.ScrollVertical = outputwindow.GetLineCount() + 1;
	}



	private async void _on_button_button_down()
	{
		x += 1;
		switch (x)
		{
			case 1:
				getName();
				break;

			case 2:
				runPrompt();
				break;

			case 3: // This is where the program is directed after the user selects whether to run or fight in runPrompt()
				yesorno = inputwindow.GetText();
				yesorno = yesorno.ToLower();
				switch (yesorno)
				{
					case "f":
						inputwindow.Text = "";
						say = "Engaging in battle...";
						await ToSignal(GetTree().CreateTimer(2), "timeout");
						battle();
						break;
					case "r":
						inputwindow.Text = "";
						run();
						break;
					default:
						inputwindow.Text = "";
						say = "I don't know what that means. Try again.";
						await ToSignal(GetTree().CreateTimer(2), "timeout");
						x -= 1;
						runPrompt();
						break;
				}
				break;

			case 4: // This is where the program is directed after the battle when the user selects whether to play again or not
				yesorno = inputwindow.GetText();
				yesorno = yesorno.ToLower();
				switch (yesorno)
				{
					case "yes":
						inputwindow.Text = "";
						say += "Very well. Engaging in another battle...";
						await ToSignal(GetTree().CreateTimer(2), "timeout");
						battle();
						x -= 1;
						break;
					case "no":
						herohp.Visible = false; herohplabel.Visible = false; monsterhp.Visible = false; monsterhplabel.Visible = false; xpbar.Visible = false; xplabel.Visible = false;
						inputwindow.Text = ""; 
						say = $"Goodbye, {name}. Thanks for playing! I hope you enjoyed.\n\n-Veronicap";
						await ToSignal(GetTree().CreateTimer(5), "timeout");
						GetTree().Quit();
						break;
					default:
						inputwindow.Text = "";
						say += "I don't know what that means. Try again.";
						await ToSignal(GetTree().CreateTimer(2), "timeout");
						x -= 1;
						end();
						break;
				}
				break;
		}
	}

	private async void intro()
	{
		inputwindow.Editable = false; button.Disabled = true;
		say = "Welcome to Veronica's Epic Battle!\n";
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		say += "Please enter your name below, adventurer.";
		inputwindow.Editable = true; button.Disabled = false;
	}

	private async void getName()
	{
		name = inputwindow.GetText(); inputwindow.Text = "";
		inputwindow.Editable = false; button.Disabled = true;
		say = $"So you've chosen the name, {name}. Very well."; 
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		say += "\nPrepare to face enemies, the likes of which you've never seen!";
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		say += "\nPress the button below to continue, iF yOu dArE!!!";
		button.Disabled = false;
	}

	private async void runPrompt()
	{
		inputwindow.Text = ""; inputwindow.Editable = false; button.Disabled = true;
		say = $"What will you do, {name}? (r)un | (f)ight?";
		await ToSignal(GetTree().CreateTimer(1), "timeout");
		inputwindow.Editable = true; button.Disabled = false;
	}

	private async void run()
	{
		inputwindow.Text = ""; inputwindow.Editable = false; button.Disabled = true;
		Random random = new Random();
		escape = random.Next(1, 4);
		if (escape >= 3)
		{
			say = "Got away safely!";
			await ToSignal(GetTree().CreateTimer(2), "timeout");
			GetTree().Quit();
		}
		else
		{
			say = "Couldn't escape!";
			await ToSignal(GetTree().CreateTimer(2), "timeout");
			battle();
		}
	}

	private async void battle()
	{
		int hero = heroStart, monster = monsterStart, hit;
		Random random = new Random();
		say = ""; inputwindow.Text = ""; inputwindow.Editable = false; button.Disabled = true;
		herohp.Visible = true; herohplabel.Visible = true; monsterhp.Visible = true; monsterhplabel.Visible = true; xpbar.Visible = true; xplabel.Visible = true;
		herohp.Value = hero; monsterhp.Value = monster; xpbar.MaxValue = lvlXP;
		herohp.MaxValue = heroStart; monsterhp.MaxValue = 20;

		do 
		{
			// Hero's attack turn
			hit = random.Next(1, 10);
			monster -= hit;
			monsterhp.Value = monster;
			say += $"Monster is hit for {hit} damage. Current HP: {monster}\n";
			if (score <= lvlXP) score += (hit / 2);			
			xpbar.Value = score;
			await ToSignal(GetTree().CreateTimer(1), "timeout");
			if (monster <= 0)
			{
				say += $"{name} Wins! Score: {score} Level: {level}";
				end();
				break;
			}

			// Monster's attack turn
			hit = random.Next(1, monsterStr);
			hero -= hit;
			herohp.Value = hero;
			say += $"{name} is hit for {hit} damage. Current HP: {hero}\n";
			await ToSignal(GetTree().CreateTimer(1), "timeout");
			if (hero <= 0)
			{
				say += $"Monster Wins! Score: {score} Level {level}";
				score = 0; level = 1; heroStart = 20; monsterStart = 20; lvlXP = 10; monsterStr = 8;
				end();
				break;
			}
		} while ((hero > 0) && (monster > 0));
	}

	private async void end()
	{
		inputwindow.Text = ""; inputwindow.Editable = false; button.Disabled = true;
		if (score >= lvlXP)
		{
			await ToSignal(GetTree().CreateTimer(1), "timeout");
			level += 1;
			xpbar.MinValue = lvlXP;
			lvlXP += ((level * 2) + 10);
			heroStart += 2;
			herohp.MaxValue = heroStart;
			monsterStr += 2;
			say += $"\nLevel up! Level: {level} +2 max HP\n";
			await ToSignal(GetTree().CreateTimer(1), "timeout");
		}
		say += "\nPlay again?\n";
		inputwindow.Editable = true; button.Disabled = false;
	}
}
