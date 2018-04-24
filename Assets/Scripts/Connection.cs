using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public string UrlInsertKalimat = "http://segarbugarindo.com/InsertKalimat.php";
    public string UrlInsertScore = "http://segarbugarindo.com/InsertScore.php";
    public string UrlGetKataDasar = "http://segarbugarindo.com/Items.php";
    public string UrlGetKategori = "http://segarbugarindo.com/itemsTheme.php";

    public void InsertKalimat(string game, string kalimat, string katadasar)
    {
        WWWForm form = new WWWForm();
        form.AddField("gamePost", game);
        form.AddField("katadasarPost", katadasar);
        form.AddField("kalimatPost", kalimat);
        WWW www = new WWW(UrlInsertKalimat, form);
    }


    public void InsertScore(string Game, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("GameNamePost", Game);
        form.AddField("ScorePost", score);
        WWW www = new WWW(UrlInsertScore, form);

    }

    public string getUrlKataDasar()
    {
        return this.UrlGetKataDasar;
    }

    public string getUrlKategori()
    {
        return this.UrlGetKategori;
    }





}
