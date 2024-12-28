using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ObjectiveC;

class Actor {
    string Name {get; set;}
    int MaxHP {get; set;}
    int CurrentHP {get; set;}
    int Attackforce {get; set;}
    int Defenseforce {get; set;}
    //This should be inherited from another class probably
    int CoinsInWallet {get; set;}
    int FoodRations {get; set;}
    bool Alive {get; set;}
    // Just a string Dictionary for equipment now
    Dictionary <string,string> Equipment;
    //Constructor added
    public Actor(string nm, int hp, int atfo, int defo){
        Name=nm;
        MaxHP=hp;
        CurrentHP=hp;
        Attackforce=atfo;
        Defenseforce=defo;
        Alive=true;
        Equipment= new Dictionary<string,string>();
        Equipment.Add("Head","Empty");
        Equipment.Add("Body","Empty");
        Equipment.Add("Left Shoulder","Empty");
        Equipment.Add("Right Shoulder","Empty");
        Equipment.Add("Left Hand","Empty");
        Equipment.Add("Right Hand","Empty");
        Equipment.Add("Left Leg","Empty");
        Equipment.Add("Right Leg","Empty");
        Equipment.Add("Left Foot","Empty");
        Equipment.Add("Right Foot","Empty");
        CoinsInWallet=0;
        FoodRations=0;
    }
    public void GibCoins(int coins) {
        CoinsInWallet+=coins;
    }
    public void GibFood(int rations) {
        FoodRations+=rations;
    }
    public string Description() {
        string outp=$"Character name is {Name}.";
        if (CurrentHP==MaxHP) {
            outp+="They are in perfect health.";
        } else {
            outp+="They are injured. ";
        }
        outp+=$"In their wallet they have {CoinsInWallet} coins. ";
        outp+=$"In their bag they have {FoodRations} rations.";
        return outp;
            }
}
class Program {
    public static void Main() {
        //Initialization
        Randomizer Objects=new Randomizer();
        Objects.ReadAFile("Objects.json");
        int CharacterWealth=0;
        string CharacterName="Unknown";
        Console.Write ("Please insert your name: ");
        string answer=Console.ReadLine();
        if (answer.Length>1) {CharacterName=answer;}
        Console.WriteLine ($"Your journey begins now, {CharacterName}!");
        Actor myChara=new Actor(CharacterName,100,10,10);
        string SampleInventory=Objects.PickAChoice(); 
        switch(SampleInventory){
            case "Coins":
                int coinno= new Random().Next(100);
                CharacterWealth+=coinno;
                SampleInventory=$"bag of coins, containing {coinno} coins,";
                myChara.GibCoins(coinno);
                break;
            case "Armor":
                Randomizer arm=new Randomizer();
                arm.ReadAFile("Armors.json");
                SampleInventory=arm.PickAChoice();
                break;
            case "Weapon":
                Randomizer wpn=new Randomizer();
                wpn.ReadAFile("Weapons.json");
                SampleInventory=wpn.PickAChoice();
                break;
            case "Food":
                int rationno=new Random().Next(10);
                SampleInventory=$"pack of food with {rationno} rations";
                myChara.GibFood(rationno);
                break;
            case "Tool":
                Randomizer tl=new Randomizer();
                tl.ReadAFile("Tools.json");
                SampleInventory=tl.PickAChoice();
                break;
            default:
                SampleInventory="thing that's wrong";
                break;
        }
        Console.WriteLine ($"You can have a {SampleInventory} as a gift.");
        Console.WriteLine ("Now, go!");
        Console.WriteLine ($"By the way this is you: {myChara.Description()}");
    }
}