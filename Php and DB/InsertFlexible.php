<?php

  $host = "localhost";
  $dbName = "katadasar";
  $user = "root";
  $password = "";

  $conn = new mysqli($host, $user, $password, $dbName);


  $katdas = $_POST["katadasarPost"];
  $katdas = mysqli_real_escape_string($katdas);
  $kalimat = $_POST["kalimatPost"];
  $kalimat = mysqli_real_escape_string($kalimat);

  if(!$conn)
  {
      die("Connection Failed. ".mysqli_connect_error());
  }

  $sql = "INSERT INTO tb_flexible ( Katadasar, Kalimat) values ('$katdas', '$kalimat')";

  $result  = mysqli_query($conn, $sql);

  if(!$result)
  {
     echo "Terdapat Error pada server";
  }

  else echo "It's Okay"

?>
