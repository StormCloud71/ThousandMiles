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
}
class Program {
    public static void Main() {
        const int CLIN = 10;
        string myLine="";
        string[] lines = new string[CLIN];
        Console.WriteLine("So, it begins...");
        Randomizer x= new Randomizer();
        Randomizer obtype= new Randomizer();
        x.AddWeight("Sword",2.0);
        x.AddWeight("Mace",10.0);
        x.AddWeight("Spear",3.0);
        x.AddWeight("Chakram",0.5);
        x.AddWeight("Kelewang",0.2);
        x.AddWeight("Bow",8.5);
        x.AddWeight("Rapier",0.6);
        string docPath=Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        for (int i=0;i<CLIN;i++) {
            lines[i]=x.PickAChoice();
        }
        File.AppendAllLines(Path.Combine(docPath,"Testlines.txt"),lines);
        try{
            StreamReader fr= new StreamReader(Path.Combine(docPath,"Objects.json"));
            myLine=fr.ReadToEnd();
        }
        catch(Exception e) {
            Console.WriteLine("Exception: " +e.Message);
        }
        finally {
            Console.WriteLine("...Oh, well..");
            if (myLine.Length>0) {
                obtype.LoadJsonWeights(myLine);
                for (int i=0; i<30; i++) {
                    Console.WriteLine(obtype.PickAChoice());
                }
                
            }
        }

    }
}
