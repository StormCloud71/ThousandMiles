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
