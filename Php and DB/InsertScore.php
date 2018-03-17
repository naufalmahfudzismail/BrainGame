<?php

  $host = "localhost";
  $dbName = "katadasar";
  $user = "root";
  $password = "";

  $conn = new mysqli($host, $user, $password, $dbName);


  $gam = $_POST["GameNamePost"];
  $skor = $_POST["ScorePost"];

  if(!$conn)
  {
      die("Connection Failed. ".mysqli_connect_error());
  }

  $sql = "INSERT INTO score ( Game_Name, Score) values ('".$gam."', '".$skor."')";

  $result  = mysqli_query($conn, $sql);

  if(!$result)
  {
     echo "Terdapat Error pada server";
  }

  else echo "It's Okay"

?>
