﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    TODO:
    use a more sophisticated sorting algorithm for searching the inventory and adding items
    Sort the items in the Take Command
*/

namespace TextAdventure
{
    enum CommandType
    {
        Use,
        Take,
        Look,
        Move,
        Inventory,
        Help
    }
	class Game
	{
		Location currentLocation;

		public bool isRunning = true;

		private List<Item> inventory; // the player's inventory
        private List<string> splitterWords = new List<string> {"the"};

		public Game()
		{
			inventory = new List<Item>();

			Console.WriteLine("Welcome adventurer, prepare yourself for a fantastical journey into the unknown.");

            // build the "map"
            // add locations and items
            
			Location l1 = new Location("Entrance to hall", "You stand at the entrance of a long hallway. The hallways gets darker\nand darker, and you cannot see what lies beyond. To the east\nis an old oaken door, unlocked and beckoning.");
            Key rock = new Key("Rock", "It's a rock!", true);// smashes the window in l2y);
            l1.addItem(rock);

			Location l2 = new Location("End of hall", "You have reached the end of a long dark hallway. You can\nsee a window above a bookcase to your left.");

			Location l3 = new Location("Small study", "This is a small and cluttered study, containing a desk covered with\npapers. Though they no doubt are of some importance,\nyou cannot read their writing");

            Location l4 = new Location("Ledge", "A ledge high above the ground, leading round to the West wing.");

            // add exits
			l1.addExit(new Exit(Exit.Directions.North, l2));
			l1.addExit(new Exit(Exit.Directions.East, l3));

			l2.addExit(new Exit(Exit.Directions.South, l1));

			l3.addExit(new Exit(Exit.Directions.West, l1));
            

             currentLocation = l1;

            // Location C1L1 = new Location("");

			showLocation();
		}


        // TODO: Implement the input handling algorithm.
		public void doAction(string command)
		{
            #region Plan
            // what do i want the user to input
            /*
                single commands
                    look at the current area
                    A single direction
                    looking in inventory
                complex commands
                    above commands including verbs and filler words
                    sentences for take commands
                    sentences for using items together
            */
            // What words will i need to look for?
            /*
                verbs: use, take, look, move, walk, help, inspect
                single words: any exit name, inventory, look
                "connector words" for splitting item names when crafting/using items together: with, and
                removable words: at, my, in, the, from
            */
            // what do i want the algorithm to "output"
            /*
                Enum for a command type
                    command type will dictate which method to use
                Array of strings
                    the various items/exits that may need to be used as paramiters for methods
                Look at the room
                Look at an item (in room or inventory)
                Change Location
                Look at inventory
                Take an item
                use an item on another item (key on lock)
            */
            // actual plan
            /*
                First:
                    Count the number of words
                        if 1,
                            check if it's
                                an exit
                                inventory
                                help
                                look
                        if more,
                            Look for a verb

            */
            #endregion
            /*Dictionary<string, CommandType> VerbCommands = new Dictionary<string, CommandType>
            {
                { "use", CommandType.Use },
                { "take", CommandType.Take },
                { "look", CommandType.Look },
                { "move", CommandType.Move },
                { "walk", CommandType.Move }
            };*/
            List<string> commandList = splitCommands(command);
        }

        private List<string> splitCommands(string _command)
        {
            List<string> commandList = _command.Split(' ').ToList();
            foreach (string command in commandList)
            {
                foreach (string removable in splitterWords)
                {
                    if (command == removable)
                    {
                        commandList.Remove(command);
                    }
                }
            }
            return commandList;
        }

        #region CommandMethods

        private void useKey(Key _key, Exit _exit)
        {
            
        }
        
        private bool inspectInventory(string itemName, List<Item> searchInventory)
        {
            // inspect an inventory and write the description of the item if found, else return false
            // check if that item is in your inventory.
            foreach (Item currentItem in searchInventory)
            {
                if (currentItem.ToString().Equals(itemName))
                {
                    // write the item's description to the output stream
                    Console.WriteLine(currentItem.ItemDescription + "\n");
                    return true;
                }
            }
            return false;
        }

        private void moveToLocation(List<string> commandList)
        {
            // check if it contains one more word
            if (commandList.Count == 2)
            {
                // check the second word aginst all other exits
                foreach (Exit exit in currentLocation.getExits())
                {
                    bool longCommand = commandList[1].Equals(exit.ToString().ToLower());
                    bool shortCommand = commandList[1].Equals(exit.getShortDirection());
                    if (longCommand || shortCommand)
                    {
                        // go to that location
                        currentLocation = exit.getLeadsTo();
                        showLocation();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\n\"" + commandList[1] + "\" Is not a valid direction. \nare you confused?\n");
                    }
                }
            }
        }
        
        private void takeItem(string itemName)
        {
            foreach (Item item in currentLocation.getInventory())
            {
                if (item.ToString().Equals(itemName))
                {
                    inventory.Add(item);
                    currentLocation.removeItem(item);
                    Console.WriteLine("You took the {0}!", item.ItemName);
                }
            }
        }

        private void showInventory()
		{
			if ( inventory.Count > 0 )
			{
				Console.WriteLine("\nA quick look in your bag reveals the following:\n");

				foreach ( Item item in inventory )
				{
					Console.WriteLine(item.ItemName);
				}
			}
			else
			{
				Console.WriteLine("Your bag is empty.");
			}

			Console.WriteLine("");
		}

		public void showLocation()
		{
			Console.WriteLine("\n" + currentLocation.getTitle() + "\n");
			Console.WriteLine(currentLocation.getDescription());

			if (currentLocation.getInventory().Count > 0)
			{
				Console.WriteLine("\nThe room contains the following:\n");

				for ( int i = 0; i < currentLocation.getInventory().Count; i++ )
				{
					Console.WriteLine(currentLocation.getInventory()[i].ItemName);
				}
			}
	
			Console.WriteLine("\nAvailable Exits: \n");

			foreach (Exit exit in currentLocation.getExits() )
			{
				Console.WriteLine(exit.getDirection());
			}

			Console.WriteLine();
		}
        #endregion

		public void Update()
		{

			string currentCommand = Console.ReadLine().ToLower();

			// instantly check for a quit
			if (currentCommand == "quit" || currentCommand == "q")
			{
				isRunning = false;
				return;
			}
				
			// otherwise, process commands.
			doAction(currentCommand);
		}
	}
}
