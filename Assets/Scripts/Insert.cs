using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertAnagram  {

   static string InsertURL = "http://localhost/BrainGameDB/Insert.php";

    public static void Insert(string kalimat, string katadasar)
    {
        WWWForm form = new WWWForm();
        form.AddField("katadasarPost", katadasar );
        form.AddField("kalimatPost", kalimat);
        WWW www = new WWW(InsertURL, form );
    }

	
}

public class InsertFlexible
{
    static string InsertURL = "http://localhost/BrainGameDB/InsertFlexible.php";
    public static void Insert(string katadasar, string kalimat)
    {
        WWWForm form = new WWWForm();
        form.AddField("katadasarPost", katadasar);
        form.AddField("kalimatPost", kalimat);
        WWW www = new WWW(InsertURL, form);
    }

}

public class InsertScore
{
    static string InsertURL = "http://localhost/BrainGameDB/InsertScore.php";
    public static void Insert(string Game, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("GameNamePost", Game);
        form.AddField("ScorePost", score);
        WWW www = new WWW(InsertURL, form);

    }

}
