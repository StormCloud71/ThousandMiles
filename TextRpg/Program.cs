using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text.Json.Nodes;
/// Road of a Thousand Miles
/// A small, random TextRPG
class Randomizer{
    /// <summary>
    /// A randomized, weighted class
    /// </summary>
    Dictionary<string,double> weightDictionary;
    double TotalWeight;
    public Randomizer() {
        weightDictionary=new Dictionary<string, double>();
        TotalWeight=0.0;
    }
    public void AddWeight(string chs, double weight) {
        if (!(weightDictionary.ContainsKey(chs))) {
            weightDictionary.Add(chs,weight);
            TotalWeight+=weight;
        } else {
            //This might be turned to just increased weight for a choice
            throw new ArgumentException ("Weighted Choice added again");
        }
    }
    public string PickAChoice() {
        Random rnd= new Random();
        double pos=rnd.NextDouble()*TotalWeight;
        double cs=0.0;
        string ret="";
        foreach(KeyValuePair<string,double> chs in weightDictionary)
        {
            ret=chs.Key;
            cs+=chs.Value;
            if (cs>pos) {
                break;
            }
        }
        return ret;

    }
    public void LoadJsonWeights(string ldWeights) {
        JsonNode objects= JsonNode.Parse(ldWeights);
        var obar=objects.AsArray();    
        foreach (JsonObject q in obar) {
            string obtyp=q["Name"].ToString();
            double obw;
            Double.TryParse(q["Weight"].ToString(), out obw);
            AddWeight(obtyp,obw);
        }
    }
    public void ReadAFile(string filename) {
        string myLine="";
        string docPath=Environment.CurrentDirectory.ToString();
        try {
            StreamReader fr= new StreamReader(Path.Combine(docPath,"config",filename));
            myLine=fr.ReadToEnd();    
        }
        catch(Exception e) {
            Console.WriteLine("Exception: "+e.Message);
        }
        finally {
            if (myLine.Length>1) {
                LoadJsonWeights(myLine);
            }
        }
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
        string SampleInventory=Objects.PickAChoice(); 
        switch(SampleInventory){
            case "Coins":
                int coinno= new Random().Next(100);
                CharacterWealth+=coinno;
                SampleInventory=$"bag of coins, containing {coinno} coins,";
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
                SampleInventory="pack of food";
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

    }
}