using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public string UrlInsertKalimat = "http://segarbugarindo.com/InsertKalimat.php";
    public string UrlInsertScore = "http://segarbugarindo.com/InsertScore.php";
    public string UrlGetKataDasar = "http://segarbugarindo.com/Items.php";
    public string UrlGetKategori = "http://segarbugarindo.com/itemsTheme.php";


    public string UrlLogin = "http://segarbugarindo.com/Login.php";
    public string UrlRegist = "http://segarbugarindo.com/Regist.php";
    public string UrlInsertKataFlx1 = "http://segarbugarindo.com/insert_kata_flx1.php";
    public string UrlInsertKataFlx2 = "http://segarbugarindo.com/insert_kata_flx2.php";
    public string UrlInsertKataAna = "http://segarbugarindo.com/insert_kata_anagram.php";
    public string UrlInsertScoreFlx1 = "http://segarbugarindo.com/insert_score_flx1.php";
    public string UrlInsertScoreFlx2 = "http://segarbugarindo.com/insert_score_flx2.php";
    public string UrlInsertScoreAna = "http://segarbugarindo.com/insert_score_anagram.php";
    public string UrlInsertScore3Col = "http://segarbugarindo.com/insert_score_3col.php";


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

    public void InsertKataFlx1(string user, string kata, string kalimat)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user);
        form.AddField("katadasar", kata);
        form.AddField("kalimat", kalimat);
        WWW www = new WWW(UrlInsertKataFlx1, form);
    }

    public void InsertKataFlx2(string user, string kata, string kalimat)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user);
        form.AddField("katadasar", kata);
        form.AddField("kalimat", kalimat);
        WWW www = new WWW(UrlInsertKataFlx2, form);
    }

    public void InsertKataAna(string user, string kata, string kalimat)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user);
        form.AddField("katadasar", kata);
        form.AddField("kalimat", kalimat);
        WWW www = new WWW(UrlInsertKataAna, form);
    }

    public void InsertScoreFlx1(string user, int score, string akurasi)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user);
        form.AddField("score", score);
        form.AddField("akurasi", akurasi);
        WWW www = new WWW(UrlInsertScoreFlx1, form);
    }

    public void InsertScoreFlx2(string user, int score, string akurasi)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user);
        form.AddField("score", score);
        form.AddField("akurasi", akurasi);
        WWW www = new WWW(UrlInsertScoreFlx2, form);
    }

    public void InsertScoreAna(string user, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user);
        form.AddField("score", score);
        WWW www = new WWW(UrlInsertScoreAna, form);
    }

    public void InsertScore3Col(string user, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user);
        form.AddField("score", score);
        WWW www = new WWW(UrlInsertScore3Col, form);
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
